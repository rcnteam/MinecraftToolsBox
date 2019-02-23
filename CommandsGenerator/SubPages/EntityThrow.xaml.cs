using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityThrow.xaml 的交互逻辑
    /// </summary>
    public partial class EntityThrow : Grid
    {
        public EntityThrow()
        {
            InitializeComponent();
        }
        public void Enable(string id)
        {
            E1.IsEnabled = false;
            E2.IsEnabled = false;
            E3.IsEnabled = false;
            explosion.IsEnabled = false;
            Explosion.IsEnabled = false;
            switch (id)
            {
                default: break;
                case "ThrownExpBottle": E1.IsEnabled = true; break;
                case "Snowball": E1.IsEnabled = true; break;
                case "ThrownEnderpearl": E1.IsEnabled = true; break;
                case "ThrownEgg": E1.IsEnabled = true; break;
                case "Fireball": E2.IsEnabled = true; explosion.IsEnabled = true; Explosion.IsEnabled = true; break;
                case "DragonFireball": E2.IsEnabled = true; break;
                case "SmallFireball": E2.IsEnabled = true; break;
                case "SpectralArrow": E3.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (shake.IsChecked == true) tag += "shake:1,";
                if (player.Text != "") tag += "ownerName:" + player.Text + ",";
            }
            else if (E2.IsEnabled)
            {
                if (X.Value != 0 || Y.Value != 0 || Z.Value != 0) tag += "power:[" + X.Value + "d," + Y.Value + "d," + Z.Value + "],";
                if (Explosion.IsEnabled && Explosion.Value != 1) tag += "ExplosionPower:" + Explosion.Value + ",";
            }
            else if (E3.IsEnabled)
            {
                if (S.IsChecked == true) tag += "shake:1,";
                if (ground.IsChecked == true) tag += "inGround:true,";
                if (crit.IsChecked == true) tag += "crit:true,";
                if (pick.SelectedIndex != 1) tag += "pickup:" + pick.SelectedIndex + ",";
                if (effect.Value != null) tag += "Duration:" + (effect.Value * 20) + ",";
                if (life.Value != null) tag += "life:" + (1200 - (life.Value * 20)) + ",";
            }
            return tag;
        }
    }
}
