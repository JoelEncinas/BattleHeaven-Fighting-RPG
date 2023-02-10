using UnityEngine;

public static class Notifications
{
    public static int cardsUnseen;
    public static bool isInventoryNotificationsOn = false;

    public static void TurnOnNotification()
    {
        isInventoryNotificationsOn = true;
    }

    public static void TurnOffNotification()
    {
        isInventoryNotificationsOn = false;
    }

    public static void InitiateCardsUnseen()
    {
        cardsUnseen = PlayerPrefs.GetInt("cardsUnseen");
    }

    public static void IncreaseCardsUnseen()
    {
        cardsUnseen = PlayerPrefs.GetInt("cardsUnseen") + 1;
        PlayerPrefs.SetInt("cardsUnseen", cardsUnseen);
        PlayerPrefs.Save();
    }

    public static void ResetCardsUnseen()
    {
        cardsUnseen = 0;
        PlayerPrefs.SetInt("cardsUnseen", 0);
        PlayerPrefs.Save();
    }
}

