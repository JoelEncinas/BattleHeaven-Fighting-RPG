//This should be in a database in the future

using System.Collections.Generic;
using System.Collections.Specialized;

public struct SkillNames
{
    public const string DangerousStrength = "Dangerous Strength";
    public const string Heavyweight = "Heavyweight";
    //TODO V2: Change skill name as it collides with one of our VFX names that has nothing to do with this skill
    public const string Lightning = "Lightning";
    public const string Persistant = "Persistant";
    public const string FelineAgility = "Feline Agility";
    public const string CriticalBleeding = "Critical Bleeding";
    public const string Reversal = "Reversal";
    public const string CounterAttack = "Counter Attack";
    public const string Survival = "Survival";
    public const string EarlyBubble = "Early Bubble";
    public const string Initiator = "Initiator";
    public const string CosmicKicks = "Cosmic Kicks";
    public const string ShurikenFury = "Shuriken Fury";
    public const string LowBlow = "Low Blow";
    public const string JumpStrike = "Jump Strike";
    public const string GloriousShield = "Glorious Shield";
    public const string ExplosiveBomb = "Explosive Bomb";
    public const string ShadowTravel = "Shadow Travel";
    public const string HealingPotion = "Healing Potion";
    public const string ViciousTheft = "Vicious Theft";
}

public static class SkillCollection
{
    public enum SkillType
    {
        //Boost fighter stats by a % at the start of the combat and remain until the end of the combat.
        PASSIVE,
        //These are casted at a specific time during the combat and can be used from 1 to n times. Unlike SUPER they dont take a full turn as they are not the main attack but rather an IN-COMBAT like passive.
        SPONTANEOUS,
        //These are a substitute for the default autoattack. 
        //They take a full turn and can only be used once per combat and are removed from the fighter skills list once used.
        SUPER,
    }
    public enum SkillRarity
    {
        COMMON,
        RARE,
        EPIC,
        LEGENDARY
    }

    public static Dictionary<SkillType, string> skillCategoriesLore = new Dictionary<SkillType, string>
    {
        {SkillType.PASSIVE, "Boosts a stat by a set percentage. These cards are active the whole combat."},
        {SkillType.SPONTANEOUS, "Tricky cards that despite not dealing damage provide with great utility."},
        {SkillType.SUPER, "Powerful cards that have a high impact on the combat but can only be used once per combat."},
    };

    public static List<OrderedDictionary> skills =
    new List<OrderedDictionary>
    {
        new OrderedDictionary
        {
            {"name", SkillNames.DangerousStrength},
            {"description", "Increase the attack damage by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "1" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Heavyweight},
            {"description", "Increase the health by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "2" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Lightning},
            {"description", "Increase the speed by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "3" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Persistant},
            {"description", "Increase the chances of attacking multiple times by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "4" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.FelineAgility},
            {"description", "Increase the chance of dodging attacks by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "5" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.CriticalBleeding},
            {"description", "Increase the chance of landing a critical hit by 5%"},
            {"skillRarity", SkillRarity.COMMON.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "6" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Reversal},
            {"description", "Increase the chance of attacking your opponent before he has finished his turn by 5%"},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "7" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.CounterAttack},
            {"description", "Increase the chance of hitting the opponent before it hits you by 5%"},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.PASSIVE.ToString()},
            {"icon", "8" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Initiator},
            {"description", "You attack first every game."},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.SPONTANEOUS.ToString()},
            {"icon", "9" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.GloriousShield},
            {"description", "Whenever your opponent attacks you have a chance of invoking a shield that will block the attack."},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.SPONTANEOUS.ToString()},
            {"icon", "10" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.EarlyBubble},
            {"description", "At the start of the combat a protective bubble surrounds you to grant inmunity against the first attack."},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.SPONTANEOUS.ToString()},
            {"icon", "11" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.Survival},
            {"description", "Whenever you take lethal damage invoke the power of dark matter to survive with 1 health point."},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.SPONTANEOUS.ToString()},
            {"icon", "12" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.HealingPotion},
            {"description", "Use a magic potion that heals a 30% of the maximum health."},
            {"skillRarity", SkillRarity.RARE.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "19" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.CosmicKicks},
            {"description", "Land between 4 and 6 deadly kicks that can't be dodged."},
            {"skillRarity", SkillRarity.EPIC.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "13" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.LowBlow},
            {"description", "Run and slide towards your opponent to hit a low blow that deals critical damage."},
            {"skillRarity", SkillRarity.EPIC.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "15" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.JumpStrike},
            {"description", "Jump towards the opponent to execute a sequence of lightning fast attacks that grant lifesteal and can't be dodged."},
            {"skillRarity", SkillRarity.EPIC.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "16" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.ExplosiveBomb},
            {"description", "Throw an explosive bomb towards your opponent that instantly detonates to inflict severe damage."},
            {"skillRarity", SkillRarity.EPIC.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "17" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.ShadowTravel},
            {"description", "The fury of Zeus strikes you with a lightning that grants invisibility and charges your body with electricity making your next attack deadly."},
            {"skillRarity", SkillRarity.LEGENDARY.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "18" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.ShurikenFury},
            {"description", "Throw between 4 and 6 ninja shurikens at high speed to your opponent."},
            {"skillRarity", SkillRarity.LEGENDARY.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "14" }
        },
        new OrderedDictionary
        {
            {"name", SkillNames.ViciousTheft},
            {"description", "Steal one of the opponent skills and use it immediately."},
            {"skillRarity", SkillRarity.LEGENDARY.ToString()},
            {"category", SkillType.SUPER.ToString()},
            {"icon", "20" }
        }
    };
}
