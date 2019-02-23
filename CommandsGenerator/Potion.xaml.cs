using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Potion.xaml 的交互逻辑
    /// </summary>
    public partial class Potion : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate Tmp;
        public Potion(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            Tmp = tmp;
        }

        public string GenerateCommand()
        {
            string effects = GetNBT();

            if (type.SelectedIndex == 0)
            {
                return "/give @p minecraft:potion 1 0" + effects;
            }
            else if (type.SelectedIndex == 1 || type.SelectedIndex == 2)
            {
                string typ = type.SelectedIndex == 1 ? "minecraft:splash_potion" : "minecraft:lingering_potion";
                if (item.IsChecked == true) return "/give @p " + typ + " 1 0" + effects;
                else return "/summon ~ ~ ~ potion" + effects;
            }
            else if (type.SelectedIndex == 3)
            {
                //id在1.11以前为AreaEffectCloud
                return "/summon ~ ~ ~ area_effect_cloud" + effects;
            }
            else
            {
                if (item.IsChecked == true) return "/give @p minecraft:tipped_arrow 1 0" + effects;
                else return "/summon ~ ~ ~ arrow" + effects;
            }
        }
        private string GetNBT()
        {
            string effects = selector.GenerateCommand();
            int c = (Color.Red << 16) + (Color.Green << 8) + Color.Blue;
            string color = "";
            string Inbt = "", Enbt = "";
            if (inbt.Text.Length > 1) Inbt = inbt.Text.Substring(1, inbt.Text.Length - 2);
            if (enbt.Text.Length > 1)
            {
                Enbt = enbt.Text.Substring(1, enbt.Text.Length - 2);
                string split = Enbt.Split(',')[0];
                if (split.Substring(0, 2) == "id") Enbt = Enbt.Substring(split.Length + 1);
            }

            if (type.SelectedIndex == 0)
            {
                if (customColor.IsChecked == true) color = ",CustomPotionColor:" + c;
                if (effects != "") effects = "CustomPotionEffects:[" + effects + "]" + color; else if (color != "") effects = color.Substring(1);
                if (Inbt != "") { if (effects != "") effects += "," + Inbt; else effects += Inbt; }
                if (effects != "") effects = " {" + effects + "}";
            }
            else if (type.SelectedIndex == 1 || type.SelectedIndex == 2)
            {
                string typ = type.SelectedIndex == 1 ? "minecraft:splash_potion" : "minecraft:lingering_potion";
                if (customColor.IsChecked == true) color = ",CustomPotionColor:" + c;
                if (item.IsChecked == true)
                {
                    if (effects != "") effects = "CustomPotionEffects:[" + effects + "]" + color; else if (color != "") effects = color.Substring(1);
                    if (Inbt != "") { if (effects != "") effects += "," + Inbt; else effects += Inbt; }
                    if (effects != "") effects = " {" + effects + "}";
                }
                else
                {
                    if (effects != "") effects = "Potion:{id:\"" + typ + "\",tag:{CustomPotionEffects:[" + effects + "]" + color + "}}"; else if (color != "") effects = color.Substring(1);
                    if (Enbt != "") { if (effects != "") effects += "," + Enbt; else effects += Enbt; }
                    if (effects != "") effects = " {" + effects + "}";
                }
            }
            else if (type.SelectedIndex == 3)
            {
                //id在1.11以前为AreaEffectCloud
                if (effects != "") effects = "Effect:[" + effects + "]"; else if (color != "") effects = color.Substring(1);
                if (Enbt != "") { if (effects != "") effects += "," + Enbt; else effects += Enbt; }
                if (effects != "") effects = " {" + effects + "}";
            }
            else
            {
                if (customColor.IsChecked == true) color = ",Color:" + c;
                if (item.IsChecked == true)
                {
                    if (effects != "") effects = "CustomPotionEffects:[" + effects + "]" + color; else if (color != "") effects = color.Substring(1);
                    if (Inbt != "") { if (effects != "") effects += "," + Inbt; else effects += Inbt; }
                    if (effects != "") effects = " {" + effects + "}";
                }
                else
                {
                    if (effects != "") effects = "CustomPotionEffects:[" + effects + "]" + color; else if (color != "") effects = color.Substring(1);
                    if (Enbt != "") { if (effects != "") effects += "," + Enbt; else effects += Enbt; }
                    if (effects != "") effects = " {" + effects + "}";
                }
            }
            return effects;
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (type == null || GetENBT == null || enbt == null || entity == null || GetINBT == null || inbt == null || item == null) return;
            if (type.SelectedIndex == 0)
            {
                GetENBT.IsEnabled = false;
                enbt.IsEnabled = false;
                entity.IsEnabled = false;
                if (entity.IsChecked == true) item.IsChecked = true;
            }
            else if (type.SelectedIndex == 3)
            {
                GetENBT.IsEnabled = true;
                enbt.IsEnabled = true;
                entity.IsEnabled = true;
                GetINBT.IsEnabled = false;
                inbt.IsEnabled = false;
                item.IsEnabled = false;
                if (item.IsChecked == true) entity.IsChecked = true;
            }
            else
            {
                GetENBT.IsEnabled = true;
                enbt.IsEnabled = true;
                entity.IsEnabled = true;
            }
        }

        private void GetINBT_Click(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            string id = "minecraft:";
            if (type.SelectedIndex == 0) id += "potion";
            else if (type.SelectedIndex == 1) id += "splash_potion";
            else if (type.SelectedIndex == 2) id += "lingering_potion";
            else if (type.SelectedIndex == 4) id += "tipped_arrow";
            if (nbt != "") nbt = ",tag:" + nbt.Substring(1);
            Tmp.AddCommand("{id:" + id + ",Count:1,Damage:0" + nbt + "}");
        }

        private void GetENBT_Click(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            if (nbt != "") nbt = "," + nbt.Substring(2, nbt.Length - 3);
            string id = "";
            if (type.SelectedIndex == 0) id += "potion";
            else if (type.SelectedIndex == 1) id += "splash_potion";
            else if (type.SelectedIndex == 2) id += "lingering_potion";
            else if (type.SelectedIndex == 4) id += "arrow";
            Tmp.AddCommand("{id:" + id + nbt + "}");
        }
    }
}
