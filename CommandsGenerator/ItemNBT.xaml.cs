using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ItemNBT.xaml 的交互逻辑
    /// </summary>
    public partial class ItemNBT : Grid, ICommandGenerator
    {
        string itemIdx = "BlocksB";
        CommandsGeneratorTemplate CmdGenerator;

        public ItemNBT(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            Tooltip("BlocksB");
            UpdateItemPick(BlocksB, null);
            UpdateMetaData("stone", new string[] { "石头", "花岗岩", "磨制花岗岩", "闪长岩", "磨制闪长岩", "安山岩", "磨制安山岩" }, true);
            CmdGenerator = cmdGenerator;
        }
        public string GenerateNBT()
        {
            Properties(save, null);
            string Display = "", AttributeModifiers = "", CanDestroy = "", CanPlaceOn="", DATA = ""; int HideFlags = 0;
            if (never.IsChecked == true) DATA += "Age:-32768,"; else if (age.Value != 300) { DATA += "Age:" + Convert.ToInt32(6000 - age.Value * 20) + ","; }
            if (cantPick.IsChecked == true) DATA += "PickupDelay:32767,"; else if (delay.Value != 0)DATA += "PickupDelay:" + Convert.ToInt32(delay.Value * 20) + ",";
            if (hide1.IsChecked == true) HideFlags += 1; if (hide2.IsChecked == true) HideFlags += 2; if (hide4.IsChecked == true) HideFlags += 4; if (hide8.IsChecked == true) HideFlags += 8; if (hide16.IsChecked == true) HideFlags += 16; if (hide32.IsChecked == true) HideFlags += 32;
            if (HideFlags != 0) DATA += "HideFlags:" + HideFlags + ",";
            if (unBreak.IsChecked == true) DATA += "Unbreakable:1,";if (cost.Value != null) DATA += "RepairCost:" + (cost.Value - 1) + ",";

            if (IName.Text != "" || ILore.Text != "" || ENC.IsChecked == true)
            {
                Display += "display:{";
                if (IName.Text != "") Display += "Name:" + IName.Text + ",";
                if (ILore.Text != "") 
                {
                    Display += "Lore:[";
                    string[] split = ILore.Text.Split('\n');
                    for (int i = 0; i < split.LongLength; i++) Display += "\"" + split[i] + "\",";
                    Display = Display.Substring(0, Display.Length - 1);
                    Display += "],";
                    Display = Display.Replace("\r", "");
                }
                if (ENC.IsChecked == true) Display += "color:" + (Convert.ToInt16(color.SelectedColor.R) * 65536 + Convert.ToInt16(color.SelectedColor.G) * 256 + Convert.ToInt16(color.SelectedColor.B)) + ",";
                Display = Display.Substring(0, Display.Length - 1) + "},";
            }
            if (canBreak.Items.Count != 0)
            {
                CanDestroy += "CanDestroy:[";
                for (int i = 0; i < canBreak.Items.Count; i++) CanDestroy += "\"" + ((string)canBreak.Items[i]).Split('(', ')')[1].Replace("minecraft:", "") + "\",";
                CanDestroy = CanDestroy.Substring(0, CanDestroy.Length - 1) + "],";
            }
            if (canPlace.IsEnabled == true && canPlace.Items.Count != 0)
            {
                CanDestroy += "CanPlaceOn:[";
                for (int i = 0; i < canPlace.Items.Count; i++) CanDestroy += "\"" + ((string)canPlace.Items[i]).Split('(', ')')[1].Replace("minecraft:", "") + "\",";
                CanDestroy = CanDestroy.Substring(0, CanDestroy.Length - 1) + "],";
            }
            AttributeModifiers += GetAttributeModifiers(hand1, "mainhand");
            AttributeModifiers += GetAttributeModifiers(hand2, "offhand");
            AttributeModifiers += GetAttributeModifiers(onHead, "head");
            AttributeModifiers += GetAttributeModifiers(onBody, "chest");
            AttributeModifiers += GetAttributeModifiers(onLeg, "legs");
            AttributeModifiers += GetAttributeModifiers(onFeet, "feet");
            AttributeModifiers += GetAttributeModifiers(all, "");
            //全部应用
            if (AttributeModifiers.Length != 0) AttributeModifiers = "AttributeModifiers:[" + AttributeModifiers.Substring(0, AttributeModifiers.Length - 1) + "],";
            DATA += Display + AttributeModifiers + CanDestroy + CanPlaceOn + GetEnchant();
            if (DATA.Length != 0) DATA = DATA.Substring(0, DATA.Length - 1);

            string TileEntityData = "";
            if (cmd.IsEnabled) TileEntityData = "{BlockEntityTag:{Command:\"" + cmd.Text + "\"}";
            else if (signData.IsEnabled) TileEntityData = "{BlockEntityTag:{" + signData.Text + "}";
            else if (bookData.IsEnabled) TileEntityData = "," + bookData.Text.Substring(1, bookData.Text.Length - 2);
            else if (Beacon.IsEnabled)
            {
                int primary = 0, secondary = 0;
                if (e1.IsChecked == true) primary = 1;
                else if (e2.IsChecked == true) primary = 3;
                else if (e3.IsChecked == true) primary = 11;
                else if (e4.IsChecked == true) primary = 8;
                else if (e5.IsChecked == true) primary = 5;
                if (e6.IsChecked == true) secondary = 10;
                else if (e7.IsChecked == true) secondary = primary;
                if (primary != 0) TileEntityData += "Primary:" + primary;
                if (secondary != 0) TileEntityData += ",Secondary:" + secondary;
                if (BeaconLevel.Value != 1)
                    if (TileEntityData == "") TileEntityData += "Levels:" + BeaconLevel.Value + ","; else TileEntityData += "Levels:" + BeaconLevel.Value;
                TileEntityData = "BlockEntityTag:{" + TileEntityData + "}";
            }
            if (TileEntityData == "BlockEntityTag:{}") TileEntityData = "";

            return "{id:\"" + ItemName.Text.Replace("minecraft:", "") + "\",Count:" + count.Value + ",Damage:" + Meta.Value + ",tag:{" + DATA + TileEntityData + "}"  + "}";
        }
        public string GenerateCommand()
        {
            string c = GenerateNBT();
            string[] split = c.Split(',');
            int i = split[0].Length + split[1].Length + split[2].Length + 8;
            string nbt = " {" + c.Substring(i, c.Length - i - 2) + "}";
            if (nbt == " {}") nbt = "";
            return "/give @p minecraft:" + split[0].Substring(5, split[0].Length - 6) + split[1].Replace("Count:", " ") + split[2].Replace("Damage:", " ") + nbt;
        }
        private string GetAttributeModifiers(TreeViewItem t, string slot)
        {
            if ((t.Items[0] as TextBlock).Text == "无效果") return "";
            string value = "", Slot = "";
            if (slot != "") Slot = ",Slot:" + slot;
            for(int i = 0; i < t.Items.Count; i++)
            {
                value += "{";
                string s = (t.Items[i] as TextBlock).Text;
                if (s.Contains("%")) { s.Substring(0, s.Length - 1); s = "Operation:1," + s.Replace("%", ""); } else s = "Operation:0," + s;
                s = s.Replace("+", "");

                s = s.Replace("生物跟随", "UUIDLeast:229634316,UUIDMost:229634316,AttributeName:generic.followRange,Name:Range,Amount:");
                s = s.Replace("移动速度", "UUIDLeast:302789710,UUIDMost:302789710,AttributeName:generic.movementSpeed,Name:Speed,Amount:");
                s = s.Replace("攻击速度", "UUIDLeast:1783579012,UUIDMost:1783579012,AttributeName:generic.attackSpeed,Name:AttackSpeed,Amount:");
                s = s.Replace("盔甲韧性", "UUIDLeast:-193291057,UUIDMost:-193291057,AttributeName:generic.armorToughness,Name:ArmorToughness,Amount:");
                s = s.Replace("盔甲", "UUIDLeast:2044606406,UUIDMost:2044606406,AttributeName:generic.armor,Name:Armor,Amount:");
                s = s.Replace("最大生命", "UUIDLeast:957625913,UUIDMost:957625913,AttributeName:generic.maxHealth,Name:Health,Amount:");
                s = s.Replace("幸运值", "UUIDLeast:1414557585,UUIDMost:1414557585,AttributeName:generic.luck,Name:Luck,Amount:");
                s = s.Replace("攻击伤害", "UUIDLeast:1556604387,UUIDMost:1556604387,AttributeName:generic.attackDamage,Name:Attack,Amount:");
                s = s.Replace("击退抗性", "UUIDLeast:730644133,UUIDMost:730644133,AttributeName:generic.knockbackResistance,Name:KnockbackResistance,Amount:");
                value += s + Slot + "},";
            }
            return value;
        }
        private string GetEnchant()
        {
            string ENCHANT = "";
            ObservableCollection<NumericUpDown> o = new ObservableCollection<NumericUpDown>();
            foreach (UIElement element in E1.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            foreach (UIElement element in E2.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            foreach (UIElement element in E3.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            foreach (UIElement element in E4.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            foreach (UIElement element in E5.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            foreach (UIElement element in E6.Children) if (element is NumericUpDown) o.Add((NumericUpDown)element);
            int x = 0;
            for (int i = 0; i < o.Count; i++)
            {
                if (i < 11) x = i; else if (i < 17) x = i + 5; else if (i < 21) x = i + 15; else if (i < 25) x = i + 27; else if (i < 27) x = i + 36; else if (i < 29) x = i + 43;
                if (o[i].Value != 0) ENCHANT += "{id:" + x + ",lvl:" + o[i].Value + "},";
            }
            if (ENCHANT == "") return "";
            return "ench:[" + ENCHANT.Substring(0, ENCHANT.Length - 1) +"],";
        }

        private void Tooltip(string arg)
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
        private void UpdateItemPick(object sender, RoutedEventArgs e)
        {
            switch ((string)ITEM.Content)
            {
                case "建筑方块": BlocksB.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "装饰性方块": BlocksD.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "红石": Redstone.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "交通/杂项": Other.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "食物": Food.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "工具与武器": Tools_Weapons.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "材料": Materials.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
                case "杂项/其它": Other_B.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)); break;
            }
            if (sender != BlocksB) { next.IsEnabled = false;last.IsEnabled = false; } else { next.IsEnabled = true; }

            Tile t = (Tile)sender;
            t.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/" + t.Name + ".png")) };
            selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/" + t.Name + "S.png")) };
            ITEM.Content = t.ToolTip;
            Tooltip(t.Name);
            itemIdx = t.Name;
        }
        private void GetMeta(object sender, RoutedEventArgs e)
        {
            string tooltip = (string)((Button)sender).ToolTip;
            tooltip = tooltip.Split('\n')[1];
            if (tooltip.Contains("Meta")) Meta.Value = int.Parse(tooltip.Substring(5));
            else ItemName.Text = tooltip;
        }
        private void Page(object sender, RoutedEventArgs e)
        {
            if (sender == next)
            {
                next.IsEnabled = false; last.IsEnabled = true;
                selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/BlocksBS2.png")) };
                Tooltip("BlocksB2");
            }
            else
            {
                last.IsEnabled = false; next.IsEnabled = true;
                selector.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(Environment.CurrentDirectory + "/images/ItemsPick/BlocksBS.png")) };
                Tooltip("BlocksB");
            }
        }
        private void GetName(object sender, RoutedEventArgs e)
        {
            string tip = (string)((Button)sender).ToolTip;
            string[] split = tip.Split('\n');
            if (menu.SelectedIndex == 3)
            {
                string s = split[1];
                if (s == "minecraft:redstone") s = "minecraft:redstone_wire";
                can.Text = split[0] + "(" + s + ")";
                return;
            }
            ItemName.Text = split[1];
            cmdBlock.Visibility = Visibility.Visible; cmd.IsEnabled = false;
            sign.Visibility = Visibility.Visible; signData.IsEnabled = false;
            book.Visibility = Visibility.Visible; bookData.IsEnabled = false;
            beacon.Visibility = Visibility.Visible; Beacon.IsEnabled = false;
            string name = split[1].Substring(10);
            if (name == "command_block") { cmdBlock.Visibility = Visibility.Hidden; cmd.IsEnabled = true; }
            else if (name == "sign") { sign.Visibility = Visibility.Hidden; signData.IsEnabled = true; }
            else if (name == "written_book") { book.Visibility = Visibility.Hidden; bookData.IsEnabled = true; }
            else if (name == "beacon") { beacon.Visibility = Visibility.Hidden; Beacon.IsEnabled = true; }

            string now = (string)ITEM.Content;
            if (now == "建筑方块" || now == "装饰性方块" || now == "红石" || ItemName.Text == "minecraft:golden_rail" || ItemName.Text == "minecraft:detector_rail" || ItemName.Text == "minecraft:rail" || ItemName.Text == "minecraft:activator_rail")
            { CP.IsEnabled = true; canPlace.IsEnabled = true; isBlock.Visibility = Visibility.Hidden; Place.IsEnabled = true; }
            else { CP.IsEnabled = false; canPlace.IsEnabled = false; isBlock.Visibility = Visibility.Visible; Place.IsEnabled = false; }

            switch (itemIdx)
            {
                default: goto Dis;
                case "BlocksB": switch (name)
                {
                    default: goto Dis;
                    case "stone": UpdateMetaData(name, new string[] { "石头", "花岗岩", "磨制花岗岩", "闪长岩", "磨制闪长岩", "安山岩", "磨制安山岩" }, true); break;
                    case "dirt": UpdateMetaData(name, new string[] { "泥土", "砂土", "灰化土" }, true); break;
                    case "planks": UpdateMetaData(name, new string[] { "橡木木板", "云杉木板", "白桦木板", "丛林木板", "金合欢木板", "深色橡木木板" }, true); break;
                    case "sand": UpdateMetaData(name, new string[] { "沙子", "红沙" }, true); break;
                    case "log": UpdateMetaData(name, new string[] { "橡木", "云杉木", "白桦木", "丛林木" }, true); break;
                    case "sponge": UpdateMetaData(name, new string[] { "海绵", "湿海绵" }, true); break;
                    case "sandstone": UpdateMetaData(name, new string[] { "沙石", "錾制沙石", "平滑沙石" }, true); break;
                    case "wool": UpdateMetaData(name, new string[] { "羊毛", "橙色羊毛", "品红色羊毛", "淡蓝色羊毛", "黄色羊毛", "黄绿色羊毛", "粉红羊毛", "灰色羊毛", "淡灰色羊毛", "青色羊毛", "紫色羊毛", "蓝色羊毛", "棕色羊毛", "绿色羊毛", "红色羊毛", "黑色羊毛" }, true); break;
                    case "stone_slab": UpdateMetaData(name, new string[] { "石台阶", "沙石台阶", "圆石台阶", "砖台阶", "石砖台阶", "地狱砖台阶", "石英台阶" }, true); break;
                    case "stained_glass": UpdateMetaData(name, new string[] { "白色染色玻璃", "橙色染色玻璃", "品红色染色玻璃", "淡蓝色染色玻璃", "黄色染色玻璃", "黄绿色染色玻璃", "粉红染色玻璃", "灰色染色玻璃", "淡灰色染色玻璃", "青色染色玻璃", "紫色染色玻璃", "蓝色染色玻璃", "棕色染色玻璃", "绿色染色玻璃", "红色染色玻璃", "黑色染色玻璃" }, true); break;
                    case "stonebrick": UpdateMetaData(name, new string[] { "石砖", "苔石砖", "裂石砖", "錾制石砖" }, true); break;
                    case "wooden_slab": UpdateMetaData(name, new string[] { "橡木台阶", "云杉台阶", "白桦台阶", "丛林台阶", "金合欢台阶", "深色橡木台阶" }, true); break;
                    case "cobblestone_wall": UpdateMetaData(name, new string[] { "圆石墙", "苔石墙" }, true); break;
                    case "quartz_block": UpdateMetaData(name, new string[] { "石英块", "錾制石英块", "竖纹石英块" }, true); break;
                    case "stained_hardened_clay": UpdateMetaData(name, new string[] { "白色染色粘土", "橙色染色粘土", "品红色染色粘土", "淡蓝色染色粘土", "黄色染色粘土", "黄绿色染色粘土", "粉红染色粘土", "灰色染色粘土", "淡灰色染色粘土", "青色染色粘土", "紫色染色粘土", "蓝色染色粘土", "棕色染色粘土", "绿色染色粘土", "红色染色粘土", "黑色染色粘土" }, true); break;
                    case "log2": UpdateMetaData(name, new string[] { "金合欢木", "深色橡木" }, true); break;
                    case "prismarine": UpdateMetaData(name, new string[] { "海晶石", "海晶石砖", "暗海晶石" }, true); break;
                    case "red_sandstone": UpdateMetaData(name, new string[] { "红沙石", "錾制红沙石", "平滑红沙石" }, true); break;
                } break;
                case "BlocksD": switch (name)
                {
                    default: goto Dis;
                    case "sapling": UpdateMetaData(name, new string[] { "橡树树苗", "云杉树苗", "白桦树苗", "丛林树苗", "金合欢树苗", "深色橡树树苗" }, true); break;
                    case "leaves": UpdateMetaData(name, new string[] { "橡树树叶", "云杉树叶", "白桦树叶", "丛林树叶" }, true); break;
                    case "tallgrass": UpdateMetaData(name, new string[] { "草", "蕨" }, true); break;
                    case "red_flower": UpdateMetaData(name, new string[] { "罂粟", "兰花", "绒球葱", "茜草花", "红色郁金香", "橙色郁金香", "白色郁金香", "粉红色郁金香", "滨菊" }, true); break;
                    case "monster_egg": UpdateMetaData(name, new string[] { "石头怪物蛋", "圆石怪物蛋", "石砖怪物蛋", "苔石砖怪物蛋", "裂石砖怪物蛋", "錾制石砖怪物蛋" }, true); break;
                    case "anvil": UpdateMetaData(name, new string[] { "铁砧", "轻微损坏的铁砧", "严重损坏的铁砧" }, true); break;
                    case "stained_glass_pane": UpdateMetaData(name, new string[] { "白色染色玻璃板", "橙色染色玻璃板", "品红色染色玻璃板", "淡蓝色染色玻璃板", "黄色染色玻璃板", "黄绿色染色玻璃板", "粉红染色玻璃板", "灰色染色玻璃板", "淡灰色染色玻璃板", "青色染色玻璃板", "紫色染色玻璃板", "蓝色染色玻璃板", "棕色染色玻璃板", "绿色染色玻璃板", "红色染色玻璃板", "黑色染色玻璃板" }, true); break;
                    case "leaves2": UpdateMetaData(name, new string[] { "金合欢树叶", "深色橡树树叶" }, true); break;
                    case "carpet": UpdateMetaData(name, new string[] { "地毯", "橙色地毯", "品红色地毯", "淡蓝色地毯", "黄色地毯", "黄绿色地毯", "粉红地毯", "灰色地毯", "淡灰色地毯", "青色地毯", "紫色地毯", "蓝色地毯", "棕色地毯", "绿色地毯", "红色地毯", "黑色地毯" }, true); break;
                    case "double_plant": UpdateMetaData(name, new string[] { "向日葵", "丁香", "高草丛", "大型蕨", "玫瑰丛", "牡丹" }, true); break;
                } break;
                case "Food": switch (name)
                {
                    default: goto Dis;
                    case "golden_apple": UpdateMetaData(name, new string[] { "金苹果", "附魔金苹果" }, true); break;
                    case "fish": UpdateMetaData(name, new string[] { "生鱼", "生鲑鱼", "小丑鱼", "河豚" }, true); break;
                    case "cooked_fish": UpdateMetaData(name, new string[] { "熟鱼", "熟鲑鱼" }, true); break;
                } break;
                case "Tools_Weapons": switch (name)
                {
                    default: goto Dis;
                    case "diamond_sword": UpdateMetaData(name, new string[] { "钻石剑\nminecraft:diamond_sword", "金剑\nminecraft:golden_sword", "铁剑\nminecraft:iron_sword", "石剑\nminecraft:stone_sword", "木剑\nminecraft:wooden_sword" }, false); break;
                    case "diamond_shovel": UpdateMetaData(name, new string[] { "钻石锹\nminecraft:diamond_shovel", "金锹\nminecraft:golden_shovel", "铁锹\nminecraft:iron_shovel", "石锹\nminecraft:stone_shovel", "木锹\nminecraft:wooden_shovel" }, false); break;
                    case "diamond_pickaxe": UpdateMetaData(name, new string[] { "钻石镐\nminecraft:diamond_pickaxe", "金镐\nminecraft:golden_pickaxe", "铁镐\nminecraft:iron_pickaxe", "石镐\nminecraft:stone_pickaxe", "木镐\nminecraft:wooden_pickaxe" }, false); break;
                    case "diamond_axe": UpdateMetaData(name, new string[] { "钻石斧\nminecraft:diamond_axe", "金斧\nminecraft:golden_axe", "铁斧\nminecraft:iron_axe", "石斧\nminecraft:stone_axe", "木斧\nminecraft:wooden_axe" }, false); break;
                    case "diamond_hoe": UpdateMetaData(name, new string[] { "钻石锄\nminecraft:diamond_hoe", "金锄\nminecraft:golden_hoe", "铁锄\nminecraft:iron_hoe", "石锄\nminecraft:stone_hoe", "木锄\nminecraft:wooden_hoe" }, false); break;
                    case "leather_helmet": UpdateMetaData(name, new string[] { "钻石头盔\nminecraft:diamond_helmet", "金头盔\nminecraft:golden_helmet", "铁头盔\nminecraft:iron_helmet", "锁链头盔\nminecraft:chainmail_helmet", "皮革帽子\nminecraft:leather_helmet\n可染色" }, false); break;
                    case "leather_chestplate": UpdateMetaData(name, new string[] { "钻石胸甲\nminecraft:diamond_chestplate", "金胸甲\nminecraft:golden_chestplate", "铁胸甲\nminecraft:iron_chestplate", "锁链胸甲\nminecraft:chainmail_chestplate", "皮革外套\nminecraft:leather_chestplate\n可染色" }, false); break;
                    case "leather_leggings": UpdateMetaData(name, new string[] { "钻石护腿\nminecraft:diamond_leggings", "金护腿\nminecraft:golden_leggings", "铁护腿\nminecraft:iron_leggings", "锁链护腿\nminecraft:chainmail_leggings", "皮革裤子\nminecraft:leather_leggings\n可染色" }, false); break;
                    case "leather_boots": UpdateMetaData(name, new string[] { "钻石靴子\nminecraft:diamond_boots", "金靴子\nminecraft:golden_boots", "铁靴子\nminecraft:iron_boots", "锁链靴子\nminecraft:chainmail_boots", "皮革靴子\nminecraft:leather_boots\n可染色" }, false); break;
                } break;
                case "Materials": switch (name)
                {
                    default: goto Dis;
                    case "coal": UpdateMetaData(name, new string[] { "煤炭", "木炭" }, true); break;
                    case "dye": UpdateMetaData(name, new string[] { "墨囊", "玫瑰红", "仙人掌绿", "可可豆", "青金石", "紫色染料", "青色染料", "淡灰色染料", "灰色染料", "粉红色染料", "黄绿色染料", "蒲公英黄", "淡蓝色染料", "品红色染料", "橙色染料", "骨粉" }, true); break;
                } break;
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
        private void UpdateMetaData(string path, string[] data, bool meta)
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
        private void BeaconSwitch(object sender, RoutedEventArgs e)
        {
            ToggleButton b = (ToggleButton)sender;
            if (b == e7 || b == e6)
            {
                if (b.IsChecked == true) { b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0)); b.IsChecked = true; }
                else { b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); b.IsChecked = false; }
                if (b == e7) { e6.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e6.IsChecked = false; }
                else { e7.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e7.IsChecked = false; }
                return;
            }
            if (b.IsChecked == true)
            {
                e1.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e1.IsChecked = false;
                e2.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e2.IsChecked = false;
                e3.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e3.IsChecked = false;
                e4.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e4.IsChecked = false;
                e5.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)); e5.IsChecked = false;

                b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                e7.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                e7.Background = b.Background;
                e7.ToolTip = b.ToolTip + " Ⅱ";
                e7.IsChecked = false;
                e7.IsEnabled = true;
                b.IsChecked = true;
                e6.IsEnabled = true;
                e.Handled = true;
            }
            else
            {
                b.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                e7.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                e7.IsEnabled = false;
                e7.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                e7.ToolTip = "";
                e6.IsEnabled = false;
                e6.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                e6.IsChecked = false;
            }
        }
        private void Properties(object sender, RoutedEventArgs e)
        {
            if (sender == clear)
                for(int i = 0; i < pro.Children.Count; i++)
                {
                    object obj = pro.Children[i];
                    if (obj is NumericUpDown) ((NumericUpDown)obj).Value = 0;
                    else if (obj is CheckBox) ((CheckBox)obj).IsChecked = false;
                }
            else if (sender == save)
            {
                TreeViewItem g = hand1;
                switch (((string)box.Header).Replace("加成属性-", ""))
                {
                    default: return;
                    case "在主手上": g = hand1; break;
                    case "在副手上": g = hand2; break;
                    case "戴在头上": g = onHead; break;
                    case "穿在身上": g = onBody; break;
                    case "穿在腿上": g = onLeg; break;
                    case "穿在脚上": g = onFeet; break;
                    case "所有位置": g = all; break;
                }
                TextBlock l = new TextBlock();
                g.Items.Clear();
                if (value1.Value != 0) { l.Text = "生物跟随+" + value1.Value; if (percent1.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value2.Value != 0) { l.Text = "移动速度+" + value2.Value; if (percent2.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value3.Value != 0) { l.Text = "攻击速度+" + value3.Value; if (percent3.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value4.Value != 0) { l.Text = "盔甲韧性+" + value4.Value; if (percent4.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value5.Value != 0) { l.Text = "盔甲+"     + value5.Value; if (percent5.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value6.Value != 0) { l.Text = "最大生命+" + value6.Value; if (percent6.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value7.Value != 0) { l.Text = "幸运值+"   + value7.Value; if (percent7.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value8.Value != 0) { l.Text = "攻击伤害+" + value8.Value; if (percent8.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); l = new TextBlock(); }
                if (value9.Value != 0) { l.Text = "击退抗性+" + value9.Value; if (percent9.IsChecked == true) l.Text += "%"; l.ToolTip = l.Text; if (l.Text.Contains('-')) l.Text = l.Text.Replace("+", ""); g.Items.Add(l); }
                if (g.Items.Count < 1) { l.Text = "无效果"; g.Items.Add(l); }
            }
            else
            {
                Properties(save, null);Properties(clear, null);
                enablePro.Visibility = Visibility.Hidden;
                pro.IsEnabled = true;
                save.IsEnabled = true;
                box.Header = "加成属性-" + ((Button)sender).ToolTip;
                TreeViewItem g = ((StackPanel)((Button)sender).Parent).Parent as TreeViewItem;
                if (((TextBlock)g.Items[0]).Text == "无效果") return;
                for(int i = 0; i < g.Items.Count; i++)
                {
                    TextBlock l = g.Items[i] as TextBlock;
                    string[] split = l.Text.Split('+', '-');
                    NumericUpDown num = null; CheckBox percent = null;
                    switch (split[0])
                    {
                        case "生物跟随": num = value1; percent = percent1; break;
                        case "移动速度": num = value2; percent = percent2; break;
                        case "攻击速度": num = value3; percent = percent3; break;
                        case "盔甲韧性": num = value4; percent = percent4; break;
                        case "盔甲":     num = value5; percent = percent5; break;
                        case "最大生命": num = value6; percent = percent6; break;
                        case "幸运值":   num = value7; percent = percent7; break;
                        case "攻击伤害": num = value8; percent = percent8; break;
                        case "击退抗性": num = value9; percent = percent9; break;
                    }
                    string val = l.Text.Substring(split[0].Length);
                    if (val.Contains("%")) { val = val.Replace("%", ""); percent.IsChecked = true; }
                    num.Value = Convert.ToDouble(val);
                }
            }
        }
        private void BlockList(object sender, RoutedEventArgs e)
        {
            if (sender == Place)
            {
                if (canPlace.SelectedItem != null) canPlace.Items.Remove(canPlace.SelectedItem);
            }
            else
            {
                if (canBreak.SelectedItem != null) canBreak.Items.Remove(canBreak.SelectedItem);
            }
        }
        private void Add(object sender, RoutedEventArgs e)
        {
            ListBox b = null;

            if (sender == addBreak) b = canBreak;
            else b = canPlace;
            string s = can.Text;
            for (int i = 0; i < b.Items.Count; i++) if ((string)b.Items[0] == s) return;
            b.Items.Add(s);
            GC.Collect();
        }

        private void Never_Click(object sender, RoutedEventArgs e)
        {
            if(sender==never)age.IsEnabled = !(bool)never.IsChecked;
            else delay.IsEnabled = !(bool)cantPick.IsChecked;
        }
        private void ENC_Click(object sender, RoutedEventArgs e)
        {
            color.IsEnabled = (bool)ENC.IsChecked;
        }

        private void OtherGenerate(object sender, RoutedEventArgs e)
        {
            if (sender == get) CmdGenerator.AddCommand(GenerateNBT());
            else 
            {
                string ench = GetEnchant();
                ItemName.Text = "minecraft:enchanted_book";
                if (sender == getBook && ench.Length != 0) CmdGenerator.AddCommand(GenerateCommand().Replace(ench.Substring(0, ench.Length - 1), "StoredEnchantments:" + ench.Substring(4, ench.Length - 5)));
                else if (ench.Length != 0) CmdGenerator.AddCommand(GenerateNBT().Replace(ench.Substring(0, ench.Length - 1), "StoredEnchantments:" + ench.Substring(4, ench.Length - 5)));
                else CmdGenerator.AddCommand("");
            }
            
        }
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            menu.Content = e.ClickedItem;
            if (menu.SelectedIndex == 0 || menu.SelectedIndex == 3)
            {
                panel.Visibility = Visibility.Visible;
                UpdateItemPick(BlocksB, null);
            }
            else panel.Visibility = Visibility.Hidden;
            GC.Collect();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (sender == ex1)
            {
                if (ex1.IsExpanded)
                {
                    ex2.IsExpanded = false;
                    ex2.Margin = new Thickness(394, 5 + ex1.ActualHeight, 0, 0);
                    _break.Margin = new Thickness(550, 7 +ex1.ActualHeight, 0, 0);
                }
                else
                {
                    ex2.Margin = new Thickness(394, 37, 0, 0);
                    _break.Margin = new Thickness(550, 39, 0, 0);
                }
            }
            else
            {
                if (ex2.IsExpanded&&ex1.IsExpanded)
                {
                    ex1.IsExpanded = false;
                    ex2.Margin = new Thickness(394, 37, 0, 0);
                    _break.Margin = new Thickness(550, 39, 0, 0);
                }
            }
        }

        private void ex1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ex1.IsExpanded)
            {
                ex2.IsExpanded = false;
                ex2.Margin = new Thickness(394, 5 + ex1.ActualHeight, 0, 0);
                _break.Margin = new Thickness(550, 7 + ex1.ActualHeight, 0, 0);
            }
            else
            {
                ex2.Margin = new Thickness(394, 37, 0, 0);
                _break.Margin = new Thickness(550, 39, 0, 0);
            }
        }
    }
}