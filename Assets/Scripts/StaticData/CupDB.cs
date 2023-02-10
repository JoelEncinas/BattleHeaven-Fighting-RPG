using System.Collections.Generic;

public static class CupDB 
{
    public enum CupNames
    {
        DIVINE
    }

    public enum CupRounds
    {
        ZERO, 
        QUARTERS,
        SEMIS,
        FINALS,
        END
    }

    // rewards given if each round if passed
    public static readonly Dictionary<CupRounds, Dictionary<string, string>> prizes =
        new Dictionary<CupRounds, Dictionary<string, string>>
        {
            {
                // Prize if player doesn't win any round
                CupRounds.ZERO, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "1" } 
                }
            },
            {
                CupRounds.QUARTERS, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "1500" }
                }
            },
            {
                CupRounds.SEMIS, new Dictionary<string, string>
                {
                    {"reward", "gems"},
                    {"value", "50"}
                }
            },
            {
                CupRounds.FINALS, new Dictionary<string, string>
                {
                    {"reward", "chest"},
                    {"value", "LEGENDARY"}
                }
            },
        };
}
