using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SpikeMoving : MonoBehaviour
{

    GridLayout platformGridLayout;
    Tilemap[] platformTileMap;
    Tilemap onPlatform;

    float rotationSpeed = 2;

    [HideInInspector] public int indexSpeed;

    float moveSpeed;

    [SerializeField] private LayerMask platformLayerMask;

    Vector3Int startPos;
    Vector3Int currentCellPosition;

    Vector3 raycastPlatformDir;
    Vector3 raycastMoveDir;

    float platformRaycastDist;

    //List<Vector3Int> movePoints; // stores spikeMovement

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(indexSpeed);
        switch (indexSpeed)
        {
            case 0:
                moveSpeed = 1f;
                break;
            case 1:
                moveSpeed = 2f;
                break;
            case 2:
                moveSpeed = 5f;
                break;
        }

        //movePoints = new List<Vector3Int>();
        platformGridLayout = transform.parent.GetComponentInParent<GridLayout>();

        startPos = platformGridLayout.LocalToCell(transform.position);

        PlatformToMoveOn();

        GetStartRaycastDir();

        transform.position = raycastPlatformDir * platformGridLayout.cellSize.y / 2 + transform.position;
        platformRaycastDist = platformGridLayout.cellSize.x;
        /* 
         * Debug.Log("Cell Startposition: " + startPos);
         * Debug.Log("CellCenterToWorld of Startposition: " + platformGrid.GetCellCenterLocal(startPos));
         Debug.Log(platformGrid.GetCellCenterLocal(currentCellPosition + Vector3Int.FloorToInt(raycastPlatformDir) + Vector3Int.FloorToInt(raycastMoveDir))); */

    }

    private void PlatformToMoveOn()
    {
        platformTileMap = new Tilemap[2];
        platformTileMap[0] = GameObject.Find("Platform").GetComponent<Tilemap>();
        //platformTileMap[1] = GameObject.Find("MovingPlatform").GetComponent<Tilemap>();
    }

    private void GetStartRaycastDir()
    {
        bool a = false;
        for (int i = 0; i < platformTileMap.Length - 1; i++)
        {
            if (platformTileMap[i].HasTile(startPos + new Vector3Int(0, -1, 0))) // BOTTOM
            {
                raycastPlatformDir = Vector3.down;
            }
            else if (platformTileMap[i].HasTile(startPos + new Vector3Int(0, 1, 0))) // TOP
            {
                raycastPlatformDir = Vector3.up;
            }
            else if (platformTileMap[i].HasTile(startPos + new Vector3Int(1, 0, 0))) // RIGHT
            {
                raycastPlatformDir = Vector3.right;
            }
            else if (platformTileMap[i].HasTile(startPos + new Vector3Int(-1, 0, 0))) // LEFT
            {
                raycastPlatformDir = Vector3.left;
            }
            if(raycastPlatformDir != null && a == false)
            {
                onPlatform = platformTileMap[i];
                a = true;
            }
        }
        raycastMoveDir = Quaternion.Euler(0, 0, 90) * raycastPlatformDir * Mathf.Sign(moveSpeed);
        //raycastMoveDir = new Vector3(raycastPlatformDir.y, raycastPlatformDir.x) * Mathf.Sign(moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        MoveSpike();
        //transform.Translate(0, 0, 0, Space.World);
    }

    private void MoveSpike() 
    {

        currentCellPosition = platformGridLayout.LocalToCell(transform.position); // + raycastPlatformDir * platformGridLayout.cellSize.y / 2;
        // Debug.Log(cellPosition);
        //Debug.Log(raycastMoveDir);
        transform.Rotate(0, 0, 300 * rotationSpeed * Time.deltaTime);

        //RaycastHit2D raycastMove = Physics2D.Raycast(onPlatform.GetCellCenterLocal(currentCellPosition),raycastMoveDir,0.01f,platformLayerMask);
        // RaycastHit2D raycastPlatform = Physics2D.Raycast(onPlatform.GetCellCenterLocal(currentCellPosition), raycastPlatformDir, platformRaycastDist, platformLayerMask);

        RaycastHit2D raycastMove = Physics2D.Raycast(onPlatform.GetCellCenterLocal(currentCellPosition), raycastMoveDir,0.01f,platformLayerMask);
        RaycastHit2D raycastPlatform = Physics2D.Raycast(onPlatform.GetCellCenterLocal(currentCellPosition), raycastPlatformDir, platformRaycastDist, platformLayerMask);

        Debug.DrawRay(transform.position, raycastMoveDir * 0.01f, Color.red);
        Debug.DrawRay(transform.position, raycastPlatformDir * platformRaycastDist, Color.green);



        if (currentCellPosition != startPos) { // Check on Platform corners
            //Debug.Log("Checking");
            if (raycastMove.collider != null)
            {
                // reassign raycastsdirections            
                RotateRaycastDirections(1);
                Debug.Log("Spike Collision");
            }
            else if (raycastPlatform.collider == null)
            {
                RotateRaycastDirections(-1);
               // Debug.Log("Spike on Platform");
            }

            startPos = currentCellPosition;
        }
            /*transform.position = Vector3.MoveTowards(transform.position,
            platformGrid.GetCellCenterLocal(currentCellPosition + Vector3Int.FloorToInt(raycastPlatformDir) + Vector3Int.FloorToInt(raycastMoveDir)), moveSpeed * Time.deltaTime); */
                      
        transform.Translate(raycastMoveDir.x * Mathf.Abs(moveSpeed) * Time.deltaTime, raycastMoveDir.y * Mathf.Abs(moveSpeed) * Time.deltaTime, 0, Space.World);
    }
    private void RotateRaycastDirections(int angle)
    {
        raycastPlatformDir = Quaternion.Euler(0, 0, 90) * raycastPlatformDir * angle * Mathf.Sign(moveSpeed);
        raycastMoveDir = Quaternion.Euler(0, 0, 90) * raycastMoveDir * angle * Mathf.Sign(moveSpeed);



        // raycastPlatformDir = new Vector3(raycastPlatformDir.y, raycastPlatformDir.x) * angle;
        //raycastMoveDir = new Vector3(raycastMoveDir.y, raycastMoveDir.x) * -angle;
    }
}
