public class CupFighter
{
    // Use this class to generate cup opponents 
    // and save them in cup file
    private string _id;
    private string _fighterName;
    private string _species;

    public CupFighter(string id, string fighterName, string species)
    {
        this.id = id;
        this.fighterName = fighterName;
        this.species = species;
    }

    public string id
    {
        get => _id; set
        {
            _id = value;
        }
    }
    public string fighterName
    {
        get => _fighterName; set
        {
            _fighterName = value;
        }
    }
    public string species
    {
        get => _species; set
        {
            _species = value;
        }
    }
}
