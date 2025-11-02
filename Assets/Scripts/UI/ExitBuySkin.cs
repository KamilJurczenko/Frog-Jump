using UnityEngine;
using UnityEngine.UI;

public class ExitBuySkin : MonoBehaviour
{
    Button exitButton;


    private void Awake()
    {
        exitButton = GetComponent<Button>();
    }
    private void Start()
    {
        exitButton.onClick.AddListener(SetInteractable);
    }

    public static void SetInteractable()
    {
        foreach (Transform t in SkinSlotTransform.instance.skinSlotTransform)
        {
            t.GetComponent<Button>().interactable = true;
        }
    }
}
