using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardDB
{
    const int INITIAL_TROPHIES_MIN = 900;
    const int INITIAL_TROPHIES_MAX = 1200;

    const int LOW_TROPHY_GAINS_MIN = 30;
    const int LOW_TROPHY_GAINS_MAX = 40;

    const int MEDIUM_TROPHY_GAINS_MIN = 40;
    const int MEDIUM_TROPHY_GAINS_MAX = 55;

    const int BIG_TROPHY_GAINS_MIN = 50;
    const int BIG_TROPHY_GAINS_MAX = 60;


    public enum Flag
    {
        CHN,
        DEU,
        ENG,
        ESP,
        FRA,
        GRE,
        INA,
        ITA,
        JPN,
        KOR,
        POL,
        PRA,
        PRT,
        ROU,
        RUS,
        SWE,
        THA,
        TUR,
        TWN,
        UKR
    }

    public static readonly Dictionary<string, Dictionary<string, string>> players =
    new Dictionary<string, Dictionary<string, string>>
    {
        {
            "1", 
            new Dictionary<string, string>{
                {"name", "monstersarius"},
                {"country", "GRE"},
                {"trophies", "0"},
                {"specie", "Orc"},
            }
        },
        {
            "2",
            new Dictionary<string, string>{
                {"name", "Turges"},
                {"country", "ITA"},
                {"trophies", "0"},
                {"specie", "Ogre"},
            }
        },
        {
            "3",
            new Dictionary<string, string>{
                {"name", "adamqa"},
                {"country", "ESP"},
                {"trophies", "0"},
                {"specie", "FallenAngel1"},
            }
        },
        {
            "4",
            new Dictionary<string, string>{
                {"name", "ellster16"},
                {"country", "DEU"},
                {"trophies", "0"},
                {"specie", "Golem1"},
            }
        },
        {
            "5",
            new Dictionary<string, string>{
                {"name", "thenameisG"},
                {"country", "CHN"},
                {"trophies", "0"},
                {"specie", "FallenAngel2"},
            }
        },
        {
            "6",
            new Dictionary<string, string>{
                {"name", "Yorphudi"},
                {"country", "PRT"},
                {"trophies", "0"},
                {"specie", "Goblin"},
            }
        },
        {
            "7",
            new Dictionary<string, string>{
                {"name", "emf44"},
                {"country", "KOR"},
                {"trophies", "0"},
                {"specie", "FallenAngel3"},
            }
        },
        {
            "8",
            new Dictionary<string, string>{
                {"name", "kirisunaKUN"},
                {"country", "FRA"},
                {"trophies", "0"},
                {"specie", "Goblin"},
            }
        },
        {
            "9",
            new Dictionary<string, string>{
                {"name", "inkkkk8"},
                {"country", "RUS"},
                {"trophies", "0"},
                {"specie", "Golem2"},
            }
        },
        {
            "10",
            new Dictionary<string, string>{
                {"name", "jeje6789"},
                {"country", "ESP"},
                {"trophies", "0"},
                {"specie", "Golem1"},
            }
        },
        {
            "11",
            new Dictionary<string, string>{
                {"name", "Crowcifer"},
                {"country", "fra"},
                {"trophies", "0"},
                {"specie", "Orc"},
            }
        },
        {
            "12",
            new Dictionary<string, string>{
                {"name", "IISilverIII"},
                {"country", "SWE"},
                {"trophies", "0"},
                {"specie", "FallenAngel1"},
            }
        },
        {
            "13",
            new Dictionary<string, string>{
                {"name", "Stoner8008"},
                {"country", "ITA"},
                {"trophies", "0"},
                {"specie", "Golem3"},
            }
        },
        {
            "14",
            new Dictionary<string, string>{
                {"name", "EloiseJolie"},
                {"country", "FRA"},
                {"trophies", "0"},
                {"specie", "Goblin"},
            }
        },
        {
            "15",
            new Dictionary<string, string>{
                {"name", "huda khatib"},
                {"country", "TUR"},
                {"trophies", "0"},
                {"specie", "Ogre"},
            }
        },
    };

    public static int GetUserTrophies(string id)
    {
        return PlayerPrefs.GetInt("player" + id);
    }

    public static bool IsFirstTimeUsingDB()
    {
        return PlayerPrefs.GetInt("player15") == 0;
    }

    public static void UpdateDB()
    {
        PlayerPrefs.SetInt("player1", PlayerPrefs.GetInt("player1") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player2", PlayerPrefs.GetInt("player2") + GetLowTrophyGains());
        PlayerPrefs.SetInt("player3", PlayerPrefs.GetInt("player3") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player4", PlayerPrefs.GetInt("player4") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player5", PlayerPrefs.GetInt("player5") + GetLowTrophyGains());
        PlayerPrefs.SetInt("player6", PlayerPrefs.GetInt("player6") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player7", PlayerPrefs.GetInt("player7") + GetMediumTrophyGains());
        PlayerPrefs.SetInt("player8", PlayerPrefs.GetInt("player8") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player9", PlayerPrefs.GetInt("player9") + GetBigTrophyGains());
        PlayerPrefs.SetInt("player10", PlayerPrefs.GetInt("player10") + GetLowTrophyGains());
        PlayerPrefs.SetInt("player11", PlayerPrefs.GetInt("player11") + GetMediumTrophyGains());
        PlayerPrefs.SetInt("player12", PlayerPrefs.GetInt("player12") + GetLowTrophyGains());
        PlayerPrefs.SetInt("player13", PlayerPrefs.GetInt("player13") + GetLowTrophyGains());
        PlayerPrefs.SetInt("player14", PlayerPrefs.GetInt("player14") + GetMediumTrophyGains());
        PlayerPrefs.SetInt("player15", PlayerPrefs.GetInt("player15") + GetLowTrophyGains());

        PlayerPrefs.Save();
    }

    public static void GenerateBaseDB()
    {
        PlayerPrefs.SetInt("player1", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player2", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player3", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player4", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player5", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player6", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player7", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player8", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player9", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player10", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player11", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player12", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player13", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player14", GenerateInitialTrophies());
        PlayerPrefs.SetInt("player15", GenerateInitialTrophies());

        PlayerPrefs.Save();
    }

    private static int GetLowTrophyGains()
    {
        return Random.Range(LOW_TROPHY_GAINS_MIN, LOW_TROPHY_GAINS_MAX);
    }

    private static int GetMediumTrophyGains()
    {
        return Random.Range(MEDIUM_TROPHY_GAINS_MIN, MEDIUM_TROPHY_GAINS_MAX);
    }

    private static int GetBigTrophyGains()
    {
        return Random.Range(BIG_TROPHY_GAINS_MIN, BIG_TROPHY_GAINS_MAX);
    }

    private static int GenerateInitialTrophies()
    {
        return Random.Range(INITIAL_TROPHIES_MIN, INITIAL_TROPHIES_MAX);
    }
}
