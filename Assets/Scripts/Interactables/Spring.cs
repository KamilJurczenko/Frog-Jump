using UnityEngine;

public class Spring : MonoBehaviour
{
    public Animator animator;
    public bool activated;

    public enum SpringTilt
    {
        noTilt,
        leftTilt,
        rightTilt
    }

    public SpringTilt springTilt;

    public float oppositeForce = 0.95f, inForce = -1f, springForce = 1.05f;

    public BoxCollider2D bc;
    float bcCenter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bc = GetComponentInChildren<BoxCollider2D>();
    }
    private void Start()
    {
        activated = false;
        bcCenter = bc.bounds.center.x;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponentInParent<PlayerController>();

            if (Mathf.Sign(playerScript.playerRbVelocity.y) == -1 && Mathf.Approximately(collision.GetContact(0).normal.y, -1f) && !activated)
            {
                Debug.Log("Collision enter spring");
                for (int i = 0; i < animator.parameterCount; i++)
                {
                    animator.ResetTrigger(animator.GetParameter(i).name);
                    //Debug.Log(spring.animator.GetParameter(i).name);
                }
                Rigidbody2D playerRb = collision.gameObject.GetComponentInParent<Rigidbody2D>();
                playerRb.velocity = Vector2.zero;
                collision.gameObject.transform.parent.parent = bc.gameObject.transform;
                float collCenter = collision.collider.bounds.center.x;

                activated = true;
                Debug.Log("Spring activated");
                animator.SetTrigger("springDown");


                if (Mathf.Abs(bcCenter - collCenter) < 0.25f)
                {
                    springTilt = SpringTilt.noTilt;
                }
                else if (Mathf.Sign(bcCenter - collCenter) == 1) //RightTilt
                {
                    springTilt = SpringTilt.leftTilt;
                }
                else if (Mathf.Sign(bcCenter - collCenter) == -1) //LeftTilt
                {
                    springTilt = SpringTilt.rightTilt;
                }
                Debug.Log("State: " + springTilt);
            }
            else if(Mathf.Approximately(collision.GetContact(0).normal.x, -1f) || Mathf.Approximately(collision.GetContact(0).normal.x, 1f))
            {
                playerScript.ManageBounces(collision.GetContact(0), playerScript.playerRbVelocity);
                animator.SetTrigger("springWiggle");
            }

        }
    }
}
