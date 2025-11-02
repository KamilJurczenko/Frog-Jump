using UnityEngine;
using System.Collections;
using TMPro;

public class FlashingTextScript : MonoBehaviour
{

    TMP_Text flashingText;
    string text;
    [SerializeField] float speed;

    void Start()
    {
        //get the Text component
        flashingText = GetComponent<TMP_Text>();

        text = flashingText.text;

        //Call coroutine BlinkText on Start
        StartCoroutine(BlinkText());
    }

    //function to blink the text
    public IEnumerator BlinkText()
    {
        //blink it forever. You can set a terminating condition depending upon your requirement
        while (true)
        {
            //set the Text's text to blank
            flashingText.text = "";
            //display blank text for 0.5 seconds
            yield return new WaitForSeconds(speed);
            //display “I AM FLASHING TEXT” for the next 0.5 seconds
            flashingText.text = text;
            yield return new WaitForSeconds(speed);
        }
    }

}