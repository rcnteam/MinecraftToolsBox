using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// BasicCommands.xaml 的交互逻辑
    /// </summary>
    public partial class EntityCommands : DockPanel, ICommandGenerator
    {
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;
        public EntityCommands(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            LocX_sp.setNeighbour(null, LocZ_sp);
            LocZ_sp.setNeighbour(LocX_sp, null);

            LocX_tp.setNeighbour(null, LocY_tp);
            LocY_tp.setNeighbour(LocX_tp, LocZ_tp);
            LocZ_tp.setNeighbour(LocY_tp, null);
            CmdGenerator = cmdGenerator;
        }

        public string GenerateCommand()
        {
            switch (menu.SelectedIndex)
            {
                case 0: return "/kill " + EntitySelector.GetEntity();
                case 1: return Spreadplayers();
                case 2: return Tp();
                case 3: return GetEffect();
                case 4: return Me();
            }
            return "";
        }
        string Spreadplayers()
        {
            string center = "";
            string kt = "";
            if (sp_center_tilde.IsChecked == true) center = "~" + LocX_sp.Text + " ~" + LocZ_sp.Text;
                else center =LocX_sp.Text + " " + LocZ_sp.Text;
            if (keepTeam.IsChecked == true) kt = "true"; else kt = "false";
            if (area.Value < separation.Value) area.Value = separation.Value + 1;
            return "/spreadplayers " + center + " " + separation.Value + " " + area.Value + " " + kt + " " + EntitySelector.GetEntity();
        }
        string Tp()
        {
            if (c1.IsChecked == true) return "/tp " + EntitySelector.GetEntity() + " " + tp_tar.Text;
            else
            {
                string destination = "";
                string rotation = "";
                if (tp_tilde.IsChecked == true) destination = "~" + LocX_tp.Text + " ~" + LocY_tp.Text + " ~" + LocZ_tp.Text;
                    else destination = LocX_tp.Text + " " + LocY_tp.Text + " " + LocZ_tp.Text;
                if (tiled_angle.IsChecked == true) rotation = "~" + xrot.Value + " ~" + yrot.Value;
                    else rotation = xrot.Value + " " + yrot.Value;
                return "/tp " + EntitySelector.GetEntity() + " " + destination + " " + rotation;
            }
        }
        private string GetEffect()
        {
            string show = "";
            if (showParticle.IsChecked == true) show = " true"; else show = " false";
            TreeViewItem e = (TreeViewItem)effect.SelectedItem;
            if (e == eff1 || e == eff2 || e == eff3 || e == eff4 || e == eff5 || e == null) return "请选择效果";
            return "/effect " + EntitySelector.GetEntity()+" minecraft:"+e.Name+" "+time.Value+" "+level.Value+show;
        }
        private string Me()
        {
            return "/me " + me.Text;
        }       

        private void MeAdd_Click(object sender, RoutedEventArgs e)
        {
            int i = me.SelectionStart;
            string txt = " " + EntitySelector.GetEntity() + " ";
            me.SelectionLength = 0;
            me.SelectedText = txt;
        }
        private void MaxTime_Click(object sender, RoutedEventArgs e)
        {
            time.Value = 1000000;
        }

        private void Clear_Click(object sender, RoutedEventArgs e1)
        {
            string show = "";
            if (showParticle.IsChecked == true) show = " true"; else show = " false";
            TreeViewItem e = (TreeViewItem)effect.SelectedItem;
            if (e == eff1 || e == eff2 || e == eff3 || e == eff4 || e == eff5 || e == null) { CmdGenerator.AddCommand("请选择效果"); return; }
            CmdGenerator.AddCommand("/effect " + EntitySelector.GetEntity() + " minecraft:" + e.Name + " 0 " + level.Value + show);
        }

        private void EffectClear_Click(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand("/effect "+EntitySelector.GetEntity()+" clear");
        }

        private void Effect_change(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item =(TreeViewItem) effect.SelectedItem;
            if (item == eff1 || item == eff2 || item == eff3 || item == eff4 || item == eff5) return;
            string name = item.Name;
            effect_pre.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/effect/"+name+".png"));
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (c1 == null || c2 == null || stop) return;
            stop = true;
            c1.IsChecked = false;
            c2.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
    }
}