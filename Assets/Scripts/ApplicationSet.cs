using UnityEngine;

public class ApplicationSet : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        PlayerController.totalDeaths = 0;
    }

}
