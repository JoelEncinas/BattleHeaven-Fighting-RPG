using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserFactory
{
    public static void CreateUserInstance(bool firstTime, string flag, string userIcon, int energy, int wins = 0, int loses = 0, int elo = 0, int peakElo = 0, int gold = 0, int gems = 0, int cups = 0)
    {
        User user = User.Instance;
        user.SetUserValues(firstTime, flag, userIcon, wins, loses, elo, peakElo, gold, gems, energy, cups);
        user.EnableSave();
    }
}
