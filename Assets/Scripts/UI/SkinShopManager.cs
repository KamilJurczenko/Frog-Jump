using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinShopManager : MonoBehaviour
{
    [SerializeField] public TMP_Text playersCurrencyText;

    [Header("Buy Skin Menu")]
    [SerializeField] public GameObject BuySkinUI;
    [SerializeField] public Image skinToBuy;
    [SerializeField] public TMP_Text costNumberText;


    public SkinSlotManager currentSelectedSlot;
    public SkinSlotManager currentSelectedSlotToBuy;

    public static Player playerData;
    UnlockedSkins unlockedSkinsData;

    private void Awake()
    {
        playerData = (Player)SaveManager.LoadData("PlayerData");
        unlockedSkinsData = (UnlockedSkins)SaveManager.LoadData("UnlockedSkinsData");
    }
    private void Start()
    {
        if (unlockedSkinsData == null)
        {
            unlockedSkinsData = new UnlockedSkins(new List<string>());
        }
        else
        {
            foreach (Transform t in SkinSlotTransform.instance.skinSlotTransform)
            {
                if (unlockedSkinsData.unlockedSkins.Contains(t.GetComponent<SkinSlotManager>().skinName))
                {
                    SkinSlotManager currentSlot = t.GetComponent<SkinSlotManager>();
                    currentSlot.skinCost = 0;
                    currentSlot.ManageSlot();
                }
            }
        }

        if (playerData == null)
        {
            playersCurrencyText.text = "0";
            playerData = new Player(0);
        }
        else playersCurrencyText.text = playerData.playerMoney.ToString();
    }
    private void Update()
    {
        if(playerData.playerMoney != int.Parse(playersCurrencyText.text))
        {
            playersCurrencyText.text = playerData.playerMoney.ToString();
        }
    }

    public void OnBuyButtonClick()
    {
        if(playerData.playerMoney >= currentSelectedSlotToBuy.skinCost)
        {
            currentSelectedSlot.Deselect();
            currentSelectedSlot = currentSelectedSlotToBuy;
            Skin skin = new Skin(currentSelectedSlot.skinName);
            SaveManager.SaveData(skin, "SkinData");
            playerData.playerMoney -= currentSelectedSlot.skinCost;
            playersCurrencyText.text = playerData.playerMoney.ToString();
            SaveManager.SaveData(playerData, "PlayerData");
            currentSelectedSlot.skinCost = 0;
            unlockedSkinsData.unlockedSkins.Add(currentSelectedSlot.skinName);
            currentSelectedSlot.selectedTextObj.SetActive(true);
            SaveManager.SaveData(unlockedSkinsData, "UnlockedSkinsData");
            currentSelectedSlot.notBoughtObj.SetActive(false);
            BuySkinUI.SetActive(false);
            SkinChanger.instance.OverrideAnimatorController();
            ExitBuySkin.SetInteractable();
        }
    }

}
