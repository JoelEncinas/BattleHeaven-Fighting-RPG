using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Specialized;

public class DailyGift : MonoBehaviour
{
    // UI
    List<GameObject> giftItems = new List<GameObject>();
    TextMeshProUGUI timer;
    GameObject timerGO;

    // gift received 
    GameObject dailyGiftGoldAndGems;
    GameObject dailyGiftSkills;
    GameObject dailyGiftGoldPopup;
    GameObject dailyGiftGemsPopup;

    TextMeshProUGUI goldOrGemsTitle;
    TextMeshProUGUI goldQuantity;
    TextMeshProUGUI gemsQuantity;

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

    // data
    static Fighter player;

    // Manager
    MainMenu mainMenu;

    // variables
    string lastButtonClicked = "";
    const int GIFT_TIME = 1;

    private void Awake()
    {
        SetUpUI();
        
        // setup on enable
        timerGO.SetActive(false);
        GetDailyItems();
        DisableInteraction();
        LoadUI();
        EnableNextReward();

        dailyGiftGoldAndGems.SetActive(false);
        dailyGiftSkills.SetActive(false);
        dailyGiftGoldPopup.SetActive(false);
        dailyGiftGemsPopup.SetActive(false);
        player = PlayerUtils.FindInactiveFighter();
    }

    private void SetUpUI()
    {
        // gift logic
        timer = GameObject.Find("Text_Daily_Time").GetComponent<TextMeshProUGUI>();
        timerGO = GameObject.Find("Icon_Daily_Time_Gift");
        mainMenu = GameObject.Find("MainMenuManager").GetComponent<MainMenu>(); // notifications system

        // collect reward popup
        dailyGiftSkills = GameObject.Find("Popup_Skill");
        dailyGiftGoldAndGems = GameObject.Find("Popup_Currencies");
        dailyGiftGoldPopup = GameObject.Find("DailyGoldReward");
        dailyGiftGemsPopup = GameObject.Find("DailyGemsReward");
        goldOrGemsTitle = GameObject.Find("Popup_Currencies_Title").GetComponent<TextMeshProUGUI>();
        goldQuantity = GameObject.Find("Gold_Quantity").GetComponent<TextMeshProUGUI>();
        gemsQuantity = GameObject.Find("Gems_Quantity").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (IsGiftAvailable())
            EnableNextReward();
        if (IsOutOfRewards())
            ResetWeek();

        if (UpdateTimer() > TimeSpan.Zero)
        {
            timerGO.SetActive(true);
            timer.text = UpdateTimer().ToString(@"hh\:mm\:ss");
        }
        else if (UpdateTimer() == TimeSpan.Zero)
        {
            timerGO.SetActive(false);
        }
    }

    /* Day items structure
     * 
     * - 1-6 rewards GO container
     *      - DayX
     *          - VFX
     *          - UI
     *          - UI
     *          - Reward value
     *          - Reward collected UI
     *          - Focus UI (item is collectable)
     *              - GO container
     *              - Text
     * - 7 reward
     */

    private void LoadUI()
    {
        // 0 - false
        // 1 - true
        float flag;
        string day;

        for (int i = 0; i < giftItems.Count; i++)
        {
            day = "DAY" + (i + 1);
            flag = PlayerPrefs.GetFloat(day);

            if(flag == 1)
                DisableButtonOnRewardCollected(day);
        }
    }

    private void SaveDay(string day)
    {
        PlayerPrefs.SetFloat(day, 1);
        PlayerPrefs.Save();
    }

    private void GetDailyItems()
    {
        for(int i = 1; i <= 7; i++)
            giftItems.Add(GameObject.Find("Day" + i));
    }

    private void DisableInteraction()
    {
        for (int i = 0; i < giftItems.Count; i++)
            giftItems[i].GetComponent<Button>().interactable = false;
    }

    public void ResetWeek()
    {
        for(int i = 0; i < giftItems.Count; i++)
            PlayerPrefs.SetFloat("DAY" + (i + 1), 0);

        PlayerPrefs.Save();
    }

    private bool IsOutOfRewards()
    {
        float counter = 0;

        for (int i = 0; i < giftItems.Count; i++)
             counter += PlayerPrefs.GetFloat("DAY" + (i + 1), 0);

        return counter == 7;
    }

    private Dictionary<string, string> GetRewardType(string day)
    {
        day = day.ToUpper();

        return new Dictionary<string, string>
        {
            { 
                DailyGiftDB.gifts[(DailyGiftDB.Days)Enum.Parse(typeof(DailyGiftDB.Days), day)]["reward"],
                DailyGiftDB.gifts[(DailyGiftDB.Days)Enum.Parse(typeof(DailyGiftDB.Days), day)]["value"]
            }
        };
    }

