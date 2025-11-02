using UnityEngine;
using UnityEngine.SceneManagement;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] public AnimatorOverrideController[] overrideControllers;
    [SerializeField] private RuntimeAnimatorController staticRtAController;
    [SerializeField] private RuntimeAnimatorController playerRtController;

    public static SkinChanger instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        OverrideAnimatorController();
    }

    public void OverrideAnimatorController() //searches adequate ovC
    {
        string skinId = GetSkinName();
        for (int i = 0; i < overrideControllers.Length; i++)
        {
            if((skinId + "_Override") == overrideControllers[i].name)
            {
                if (SceneManager.GetActiveScene().name == "Menu") overrideControllers[i].runtimeAnimatorController = staticRtAController;
                else overrideControllers[i].runtimeAnimatorController = playerRtController;
                GetComponent<Animator>().runtimeAnimatorController = overrideControllers[i] as RuntimeAnimatorController;
            }
        }
    }

    public static string GetSkinName()
    {
        Skin skin = (Skin)SaveManager.LoadData("SkinData");
        if (skin == null) 
        { 
            //Debug.Log("No Skin found!"); 
            return "default"; 
        }
        else 
        { 
            //Debug.Log("Skin: " + skin.skinName); 
            return skin.skinName; 
        }
    }
    public string GetSkinRuntimeAnimatorControllerName()
    {
        return GetSkinName() + "_Override";
    }
}
