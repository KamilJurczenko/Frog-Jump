using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] GameObject levelButton; //prefab LevelButton

    GameObject[] levelButtonArr;

    [SerializeField] int levelButtonsPerPage;

    [SerializeField] Transform levelParent;

    int totalLevelNumber;

    int unlockedLevels;

    int startIndex;

    private void Awake()
    {
        totalLevelNumber = SceneManager.sceneCountInBuildSettings - 1;
        levelButtonArr = new GameObject[totalLevelNumber];
        unlockedLevels = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        SetUpLevelButtons();
        //Debug.Log("Unlocked Levels : " + unlockedLevels);
        startIndex = unlockedLevels / levelButtonsPerPage * levelButtonsPerPage; //Start page at current progress
    }

    public void OnStartButtonClick()
    {
        HandlePage(startIndex, startIndex + levelButtonsPerPage);
    }

    public void NextPage()
    {
        if(levelButtonArr.ElementAtOrDefault(startIndex + 8))
        {
            HandlePage(startIndex, startIndex + 8, true);

            startIndex += 8;
            HandlePage(startIndex, startIndex + 8);
        }
    }
    public void PrevPage()
    {
        if (levelButtonArr.ElementAtOrDefault(startIndex - 8))
        {
            HandlePage(startIndex, startIndex + 8, true);

            startIndex -= 8;
            HandlePage(startIndex, startIndex + 8);
        }
    }
    private void HandlePage(int beginIndex, int endIndex, bool delete = false)
    {
        if (endIndex > totalLevelNumber) endIndex = totalLevelNumber;
        for (int i = beginIndex; i < endIndex; i++)
        {
            if(delete) levelButtonArr[i].SetActive(false);
            else levelButtonArr[i].SetActive(true);
        }
    }


    public void OnLevelButtonClick()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;
        if (pressedButton.GetComponent<LevelButton>().unlocked == true)
        {
            int levelNumber = int.Parse(pressedButton.GetComponentInChildren<TMP_Text>().text);
            SceneManager.LoadScene(levelNumber);
        }
    }

    private void SetUpLevelButtons()
    {
        CompletedLevels cmpltLevels = (CompletedLevels)SaveManager.LoadData("LevelData");

        for (int i = 0; i < totalLevelNumber; i++)
        {
            GameObject instantiatedButton = Instantiate(levelButton, levelParent);
            levelButtonArr[i] = instantiatedButton;
            instantiatedButton.SetActive(false);
        }

        levelButtonArr[0].GetComponent<LevelButton>().unlocked = true;

        for (int i = 0; i < totalLevelNumber; i++)
        {
            if (cmpltLevels != null)
            {
                /*if (((cmpltLevels.completedLevelList.ElementAtOrDefault(i) != null &&
                    cmpltLevels.completedLevelList.ElementAtOrDefault(i + 1) == null) ||
                    cmpltLevels.completedLevelList.ElementAtOrDefault(i) != null) && levelButtonArr.ElementAtOrDefault(i + 1) != null) */
                if(cmpltLevels.completedLevelList.ElementAtOrDefault(i) != null)
                {
                    levelButtonArr[i].GetComponent<LevelButton>().unlocked = true;
                    if (i < (totalLevelNumber - 1))
                    {
                        levelButtonArr[i + 1].GetComponent<LevelButton>().unlocked = true;
                    }
                    unlockedLevels++;
                }
                //else levelButtonArr[i].GetComponent<LevelButton>().unlocked = false;
                if (cmpltLevels.completedLevelList.ElementAtOrDefault(i) != null) levelButtonArr[i].GetComponent<LevelButton>().stars = cmpltLevels.completedLevelList[i].stars;
            }
            Transform[] childObj = levelButtonArr[i].transform.GetComponentsInChildren<Transform>();
            foreach (Transform t in childObj)
            {
                if (t.gameObject.name == "LevelNumber") t.gameObject.GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();

                if (t.gameObject.GetInstanceID() != levelButtonArr[i].GetInstanceID())
                {
                    if (levelButtonArr[i].GetComponent<LevelButton>().unlocked == false)
                    {
                        if (t.gameObject.name != "Locked") t.gameObject.SetActive(false);
                    }
                    else
                    {
                        if (t.gameObject.name == "Locked") t.gameObject.SetActive(false);
                    }
                }
                if (t.gameObject.name == "Stars")
                {
                    Transform child = t.GetChild(1);
                    for (int tmp = 0; tmp < levelButtonArr[i].GetComponent<LevelButton>().stars; tmp++)
                    {
                        child.GetChild(tmp).gameObject.SetActive(false);
                    }
                }
            }
        }
    }


}
