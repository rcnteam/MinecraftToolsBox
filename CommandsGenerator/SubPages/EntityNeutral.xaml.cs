using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityNeutral.xaml 的交互逻辑
    /// </summary>
    public partial class EntityNeutral : Grid
    {
        public EntityNeutral()
        {
            InitializeComponent();
        }
        public void Enable(string id)
        {
            E1.IsEnabled = false;
            E2.IsEnabled = false;
            E3.IsEnabled = false;
            switch (id)
            {
                default: break;
                case "Enderman": E1.IsEnabled = true; break;
                case "Wolf": E2.IsEnabled = true; break;
                case "PigZombie": E3.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E3.IsEnabled)
            {
                if (isBaby.IsChecked == true) tag += "IsBaby:" + IsBaby.IsChecked + ",";
                if (angry.Value != 0) tag += "Anger:" + (angry.Value * 20) + ",";
                if (BreakDoors.IsChecked == true) tag += "CanBreakDoors:" + BreakDoors.IsChecked + ",";
                if (UUID.Text != "") tag += "HurtBy:" + UUID.Text + ",";
            } else if (E1.IsEnabled)
            {
                if (ID.Value != 0) tag += "carried:" + ID.Value + ",";
                if (Meta.Value != 0 && ID.Value != 0) tag += "carriedData:" + Meta.Value + ",";
            }
            else if(E2.IsEnabled)
            {
                if (color.Value != 14) tag += "CollarColor:" + color.Value + ",";
                if (isAngry.IsChecked==true) tag += "Angry:true,";
            }
            return tag;
        }
    }
}
