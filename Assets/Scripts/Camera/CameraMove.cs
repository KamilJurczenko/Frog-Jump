using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //[SerializeField] Camera mainCameraObj;
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject leftLockObj;
    [SerializeField] GameObject rightLockObj;
    [SerializeField] GameObject upLockObj;

    Transform playerTransformCache;

    private Transform transformCache;

    [SerializeField] bool startAtPlayer;
    //[SerializeField] bool movePermanently; 
    [SerializeField] bool moveRightOnly;
    [SerializeField] bool moveLeftOnly;

    float rightLockPosCam;
    float rightLockPos;
    float leftLockPosCam;
    float leftLockPos;
    float upLockPosCam;
    float downLockPosCam;

    bool staticX;
    bool staticY;

    float clampX;
    float clampY;

    float camHalfHeight;
    float camHalfWidth;


    // Start is called before the first frame update
    private void Awake()
    {
        transformCache = GetComponent<Transform>();
        playerTransformCache = playerObj.transform;
    }

    void Start()
    {

        clampX = transformCache.position.x;
        clampY = transformCache.position.y;

        float screenAspect = (float)Screen.width / (float)Screen.height;
        camHalfHeight = GetComponent<Camera>().orthographicSize;
        camHalfWidth = Mathf.Round(screenAspect * camHalfHeight * 100f / 100f);

        /*float camRightBound = transform.position.x + camHalfWidth;
        float camLeftBound = transform.position.x - camHalfWidth;
        float camUpBound = transform.position.y + camHalfHeight;
        float camDownBound = transform.position.y - camHalfHeight; */

        float upLockPos = upLockObj.transform.position.y;

        upLockPosCam = upLockObj.transform.position.y - camHalfHeight;
        rightLockPosCam = rightLockObj.transform.position.x - camHalfWidth;
        leftLockPosCam = leftLockObj.transform.position.x + camHalfWidth;
        downLockPosCam = transformCache.position.y;

        rightLockPos = rightLockObj.transform.position.x;
        leftLockPos = leftLockObj.transform.position.x;
        /* float distUp = Mathf.Abs(upLockPos - transform.position.y);
         float distLeft = Mathf.Abs(leftLockPosCam - transform.position.x);
         float distRight = Mathf.Abs(rightLockPosCam - transform.position.x); */

        if (upLockPos == 0) staticY = true; else staticY = false;
        if (rightLockPos == 0 && leftLockPos == 0) staticX = true; else staticX = false;

        if (startAtPlayer) transformCache.position = new Vector3(playerTransformCache.position.x, transformCache.position.y, -5f);
        else if (leftLockPos != 0 || rightLockPos != 0)
        {
            /*if ((playerTransformCache.position.x + leftLockPos) < camHalfWidth)
            {
                transformCache.position = new Vector3(leftLockPosCam, transformCache.position.y, -5f);
            }
            else if ((playerTransformCache.position.x + rightLockPos) < camHalfWidth)
            {
                transformCache.position = new Vector3(rightLockPosCam, transformCache.position.y, -5f);
            } */
            if(Mathf.Abs(transformCache.position.x - rightLockPos) > Mathf.Abs(transformCache.position.x - leftLockPos))
            {
                transformCache.position = new Vector3(leftLockPosCam, transformCache.position.y, -5f);
            }
            else
            {
                transformCache.position = new Vector3(rightLockPosCam, transformCache.position.y, -5f);
            }
        }
        /*
        if (rightLockObj.transform.position.x > camRightBound || leftLockObj.transform.position.x < camLeftBound)
        {

            if (!startAtPlayer)
            {
                if (distRight > distLeft)
                {
                    transform.position = new Vector3(leftLockPosCam, transform.position.y, transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(rightLockPosCam, transform.position.y, transform.position.z);
                }
            }
            else
            {
                transform.position = new Vector3(playerTransformCache.position.x, playerTransformCache.position.y, transform.position.z);
            }
        }
        else staticX = true;

        if (upLockObj.transform.position.y > camUpBound || downLockObj.transform.position.y < camDownBound)
        {
            if (distUp > distDown)
            {
                transform.position = new Vector3(transform.position.x, downLockPos, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, upLockPos, transform.position.z);
            }
        }
        else staticY = true; */
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!staticX) {  clampX = Mathf.Clamp(playerTransformCache.position.x, leftLockPosCam, rightLockPosCam); }
        if (!staticY) {  clampY = Mathf.Clamp(playerTransformCache.position.y, downLockPosCam, upLockPosCam); }
        //transform.position = new Vector3(playerObj.transform.position.x, transform.position.y, transform.position.z);
        if (!PlayerController.isDead && !GameUI.gameOverBool &&  (!staticX || !staticY))
        {
            transformCache.position = Vector3.Lerp(transformCache.position, new Vector3(clampX, clampY, transformCache.position.z), 0.125f);
        }
        if(playerTransformCache.position.x > (transform.position.x + camHalfWidth) || playerTransformCache.position.x < (transform.position.x - camHalfWidth))
        {
            PlayerController.isDead = true;
            Debug.Log("Player out of Camera");
        }
       // if (playerTransformCache.position.x > transform.position.x - camHalfWidth || transform.position.x + camHalfWidth < transform.position.x) GameUI.gameOverBool = true;
    }
}
