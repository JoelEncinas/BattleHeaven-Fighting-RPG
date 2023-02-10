using UnityEngine;

public class Explosion : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Animator explosionAnimator = fighter.transform.Find("VFX/Explosion_VFX").GetComponent<Animator>();
        explosionAnimator.Play("explosion_0", -1, 0f);
    }
}