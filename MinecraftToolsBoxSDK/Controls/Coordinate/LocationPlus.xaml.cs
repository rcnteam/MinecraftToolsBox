using System.Windows.Controls;
using System.Windows.Media;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// LocationPlus.xaml 的交互逻辑
    /// </summary>
    public partial class LocationPlus : StackPanel
    {
        public LocationPlus()
        {
            InitializeComponent();
            LocX.setNeighbour(LocZ, LocY);
            LocY.setNeighbour(LocX, LocZ);
            LocZ.setNeighbour(LocY, LocX);
        }
        /// <summary>
        /// 获取坐标：x y z
        /// </summary>
        /// <returns></returns>
        public string GetLocation()
        {
            if (tilde.IsChecked == true)
            { 
                return "~"+LocX.Text + " ~" + LocY.Text + " ~" + LocZ.Text;
            }
            else
            {
                if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
                else return LocX.Text + " " + LocY.Text + " " + LocZ.Text;
            }
        }

        public void SetDefaultValue()
        {
            LocX.Text = "0";
            LocY.Text = "0";
            LocZ.Text = "0";
        }

        public void SetForeground(Color a)
        {
            tilde.Foreground = new SolidColorBrush(a);
            X.Foreground = new SolidColorBrush(a);
            Y.Foreground = new SolidColorBrush(a);
            Z.Foreground = new SolidColorBrush(a);
        }
    }
}
