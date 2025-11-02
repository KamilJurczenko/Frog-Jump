using System;
using TMPro;
using UnityEngine;

public class RewardButtonTimer : MonoBehaviour
{
    [SerializeField] GameObject CanEarnObj;

    public static float rewardTimer = 30f * 60f;

    public static TimeData timeDate;

    private TMP_Text timerForRewardText;

    public static float timeForRewardLeft;

    float timerMinutes;
    float timerSeconds;

    bool isEarn;
    // Start is called before the first frame update
    void Awake()
    {
        timerForRewardText = GetComponentInChildren<TMP_Text>();

        timeDate = (TimeData)SaveManager.LoadData("ExitTimeDate");
        if (timeDate != null)
        {
            TimeSpan diff = DateTime.Now - timeDate.dateTimeOnExit;
            timeForRewardLeft = rewardTimer - (float)diff.TotalSeconds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeForRewardLeft > 0)
        {
            if (isEarn) 
            { 
                CanEarnObj.SetActive(false);
                isEarn = false;
            }

            timeForRewardLeft -= Time.deltaTime;

            timerMinutes = Mathf.FloorToInt(timeForRewardLeft / 60f);
            timerSeconds = Mathf.FloorToInt(timeForRewardLeft - timerMinutes * 60);

            timerForRewardText.text = string.Format("{0:00}:{1:00}", timerMinutes, timerSeconds);
        }
        else if(!isEarn)
        {
            timerForRewardText.text = "";
            CanEarnObj.SetActive(true);
            isEarn = true;
        }
    }
}
