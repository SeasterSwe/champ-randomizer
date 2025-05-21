using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WinUI.Helpers;
using WinUI.Models;

namespace WinUI.ViewModels;

public class ManageViewModel
{
    public ChampionData ChampionData { get; set; }

    public ICommand EditListCommand { get; }
    public ICommand RenameChampionCommand { get; }
    public ICommand RemoveChampionCommand { get; }

    public ManageViewModel()
    {
        // Load your data here (for now, just create dummy data)
        ChampionData = new ChampionData();
        for (int i = 0; i < 9; i++)
        {
            string role = Role.Roles[i % Role.Roles.Count];
            ChampionData.Lists.Add(new ChampionList($"Example List {i + 1}", role));
        }

        EditListCommand = new RelayCommand<ChampionList>(EditList);
        RenameChampionCommand = new RelayCommand<Champion>(RenameChampion);
        RemoveChampionCommand = new RelayCommand<Champion>(RemoveChampion);
    }

    private void EditList(ChampionList list)
    {
        // Implement edit logic
    }

    private void RenameChampion(Champion champion)
    {
        // Implement rename logic
    }

    private void RemoveChampion(Champion champion)
    {
        // Implement remove logic
    }
}