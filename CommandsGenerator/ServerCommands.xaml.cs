using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ServerCommands.xaml 的交互逻辑
    /// </summary>
    public partial class ServerCommands : HamburgerMenu, ICommandGenerator
    {
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;
        public ServerCommands(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            CmdGenerator = cmdGenerator;
        }
        private void TargetKind_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TargetKind.SelectedIndex == 0) { if (ip != null) ip.IsEnabled = false; if (IP != null) IP.IsEnabled = false; if (player != null) player.IsEnabled = true; if (Player != null) Player.IsEnabled = true; }
            else { if (ip != null) ip.IsEnabled = true; if (IP != null) IP.IsEnabled = true; if (player != null) player.IsEnabled = false; if (Player != null) Player.IsEnabled = false; }
        }
        private void Operation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Operation.SelectedIndex == 0)
            {
                if (reason != null) reason.IsEnabled = true;
                if (Reason != null) Reason.IsEnabled = true;
            }else{
                if (reason != null) reason.IsEnabled = false;
                if (Reason != null) Reason.IsEnabled = false;
            }
        }
        
        public string GenerateCommand()
        {
            switch (menu.SelectedIndex)
            {
                case 0:
                    string str = "";
                    if (Operation.SelectedIndex == 0)
                    {
                        if (TargetKind.SelectedIndex == 0) str = "/ban ";
                        else str = "/ban-ip ";
                    }
                    else
                    {
                        if (TargetKind.SelectedIndex == 0) str = "/pardon ";
                        else str = "/pardon-ip ";
                    }
                    switch (str)
                    {
                        case "/ban ": return str + Player.Text + " " + Reason.Text;
                        case "/ban-ip ": return str + IP.GetIP() + " " + Reason.Text;
                        case "/pardon ": return str + Player.Text;
                        case "/pardon-ip ": return str + IP.GetIP();
                    }
                    break;
                case 1:
                    string str2 = "/whitelist ";
                    if (Add.IsChecked == true) str2 += "add ";
                    else str2 += "remove ";
                    return str2 + WPlayer.Text;
                case 2:
                    string str3 = null;
                    if (OP_.IsChecked == true) str3 = "/op "; else str3 = "/deop ";
                    return str3 + OPPlayer.Text;
                case 3:
                    if (c1.IsChecked == true) return "/kick " + OPlayer.Text + " " + kickReason.Text;
                    else return "/setidletimeout " + idletime.Value;
            }
            return "";
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }
        private void AddCommand(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand((string)((Button)sender).ToolTip);
        }
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (c1==null || c2 == null || stop) return;
            stop = true;
            c1.IsChecked = false;
            c2.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
    }
}