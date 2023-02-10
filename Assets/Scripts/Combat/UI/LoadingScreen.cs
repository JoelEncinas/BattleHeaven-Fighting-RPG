using UnityEngine;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject playerName;
    public GameObject botName;
    public GameObject botSprite;
    public GameObject botData;
    public GameObject botLevels;
    public GameObject spinner;

    public void SetPlayerLoadingScreenData(Fighter player)
    {
        playerName.GetComponent<TextMeshProUGUI>().text = player.fighterName;
        MenuUtils.DisplayLevelIcon(player.level, GameObject.Find("PlayerLoadingScreenLevels"));
    }

    public void DisplayLoaderForEnemy()
    {
        GetGameObjects();
        ToggleSpinnerAndBotData(true, true, true);
    }

    public void SetBotLoadingScreenData(Fighter bot)
    {
        botName.GetComponent<TextMeshProUGUI>().text = bot.fighterName;
        MenuUtils.DisplayLevelIcon(bot.level, botLevels);
        ToggleSpinnerAndBotData(true, false, false);
        StartCoroutine(FadeBotGradually());
    }

    IEnumerator FadeBotGradually()
    {
        // 2f is the duration of spinner animation
        // change it according to that to sync
        float color = 0;
        float colorIncrement = 12.75f;
        float time = 0f;
        float increment = 0.1f;

        do
        {
            botSprite.GetComponent<SpriteRenderer>().color = new Color32((byte)color, (byte)color, (byte)color, 255);
            color += colorIncrement;
            time += increment;
            yield return new WaitForSeconds(increment);
        }
        while (time < 2f);
    }

    public void ToggleSpinnerAndBotData(bool showBot, bool showSpinner, bool shadow)
    {
        botSprite.SetActive(showBot);
        if (shadow)
            botSprite.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
        else
            botSprite.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        botData.SetActive(showBot);
        spinner.SetActive(showSpinner);
    }

    private void GetGameObjects()
    {
        botSprite = GameObject.FindGameObjectWithTag("LoadingScreenBot");
        botData = GameObject.FindGameObjectWithTag("CombatLoadingScreenBotData");
        botLevels = GameObject.Find("BotLoadingScreenLevels");
        spinner = GameObject.FindGameObjectWithTag("CombatLoadingScreenSpinner");
    }

    public void HideBotLevelText(TextMeshProUGUI levelTextBot)
    {
        levelTextBot.GetComponent<TextMeshProUGUI>().enabled = false;
    }
}