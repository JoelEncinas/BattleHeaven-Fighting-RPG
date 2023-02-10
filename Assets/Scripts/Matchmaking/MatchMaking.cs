using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using UnityEngine;

public static class MatchMaking
{
    private static readonly int baseEloGain = 15;
    public static CupFighter bot; // use for cup mode

    public static void GenerateCupBotData(Fighter player, Fighter bot)
    {
        CupFighter cupBot = GetCupBotData();
        string botName = cupBot.fighterName;
        SpeciesNames botSpecies = (SpeciesNames)Enum.Parse(typeof(SpeciesNames), cupBot.species);

        GenerateBotData(player, bot, botName, botSpecies);
    }

    public static void GenerateSoloQBotData(Fighter player, Fighter bot)
    {
        string botName = "";

        if(User.Instance.firstTime)
        {
            botName = "Tutorial Bot";
        } 
        else
        {
            botName = FetchBotRandomName();
        }
        SpeciesNames randomSpecies = GetRandomSpecies();

        GenerateBotData(player, bot, botName, randomSpecies);
    }

    public static void GenerateBotData(Fighter player, Fighter bot, string botName, SpeciesNames botSpecies)
    {
        int botLevel = GenerateBotLevel(player.level);
        Combat.botElo = GenerateBotElo(User.Instance.elo);

        List<Skill> botSkills = GenerateBotSkills(player);

        Dictionary<string, float> botStats = GenerateBotRandomStats(botSpecies);

        //WeightedHealth to give the player a little advantadge. We probably don't need it as the actives make the difference.
        //float weightedHealth = (float)Math.Round(UnityEngine.Random.Range(botStats["hp"] * 0.93f, botStats["hp"] * 1.04f));

        bot.FighterConstructor(botName, botStats["hp"], botStats["damage"], botStats["speed"],
            botSpecies.ToString(), botSpecies.ToString(), botLevel, 0, botSkills);
    }

    private static List<Skill> GenerateBotSkills(Fighter player)
    {
        List<Skill> botSkills = new List<Skill>();

        int skillCountBottomRange;
        int skillCountTopRange;

        //If player is lvl 0 no one has skills
        if (player.skills.Count == 0)
        {
            skillCountBottomRange = 0;
            skillCountTopRange = 0;
        }
        else
        {
            skillCountBottomRange = player.skills.Count - 1;
            skillCountTopRange = player.skills.Count;
        }

        //Add random skills for the bot, -1 to +0 skills relative to the player
        int botSkillsCount = UnityEngine.Random.Range(skillCountBottomRange, skillCountTopRange + 1);
        int randomSkillIndex = UnityEngine.Random.Range(0, SkillCollection.skills.Count);

        for (int i = 0; i < botSkillsCount; i++)
        {
            OrderedDictionary skill = SkillCollection.skills[randomSkillIndex];
            Skill skillInstance = new Skill(skill["name"].ToString(), skill["description"].ToString(),
                skill["skillRarity"].ToString(), skill["category"].ToString(), skill["icon"].ToString());

            botSkills.Add(skillInstance);
        }

        return botSkills;
    }

    private static CupFighter GetCupBotData()
    {
        string cupBotId = "";
        int counter = 0;

        // player enemies will be on seed2, seed10, seed14
        if (Cup.Instance.round == CupDB.CupRounds.QUARTERS.ToString())
            cupBotId = Cup.Instance.cupInfo[CupDB.CupRounds.QUARTERS.ToString()]["1"]["2"];
        if (Cup.Instance.round == CupDB.CupRounds.SEMIS.ToString())
            cupBotId = Cup.Instance.cupInfo[CupDB.CupRounds.SEMIS.ToString()]["5"]["10"];
        if (Cup.Instance.round == CupDB.CupRounds.FINALS.ToString())
            cupBotId = Cup.Instance.cupInfo[CupDB.CupRounds.FINALS.ToString()]["7"]["14"];

        for (int i = 0; i < Cup.Instance.participants.Count; i++)
        {
            if (cupBotId == Cup.Instance.participants[counter].id)
                return Cup.Instance.participants[counter];
            counter++;
        }

        Debug.Log("Couldn't get fighter!");
        return new CupFighter("", "", "");
    }

    private static Dictionary<string, float> GenerateBotRandomStats(SpeciesNames randomSpecies)
    {
        float hp = Species.defaultStats[randomSpecies]["hp"] + (Species.statsPerLevel[randomSpecies]["hp"] * (Combat.player.level - 1));
        float damage = Species.defaultStats[randomSpecies]["damage"] + (Species.statsPerLevel[randomSpecies]["damage"] * (Combat.player.level - 1));
        float speed = Species.defaultStats[randomSpecies]["speed"] + (Species.statsPerLevel[randomSpecies]["speed"] * (Combat.player.level - 1));

        return new Dictionary<string, float>
        {
            {"hp", hp},
            {"damage", damage},
            {"speed", speed},
        };
    }
    private static SpeciesNames GetRandomSpecies()
    {
        System.Random random = new System.Random();
        Array species = Enum.GetValues(typeof(SpeciesNames));
        return (SpeciesNames)species.GetValue(random.Next(species.Length));
    }
    private static string FetchBotRandomName()
    {
        return RandomNameGenerator.GenerateRandomName();
    }

    private static int GenerateBotElo(int playerElo)
    {
        int botElo = UnityEngine.Random.Range(playerElo - 50, playerElo + 50);
        return botElo >= 0 ? botElo : 0;
    }
    private static int GenerateBotLevel(int playerLevel)
    {
        return playerLevel > 1 ? UnityEngine.Random.Range(playerLevel - 1, playerLevel + 2) : playerLevel;
    }

    public static int CalculateEloChange(int playerElo, int botElo, bool isPlayerWinner)
    {
        int eloDifference = botElo - playerElo;
        int eloPonderance = eloDifference / 10;
        int absoluteEloChange = baseEloGain + eloPonderance;
        int modifierToRankUpOverTime = 2;
        int eloChange = isPlayerWinner ? absoluteEloChange : -absoluteEloChange + modifierToRankUpOverTime;
        if (playerElo + eloChange < 0) return -playerElo;
        return eloChange;
    }
}