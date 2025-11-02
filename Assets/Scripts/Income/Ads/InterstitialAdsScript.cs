using UnityEngine;
using UnityEngine.Advertisements;

public class InterstitialAdsScript : MonoBehaviour
{
    private string gameId = "3816077";
    //private string gameId = "3816076"; iosKey

    public bool isShowing;

    void Start()
    {
        // Initialize the Ads service:
        Advertisement.Initialize(gameId, AdsEnabled.instance.testMode);
    }

    public void ShowInterstitialAd()
    {
        // Check if UnityAds ready before calling Show method:
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
        else
        {
            Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
        }
    }
    public void Update()
    {
        isShowing = Advertisement.isShowing;
    }
}