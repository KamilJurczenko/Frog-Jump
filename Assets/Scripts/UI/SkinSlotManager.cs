using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinSlotManager : MonoBehaviour
{
    public int skinCost;
    public string skinName;

    bool isSelected;
    bool canSelect;

    [SerializeField] public GameObject notBoughtObj;
    [SerializeField] public GameObject selectedTextObj;
    
    [SerializeField] TMP_Text costTextObj;
    [SerializeField] GameObject selectTextObj;
    [SerializeField] Image skinImage;

    SkinShopManager skinManager;

    private void Awake()
    {
        skinManager = GetComponentInParent<SkinShopManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ManageSlot();
    }

    public void ManageSlot()
    {
        isSelected = SkinChanger.GetSkinName() == skinName;
        canSelect = skinCost == 0 && !isSelected;

        costTextObj.text = skinCost.ToString();

        if (canSelect)
        { 
            selectTextObj.SetActive(true);
            notBoughtObj.SetActive(false);
        }
        else if (isSelected)
        {
            selectedTextObj.SetActive(true);
            if(skinManager != null)
                skinManager.currentSelectedSlot = this;
        }
        else notBoughtObj.SetActive(true);
    }
    public void OnSlotButtonClick()
    {
        if (canSelect) //if skin is bought then select
        {
            skinManager.currentSelectedSlot.Deselect();
            skinManager.currentSelectedSlot = this;
            skinName = skinName.ToLower();
            Skin skin = new Skin(skinName);
            SaveManager.SaveData(skin, "SkinData");
            selectedTextObj.SetActive(true);
            selectTextObj.SetActive(false);
            SkinChanger.instance.OverrideAnimatorController();
        }
        else if(!isSelected && skinCost != 0)
        {
            foreach (Transform t in SkinSlotTransform.instance.skinSlotTransform)
            {
                t.GetComponent<Button>().interactable = false;
            }
            skinManager.currentSelectedSlotToBuy = this;
            skinManager.BuySkinUI.SetActive(true);
            skinManager.costNumberText.text = skinCost.ToString();
            skinManager.skinToBuy.sprite = skinImage.sprite;
            skinManager.skinToBuy.SetNativeSize();
        }

    }
    public void Deselect()
    {
        selectedTextObj.SetActive(false);
        selectTextObj.SetActive(true);
        canSelect = true;
    }
}
