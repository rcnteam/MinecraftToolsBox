using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Input;

namespace MinecraftToolsBox
{
    /// <summary>
    /// About.xaml 的交互逻辑
    /// </summary>
    public partial class About : VerticalSmoothScrollViewer
    {
        public About()
        {
            InitializeComponent();
        }
        private void OpenUrl(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start((sender as FrameworkElement).ToolTip as string);
        }
    }
}
