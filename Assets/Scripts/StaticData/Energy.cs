using System.Collections.Generic;

public static class Energy 
{
    public enum ShopEnergyBundles
    {
        SMALLENERGY,
        MEDIUMENERGY,
        BIGENERGY
    }

    public static readonly Dictionary<ShopEnergyBundles, Dictionary<string, int>> shopEnergyBundlesCost =
    new Dictionary<ShopEnergyBundles, Dictionary<string, int>>
    {
                {
                    ShopEnergyBundles.SMALLENERGY, new Dictionary<string, int>
                    {
                        {"gems", 15},
                    }
                },
                {
                    ShopEnergyBundles.MEDIUMENERGY, new Dictionary<string, int>
                    {
                        {"gems", 60},
                    }
                },
                {
                    ShopEnergyBundles.BIGENERGY, new Dictionary<string, int>
                    {
                        {"gems", 100},
                    }
                }
    };

    public static readonly Dictionary<ShopEnergyBundles, Dictionary<string, int>> shopEnergyBundlesValue =
        new Dictionary<ShopEnergyBundles, Dictionary<string, int>>
        {
                {
                    ShopEnergyBundles.SMALLENERGY, new Dictionary<string, int>
                    {
                        {"energy", 1},
                    }
                },
                {
                    ShopEnergyBundles.MEDIUMENERGY, new Dictionary<string, int>
                    {
                        {"energy", 2},
                    }
                },
                {
                    ShopEnergyBundles.BIGENERGY, new Dictionary<string, int>
                    {
                        {"energy", 5},
                    }
                }
        };
}
