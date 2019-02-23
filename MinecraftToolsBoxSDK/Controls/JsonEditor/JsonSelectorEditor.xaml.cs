using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// JsonSelectorEditor.xaml 的交互逻辑
    /// </summary>
    public partial class JsonSelectorEditor : Grid
    {
        public Button Apply { get { return apply; } }
        public Button Cancel { get { return cancel; } }
        public TextBox Selector { get { return selector; } }
        public JsonEditor Editor { get; set; }
        public static ImageBrush SelectorBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MinecraftToolsBoxSDK;component/Controls/JsonEditor/selector.png")));

        public JsonSelectorEditor()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            selector.Text = "";
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.Selection.Text != "")
            {
                Span ss = new Span(Editor.Selection.Start, Editor.Selection.End);
                (ss.Parent as Paragraph).Inlines.Remove(ss);
            }
            TextPointer start = Editor.Selection.Start;
            InlineUIContainer container = new InlineUIContainer(new TextBlock { Text = selector.Text, Background = SelectorBrush, SnapsToDevicePixels = true, ToolTip = "Selector" }, start);
            Span s = new Span(container, start);
        }
    }
}
