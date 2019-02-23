using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityTraffic.xaml 的交互逻辑
    /// </summary>
    public partial class EntityTraffic : Grid
    {
        public EntityTraffic()
        {
            InitializeComponent();
        }
        public void Enable(string id)
        {
            E1.IsEnabled = false;
            E2.IsEnabled = false;
            E3.IsEnabled = false;
            E4.IsEnabled = false;
            switch (id)
            {
                default: break;
                case "Boat": E1.IsEnabled = true; break;
                case "MinecartFurnace": E2.IsEnabled = true; break;
                case "MinecartTNT": E3.IsEnabled = true; break;
                case "MinecartCommandBlock": E4.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (boat.SelectedIndex != 0) tag += "Type:" + ((ComboBoxItem)boat.SelectedItem).ToolTip + ",";
            }
            else if (E2.IsEnabled)
            {
                if (fuel.Value != null) tag += "Fuel:" + (fuel.Value * 20) + ",";
            }
            else if (E3.IsEnabled)
            {
                if (tnt.Value != null) tag += "TNTFuse:" + (tnt.Value * 20) + ",";
            }
            if (E4.IsEnabled)
            {
                if (cmd.Text != "") tag += "Command:" + cmd.Text + ",";
                if (count.Value != null) tag += "SuccessCount:" + count.Value + ",";
                if (_out.Text != "") tag += "LastOutput:" + _out.Text + ",";
                if (track.IsChecked == false) tag += "TrackOutput:false,";
            }
            return tag;
        }
    }
}
