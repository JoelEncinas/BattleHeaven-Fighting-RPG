using UnityEngine;
using System;
public static class EnergyManager
{
    public static int defaultEnergyRefreshTimeInMinutes = 60;
    public static void SubtractOneEnergyPoint()
    {
        if (User.Instance.energy == PlayerUtils.maxEnergy) StartCountdown();
        if (User.Instance.energy == 0) Debug.LogError("ERROR, THE USER ENERGY IS ALREADY 0!");
        User.Instance.energy--;
    }

    public static void StartCountdown()
    {
        //Save the countdownStartTime to player prefs so they can be recovered after closing the game
        PlayerPrefs.SetString("countdownStartTime", DateTime.Now.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public static bool UserHasMaxEnergy()
    {
        return User.Instance.energy == PlayerUtils.maxEnergy;
    }

    public static long GetCountDownStartTime()
    {
        bool countdownPlayerPrefExists = PlayerPrefs.GetString("countdownStartTime") != "";
        return countdownPlayerPrefExists ? Convert.ToInt64(PlayerPrefs.GetString("countdownStartTime")) : 0;
    }

    public static TimeSpan GetTimeSinceCountdownStart()
    {
        return DateTime.Now - DateTime.FromBinary(GetCountDownStartTime());
    }

    public static void RefreshEnergyBasedOnCountdown()
    {
        if (UserHasMaxEnergy()) return;
        
        int minutesSinceCountdownStart = GetTimeSinceCountdownStart().Hours * 60 + GetTimeSinceCountdownStart().Minutes;
        
        //Refresh between 0 and N energy. 
        int energyToAdd = (int)Mathf.Floor(minutesSinceCountdownStart / defaultEnergyRefreshTimeInMinutes);

        if(energyToAdd > 0){
            int updatedEnergy = User.Instance.energy + energyToAdd;
            User.Instance.energy = updatedEnergy > PlayerUtils.maxEnergy ? PlayerUtils.maxEnergy : updatedEnergy;
            UpdateCountdown();
        }
    }

    public static void UpdateCountdown()
    {
        double minutesPassed = GetTimeSinceCountdownStart().TotalMinutes;
        double minutesLeftOnCurrentCountdown = minutesPassed % defaultEnergyRefreshTimeInMinutes;
        double minutesToAddToPreviousCountdown = minutesPassed - minutesLeftOnCurrentCountdown;

        DateTime newCountdown = DateTime.FromBinary(GetCountDownStartTime()).AddMinutes(minutesToAddToPreviousCountdown);
        PlayerPrefs.SetString("countdownStartTime", newCountdown.ToBinary().ToString());
        PlayerPrefs.Save();
    }

    public static void DebugCountdownHelper(){
        Debug.Log("GetTimeSinceCountdownStart");
        Debug.Log(GetTimeSinceCountdownStart());
        Debug.Log("Minutes");
        Debug.Log(GetTimeSinceCountdownStart().Minutes);
        Debug.Log("Seconds");
        Debug.Log(GetTimeSinceCountdownStart().Seconds);
        Debug.Log("Energies refreshed");
        Debug.Log((int)Mathf.Floor(GetTimeSinceCountdownStart().Minutes / defaultEnergyRefreshTimeInMinutes));
    }
}