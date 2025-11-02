using UnityEngine;


[ExecuteAlways]
public class ObjectRotation : MonoBehaviour
{
    [HideInInspector] public int rotation;

    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (rotation)
        {
            case 0:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1: //left
                transform.rotation = Quaternion.Euler(0, 0, -90);
                sprite.flipX = true;
                break;
            case 2: //down
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3: //right
                transform.rotation = Quaternion.Euler(180, 0, 0);
                break;
        }
    }
}
