using UnityEngine;

public class AcidBall : MonoBehaviour
{
    Animator animator;
    BoxCollider2D bc;
    private void Start()
    {
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            bc.isTrigger = false;
            animator.SetTrigger("destroyDot");
            Invoke("DestroyObject", 0.35f);
        }
    }
    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
