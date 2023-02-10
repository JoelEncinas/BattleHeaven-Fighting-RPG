using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Linq;
using System.Collections.Specialized;

public enum Transactions
{
    CHEST,
    ENERGY,
    GOLD
}

public class ShopUI : MonoBehaviour
{
    // UI
    GameObject buyConfirmation;
    GameObject noCurrencyButton;
    GameObject abortButton;
    GameObject confirmButton;
    TextMeshProUGUI messageText;
    GameObject chestsTab;
    GameObject energyTab;
    GameObject goldTab;
    GameObject chestsGroupdown;
    GameObject energyGroupdown;
    GameObject goldGroupdown;

    // UI chest rewards
    public GameObject skillTitle;
    public GameObject skillType;
    public GameObject skillRarity;
    public GameObject skillDescription;
    public GameObject skillIcon;
    public GameObject commonSkill;
    public GameObject rareSkill;
    public GameObject epicSkill;
    public GameObject legendarySkill;
    public Button skillInventory;
    public Button skillMainMenu;
    public Button allSkills;
    public GameObject canvasSkill;

    // chests
    public GameObject chestsContainer;
    public GameObject chestsScrollrect;
    public List<GameObject> chestsList;
    public GameObject soldOut;

    // energy on open
    GameObject energyPopUp;
    GameObject energyNextButton;

    // gold on open
    GameObject goldPopUp;
    GameObject goldNextButton;

    // shop flow
    string chestButtonPressed;
    string energyButtonPressed;
    string goldButtonPressed;
    int gemsValue = 0;
    int goldValue = 0;
    int previousTransaction;

    // data
    static Fighter player;

    private void Awake()
    {
        // Energy
        energyPopUp = GameObject.Find("Canvas_PopUp_Energy");
        energyNextButton = GameObject.Find("Button_Next_Energy");

        // Gold
        goldPopUp = GameObject.Find("Canvas_PopUp_Gold");
        goldNextButton = GameObject.Find("Button_Next_Gold");

        buyConfirmation = GameObject.Find("Canvas_Buy_Confirmation");
        noCurrencyButton = GameObject.Find("Button_NoCurrency");
        abortButton = GameObject.Find("Button_Abort");
        confirmButton = GameObject.Find("Button_Confirm");
        messageText = GameObject.Find("Message_Text").GetComponent<TextMeshProUGUI>();
        player = PlayerUtils.FindInactiveFighter();

        // tabs
        chestsTab = GameObject.Find("Button_Chests_Tab");
        energyTab = GameObject.Find("Button_Energy_Tab");
        goldTab = GameObject.Find("Button_Gold_Tab");

        // containers
        chestsGroupdown = GameObject.Find("Group_Down_Chests");
        chestsScrollrect = GameObject.Find("ScrollRect_Chests"); 
        energyGroupdown = GameObject.Find("Group_Down_Energy");
        goldGroupdown = GameObject.Find("Group_Down_Gold");

        // chests
        chestsContainer = GameObject.Find("Content_Chest");
        soldOut = GameObject.Find("SoldOut");
        InitChests();
        ManageChests();

        // hide UI on shop enter
        buyConfirmation.SetActive(false);
        noCurrencyButton.SetActive(false);
        abortButton.SetActive(false);
        confirmButton.SetActive(false);
        energyGroupdown.SetActive(false);
        goldGroupdown.SetActive(false);

        energyPopUp.SetActive(false);
        energyNextButton.SetActive(false);

        goldPopUp.SetActive(false);
        goldNextButton.SetActive(false);

        canvasSkill.SetActive(false);

        soldOut.SetActive(false);
    }

    private void Start()
    {
        ShowChestsTab();

        // button pressed from main menu
        ShowTab(ShopTab.GetTab());

        SceneFlag.sceneName = SceneNames.Shop.ToString();
    }

    public void ConfirmPurchase()
    {
        switch (previousTransaction)
        {
            case (int)Transactions.CHEST:
                HandleChestPopUp();
                break;
            case (int)Transactions.ENERGY:
                HandleEnergyPopUp();
                break;
            case (int)Transactions.GOLD:
                HandleGoldPopUp();
                break;
        }
    }

    public void ClosePurchase()
    {
        buyConfirmation.SetActive(false);
        noCurrencyButton.SetActive(false);
        energyPopUp.SetActive(false);
        energyNextButton.SetActive(false);
        goldPopUp.SetActive(false);
        goldNextButton.SetActive(false);
        allSkills.gameObject.SetActive(false);
    }

