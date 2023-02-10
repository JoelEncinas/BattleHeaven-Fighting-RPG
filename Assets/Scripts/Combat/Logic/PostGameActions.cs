using System;

public class PostGameActions
{
    const int MIN_GOLD = 100;
    const int MAX_GOLD = 150;
    const int MIN_GEMS = 10;
    const int MAX_GEMS = 30;

    public static void SetElo(int eloChange)
    {
        User.Instance.elo += eloChange;
    }

    //Functional Pattern. Func<ParameterType, ReturnType>
    public static Func<Fighter, bool> HasPlayerWon = player => player.hp > 0 ? true : false;

    public static void SetExperience(Fighter player, bool isPlayerWinner)
    {
        player.experiencePoints += Levels.GetXpGain(isPlayerWinner);
    }
    public static void SetLevelUpSideEffects(Fighter player)
    {
        Levels.ResetExperience(player);
        Levels.UpgradeStats(player);
        Levels.SetLevel(player);
    }

    public static void SetWinLoseCounter(bool isPlayerWinner)
    {
        if (isPlayerWinner) User.Instance.wins++;
        else User.Instance.loses++;
    }

    public static void SetCurrencies(int goldReward, int gemsReward)
    {
        CurrencyHandler.instance.AddGold(goldReward);
        CurrencyHandler.instance.AddGems(gemsReward);
    }
    public static Action<Fighter> Save = (player) => player.SaveFighter();

    public static int GoldReward(bool isPlayerWinner)
    {
        return isPlayerWinner ? MIN_GOLD : MAX_GOLD;
    }

    public static int GemsReward()
    {
        return Probabilities.IsHappening(10) ? UnityEngine.Random.Range(MIN_GEMS, MAX_GEMS) : 0;
    }
}