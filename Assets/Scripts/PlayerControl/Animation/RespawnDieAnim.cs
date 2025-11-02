using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnDieAnim : MonoBehaviour
{
    Color[,] playerTextureData;

    Object pixelObj;

    Sprite playerSprite;

    GameObject parentGO;

    [SerializeField] float force;
    float pixelSize;

    Rigidbody2D rb;

    Vector3 forcePoint;
    Vector3 startPointToRender;

    private bool died;
    private bool spawned;
    private void Awake()
    {
        parentGO = new GameObject("RespawnDiePlayerAnim");
        parentGO.transform.parent = transform.parent.transform.parent;
        parentGO.transform.localPosition = Vector3.zero;
        pixelObj = Resources.Load("Pixel");
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        died = false;
        spawned = false;

        playerSprite = GetComponent<SpriteRenderer>().sprite;
        forcePoint = new Vector3(0, playerSprite.bounds.extents.y) * 0.8f;

        GameObject tmpPixelGO = (GameObject)pixelObj;
        pixelSize = tmpPixelGO.GetComponent<SpriteRenderer>().bounds.size.x;
        startPointToRender = transform.localPosition + playerSprite.bounds.min * 0.8f;

        ReadCurrentSprite();

        RenderPixels(); // on spawn

        //SetForceToPixel();
    }

    // Update is called once per frame
    void Update()
    {
        parentGO.transform.position = transform.position;

        if (PlayerController.isDead && !died)
        {
            MoveRenderedPixels(out died, true);
        }
        else if ((died && !LevelManager.hasNextLevelLocked) || (died && LevelManager.hasNextLevelLocked && PlayerController.totalDeaths % 5 != 0) || (died && SceneManager.sceneCountInBuildSettings == SceneManager.GetActiveScene().buildIndex + 1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (parentGO.transform.childCount > 0 && !spawned)
        {
            MoveRenderedPixels(out spawned, false);
        }
    }

    private void MoveRenderedPixels(out bool atPosition, bool random)
    {
        parentGO.SetActive(true);
        if (!PlayerController.faceRight)
        {
            parentGO.transform.localScale = new Vector3(-1,1);
        }
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<SpriteRenderer>().enabled = false;
        atPosition = false;

        for (int i = 0; i < parentGO.transform.childCount; i++)
        {
            Transform childObj = parentGO.transform.GetChild(i);
            //string[] childObjName = childObj.name.Split(' ');
            Vector3 tmpPos;
            if (random)
            {
                tmpPos = childObj.GetComponent<PixelData>().randomPosition;
            }
            else
            {
                tmpPos = childObj.GetComponent<PixelData>().position;
            }
            childObj.transform.localPosition = Vector3.Lerp(childObj.transform.localPosition, tmpPos + forcePoint, 0.125f);
            if (Vector3.Distance(tmpPos + forcePoint, childObj.transform.localPosition) < 0.01f)
            {
                atPosition = true;
                if (!random) 
                { 
                    GetComponent<SpriteRenderer>().enabled = true;
                    parentGO.SetActive(false);
                    rb.bodyType = RigidbodyType2D.Dynamic;
                }
            }
        }
    }

    private void ReadCurrentSprite()
    {
        string playerSkin = SkinChanger.GetSkinName();
        playerSkin = char.ToUpper(playerSkin[0]) + playerSkin.Substring(1);

        string playerSpriteName = playerSprite.name;
        //playerSpriteName = playerSpriteName.Remove(playerSpriteName.Length - 2);
        //Debug.Log(playerSpriteName);
        Sprite[] playerSprites = Resources.LoadAll<Sprite>("Graphics/Player/Skins/" + playerSkin + "/" + playerSpriteName);
        //Debug.Log("Current PlayerSprite state: " + playerSprites[0].name);

        ReadTextureToArray(ConvertToTexture(playerSprites[0]));
    }
    private Texture2D ConvertToTexture(Sprite sprite)
    {
        var croppedTexture = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
        var pixels = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                (int)sprite.textureRect.y,
                                                (int)sprite.textureRect.width,
                                                (int)sprite.textureRect.height);
        croppedTexture.SetPixels(pixels);
        croppedTexture.Apply();
        return croppedTexture;
    }
    private void ReadTextureToArray(Texture2D texture)
    {
        int pixels = 0;
        playerTextureData = new Color[texture.width, texture.height];
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                playerTextureData[i, j] = texture.GetPixel(i, j);
                pixels++;
            }
        }
        //Debug.Log("Total Pixels: " + pixels);
    }
    private void RenderPixels()
    {
        /*  rb.bodyType = RigidbodyType2D.Static;
          GetComponent<SpriteRenderer>().enabled = false; */

            //Debug.Log(startPointToRender);

            for (int i = 0; i < playerTextureData.GetLength(0); i++)
            {
                for (int j = 0; j < playerTextureData.GetLength(1); j++)
                {
                    if (playerTextureData[i, j].a != 0f)
                    {
                        GameObject pixelGO = (GameObject)Instantiate(pixelObj, parentGO.transform);
                        pixelGO.name = "Pixel " + i + " " + j;
                        pixelGO.GetComponent<SpriteRenderer>().color = playerTextureData[i, j];
                        float randomRange = Random.Range(1f, 1.5f);
                        Vector3 tmpPos = startPointToRender + (new Vector3(i, j) * pixelSize) - forcePoint;
                        pixelGO.GetComponent<PixelData>().randomPosition = tmpPos.normalized * randomRange;
                        pixelGO.transform.localPosition = tmpPos.normalized * randomRange;
                        pixelGO.GetComponent<PixelData>().position = tmpPos;
                    }
                }
            }
    }

            /*  private void SetForceToPixel()
              {
                  QualitySettings.vSyncCount = 0;
                  Application.targetFrameRate = 30; 
                  Vector3 forcePoint = new Vector3(0,playerSprite.bounds.extents.y) * 0.8f;
                  Debug.Log("Force Point: " + forcePoint);
                  for (int i = 0; i < parentGO.transform.childCount; i++)
                  {
                      Transform childObj = parentGO.transform.GetChild(i);
                      Vector3 forceDir = (forcePoint + childObj.transform.localPosition).normalized;
                      //Debug.Log("Force Direction of Pixel: " + childObj.name + " is: " + forceDir);
                      //Debug.Log(childObj.transform.position);
                      childObj.GetComponent<Rigidbody2D>().velocity = new Vector3(force * Mathf.Sign(childObj.transform.localPosition.x),
                          force * Mathf.Sign(childObj.transform.localPosition.y - forcePoint.y)); 
                      childObj.GetComponent<Rigidbody2D>().AddForce(forceDir * force, ForceMode2D.Impulse);

                  } 

              } */
}

