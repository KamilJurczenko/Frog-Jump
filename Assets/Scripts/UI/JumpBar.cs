using UnityEngine;
using UnityEngine.UI;

public class JumpBar : MonoBehaviour
{
    Slider jumpBar;

    [SerializeField] Transform target;
    [SerializeField] float offset;

    bool childrenActive;

    // Start is called before the first frame update
    void Start()
    {
        int difficulty = PlayerPrefs.GetInt("Difficulty");
        if (difficulty == 1)
            gameObject.SetActive(false);

        jumpBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.timeHeld != 0 && childrenActive == false)
        {
            foreach (Transform t in transform)
                t.gameObject.SetActive(true);
            childrenActive = true;
        }
        else if (PlayerController.timeHeld == 0)
        {
            childrenActive = false;
            foreach (Transform t in transform)
                t.gameObject.SetActive(false);
        }
        transform.position = new Vector2(target.position.x, target.transform.position.y + offset);

        float progress = Mathf.Clamp01(PlayerController.timeHeld);
        jumpBar.value = progress;
    }
}
