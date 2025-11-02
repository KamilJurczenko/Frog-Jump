using Google.Play.Review;
using System.Collections;
using UnityEngine;

public class InAppReview : MonoBehaviour
{
    private ReviewManager _reviewManager;
    private PlayReviewInfo _playReviewInfo;

    int launchCount;

    public static InAppReview instance;

    private void Awake()
    {
        transform.parent = null;

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        launchCount = PlayerPrefs.GetInt("TimesLaunched", 0);
        launchCount++;
        PlayerPrefs.SetInt("TimesLaunched", launchCount);
        //Debug.Log("App " + launchCount + " times launched");

        if (launchCount == 3 || launchCount == 6)
        {
            StartCoroutine(RequestReviews());
        }
    }

    public IEnumerator RequestReviews()
    {
        Debug.Log("Requesting Review...");

        _reviewManager = new ReviewManager();

        //Requests a ReviewInfo 
        var requestFlowOperation = _reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;
        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        _playReviewInfo = requestFlowOperation.GetResult();

        //Launches InApp Review Flow
        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
        yield return launchFlowOperation;
        _playReviewInfo = null; // Reset the object
        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            // Log error. For example, using requestFlowOperation.Error.ToString().
            yield break;
        }
        // The flow has finished. The API does not indicate whether the user
        // reviewed or not, or even whether the review dialog was shown. Thus, no
        // matter the result, we continue our app flow.
    }


}
