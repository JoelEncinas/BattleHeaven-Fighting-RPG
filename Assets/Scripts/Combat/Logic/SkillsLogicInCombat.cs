using System.Collections;
using UnityEngine;
using TMPro;

public class SkillsLogicInCombat : MonoBehaviour
{
    private Combat combatScript;
    private Movement movementScript;
    private Attack attackScript;
    //TODO v2: This should be encapsulated on a class whenever we have a class for each skill
    private const float PassiveSkillsModifier = 1.05f;
    private void Awake()
    {
        combatScript = this.GetComponent<Combat>();
        movementScript = this.GetComponent<Movement>();
        attackScript = this.GetComponent<Attack>();
    }
    public IEnumerator AttackWithoutSkills(Fighter attacker, Fighter defender)
    {
        yield return combatScript.MoveForwardHandler(attacker, defender);

        //Counter attack
        if (attackScript.IsCounterAttack(defender)) yield return BasicAttackLogic(defender, attacker);

        // Attack
        yield return BasicAttackLogic(attacker, defender);

        //Reversal attack
        if (attackScript.IsReversalAttack(defender)) yield return BasicAttackLogic(defender, attacker);

        if (!Combat.isGameOver)
        {
            FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
            yield return combatScript.MoveBackHandler(attacker);
        }
    }
    private IEnumerator BasicAttackLogic(Fighter attacker, Fighter defender)
    {
        int attackCounter = 0;

        while (!Combat.isGameOver && (attackCounter == 0 || attackScript.IsBasicAttackRepeated(attacker)))
        {
            yield return StartCoroutine(attackScript.PerformAttack(attacker, defender));
            attackCounter++;
        };
    }
    public IEnumerator LowBlow(Fighter attacker, Fighter defender)
    {
        MoveBloodPosition(defender);
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.RUN);
        yield return movementScript.MoveSlide(attacker, Combat.GetAttackerDestinationPosition(defender, 0.8f));
        yield return StartCoroutine(attackScript.PerformLowBlow(attacker, defender));
        yield return combatScript.MoveBackHandler(attacker);
        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
        ResetBloodPosition(defender);
    }

    public IEnumerator JumpStrike(Fighter attacker, Fighter defender)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.RUN);
        Vector3 attackerDestinationPosition = Combat.GetAttackerDestinationPosition(defender, 1f);

        yield return movementScript.MoveJumpStrike(attacker, attackerDestinationPosition);

        float rotationDegrees = attacker == Combat.player ? -35f : 35f;
        movementScript.Rotate(attacker, rotationDegrees);

        int nStrikes = UnityEngine.Random.Range(4, 7); // 4-6 attacks

        for (int i = 0; i < nStrikes && !Combat.isGameOver; i++)
        {
            yield return StartCoroutine(attackScript.PerformJumpStrike(attacker, defender));
        }

        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);

        //Go back to the ground
        yield return StartCoroutine(movementScript.Move(attacker, attacker.transform.position, attackerDestinationPosition, 0.1f));
        movementScript.ResetRotation(attacker);

        yield return combatScript.MoveBackHandler(attacker);
    }

    public IEnumerator ShurikenFury(Fighter attacker, Fighter defender)
    {
        int nShurikens = UnityEngine.Random.Range(4, 7); // 4-6 shurikens

        for (int i = 0; i < nShurikens && !Combat.isGameOver; i++)
        {
            yield return StartCoroutine(attackScript.PerformShurikenFury(attacker, defender));
        }

        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
    }

    public IEnumerator CosmicKicks(Fighter attacker, Fighter defender)
    {
        MoveBloodPosition(defender);

        yield return combatScript.MoveForwardHandler(attacker, defender, 1.5f);

        int nKicks = UnityEngine.Random.Range(4, 7); // 4-6 kicks

        for (int i = 0; i < nKicks && !Combat.isGameOver; i++)
        {
            yield return StartCoroutine(attackScript.PerformCosmicKicks(attacker, defender));
        }

        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);

        yield return combatScript.MoveBackHandler(attacker);

        ResetBloodPosition(defender);
    }

    private void ResetBloodPosition(Fighter defender)
    {
        Transform blood = defender.transform.Find("VFX/Hit_VFX");
        Vector3 bloodPosition = blood.transform.position;
        blood.transform.position = new Vector3(bloodPosition.x, Combat.defaultBloodPositionY, bloodPosition.z);
    }

    private void MoveBloodPosition(Fighter defender)
    {
        Transform blood = defender.transform.Find("VFX/Hit_VFX");
        Vector3 bloodPosition = blood.transform.position;
        bloodPosition.y -= 1.2f;
        blood.transform.position = new Vector3(bloodPosition.x, bloodPosition.y, bloodPosition.z);
    }
    public IEnumerator ExplosiveBomb(Fighter attacker, Fighter defender)
    {
        yield return StartCoroutine(attackScript.PerformExplosiveBomb(attacker, defender));
        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
    }
    public IEnumerator ShadowTravel(Fighter attacker, Fighter defender)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.IDLE_BLINKING);
        Clock.StartAnimation(attacker);
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1f));
        SetOpacityOfFighterAndShadow(attacker, 0.15f);
        yield return combatScript.MoveForwardHandler(attacker, defender);
        SetOpacityOfFighterAndShadow(attacker, 1f);
        yield return StartCoroutine(attackScript.PerformAttack(attacker, defender, GlobalConstants.SkillDamages.ShadowTravel));

        if (!Combat.isGameOver) FighterAnimations.ChangeAnimation(defender, FighterAnimations.AnimationNames.IDLE);
        yield return combatScript.MoveBackHandler(attacker);
    }
    public IEnumerator HealingPotion(Fighter attacker)
    {
        FighterAnimations.ChangeAnimation(attacker, FighterAnimations.AnimationNames.IDLE_BLINKING);
        yield return StartCoroutine(attackScript.PerformHealingPotion(attacker));
    }

    public IEnumerator ViciousTheft(Fighter attacker, string stolenSkill)
    {

        Anomaly.StartAnimation(attacker);

        TextMeshPro anomalyInfoText = attacker.transform.Find("VFX/Anomaly_VFX/TextContainer/Text").GetComponent<TextMeshPro>();
        TextMeshPro anomalySkillText = attacker.transform.Find("VFX/Anomaly_VFX/TextContainer/Skill").GetComponent<TextMeshPro>();

        anomalyInfoText.text = "STEALING.";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.5f));
        anomalyInfoText.text = "STEALING..";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.5f));
        anomalyInfoText.text = "STEALING...";
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(.5f));
        anomalyInfoText.text = "STOLEN!";
        anomalySkillText.text = stolenSkill.ToUpper();
        yield return new WaitForSeconds(GeneralUtils.GetRealOrSimulationTime(1.3f));

        attacker.transform.Find("VFX/Anomaly_VFX/TextContainer").gameObject.SetActive(false);
        Anomaly.StopAnimation(attacker);
    }
    private void SetOpacityOfFighterAndShadow(Fighter attacker, float opacity)
    {
        attacker.GetComponent<Renderer>().material.color = GetFighterColorWithCustomOpacity(attacker, opacity);
        GetFighterShadow(attacker).GetComponent<SpriteRenderer>().color = GetFighterShadowColorWithCustomOpacity(attacker, opacity);
    }
    private Color GetFighterShadowColorWithCustomOpacity(Fighter fighter, float opacity)
    {
        GameObject shadow = GetFighterShadow(fighter);
        Color shadowColor = shadow.GetComponent<SpriteRenderer>().color;
        shadowColor.a = opacity;
        return shadowColor;
    }
    private GameObject GetFighterShadow(Fighter fighter)
    {
        return fighter == Combat.player ? GameObject.FindGameObjectWithTag("PlayerShadow") : GameObject.FindGameObjectWithTag("BotShadow");
    }
    private Color GetFighterColorWithCustomOpacity(Fighter fighter, float opacity)
    {
        Color fighterColor = fighter.GetComponent<Renderer>().material.color;
        fighterColor.a = opacity;
        return fighterColor;
    }

    public void BoostStatsBasedOnPassiveSkills(Fighter fighter)
    {
        if (fighter.HasSkill(SkillNames.DangerousStrength)) fighter.damage *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.Heavyweight)) fighter.hp *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.Lightning)) fighter.speed *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.Persistant)) fighter.repeatAttackChance *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.FelineAgility)) fighter.dodgeChance *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.CriticalBleeding)) fighter.criticalChance *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.Reversal)) fighter.reversalChance *= PassiveSkillsModifier;
        if (fighter.HasSkill(SkillNames.CounterAttack)) fighter.counterAttackChance *= PassiveSkillsModifier;
    }
}