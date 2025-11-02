using UnityEngine;

public class AcidShoot : MonoBehaviour
{
    [SerializeField] float shootTimer;
    float resetTimer;

    Object acidBubble;

    Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
        acidBubble = Resources.Load("acidDot_0");
        resetTimer = shootTimer;
        ShootAcid();
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer -= Time.deltaTime;
        if(shootTimer <= 0)
        {
            shootTimer = resetTimer;
            ShootAcid();
        }
    }

    private void ShootAcid()
    {
        int tmpSignX = 0, tmpSignY = 0;

        Mirroring objectsRotation = GetComponent<Mirroring>();
        if (!objectsRotation.flipHorizontally && !objectsRotation.upSideDown && !objectsRotation.flipVertically ) { tmpSignX = -1; tmpSignY = 0; }
        else if (objectsRotation.flipHorizontally) { tmpSignX = 1; tmpSignY = 0; }
        else if (objectsRotation.upSideDown) { tmpSignX = 0; tmpSignY = -1; }
        else if (objectsRotation.upSideDown && objectsRotation.flipVertically) { tmpSignX = 0; tmpSignY = 1; }
        animator.SetTrigger("shootAcid");
        GameObject acidBubbleGO = (GameObject)Instantiate(acidBubble,transform.position, transform.rotation * Quaternion.Euler(0, 0, 0));
        acidBubbleGO.transform.parent = transform;
        acidBubbleGO.transform.localPosition = new Vector2(-0.2f, -0.3f);
        acidBubbleGO.GetComponent<Rigidbody2D>().velocity = new Vector2(2f * tmpSignX,2f * tmpSignY);
    }

}
