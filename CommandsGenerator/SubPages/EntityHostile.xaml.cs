using System;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityHostile.xaml 的交互逻辑
    /// </summary>
    public partial class EntityHostile : Grid
    {
        public EntityHostile()
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
                case "Slime": E1.IsEnabled = true; break;
                case "LavaSlime": E1.IsEnabled = true; break;
                case "Ghast": E2.IsEnabled = true; break;
                case "Creeper": E3.IsEnabled = true; break;
                case "Endermite": E4.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (size.Value != null) tag += "Size:" + size.Value + ",";
                if (ground.IsChecked == true) tag += "wasOnGround:true,";
            }
            else if (E2.IsEnabled)
            {
                if (power.Value != 1) tag += "ExplosionPower:" + power.Value + ",";
            }
            else if (E3.IsEnabled)
            {
                if (powered.IsChecked == true) tag += "powered:true,";
                if (r.Value != 3) tag += "ExplosionRadius:" + r.Value + ",";
                if (p.IsChecked == true) tag += "ignited:true,";
                if (t.Value != 1.5) tag += "Fuse:" + Convert.ToInt32(t.Value * 20) + ",";
            }
            else if (E4.IsEnabled)
            {
                if (life.Value != 240) tag += "Lifetime:" + Convert.ToInt32((120 - life.Value) * 20) + ",";
                if (player.IsChecked == true) tag += "PlayerSpawned:true,";
            }
            return tag;
        }
    }
}
