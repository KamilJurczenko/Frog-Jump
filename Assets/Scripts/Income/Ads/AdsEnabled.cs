using UnityEngine;

public class AdsEnabled : MonoBehaviour
{
    public static AdsEnabled instance;

    public bool adsEnabled;

    public bool testMode = false;
    private void Awake()
    {
        adsEnabled = true;

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

}
