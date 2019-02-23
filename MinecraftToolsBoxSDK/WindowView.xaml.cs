using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MinecraftToolsBox
{
    /// <summary>
    /// WindowView.xaml 的交互逻辑
    /// </summary>
    public partial class WindowView : Grid
    {
        public MtbWindow Window;

        public WindowView(ImageSource Icon, string Title, MtbWindow window)
        {
            InitializeComponent();
            icon.Source = Icon;
            title.Text = Title;
            Window = window;
            shot.Fill = new VisualBrush { Visual = window };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window.Visibility = Visibility.Visible;
            Canvas container = Window.Parent as Canvas;
            if (container != null)
            {
                container.Children.Remove(Window);
                container.Children.Add(Window);
            }
        }
    }
}