    public void BuyChest()
    {
        chestButtonPressed = EventSystem.current.currentSelectedGameObject.name;
        GetChestValueFromType(chestButtonPressed);
        previousTransaction = (int)Transactions.CHEST;

        // handle which chest was opened to change icon after
        if (CurrencyHandler.instance.HasEnoughGold(goldValue))
        {
            buyConfirmation.SetActive(true);
            abortButton.SetActive(true);
            confirmButton.SetActive(true);
            messageText.text = "Are you sure about buying this item ?";
        }
        else
        {
            buyConfirmation.SetActive(true);
            abortButton.SetActive(false);
            confirmButton.SetActive(false);
            noCurrencyButton.SetActive(true);
            messageText.text = "Not enough gold!";
        }
    }

    private void HandleChestPopUp()
    {
        FindObjectOfType<AudioManager>().Play("S_Reward_Received");
        CurrencyHandler.instance.SubstractGold(goldValue);
        buyConfirmation.SetActive(false);
        abortButton.SetActive(false);
        confirmButton.SetActive(false);
        canvasSkill.SetActive(true);
        SkillPopUpLogic();
    }

    public void BuyEnergy()
    {
        energyButtonPressed = EventSystem.current.currentSelectedGameObject.name;
        GetEnergyCostFromType(energyButtonPressed);
        previousTransaction = (int)Transactions.ENERGY;

        if (!CurrencyHandler.instance.HasEnergySurplus(GetEnergyValueFromType(energyButtonPressed)))
        {
            if (CurrencyHandler.instance.HasEnoughGems(gemsValue))
            {
                buyConfirmation.SetActive(true);
                abortButton.SetActive(true);
                confirmButton.SetActive(true);
                messageText.text = "Are you sure about buying this item ?";
            }
            else
            {
                buyConfirmation.SetActive(true);
                abortButton.SetActive(false);
                confirmButton.SetActive(false);
                noCurrencyButton.SetActive(true);
                messageText.text = "Not enough gems!";
            }
        }
        else
        {
            buyConfirmation.SetActive(true);
            abortButton.SetActive(false);
            confirmButton.SetActive(false);
            noCurrencyButton.SetActive(true);
            messageText.text = "Your energy can't be greater than " + PlayerUtils.maxEnergy + "!";
        }
    }

    public void HandleEnergyPopUp()
    {
        CurrencyHandler.instance.SubstractGems(gemsValue);
        CurrencyHandler.instance.AddEnergy(GetEnergyValueFromType(energyButtonPressed));
        buyConfirmation.SetActive(false);
        abortButton.SetActive(false);
        confirmButton.SetActive(false);
        energyPopUp.SetActive(true);
        energyNextButton.SetActive(true);
    }

    public void BuyGold()
    {
        goldButtonPressed = EventSystem.current.currentSelectedGameObject.name;
        GetGoldCostFromType(goldButtonPressed);
        previousTransaction = (int)Transactions.GOLD;

        if (CurrencyHandler.instance.HasEnoughGems(gemsValue))
        {
            buyConfirmation.SetActive(true);
            abortButton.SetActive(true);
            confirmButton.SetActive(true);
            messageText.text = "Are you sure about buying this item ?";
        }
        else
        {
            buyConfirmation.SetActive(true);
            abortButton.SetActive(false);
            confirmButton.SetActive(false);
            noCurrencyButton.SetActive(true);
            messageText.text = "Not enough gems!";
        }
    }

    public void HandleGoldPopUp()
    {
        CurrencyHandler.instance.SubstractGems(gemsValue);
        CurrencyHandler.instance.AddGold(GetGoldValueFromType(goldButtonPressed));
        buyConfirmation.SetActive(false);
        abortButton.SetActive(false);
        confirmButton.SetActive(false);
        goldPopUp.SetActive(true);
        goldNextButton.SetActive(true);
    }

    public void ShowTab(string buttonPressed)
    {
        switch (buttonPressed)
        {
            case "Button_Chests":
                ShowChestsTab();
                HideEnergyTab();
                HideGoldTab();
                break;
            case "Button_Energy":
                ShowEnergyTab();
                HideChestsTab();
                HideGoldTab();
                break;
            case "Button_Gold":
                ShowGoldTab();
                HideChestsTab();
                HideEnergyTab();
                break;
        }
    }

    public void ShowChestsTab()
    {
        chestsTab.transform.Find("Focus").GetComponent<Image>().enabled = true;
        if (PlayerHasAllSkills())
        {
            chestsGroupdown.SetActive(true);
            chestsScrollrect.SetActive(false);
            soldOut.SetActive(true);
        }
        else
        {
            chestsGroupdown.SetActive(true);
            chestsScrollrect.SetActive(true);
            soldOut.SetActive(false);
        }

    }

