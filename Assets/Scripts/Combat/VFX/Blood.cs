using UnityEngine;

public class Blood : MonoBehaviour
{
    public static void StartAnimation(Fighter fighter)
    {
        Animator bloodAnimator = fighter.transform.Find("VFX/Hit_VFX").GetComponent<Animator>();
        bloodAnimator.Play(GetRandomBloodClipName(), -1, 0f);
    }

    private static string GetRandomBloodClipName()
    {
        var bloodAnimatorController = GameObject.Find("Hit_VFX").GetComponent<Animator>().runtimeAnimatorController;
        int randomIndex = Random.Range(0, bloodAnimatorController.animationClips.Length);
        return bloodAnimatorController.animationClips[randomIndex].name;
    }
}