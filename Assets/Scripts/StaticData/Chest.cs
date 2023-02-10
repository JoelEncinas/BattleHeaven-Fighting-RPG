using System.Collections.Generic;

public static class Chest
{
    public enum BattleChestRarities
    {
        COMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    //FIXME: BattleChestRarities and ShopChestTypes should just be one enum. Not 2 different ones with overlapping fields.
    //THIS IS WONT BE MOVED TO THE DATABASE
    public enum ShopChestTypes
    {
        NORMAL,
        EPIC,
        LEGENDARY,
        //TODO v2: This chest is unactive in the game until we create a timer for it.
        SPECIAL
    }

    // Chance of receiving a chest of each one of the rarities on level up

    public static readonly Dictionary<BattleChestRarities, float> battleChestsProbabilities =
        new Dictionary<BattleChestRarities, float>()
        {
            //FIXME: This numbers don't add up to 100. What is going on here?
            { BattleChestRarities.COMMON, 1}, // 60
            { BattleChestRarities.RARE, 1}, // 25
            { BattleChestRarities.EPIC, 98},    // 12
            { BattleChestRarities.LEGENDARY, 2} // 3
        };

    //Battle chests contain either gold & gems OR a skill
    public static readonly Dictionary<BattleChestRarities, Dictionary<string, int>> battleChestCurrencyRewards =
        new Dictionary<BattleChestRarities, Dictionary<string, int>>
        {
            {
                BattleChestRarities.COMMON, new Dictionary<string, int>
                {
                    {"minGold", 400},
                    {"maxGold", 600},
                    {"minGems", 5},
                    {"maxGems", 10},
                }

            },
            {
                BattleChestRarities.RARE, new Dictionary<string, int>
                {
                    {"minGold", 500},
                    {"maxGold", 800},
                    {"minGems", 10},
                    {"maxGems", 20},
                }
            },
        };

    public static readonly Dictionary<BattleChestRarities, Dictionary<SkillCollection.SkillRarity, float>> battleChestSkillRewardProbability =
        new Dictionary<BattleChestRarities, Dictionary<SkillCollection.SkillRarity, float>>
        {
            {BattleChestRarities.EPIC, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    {SkillCollection.SkillRarity.COMMON, 70},
                    {SkillCollection.SkillRarity.RARE, 30},
                }
            },
            {BattleChestRarities.LEGENDARY, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    {SkillCollection.SkillRarity.EPIC, 70},
                    {SkillCollection.SkillRarity.LEGENDARY, 30},
                }
            },
        };

    public static readonly Dictionary<ShopChestTypes, Dictionary<SkillCollection.SkillRarity, float>> shopChests =
        new Dictionary<ShopChestTypes, Dictionary<SkillCollection.SkillRarity, float>>
        {
            {
                ShopChestTypes.NORMAL, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    // chances of SkillRarity
                    {SkillCollection.SkillRarity.COMMON, 1},
                    {SkillCollection.SkillRarity.RARE, 94},
                    {SkillCollection.SkillRarity.EPIC, 4},
                    {SkillCollection.SkillRarity.LEGENDARY, 1}
                }
            },
            {
                ShopChestTypes.EPIC, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    // chances of SkillRarity
                    {SkillCollection.SkillRarity.COMMON, 0},
                    {SkillCollection.SkillRarity.RARE, 0},
                    {SkillCollection.SkillRarity.EPIC, 95},
                    {SkillCollection.SkillRarity.LEGENDARY, 5}
                }
            },
            {
                ShopChestTypes.LEGENDARY, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    // chances of SkillRarity
                    {SkillCollection.SkillRarity.COMMON, 0},
                    {SkillCollection.SkillRarity.RARE, 0},
                    {SkillCollection.SkillRarity.EPIC, 0},
                    {SkillCollection.SkillRarity.LEGENDARY, 100}
                }
            },
            {
                ShopChestTypes.SPECIAL, new Dictionary<SkillCollection.SkillRarity, float>
                {
                    // chances of SkillRarity
                    {SkillCollection.SkillRarity.COMMON, 0},
                    {SkillCollection.SkillRarity.RARE, 30},
                    {SkillCollection.SkillRarity.EPIC, 60},
                    {SkillCollection.SkillRarity.LEGENDARY, 10}
                }
            },
        };

    public static readonly Dictionary<ShopChestTypes, Dictionary<string, int>> shopChestsValue =
        new Dictionary<ShopChestTypes, Dictionary<string, int>>
        {
                {
                    ShopChestTypes.NORMAL, new Dictionary<string, int>
                    {
                        {"gold", 1000},
                    }
                },
                {
                    ShopChestTypes.EPIC, new Dictionary<string, int>
                    {
                        {"gold", 2500},
                    }
                },
                {
                    ShopChestTypes.LEGENDARY, new Dictionary<string, int>
                    {
                        {"gold", 10000},
                    }
                },
                {
                    ShopChestTypes.SPECIAL, new Dictionary<string, int>
                    {
                        {"gold", 7500},
                    }
                },
        };
}
