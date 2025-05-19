using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using WinUI.Views;

namespace WinUI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();
        ContentFrame.Navigate(typeof(HomeView));
    }

    private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItem is NavigationViewItem selectedItem)
        {
            string tag = selectedItem.Tag as string ?? "Home";
            switch (tag)
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomeView));
                    break;

                case "Manage":
                    ContentFrame.Navigate(typeof(ManageView));
                    break;

                default:
                    break;
            }
        }
    }
}