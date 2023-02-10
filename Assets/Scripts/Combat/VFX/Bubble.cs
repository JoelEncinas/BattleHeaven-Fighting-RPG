using UnityEngine;

public class Bubble : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Animator bubbleAnimator = fighter.transform.Find("VFX/Bubble_VFX").GetComponent<Animator>();
        bubbleAnimator.Play("bubble_0", -1, 0f);
    }
}