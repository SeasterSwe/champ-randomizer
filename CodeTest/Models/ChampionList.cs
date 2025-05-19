namespace ChampionListManager;

public class ChampionList
{
    public string Name { get; set; }
    public string Role { get; set; }
    public List<Champion> Champions { get; set; }

    public ChampionList(string name, string role)
    {
        Name = name;
        Role = role;
        Champions = new List<Champion>();
    }
}