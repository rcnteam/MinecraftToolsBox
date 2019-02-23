using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ScoreboardPlayers.xaml 的交互逻辑
    /// </summary>
    public partial class ScoreboardPlayers : DockPanel, ICommandGenerator
    {
        string oper="+= ";
        bool stop = false;
        CommandsGeneratorTemplate CmdGenerator;

        public ScoreboardPlayers(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            CmdGenerator = cmdGenerator;
        }

        private void Ope_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ope.SelectedIndex)
            {
                case 0: if (tip != null) tip.Content = "增加X分数，X=X+Y"; oper = "+= "; break;
                case 1: if (tip != null) tip.Content = "减少X分数，X=X-Y"; oper = "-= "; break;
                case 2: if (tip != null) tip.Content = "设置X的分数为X与Y的积，X=X×Y"; oper = "*= "; break;
                case 3: if (tip != null) tip.Content = "设置X的分数为X与Y的商，X=X÷Y"; oper = "/= "; break;
                case 4: if (tip != null) tip.Content = "设置X的分数为X与Y的商的余数"; oper = "%= "; break;
                case 5: if (tip != null) tip.Content = "设置X的分数为Y"; oper = "="; break;
                case 6: if (tip != null) tip.Content = "比较X与Y的大小，设置X为较小值"; oper = "< "; break;
                case 7: if (tip != null) tip.Content = "比较X与Y的大小，设置X为较大值"; oper = "> "; break;
                case 8: if (tip != null) tip.Content = "交换X和Y的值"; oper = "><"; break;
            }
        }

        private void GetTarget(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            if (b == get1) p1.Text = Target.GetEntity();
            else p2.Text = Target.GetEntity();
        }

        public string GenerateCommand()
        {
            switch (menu.SelectedIndex)
            {
                case 0:
                    return "/scoreboard players list " + Target.GetEntity();
                case 1:
                    return "/scoreboard players enable " + Target.GetEntity() + " " + board1.Text;
                case 2:
                    if (add.IsChecked == true)
                    {
                        if(Add.IsChecked==true) return "/scoreboard players add "+Target.GetEntity()+" "+board2.Text+" "+score.Value;
                        else return "/scoreboard players remove " + Target.GetEntity() + " " + board2.Text + " " + score.Value;
                    }
                    if (set.IsChecked == true) return "/scoreboard players set " + Target.GetEntity() + " " + board2.Text + " " + value.Value;
                    return "/";
                case 3:
                    return "/scoreboard players test " + Target.GetEntity() + " " + board3.Text + " " + min.Value + " " + max.Value;
                case 4:
                    return "/scoreboard players operation " + p1.Text + " " + s1.Text + " " + oper + " " + p2.Text + " " + s2.Text;
                case 5:
                    if (addTag.IsChecked == true) return "/scoreboard players tag " + Target.GetEntity() + " add " + addTagName.Text;
                    else return "/scoreboard players tag " + Target.GetEntity() + " remove " + removeTagName.Text;
            }
            return "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)e.Source;
            if (b == list) CmdGenerator.AddCommand("/scoreboard players list");
            if (b == reset1) CmdGenerator.AddCommand("/scoreboard players reset " + Target.GetEntity());
            if (b == reset2) CmdGenerator.AddCommand("/scoreboard players reset " + Target.GetEntity() + " " + board2.Text);
            if (b == tagList) CmdGenerator.AddCommand("/scoreboard players tag " + Target.GetEntity() + " list");
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }
        private void Checked(object sender, RoutedEventArgs e)
        {
            if (set == null || add == null || stop) return;
            stop = true;
            set.IsChecked = false;
            add.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }
        private void Checked2(object sender, RoutedEventArgs e)
        {
            if (removeTag == null || addTag == null || stop) return;
            stop = true;
            removeTag.IsChecked = false;
            addTag.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }

    }
}
