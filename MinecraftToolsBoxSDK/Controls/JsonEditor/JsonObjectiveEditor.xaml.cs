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
    public partial class JsonObjectiveEditor : Grid
    {
        public Button Apply { get { return apply; } }
        public Button Cancel { get { return cancel; } }
        public JsonEditor Editor { get; set; }
        public static ImageBrush ObjectiveBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MinecraftToolsBoxSDK;component/Controls/JsonEditor/objective.png")));
        public TextBox Selector { get { return selector; } }

        public JsonObjectiveEditor()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            selector.Text = "";
            objective.Text = "";
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (Editor.Selection.Text != "")
            {
                Span ss = new Span(Editor.Selection.Start, Editor.Selection.End);
                (ss.Parent as Paragraph).Inlines.Remove(ss);
            }
            TextPointer start = Editor.Selection.Start;
            InlineUIContainer container = new InlineUIContainer(new TextBlock { Text = objective.Text, Background = ObjectiveBrush, SnapsToDevicePixels = true, ToolTip = selector.Text }, start);
            Span s = new Span(container, start);
        }
    }
}
