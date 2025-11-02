using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyButton : MonoBehaviour
{

    [SerializeField] GameObject EasyModeButtonOff;

    [SerializeField] GameObject HardModeButtonOff;

    int difficulty;

    // Start is called before the first frame update
    void Start()
    {
        // difficulty == 0 = Easy Mode On
        difficulty = PlayerPrefs.GetInt("Difficulty", 0);
        if(difficulty == 0)
        {
            EasyModeOn();
        }
        else
        {
            HardModeOn();
        }
    }

    private void EasyModeOn()
    {
        EasyModeButtonOff.SetActive(false);
        HardModeButtonOff.SetActive(true);
        difficulty = 0;
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    private void HardModeOn()
    {
        EasyModeButtonOff.SetActive(true);
        HardModeButtonOff.SetActive(false);
        difficulty = 1;
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void OnEasyButtonClick()
    {
        if(difficulty == 1)
        {
            EasyModeOn();
        }
    }

    public void OnHardButtonClick()
    {
        if (difficulty == 0)
        {
            HardModeOn();
        }
    }
}
