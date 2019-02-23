using System.Windows;
using MinecraftToolsBoxSDK;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ExecuteAndTestfor.xaml 的交互逻辑
    /// </summary>
    public partial class ExecuteAndDetect : Grid, ICommandGenerator
    {
        bool stop = false;
        public ExecuteAndDetect()
        {
            InitializeComponent();
        }

        public string GenerateCommand()
        {
            if (c1.IsChecked == true)
            {
                return "/testfor " + ES.GetEntity() + (enbt.Text == "" ? "" : " " + enbt.Text);
            }
            else if (c2.IsChecked == true)
            {
                return "等待更新";
                //return "/testforblock "+loc.GetLocation()+"";
            }
            else if (c3.IsChecked == true)
            {
                return "等待更新";
            }
            else
            {
                return "/execute " + ES.GetEntity() + " " + exeloc.GetLocation() + (detect.IsChecked == true ? " detect " + detectloc.GetLocation() + " " + detectblockinfo.Text : " ") + cmd.Text;
            }
        }

        private void Checked(object sender, RoutedEventArgs e)
        {
            if (c1 == null || c2 == null || c3 == null || c4 == null || stop) return;
            stop = true;
            c1.IsChecked = false;
            c2.IsChecked = false;
            c3.IsChecked = false;
            c4.IsChecked = false;
            (sender as RadioButton).IsChecked = true;
            stop = false;
        }

    }
}
