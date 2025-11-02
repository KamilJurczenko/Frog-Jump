using UnityEngine;

public class CameraPoint : MonoBehaviour
{
    float smoothspeed = 0.02f;
    [SerializeField] Vector3 targetPos;

    private GameObject mainCameraObj;
    private Camera mainCamera;

    bool moveCamera;

    private void Start()
    {
        mainCameraObj = GameObject.Find("Main Camera");
        mainCamera = mainCameraObj.GetComponent<Camera>();
        targetPos.z = mainCamera.transform.position.z;
        if (targetPos.x == 0f && targetPos.y == 0f)
        {
            targetPos = mainCamera.transform.position + mainCamera.ScreenToViewportPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveCamera)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, smoothspeed);
            if((mainCamera.transform.position - targetPos).magnitude <= 0.1)
            {
                Destroy(this.gameObject);
            } 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        moveCamera = true;
    }
}
