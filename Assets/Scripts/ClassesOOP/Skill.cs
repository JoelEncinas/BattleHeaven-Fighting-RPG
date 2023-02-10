using UnityEngine;
//Unused enum atm + we already have SkillTypes concept
enum SkillTypes
{
    Basic,
    Neutral,
    Species,
}

public class Skill
{
    private string _skillName;
    private string _description;
    private string _rarity;
    private string _category;
    private string _icon;
    public string skillName { get => _skillName; set => _skillName = value; }
    public string description { get => _description; set => _description = value; }
    public string rarity { get => _rarity; set => _rarity = value; }
    public string category { get => _category; set => _category = value; }
    public string icon { get => _icon; set => _icon = value; }

    public Skill(string skillName, string description, string rarity, string category, string icon)
    {
        this.skillName = skillName;
        this.description = description;
        this.rarity = rarity;
        this.category = category;
        this.icon = icon;
    }
}
