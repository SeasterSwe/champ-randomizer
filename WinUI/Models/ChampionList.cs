using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Models;

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