    public void ShowEnergyTab()
    {
        energyTab.transform.Find("Focus").GetComponent<Image>().enabled = true;
        energyGroupdown.SetActive(true);
    }

    public void ShowGoldTab()
    {
        goldTab.transform.Find("Focus").GetComponent<Image>().enabled = true;
        goldGroupdown.SetActive(true);
    }

    public void HideChestsTab()
    {
        chestsTab.transform.Find("Focus").GetComponent<Image>().enabled = false;
        chestsGroupdown.SetActive(false);
    }

    public void HideEnergyTab()
    {
        energyTab.transform.Find("Focus").GetComponent<Image>().enabled = false;
        energyGroupdown.SetActive(false);
    }

    public void HideGoldTab()
    {
        goldTab.transform.Find("Focus").GetComponent<Image>().enabled = false;
        goldGroupdown.SetActive(false);
    }

    public void GetChestValueFromType(string chestType)
    {
        goldValue = Chest.shopChestsValue[(Chest.ShopChestTypes)Enum.Parse
            (typeof(Chest.ShopChestTypes), chestType.ToUpper())]["gold"];
    }

    public void GetEnergyCostFromType(string energyBundle)
    {
        gemsValue = Energy.shopEnergyBundlesCost[(Energy.ShopEnergyBundles)Enum.Parse
            (typeof(Energy.ShopEnergyBundles), energyBundle.ToUpper())]["gems"];
    }

    public int GetEnergyValueFromType(string energyBundle)
    {
        return Energy.shopEnergyBundlesValue[(Energy.ShopEnergyBundles)Enum.Parse
            (typeof(Energy.ShopEnergyBundles), energyBundle.ToUpper())]["energy"];
    }

    public void GetGoldCostFromType(string goldBundle)
    {
        gemsValue = Gold.shopGoldBundlesCost[(Gold.ShopGoldBundles)Enum.Parse
            (typeof(Gold.ShopGoldBundles), goldBundle.ToUpper())]["gems"];
    }

    public int GetGoldValueFromType(string goldBundle)
    {
        return Gold.shopGoldBundlesValue[(Gold.ShopGoldBundles)Enum.Parse
            (typeof(Gold.ShopGoldBundles), goldBundle.ToUpper())]["gold"];
    }

    static Func<bool> PlayerHasAllSkills = () => SkillCollection.skills.Count() == player.skills.Count();

    private void SkillPopUpLogic()
    {
        SkillCollection.SkillRarity skillRarityAwarded = GetRandomSkillRarityBasedOnChest();
        Skill skillInstance = GetAwardedSkill(skillRarityAwarded);
        Fighter player = PlayerUtils.FindInactiveFighter();
        player.skills.Add(skillInstance);
        player.SaveFighter();
        Notifications.TurnOnNotification();
        Notifications.IncreaseCardsUnseen();

        ShowSkillData(skillInstance);
        ShowSkillIcon(skillInstance);
    }

    private void ShowSkillData(Skill skill)
    {
        skillTitle.GetComponent<TextMeshProUGUI>().text = skill.skillName;
        skillType.GetComponent<TextMeshProUGUI>().text = skill.category;
        skillRarity.GetComponent<TextMeshProUGUI>().text = skill.rarity;
        skillDescription.GetComponent<TextMeshProUGUI>().text = skill.description;
    }

    private bool HasSkillAlready(OrderedDictionary skill)
    {
        return player.skills.Any(playerSkill => playerSkill.skillName == skill["name"].ToString());
    }
    private Skill GetAwardedSkill(SkillCollection.SkillRarity skillRarityAwarded)
    {
        //List of OrderedDictionaries
        //Filter the ones that are from another rarity and the ones the player already has
        var skills = SkillCollection.skills
            .Where(skill => (string)skill["skillRarity"] == skillRarityAwarded.ToString())
            .Where(skill => !HasSkillAlready(skill))
            .ToList();

        //If player has all skill for the current rarity get skills from a rarity above. 
        //Does not matter that they might not belong to the current chest
        if (!skills.Any())
        {
            Debug.Log("User has all skills for the given rarity.");

            //Cast enum to int
            int skillRarityIndex = (int)skillRarityAwarded;

            //If value for the next index in the enum exists return that rarity. Otherwise return the first value of the enum (COMMON)
            SkillCollection.SkillRarity newRarity = (Enum.IsDefined(typeof(SkillCollection.SkillRarity), (SkillCollection.SkillRarity)skillRarityIndex++) && skillRarityIndex < 4)
            ? (SkillCollection.SkillRarity)skillRarityIndex++
            : (SkillCollection.SkillRarity)0;

            //Recursive call with the new rarity
            return GetAwardedSkill(newRarity);
        }

        int skillIndex = UnityEngine.Random.Range(0, skills.Count());

        //OrderedDictionary
        var awardedSkill = skills[skillIndex];

        return new Skill(awardedSkill["name"].ToString(), awardedSkill["description"].ToString(),
                awardedSkill["skillRarity"].ToString(), awardedSkill["category"].ToString(), awardedSkill["icon"].ToString());
    }

