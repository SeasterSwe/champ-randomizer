using CodeTest.ConsoleUI;
using CodeTest.Services;
using CodeTest.ViewModels;
using System.Text.Json;

namespace ChampionListManager;

internal class Program
{
    private static ChampionData _data = new ChampionData();
    private static readonly string _dataFilePath = "championLists.json";
    private static readonly string[] _validRoles = { "Jungle", "Mid", "Adc", "Support", "Top" };

    private static void Main(string[] args)
    {
        var dataService = new ChampionDataService();
        dataService.LoadData();

        // Initialize view model
        var viewModel = new ChampionListViewModel(dataService);

        // Start console application
        var consoleApp = new ConsoleApp(viewModel);
        consoleApp.Run();
    }
}