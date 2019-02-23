using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// SetBlocks.xaml 的交互逻辑
    /// </summary>
    public partial class SetBlocks : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate CmdGenerator;
        public Brush backColor { get; }
        string itemIdx = "BlocksB";
        public SetBlocks(CommandsGeneratorTemplate cmdGenerator)
        {
            backColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            InitializeComponent();
            UpdateItemPick(BlocksB, null);
            LocA.SetForeground(Color.FromRgb(255, 255, 255));
            LocB.SetForeground(Color.FromRgb(255, 255, 255));
            clone_loc.SetForeground(Color.FromRgb(255, 255, 255));
            CmdGenerator = cmdGenerator;
        }
        private void UpdateItemPick(object sender, RoutedEventArgs e)
        {
            switch ((string)ITEM.Content)
            {
                case "建筑方块": BlocksB.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "装饰性方块": BlocksD.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "红石": Redstone.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "杂项/其它": Other_B.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
            }
            if (sender != BlocksB) { next.IsEnabled = false; last.IsEnabled = false; } else { next.IsEnabled = true; }

            Tile t = (Tile)sender;
            t.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/" + t.Name + ".png")) };//pack://application:,,,
            selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/" + t.Name + "S.png")) };
            ITEM.Content = t.ToolTip;
            tooltip(t.Name);
            itemIdx = t.Name;
        }
        private void page(object sender, RoutedEventArgs e)
        {
            if (sender == next)
            {
                next.IsEnabled = false; last.IsEnabled = true;
                selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/BlocksBS2.png")) };
                tooltip("BlocksB2");
            }
            else
            {
                last.IsEnabled = false; next.IsEnabled = true;
                selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/BlocksBS.png")) };
                tooltip("BlocksB");
            }
        }
        private void tooltip(string arg)
        {
            FileStream file = new FileStream("data/" + arg + ".txt", FileMode.Open);
            StreamReader read = new StreamReader(file, Encoding.UTF8);
            file.Seek(0, SeekOrigin.Begin);
            string d = read.ReadToEnd();
            string[] data = d.Replace("\r", "").Split('\n');
            int i = 0;
            for (; i < data.Length; i++)
            {
                string[] split = data[i].Split('=');
                Button b = (Button)selector.Children[i];
                b.IsEnabled = true;
                b.ToolTip = split[0] + "\nminecraft:" + split[1];
            }
            for (; i < 54; i++)
            {
                ((Button)selector.Children[i]).IsEnabled = false;
            }
            file.Close();
            GC.Collect();
        }
        private void getName(object sender, RoutedEventArgs e)
        {
            string tip = (string)((Button)sender).ToolTip;
            string[] split = tip.Split('\n');
            if (split[1] == "minecraft:redstone") BlockName.Text = split[0] + "minecraft:redstone_wire";
            BlockName.Text = split[1];
            string name = split[1].Substring(10);

            switch (itemIdx)
            {
                default: goto Dis;
                case "BlocksB":
                    switch (name)
                    {
                        default: goto Dis;
                        case "stone": updateMetaData(name, new string[] { "石头", "花岗岩", "磨制花岗岩", "闪长岩", "磨制闪长岩", "安山岩", "磨制安山岩" }, true); break;
                        case "dirt": updateMetaData(name, new string[] { "泥土", "砂土", "灰化土" }, true); break;
                        case "planks": updateMetaData(name, new string[] { "橡木木板", "云杉木板", "白桦木板", "丛林木板", "金合欢木板", "深色橡木木板" }, true); break;
                        case "sand": updateMetaData(name, new string[] { "沙子", "红沙" }, true); break;
                        case "log": updateMetaData(name, new string[] { "橡木", "云杉木", "白桦木", "丛林木" }, true); break;
                        case "sponge": updateMetaData(name, new string[] { "海绵", "湿海绵" }, true); break;
                        case "sandstone": updateMetaData(name, new string[] { "沙石", "錾制沙石", "平滑沙石" }, true); break;
                        case "wool": updateMetaData(name, new string[] { "羊毛", "橙色羊毛", "品红色羊毛", "淡蓝色羊毛", "黄色羊毛", "黄绿色羊毛", "粉红羊毛", "灰色羊毛", "淡灰色羊毛", "青色羊毛", "紫色羊毛", "蓝色羊毛", "棕色羊毛", "绿色羊毛", "红色羊毛", "黑色羊毛" }, true); break;
                        case "stone_slab": updateMetaData(name, new string[] { "石台阶", "沙石台阶", "圆石台阶", "砖台阶", "石砖台阶", "地狱砖台阶", "石英台阶" }, true); break;
                        case "stained_glass": updateMetaData(name, new string[] { "白色染色玻璃", "橙色染色玻璃", "品红色染色玻璃", "淡蓝色染色玻璃", "黄色染色玻璃", "黄绿色染色玻璃", "粉红染色玻璃", "灰色染色玻璃", "淡灰色染色玻璃", "青色染色玻璃", "紫色染色玻璃", "蓝色染色玻璃", "棕色染色玻璃", "绿色染色玻璃", "红色染色玻璃", "黑色染色玻璃" }, true); break;
                        case "stonebrick": updateMetaData(name, new string[] { "石砖", "苔石砖", "裂石砖", "錾制石砖" }, true); break;
                        case "wooden_slab": updateMetaData(name, new string[] { "橡木台阶", "云杉台阶", "白桦台阶", "丛林台阶", "金合欢台阶", "深色橡木台阶" }, true); break;
                        case "cobblestone_wall": updateMetaData(name, new string[] { "圆石墙", "苔石墙" }, true); break;
                        case "quartz_block": updateMetaData(name, new string[] { "石英块", "錾制石英块", "竖纹石英块" }, true); break;
                        case "stained_hardened_clay": updateMetaData(name, new string[] { "白色染色粘土", "橙色染色粘土", "品红色染色粘土", "淡蓝色染色粘土", "黄色染色粘土", "黄绿色染色粘土", "粉红染色粘土", "灰色染色粘土", "淡灰色染色粘土", "青色染色粘土", "紫色染色粘土", "蓝色染色粘土", "棕色染色粘土", "绿色染色粘土", "红色染色粘土", "黑色染色粘土" }, true); break;
                        case "log2": updateMetaData(name, new string[] { "金合欢木", "深色橡木" }, true); break;
                        case "prismarine": updateMetaData(name, new string[] { "海晶石", "海晶石砖", "暗海晶石" }, true); break;
                        case "red_sandstone": updateMetaData(name, new string[] { "红沙石", "錾制红沙石", "平滑红沙石" }, true); break;
                    }
                    break;
                case "BlocksD":
                    switch (name)
                    {
                        default: goto Dis;
                        case "sapling": updateMetaData(name, new string[] { "橡树树苗", "云杉树苗", "白桦树苗", "丛林树苗", "金合欢树苗", "深色橡树树苗" }, true); break;
                        case "leaves": updateMetaData(name, new string[] { "橡树树叶", "云杉树叶", "白桦树叶", "丛林树叶" }, true); break;
                        case "tallgrass": updateMetaData(name, new string[] { "草", "蕨" }, true); break;
                        case "red_flower": updateMetaData(name, new string[] { "罂粟", "兰花", "绒球葱", "茜草花", "红色郁金香", "橙色郁金香", "白色郁金香", "粉红色郁金香", "滨菊" }, true); break;
                        case "monster_egg": updateMetaData(name, new string[] { "石头怪物蛋", "圆石怪物蛋", "石砖怪物蛋", "苔石砖怪物蛋", "裂石砖怪物蛋", "錾制石砖怪物蛋" }, true); break;
                        case "anvil": updateMetaData(name, new string[] { "铁砧", "轻微损坏的铁砧", "严重损坏的铁砧" }, true); break;
                        case "stained_glass_pane": updateMetaData(name, new string[] { "白色染色玻璃板", "橙色染色玻璃板", "品红色染色玻璃板", "淡蓝色染色玻璃板", "黄色染色玻璃板", "黄绿色染色玻璃板", "粉红染色玻璃板", "灰色染色玻璃板", "淡灰色染色玻璃板", "青色染色玻璃板", "紫色染色玻璃板", "蓝色染色玻璃板", "棕色染色玻璃板", "绿色染色玻璃板", "红色染色玻璃板", "黑色染色玻璃板" }, true); break;
                        case "leaves2": updateMetaData(name, new string[] { "金合欢树叶", "深色橡树树叶" }, true); break;
                        case "carpet": updateMetaData(name, new string[] { "地毯", "橙色地毯", "品红色地毯", "淡蓝色地毯", "黄色地毯", "黄绿色地毯", "粉红地毯", "灰色地毯", "淡灰色地毯", "青色地毯", "紫色地毯", "蓝色地毯", "棕色地毯", "绿色地毯", "红色地毯", "黑色地毯" }, true); break;
                        case "double_plant": updateMetaData(name, new string[] { "向日葵", "丁香", "高草丛", "大型蕨", "玫瑰丛", "牡丹" }, true); break;
                    }
                    break;
                case "Food":
                    switch (name)
                    {
                        default: goto Dis;
                        case "golden_apple": updateMetaData(name, new string[] { "金苹果", "附魔金苹果" }, true); break;
                        case "fish": updateMetaData(name, new string[] { "生鱼", "生鲑鱼", "小丑鱼", "河豚" }, true); break;
                        case "cooked_fish": updateMetaData(name, new string[] { "熟鱼", "熟鲑鱼" }, true); break;
                    }
                    break;
                case "Tools_Weapons":
                    switch (name)
                    {
                        default: goto Dis;
                        case "diamond_sword": updateMetaData(name, new string[] { "钻石剑\nminecraft:diamond_sword", "金剑\nminecraft:golden_sword", "铁剑\nminecraft:iron_sword", "石剑\nminecraft:stone_sword", "木剑\nminecraft:wooden_sword" }, false); break;
                        case "diamond_shovel": updateMetaData(name, new string[] { "钻石锹\nminecraft:diamond_shovel", "金锹\nminecraft:golden_shovel", "铁锹\nminecraft:iron_shovel", "石锹\nminecraft:stone_shovel", "木锹\nminecraft:wooden_shovel" }, false); break;
                        case "diamond_pickaxe": updateMetaData(name, new string[] { "钻石镐\nminecraft:diamond_pickaxe", "金镐\nminecraft:golden_pickaxe", "铁镐\nminecraft:iron_pickaxe", "石镐\nminecraft:stone_pickaxe", "木镐\nminecraft:wooden_pickaxe" }, false); break;
                        case "diamond_axe": updateMetaData(name, new string[] { "钻石斧\nminecraft:diamond_axe", "金斧\nminecraft:golden_axe", "铁斧\nminecraft:iron_axe", "石斧\nminecraft:stone_axe", "木斧\nminecraft:wooden_axe" }, false); break;
                        case "diamond_hoe": updateMetaData(name, new string[] { "钻石锄\nminecraft:diamond_hoe", "金锄\nminecraft:golden_hoe", "铁锄\nminecraft:iron_hoe", "石锄\nminecraft:stone_hoe", "木锄\nminecraft:wooden_hoe" }, false); break;
                        case "leather_helmet": updateMetaData(name, new string[] { "钻石头盔\nminecraft:diamond_helmet", "金头盔\nminecraft:golden_helmet", "铁头盔\nminecraft:iron_helmet", "锁链头盔\nminecraft:chainmail_helmet", "皮革帽子\nminecraft:leather_helmet\n可染色" }, false); break;
                        case "leather_chestplate": updateMetaData(name, new string[] { "钻石胸甲\nminecraft:diamond_chestplate", "金胸甲\nminecraft:golden_chestplate", "铁胸甲\nminecraft:iron_chestplate", "锁链胸甲\nminecraft:chainmail_chestplate", "皮革外套\nminecraft:leather_chestplate\n可染色" }, false); break;
                        case "leather_leggings": updateMetaData(name, new string[] { "钻石护腿\nminecraft:diamond_leggings", "金护腿\nminecraft:golden_leggings", "铁护腿\nminecraft:iron_leggings", "锁链护腿\nminecraft:chainmail_leggings", "皮革裤子\nminecraft:leather_leggings\n可染色" }, false); break;
                        case "leather_boots": updateMetaData(name, new string[] { "钻石靴子\nminecraft:diamond_boots", "金靴子\nminecraft:golden_boots", "铁靴子\nminecraft:iron_boots", "锁链靴子\nminecraft:chainmail_boots", "皮革靴子\nminecraft:leather_boots\n可染色" }, false); break;
                    }
                    break;
                case "Materials":
                    switch (name)
                    {
                        default: goto Dis;
                        case "coal": updateMetaData(name, new string[] { "煤炭", "木炭" }, true); break;
                        case "dye": updateMetaData(name, new string[] { "墨囊", "玫瑰红", "仙人掌绿", "可可豆", "青金石", "紫色染料", "青色染料", "淡灰色染料", "灰色染料", "粉红色染料", "黄绿色染料", "蒲公英黄", "淡蓝色染料", "品红色染料", "橙色染料", "骨粉" }, true); break;
                    }
                    break;
            }
            GC.Collect(); return;
            Dis:
            EnableMeta.Visibility = Visibility.Visible;
            MetaData.Background = null;
            for (int i = 0; i < 16; i++)
            {
                Button b = (Button)MetaData.Children[i];
                b.ToolTip = null;
                b.IsEnabled = false;
            }
            GC.Collect();
        }
        private void getMeta(object sender, RoutedEventArgs e)
        {
            string tooltip = (string)((Button)sender).ToolTip;
            tooltip = tooltip.Split('\n')[1];
            if (tooltip.Contains("Meta")) Meta.Value = int.Parse(tooltip.Substring(5));
            else BlockName.Text = tooltip;
        }
        private void updateMetaData(string path, string[] data, bool meta)
        {
            EnableMeta.Visibility = Visibility.Hidden;
            MetaData.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/Meta/" + path + ".png")) };
            int i = 0;
            for (; i < data.Length; i++)
            {
                Button b = (Button)MetaData.Children[i];
                b.IsEnabled = true;
                if (meta) b.ToolTip = data[i] + "\nMeta=" + i; else b.ToolTip = data[i];
            }
            for (; i < MetaData.Children.Count; i++) ((Button)MetaData.Children[i]).IsEnabled = false;
        }

        public string GenerateCommand()
        {
            int a = 0;
            if (a == 0) return "等待更新";// 当前页面仅1.13版本有效";
            if (setblock.IsChecked == true)
            {
                string s = "";
                if (Meta.Value != 0) s += " 0";
                if (setblock_method.SelectedIndex != 2)
                {
                    if (s == "") s += " 0";
                    if (setblock_method.SelectedIndex == 0) s += " destroy"; else s += " keep";
                }
                return "/setblock " + LocA.GetLocation() + " " + BlockName.Text + s;
            }else if (blockdata.IsChecked == true)
            {
                if (blockNBT.Text == "") return "请填写方块NBT！";
                string nbt = blockNBT.Text;
                string[] nbts = nbt.Split('-');
                if (nbts.Length != 3) return "错误的方块NBT数据！";
                return "/blockdata " + LocA.GetLocation() + " " + nbts[2];
            }else if (fill.IsChecked == true)
            {
                string nbt = fill_block1.Text;
                string[] nbts = nbt.Split('-');
                if(nbts.Length!=3)return "错误的方块NBT数据！";

                string method = "", info = "";
                switch (fill_method.SelectedIndex)
                {
                    case 0: method = nbts[1] + " destory"; if (nbts[2] != "{}") method += " " + nbts[2]; break;
                    case 1: method = nbts[1] + " hollow"; if (nbts[2] != "{}") method += " " + nbts[2]; break;
                    case 2: method = nbts[1] + " keep"; if (nbts[2] != "{}") method += " " + nbts[2]; break;
                    case 3: method = nbts[1] + " outline"; if (nbts[2] != "{}") method += " " + nbts[2]; break;
                    case 4: if (isReplace.IsChecked == true)
                        {
                            method = nbts[1] + " replace ";
                            string[] replace = replace_NBT.Text.Split('-');
                            if (replace.Length != 3) return "错误的方块NBT数据！";
                            method += replace[0];
                            if (replace[1] != "0") method += " " + replace[1];
                        }
                            else if (nbts[2] != "{}") info = nbts[1] + " replace " + nbts[2]; else if (nbts[1] != "0") info += nbts[1]; break;
                }
                return "/fill " + LocA.GetLocation() + " " + LocB.GetLocation() + " " + nbts[0]+" " + method + info;
            }
            else
            {
                string data = "",copy="",clone="";

                switch (copy_method.SelectedIndex)
                {
                    case 0: copy = " force"; break;
                    case 1: copy = " move"; break;
                    case 2: copy = " normal"; break;
                }
                switch (clone_method.SelectedIndex)
                {
                    case 0: clone = " filtered"; break;
                    case 1: clone = " masked"; break;
                    case 2: clone = " replace"; break;
                }
                if (copy != " normal") data = clone + copy; else if (clone != " replace") data = clone;
                if (clone == " filtered") if (TileName.Text == "") return "请填写方块信息！"; else data = clone + copy + " " + TileName.Text.Split('-')[0];
                return "/clone " + LocA.GetLocation() + " " + LocB.GetLocation() + " " + clone_loc.GetLocation() + data;
            }
        }

        private void isReplace_Click(object sender, RoutedEventArgs e)
        {
            if (isReplace.IsChecked == true) fill_block1.Width = 217; else fill_block1.Width = 498;
        }
        private void fill_method_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isReplace == null) return;
            if (sender == fill_method) if (fill_method.SelectedIndex != 4) { isReplace.IsChecked = false;isReplace.IsEnabled = false;isReplace_Click(null, null); } else { isReplace.IsEnabled = true; isReplace_Click(null, null); }
        }
        private void cmd_Changed(object sender, RoutedEventArgs e)
        {
            if (sender == setblock) LocB.IsEnabled = false; else LocB.IsEnabled = true;
        }
        private void generate(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand(BlockName.Text + "{Damage:" + Meta.Value + "}");
        }
    }
}