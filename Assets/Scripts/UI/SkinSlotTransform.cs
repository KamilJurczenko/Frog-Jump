using UnityEngine;

public class SkinSlotTransform : MonoBehaviour
{
    [SerializeField] public Transform skinSlotTransform;
    public static SkinSlotTransform instance;

    private void Awake()
    {
        instance = this;
    }
}
