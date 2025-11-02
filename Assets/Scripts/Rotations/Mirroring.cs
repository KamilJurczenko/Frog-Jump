using UnityEngine;

[ExecuteInEditMode]
public class Mirroring : MonoBehaviour
{
    [SerializeField] public bool flipHorizontally;
    [SerializeField] public bool upSideDown;
    [SerializeField] public bool flipVertically;

    Quaternion cachedQuaternion;

    Quaternion horizontalFlipQuat = Quaternion.Euler(0, 180, 0);
    Quaternion zeroQuat = Quaternion.Euler(0, 0, 0);
    Quaternion verticalFlipQuat = Quaternion.Euler(180, 0, 90);
    Quaternion upSideDownRotation = Quaternion.Euler(0, 0, 90);

    // Start is called before the first frame update
    private void Update()
    {
        Flip();
    }
    private void Flip()
    {
        if (flipHorizontally && cachedQuaternion != horizontalFlipQuat)
        {
            upSideDown = false; flipVertically = false;
            transform.rotation = horizontalFlipQuat;
            cachedQuaternion = horizontalFlipQuat;
        }
        else if (!flipHorizontally && cachedQuaternion != zeroQuat) { transform.rotation = zeroQuat; cachedQuaternion = zeroQuat; }

        if (upSideDown && cachedQuaternion != upSideDownRotation)
        {
            transform.rotation = upSideDownRotation;
            cachedQuaternion = upSideDownRotation;
        }
        if (flipVertically && upSideDown && cachedQuaternion != verticalFlipQuat)
        {
            transform.rotation = verticalFlipQuat;
            cachedQuaternion = verticalFlipQuat;
        }
    }
}
