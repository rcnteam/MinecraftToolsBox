using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Scoreboard.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreboardObjective : VerticalSmoothScrollViewer, ICommandGenerator
    {
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;
        public ScoreboardObjective(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            CmdGenerator = cmdGenerator;
        }
        public string GenerateCommand()
        {
            string cmd = "/scoreboard objectives ";
            if (setdisplay.IsChecked == true)
            {
                cmd += "setdisplay ";
                ComboBoxItem s = (ComboBoxItem)slot.SelectedItem;
                string Slot = s.Name;
                if (s.Name == "sbt")
                {
                    ComboBoxItem c = (ComboBoxItem)dis_color.SelectedItem;
                    Slot = "sidebar.team." + s.Name;
                }
                return cmd + Slot +" "+display_tar.Text;
            }
            if (remove.IsChecked == true) return cmd+"remove "+ rem_tar.Text;
            if (add.IsChecked == true)
            {
                string criteria = "dummy";
                if (CRIT.IsChecked == true)
                {
                    ComboBoxItem cbi = (ComboBoxItem)crit.SelectedItem;
                    criteria = cbi.Name;
                }else{
                    if(ach.IsChecked == true)
                    {
                        ComboBoxItem cb = (ComboBoxItem)achievement.SelectedItem;
                        criteria = "achievement."+cb.Name;
                    }
                    if(stat.IsChecked==true)
                    {
                        ComboBoxItem cb = (ComboBoxItem)Stat.SelectedItem;
                        criteria = "stat."+cb.Name;
                    }
                    if (craft.IsChecked == true) criteria = "stat.craftItem.minecraft."+craftItem.Text;
                    if (use.IsChecked == true) criteria = "stat.useItem.minecraft." + useItem.Text;
                    if (_break.IsChecked == true) criteria = "stat.breakItem.minecraft." + breakItem.Text;
                    if (mine.IsChecked == true) criteria = "stat.mineBlock.minecraft." + mineBlock.Text;
                    if(entity.IsChecked == true)
                    {
                        if (killEntity.IsChecked == true) criteria = "stat.killEntity.";
                        if (entityKilledBy.IsChecked == true) criteria = "stat.entityKilledBy.";
                        ComboBoxItem cb = (ComboBoxItem)Entity.SelectedItem;
                        criteria += cb.Name;
                    }
                    if (team.IsChecked == true)
                    {
                        if (teamkill.IsChecked == true) criteria = "teamkill.";
                        if (killedByTeam.IsChecked == true) criteria = "killedByTeam.";
                        ComboBoxItem cb = (ComboBoxItem)color.SelectedItem;
                        criteria += cb.Name;
                    }
                }
                cmd += "add ";
                return cmd + name.Text + " " + criteria + " " + dis_name.Text;
            }
            return "";
        }

        private void Obj_list_Click(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand("/scoreboard objectives list");
        }
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (remove == null || add == null || setdisplay == null || stop) return;
            stop = true;
            remove.IsChecked = false;
            add.IsChecked = false;
            setdisplay.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
        private void Checked2(object sender, RoutedEventArgs e)
        {
            if (CRIT == null || ach == null || stat == null || craft == null || use == null || _break == null || mine == null || entity == null || team == null|| stop) return;
            stop = true;
            CRIT.IsChecked = false;
            ach.IsChecked = false;
            stat.IsChecked = false;
            craft.IsChecked = false;
            use.IsChecked = false;
            _break.IsChecked = false;
            mine.IsChecked = false;
            entity.IsChecked = false;
            team.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }

    }
}
