using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FightersUIData : MonoBehaviour
{
    public GameObject playerNameGO;
    public GameObject playerEloGO;
    public GameObject playerHealthBarGO;
    public GameObject botEloGO;
    public GameObject botNameGO;
    public GameObject botHealthBarGO;
    private float playerMaxHealth;
    private float botMaxHealth;
    public GameObject resultsEloChange;
    public GameObject playerLevelGO;
    public GameObject playerExpGO;
    public GameObject playerLevelSliderGO;
    public GameObject playerExpGainTextGO;
    public GameObject levelUpIcon;
    public GameObject victoryBanner;
    public GameObject defeatBanner;
    public GameObject goldRewardGO;
    public GameObject gemsRewardGO;
    public GameObject chestRewardGO;
    public GameObject nextButtonGO;
    public GameObject countdownGO;

    // tutorial
    public GameObject nextTutorialButtonGO;

    // health bar animations
    public GameObject playerHealthBarFadeGO;
    public GameObject botHealthBarFadeGO;
    private float previousPlayerHp = 1f;
    private float previousBotHp = 1f;

    // usericons
    public GameObject playerIcon;
    public GameObject botIcon;

    private void AddListenerToNextBtn(bool isLevelUp) {
        nextButtonGO.GetComponent<Button>().onClick.AddListener(() => OnClickNextHandler(isLevelUp));
    }

    private void AddListenerToNextTutorialBtn()
    {
        nextTutorialButtonGO.GetComponent<Button>().onClick.AddListener(() => OnClickNextTutorialHandler());
    }

    private void OnClickNextHandler(bool isLevelUp){
        if(isLevelUp)
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneNames.LevelUp.ToString());
        else
        {
            if(!CombatMode.isSoloqEnabled)
                IGoToScene(SceneNames.Cup);
            else
                IGoToScene(SceneNames.MainMenu);
        }
    }

    private void OnClickNextTutorialHandler()
    {
        User.Instance.firstTime = false;
        IGoToScene(SceneNames.MainMenu);
    }

    public void ShowPostCombatInfo(Fighter player, bool isPlayerWinner, int eloChange, bool isLevelUp, int goldReward, int gemsReward, GameObject results)
    {
        EnableResults(results);
        AddListenerToNextBtn(isLevelUp);
        SetResultsBanner(isPlayerWinner);
        SetResultsEloChange(eloChange);
        SetResultsLevel(player.level, player.experiencePoints);
        SetResultsExpGainText(isPlayerWinner);
        ShowLevelUpIcon(isLevelUp);
        ShowRewards(goldReward, gemsReward);
    }

    public void ShowPostTutorialInfo(GameObject tutorialResults)
    {
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.Stop("V_Combat_Theme");
        audioManager.Play("S_Reward_Received");
        ShowTutorialMessage(tutorialResults);
        AddListenerToNextTutorialBtn();
    }

    public void ShowTutorialMessage(GameObject tutorialResults)
    {
        tutorialResults.SetActive(true);
    }

    public void SetFightersUIInfo(Fighter player, Fighter bot, int botElo)
    {
        SetFightersName(player.fighterName, bot.fighterName);
        SetFightersMaxHealth(player.hp, bot.hp);
        SetPlayerIcons(playerIcon, botIcon);
    }

    private void SetFightersName(string playerName, string botName)
    {
        playerNameGO.GetComponent<TextMeshProUGUI>().text = playerName;
        botNameGO.GetComponent<TextMeshProUGUI>().text = botName;
    }

    public void SetFightersMaxHealth(float playerMaxHealth, float botMaxHealth)
    {
        this.playerMaxHealth = playerMaxHealth;
        this.botMaxHealth = botMaxHealth;
    }

    private void SetPlayerIcons(GameObject playerIcon, GameObject botIcon)
    {
        playerIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/UserIcons/" + User.Instance.userIcon);
        botIcon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/UserIcons/" + (UnityEngine.Random.Range(0, Resources.LoadAll<Sprite>("Icons/UserIcons/").Length) + 1));
    }

    public IEnumerator Countdown()
    {
        const float TIME_BETWEEN_COUNTDOWN = 1f;
        TextMeshProUGUI countdownText = countdownGO.GetComponent<TextMeshProUGUI>();
        countdownGO.GetComponent<Animator>().enabled = true;

        countdownText.enabled = true;

        countdownText.text = "3";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_BETWEEN_COUNTDOWN));

        countdownText.text = "2";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_BETWEEN_COUNTDOWN));

        countdownText.text = "1";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_BETWEEN_COUNTDOWN));

        countdownGO.GetComponent<Animator>().enabled = false;
        countdownGO.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);

        countdownText.text = "FIGHT!";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_BETWEEN_COUNTDOWN));

        countdownText.enabled = false;
    }

    public IEnumerator AnnounceWinner(bool isPlayerWinner, Fighter player, Fighter bot)
    {
        const float TIME_ANNOUNCEMENT = 2f;
        TextMeshProUGUI countdownText = countdownGO.GetComponent<TextMeshProUGUI>();

        countdownText.enabled = true;

        if (isPlayerWinner)
            countdownText.text = player.fighterName + "<br>WINS!";
        else
            countdownText.text = bot.fighterName + "<br>WINS!";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(TIME_ANNOUNCEMENT));

        countdownText.enabled = false;
    }

    public void ModifyHealthBar(Fighter fighter)
    {
        if (Combat.player == fighter)
        {
            SetHealthBarValue(playerHealthBarFadeGO, playerHealthBarGO, fighter, playerMaxHealth);
            return;
        }

        SetHealthBarValue(botHealthBarFadeGO, botHealthBarGO, fighter, botMaxHealth);
    }

    private void SetHealthBarValue(GameObject healthBarFade, GameObject healthBar, Fighter fighter, float maxHealth)
    {
        StartCoroutine(HealthAnimation(Combat.player == fighter, fighter.hp, maxHealth, healthBarFade));
        healthBar.GetComponent<Slider>().value = fighter.hp / maxHealth;
    }

    IEnumerator HealthAnimation(bool isPlayer, float health, float maxHealth, GameObject healthBarFade)
    {
        const int ANIMATION_FRAMES = 10;
        const float SECONDS_PER_FRAME = 0.075f;
        Slider healthBarFadeSliderValue = healthBarFade.GetComponent<Slider>();

        float newHp = health / maxHealth;

        if(isPlayer)
        {
            if (newHp > 0 && !Combat.isGameOver)
            {
                if(newHp > previousPlayerHp)
                {
                    healthBarFadeSliderValue.value = newHp;
                }
                else
                {
                    do
                    {
                        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SECONDS_PER_FRAME));
                        healthBarFadeSliderValue.value -= (previousPlayerHp - newHp) / ANIMATION_FRAMES;
                    } while (healthBarFadeSliderValue.value >= newHp);
                }

                previousPlayerHp = newHp;
            }
            else if (newHp == maxHealth && !Combat.isGameOver)
            {
                // don't do anything
            }
            else
            {
                double noHp = -0.5;

                do
                {
                    yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SECONDS_PER_FRAME));
                    healthBarFadeSliderValue.value -= (previousPlayerHp - newHp) / ANIMATION_FRAMES;
                } while (healthBarFadeSliderValue.value >= noHp);
            }
        } 
        else
        {
            if (newHp > 0 && !Combat.isGameOver)
            {
                if(newHp > previousBotHp)
                {
                    healthBarFadeSliderValue.value = newHp;
                }
                else
                {
                    do
                    {
                        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SECONDS_PER_FRAME));
                        healthBarFadeSliderValue.value -= (previousBotHp - newHp) / ANIMATION_FRAMES;
                    } while (healthBarFadeSliderValue.value >= newHp);
                }

                previousBotHp = newHp;
            }
            else if (newHp == maxHealth && !Combat.isGameOver)
            {
                // don't do anything
            }
            else
            {
                double noHp = -0.5;

                do
                {
                    yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(SECONDS_PER_FRAME));
                    healthBarFadeSliderValue.value -= (previousBotHp - newHp) / ANIMATION_FRAMES;
                } while (healthBarFadeSliderValue.value >= noHp);
            }
        }
    }

    public void SetResultsEloChange(int eloChange)
    {
        resultsEloChange.GetComponent<TextMeshProUGUI>().text = eloChange > 0 ? $"+{eloChange.ToString()}" : eloChange.ToString();
    }
    public void SetResultsLevel(int playerLevel, int playerExp)
    {
        MenuUtils.SetLevelSlider(playerExpGO, playerLevelSliderGO, playerLevel, playerExp);
        MenuUtils.DisplayLevelIcon(playerLevel, GameObject.FindGameObjectWithTag("ResultsLevelIcons"));
    }
    public void SetResultsExpGainText(bool isPlayerWinner)
    {
        playerExpGainTextGO.GetComponent<TextMeshProUGUI>().text = $"+{Levels.GetXpGain(isPlayerWinner).ToString()}";
    }
    public void ShowLevelUpIcon(bool isLevelUp)
    {
        levelUpIcon.SetActive(isLevelUp);
    }

    public void EnableResults(GameObject results)
    {
        results.SetActive(true);
    }
    public void SetResultsBanner(bool isPlayerWinner)
    {
        victoryBanner.SetActive(isPlayerWinner);
        defeatBanner.SetActive(!isPlayerWinner);
    }

    public void ShowRewards(int goldReward, int gemsReward)
    {
        gemsRewardGO.SetActive(Convert.ToBoolean(gemsReward));
        goldRewardGO.transform.Find("TextValue").gameObject.GetComponent<TextMeshProUGUI>().text = goldReward.ToString();
        if (Convert.ToBoolean(gemsReward)) gemsRewardGO.transform.Find("TextValue").gameObject.GetComponent<TextMeshProUGUI>().text = gemsReward.ToString();
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
