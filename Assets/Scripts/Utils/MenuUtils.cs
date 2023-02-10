using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuUtils
{
    public static void SetName(GameObject playerNameGO, string fighterName)
    {
        playerNameGO.GetComponent<TextMeshProUGUI>().text = fighterName;
    }
    public static void SetLevelSlider(GameObject playerExpGO, GameObject playerLevelSliderGO, int playerLevel, int playerExp)
    {
        int maxExp = Levels.MaxXpOfCurrentLevel(playerLevel);
        playerExpGO.GetComponent<TextMeshProUGUI>().text = $"{playerExp.ToString()}/{maxExp}";
        playerLevelSliderGO.GetComponent<Slider>().value = (float)playerExp / (float)maxExp;
    }
    public static void SetGold(GameObject goldGO)
    {
        goldGO.GetComponent<TextMeshProUGUI>().text = User.Instance.gold.ToString();
    }

    public static void SetGems(GameObject gemsGO)
    {
        gemsGO.GetComponent<TextMeshProUGUI>().text = User.Instance.gems.ToString();
    }
    public static void ShowElo(GameObject playerEloGO)
    {
        playerEloGO.GetComponent<TextMeshProUGUI>().text = User.Instance.elo.ToString();
    }

    public static void SetEnergy(GameObject energyGO)
    {
        energyGO.GetComponent<TextMeshProUGUI>().text = $"{User.Instance.energy.ToString()}/{PlayerUtils.maxEnergy}";
    }
    public static void DisplayEnergyCountdown(GameObject timerContainerGO, GameObject timerGO)
    {
        int energyRefreshTimeInHours = EnergyManager.defaultEnergyRefreshTimeInMinutes / 60;
        var timeSinceCountdownStart = EnergyManager.GetTimeSinceCountdownStart();
  
        // int hoursPassedOfCurrentCountdown = Math.Abs(timeSinceCountdownStart.Hours % energyRefreshTimeInHours);
        // string hoursUntilCountdownEnd = (energyRefreshTimeInHours - hoursPassedOfCurrentCountdown - 1).ToString();

        string minutesUntilCountdownEnd = (60 - timeSinceCountdownStart.Minutes - 1).ToString();
        string secondsUntilCountdownEnd = (60 - timeSinceCountdownStart.Seconds - 1).ToString();

        timerContainerGO.SetActive(true);
        timerGO.GetComponent<TextMeshProUGUI>().text = $"{minutesUntilCountdownEnd}m {secondsUntilCountdownEnd}s";
        return;
    }

    public static void DisplayLevelIcon(int fighterLevel, GameObject iconsContainer)
    {
        Image icon = GetLevelIconImage(fighterLevel, iconsContainer);
        SetLevelIcon(fighterLevel, icon);
        HidePreviousIconImage(GetIconNumber(fighterLevel));
    }

    private static void SetLevelIcon(int fighterLevel, Image iconGO)
    {
        iconGO.enabled = true;
        iconGO.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = fighterLevel.ToString();
    }

    private static Image GetLevelIconImage(int fighterLevel, GameObject iconsContainer)
    {
        double nIcon = GetIconNumber(fighterLevel);
        return iconsContainer.transform.Find($"Level_Icon_{nIcon}").GetComponent<Image>();
    }

    private static double GetIconNumber(int fighterLevel)
    {
        float iconNumber = Mathf.Ceil(fighterLevel / Levels.levelsUntilIconUpgrade);
        return iconNumber <= Levels.totalIconsForLevels ? iconNumber : Levels.totalIconsForLevels;
    }

    private static void HidePreviousIconImage(double nIcon)
    {
        if (GameObject.Find($"Level_Icon_{nIcon - 1}")) GameObject.Find($"Level_Icon_{nIcon - 1}").GetComponent<Image>().enabled = false;
    }

    public static void SetFighterStats(TextMeshProUGUI attack, TextMeshProUGUI hp, TextMeshProUGUI speed)
    {
        Fighter player = PlayerUtils.FindInactiveFighter();
        attack.text = player.damage.ToString();
        hp.text = player.hp.ToString();
        speed.text = player.speed.ToString();
    }

    public static void SetProfilePicture(GameObject pictureGO)
    {
        Fighter player = PlayerUtils.FindInactiveFighter();
        pictureGO.GetComponent<Image>().sprite = MenuUtils.GetProfilePicture(player.species);
    }

    public static Sprite GetProfilePicture(string specie)
    {
        List<Sprite> profilePictures = new List<Sprite>(Resources.LoadAll<Sprite>("CharacterProfilePicture/"));

        foreach (Sprite picture in profilePictures)
        {
            if (picture.name.Contains(specie))
                return picture;
        }

        return null;
    }

    public static void SetProfileUserIcon(GameObject iconGO)
    {
        iconGO.GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/UserIcons/" + User.Instance.userIcon);
    }
}