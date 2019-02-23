using System.Windows.Controls;

namespace MinecraftToolsBox.Database
{
    /// <summary>
    /// PSEditor.xaml 的交互逻辑
    /// </summary>
    public partial class Template : Grid
    {
        public Template(int code)
        {
            InitializeComponent();
            switch (code)
            {
                case 1:Button.Content = "玩家选择器";Button.ToolTip = "打开玩家选择器编辑";
                    break;
                case 2:Button.Content = "实体选择器";Button.ToolTip = "打开实体选择器编辑";
                    break;
                case 5:Button.Content = "编辑";Button.ToolTip = "打开Json书编辑器";
                    break;
                case 6:Button.Content = "编辑"; Button.ToolTip = "打开告示牌编辑器";
                    break;
                case 7:Button.Content = "编辑"; Button.ToolTip = "打开告示牌编辑器";
                    break;
                case 8:Button.Content = "物品NBT";Button.ToolTip = "编辑物品";
                    break;
                case 9:Button.Content = "实体NBT";Button.ToolTip = "编辑实体";
                    break;
            }
        }
        public void importData(string Data)
        {
            data.Text = Data;
        }
        public string getData()
        {
            return data.Text;
        }
    }
}
