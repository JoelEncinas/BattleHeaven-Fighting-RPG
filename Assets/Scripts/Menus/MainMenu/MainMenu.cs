using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // UI
    public GameObject playerEloGO;
    public GameObject battleButtonGO;
    public GameObject cardsButtonGO;
    public GameObject playerExpGO;
    public GameObject playerLevelSlider;
    public GameObject notifyCards;
    public GameObject settings;
    public GameObject deleteConfirmation;
    public GameObject aboutPopup;
    public GameObject buttonDelete;
    public GameObject buttonAbout;
    public GameObject buttonCredits;
    public GameObject buttonCloseConfirmation;
    public GameObject buttonCloseAbout;
    public GameObject battleEnergyIcon;
    public GameObject battleSwordIcon;
    Image lockIcon;

    // stats
    public TextMeshProUGUI attack;
    public TextMeshProUGUI hp;
    public TextMeshProUGUI speed;

    // notifications
    public TextMeshProUGUI notifyCardsTxt;

    // daily gift
    GameObject dailyGiftCanvas;
    DailyGift dailyGift;
    GameObject dailyGiftsNotification;

    // ranking 
    GameObject rankingCanvas;

    // profile
    GameObject profileCanvas;

    void Awake()
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        AudioSource audioSource = audioManager.GetComponent<AudioSource>();
        audioManager.Stop("V_Combat_Theme");
        //If Main Menu theme is already playing keep playing it
        if (!audioSource.isPlaying) audioSource.Play();

        settings = GameObject.Find("Settings");
        deleteConfirmation = GameObject.Find("Delete_Confirmation");
        aboutPopup = GameObject.Find("AboutPopup");
        dailyGiftCanvas = GameObject.Find("DailyRewardsCanvas");
        rankingCanvas = GameObject.Find("RankingCanvas");
        profileCanvas = GameObject.Find("ProfileCanvas");
        dailyGift = dailyGiftCanvas.GetComponent<DailyGift>();
        dailyGiftsNotification = GameObject.Find("DailyGiftsNotification");
        buttonDelete = GameObject.Find("Button_Delete");
        buttonAbout = GameObject.Find("Button_About");
        buttonCredits = GameObject.Find("Button_Credits");
        buttonCloseConfirmation = GameObject.Find("Button_Close_Confirmation");
        buttonCloseAbout = GameObject.Find("Button_Close_About");
        lockIcon = GameObject.Find("Battle_Lock").GetComponent<Image>();

        // stats
        attack = GameObject.Find("Attack_Value").GetComponent<TextMeshProUGUI>();
        hp = GameObject.Find("Hp_Value").GetComponent<TextMeshProUGUI>();
        speed = GameObject.Find("Speed_Value").GetComponent<TextMeshProUGUI>();

        Fighter player = PlayerUtils.FindInactiveFighter();
        PlayerUtils.FindInactiveFighterGameObject().SetActive(false);
        MenuUtils.ShowElo(playerEloGO);
        MenuUtils.SetLevelSlider(playerExpGO, playerLevelSlider, player.level, player.experiencePoints);
        MenuUtils.DisplayLevelIcon(player.level, GameObject.Find("Levels"));
        MenuUtils.SetFighterStats(attack, hp, speed);
        cardsButtonGO.GetComponent<Button>().interactable = player.skills.Count > 0;
        notifyCardsTxt = notifyCards.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        SetBattleButton();

        // Hide poppups
        settings.SetActive(false);
        deleteConfirmation.SetActive(false);
        aboutPopup.SetActive(false);
        dailyGiftCanvas.SetActive(false);
        rankingCanvas.SetActive(false);
        profileCanvas.SetActive(false);
        dailyGiftsNotification.SetActive(false);
        if (dailyGift.IsFirstTime())
            dailyGiftsNotification.SetActive(true);

        buttonDelete.GetComponent<Button>().onClick.AddListener(() => ShowDeleteConfirmation());
        buttonCloseConfirmation.GetComponent<Button>().onClick.AddListener(() => CloseSettingsConfirmation());
        buttonAbout.GetComponent<Button>().onClick.AddListener(() => ShowAboutPopup());
        buttonCloseAbout.GetComponent<Button>().onClick.AddListener(() => HideAboutPopup());
        buttonCredits.GetComponent<Button>().onClick.AddListener(() => IShowCredits());
    }

    public void SetBattleButton()
    {
        bool userHasEnergy = User.Instance.energy > 0;
        battleButtonGO.GetComponent<Button>().enabled = userHasEnergy;
        lockIcon.enabled = !userHasEnergy;
        battleEnergyIcon.SetActive(!userHasEnergy);
        battleSwordIcon.SetActive(userHasEnergy);
    }
    IEnumerator Start()
    {
        if (SceneFlag.sceneName == SceneNames.EntryPoint.ToString() ||
            SceneFlag.sceneName == SceneNames.Combat.ToString() ||
            SceneFlag.sceneName == SceneNames.LevelUp.ToString() ||
                SceneFlag.sceneName == SceneNames.Credits.ToString())
        {
            StartCoroutine(SceneManagerScript.instance.FadeIn());
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        }

        battleButtonGO.GetComponent<Button>().enabled = User.Instance.energy > 0;

        StartCoroutine(RefreshItems());

        SceneFlag.sceneName = SceneNames.MainMenu.ToString();
    }

    IEnumerator RefreshItems()
    {
        do
        {
            if (dailyGift.IsGiftAvailable() || dailyGift.IsFirstTime())
                dailyGiftsNotification.SetActive(true);

            // Notifications
            if (Notifications.isInventoryNotificationsOn)
            {
                notifyCards.SetActive(true);
                notifyCardsTxt.text = Notifications.cardsUnseen.ToString();
            }
            else
                notifyCards.SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }

        while (true);
    }

    // on settings button
    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }

    public void ShowDeleteConfirmation()
    {
        deleteConfirmation.SetActive(true);
    }

    public void ShowAboutPopup()
    {
        aboutPopup.SetActive(true);
    }

    public void HideAboutPopup()
    {
        aboutPopup.SetActive(false);
    }

    public void IShowCredits()
    {
        StartCoroutine(ShowCredits());
    }

    public IEnumerator ShowCredits()
    {
        StartCoroutine(SceneManagerScript.instance.FadeOut());
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SceneFlag.FADE_DURATION));
        SceneManager.LoadScene(SceneNames.Credits.ToString());
    }

    public void CloseSettingsConfirmation()
    {
        deleteConfirmation.SetActive(false);
    }

    public void EnableDailyGiftNotification()
    {
        dailyGiftsNotification.SetActive(true);
        dailyGiftsNotification.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "1";
    }

    public void DisableDailyGiftNotification()
    {
        dailyGiftsNotification.SetActive(false);
    }
}
