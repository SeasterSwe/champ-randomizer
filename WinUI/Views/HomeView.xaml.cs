using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;

namespace WinUI.Views;

public sealed partial class HomeView : Page, INotifyPropertyChanged
{
    private const double ItemWidth = 236; // 220 + 16 margin (8 on each side)
    private int _columns = 1;

    public int Columns
    {
        get => _columns;
        set
        {
            if (_columns != value)
            {
                _columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }
    }

    public HomeView()
    {
        this.InitializeComponent();
        this.SizeChanged += OnSizeChanged;
        // Calculate initial columns
        Loaded += (s, e) => CalculateColumns();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        CalculateColumns();
    }

    private void CalculateColumns()
    {
        var availableWidth = ActualWidth;
        if (availableWidth > 0)
        {
            Columns = Math.Max(1, (int)(availableWidth / ItemWidth));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void RandomChampionButton_Click(object sender, RoutedEventArgs e)
    {
        // Find the ChampionList for the clicked button
        if (sender is Button button && button.DataContext is WinUI.Models.ChampionList list && list.Champions?.Count > 0)
        {
            var random = new Random();
            var champion = list.Champions[random.Next(list.Champions.Count)];
            string champName = champion?.Name ?? "No champion found";

            // Show a simple dialog with the random champion's name
            var dialog = new ContentDialog
            {
                Title = "Random Champion",
                Content = champName,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
        else
        {
            var dialog = new ContentDialog
            {
                Title = "Random Champion",
                Content = "No champion found",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = dialog.ShowAsync();
        }
    }
}