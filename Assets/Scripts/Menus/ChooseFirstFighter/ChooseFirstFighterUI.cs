using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseFirstFighterUI : MonoBehaviour
{
    // UI
    // canvases
    public Canvas chooseFighter;
    public Canvas chooseName;
    public Canvas chooseCountry;

    // fighters
    private Transform fighterLeftObject;
    private Transform fighterMidObject;
    private Transform fighterRightObject;
    private Transform fighterLeftRing;
    private Transform fighterMidRing;
    private Transform fighterRightRing;
    private Transform fighterLeftSpecieTitle;
    private Transform fighterMidSpecieTitle;
    private Transform fighterRightSpecieTitle;

    // stats bar fade
    private CanvasGroup fighterLeftGroup;
    private CanvasGroup fighterMidGroup;
    private CanvasGroup fighterRightGroup;

    // animations
    private SetFighterAnimations fighterLeftAnimator;
    private SetFighterAnimations fighterMidAnimator;
    private SetFighterAnimations fighterRightAnimator;

    // stats per fighter
    private TextMeshProUGUI fighterLeftDamageText;
    private TextMeshProUGUI fighterLeftHpText;
    private TextMeshProUGUI fighterLeftSpeedText;

    private TextMeshProUGUI fighterMidDamageText;
    private TextMeshProUGUI fighterMidHpText;
    private TextMeshProUGUI fighterMidSpeedText;

    private TextMeshProUGUI fighterRightDamageText;
    private TextMeshProUGUI fighterRightHpText;
    private TextMeshProUGUI fighterRightSpeedText;

    // Shadow fighters and UI
    private Color32 shadowedColor = new Color32(90, 90, 90, 255);
    private Color32 visibleColor = new Color32(255, 255, 255, 255);
    private readonly float initialAlphaUI = 0.5f;
    private readonly float visibleAlphaUI = 1f;

    private Button prev;
    private Button next;
    public TextMeshProUGUI panelInfo;

    // NAME
    public TMP_InputField nameInputField;
    public TextMeshProUGUI regexText;

    private List<Transform> flags;
    private Transform flagsContainer;

    public GameObject flagErrorOk;

    // RANKING
    const int MINUTES_BETWEEN_USER_RANKING_UPDATE = 720;
    const int RANKING_MIN_STARTING_POSITION = 1100;
    const int RANKING_MAX_STARTING_POSITION = 1500;

    private void Awake()
    {
        HandleUIOnAwake();
    }

    private void HandleUIOnAwake()
    {
        // References ------------------------------------------------------------------------------------------
        chooseFighter = GameObject.Find("Canvas_Choose_Fighter").GetComponent<Canvas>();
        chooseName = GameObject.Find("Canvas_Choose_Name_Fighter").GetComponent<Canvas>();
        chooseCountry = GameObject.Find("Canvas_Choose_Country").GetComponent<Canvas>();

        fighterLeftObject = GameObject.Find("Fighter_Left").GetComponent<Transform>();
        fighterMidObject = GameObject.Find("Fighter_Mid").GetComponent<Transform>();
        fighterRightObject = GameObject.Find("Fighter_Right").GetComponent<Transform>();
        fighterLeftRing = GameObject.Find("Ring_Left").GetComponent<Transform>();
        fighterMidRing = GameObject.Find("Ring_Mid").GetComponent<Transform>();
        fighterRightRing = GameObject.Find("Ring_Right").GetComponent<Transform>();
        fighterLeftSpecieTitle = GameObject.Find("Specie_Left").GetComponent<Transform>();
        fighterMidSpecieTitle = GameObject.Find("Specie_Mid").GetComponent<Transform>();
        fighterRightSpecieTitle = GameObject.Find("Specie_Right").GetComponent<Transform>();

        fighterLeftGroup = GameObject.Find("Bar_Level_Left").GetComponent<CanvasGroup>();
        fighterMidGroup = GameObject.Find("Bar_Level_Mid").GetComponent<CanvasGroup>();
        fighterRightGroup = GameObject.Find("Bar_Level_Right").GetComponent<CanvasGroup>();

        fighterLeftAnimator = GameObject.Find("Fighter_Left").GetComponent<SetFighterAnimations>();
        fighterMidAnimator = GameObject.Find("Fighter_Mid").GetComponent<SetFighterAnimations>();
        fighterRightAnimator = GameObject.Find("Fighter_Right").GetComponent<SetFighterAnimations>();

        chooseCountry = GameObject.Find("Canvas_Choose_Country").GetComponent<Canvas>();

        prev = GameObject.Find("Button_Prev").GetComponent<Button>();
        next = GameObject.Find("Button_Next").GetComponent<Button>();
        panelInfo = GameObject.Find("PanelControlTitle").GetComponent<TextMeshProUGUI>();

        fighterLeftDamageText = GameObject.Find("Attack_Value_Left").GetComponent<TextMeshProUGUI>();
        fighterLeftHpText = GameObject.Find("Life_Value_Left").GetComponent<TextMeshProUGUI>();
        fighterLeftSpeedText = GameObject.Find("Speed_Value_Left").GetComponent<TextMeshProUGUI>();
        fighterMidDamageText = GameObject.Find("Attack_Value_Mid").GetComponent<TextMeshProUGUI>();
        fighterMidHpText = GameObject.Find("Life_Value_Mid").GetComponent<TextMeshProUGUI>();
        fighterMidSpeedText = GameObject.Find("Speed_Value_Mid").GetComponent<TextMeshProUGUI>();
        fighterRightDamageText = GameObject.Find("Attack_Value_Right").GetComponent<TextMeshProUGUI>();
        fighterRightHpText = GameObject.Find("Life_Value_Right").GetComponent<TextMeshProUGUI>();
        fighterRightSpeedText = GameObject.Find("Speed_Value_Right").GetComponent<TextMeshProUGUI>();

        // setup stats
        SetDefaultStats(GameObject.Find("Fighter_Left").GetComponent<FighterSkinData>().species, "Fighter_Left");
        SetDefaultStats(GameObject.Find("Fighter_Mid").GetComponent<FighterSkinData>().species, "Fighter_Mid");
        SetDefaultStats(GameObject.Find("Fighter_Right").GetComponent<FighterSkinData>().species, "Fighter_Right");

        nameInputField = GameObject.Find("NameInputField").GetComponent<TMP_InputField>();
        regexText = GameObject.Find("RegexText").GetComponent<TextMeshProUGUI>();

        flagsContainer = GameObject.Find("Flags").GetComponent<Transform>();
        flags = new List<Transform>();
        flagErrorOk = GameObject.Find("FlagErrorBg");

        // Initial setup -----------------------------------------------------------------------------------------
        chooseName.enabled = false;
        chooseCountry.enabled = false;

        fighterLeftObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterMidObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterRightObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;

        fighterLeftGroup.alpha = initialAlphaUI;
        fighterMidGroup.alpha = initialAlphaUI;
        fighterRightGroup.alpha = initialAlphaUI;

        fighterLeftRing.gameObject.SetActive(false);
        fighterMidRing.gameObject.SetActive(false);
        fighterRightRing.gameObject.SetActive(false);
        fighterLeftSpecieTitle.gameObject.SetActive(false);
        fighterMidSpecieTitle.gameObject.SetActive(false);
        fighterRightSpecieTitle.gameObject.SetActive(false);

        prev.gameObject.SetActive(false);
        next.gameObject.SetActive(false);
            
        regexText.gameObject.SetActive(false);
        GetFlagGOs();
        flagErrorOk.SetActive(false);

        // set canvas state
        FirstPlayTempData.state = FirstPlayTempData.FirstPlayState.FIGHTER.ToString();
    }

    private IEnumerator Start()
    {
        StartCoroutine(SceneManagerScript.instance.FadeIn());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
    }

    private void SetDefaultStats(string specie, string fighter)
    {
        switch (fighter)
        {
            case "Fighter_Left":
                SetBaseStats(fighterLeftDamageText, fighterLeftHpText, fighterLeftSpeedText, specie);
                break;
            case "Fighter_Mid":
                SetBaseStats(fighterMidDamageText, fighterMidHpText, fighterMidSpeedText, specie);
                break;
            case "Fighter_Right":
                SetBaseStats(fighterRightDamageText, fighterRightHpText, fighterRightSpeedText, specie);
                break;
        }
    }

    private void SetBaseStats(TextMeshProUGUI damage, TextMeshProUGUI health, TextMeshProUGUI speed, string specie)
    {
        damage.text = Species.defaultStats[(SpeciesNames)Enum.Parse(typeof(SpeciesNames), specie)]["damage"].ToString();
        health.text = Species.defaultStats[(SpeciesNames)Enum.Parse(typeof(SpeciesNames), specie)]["hp"].ToString();
        speed.text = Species.defaultStats[(SpeciesNames)Enum.Parse(typeof(SpeciesNames), specie)]["speed"].ToString();
    }

    public void EnableFighterHighlight(string fighter)
    {
        switch (fighter)
        {
            case "left":
                fighterLeftGroup.alpha = visibleAlphaUI;
                fighterMidGroup.alpha = initialAlphaUI;
                fighterRightGroup.alpha = initialAlphaUI;
                fighterLeftAnimator.PlayRunAnimation();
                fighterMidAnimator.PlayIdleAnimation();
                fighterRightAnimator.PlayIdleAnimation();
                DisableMidFighterHighlight();
                DisableRightFighterHighlight();
                fighterLeftObject.gameObject.GetComponent<SpriteRenderer>().color = visibleColor;
                fighterLeftRing.gameObject.SetActive(true);
                fighterLeftSpecieTitle.gameObject.SetActive(true);
                break;
            case "mid":
                fighterLeftGroup.alpha = initialAlphaUI;
                fighterMidGroup.alpha = visibleAlphaUI;
                fighterRightGroup.alpha = initialAlphaUI;
                fighterLeftAnimator.PlayIdleAnimation();
                fighterMidAnimator.PlayRunAnimation();
                fighterRightAnimator.PlayIdleAnimation();
                DisableLeftFighterHighlight();
                DisableRightFighterHighlight();
                fighterMidObject.gameObject.GetComponent<SpriteRenderer>().color = visibleColor;
                fighterMidRing.gameObject.SetActive(true);
                fighterMidSpecieTitle.gameObject.SetActive(true);
                break;
            case "right":
                fighterLeftGroup.alpha = initialAlphaUI;
                fighterMidGroup.alpha = initialAlphaUI;
                fighterRightGroup.alpha = visibleAlphaUI;
                fighterLeftAnimator.PlayIdleAnimation();
                fighterMidAnimator.PlayIdleAnimation();
                fighterRightAnimator.PlayRunAnimation();
                DisableLeftFighterHighlight();
                DisableMidFighterHighlight();
                fighterRightObject.gameObject.GetComponent<SpriteRenderer>().color = visibleColor;
                fighterRightRing.gameObject.SetActive(true);
                fighterRightSpecieTitle.gameObject.SetActive(true);
                break;
        }

        next.gameObject.SetActive(true);
    }

    public void DisableLeftFighterHighlight()
    {
        fighterRightObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterMidObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterRightRing.gameObject.SetActive(false);
        fighterRightSpecieTitle.gameObject.SetActive(false);
        fighterMidRing.gameObject.SetActive(false);
        fighterMidSpecieTitle.gameObject.SetActive(false);
    }

    public void DisableMidFighterHighlight()
    {
        fighterLeftObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterRightObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterLeftRing.gameObject.SetActive(false);
        fighterLeftSpecieTitle.gameObject.SetActive(false);
        fighterRightRing.gameObject.SetActive(false);
        fighterRightSpecieTitle.gameObject.SetActive(false);
    }

    public void DisableRightFighterHighlight()
    {
        fighterLeftObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterMidObject.gameObject.GetComponent<SpriteRenderer>().color = shadowedColor;
        fighterLeftRing.gameObject.SetActive(false);
        fighterLeftSpecieTitle.gameObject.SetActive(false);
        fighterMidRing.gameObject.SetActive(false);
        fighterMidSpecieTitle.gameObject.SetActive(false);
    }

    public void OnExitNamePopUp()
    {
        GameObject.FindGameObjectWithTag("FighterNamePopup").GetComponent<Canvas>().enabled = false;
    }

    private void GetFlagGOs()
    {
        for(int i = 0; i < flagsContainer.childCount; i++)
            flags.Add(flagsContainer.GetChild(i));
    }

    // next button

    public void ChooseFighter()
    {
        chooseFighter.enabled = false;
        chooseName.enabled = true;
        prev.gameObject.SetActive(true);
        FirstPlayTempData.state = FirstPlayTempData.FirstPlayState.NAME.ToString();
        panelInfo.text = "Fighter Name";
    }

    public bool IsNameEmpty()
    {
        return nameInputField.text == "";
    }

    public bool IsNameLengthCorrect()
    {
        return nameInputField.text.Length < 4;
    }

    private void ShowError(string errorMessage)
    {
        regexText.gameObject.SetActive(true);
        regexText.text = errorMessage;
    }

    public void CheckName()
    {
        if (IsNameEmpty())
            ShowError("Name can't be empty!");
        else if (IsNameLengthCorrect())
            ShowError("Name must be at least 4 characters!");
        else
        {
            FirstPlayTempData.fighterName = nameInputField.text;
            FirstPlayTempData.state = FirstPlayTempData.FirstPlayState.COUNTRY.ToString();
            panelInfo.text = "Country";
            chooseCountry.enabled = true;
            chooseName.enabled = false;
        }
    }

    private bool IsFlagEmpty()
    {
        return FirstPlayTempData.countryFlag == null;
    }

    public void CheckFlag()
    {
        if (IsFlagEmpty())
            flagErrorOk.SetActive(true);
        else
        {
            CreateFighterFile();
            CreateUserFile();
            IGoToScene(SceneNames.EntryPoint);
        }
    }

    public void EnableCheckOnFlag(string flagName)
    {
        for(int i = 0; i < flags.Count; i++)
            if(flagName == flags[i].transform.name)
                flags[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    public void DisableCheckOnFlag(string flagName)
    {
        for (int i = 0; i < flags.Count; i++)
            if (flagName == flags[i].transform.name)
                flags[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    // previous button
    public void BackToChooseFighter()
    {
        chooseFighter.enabled = true;
        chooseName.enabled = false;
        prev.gameObject.SetActive(false);
        FirstPlayTempData.state = FirstPlayTempData.FirstPlayState.FIGHTER.ToString();
        panelInfo.text = "Fighter";
    }
    public void BackToName()
    {
        chooseName.enabled = true;
        chooseCountry.enabled = false;
        FirstPlayTempData.state = FirstPlayTempData.FirstPlayState.NAME.ToString();
        panelInfo.text = "Fighter Name";
    }

    // files creation
    private void CreateFighterFile()
    {
        SpeciesNames speciesEnumMember = GeneralUtils.StringToSpeciesNamesEnum(FirstPlayTempData.species);
        JObject serializableFighter = JObject.FromObject(JsonDataManager.CreateSerializableFighterInstance(FighterFactory.CreatePlayerFighterInstance(
            FirstPlayTempData.fighterName, FirstPlayTempData.skinName, FirstPlayTempData.species,
            Species.defaultStats[speciesEnumMember]["hp"],
            Species.defaultStats[speciesEnumMember]["damage"],
            Species.defaultStats[speciesEnumMember]["speed"],
            new List<Skill>())));
        JsonDataManager.SaveData(serializableFighter, JsonDataManager.FighterFileName);

        ResetAllPrefs();

        CreateLeaderboardPosition();
    }

    public void CreateUserFile()
    {
        bool firstTime = true;
        string flag = FirstPlayTempData.countryFlag;
        string userIcon = GenerateIcon().ToString();
        UserFactory.CreateUserInstance(firstTime, flag, userIcon, PlayerUtils.maxEnergy);
        JObject user = JObject.FromObject(User.Instance);
        JsonDataManager.SaveData(user, JsonDataManager.UserFileName);
    }

    private void CreateLeaderboardPosition()
    {
        PlayerPrefs.SetString("userRankingTimestamp", DateTime.Now.AddMinutes(MINUTES_BETWEEN_USER_RANKING_UPDATE).ToBinary().ToString());
        PlayerPrefs.SetInt("userRankingPosition", GenerateRandomRankingPosition());
        PlayerPrefs.SetInt("userCups", 0);
        PlayerPrefs.Save();
    }

    private int GenerateRandomRankingPosition()
    {
        return UnityEngine.Random.Range(RANKING_MIN_STARTING_POSITION, RANKING_MAX_STARTING_POSITION);
    }

    private int GenerateIcon()
    {
        return UnityEngine.Random.Range(0, Resources.LoadAll<Sprite>("Icons/UserIcons/").Length) + 1;
    }

    private void ResetAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    private IEnumerator GoToScene(SceneNames sceneName)
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName.ToString());
    }

    private void IGoToScene(SceneNames sceneName)
    {
        StartCoroutine(GoToScene(sceneName));
    }
}