    private void GiveReward(Dictionary<string, string> reward)
    {
        FindObjectOfType<AudioManager>().Play("S_Reward_Received");
        
        if (reward.ContainsKey("gold"))
            EnableGoldPopup(reward);
        if (reward.ContainsKey("gems"))
            EnableGemsPopup(reward);
        if (reward.ContainsKey("chest"))
            EnableChestPopup(reward);
    }

    private void EnableGoldPopup(Dictionary<string, string> reward)
    {
        dailyGiftGoldAndGems.SetActive(true);
        CurrencyHandler.instance.AddGold(int.Parse(reward["gold"]));
        goldOrGemsTitle.text = "GOLD REWARD";
        dailyGiftGoldPopup.SetActive(true);
        goldQuantity.text = reward["gold"];
    }

    private void EnableGemsPopup(Dictionary<string, string> reward)
    {
        dailyGiftGoldAndGems.SetActive(true);
        CurrencyHandler.instance.AddGems(int.Parse(reward["gems"]));
        goldOrGemsTitle.text = "GEMS REWARD";
        dailyGiftGemsPopup.SetActive(true);
        gemsQuantity.text = reward["gems"];
    }

    private void EnableChestPopup(Dictionary<string, string> reward)
    {
        dailyGiftSkills.SetActive(true);
        SkillPopUpLogic(reward);
    }

    public void GiveRewardButton()
    {
        if(PlayerPrefs.GetFloat("firstDailyGift") == 0)
            SaveFirstTime(1);
        lastButtonClicked = EventSystem.current.currentSelectedGameObject.name.ToUpper();
        GiveReward(GetRewardType(lastButtonClicked));
        DisableButtonOnRewardCollected(lastButtonClicked);
        SaveDay(lastButtonClicked);
        mainMenu.DisableDailyGiftNotification();
        StartCountdown();
    }

    private void DisableButtonOnRewardCollected(string day)
    {
        for(int i = 0; i < giftItems.Count; i++)
        {
            if(giftItems[i].name.ToUpper() == day)
            {
                giftItems[i].transform.GetChild(4).gameObject.SetActive(true);
                giftItems[i].transform.GetChild(5).gameObject.SetActive(false);
                giftItems[i].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void EnableNextReward()
    {
        for (int i = 0; i < giftItems.Count; i++)
        {
            if (!giftItems[i].transform.GetChild(4).gameObject.activeSelf && IsGiftAvailable())
            {
                giftItems[i].transform.GetChild(4).gameObject.SetActive(false);
                giftItems[i].transform.GetChild(5).gameObject.SetActive(true);
                giftItems[i].GetComponent<Button>().interactable = true;

                return;
            }
        }
    }

    private void StartCountdown()
    {
        PlayerPrefs.SetString("giftCountdown", DateTime.Now.AddDays(GIFT_TIME).ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public bool IsGiftAvailable()
    {
        if (PlayerPrefs.GetFloat("firstDailyGift") == 0)
            return true;

        if (PlayerPrefs.GetString("giftCountdown") != "")
            return DateTime.Compare(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("giftCountdown"))), DateTime.Now) <= 0;

        return false;
    }

    public void GoToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.MainMenu.ToString());
    }

    public bool IsFirstTime()
    {
        return PlayerPrefs.GetFloat("firstDailyGift") == 0;
    }

    public void SaveFirstTime(int flag)
    {
        PlayerPrefs.SetFloat("firstDailyGift", flag);
        PlayerPrefs.Save();
    }

    private TimeSpan UpdateTimer()
    {
        if (PlayerPrefs.GetString("giftCountdown") != "")
            return DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("giftCountdown"))) - DateTime.Now;
        return TimeSpan.Zero;
    }

    private void SkillPopUpLogic(Dictionary<string, string> reward)
    {
        SkillCollection.SkillRarity skillRarityAwarded = GetRandomSkillRarityBasedOnChest(reward);
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

        Debug.Log(SkillCollection.skills
            .Where(skill => !HasSkillAlready(skill)).ToList().Count + " " + skillRarityAwarded);

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

        Debug.Log(skills.Count);
        //OrderedDictionary
        var awardedSkill = skills[skillIndex];

        return new Skill(awardedSkill["name"].ToString(), awardedSkill["description"].ToString(),
                awardedSkill["skillRarity"].ToString(), awardedSkill["category"].ToString(), awardedSkill["icon"].ToString());
    }

    private SkillCollection.SkillRarity GetRandomSkillRarityBasedOnChest(Dictionary<string, string> reward)
    {
        Dictionary<SkillCollection.SkillRarity, float> skillRarityProbabilitiesForChest =
            Chest.shopChests[(Chest.ShopChestTypes)Enum.Parse
            (typeof(Chest.ShopChestTypes), reward["chest"].ToUpper())];

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
}
