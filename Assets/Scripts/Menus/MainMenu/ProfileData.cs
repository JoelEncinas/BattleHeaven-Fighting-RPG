using UnityEngine;

public class ProfileData
{
    public static void SavePeakElo(int elo)
    {
        if (elo > User.Instance.peakElo) User.Instance.peakElo = elo;
    }

    public static void SaveCups()
    {
        User.Instance.cups++;
    }
}
