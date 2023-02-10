using UnityEngine;

//Game config variables
public class GlobalConstants
{
    //Simulation mode
    public static bool SimulationEnabled = false;
    public static float SimulationTime = 0f;

    //Skills
    public static int ProbabilityOfUsingSkillEachTurn = 25;

    //Colors
    public static Color noColor = new Color(1, 1, 1);
    public static Color healColor = new Color32(134, 255, 117, 255);
    public static Color criticalAttackColor = new Color32(240, 164, 0, 255);

    //Skill damage weight -> attacker.damage * skillDamage
    public struct SkillDamages
    {
        public const float CosmicKicks = 0.6f;
        public const float ShurikenFury = 0.7f;
        public const float LowBlow = 3.5f;
        public const float JumpStrike = 0.6f;
        public const float ExplosiveBomb = 3.5f;
        public const float ShadowTravel = 4f;
        public const float Boost = 1.75f;
    }

    //Skill heal in %
    public struct SkillHeals
    {
        public const float HealingPotion = 25f; //of max hp
        public const float JumpStrike = 3f; //of max hp
        public const float Elixir = 30f; //of missing hp
    }
}