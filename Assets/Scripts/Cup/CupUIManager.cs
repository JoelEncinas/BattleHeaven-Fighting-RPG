using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/* CUP STRUCTURE IDS
 * 
 * QUARTERS     SEMIS       FINAL       SEMIS       QUARTERS
 * 
 *    1            9         13          11            5
 *    2           10         14          12            6
 * 
 *    3                                                7
 *    4                                                8
 *    
 * ----------------------------------------------------------
 * 
 * MATCHES STRUCTURE IDS
 * 
 * QUARTERS     SEMIS       FINAL       SEMIS       QUARTERS
 * 
 *    1           5           7           6            3
 *                                              
 *    2                                                4
 *                                                    
 *    
 * ----------------------------------------------------------
 * 
 * Players gameobject structure
 * 
 * - BG
 *      - mask
 *          - species portrait
 * - nickname
 * - BGFade (when eliminated)
 * 
 */

public class CupUIManager : MonoBehaviour
{
    // UI
    Transform labelContainer;
    Transform playersContainer;
    List<Transform> participants;
    TextMeshProUGUI roundAnnouncer;
    Button buttonCollectRewards;

    // prize
    Canvas prizeCanvas;
    GameObject cupGoldAndGems;
    GameObject cupSkills;
    GameObject cupGoldPopup;
    GameObject cupGemsPopup;

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
    public GameObject battleBtnContainer;
    public Button skillInventory;
    public Button skillMainMenu;
    public Button allSkills;
    

    // data
    static Fighter player;

    // scripts
    CupManager cupManager;

    // vars
    public string round;
    private Color32 playerHihglight = new Color32(254, 161, 0, 255);
    const int CUP_COOLDOWN_IN_DAYS = 5;

    private void Awake()
    {
        SetUpUI();
        IsTournamentOver();
        HideCupLabels();
        GetAllUIPlayers();
        SetUIBasedOnRound();
        SetUpButtons();

        // Initial setup
        cupGoldAndGems.SetActive(false);
        cupSkills.SetActive(false);
        cupGoldPopup.SetActive(false);
        cupGemsPopup.SetActive(false);
        player = PlayerUtils.FindInactiveFighter();
    }

