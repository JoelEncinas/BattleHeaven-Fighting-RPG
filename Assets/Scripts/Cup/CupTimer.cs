using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CupTimer : MonoBehaviour
{
    // UI
    GameObject buttonCup;
    Image cupIcon;
    TextMeshProUGUI textCup;
    TextMeshProUGUI textCupDisabled;
    GameObject timerGO;
    TextMeshProUGUI textTimer;
    Image lockIcon;

    const int CUP_COOLDOWN_IN_DAYS = 5; 

    private void Awake()
    {
        SetupUI();
        SetupCup();
    }

    private void SetupUI()
    {
        buttonCup = GameObject.Find("Button_Cup");
        cupIcon = GameObject.Find("Icon_Cup").GetComponent<Image>();
        textCup = GameObject.Find("Text_Cup").GetComponent<TextMeshProUGUI>();
        textCupDisabled = GameObject.Find("Text_Cup_Disabled").GetComponent<TextMeshProUGUI>();
        timerGO = GameObject.Find("Icon_Daily_Time_Cup");
        textTimer = GameObject.Find("Text_Daily_Time").GetComponent<TextMeshProUGUI>();
        lockIcon = GameObject.Find("Cup_Lock").GetComponent<Image>();
    }

    private void Update()
    {
        SetupCup();
    }

    private void SetupCup()
    {
        if (IsCupAvailable())
            EnableCup();
        if (UpdateTimer() >= TimeSpan.Zero)
        {
            DisableCup();
            timerGO.SetActive(true);
            TimeSpan timer = UpdateTimer();
            textTimer.text = $"{timer.Days}d {timer.Hours}h {timer.Minutes}m";
        }
    }

    private void EnableCup()
    {
        buttonCup.GetComponent<Button>().enabled = true;
        cupIcon.enabled = true;
        lockIcon.enabled = false;
        textCup.enabled = true;
        textCupDisabled.enabled = false;
        timerGO.SetActive(false);
    }

    private void DisableCup()
    {
        buttonCup.GetComponent<Button>().enabled = false;
        cupIcon.enabled = false;
        lockIcon.enabled = true;
        textCup.enabled = false;
        textCupDisabled.enabled = true;
        timerGO.SetActive(true);
    }

    public bool IsCupAvailable()
    {
        if (PlayerPrefs.GetFloat("firstCup") == 0)
        {
            PlayerPrefs.SetFloat("firstCup", 1);
            PlayerPrefs.Save();
            StartCountdown();
            return false;
        }
        else if (PlayerPrefs.GetString("cupCountdown") != "")
        {
            return DateTime.Compare(DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("cupCountdown"))), DateTime.Now) <= 0;
        }
            
        return false;
    }

    private void StartCountdown()
    {
        PlayerPrefs.SetString("cupCountdown", DateTime.Now.AddDays(CUP_COOLDOWN_IN_DAYS).ToBinary().ToString());
        PlayerPrefs.Save();
    }

    private TimeSpan UpdateTimer()
    {
        if (PlayerPrefs.GetString("cupCountdown") != "")
            return DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("cupCountdown"))) - DateTime.Now;
        return TimeSpan.Zero;
    }
}
