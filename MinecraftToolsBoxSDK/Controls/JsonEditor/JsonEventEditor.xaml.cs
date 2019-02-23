using System;
using System.Collections.Generic;
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
    public partial class JsonEventEditor : Grid
    {
        public Button Apply { get { return apply; } }
        public Button Cancel { get { return cancel; } }
        public JsonEditor Editor { get; set; }
        public static ImageBrush EventBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/MinecraftToolsBoxSDK;component/Controls/JsonEditor/event.png")));
        public bool IsEdit = false;

        public JsonEventEditor()
        {
            InitializeComponent();
        }
        public void Clear()
        {
            ClickEvtType.SelectedIndex = 3;
            HoverEvtType.SelectedIndex = 4;
            ClickEvtValue.Text = "";
            HoverEvtValue.Text = "";
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            TextPointer SelectionStart = Editor.Selection.Start, SelectionEnd = Editor.Selection.End;
            if (IsEdit) UpdateJsonEventInfo(((SelectionStart.Parent as Inline).Parent as Inline).ToolTip as JsonEventInfo);
            else
            {
                if (Editor.Selection.Text == "") return;
                JsonEventInfo info = new JsonEventInfo();
                if ((SelectionStart.Parent as Inline).Parent == (SelectionEnd.Parent as Inline).Parent && (SelectionStart.Parent as Inline).Parent is Span)
                {
                    object jei = ((SelectionStart.Parent as Inline).Parent as Span).ToolTip;
                    Span tmp1 = new Span(((SelectionStart.Parent as Inline).Parent as Span).ContentStart, SelectionStart) { ToolTip = jei };
                    Span tmp2 = new Span(SelectionEnd, ((SelectionEnd.Parent as Inline).Parent as Span).ElementEnd) { ToolTip = jei };
                }
                else
                {
                    if (SelectionStart.Parent is Span)
                    {
                        JsonEventInfo i = (SelectionStart.Parent as Span).ToolTip as JsonEventInfo;
                        TextPointer spanstart = (SelectionStart.Parent as Span).ContentStart;
                        Span s = new Span(spanstart, Editor.Selection.Start) { ToolTip = i };
                    }
                    if (SelectionEnd.Parent is Span)
                    {
                        JsonEventInfo i = (SelectionEnd.Parent as Span).ToolTip as JsonEventInfo;
                        TextPointer spanend = (SelectionEnd.Parent as Span).ContentEnd;
                        Span s = new Span(Editor.Selection.End, spanend) { ToolTip = i };
                    }
                }

                Span evt = new Span(Editor.Selection.Start, Editor.Selection.End) { Background = EventBrush, ToolTip = info };
                UpdateJsonEventInfo(info);

                List<Span> spans = new List<Span>();
                IEnumerator<Inline> enumerator = evt.Inlines.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is Span) spans.Add(enumerator.Current as Span);
                }
                foreach(Span s in spans)
                {
                    List<Inline> items = new List<Inline>(s.Inlines);
                    s.Inlines.Clear();
                    foreach (Inline i in items) evt.Inlines.InsertAfter(s, i);
                    evt.Inlines.Remove(s);
                }
            }
        }
        void UpdateJsonEventInfo(JsonEventInfo info)
        {
            switch (ClickEvtType.SelectedIndex)
            {
                case 0: info.ClickEventName = ClickEvents.OpenUrl; break;
                case 1: info.ClickEventName = ClickEvents.RunCommand; break;
                case 2: info.ClickEventName = ClickEvents.SuggestCommand; break;
            }
            if (info.ClickEventName != null) info.ClickEventValue = ClickEvtValue.Text; else info.ClickEventValue = null;
            switch (HoverEvtType.SelectedIndex)
            {
                case 0: info.HoverEventName = HoverEvents.ShowText; break;
                case 1: info.HoverEventName = HoverEvents.ShowAchievements; break;
                case 2: info.HoverEventName = HoverEvents.ShowItem; break;
                case 3: info.HoverEventName = HoverEvents.ShowEntity; break;
            }
            if (info.HoverEventName != null) info.HoverEventValue = HoverEvtValue.Text; else info.HoverEventValue = null;
        }

        private void ClickEvtType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClickEvtValue.IsEnabled = true;
            switch (ClickEvtType.SelectedIndex)
            {
                case 0: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(ClickEvtValue, "点击时打开的URL"); break;
                case 1: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(ClickEvtValue, "点击时执行的Minecraft原版命令"); break;
                case 2: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(ClickEvtValue, "点击后在聊天栏添加指定文本"); break;
                case 3: ClickEvtValue.IsEnabled = false; MahApps.Metro.Controls.TextBoxHelper.SetWatermark(ClickEvtValue, ""); break;
            }
        }
        private void HoverEvtType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HoverEvtValue.IsEnabled = true;
            switch (HoverEvtType.SelectedIndex)
            {
                case 0: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(HoverEvtValue, "显示一段文字"); break;
                case 1: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(HoverEvtValue, "显示指定的成就（进度）或统计数据"); break;
                case 2: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(HoverEvtValue, "根据NBT标签显示物品的属性，就像把鼠标放在一个物品上"); break;
                case 3: MahApps.Metro.Controls.TextBoxHelper.SetWatermark(HoverEvtValue, "根据NBT标签显示一个生物的信息"); break;
                case 4: HoverEvtValue.IsEnabled = false; MahApps.Metro.Controls.TextBoxHelper.SetWatermark(HoverEvtValue, ""); break;
            }
        }
    }
    public class JsonEventInfo
    {
        public string HoverEventValue { get; set; }
        public string ClickEventValue { get; set; }
        public HoverEvents? HoverEventName { get; set; } = null;
        public ClickEvents? ClickEventName { get; set; } = null;
    }
    public enum ClickEvents { OpenUrl, RunCommand, SuggestCommand, ChangePage }
    public enum HoverEvents { ShowText, ShowAchievements, ShowItem, ShowEntity }
}