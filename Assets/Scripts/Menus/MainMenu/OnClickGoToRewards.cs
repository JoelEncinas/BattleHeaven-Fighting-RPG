using UnityEngine;

public class OnClickGoToRewards : MonoBehaviour
{
    public GameObject dailyRewards;

    public void ShowDailyRewards()
    {
        dailyRewards.SetActive(true);
    }
}
