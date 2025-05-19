using ChampionListManager;
using CodeTest.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTest.ConsoleUI;

public class ConsoleApp
{
    private readonly ChampionListViewModel _viewModel;

    public ConsoleApp(ChampionListViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void Run()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            DisplayMainMenu();
            string choice = Console.ReadLine() ?? string.Empty;
            Console.WriteLine();

            running = ProcessMenuChoice(choice);

            if (running && choice != "1") // Skip waiting after viewing lists
            {
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }
        }

        Console.WriteLine("Thank you for using Champion List Manager. Goodbye!");
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("===== Champion List Manager =====");
        Console.WriteLine("\n----- USE CHAMPIONS -----");
        Console.WriteLine("1. View All Lists");
        Console.WriteLine("2. Random Champion by Role");
        Console.WriteLine("3. Random Champion from Specific List");
        Console.WriteLine("\n----- MANAGE LISTS -----");
        Console.WriteLine("4. Create New List");
        Console.WriteLine("5. Add Champion to List");
        Console.WriteLine("6. Remove Champion from List");
        Console.WriteLine("7. Delete List");
        Console.WriteLine("\n8. Exit");
        Console.Write("\nChoose an option: ");
    }

    private bool ProcessMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                ViewAllLists();
                break;

            case "2":
                RandomChampionByRole();
                break;

            case "3":
                RandomChampionFromList();
                break;

            case "4":
                CreateNewList();
                break;

            case "5":
                AddChampionToList();
                break;

            case "6":
                RemoveChampionFromList();
                break;

            case "7":
                DeleteList();
                break;

            case "8":
                return false;

            default:
                Console.WriteLine("Invalid option. Press any key to continue...");
                Console.ReadKey();
                break;
        }

        return true;
    }

    private void ViewAllLists()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a new list first.");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.WriteLine("All Champion Lists:");
        Console.WriteLine("-------------------");

        foreach (var list in _viewModel.GetAllLists())
        {
            Console.WriteLine($"\nList: {list.Name} (Role: {list.Role})");
            Console.WriteLine("Champions:");

            if (list.Champions.Count == 0)
            {
                Console.WriteLine("  No champions in this list");
            }
            else
            {
                foreach (var champion in list.Champions)
                {
                    Console.WriteLine($"  - {champion.Name}");
                }
            }
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void CreateNewList()
    {
        Console.Write("Enter list name: ");
        string name = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("List name cannot be empty.");
            return;
        }

        // Check if a list with the same name already exists
        if (_viewModel.GetAllLists().Any(l => l.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"A list with the name '{name}' already exists.");
            return;
        }

        string[] validRoles = _viewModel.GetValidRoles();
        Console.WriteLine("\nAvailable Roles:");
        for (int i = 0; i < validRoles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {validRoles[i]}");
        }

        Console.Write("\nSelect role number: ");
        if (!int.TryParse(Console.ReadLine(), out int roleIndex) || roleIndex < 1 || roleIndex > validRoles.Length)
        {
            Console.WriteLine("Invalid role selection.");
            return;
        }

        string role = validRoles[roleIndex - 1];

        if (_viewModel.CreateNewList(name, role))
        {
            Console.WriteLine($"List '{name}' for role '{role}' created successfully.");
        }
        else
        {
            Console.WriteLine("Failed to create list.");
        }
    }

    private void AddChampionToList()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a new list first.");
            return;
        }

        var lists = _viewModel.GetAllLists();
        Console.WriteLine("Available Lists:");
        for (int i = 0; i < lists.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lists[i].Name} ({lists[i].Role})");
        }

        Console.Write("\nSelect list number: ");
        if (!int.TryParse(Console.ReadLine(), out int listIndex) || listIndex < 1 || listIndex > lists.Count)
        {
            Console.WriteLine("Invalid list selection.");
            return;
        }

        Console.WriteLine("Enter champion names to add (separate multiple champions with commas):");
        string input = Console.ReadLine().Trim();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Champion name(s) cannot be empty.");
            return;
        }

        string[] championNames = input.Split(',').Select(name => name.Trim()).Where(name => !string.IsNullOrWhiteSpace(name)).ToArray();

        if (_viewModel.AddChampionsToList(listIndex - 1, championNames))
        {
            Console.WriteLine($"Champion(s) added to list '{lists[listIndex - 1].Name}'.");
        }
        else
        {
            Console.WriteLine("No new champions were added to the list.");
        }
    }

    private void RemoveChampionFromList()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a new list first.");
            return;
        }

        var lists = _viewModel.GetAllLists();
        Console.WriteLine("Available Lists:");
        for (int i = 0; i < lists.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lists[i].Name} ({lists[i].Role})");
        }

        Console.Write("\nSelect list number: ");
        if (!int.TryParse(Console.ReadLine(), out int listIndex) || listIndex < 1 || listIndex > lists.Count)
        {
            Console.WriteLine("Invalid list selection.");
            return;
        }

        var selectedList = lists[listIndex - 1];

        if (selectedList.Champions.Count == 0)
        {
            Console.WriteLine($"No champions in list '{selectedList.Name}'.");
            return;
        }

        Console.WriteLine($"\nChampions in {selectedList.Name}:");
        for (int i = 0; i < selectedList.Champions.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {selectedList.Champions[i].Name}");
        }

        Console.Write("\nSelect champion number to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int championIndex) || championIndex < 1 || championIndex > selectedList.Champions.Count)
        {
            Console.WriteLine("Invalid champion selection.");
            return;
        }

        string removedChampion = selectedList.Champions[championIndex - 1].Name;

        if (_viewModel.RemoveChampionFromList(listIndex - 1, championIndex - 1))
        {
            Console.WriteLine($"Champion '{removedChampion}' removed from list '{selectedList.Name}'.");
        }
        else
        {
            Console.WriteLine("Failed to remove champion.");
        }
    }

    private void RandomChampionByRole()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a list first.");
            return;
        }

        string[] validRoles = _viewModel.GetValidRoles();
        Console.WriteLine("Available Roles:");
        for (int i = 0; i < validRoles.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {validRoles[i]}");
        }

        Console.Write("\nSelect role number: ");
        if (!int.TryParse(Console.ReadLine(), out int roleIndex) || roleIndex < 1 || roleIndex > validRoles.Length)
        {
            Console.WriteLine("Invalid role selection.");
            return;
        }

        string selectedRole = validRoles[roleIndex - 1];
        Champion randomChampion = _viewModel.GetRandomChampionByRole(selectedRole);

        if (randomChampion != null)
        {
            Console.WriteLine($"\n - Random champion for {selectedRole}: {randomChampion.Name} - ");
        }
        else
        {
            Console.WriteLine($"No champions found for role '{selectedRole}'.");
        }
    }

    private void RandomChampionFromList()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a list first.");
            return;
        }

        var lists = _viewModel.GetAllLists();
        Console.WriteLine("Available Lists:");
        for (int i = 0; i < lists.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lists[i].Name} ({lists[i].Role})");
        }

        Console.Write("\nSelect list number: ");
        if (!int.TryParse(Console.ReadLine(), out int listIndex) || listIndex < 1 || listIndex > lists.Count)
        {
            Console.WriteLine("Invalid list selection.");
            return;
        }

        var selectedList = lists[listIndex - 1];
        Champion randomChampion = _viewModel.GetRandomChampionFromList(listIndex - 1);

        if (randomChampion != null)
        {
            Console.WriteLine($"\n - Random champion from '{selectedList.Name}': {randomChampion.Name} - ");
        }
        else
        {
            Console.WriteLine($"No champions in list '{selectedList.Name}'. Add champions first.");
        }
    }

    private void DeleteList()
    {
        if (!_viewModel.HasLists())
        {
            Console.WriteLine("No lists found. Create a new list first.");
            return;
        }

        var lists = _viewModel.GetAllLists();
        Console.WriteLine("Available Lists:");
        for (int i = 0; i < lists.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {lists[i].Name} ({lists[i].Role})");
        }

        Console.Write("\nSelect list number to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int listIndex) || listIndex < 1 || listIndex > lists.Count)
        {
            Console.WriteLine("Invalid list selection.");
            return;
        }

        string deletedListName = lists[listIndex - 1].Name;

        if (_viewModel.DeleteList(listIndex - 1))
        {
            Console.WriteLine($"List '{deletedListName}' deleted successfully.");
        }
        else
        {
            Console.WriteLine("Failed to delete list.");
        }
    }
}