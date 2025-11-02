using UnityEngine;
public class InfiniteBackground : MonoBehaviour
{
    [SerializeField] private new Transform camera;
    [SerializeField] float parallaxEffect;

    private float length, startpos;

    private void Start()
    {
        startpos = camera.position.x;
        if(parallaxEffect != 1) length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = camera.position.x * (1 - parallaxEffect);
        float dist = camera.position.x * parallaxEffect;

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}
