using System.Windows.Controls;
using MinecraftToolsBoxSDK;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Grid, ICommandGenerator
    {
        public Search()
        {
            InitializeComponent();
        }
        public string GenerateCommand()
        {
            return "";
        }
    }
}
