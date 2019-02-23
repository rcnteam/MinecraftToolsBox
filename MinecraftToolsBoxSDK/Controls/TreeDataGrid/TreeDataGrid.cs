using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftToolsBoxSDK
{
    public class TreeDataGrid : DataGrid
    {
        private const string subItem = "M0,0 L0,29 M1,19 L12,19";
        private const string subItemLast = "M0,0 L0,23 M1,22 L12,22";

        public TreeDataGridModel Children { get; set; } = new TreeDataGridModel();

        static TreeDataGrid()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeDataGrid), new FrameworkPropertyMetadata(typeof(TreeDataGrid)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            ItemsSource = Children.FlatModel;
        }
    }

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If the item has children, then show the checkbox, otherwise hide it
            return ((bool)value ? Visibility.Visible : Visibility.Hidden);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class LevelConverter : IValueConverter
    {
        public GridLength LevelWidth { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return the width multiplied by the level
            return new Thickness(((int)value * LevelWidth.Value), 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
