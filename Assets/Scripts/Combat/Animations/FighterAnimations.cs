using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FighterAnimations : MonoBehaviour
{
    public enum AnimationNames
    {
        IDLE,
        RUN,
        ATTACK,
        JUMP,
        DEATH,
        HURT,
        KICK,
        THROW,
        SLIDE,
        AIR_ATTACK,
        IDLE_BLINKING,
    }

    public static void ChangeAnimation(Fighter fighter, AnimationNames newAnimation)
    {
        if(fighter.currentAnimation == newAnimation.ToString() && AnimationNames.IDLE == newAnimation) return; 
        fighter.GetComponent<Animator>().Play(newAnimation.ToString(), -1, 0f);
        fighter.currentAnimation = newAnimation.ToString();
    }

    public static void ResetToDefaultAnimation(Fighter player)
    {
        ChangeAnimation(player, FighterAnimations.AnimationNames.IDLE);
    }
}