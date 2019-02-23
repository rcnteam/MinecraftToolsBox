using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// ColorEditor.xaml 的交互逻辑
    /// </summary>
    public partial class ColorEditor : Grid
    {
        public object TargetItem;
        public Button Apply { get { return apply; } }
        public Button Cancel { get { return cancel; } }
        public ColorPicker ColorPicker { get { return cp; } }
        public DataGrid DG;
        public string Mode = "Color";

        public ColorEditor()
        {
            InitializeComponent();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if(TargetItem is FireworkItem)
            {
                if (Mode == "Color") (TargetItem as FireworkItem).Color = new SolidColorBrush(cp.SelectedColor); else (TargetItem as FireworkItem).FadeColor = new SolidColorBrush(cp.SelectedColor);
                DG.Items.Refresh();
            }
        }
    }
}
