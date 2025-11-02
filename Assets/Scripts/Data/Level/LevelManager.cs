using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject FlyTileMap;

    Transform[] flySmallObjects;

    public static int levelId;

    public static CompletedLevels loadedLevelList;

    public static bool hasNextLevelLocked;

    public static bool hasNextLevel;

    private void Awake()
    {
        levelId = SceneManager.GetActiveScene().buildIndex - 1;

        loadedLevelList = (CompletedLevels)SaveManager.LoadData("LevelData");

        if (loadedLevelList == null)
        {
            loadedLevelList = new CompletedLevels(new List<Level>());
        }
        //Debug.Log(SceneManager.sceneCountInBuildSettings);
        hasNextLevel = (levelId) < SceneManager.sceneCountInBuildSettings - 2;
        hasNextLevelLocked = loadedLevelList.completedLevelList.ElementAtOrDefault(levelId) == null;
    }

    private void Start()
    {
        SetUpFlies();
    }

    private void SetUpFlies()
    {
        flySmallObjects = FlyTileMap.transform.GetComponentsInChildren<Transform>();

            List<Level> loadedFlyIdList = loadedLevelList.completedLevelList;
            if (loadedFlyIdList.ElementAtOrDefault(levelId) != null)
            {
             /*   foreach (Transform obj in flySmallObjects)
                {
                    for (int i = 0; i < loadedFlyIdList[levelId].flySmallIdList.Count; i++)
                    {
                        if (obj.GetInstanceID() == loadedFlyIdList[levelId].flySmallIdList[i])
                        {
                            obj.gameObject.SetActive(false);
                        }
                    }
                } */
                for (int i = 0; i < flySmallObjects.Count(); i++)
                {
                    if (loadedFlyIdList[levelId].flySmallIdList.Contains(i))
                    {
                        flySmallObjects[i].gameObject.SetActive(false);
                    }
                }
        }
    }

    private List<int> CollectedFlySmallObjectsIDs()
    {
        List<int> remainingObjects = new List<int>();
        if (flySmallObjects[0] != null)
        {
            for(int i = 0; i < flySmallObjects.Count(); i++)
            {
                if (!flySmallObjects[i].gameObject.activeInHierarchy)
                {
                    remainingObjects.Add(i); // Add index as reference
                }
            }
        }
        return remainingObjects;       
    }

    public void LevelCompleted(float levelTimer, out float bestTime)
    {
        PlayerController.totalDeaths = 0;

        bestTime = 0;

        List<Level> completedLevelListData = loadedLevelList.completedLevelList;
        // check if completed Level Exists
        if (completedLevelListData.ElementAtOrDefault(levelId) != null)
        {
            bestTime = completedLevelListData[levelId].bestTime; //GetCurrentHighscore
            if (bestTime > levelTimer || bestTime == 0)
            {
                bestTime = levelTimer;
            }
            completedLevelListData[levelId] = new Level(bestTime,StarsAchieved(),CollectedFlySmallObjectsIDs());
            //RemainingFlySmallobject collected??!?
        }
        else
        {
            bestTime = levelTimer;
            completedLevelListData.Add(new Level(bestTime, StarsAchieved(), CollectedFlySmallObjectsIDs()));
            Debug.Log(completedLevelListData.Count());
        }
        SaveManager.SaveData(loadedLevelList, "LevelData");

        //Level currentLevel = loadedLevelList.completedLevelList[levelId];

    }

   /* private int ConvertIntFromString(string subjectString)
    {
        string resultString = string.Join(string.Empty, Regex.Matches(subjectString, @"\d+").OfType<Match>().Select(m => m.Value));
        return int.Parse(resultString);
    } */
    public ushort StarsAchieved() //depending on collected small flies 3 = all flies, 2 = half
    {
        int collectedFlies = CollectedFlySmallObjectsIDs().Count;
        //Debug.Log("Collected Flies: " + collectedFlies);
        if (collectedFlies == flySmallObjects.Length - 1) return 3;
        else if (collectedFlies >= (flySmallObjects.Length - 1) * 0.5) return 2;
        else return 1;
    }

 /*   private void CreateLevelScriptableObject(ushort levelNumber)
    {
        if (!File.Exists("Assets/Level/Level " + levelNumber + ".asset"))
        {
            LevelScriptableObject asset = ScriptableObject.CreateInstance<LevelScriptableObject>();
            AssetDatabase.CreateAsset(asset, "Assets/Level/Level" + levelNumber + ".asset");
        }
    } */


}
