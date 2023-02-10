using System.Collections.Generic;

public static class Gold
{
    public enum ShopGoldBundles
    {
        SMALLGOLD1,
        SMALLGOLD2,
        MEDIUMGOLD1,
        MEDIUMGOLD2,
        BIGGOLD
    }

    public static readonly Dictionary<ShopGoldBundles, Dictionary<string, int>> shopGoldBundlesCost =
        new Dictionary<ShopGoldBundles, Dictionary<string, int>>
        {
            {
                ShopGoldBundles.SMALLGOLD1, new Dictionary<string, int>
                {
                    {"gems", 10},
                }
            },
            {
                ShopGoldBundles.SMALLGOLD2, new Dictionary<string, int>
                {
                    {"gems", 22},
                }
            },
            {
                ShopGoldBundles.MEDIUMGOLD1, new Dictionary<string, int>
                {
                    {"gems", 38},
                }
            },
            {
                ShopGoldBundles.MEDIUMGOLD2, new Dictionary<string, int>
                {
                    {"gems", 70},
                }
            },
            {
                ShopGoldBundles.BIGGOLD, new Dictionary<string, int>
                {
                    {"gems", 300},
                }
            }
        };

    public static readonly Dictionary<ShopGoldBundles, Dictionary<string, int>> shopGoldBundlesValue =
        new Dictionary<ShopGoldBundles, Dictionary<string, int>>
        {
            { 
                ShopGoldBundles.SMALLGOLD1, new Dictionary<string, int>
                {
                    {"gold", 100},
                }
            },
            {
                ShopGoldBundles.SMALLGOLD2, new Dictionary<string, int>
                {
                    {"gold", 250},
                }
            },
            {
                ShopGoldBundles.MEDIUMGOLD1, new Dictionary<string, int>
                {
                    {"gold", 500},
                }
            },
            {
                ShopGoldBundles.MEDIUMGOLD2, new Dictionary<string, int>
                {
                    {"gold", 1000},
                }
            },
            {
                ShopGoldBundles.BIGGOLD, new Dictionary<string, int>
                {
                    {"gold", 5000},
                }
            }
        };
}
