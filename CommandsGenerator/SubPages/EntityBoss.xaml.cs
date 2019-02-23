using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityBoss.xaml 的交互逻辑
    /// </summary>
    public partial class EntityBoss : Grid
    {
        public EntityBoss()
        {
            InitializeComponent();
        }
        public void Enable(string id)
        {
            E1.IsEnabled = false;
            E2.IsEnabled = false;
            switch (id)
            {
                default: break;
                case "EnderDragon": E1.IsEnabled = true; break;
                case "WitherBoss": E2.IsEnabled = true; break;
            }
        }
        public string getNBT()
        {
            string tag = "";
            if (E1.IsEnabled)
            {
                if (phase.SelectedIndex != 10) tag += "DragonPhase:" + phase.SelectedIndex + ",";
            }else if (E2.IsEnabled)
            {
                if (invul.Value != null) tag += "Invul:" + invul.Value + ",";
            }
            return tag;
        }
    }
}
