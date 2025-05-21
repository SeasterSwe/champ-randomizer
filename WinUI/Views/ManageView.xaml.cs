using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using WinUI.ViewModels;

namespace WinUI.Views;

public sealed partial class ManageView : Page
{
    public ManageView()
    {
        this.InitializeComponent();
        this.DataContext = new ManageViewModel();
    }

    private void NewChampionTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            // Call the ViewModel method to add the champion
            if (DataContext is ManageViewModel vm)
            {
                vm.AddTempChampionOnEnter();
            }

            // Mark the event as handled to prevent default behavior
            e.Handled = true;
        }
    }
}