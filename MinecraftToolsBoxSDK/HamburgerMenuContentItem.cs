using MahApps.Metro.Controls;
using System.Windows;

namespace MinecraftToolsBoxSDK
{
    public class HamburgerMenuContentItem : HamburgerMenuItem
    {
        FrameworkElement _content;
        public FrameworkElement Content { get { return _content; } set { _content = value;_content.DataContext = Application.Current.MainWindow; } }
    }
}
