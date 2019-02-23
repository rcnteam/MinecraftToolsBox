using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// PlayerCommands.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerCommands : DockPanel, ICommandGenerator
    {
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;
        public PlayerCommands(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();

            LocX_sp.setNeighbour(null, LocY_sp);
            LocY_sp.setNeighbour(LocX_sp, LocZ_sp);
            LocZ_sp.setNeighbour(LocY_sp, null);

            LocX.setNeighbour(null, LocY);
            LocY.setNeighbour(LocX, LocZ);
            LocZ.setNeighbour(LocY, null);
            CmdGenerator = cmdGenerator;
        }
        string Achievement()
        {
            string cmd = "/achievement ";
            if (give.IsChecked == true) cmd += "give "; else cmd += "take ";
            if (achi.IsChecked == true)
            {
                cmd += "achievement.";
                if (ach.SelectedIndex == 0) cmd = cmd.Substring(0, cmd.Length - 12) + "*";
                else cmd += ((ComboBoxItem)ach.SelectedItem).Name + " " + PlayerSelector.GetPlayer();
            }
            else cmd += "stat." + ((ComboBoxItem)Stat.SelectedItem).Name + " " + PlayerSelector.GetPlayer();
            return cmd;
        }

        public string GenerateCommand()
        {
            switch (menu.SelectedIndex)
            {
                case 0:
                    if (c1.IsChecked == true)
                    {
                        return "/xp " + exp.Value + " " + PlayerSelector.GetPlayer();
                    }
                    else if (c2.IsChecked == true)
                    {
                        if (addL.IsChecked == true) return "/xp " + levels.Value + "L " + PlayerSelector.GetPlayer();
                        else return "/xp " + "-" + levels.Value + "L " + PlayerSelector.GetPlayer();
                    }
                    else if (c3.IsChecked == true) return "/gamemode " + game_mode.SelectedIndex;
                    else
                    {
                        string loc2 = " ";
                        if (tilde_Copy.IsChecked == true) { loc2 = "~" + LocX_sp.Text + " ~" + LocY_sp.Text + " ~" + LocZ_sp.Text; }
                        else { loc2 = LocX_sp.Text + " " + LocY_sp.Text + " " + LocZ_sp.Text; }
                        return "/spawnpoint " + PlayerSelector.GetPlayer() + loc2;
                    }
                case 1:
                    if (isPrivate.IsChecked == true) return "/tell " + PlayerSelector.GetPlayer() + " " + msg.Text;
                    else return "/say " + msg.Text;
                case 2: return Achievement();
                case 3:
                    string loc = "";
                    if (tilde.IsChecked == true) { loc = "~" + LocX.Text + " ~" + LocY.Text + " ~" + LocZ.Text; }
                    else { loc = LocX.Text + " " + LocY.Text + " " + LocZ.Text; }
                    return "/playsound " + sound.Text + " " + PlayerSelector.GetPlayer() + " " + loc + " " + volume.Value + " " + tune.Value + " " + min_volume.Value;
                case 4:
                    TreeViewItem e = (TreeViewItem)enchant.SelectedItem;
                    if (e == en1 || e == en2 || e == en3 || e == en4 || e == en5 || e == en6 || e == null) return "";
                    return "/enchant " + PlayerSelector.GetPlayer() + " " + e.Name + " " + en_level.Value;
            }
            return "";
        }
        private void Playsound_search(object sender, RoutedEventArgs e)
        {
            result.Items.Clear();
            result.SelectedIndex = -1;
            string keyword = sound.Text;
            FileStream data = new FileStream("data/sound_data.txt", FileMode.Open);
            StreamReader streamReader = new StreamReader(data, Encoding.Default);
            streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string strLine = streamReader.ReadLine();
            do
            {
                string[] split = strLine.Split('=');
                if (split[1].Contains(keyword)) result.Items.Add(split[0]+"("+split[1]+")");
                strLine = streamReader.ReadLine();

            } while (strLine != null && strLine != "");
            streamReader.Close();
            streamReader.Dispose();
            data.Close();
            data.Dispose();
        }

        private void Level_min_Click(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand("/xp -2147483648L " + PlayerSelector.GetPlayer());
        }
        private void Sp1(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand("/spawnpoint");
        }

        private void Sp2(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand("/spawnpoint "+PlayerSelector.GetPlayer());
        }
        
        private void AddTarget_Click(object sender, RoutedEventArgs e)
        {
            msg.Text += " " + PlayerSelector.GetPlayer() + " ";
        }

        private void Select_sound(object sender, SelectionChangedEventArgs e)
        {
            string res = (string)result.SelectedItem;
            if (res == null) return;
            res = res.Split('(',')')[1];
            sound.Text = res;
        }

        private void ACHIEVEMENT(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            name = name.Replace("_", "");
            ach.SelectedIndex = int.Parse(name);
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (c1 == null || c2 == null || c3 == null || c4 == null || stop) return;
            stop = true;
            c1.IsChecked = false;
            c2.IsChecked = false;
            c3.IsChecked = false;
            c4.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
    }
}
