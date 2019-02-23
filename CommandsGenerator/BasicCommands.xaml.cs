using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// BasicCommands.xaml 的交互逻辑
    /// </summary>
    public partial class BasicCommands : HamburgerMenu, ICommandGenerator
    {
        CommandsGeneratorTemplate CmdGenerator;
        BitmapImage img1 = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/peaceful.png"));
        BitmapImage img2 = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/easy.png"));
        BitmapImage img3 = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/normal.png"));
        BitmapImage img4 = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/hard.png"));
        bool stop = false;
        public BasicCommands(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            view.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/particle/0.png"));
            CmdGenerator = cmdGenerator;
        }

        public string GenerateCommand()
        {
            switch (menu.SelectedIndex)
            {
                case 0:
                    if (Set.IsChecked == true) return "/time set " + Time1.Value;
                    else if (add.IsChecked == true) return "/time add " + Time1.Value;
                    else if (c2.IsChecked == true)
                        switch (query.Text)
                        {
                            default: return null;
                            case "今天时间": return "/time quary daytime";
                            case "游戏总时间": return "/time quary gametime";
                            case "游戏天数": return "/time quary day";
                        }
                    else if (c3.IsChecked == true)
                        switch (weather.SelectedIndex)
                        {
                            default: return null;
                            case 0: return "/weather clear " + time.Value;
                            case 1: return "/weather rain " + time.Value;
                            case 2: return "/weather thunder " + time.Value;
                        }
                    else if (c4.IsChecked == true) return "/difficulty " + difficulty.SelectedIndex;
                    else return "/setworldspawn " + LOC.GetLocation();
                case 1:
                    if (c5.IsChecked == true) return "/defaultgamemode " + gamemode.SelectedIndex;
                    string str = "/gamerule ";
                    string TF = "";
                    switch (gamerules.SelectedIndex)
                    {
                        case 1: return str + "randomTickSpeed " + RandomTick.Value;
                        case 2: return str + "spawnRadius " + Distance.Value;
                    }
                    if (isEnable.IsChecked == true) TF = "true";
                    else TF = "false";
                    switch (gamerules.SelectedIndex)
                    {
                        case 0: return str + "commandBlockOutput " + TF;
                        case 3: return str + "disableElytraMovementCheck " + TF;
                        case 4: return str + "doDaylightCycle " + TF;
                        case 5: return str + "doEntityDrops " + TF;
                        case 6: return str + "doFireTick " + TF;
                        case 7: return str + "doMobLoot " + TF;
                        case 8: return str + "doMobSpawning " + TF;
                        case 9: return str + "doTileDrops " + TF;
                        case 10: return str + "keepInventory " + TF;
                        case 11: return str + "logAdminCommands " + TF;
                        case 12: return str + "mobGriefing " + TF;
                        case 13: return str + "naturalRegeneration " + TF;
                        case 14: return str + "reducedDebugInfo " + TF;
                        case 15: return str + "sendCommandFeedback " + TF;
                        case 16: return str + "showDeathMessages " + TF;
                        case 17: return str + "spectatorsGenerateChunks " + TF;
                    }
                    break;
                case 2:
                    string ptl = "";
                    string mode = "";
                    if (particle_mode.SelectedIndex == 0) mode = "normal"; else mode = "force";
                    switch (particle.SelectedIndex)
                    {
                        case 0: ptl = "explode"; break;
                        case 1: ptl = "largeexplode"; break;
                        case 2: ptl = "hugeexplosion"; break;
                        case 3: ptl = "fireworksSpark"; break;
                        case 4: ptl = "bubble"; break;
                        case 5: ptl = "splash"; break;
                        case 6: ptl = "wake"; break;
                        case 7: ptl = "suspended"; break;
                        case 8: ptl = "depthsuspend"; break;
                        case 9: ptl = "crit"; break;
                        case 10: ptl = "magicCrit"; break;
                        case 11: ptl = "smoke"; break;
                        case 12: ptl = "largesmoke"; break;
                        case 13: ptl = "spell"; break;
                        case 14: ptl = "instantSpell"; break;
                        case 15: ptl = "mobSpell"; break;
                        case 16: ptl = "mobSpellAmbient"; break;
                        case 17: ptl = "witchMagic"; break;
                        case 18: ptl = "dripWater"; break;
                        case 19: ptl = "dripLava"; break;
                        case 20: ptl = "angryVillager"; break;
                        case 21: ptl = "happyVillager"; break;
                        case 22: ptl = "townaura"; break;
                        case 23: ptl = "note"; break;
                        case 24: ptl = "portal"; break;
                        case 25: ptl = "enchantmenttable"; break;
                        case 26: ptl = "flame"; break;
                        case 27: ptl = "lava"; break;
                        case 28: ptl = "footstep"; break;
                        case 29: ptl = "reddust"; break;
                        case 30: ptl = "snowballpoof"; break;
                        case 31: ptl = "slime"; break;
                        case 32: ptl = "heart"; break;
                        case 33: ptl = "barrier"; break;
                        case 34: ptl = "cloud"; break;
                        case 35: ptl = "snowshovel"; break;
                        case 36: ptl = "droplet"; break;
                        case 37: ptl = "mobapperance"; break;
                        case 38: ptl = "endRod"; break;
                        case 39: ptl = "dragonbreath"; break;
                        case 40: ptl = "damageIndicator"; break;
                        case 41: ptl = "sweepAttack"; break;
                        case 42: ptl = "fallingdust"; break;
                    }
                    return "/particle " + ptl + " " + loc.GetLocation() + " " + dloc.GetLocation() + " " + speed.Value + " " + amount.Value + " " + mode;
                case 3:
                    string cmd = "/worldborder ";
                    if (bordersize.IsChecked == true)
                        if (add_border.IsChecked == true) return cmd + "add " + size.Value + " " + change_time.Value;
                        else return cmd + "set " + size.Value + " " + change_time.Value;
                    if (center.IsChecked == true)
                    {
                        string loc = "";
                        if (tilde.IsChecked == true) loc = "~" + center_X.Value + " ~" + center_Z.Value;
                        else loc = center_X.Value + " " + center_Z.Value;
                        return cmd + "center " + loc;
                    }
                    if (damage.IsChecked == true)
                    {
                        string dmg_arg = "";
                        if (damage_amount.IsChecked == true) dmg_arg = "damage amount ";
                        else dmg_arg = "damage buffer ";
                        return cmd + dmg_arg + damage_arg.Value;
                    }
                    if (warn.IsChecked == true)
                    {
                        string warn_arg = "";
                        if (warn_distance.IsChecked == true) warn_arg = "warning distance ";
                        else warn_arg = "warning time ";
                        return cmd + warn_arg + warning_arg.Value;
                    }
                    break;
                case 4:
                    if (page.IsChecked == true) return "/help " + (Page.SelectedIndex + 1);
                    if (command.IsChecked == true) return "/help " + Command.Text;
                    break;
            }
            return "";
        }
        private void NumberOnly(object sender, KeyEventArgs e)//文本框只能输入数字
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)))
            {
                e.Handled = true;
            }
        }
        private void Difficulty_SelectionChanged(object sender, SelectionChangedEventArgs e)//难度选项卡-提示信息
        {
            if (DIFFIMG != null)
                switch (difficulty.SelectedIndex)
                {
                    case 0: DIFFIMG.Source = img1; break;
                    case 1: DIFFIMG.Source = img2; break;
                    case 2: DIFFIMG.Source = img3; break;
                    case 3: DIFFIMG.Source = img4; break;
                }
        }
        private void Gamerules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gamerules.SelectedIndex == 2)
            {
                distance.IsEnabled = true;
                Distance.IsEnabled = true;
                randomTick.IsEnabled = false;
                RandomTick.IsEnabled = false;
                isEnable.IsEnabled = false;
                Enable.IsEnabled = false;
                return;
            }
            if (gamerules.SelectedIndex == 1)
            {
                randomTick.IsEnabled = true;
                RandomTick.IsEnabled = true;
                distance.IsEnabled = false;
                Distance.IsEnabled = false;
                isEnable.IsEnabled = false;
                Enable.IsEnabled = false;
                return;
            }
            else
            {
                if (randomTick != null) randomTick.IsEnabled = false;
                if (RandomTick != null) RandomTick.IsEnabled = false;
                if (distance != null) distance.IsEnabled = false;
                if (Distance != null) Distance.IsEnabled = false;
                if (isEnable != null) isEnable.IsEnabled = true;
                if (Enable != null) Enable.IsEnabled = true;
            }
        }    
        private void Particle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (view != null) view.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/particle/" + particle.SelectedIndex + ".png"));
        }

        private void AddCommand(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand((string)((Button)sender).ToolTip);
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (Set == null || add == null || c2 == null || c3 == null || c4 == null || stop) return;
            stop = true;
            Set.IsChecked = false;
            add.IsChecked = false;
            c2.IsChecked = false;
            c3.IsChecked = false;
            c4.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
        private void Checked2(object sender, RoutedEventArgs e)
        {
            if (c5 == null || c6 == null || stop) return;
            stop = true;
            c5.IsChecked = false;
            c6.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
        private void Checked3(object sender, RoutedEventArgs e)
        {
            if (bordersize == null || center == null || warn == null || damage == null || stop) return;
            stop = true;
            bordersize.IsChecked = false;
            center.IsChecked = false;
            warn.IsChecked = false;
            damage.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
    }
}