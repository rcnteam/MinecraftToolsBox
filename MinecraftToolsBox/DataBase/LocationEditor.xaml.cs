using System.Windows.Controls;

namespace MinecraftToolsBox.Database
{
    /// <summary>
    /// LocationEditor.xaml 的交互逻辑
    /// </summary>
    public partial class LocationEditor : Grid
    {
        public LocationEditor()
        {
            InitializeComponent();
            LocX.setNeighbour(LocZ, LocY);
            LocY.setNeighbour(LocX, LocZ);
            LocZ.setNeighbour(LocY, LocX);
        }
        public string getData()
        {
            if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
            else return LocX.Text + " " + LocY.Text + " " + LocZ.Text;
        }
        public void importData(string loc)
        {
            string[] split = loc.Split(' ');
            if (split.Length < 3) split = new string[] { "0", "0", "0" };
            LocX.Text = split[0];
            LocY.Text = split[1];
            LocZ.Text = split[2];
        }
    }
}
