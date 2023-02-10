using UnityEngine;

public class Clock : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Animator clockTravelAnimator = fighter.transform.Find("VFX/Clock_VFX").GetComponent<Animator>();
        clockTravelAnimator.Play("clock_0", -1, 0f);
    }
}