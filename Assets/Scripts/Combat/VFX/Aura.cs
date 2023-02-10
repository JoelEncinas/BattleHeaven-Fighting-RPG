using UnityEngine;

public class Aura : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Animator textAnimator = fighter.transform.Find("VFX/Aura_VFX/Text").GetComponent<Animator>();
        Animator auraAnimator = fighter.transform.Find("VFX/Aura_VFX").GetComponent<Animator>();
        auraAnimator.Play("aura_0", -1, 0f);
        textAnimator.Play("survival_text", -1, 0f);
    }
}