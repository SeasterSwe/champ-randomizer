using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace WinUI.Views;

public sealed partial class HomeView : Page, INotifyPropertyChanged
{
    private const double ItemWidth = 236; // 220 + 16 margin (8 on each side)
    private int _columns = 1;

    // Popup state
    private CancellationTokenSource? _popupCts;
    private Flyout? _popupFlyout;
    private TextBlock? _popupTextBlock;

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

    public event PropertyChangedEventHandler? PropertyChanged;

    public HomeView()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
        Loaded += (s, e) => CalculateColumns();
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e) => CalculateColumns();

    private void CalculateColumns()
    {
        var availableWidth = ActualWidth;
        if (availableWidth > 0)
        {
            Columns = Math.Max(1, (int)(availableWidth / ItemWidth));
        }
    }

    private void OnPropertyChanged(string propertyName)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    // Handles the random champion button click
    private void RandomChampionButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.DataContext is WinUI.Models.ChampionList list && list.Champions?.Count > 0)
        {
            string champName = GetRandomChampionName(list);
            ShowPopup(champName, button);
        }
        else
        {
            ShowPopup("No champion found", sender as FrameworkElement);
        }
    }

    // Gets a random champion name from the list
    private static string GetRandomChampionName(WinUI.Models.ChampionList list)
    {
        var random = new Random();
        var champion = list.Champions![random.Next(list.Champions.Count)];
        return champion?.Name ?? "No champion found";
    }

    // Shows a popup with the given message at the given anchor
    private async void ShowPopup(string message, FrameworkElement? anchor)
    {
        CancelPreviousPopup();

        EnsurePopupInitialized();

        _popupTextBlock!.Text = message;
        if (anchor != null)
        {
            _popupFlyout!.ShowAt(anchor);
        }

        try
        {
            await Task.Delay(5000, _popupCts!.Token);
            if (!_popupCts.Token.IsCancellationRequested)
            {
                _popupFlyout.Hide();
            }
        }
        catch (TaskCanceledException)
        {
            // Popup was replaced by a new one, ignore
        }
    }

    // Cancels any previous popup timer
    private void CancelPreviousPopup()
    {
        _popupCts?.Cancel();
        _popupCts = new CancellationTokenSource();
    }

    // Ensures the popup and its content are created
    private void EnsurePopupInitialized()
    {
        if (_popupFlyout == null)
        {
            _popupTextBlock = new TextBlock
            {
                FontSize = 16,
                Padding = new Thickness(12)
            };

            _popupFlyout = new Flyout
            {
                Content = _popupTextBlock,
                Placement = FlyoutPlacementMode.Bottom
            };
        }
    }
}