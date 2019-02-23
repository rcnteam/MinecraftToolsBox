using System.Windows.Controls;

namespace MinecraftToolsBox.Database
{
    /// <summary>
    /// LocationEditor.xaml 的交互逻辑
    /// </summary>
    public partial class TLocationEditor : Grid
    {
        public TLocationEditor()
        {
            InitializeComponent();
            LocX.setNeighbour(LocZ, LocY);
            LocY.setNeighbour(LocX, LocZ);
            LocZ.setNeighbour(LocY, LocX);
        }
        public string getData()
        {
            if (tilde.IsChecked == true)
            {
                if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "~ ~ ~";
                else return "~" + LocX.Text + " ~" + LocY.Text + " ~" + LocZ.Text;
            }
            else
            {
                if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "";
                else return LocX.Text + " " + LocY.Text + " " + LocZ.Text;
            }
        }

        public void importData(string loc)
        {
            string[] split = loc.Split(' ');
            if (split.Length < 3) split = new string[] { "0", "0", "0" };
            if (split[0].Contains("~")) tilde.IsChecked = true;
            LocX.Text = split[0].Replace("~", "");
            LocY.Text = split[1].Replace("~", "");
            LocZ.Text = split[2].Replace("~", "");
        }
    }
}
