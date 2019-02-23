using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using MinecraftToolsBoxSDK;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// GetElement.xaml 的交互逻辑
    /// </summary>
    public partial class GetElement : Grid, ICommandGenerator
    {
        bool stop = false;
        public GetElement()
        {
            InitializeComponent();
            loc.SetForeground(Color.FromRgb(255, 255, 255));
            loc.SetDefaultValue();
            block_loc.SetForeground(Color.FromRgb(255, 255, 255));
            block_loc.SetDefaultValue();
        }

        public string GenerateCommand()
        {
            if (Give.IsChecked == true)
            {
                string NBT = give_NBT.Text;
                if (NBT == "") return "请填写NBT数据！";
                string[] s = { "tag:" };
                s = NBT.Split(s, StringSplitOptions.None);
                string[] info = s[0].Substring(1, s[0].Length - 1).Split(',');
                if (info.Length <= 3) return "物品NBT标签错误";
                string result = "/give " + ES.GetEntity() + " minecraft:" + info[0].Replace("id:", "").Replace("\"", "") + " " + info[1].Replace("Count:", "") + " " + info[2].Replace("Damage:", "");
                int i = info[0].Length + info[1].Length + info[2].Length + 8;
                string tag = NBT.Substring(i, NBT.Length - i - 1);
                if (tag != "{}") result += " " + tag;
                return result;
            }
            else if (Summon.IsChecked == true)
            {
                string NBT = summon_NBT.Text;
                if (NBT == "") return "请填写NBT数据！";
                string id = NBT.Split(',')[0];
                if (!id.Contains("id:")) return "错误的实体ID！";
                id =id.Replace("{", "").Replace("id:", "").Replace("}", "");
                if (id.Length ==0) return "错误的实体ID！";
                if (loc.GetLocation() == "") return "/summon " + id;
                NBT = " {" + NBT.Substring(id.Length + 5, NBT.Length - id.Length - 5);
                return "/summon " + id + " " + loc.GetLocation() + NBT;
            }
            else
            {
                string NBT = replace_NBT.Text;
                if (NBT == "") return "请填写NBT数据！";
                string[] s = { "tag:" };
                s = NBT.Split(s, StringSplitOptions.None);
                string[] info = s[0].Substring(1, s[0].Length - 1).Split(',');
                if (info.Length <= 3) return "物品NBT标签错误";
                string result = "minecraft:" + info[0].Replace("id:", "").Replace("\"", "") + " " + info[1].Replace("Count:", "") + " " + info[2].Replace("Damage:", "");
                int i = info[0].Length + info[1].Length + info[2].Length + 8;
                string tag = NBT.Substring(i, NBT.Length - i - 1);
                if (tag != "{}") result += " " + tag;

                string slot = "", target = "";
                switch (replaceMode.SelectedIndex)
                {
                    case 0: slot = "slot.horse.saddle"; break;
                    case 1: slot = "slot.horse.armor"; break;
                    case 2: switch (menu.SelectedIndex)
                        {
                            case 0: slot = (string)B1.ToolTip; break;
                            case 1: slot = (string)B2.ToolTip; break;
                            case 2: slot = (string)B3.ToolTip; break;
                        }
                        break;
                }
                switch (menu.SelectedIndex)
                {
                    case 0: target = "entity " + ES.GetEntity(); break;
                    case 1: target = "block " + block_loc.GetLocation(); break;
                    case 2: if (chest.IsChecked == true) target = "block " + block_loc.GetLocation(); else target = "entity " + ES.GetEntity(); break;
                }

                return "/replaceitem " + target + " " + slot + " " + result;
            }
        }
        private void GetSlot(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            Rectangle r = null;
            if (menu.SelectedIndex == 0) r = B1; else if (menu.SelectedIndex == 1) r = B2; else r = B3;
            r.Margin = b.Margin;
            r.ToolTip = b.ToolTip;
        }
        private void GRID_CLICK(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition((IInputElement)sender);
            int X = (int)Math.Floor(p.X / 36), Y = (int)Math.Floor(p.Y / 36);
            if (sender == I1)
            {
                B1.Margin = new Thickness(X * 36+46, Y * 36+9, 0, 0);
                B1.ToolTip = "slot.inventory." + (X + 9 * Y);
            }else if (sender == I2)
            {
                B1.Margin = new Thickness(X * 36 + 46, Y * 36 + 153, 0, 0);
                B1.ToolTip= "slot.hotbar." + (X + 9 * Y);
            }
            else if (sender == I3)
            {
                B2.Margin = new Thickness(X * 36 + 48, Y * 36 + 10, 0, 0);
                B2.ToolTip = "slot.container." + (X + 9 * Y);
            }
            else if (sender == I4)
            {
                B2.Margin = new Thickness(X * 36 + 10, Y * 36 + 80, 0, 0);
                B2.ToolTip = "slot.container." + (X + 9 * Y);
            }
            else if (sender == I5)
            {
                B3.Margin = new Thickness(X * 36 + 10, Y * 36 + 58, 0, 0);
                if (chest.IsChecked == true) B3.ToolTip = "slot.container." + (X + 9 * Y); else B3.ToolTip = "slot.enderchest." + (X + 9 * Y);
            }
        }

        private void Chest_Click(object sender, RoutedEventArgs e)
        {
            if (chest.IsChecked == false) B3.ToolTip=((string)B3.ToolTip).Replace("container", "enderchest");else B3.ToolTip = ((string)B3.ToolTip).Replace("enderchest", "container");
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (Give == null || Summon == null || Replaceitem == null || stop) return;
            stop = true;
            Give.IsChecked = false;
            Summon.IsChecked = false;
            Replaceitem.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }

    }
}
