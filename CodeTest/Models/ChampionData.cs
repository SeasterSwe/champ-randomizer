namespace ChampionListManager;

public class ChampionData
{
    public List<ChampionList> Lists { get; set; }

    public ChampionData()
    {
        Lists = new List<ChampionList>();
    }
}