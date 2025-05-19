using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RoleView : Page
{
    private string currentRole;

    public RoleView()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter is string role)
        {
            currentRole = role;
            RoleTitle.Text = $"{role} Role";

            // You can load role-specific content here based on the parameter
            LoadRoleContent(role);
        }
    }

    private void LoadRoleContent(string role)
    {
        // Placeholder method to load role-specific content
        // In a real application, you might fetch data or change the UI based on the role

        switch (role)
        {
            case "Top":
                RoleDescription.Text = "Top lane champions are typically tanky and can hold their own in a 1v1 situation.";
                break;

            case "Jungle":
                RoleDescription.Text = "Junglers roam the map, secure objectives, and help lanes that are struggling.";
                break;

            case "Mid":
                RoleDescription.Text = "Mid lane champions are usually mages or assassins with high burst damage.";
                break;

            case "Bot":
                RoleDescription.Text = "Bot lane carries deal sustained damage and become powerful in late game.";
                break;

            case "Support":
                RoleDescription.Text = "Supports help protect their teammates and provide utility to the team.";
                break;

            default:
                RoleDescription.Text = "Select a role to view more information.";
                break;
        }
    }
}