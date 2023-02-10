using UnityEngine;

public class CupEnabler : MonoBehaviour
{
    private void Awake()
    {
        // activate cup
        // handles modified version of combat
        if (Cup.Instance.playerStatus)
        {
            Cup.Instance.isActive = true;
            Cup.Instance.SaveCup();
        }
    }
}
