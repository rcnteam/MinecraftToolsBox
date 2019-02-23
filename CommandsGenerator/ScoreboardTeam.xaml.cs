using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ScoreboardTeam.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreboardTeam : DockPanel, ICommandGenerator
    {
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;
        public ScoreboardTeam(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            CmdGenerator = cmdGenerator;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)e.Source;
            if (btn == list) CmdGenerator.AddCommand("/scoreboard teams list");
            if (btn == Leave) CmdGenerator.AddCommand("/scoreboard teams leave");
        }
        public string GenerateCommand()
        {
            string cmd = "/scoreboard teams ";
            switch (menu.SelectedIndex)
            {
                case 0: return cmd+"list " + teamName1.Text;
                case 1:
                    if (remove.IsChecked == true) return cmd + "remove " + removeName.Text;
                    if (empty.IsChecked == true) return cmd + "empty " + emptyName.Text;
                    else return cmd + "add " + Tname.Text + " " + disName.Text;
                case 2:
                    if (join.IsChecked == true) return cmd + "join " + joinTeam.Text;
                    else return cmd + "leave " + leaveTeam.Text;
                case 3:
                    cmd += "option " + teamName.Text+" ";
                    if (color.IsChecked == true)
                    {
                        ComboBoxItem c = (ComboBoxItem)dis_color.SelectedItem;
                        return cmd + "color " + c.Name;
                    }
                    if (friendlyfire.IsChecked == true)
                    {
                        if (Enable.IsChecked == true) return cmd + "friendlyfire true";
                        else return cmd + "friendlyfire false";
                    }
                    if (seeFriendlyInvisibles.IsChecked == true)
                    {
                        return cmd + "seeFriendlyInvisibles "+Visible.IsChecked;
                    }
                    if (nametagVisibility.IsChecked == true)
                    {
                        ComboBoxItem c = (ComboBoxItem)pro1.SelectedItem;
                        return cmd + "nametagVisibility " + c.Content;
                    }
                    if (deathMessageVisibility.IsChecked == true)
                    {
                        string dmv = "";
                        switch (pro2.SelectedIndex)
                        {
                            case 0: dmv = "always"; break;
                            case 1: dmv = "hideForOtherTeams"; break;
                            case 2: dmv = "hideForOwnTeam"; break;
                            case 3: dmv = "never"; break;
                        }
                        return cmd + "deathMessageVisibility " + dmv;
                    } else {
                        string cr = "";
                        switch (pro3.SelectedIndex)
                        {
                            case 0: cr = "always"; break;
                            case 1: cr = "pushOwnTeam"; break;
                            case 2: cr = "pushOtherTeams"; break;
                            case 3: cr = "never"; break;
                        }
                        return cmd + "collisionRule " + cr;
                    }
            }
            return "";
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (remove == null || add == null || empty == null || join == null || leave == null || stop) return;
            stop = true;
            remove.IsChecked = false;
            add.IsChecked = false;
            empty.IsChecked = false;
            join.IsChecked = false;
            leave.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
        private void Checked2(object sender, RoutedEventArgs e)
        {
            if (color == null || nametagVisibility == null || deathMessageVisibility == null || collisionRule == null || stop) return;
            stop = true;
            color.IsChecked = false;
            nametagVisibility.IsChecked = false;
            deathMessageVisibility.IsChecked = false;
            collisionRule.IsChecked = false;
            seeFriendlyInvisibles.IsChecked = false;
            friendlyfire.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
    }
}
