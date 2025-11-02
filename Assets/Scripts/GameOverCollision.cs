using UnityEngine;

public class GameOverCollision : MonoBehaviour
{
    [SerializeField] new Transform camera;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(camera.position.x, transform.position.y);
    }
}
