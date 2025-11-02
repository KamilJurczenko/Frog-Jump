using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]GameObject UIObject;
    [SerializeField]GameObject SpawnPoint;

    GameUI gameUI;

    Transform playerParentTransform;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider2d;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask platformLayerMask;

    Animator animator;

    public static bool faceRight;
    public bool isCharging;
    public static bool isDead;

    ContactPoint2D cachedContactPoint;

    [Header("Jump Forces")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float minJump;
    [SerializeField] private float xForce;
    [SerializeField] private float yForce;

    public static float timeHeld;
    public static float timeHeldLimit = 1.0f;

    Vector2 jumpDirection;
    public Vector2 playerRbVelocity;

    private bool hasDied;

    private int bounceCount;
    private int collectedFlies;

    public static int totalDeaths;

    private void Awake()
    {
        boxCollider2d = GetComponentInChildren<BoxCollider2D>();
        rb = transform.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        gameUI = UIObject.GetComponent<GameUI>();
    }
    private void Start()
    {
        collectedFlies = 0;
        playerParentTransform = transform.parent;
        playerParentTransform.position = SpawnPoint.transform.position;
        hasDied = false;
        faceRight = true;
        isCharging = false;
        isDead = false;
    }

    private void Update()
    {
        if (Mathf.Abs(playerRbVelocity.y) > 0.1f) animator.SetBool("hasVelocityY", true);
        else animator.SetBool("hasVelocityY", false);

        animator.SetBool("isGrounded", IsGrounded());

        if (isDead && !hasDied)
        {
            hasDied = true;

            if (LevelManager.hasNextLevelLocked && LevelManager.hasNextLevel)
            {
                totalDeaths++;
                if (totalDeaths % 5 == 0)
                {
                    gameUI.SkipLevel();
                    GameUI.gameOverBool = true;
                }
            }
        }
         if (!GameUI.gameOverBool && !GameUI.gamePaused)
        {

            if (IsGrounded())
            {
                bounceCount = 0;
            }

            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonUp(0)) timeHeld = 0;
            if (timeHeld > 0) isCharging = true;
            else if (playerRbVelocity != Vector2.zero) isCharging = false;
            animator.SetBool("isCharging", isCharging);

            if (Input.GetMouseButton(0) && IsGrounded())
            {
                if (timeHeld <= timeHeldLimit) timeHeld += Time.deltaTime;
                RotateToDirection();
                JumpVelocity();
            }
        }
    }

    private void FixedUpdate()
    {
        Jump();

        FixTouchingWall();

        playerRbVelocity = rb.velocity;

    }
    private void LateUpdate()
    {
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0); //alternative solution for spring rotation bug
    }

    private void FixTouchingWall()
    {
        if (cachedContactPoint.normal != Vector2.zero)
        {
            bool toWall = false;
            if (Mathf.Approximately(cachedContactPoint.normal.x, 1))
            {
                if (!faceRight) toWall = true;
            }
            else if (Mathf.Approximately(cachedContactPoint.normal.x, -1))
            {
                if (faceRight) toWall = true;
            }
            if (toWall && Input.GetMouseButtonUp(0))
            {
                ManageBounces(cachedContactPoint, jumpDirection / 3);
                cachedContactPoint = new ContactPoint2D();
                isCharging = false;
                timeHeld = 0;
            }
        }
    }

    private void Jump() 
    {
        if (Input.GetMouseButtonUp(0) && IsGrounded() && timeHeld > 0 && !GameUI.gamePaused)
        {
            AudioManager.instance.Play("PlayerJump");
            timeHeld = 0;
            rb.AddForce(jumpDirection, ForceMode2D.Impulse);
            isCharging = false;
            //Debug.Log("Charged" + rb.velocity);
        }
        if(playerRbVelocity != Vector2.zero) animator.SetBool("hasVelocity", true);
        else animator.SetBool("hasVelocity", false);
    }
    public void ManageBounces(ContactPoint2D collisionPoint, Vector2 velocityCache)
    {
        AudioManager.instance.Play("PlayerBounce");
        Vector2 normal = collisionPoint.normal;

        //Debug.Log("COLLISIONCHECK");          
        //Debug.Log(rb.velocity);
        if (velocityCache != Vector2.zero) rb.velocity = Vector2.Reflect(velocityCache, normal);
        else rb.velocity = Vector2.Reflect(playerRbVelocity, normal);
        bounceCount++;
        //Debug.Log("Collision");
        //Debug.Log(rb.velocity);       
    }
    private void RotateToDirection()
    {
        if (!isCharging && rb.bodyType != RigidbodyType2D.Static)
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                if (faceRight)
                {
                    //transform.Rotate(new Vector3(0, 180, 0));  
                    if (sprite.flipX) sprite.flipX = false;
                    else sprite.flipX = true;
                    faceRight = false;
                }
            }
            else if (Input.mousePosition.x > Screen.width / 2)
            {
                if (!faceRight)
                {
                    //transform.Rotate(new Vector3(0, 180, 0));  
                    if (sprite.flipX) sprite.flipX = false;
                    else sprite.flipX = true;
                    faceRight = true;
                }
            }
        }
    }

    private void JumpVelocity()
    {
        int a;
        if (!faceRight) a = -1; else a = 1;
        if (timeHeld <= timeHeldLimit)
        {
            jumpDirection = new Vector2((timeHeld * jumpForce + minJump ) * a + xForce, (timeHeld * jumpForce + minJump) + yForce);
        }
        else { jumpDirection = new Vector2((timeHeldLimit * jumpForce + minJump) * a + xForce, timeHeldLimit * jumpForce + minJump + yForce); }
    }
    public void AddVelocity(float xVel, float yVel)
    {
        rb.velocity += new Vector2(xVel, yVel);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (IsGrounded()) //FIX IsGrounded() BUGGED
            {
                if (collision.gameObject.GetComponent<MovingPlatform>() != null)
                {
                    playerParentTransform.parent = collision.transform;
                }
                rb.velocity = Vector2.zero;
                //Debug.Log("On Platform");
            }
            else 
            {
                //Debug.Log("Bouncing on: " + collision.gameObject.name);
                if(bounceCount < 2) ManageBounces(collision.GetContact(0), Vector2.zero);
            }
        }
        else if (collision.gameObject.CompareTag("Ice"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                //Debug.Log("On ice");
            }
            else
            {
                //Debug.Log("Bouncing on: " + collision.gameObject.name);
                if (bounceCount < 2) ManageBounces(collision.GetContact(0), Vector2.zero);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            PlayerCrushed();
            ContactPoint2D[] collNormal = new ContactPoint2D[collision.contactCount];
            collision.collider.GetContacts(collNormal);
            foreach (ContactPoint2D normal in collNormal)
            {
                float normalX = normal.normal.x, normalY = normal.normal.y;
                if (Mathf.Approximately(normalX, Mathf.Sign(normalX) * 1)) normalX = Mathf.Sign(normalX) * 1;
                else if (Mathf.Approximately(normalX, 0)) normalX = 0;
                if (Mathf.Approximately(normalY, Mathf.Sign(normalY) * 1)) normalY = Mathf.Sign(normalY) * 1;
                else if (Mathf.Approximately(normalY, 0)) normalY = 0;

                if (playerRbVelocity == Vector2.zero
                    && (Mathf.Approximately(normalX, 1)
                    || Mathf.Approximately(normalX, -1)))
                {
                    {
                        //Debug.Log("ISSTUCKTOWALL");
                        cachedContactPoint = collision.GetContact(0);
                    }
                }
            }

            if (collision.gameObject.GetComponent<MovingPlatform>() != null && IsGrounded()
                && Mathf.Approximately(collision.GetContact(0).normal.y, 1))
            {
                playerParentTransform.parent = collision.transform;
            }
          }
        else if (collision.gameObject.CompareTag("Spring"))
        {
            if (Input.GetMouseButtonUp(0) && Mathf.Approximately(collision.GetContact(0).normal.y,1f))
            {
                Spring spring = collision.gameObject.GetComponent<Spring>();
                spring.activated = false;
                //Debug.Log(spring.springTilt);
                playerParentTransform.parent = null;
                for (int i = 0; i < spring.animator.parameterCount; i++)
                {
                    spring.animator.ResetTrigger(spring.animator.GetParameter(i).name);
                    //Debug.Log(spring.animator.GetParameter(i).name);
                }
                spring.animator.SetTrigger("springUp");
                AddVelocity(0f, rb.velocity.y * spring.springForce); 
            }
        }
      }
    private void OnCollisionExit2D(Collision2D collision)
    {
          playerParentTransform.parent = null;
          cachedContactPoint = new ContactPoint2D();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fly"))
        {
            collision.gameObject.SetActive(false);
            animator.SetTrigger("lvlFinished");
            animator.SetBool("gameEnd", true);
            gameUI.LevelCompleted();
            rb.velocity = Vector2.zero;

            Player playerData = (Player)SaveManager.LoadData("PlayerData");
            playerData.playerMoney += collectedFlies;
            SaveManager.SaveData(playerData, "PlayerData");
        }
        if (collision.gameObject.CompareTag("Smallfly"))
        {
              collectedFlies++;
              collision.gameObject.SetActive(false);
        }
        if ((collision.gameObject.CompareTag("GameOver") 
              || collision.gameObject.CompareTag("Spike")) 
              && !GameUI.gameOverBool)
        {
            GameUI.adTimer += GameUI.timerForLevel;
            AudioManager.instance.Play("Die");
            isDead = true;
            //Debug.Log("Player hit GameOver Object");
        }
    }

    public bool IsGrounded() 
    {
        Vector2 origin = boxCollider2d.bounds.center - (new Vector3(0f, boxCollider2d.bounds.size.y / 2, 0f));
          Vector2 size = new Vector2(boxCollider2d.bounds.size.x, 0.2f);

          RaycastHit2D checkForGround = Physics2D.BoxCast(origin,size,0,Vector2.down,0f,platformLayerMask);
        return checkForGround.collider != null;
    }
    private void PlayerCrushed()
    {
        float offset = 0.1f;
        RaycastHit2D rightHit = Physics2D.Raycast(boxCollider2d.bounds.center,Vector2.right,boxCollider2d.bounds.extents.x + offset,platformLayerMask);
        RaycastHit2D leftHit = Physics2D.Raycast(boxCollider2d.bounds.center,Vector2.left,boxCollider2d.bounds.extents.x + offset,platformLayerMask);
        RaycastHit2D upHit = Physics2D.Raycast(boxCollider2d.bounds.center,Vector2.up,boxCollider2d.bounds.extents.y + offset,platformLayerMask);
        /*Debug.Log(rightHit.collider != null);
        Debug.Log(leftHit.collider != null);
        Debug.Log(upHit.collider != null); */
        if ((rightHit.collider != null && leftHit.collider != null) || (upHit.collider != null && IsGrounded()))
        {
            //Debug.Log("Player Crushed!");
            isDead = true;
        }
    }

    /*  public void DieAnimation()
      {
          //transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.AngleAxis(-90, Vector3.forward),0.01f);
          GameObject expl = (GameObject)Instantiate(playerExplosion);
          ManageParticleColor(expl);
          expl.transform.position = transform.position;
          GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
          float tmp = expl.GetComponent<ParticleSystem>().startLifetime;
          Invoke("WaitForExplosion", tmp);
      }
      private void WaitForExplosion()
      {
          gameUI.GameOver(false);
      } 
      private void ManageParticleColor(GameObject prtcSystemGameObject)
      {
          ParticleSystem prtcSystem = prtcSystemGameObject.GetComponent<ParticleSystem>();
          switch (animator.runtimeAnimatorController.name)
          {
              case "bat_Override":
                  prtcSystem.startColor = new Color32(47, 47, 47,255); // #2f2f2f
                  break;
          }
      } */

      
      /*private void OnDrawGizmos()
      {
        Gizmos.color = Color.green;
         Gizmos.DrawCube(boxCollider2d.bounds.center - (new Vector3(0f, boxCollider2d.bounds.size.y / 2, 0f)),
             new Vector2(boxCollider2d.bounds.size.x, 0.2f));
        Gizmos.DrawLine(boxCollider2d.bounds.center, boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x + 0.1f, 0));
        Gizmos.DrawLine(boxCollider2d.bounds.center, boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x + 0.1f, 0));
        Gizmos.DrawLine(boxCollider2d.bounds.center, boxCollider2d.bounds.center + new Vector3(0,boxCollider2d.bounds.extents.y + 0.1f));
    }   */
 }
