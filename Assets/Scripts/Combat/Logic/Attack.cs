using System.Collections;
using UnityEngine;
using System;
public class Attack : MonoBehaviour
{
    public GameObject shuriken;
    public GameObject bomb;
    public GameObject potion;

    public IEnumerator PerformAttack(Fighter attacker, Fighter defender, float damageWeight = 1)
    {
        if (Combat.movementScript.FighterShouldAdvanceToAttack(attacker)) yield return StartCoroutine(Combat.movementScript.MoveToMeleeRangeAgain(attacker, defender));

        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.ATTACK);

        if (IsAttackShielded(defender))
        {
            yield return StartCoroutine(ShieldAttack(attacker, defender));
            yield break;
        }

        if (IsAttackDodged(defender))
        {
            yield return DefenderDodgesAttack(defender);
            yield break;
        }
        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * damageWeight, 0.25f, 0.05f);
    }

    IEnumerator ShieldAttack(Fighter attacker, Fighter defender, float secondsToWaitForAttackAnim = 0.4f)
    {
        VFXUtils.DisplayFloatingText(defender, this.gameObject.GetComponent<Combat>().floatingHp, "BLOCKED!");
        Renderer attackerRenderer = attacker.GetComponent<Renderer>();
        //Change sorting order so attack sword goes over defender shield
        attackerRenderer.sortingOrder = Combat.fighterSortingOrder + 2;
        FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.JUMP);
        SpriteRenderer shieldSprite = defender.transform.Find("Shield").GetComponent<SpriteRenderer>();
        shieldSprite.enabled = true;
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(secondsToWaitForAttackAnim));
        shieldSprite.enabled = false;
        FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
        attackerRenderer.sortingOrder = Combat.fighterSortingOrder;
    }

    public IEnumerator PerformCosmicKicks(Fighter attacker, Fighter defender)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.KICK);
        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.CosmicKicks, 0.1f, 0.05f);
    }
    //NEW
    public IEnumerator PerformShadowTravel(Fighter attacker, Fighter defender)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.KICK);
        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.CosmicKicks, 0.1f, 0.05f);
    }
    public IEnumerator PerformLowBlow(Fighter attacker, Fighter defender)
    {
        if (IsAttackShielded(defender))
        {
            yield return StartCoroutine(ShieldAttack(attacker, defender, 0.22f));
            yield break;
        }

        if (IsAttackDodged(defender))
        {
            yield return DefenderDodgesAttack(defender);
            yield break;
        }

        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.LowBlow, 0, 0);
    }
    public IEnumerator PerformJumpStrike(Fighter attacker, Fighter defender)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.AIR_ATTACK);

        if (IsAttackShielded(defender))
        {
            yield return StartCoroutine(ShieldAttack(attacker, defender));
            yield break;
        }

        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.JumpStrike, 0.15f, 0.05f);
        RestoreLife(attacker, GlobalConstants.SkillHeals.JumpStrike);
        Combat.fightersUIDataScript.ModifyHealthBar(attacker);
    }
    public IEnumerator PerformShurikenFury(Fighter attacker, Fighter defender)
    {
        bool dodged = IsAttackDodged(defender);

        Vector3 shurikenStartPos = attacker.transform.position;
        Vector3 shurikenEndPos = defender.transform.position;
        shurikenStartPos.y -= 0.4f;
        shurikenEndPos.y -= 0.4f;
        shurikenEndPos.x = GetShurikenEndPositionX(dodged, attacker, shurikenEndPos);

        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.THROW);
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.1f)); //Throw the shuriken when the fighter arm is already up

        GameObject shurikenInstance = Instantiate(shuriken, shurikenStartPos, Quaternion.identity);

        if (dodged)
        {
            StartCoroutine(Combat.movementScript.RotateObjectOverTime(shurikenInstance, new Vector3(0, 0, 3000), 0.5f));
            StartCoroutine(Combat.movementScript.MoveShuriken(shurikenInstance, shurikenStartPos, shurikenEndPos, 0.5f)); //We dont yield here so we can jump mid animation
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.2f)); //Wait for the shuriken to approach before jumping
            yield return DefenderDodgesAttack(defender);
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.2f)); //Wait for the shuriken to be in its final position before destroying it (This could be avoided with colliders)
            Destroy(shurikenInstance);
            yield break;
        }

        StartCoroutine(Combat.movementScript.RotateObjectOverTime(shurikenInstance, new Vector3(0, 0, 2000), 0.35f));
        yield return StartCoroutine(Combat.movementScript.MoveShuriken(shurikenInstance, shurikenStartPos, shurikenEndPos, 0.35f));
        Destroy(shurikenInstance);

        if (IsAttackShielded(defender))
        {
            yield return StartCoroutine(ShieldAttack(attacker, defender));
            yield break;
        }

        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.ShurikenFury, 0.25f, 0);
    }

    public IEnumerator PerformExplosiveBomb(Fighter attacker, Fighter defender)
    {
        Vector3 bombStartPos = attacker.transform.position;

        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.THROW);
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.1f)); //Throw the bomb when the fighter arm is already up

        GameObject bombInstance = Instantiate(bomb, bombStartPos, Quaternion.identity);
        bombInstance.AddComponent(Type.GetType("BombAnimation"));
        bombInstance.GetComponent<BombAnimation>().targetPos = defender.initialPosition;

        if (IsAttackShielded(defender))
        {
            //Cast shield when bomb is mid air
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.35f));
            StartCoroutine(ShieldAttack(attacker, defender));
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.25f));
            Explosion.StartAnimation(defender);
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.6f));
            yield break;
        }

        //Wait bomb travel time        
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.6f));
        Explosion.StartAnimation(defender);

        yield return DefenderReceivesAttack(attacker, defender, attacker.damage * GlobalConstants.SkillDamages.ExplosiveBomb, 0.4f, 0.1f);
    }

    public IEnumerator PerformHealingPotion(Fighter attacker)
    {
        Vector3 potionPosition = attacker.transform.position;
        potionPosition.y += 2.5f;
        GameObject potionInstance = Instantiate(potion, potionPosition, Quaternion.identity);
        RestoreLife(attacker, GlobalConstants.SkillHeals.HealingPotion);
        Combat.fightersUIDataScript.ModifyHealthBar(attacker);

        Renderer attackerRenderer = attacker.GetComponent<Renderer>();
        //Don't change color if we already have other color modifications active
        if (attackerRenderer.material.color == GlobalConstants.noColor)
        {
            attackerRenderer.material.color = new Color(147 / 255f, 255 / 255f, 86 / 255f);
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1.5f));
            attackerRenderer.material.color = GlobalConstants.noColor;
        }
        else yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1.5f));
        Destroy(potionInstance);
    }

    private float GetShurikenEndPositionX(bool dodged, Fighter attacker, Vector3 shurikenEndPos)
    {
        if (dodged) return Combat.player == attacker ? shurikenEndPos.x + 10 : shurikenEndPos.x - 10;
        //To move the hitbox a bit upfront
        return Combat.player == attacker ? shurikenEndPos.x - 1f : shurikenEndPos.x + 1f;
    }


    IEnumerator DefenderDodgesAttack(Fighter defender)
    {
        VFXUtils.DisplayFloatingText(defender, this.gameObject.GetComponent<Combat>().floatingHp, "DODGED!");
        StartCoroutine(Combat.movementScript.DodgeMovement(defender));
        FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.JUMP);
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.3f));
        FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
    }

    IEnumerator DefenderReceivesAttack(Fighter attacker, Fighter defender, float damagePerHit, float secondsToWaitForHurtAnim, float secondsUntilHitMarker)
    {
        if (defender.HasSkill(SkillNames.EarlyBubble))
        {
            defender.removeUsedSkill(SkillNames.EarlyBubble);
            Bubble.StartAnimation(defender);
            //Wait for attack animation to finish
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.3f));
            yield break;
        }

        DealDamage(attacker, defender, damagePerHit);

        Combat.isGameOver = defender.hp <= 0 ? true : false;

        if (Combat.isGameOver)
        {
            FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.DEATH);
            yield return StartCoroutine(ReceiveDamageAnimation(defender, secondsUntilHitMarker));
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.15f)); //Wait for attack animation to finish
        }
        else
        {
            FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.HURT);
            yield return StartCoroutine(ReceiveDamageAnimation(defender, secondsUntilHitMarker));
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(secondsToWaitForHurtAnim));
        }
    }

    private void PlayDamageSound(){
        int soundIndex = UnityEngine.Random.Range(1, 4);
        string soundName = $"S_Damage_Received_{soundIndex}";
        FindObjectOfType<AudioManager>().Play(soundName);
    }

    private void DealDamage(Fighter attacker, Fighter defender, float damagePerHit)
    {
        PlayDamageSound();

        bool isAttackCritical = IsAttackCritical(attacker);
        Color floatingHpColor = GlobalConstants.noColor;

        if (isAttackCritical)
        {
            damagePerHit = damagePerHit * 1.5f;
            floatingHpColor = GlobalConstants.criticalAttackColor;
        }

        VFXUtils.DisplayFloatingHp(defender, this.gameObject.GetComponent<Combat>().floatingHp, damagePerHit, floatingHpColor);

        defender.hp -= damagePerHit;

        if (defender.hp <= 0 && defender.HasSkill(SkillNames.Survival))
        {
            Aura.StartAnimation(defender);
            defender.hp = 1;
            defender.removeUsedSkill(SkillNames.Survival);
        }

        Combat.fightersUIDataScript.ModifyHealthBar(defender);
    }

    //Restores x % of total health
    private void RestoreLife(Fighter attacker, double percentage)
    {
        float maxHp = Combat.player == attacker ? Combat.playerMaxHp : Combat.botMaxHp;
        double hpAfterHeal = attacker.hp + (percentage / 100 * maxHp);
        double updatedHp = hpAfterHeal > maxHp ? maxHp : hpAfterHeal;
        float hpToRestore = (float)updatedHp - attacker.hp;
        VFXUtils.DisplayFloatingHp(attacker, this.gameObject.GetComponent<Combat>().floatingHp, hpToRestore, GlobalConstants.healColor);
        attacker.hp += hpToRestore;
    }

    IEnumerator ReceiveDamageAnimation(Fighter defender, float secondsUntilHitMarker)
    {
        Blood.StartAnimation(defender);
        Renderer defenderRenderer = defender.GetComponent<Renderer>();

        if (defenderRenderer.material.color == GlobalConstants.noColor)
        {
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(secondsUntilHitMarker));
            defenderRenderer.material.color = new Color(255, 1, 1);
            yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.08f));
            defenderRenderer.material.color = GlobalConstants.noColor;
        }
        else yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(secondsUntilHitMarker + .08f));
    }

    public bool IsAttackShielded(Fighter fighter)
    {
        int probabilityOfShielding = 10;
        return fighter.HasSkill(SkillNames.GloriousShield) && Probabilities.IsHappening(probabilityOfShielding);
    }

    public bool IsBasicAttackRepeated(Fighter attacker)
    {
        return Probabilities.IsHappening(attacker.repeatAttackChance);
    }

    private bool IsAttackDodged(Fighter defender)
    {
        return Probabilities.IsHappening(defender.dodgeChance);
    }

    private bool IsAttackCritical(Fighter attacker)
    {
        return Probabilities.IsHappening(attacker.criticalChance);
    }

    public bool IsReversalAttack(Fighter defender)
    {
        return Probabilities.IsHappening(defender.reversalChance);
    }

    public bool IsCounterAttack(Fighter defender)
    {
        return Probabilities.IsHappening(defender.counterAttackChance);
    }
    public bool IsExtraTurn(Fighter attacker)
    {
        return Probabilities.IsHappening(attacker.speed);
    }
}