    private IEnumerator Start()
    {
        ShowCupLabel();

        if (SceneFlag.sceneName == SceneNames.Combat.ToString() || SceneFlag.sceneName == SceneNames.LevelUp.ToString())
        {
            StartCoroutine(SceneManagerScript.instance.FadeIn());
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));
        }

        SceneFlag.sceneName = SceneNames.Cup.ToString();
    }

    private void SetUpUI()
    {
        // cup bracket
        labelContainer = GameObject.Find("LabelContainer").GetComponent<Transform>();
        playersContainer = GameObject.Find("Players").GetComponent<Transform>();
        roundAnnouncer = GameObject.Find("RoundAnnouncerTxt").GetComponent<TextMeshProUGUI>();
        buttonCollectRewards = GameObject.Find("Button_Rewards").GetComponent<Button>();
        cupManager = GetComponent<CupManager>();

        // collect reward popup
        prizeCanvas = GameObject.Find("PrizeCanvas").GetComponent<Canvas>();
        cupSkills = GameObject.Find("Popup_Skill");
        cupGoldAndGems = GameObject.Find("Popup_Currencies");
        cupGoldPopup = GameObject.Find("GoldReward");
        cupGemsPopup = GameObject.Find("GemsReward");
        goldOrGemsTitle = GameObject.Find("Popup_Currencies_Title").GetComponent<TextMeshProUGUI>();
        goldQuantity = GameObject.Find("Gold_Quantity").GetComponent<TextMeshProUGUI>();
        gemsQuantity = GameObject.Find("Gems_Quantity").GetComponent<TextMeshProUGUI>();
    }

    private void SetUpButtons()
    {
        buttonCollectRewards.GetComponent<Button>().onClick.AddListener(() => GiveReward(GetRewardType(GetCupRound())));
    }

    private void GetAllUIPlayers()
    {
        participants = new List<Transform>();

        for (int i = 0; i < playersContainer.childCount; i++)
            participants.Add(playersContainer.GetChild(i));
    }

    private void IsTournamentOver()
    {
        if (Cup.Instance.round == CupDB.CupRounds.END.ToString() || !Cup.Instance.playerStatus)
        {
            battleBtnContainer.SetActive(false);
            buttonCollectRewards.gameObject.SetActive(true);
        }
        else
        {
            battleBtnContainer.SetActive(true);
            buttonCollectRewards.gameObject.SetActive(false);
        }
    }

    private void DisplayPlayerQuarters()
    {
        var participantsList = Cup.Instance.participants;

        playersContainer.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().color = playerHihglight;
        int counter = 0;

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Quarters"))
            {
                player.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                    GetSpeciePortrait(participantsList[counter].species);
                player.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    participantsList[counter].fighterName;

                counter++;
            }
        }
    }

    private void DisplayPlayerSemis()
    {
        int counter = 0;
        List<CupFighter> _participants = cupManager.GenerateParticipantsBasedOnQuarters();

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Semis"))
            {
                player.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                    GetSpeciePortrait(_participants[counter].species);
                player.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    _participants[counter].fighterName;

                if (_participants[counter].id == "0")
                    playersContainer.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>().color = playerHihglight;

                counter++;
            }
        }

        GrayOutLosersQuarters();
    }

    private void DisplayPlayerFinals(string round)
    {
        int counter = 0;
        List<CupFighter> _participants = cupManager.GenerateParticipantsBasedOnSemis();

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Finals"))
            {
                player.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite =
                    GetSpeciePortrait(_participants[counter].species);
                player.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                    _participants[counter].fighterName;

                if (_participants[counter].id == "0")
                    playersContainer.GetChild(12).GetChild(1).GetComponent<TextMeshProUGUI>().color = playerHihglight;

                counter++;
            }
        }

        if (round == "FINALS")
            GrayOutLosersSemis();
        if (round == "END")
            GrayOutLoserFinals();
    }

    private void SetUIBasedOnRound()
    {
        switch (Cup.Instance.round)
        {
            case "QUARTERS":
                SetUIQuarters();
                DisplayPlayerQuarters();
                break;
            case "SEMIS":
                SetUISemis();
                DisplayPlayerQuarters();
                DisplayPlayerSemis();
                break;
            case "FINALS":
                SetUIFinals();
                DisplayPlayerQuarters();
                DisplayPlayerSemis();
                DisplayPlayerFinals("FINALS");
                break;
            case "END":
                SetUIFinalsEnd();
                DisplayPlayerQuarters();
                DisplayPlayerSemis();
                DisplayPlayerFinals("FINALS");
                DisplayPlayerFinals("END");
                break;
        }
    }

    private void SetUIQuarters()
    {
        roundAnnouncer.text = CupDB.CupRounds.QUARTERS.ToString();

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Semis") || player.name.Contains("Finals"))
            {
                player.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0);
                player.GetChild(1).GetComponent<TextMeshProUGUI>().text = "???";
            }
        }
    }

    private void SetUISemis()
    {
        roundAnnouncer.text = CupDB.CupRounds.SEMIS.ToString(); ;

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Finals"))
            {
                player.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(0, 0, 0);
                player.GetChild(1).GetComponent<TextMeshProUGUI>().text = "???";
            }
        }
    }

    private void SetUIFinals()
    {
        roundAnnouncer.text = CupDB.CupRounds.FINALS.ToString();
    }

    private void SetUIFinalsEnd()
    {
        string winnerId = Cup.Instance.cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["winner"];
        string winnerName = "";
        foreach (CupFighter fighter in Cup.Instance.participants)
        {
            if (fighter.id == winnerId)
                winnerName = fighter.fighterName;
        }

        roundAnnouncer.text = "TOURNAMENT ENDED\n" + "WINNER " + winnerName + "!";
    }

    private void HideCupLabels()
    {
        for (int i = 0; i < labelContainer.childCount; i++)
            labelContainer.GetChild(i).gameObject.SetActive(false);
    }

    private Transform GetCupLabelByName(string name)
    {
        switch (name)
        {
            case "DIVINE":
                return labelContainer.GetChild(0);
        }

        // default
        return labelContainer.GetChild(0);
    }

    private void ShowCupLabel()
    {
        GetCupLabelByName(Cup.Instance.cupName).gameObject.SetActive(true);
    }

    private Sprite GetSpeciePortrait(string species)
    {
        return Resources.Load<Sprite>("CharacterProfilePicture/" + species);
    }

    public void GrayOutLosersQuarters()
    {
        var participantsList = Cup.Instance.participants;
        var cupInfo = Cup.Instance.cupInfo;
        List<string> loserIds = new List<string>();
        int counter = 5; // match ids + 1

        for (int i = 1; i < counter; i++)
            loserIds.Add(cupInfo[CupDB.CupRounds.QUARTERS.ToString()][i.ToString()]["loser"]);

        counter = 0;

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Quarters"))
            {
                for (int i = 0; i < loserIds.Count; i++)
                    if (participantsList[counter].id == loserIds[i])
                        player.GetChild(2).GetComponent<Image>().enabled = true;

                counter++;
            }
        }
    }

    public void GrayOutLosersSemis()
    {
        var participantsList = cupManager.GenerateParticipantsBasedOnQuarters();
        var cupInfo = Cup.Instance.cupInfo;
        List<string> loserIds = new List<string>();
        int counter = 7; // match ids + 1

        for (int i = 5; i < counter; i++)
            loserIds.Add(cupInfo[CupDB.CupRounds.SEMIS.ToString()][i.ToString()]["loser"]);

        counter = 0;

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Semis"))
            {
                for (int i = 0; i < loserIds.Count; i++)
                    if (participantsList[counter].id == loserIds[i])
                        player.GetChild(2).GetComponent<Image>().enabled = true;

                counter++;
            }
        }
    }

    public void GrayOutLoserFinals()
    {
        var participantsList = cupManager.GenerateParticipantsBasedOnSemis();
        var cupInfo = Cup.Instance.cupInfo;
        List<string> loserIds = new List<string>();
        int counter = 7; // match ids + 1 

        loserIds.Add(cupInfo[CupDB.CupRounds.FINALS.ToString()][counter.ToString()]["loser"]);

        counter = 0;

        foreach (Transform player in participants)
        {
            if (player.name.Contains("Finals"))
            {
                for (int i = 0; i < loserIds.Count; i++)
                    if (participantsList[counter].id == loserIds[i])
                        player.GetChild(2).GetComponent<Image>().enabled = true;

                counter++;
            }
        }
    }

    private string GetPlayerFinalResult()
    {
        if (Cup.Instance.cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["winner"] == "0")
        {
            if (Cup.Instance.cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["winner"] == "0")
            {
                if (Cup.Instance.cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["winner"] == "0")
                {
                    return CupDB.CupRounds.FINALS.ToString();
                }
                else
                {
                    return CupDB.CupRounds.QUARTERS.ToString();
                }
            } else
            {
                return CupDB.CupRounds.SEMIS.ToString();
            }
        } 
        else
        {
            return CupDB.CupRounds.ZERO.ToString();
        }
    }

    // Prizes logic
    private string GetCupRound()
    {
        return GetPlayerFinalResult();
    }

    private Dictionary<string, string> GetRewardType(string round)
    {
        return new Dictionary<string, string>
        {
            {
                CupDB.prizes[(CupDB.CupRounds)Enum.Parse(typeof(CupDB.CupRounds), round)]["reward"],
                CupDB.prizes[(CupDB.CupRounds)Enum.Parse(typeof(CupDB.CupRounds), round)]["value"]
            }
        };
    }

    private void GiveReward(Dictionary<string, string> reward)
    {
        FindObjectOfType<AudioManager>().Play("S_Reward_Received");

        prizeCanvas.enabled = true;

        if (reward.ContainsKey("gold"))
            EnableGoldPopup(reward);
        if (reward.ContainsKey("gems"))
            EnableGemsPopup(reward);
        if (reward.ContainsKey("chest"))
            EnableChestPopup(reward);

        ResetCup();
    }

    private void ResetCup()
    {
        PlayerPrefs.SetString("cupCountdown", DateTime.Now.AddDays(CUP_COOLDOWN_IN_DAYS).ToBinary().ToString());
        PlayerPrefs.Save();

        cupManager.DeleteCupFile();
    }

    private void EnableGoldPopup(Dictionary<string, string> reward)
    {
        cupGoldAndGems.SetActive(true);
        CurrencyHandler.instance.AddGold(int.Parse(reward["gold"]));
        goldOrGemsTitle.text = "GOLD REWARD";
        cupGoldPopup.SetActive(true);
        goldQuantity.text = reward["gold"];
    }

    private void EnableGemsPopup(Dictionary<string, string> reward)
    {
        cupGoldAndGems.SetActive(true);
        CurrencyHandler.instance.AddGems(int.Parse(reward["gems"]));
        goldOrGemsTitle.text = "GEMS REWARD";
        cupGemsPopup.SetActive(true);
        gemsQuantity.text = reward["gems"];
    }

    private void EnableChestPopup(Dictionary<string, string> reward)
    {
        cupSkills.SetActive(true);
        SkillPopUpLogic(reward);
    }

    // Handle chest
    // TODO v2: This function is declared in 4 different places: Cup, shop, levelup & dailygift
    // We could create a static class to reuse all of this logic. 
    // Its a bit of a mess because the functions called inside this function are different on each class
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
