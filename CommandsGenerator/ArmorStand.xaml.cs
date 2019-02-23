using MinecraftToolsBoxSDK;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ArmorStand.xaml 的交互逻辑
    /// </summary>
    public partial class ArmorStand : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate Tmp;
        public ArmorStand(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            Tmp = tmp;
        }

        public string GenerateCommand()
        {
            return "";
        }
    }
}
