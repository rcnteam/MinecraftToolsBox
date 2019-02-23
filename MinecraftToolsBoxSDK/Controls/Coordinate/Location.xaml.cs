using System.Windows.Controls;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// Location.xaml 的交互逻辑
    /// </summary>
    public partial class Location : StackPanel
    {
        public Location()
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
            if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
            else return LocX.Text+" "+LocY.Text+" "+LocZ.Text;
        }
        /// <summary>
        /// 获取坐标：x=X,y=Y,z=Z
        /// </summary>
        public string GetLoc()
        {
            if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";else 
            return "x=" + LocX.Text+",y=" + LocY.Text+",z=" + LocZ.Text;
        }
        /// <summary>
        /// 获取坐标：dx=X,dy=Y,dz=Z
        /// </summary>
        /// <returns></returns>
        public string GetDLoc()
        {
            if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
            else return "dx=" + LocX.Text + ",dy=" + LocY.Text + ",dz=" + LocZ.Text;
        }
        public string GetLoc2()
        {
            if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
            else return "X:" + LocX.Text + ",Y:" + LocY.Text + ",Z:" + LocZ.Text;
        }
    }
}
