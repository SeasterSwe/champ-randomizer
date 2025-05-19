using ChampionListManager;
using CodeTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTest.ViewModels;

public class ChampionListViewModel
{
    private readonly ChampionDataService _dataService;
    private readonly Random _random;

    public ChampionListViewModel(ChampionDataService dataService)
    {
        _dataService = dataService;
        _random = new Random();
    }

    public List<ChampionList> GetAllLists() => _dataService.Data.Lists;

    public bool HasLists() => _dataService.Data.Lists.Count > 0;

    public string[] GetValidRoles() => ChampionDataService.ValidRoles;

    public bool CreateNewList(string name, string role)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        // Check if a list with the same name already exists
        if (_dataService.Data.Lists.Any(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            return false;

        var newList = new ChampionList(name, role);
        _dataService.Data.Lists.Add(newList);
        _dataService.SaveData();
        return true;
    }

    public bool AddChampionsToList(int listIndex, string[] championNames)
    {
        if (listIndex < 0 || listIndex >= _dataService.Data.Lists.Count)
            return false;

        var selectedList = _dataService.Data.Lists[listIndex];
        int addedCount = 0;

        foreach (string championName in championNames)
        {
            string trimmedName = championName.Trim();
            if (string.IsNullOrWhiteSpace(trimmedName))
                continue;

            // Check if champion already exists in the list
            if (selectedList.Champions.Any(c => c.Name.Equals(trimmedName, StringComparison.OrdinalIgnoreCase)))
                continue;

            selectedList.Champions.Add(new Champion(trimmedName));
            addedCount++;
        }

        if (addedCount > 0)
        {
            _dataService.SaveData();
            return true;
        }

        return false;
    }

    public bool RemoveChampionFromList(int listIndex, int championIndex)
    {
        if (listIndex < 0 || listIndex >= _dataService.Data.Lists.Count)
            return false;

        var selectedList = _dataService.Data.Lists[listIndex];

        if (championIndex < 0 || championIndex >= selectedList.Champions.Count)
            return false;

        selectedList.Champions.RemoveAt(championIndex);
        _dataService.SaveData();
        return true;
    }

    public Champion GetRandomChampionByRole(string role)
    {
        // Get all champions from lists with the selected role
        var champsInRole = _dataService.Data.Lists
            .Where(l => l.Role.Equals(role, StringComparison.OrdinalIgnoreCase))
            .SelectMany(l => l.Champions)
            .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();

        if (champsInRole.Count == 0)
            return null;

        // Select a random champion
        int randomIndex = _random.Next(champsInRole.Count);
        return champsInRole[randomIndex];
    }

    public Champion GetRandomChampionFromList(int listIndex)
    {
        if (listIndex < 0 || listIndex >= _dataService.Data.Lists.Count)
            return null;

        var selectedList = _dataService.Data.Lists[listIndex];

        if (selectedList.Champions.Count == 0)
            return null;

        // Select a random champion
        int randomIndex = _random.Next(selectedList.Champions.Count);
        return selectedList.Champions[randomIndex];
    }

    public bool DeleteList(int listIndex)
    {
        if (listIndex < 0 || listIndex >= _dataService.Data.Lists.Count)
            return false;

        _dataService.Data.Lists.RemoveAt(listIndex);
        _dataService.SaveData();
        return true;
    }

    public void SaveChanges()
    {
        _dataService.SaveData();
    }
}