using System.Collections.Generic;
using UnityEngine;

public static class FighterSkin
{
    public static void SetFightersSkin(Fighter player, Fighter bot)
    {
        SetSkin(player);
        SetSkin(bot);
    }

    private static void SetSkin(Fighter fighter)
    {
        LoadFighterSkin(fighter);
        SetAnimationClipToAnimator(fighter);
        SetDefaultAnimation(fighter);
    }

    private static void LoadFighterSkin(Fighter fighter)
    {
        fighter.skinAnimations = Resources.LoadAll<AnimationClip>("Animations/Characters/" + fighter.skin);
    }
    
    //This method gets a list (aoc) with the animation nodes in the animator controller (ordered by creation date)
    //We then loop through all the animation nodes and override them with the skinAnimations loaded from Resources.
    //It is very important that the animation nodes and the animations loaded from resources are in the same order.
    //Consider adding tags to animations to avoid this behaviour
    private static void SetAnimationClipToAnimator(Fighter fighter)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(fighter.animator.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        int index = 0;

        foreach (var defaultClip in aoc.animationClips)
        {
            anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(defaultClip, fighter.skinAnimations[index]));
            index++;
        }
        aoc.ApplyOverrides(anims);
        fighter.animator.runtimeAnimatorController = aoc;
    }
    private static void SetDefaultAnimation(Fighter fighter)
    {
        FighterAnimations.ChangeAnimation(fighter, FighterAnimations.AnimationNames.IDLE);
    }
    public static void SwitchFighterOrientation(SpriteRenderer sprite)
    {
        sprite.flipX = !sprite.flipX;
    }
}