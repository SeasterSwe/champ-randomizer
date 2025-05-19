using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using WinUI.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI
{
    public sealed partial class MainWindow : Window
    {
        private List<NavigationViewItem> roleNavItems;

        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize navigation items
            InitializeNavigation();

            // Set the default view
            ContentFrame.Navigate(typeof(HomeView));
        }

        private void InitializeNavigation()
        {
            // Setup role navigation items
            var roles = new List<string> { "Top", "Jungle", "Mid", "Bot", "Support" };
            roleNavItems = roles.Select(role => new NavigationViewItem
            {
                Content = role,
                Tag = role
            }).ToList();

            // Add to the top navigation
            foreach (var item in roleNavItems)
            {
                TopNavView.MenuItems.Add(item);
            }
        }

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string tag = selectedItem.Tag as string;
                switch (tag)
                {
                    case "Home":
                        ContentFrame.Navigate(typeof(HomeView));
                        break;

                    case "ListManage":
                        ContentFrame.Navigate(typeof(ListManageView));
                        break;

                    default:
                        // For role navigation items
                        if (roleNavItems.Contains(selectedItem))
                        {
                            ContentFrame.Navigate(typeof(RoleView), tag);
                        }
                        break;
                }
            }
        }

        private void TopNavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                string tag = selectedItem.Tag as string;
                ContentFrame.Navigate(typeof(RoleView), tag);
            }
        }
    }
}