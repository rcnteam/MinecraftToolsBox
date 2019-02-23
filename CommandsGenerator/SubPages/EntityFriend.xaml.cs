using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityFriend.xaml 的交互逻辑
    /// </summary>
    public partial class EntityFriend : Grid
    {
        public EntityFriend()
        {
            InitializeComponent();
        }
        public void Enable(string id)
        {
            E1.IsEnabled = false;
            E2.IsEnabled = false;
            E3.IsEnabled = false;
            E4.IsEnabled = false;
            E5.IsEnabled = false;
            E6.IsEnabled = false;
            E7.IsEnabled = false;
            E8.IsEnabled = false;
            switch (id)
            {
                default: break;
                case "SnowMan": E1.IsEnabled = true; break;
                case "Pig": E2.IsEnabled = true; break;
                case "Bat": E3.IsEnabled = true; break;
                case "VillagerGolem": E4.IsEnabled = true; break;
                case "Sheep": E5.IsEnabled = true; break;
                case "Chicken": E6.IsEnabled = true; break;
                case "Ozelot": E7.IsEnabled = true; break;
                case "Rabbit": E8.IsEnabled = true; break;
            }
        }

        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (pump.IsChecked == false) tag += "Pumpkin:false,";
            }
            else if (E2.IsEnabled)
            {
                if (saddle.IsChecked == true) tag += "Saddle:true,";
            }
            else if (E3.IsEnabled)
            {
                if (bat.IsChecked == true) tag += "BatFlags:true,";
            }
            else if (E4.IsEnabled)
            {
                if (player.IsChecked == true) tag += "PlayerCreated:true,";
            }
            else if (E5.IsEnabled)
            {
                if (shear.IsChecked == true) tag += "Sheared:true,";
                if (color.Value != null) tag += "Color:" + color.Value + ",";
            }
            else if (E6.IsEnabled)
            {
                if (jokey.IsChecked == true) tag += "IsChickenJockey:true,";
                if (lay.Value != null) tag += "EggLayTime:" + (lay.Value * 20) + ",";
            }
            else if (E7.IsEnabled)
            {
                if (cat.SelectedIndex != 4) tag += "CatType:" + cat.SelectedIndex + ",";
            }
            else if (E8.IsEnabled)
            {
                if (rabbit.SelectedIndex != 7) tag += "RabbitType:" + rabbit.SelectedIndex + ",";
                if (food.Value != null) tag += "MoreCarrotTicks:" + food.Value + ",";
            }
            return tag;
        }
    }
}