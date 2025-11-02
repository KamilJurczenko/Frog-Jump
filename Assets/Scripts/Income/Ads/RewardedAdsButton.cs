using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Button))]
public class RewardedAdsButton : MonoBehaviour, IUnityAdsListener
{

    private string gameId = "3816077";
    //private string gameId = "3816076"; iosKey

    Button myButton;
    public string myPlacementId = "rewardedVideo";

    bool inMenu;

    TimeData timeDate;

    static List<RewardedAdsButton> rewardedAdButtons;

    private void Awake()
    {
        if (rewardedAdButtons == null)
        {
            rewardedAdButtons = new List<RewardedAdsButton>();
        }
    }
    void Start()
    {
        //Debug.Log("TEST");
        if (SceneManager.GetActiveScene().name == "Menu")
        {
            timeDate = RewardButtonTimer.timeDate;
            inMenu = true;
        }
        myButton = GetComponent<Button>();
        rewardedAdButtons.Add(this);
        if(rewardedAdButtons.Count > 1)
        {
            Advertisement.RemoveListener(rewardedAdButtons[0]);
            rewardedAdButtons.RemoveAt(0);
        }

        if (AdsEnabled.instance.adsEnabled)
        {
            // Set interactivity to be dependent on the Placement’s status:
            myButton.interactable = Advertisement.IsReady(myPlacementId);

            // Map the ShowRewardedVideo function to the button’s click listener:
            if (myButton) myButton.onClick.AddListener(ShowRewardedVideo);
            // Initialize the Ads listener and service:
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, true);
        }
        else
        {
            if (myButton)
            {
                if (!inMenu)
                    myButton.onClick.AddListener(SkipLevel);
                else
                {
                    if(RewardButtonTimer.timeForRewardLeft <= 0)
                        myButton.onClick.AddListener(AddFlies);
                }
            }
        }
    }

    // Implement a function for showing a rewarded video ad:
    public void ShowRewardedVideo()
    {
        if ((AdsEnabled.instance.adsEnabled && !inMenu) || (inMenu && AdsEnabled.instance.adsEnabled && RewardButtonTimer.timeForRewardLeft <= 0))
            Advertisement.Show(myPlacementId);
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, activate the button: 
        if (placementId == myPlacementId)
        {
            myButton.interactable = true;
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            // Reward the user for watching the ad to completion.
            if (!inMenu) SkipLevel();
            else 
            { 
                AddFlies();
            }
        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
    private void AddFlies()
    {
        //Debug.Log("Player received 10 flies by watching an ad!");
        SkinShopManager.playerData.playerMoney += 10;
        SaveManager.SaveData(SkinShopManager.playerData, "PlayerData");
        RewardButtonTimer.timeForRewardLeft = RewardButtonTimer.rewardTimer;
        timeDate = new TimeData(DateTime.Now);

        SaveManager.SaveData(timeDate, "ExitTimeDate");
    }
    private void SkipLevel()
    {
        CompletedLevels loadedLevelList = (CompletedLevels)SaveManager.LoadData("LevelData");
        if (loadedLevelList == null) loadedLevelList = new CompletedLevels(new List<Level>());
        loadedLevelList.completedLevelList.Add(new Level(0, 0, new List<int>()));
        SaveManager.SaveData(loadedLevelList, "LevelData");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}