using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovingPlatform : MonoBehaviour
{
    [HideInInspector] public float distance;
    [HideInInspector] public float xSpeed;
    [HideInInspector] public float ySpeed;

    [SerializeField] bool startPos;
    [SerializeField] bool delay; // time for rest
    [SerializeField] bool oppositeDir;
    [SerializeField] bool stopAfterPath;

    Vector2 origin;

    static float[] speedValues = { 0f, 2f, 3f, 5f };

    bool canMove;
    bool a; //var for platformDir
    bool b;

    float distanceFromOrigin;
    float stopTime;

    public int maxDistance;
    [HideInInspector]public int xIndex, yIndex;
    // Start is called before the first frame update
    void Start()
    {

        xSpeed = speedValues[xIndex];
        ySpeed = speedValues[yIndex];

        a = false;
        if (oppositeDir)
        {
            xSpeed *= -1;
            ySpeed *= -1;
        }
        if (delay)
        {
            stopTime = 1f;
        }
        origin = transform.position;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        distanceFromOrigin = (origin - (Vector2)transform.position).magnitude;
        Move();
    } 

    private void Move()
    {
        //Debug.Log(distanceFromOrigin);
        if (startPos)
        {
           /* if ((distanceFromOrigin > distance && a) || (distanceFromOrigin <= 0.1f && !a))
            {
                if (a) a = false;
                else a = true;

                ChangeDir();
            } */
            if (distanceFromOrigin > distance)
            {
                a = true;
                ChangeDir();
            }
            else if(distanceFromOrigin <= 0.1f && a)
            {
                a = false;
                ChangeDir();
            }
        }
        else
        {
            if (distanceFromOrigin > distance)
            {
                ChangeDir();
            }
        }
        if (canMove && !stopAfterPath || canMove && !b) transform.Translate(xSpeed * Time.deltaTime, ySpeed * Time.deltaTime, 0);
        //Debug.DrawLine(transform.position, new Vector3(distance, transform.position.y, transform.position.z),Color.red);
    }

    private void ChangeDir()
    {
        if (!stopAfterPath)
        {
            if (distanceFromOrigin > distance) transform.Translate(-xSpeed * Time.deltaTime, -ySpeed * Time.deltaTime, 0);
            xSpeed *= -1; ySpeed *= -1;
            if (stopTime > 0f) StartCoroutine(Rest());
        }
        else b = true;
    }

    IEnumerator Rest()
    {
        canMove = false;
        yield return new WaitForSeconds(stopTime);
        canMove = true;
    }
   
    private void OnDrawGizmos()
    {
        float xSpeedGizmo = speedValues[xIndex];
        float ySpeedGizmo = speedValues[yIndex];
        int tmp = 1;
        if (oppositeDir) tmp = -1;
        Tilemap tileMap = transform.GetComponent<Tilemap>();
        List<Vector3>  availablePlaces = new List<Vector3>();
        for (int n = tileMap.cellBounds.xMax; n > tileMap.cellBounds.xMin; n--)
        {
            for (int p = tileMap.cellBounds.yMax; p > tileMap.cellBounds.yMin; p--)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tileMap.transform.position.y));
                Vector3 place = tileMap.CellToWorld(localPlace) + new Vector3(tileMap.cellSize.x/2, tileMap.cellSize.y / 2);
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);
                }
            }
        }
        float a = 0; float b = 0;
        if (xSpeedGizmo != 0) a = 1; if (ySpeedGizmo != 0) b = 1;
        Vector2 direction = (new Vector2((distance * Mathf.Sign(xSpeedGizmo) - 0.5f) * a, (distance * Mathf.Sign(ySpeedGizmo) - 0.5f) * b) * tmp) + new Vector2(a * 0.5f, b * 0.5f) * tmp;
        Gizmos.color = Color.red;
        if (availablePlaces.Count % 2 != 0)
        {
            Gizmos.DrawRay(availablePlaces[Mathf.CeilToInt(availablePlaces.Count / 2)], direction );
        }
        else
        {
            Gizmos.DrawRay((availablePlaces[availablePlaces.Count - 1] + availablePlaces[0]) / 2, direction);
        }

    }
}
