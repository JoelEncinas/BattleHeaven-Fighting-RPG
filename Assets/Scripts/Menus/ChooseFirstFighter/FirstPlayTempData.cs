public static class FirstPlayTempData
{
    public enum FirstPlayState
    {
        FIGHTER,
        NAME,
        COUNTRY
    }

    public static string fighterName;
    public static string skinName;
    public static string species;
    public static string userName;
    public static string countryFlag;

    // check bools
    public static string lastFlag;
    public static bool firstFlag = false;

    // fighter creation state
    // chooseFighter - name - country
    public static string state;
}