    private SkillCollection.SkillRarity GetRandomSkillRarityBasedOnChest()
    {
        Dictionary<SkillCollection.SkillRarity, float> skillRarityProbabilitiesForChest = 
            Chest.shopChests[(Chest.ShopChestTypes)Enum.Parse
            (typeof(Chest.ShopChestTypes), chestButtonPressed.ToUpper())];

        float diceRoll = UnityEngine.Random.Range(0f, 100);

        foreach (KeyValuePair<SkillCollection.SkillRarity, float> skill in skillRarityProbabilitiesForChest)
        {
            if (skill.Value >= diceRoll)
                return skill.Key;

            diceRoll -= skill.Value;
        }

        Debug.LogError("Error");
        //Fallback
        return SkillCollection.SkillRarity.COMMON;
    }

    private void ShowSkillIcon(Skill skill)
    {
        //ShowFrame
        commonSkill.SetActive(SkillCollection.SkillRarity.COMMON == GeneralUtils.StringToSkillRarityEnum(skill.rarity));
        rareSkill.SetActive(SkillCollection.SkillRarity.RARE == GeneralUtils.StringToSkillRarityEnum(skill.rarity));
        epicSkill.SetActive(SkillCollection.SkillRarity.EPIC == GeneralUtils.StringToSkillRarityEnum(skill.rarity));
        legendarySkill.SetActive(SkillCollection.SkillRarity.LEGENDARY == GeneralUtils.StringToSkillRarityEnum(skill.rarity));

        //Show icon
        Sprite icon = Resources.Load<Sprite>("Icons/IconsSkills/" + skill.icon);
        skillIcon.GetComponent<Image>().sprite = icon;
    }

    private void InitChests()
    {
        chestsList = new List<GameObject>();

        for(int i = 0; i < chestsContainer.transform.childCount; i++)
            chestsList.Add(chestsContainer.transform.GetChild(i).gameObject);
    }

    private void ManageChests()
    {
        ManageLegendaryChest();
        ManageEpicChest();
        ManageSpecialChest();
    }

    private void ManageLegendaryChest()
    {
        var skills = SkillCollection.skills
            .Where(skill => (string)skill["skillRarity"] == SkillCollection.SkillRarity.LEGENDARY.ToString())
            .Where(skill => !HasSkillAlready(skill))
            .ToList();

        if (!skills.Any())
            chestsList.Where(gameObject => gameObject.name.ToUpper() == Chest.ShopChestTypes.LEGENDARY.ToString()).ToList()[0].SetActive(false);    
    }

    private void ManageEpicChest()
    {
        var skills = SkillCollection.skills
            .Where(skill => ((string)skill["skillRarity"] == SkillCollection.SkillRarity.EPIC.ToString()) ||  
                             (string)skill["skillRarity"] == SkillCollection.SkillRarity.LEGENDARY.ToString())
            .Where(skill => !HasSkillAlready(skill))
            .ToList();

        if (!skills.Any())
            chestsList.Where(gameObject => gameObject.name.ToUpper() == Chest.ShopChestTypes.EPIC.ToString()).ToList()[0].SetActive(false);
    }

    private void ManageSpecialChest()
    {
        var skills = SkillCollection.skills
            .Where(skill => ((string)skill["skillRarity"] == SkillCollection.SkillRarity.RARE.ToString()) ||
                             (string)skill["skillRarity"] == SkillCollection.SkillRarity.EPIC.ToString() ||
                             (string)skill["skillRarity"] == SkillCollection.SkillRarity.LEGENDARY.ToString())
            .Where(skill => !HasSkillAlready(skill))
            .ToList();

        if (!skills.Any())
            chestsList.Where(gameObject => gameObject.name.ToUpper() == Chest.ShopChestTypes.SPECIAL.ToString()).ToList()[0].SetActive(false);
    }
}