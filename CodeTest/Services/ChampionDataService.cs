using ChampionListManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeTest.Services;

public class ChampionDataService
{
    private readonly string _dataFilePath;

    public ChampionData Data { get; private set; }
    public static readonly string[] ValidRoles = { "Jungle", "Mid", "Adc", "Support", "Top" };

    public ChampionDataService(string dataFilePath = "championLists.json")
    {
        _dataFilePath = dataFilePath;
        Data = new ChampionData();
    }

    public bool LoadData()
    {
        if (File.Exists(_dataFilePath))
        {
            try
            {
                string jsonData = File.ReadAllText(_dataFilePath);
                Data = JsonSerializer.Deserialize<ChampionData>(jsonData) ?? new ChampionData();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
                Data = new ChampionData();
                return false;
            }
        }
        return false;
    }

    public bool SaveData()
    {
        try
        {
            string jsonData = JsonSerializer.Serialize(Data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataFilePath, jsonData);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving data: {ex.Message}");
            return false;
        }
    }

    public ChampionList GetList(int index)
    {
        if (index >= 0 && index < Data.Lists.Count)
        {
            return Data.Lists[index];
        }
        throw new ArgumentOutOfRangeException(nameof(index), "Invalid list index");
    }

    public void DeleteList(int index)
    {
        if (index >= 0 && index < Data.Lists.Count)
        {
            Data.Lists.RemoveAt(index);
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Invalid list index");
        }
    }
}