using System.Collections.Generic;

public class SerializableFighter
{
    private string _fighterName;
    private float _hp;
    private float _damage;
    private float _speed;
    private string _species;
    private string _skin;
    private int _level;
    private int _experiencePoints;
    private List<Skill> _skills;

    public string fighterName { get => _fighterName; set => _fighterName = value; }
    public float hp { get => _hp; set => _hp = value; }
    public float damage { get => _damage; set => _damage = value; }
    public float speed { get => _speed; set => _speed = value; }
    public string species { get => _species; set => _species = value; }
    public string skin { get => _skin; set => _skin = value; }
    public int level { get => _level; set => _level = value; }
    public int experiencePoints { get => _experiencePoints; set => _experiencePoints = value; }
    public List<Skill> skills { get => _skills; set => _skills = value; }

    public SerializableFighter(Fighter fighter)
    {
        this.fighterName = fighter.fighterName;
        this.hp = fighter.hp;
        this.damage = fighter.damage;
        this.speed = fighter.speed;
        this.species = fighter.species;
        this.skin = fighter.skin;
        this.level = fighter.level;
        this.experiencePoints = fighter.experiencePoints;
        this.skills = fighter.skills;
    }
}