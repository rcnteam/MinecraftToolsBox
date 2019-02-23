using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// PotionEditor.xaml 的交互逻辑
    /// </summary>
    public partial class PotionEditor : Grid, ICommandGenerator
    {
        public PotionEditor()
        {
            InitializeComponent();
        }

        public string GenerateCommand()
        {
            string value = "";
            foreach(var item in Children)
            {
                if(item is GroupBox)
                {
                    var grid = (item as GroupBox).Content as Grid;
                    int c = grid.Children.Count / 4;
                    for (int i = 0; i < c; i++)
                    {
                        int level = (int)((NumericUpDown)grid.Children[i + c]).Value;
                        int duration = (int)((NumericUpDown)grid.Children[i + c * 2]).Value;
                        bool particle = (bool)((CheckBox)grid.Children[i + c * 3]).IsChecked;
                        if (level != 0)
                        {
                            string showParticle = "";
                            if (particle) showParticle = ",ShowParticles:0b";
                            value += "{" + (grid.Children[i] as TextBlock).ToolTip + ",Amplifier:" + level + ",Duration:" + duration + showParticle + "},";
                        }
                    }
                }
            }
            if (value != "") value = value.Substring(0, value.Length - 1);
            return value;
        }
    }
}
