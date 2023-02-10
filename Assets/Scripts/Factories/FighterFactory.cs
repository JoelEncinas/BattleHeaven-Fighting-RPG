using System.Collections.Generic;

public static class FighterFactory
{
    public static Fighter CreatePlayerFighterInstance(string fighterName, string skin, string species, float hp, float damage, float speed, 
        List<Skill> skills, int level = 1, int experiencePoints = 0)
    {
        Fighter fighter = PlayerUtils.FindInactiveFighter();
        fighter.FighterConstructor(fighterName, hp, damage, speed, species, skin, level, experiencePoints, skills);
        fighter.EnableSave();
        return fighter;
    }
}
