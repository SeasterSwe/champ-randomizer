using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using WinUI.Helpers;
using WinUI.Models;

namespace WinUI.ViewModels;

public class ManageViewModel : NotifyPropertyBase
{
    private ChampionData _championData;
    private ChampionList _currentEditingList;
    private string _newChampionName;
    private string _newListName;
    private string _newListRole;
    private bool _isEditingList;
    private string _tempChampionName;
    private ObservableCollection<string> _tempChampions;

    public ChampionData ChampionData
    {
        get => _championData;
        set => SetProperty(ref _championData, value);
    }

    public ChampionList CurrentEditingList
    {
        get => _currentEditingList;
        set => SetProperty(ref _currentEditingList, value);
    }

    public string NewChampionName
    {
        get => _newChampionName;
        set => SetProperty(ref _newChampionName, value);
    }

    public string NewListName
    {
        get => _newListName;
        set => SetProperty(ref _newListName, value);
    }

    public string NewListRole
    {
        get => _newListRole;
        set => SetProperty(ref _newListRole, value);
    }

    public bool IsEditingList
    {
        get => _isEditingList;
        set => SetProperty(ref _isEditingList, value);
    }

    public string TempChampionName
    {
        get => _tempChampionName;
        set => SetProperty(ref _tempChampionName, value);
    }

    public ObservableCollection<string> TempChampions
    {
        get => _tempChampions;
        set => SetProperty(ref _tempChampions, value);
    }

    public ICommand EditListCommand { get; }
    public ICommand RenameChampionCommand { get; }
    public ICommand RemoveChampionCommand { get; }
    public ICommand AddChampionCommand { get; }
    public ICommand CreateNewListCommand { get; }
    public ICommand SaveDataCommand { get; }
    public ICommand AddTempChampionCommand { get; }
    public ICommand RemoveTempChampionCommand { get; }

    public ObservableCollection<string> AvailableRoles { get; } = new ObservableCollection<string>(Role.Roles);

    private readonly string _dataFilePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ChampionManager",
        "championdata.json");

    public ManageViewModel()
    {
        // Initialize data
        _championData = new ChampionData();
        _tempChampions = new ObservableCollection<string>();

        // Initialize commands
        EditListCommand = new RelayCommand<ChampionList>(EditList);
        RenameChampionCommand = new RelayCommand<Champion>(RenameChampion);
        RemoveChampionCommand = new RelayCommand<Champion>(RemoveChampion);
        AddChampionCommand = new RelayCommand(AddChampion, CanAddChampion);
        CreateNewListCommand = new RelayCommand(CreateNewList);
        SaveDataCommand = new RelayCommand(async () => await SaveDataAsync());
        AddTempChampionCommand = new RelayCommand(AddTempChampion, CanAddTempChampion);
        RemoveTempChampionCommand = new RelayCommand<string>(RemoveTempChampion);

        // Load initial data
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            // Ensure directory exists
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            if (File.Exists(_dataFilePath))
            {
                var json = await File.ReadAllTextAsync(_dataFilePath);
                var loadedData = JsonSerializer.Deserialize<ChampionData>(json);
                if (loadedData != null)
                {
                    ChampionData = loadedData;
                    return;
                }
            }

            // If we couldn't load data, create sample data
            ChampionData = new ChampionData();
            for (int i = 0; i < 2; i++)
            {
                string role = Role.Roles[i % Role.Roles.Count];
                var list = new ChampionList($"Example List {i + 1}", role);

                // Add some sample champions
                list.Champions.Add(new Champion($"Champion {i * 3 + 1}"));
                list.Champions.Add(new Champion($"Champion {i * 3 + 2}"));
                list.Champions.Add(new Champion($"Champion {i * 3 + 3}"));

                ChampionData.Lists.Add(list);
            }
        }
        catch (Exception ex)
        {
            // In a real app, handle the exception more gracefully
            Console.WriteLine($"Error loading data: {ex.Message}");
            ChampionData = new ChampionData();
        }
    }

    public async Task SaveDataAsync()
    {
        try
        {
            var directory = Path.GetDirectoryName(_dataFilePath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(ChampionData, options);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }
        catch (Exception ex)
        {
            // In a real app, handle the exception more gracefully
            Console.WriteLine($"Error saving data: {ex.Message}");
        }
    }

    private void EditList(ChampionList list)
    {
        IsEditingList = true;
        CurrentEditingList = list;
        NewChampionName = string.Empty;
    }

    private async void RenameChampion(Champion champion)
    {
        // Using ContentDialog for a simple rename UI
        var dialog = new ContentDialog
        {
            Title = "Rename Champion",
            PrimaryButtonText = "Rename",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary,
            Content = new TextBox { Text = champion.Name, PlaceholderText = "Enter new name" }
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary && dialog.Content is TextBox textBox)
        {
            var newName = textBox.Text?.Trim();
            if (!string.IsNullOrEmpty(newName))
            {
                champion.Name = newName;
                // Save the changes
                await SaveDataAsync();
            }
        }
    }

    private async void RemoveChampion(Champion champion)
    {
        foreach (var list in ChampionData.Lists)
        {
            if (list.Champions.Remove(champion))
            {
                // Save the changes
                await SaveDataAsync();
                break;
            }
        }
    }

    private async void AddChampion()
    {
        if (CurrentEditingList != null && !string.IsNullOrWhiteSpace(NewChampionName))
        {
            CurrentEditingList.Champions.Add(new Champion(NewChampionName));
            NewChampionName = string.Empty;
            // Save the changes
            await SaveDataAsync();
        }
    }

    private bool CanAddChampion()
    {
        return CurrentEditingList != null && !string.IsNullOrWhiteSpace(NewChampionName);
    }

    private async void CreateNewList()
    {
        if (string.IsNullOrWhiteSpace(NewListName))
        {
            return;
        }

        var role = string.IsNullOrWhiteSpace(NewListRole) ? Role.Roles[0] : NewListRole;
        var newList = new ChampionList(NewListName, role);

        // Add all temporary champions to the new list
        foreach (var championName in TempChampions)
        {
            newList.Champions.Add(new Champion(championName));
        }

        ChampionData.Lists.Add(newList);

        // Clear the input fields
        NewListName = string.Empty;
        NewListRole = string.Empty;
        TempChampions.Clear();
        TempChampionName = string.Empty;

        // Save the changes
        await SaveDataAsync();
    }

    private void AddTempChampion()
    {
        if (!string.IsNullOrWhiteSpace(TempChampionName))
        {
            TempChampions.Add(TempChampionName);
            TempChampionName = string.Empty;
        }
    }

    private bool CanAddTempChampion()
    {
        return !string.IsNullOrWhiteSpace(TempChampionName);
    }

    private void RemoveTempChampion(string championName)
    {
        if (!string.IsNullOrEmpty(championName))
        {
            TempChampions.Remove(championName);
        }
    }

    // Method to handle Enter key press in the champion name TextBox
    public void AddTempChampionOnEnter()
    {
        if (CanAddTempChampion())
        {
            AddTempChampion();
        }
    }
}