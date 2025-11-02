using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAdScript : MonoBehaviour
{
    private string gameId = "3816077";
    //private string gameId = "3816076"; iosKey

    public string placementId = "bannerAd";

    void Start()
    {
        if (AdsEnabled.instance.adsEnabled)
        {
            Advertisement.Initialize(gameId, AdsEnabled.instance.testMode);
            StartCoroutine(ShowBannerWhenInitialized());
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        }
    }
    public void HideBanner()
    {
        Advertisement.Banner.Hide(false);
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show(placementId);
    }
}