using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Models;

public class Champion
{
    public string Name { get; set; }

    public Champion(string name)
    {
        Name = name;
    }
}