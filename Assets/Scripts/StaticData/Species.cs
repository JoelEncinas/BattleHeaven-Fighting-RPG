using System.Collections.Generic;
public enum SpeciesNames
{
    // Player skins
    FallenAngel1,
    Golem1,
    Orc,

    // AI
    FallenAngel2,
    FallenAngel3,
    Golem2,
    Golem3,
    Goblin,
    Ogre
}

public class Species
{
    public static readonly Dictionary<SpeciesNames, Dictionary<string, float>> defaultStats =
    new Dictionary<SpeciesNames, Dictionary<string, float>>
    {
        // Orcs
        {SpeciesNames.Orc, new Dictionary<string, float>{{"hp", 100},{"damage", 11f},{"speed", 1}}},
        {SpeciesNames.Goblin, new Dictionary<string, float>{{"hp", 100},{"damage", 11f},{"speed", 1}}},
        {SpeciesNames.Ogre, new Dictionary<string, float>{{"hp", 100},{"damage", 11f},{"speed", 1}}},

        // Golems
        {SpeciesNames.Golem1, new Dictionary<string, float>{{"hp", 120},{"damage", 9f},{"speed", 3}}},
        {SpeciesNames.Golem2, new Dictionary<string, float>{{"hp", 120},{"damage", 9f},{"speed", 3}}},
        {SpeciesNames.Golem3, new Dictionary<string, float>{{"hp", 120},{"damage", 9f},{"speed", 3}}},

        // Angels
        {SpeciesNames.FallenAngel1, new Dictionary<string, float>{{"hp", 110},{"damage", 10f},{"speed", 2}}},
        {SpeciesNames.FallenAngel2, new Dictionary<string, float>{{"hp", 110},{"damage", 10f},{"speed", 2}}},
        {SpeciesNames.FallenAngel3, new Dictionary<string, float>{{"hp", 110},{"damage", 10f},{"speed", 2}}},
    };

    public static readonly Dictionary<SpeciesNames, Dictionary<string, float>> statsPerLevel =
    new Dictionary<SpeciesNames, Dictionary<string, float>>
    {
        // Orcs
        {SpeciesNames.Orc, new Dictionary<string, float>{{"hp", 30},{"damage", 2.8f},{"speed", 0.75f}}},
        {SpeciesNames.Goblin, new Dictionary<string, float>{{"hp", 30},{"damage", 2.8f},{"speed", 0.75f}}},
        {SpeciesNames.Ogre, new Dictionary<string, float>{{"hp", 30},{"damage", 2.8f},{"speed", 0.75f}}},

        // Golems
        {SpeciesNames.Golem1, new Dictionary<string, float>{{"hp", 35},{"damage", 2.25f},{"speed", 1.25f}}},
        {SpeciesNames.Golem2, new Dictionary<string, float>{{"hp", 35},{"damage", 2.25f},{"speed", 1.25f}}},
        {SpeciesNames.Golem3, new Dictionary<string, float>{{"hp", 35},{"damage", 2.25f},{"speed", 1.25f}}},

        // Angels
        {SpeciesNames.FallenAngel1, new Dictionary<string, float>{{"hp", 33},{"damage", 2.5f},{"speed", 1f}}},
        {SpeciesNames.FallenAngel2, new Dictionary<string, float>{{"hp", 33},{"damage", 2.5f},{"speed", 1f}}},
        {SpeciesNames.FallenAngel3, new Dictionary<string, float>{{"hp", 33},{"damage", 2.5f},{"speed", 1f}}},
    };
}