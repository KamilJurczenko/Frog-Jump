using UnityEngine;

public class FlyAttachTo : MonoBehaviour
{
    [SerializeField] Transform AttachToTransform;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = AttachToTransform;
    }
}
