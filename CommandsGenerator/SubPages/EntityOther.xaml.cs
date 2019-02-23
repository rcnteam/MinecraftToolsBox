using System;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityOther.xaml 的交互逻辑
    /// </summary>
    public partial class EntityOther : Grid
    {
        public EntityOther()
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
            switch (id)
            {
                default: break;
                case "ItemFrame": E1.IsEnabled = true; break;
                case "XPOrb": E2.IsEnabled = true; break;
                case "Painting": E3.IsEnabled = true; break;
                case "PrimedTnt": E4.IsEnabled = true; break;
                case "EnderCrystal": E5.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (nbt.Text != "") tag += "Item:" + nbt.Text + ",";
                if (face.SelectedIndex != 0) tag += "Facing:" + face.SelectedIndex + ",";
                if (chance.Value != 100) tag += "ItemDropChance:" + Convert.ToSingle(chance.Value / 100) + ",";
                if (rotation.SelectedIndex != 0) tag += "ItemRotation:" + rotation.SelectedIndex + ",";
            }
            else if (E2.IsEnabled)
            {
                if (health.Value != 255) tag += "Health:" + health.Value + ",";
                if (age.Value != 6000) tag += "Age:" + age.Value + ",";
                if (exp.Value != 6000) tag += "Value:" + exp.Value + ",";
            }
            else if (E3.IsEnabled)
            {
                if (pit.SelectedIndex != 0) tag += "Facing:" + pit.SelectedIndex + ",";
                if (picName.SelectedIndex != -1) tag += "Motive:" + ((ComboBoxItem)picName.SelectedItem).Content + ",";
            }
            else if (E4.IsEnabled)
            {
                if (tnt.Value != null) tag += "Fuse:" + Convert.ToInt16(tnt.Value * 20) + ",";
            }
            else if (E5.IsEnabled)
            {
                if (show.IsChecked == false) tag += "ShowBottom:false,";
            }
            return tag;
        }
    }
}
