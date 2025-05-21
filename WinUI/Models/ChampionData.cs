using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Models;

public class ChampionData
{
    public List<ChampionList> Lists { get; set; }

    public ChampionData()
    {
        Lists = new List<ChampionList>();
    }
}