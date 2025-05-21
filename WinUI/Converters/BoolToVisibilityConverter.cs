using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUI.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            if (parameter is string paramString && paramString == "IsCurrentEditingList")
            {
                // Special case for checking if this is the current editing list
                var list = value as WinUI.Models.ChampionList;
                var viewModel = App.Current.Resources["ManageViewModel"] as WinUI.ViewModels.ManageViewModel;

                if (list != null && viewModel != null && viewModel.IsEditingList && list == viewModel.CurrentEditingList)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }

            return boolValue ? Visibility.Visible : Visibility.Collapsed;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility visibility)
        {
            return visibility == Visibility.Visible;
        }

        return false;
    }
}