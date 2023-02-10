using System.Collections.Generic;

public static class DailyGiftDB 
{
    public enum Days
    {
        DAY1,
        DAY2,
        DAY3,
        DAY4,
        DAY5,
        DAY6,
        DAY7,
    }

    public static readonly Dictionary<Days, Dictionary<string, string>> gifts =
        new Dictionary<Days, Dictionary<string, string>>
        {
            {
                Days.DAY1, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "250" }
                }
            },
            {
                Days.DAY2, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "500"}
                }
            },
            {
                Days.DAY3, new Dictionary<string, string>
                {
                    {"reward", "gems"},
                    {"value", "10"}
                }
            },
            {
                Days.DAY4, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "750"}
                }
            },
            {
                Days.DAY5, new Dictionary<string, string>
                {
                    {"reward", "gold"},
                    {"value", "1000"}
                }
            },
            {
                Days.DAY6, new Dictionary<string, string>
                {
                    {"reward", "gems"},
                    {"value", "20"}
                }
            },
            {
                Days.DAY7, new Dictionary<string, string>
                {
                    {"reward", "chest"},
                    {"value", "EPIC"}
                }
            },
        };
}
