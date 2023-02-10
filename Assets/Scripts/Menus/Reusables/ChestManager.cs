using System.Collections.Generic;
using UnityEngine;

public static class ChestManager
{
    const float TOTALWEIGHT = 100f;

    public static Chest.BattleChestRarities GetRandomBattleChestRarity()
    {
        float diceRoll = Random.Range(0f, TOTALWEIGHT);

        foreach (KeyValuePair<Chest.BattleChestRarities, float> chest in
            Chest.battleChestsProbabilities)
        {
            if (chest.Value >= diceRoll)
                return chest.Key;

            diceRoll -= chest.Value;
        }

        Debug.Log("ERROR. Something went wrong when getting a random chest");
        return Chest.BattleChestRarities.COMMON;
    }
}
