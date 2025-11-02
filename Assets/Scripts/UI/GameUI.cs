using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text actualTimeText;
    [SerializeField] TMP_Text bestTimerText;
    [SerializeField] GameObject startTextObj;
    [SerializeField] GameObject levelCompleteObj;
    [SerializeField] GameObject starsObj;
    [SerializeField] GameObject fadeObj;
    [SerializeField] GameObject pauseMenue;
    [SerializeField] GameObject player;
    [SerializeField] GameObject skipLevel;
    [SerializeField] GameObject tutorialObj;
    [SerializeField] GameObject noNextLevelObj;

    private bool startTextActive;
    public static bool gameOverBool;
    public static float timerForLevel;
    public static bool gamePaused;
    private bool pauseMenueClosed;

    public static float adTimer;

    public static bool canShowAd;

    private void Start()
    {
        //Debug.Log(LevelManager.levelId);
        startTextActive = true;
        gameOverBool = false;
        timerForLevel = 0f;
        gamePaused = false;
        Time.timeScale = 1;
        //Debug.Log(adTimer);
        if (adTimer >= 45f && canShowAd)
        {
            AdsEnabled.instance.GetComponent<InterstitialAdsScript>().ShowInterstitialAd();
            adTimer = 0;
            canShowAd = false;
        }
    }
    private void Update()
    {
        if (AdsEnabled.instance.GetComponent<InterstitialAdsScript>().isShowing)
        {
            gamePaused = true;
        }
        else
        {
            if (pauseMenueClosed)
            {
                if (Input.GetMouseButtonDown(0)) { Time.timeScale = 1; gamePaused = false; }
            }
            if (!startTextActive && !gameOverBool) SetTimer();
            if (startTextActive && Input.GetMouseButton(0))
            {
                startTextObj.SetActive(false);
                startTextActive = false;
                if (tutorialObj != null) tutorialObj.SetActive(false);
            }
            if (Input.GetMouseButtonDown(0) && gamePaused)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                    Camera.main.ScreenToWorldPoint(Input.mousePosition).y), Vector2.zero, 0);
                if (hit == false)
                {
                    ResumeGame();
                    pauseMenue.SetActive(false);
                }
            }
        }
    }
    

    void SetTimer()
    {
        timerForLevel += Time.deltaTime;
        timerText.text = timerForLevel.ToString("F1");
    }

    public void SkipLevel()
    {
        skipLevel.SetActive(true);
        adTimer = 0;
    }

    public void LevelCompleted()
    {
        if ((LevelManager.levelId + 3) <= SceneManager.sceneCountInBuildSettings)
        {
            adTimer += timerForLevel;
            levelCompleteObj.SetActive(true);

            if(LevelManager.levelId == 4 || LevelManager.levelId == 9 || LevelManager.levelId == 14 || LevelManager.levelId == 19)
            {
                int reviewDisplayed = PlayerPrefs.GetInt("ReviewDisplayed", 0);
                if(reviewDisplayed == 0)
                {
                    PlayerPrefs.SetInt("ReviewDisplayed", 1);
                    StartCoroutine(InAppReview.instance.RequestReviews());
                }
            }

            GameObject.Find("LevelManager").GetComponent<LevelManager>().LevelCompleted(timerForLevel, out float bestTime);
            timerText.gameObject.SetActive(false);
            actualTimeText.text = timerText.text;
            bestTimerText.text = bestTime.ToString("F1");
            SetupAchievedStars();
         
            canShowAd = true;
        }
        else noNextLevelObj.SetActive(true);

        gameOverBool = true;

    }

    public void PauseGame()
    {
        pauseMenue.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0;
        pauseMenueClosed = false;
    }

    public void ResumeGame()
    {
        pauseMenueClosed = true;
        gamePaused = false;
    }

    private void SetupAchievedStars()
    {
        ushort stars = GameObject.Find("LevelManager").GetComponent<LevelManager>().StarsAchieved();
        //Debug.Log("Stars Achieved: " + stars);
        Transform[] childObj = starsObj.transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i <= stars; i++)
        {
            childObj[i].gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "PauseButton" && !gameOverBool && !PlayerController.isDead)
        {
            PauseGame();
        }
    }
}
