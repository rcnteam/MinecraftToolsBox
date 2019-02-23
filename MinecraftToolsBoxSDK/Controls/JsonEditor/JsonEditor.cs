using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace MinecraftToolsBoxSDK
{
    public class JsonEditor : RichTextBox
    {
        public static JsonSelectorEditor AddSelectorTextUI = new JsonSelectorEditor();
        public static JsonObjectiveEditor AddObjectiveTextUI = new JsonObjectiveEditor();
        public static JsonEventEditor AddEventUI = new JsonEventEditor();

        public ToggleButton Bold { get; set; }
        public ToggleButton Italic { get; set; }
        public ToggleButton Underlined { get; set; }
        public ToggleButton Strikethrough { get; set; }
        public ToggleButton Obfuscated { get; set; }
        public ComboBox ColorPicker { get; set; }
        public IMtbWindow Window { get; set; }
        public EntitySelector EntitySelector { get; set; }
        public Brush DefaultFontColor { get; set; }
        MenuItem item = new MenuItem { Header = "编辑", IsEnabled = false };

        public void InitializeSettings(Button ClearStyle, Button ChangeColor, Button AddSelectorText, Button AddObjective, Button AddEvent, ComboBox TextColor, Brush defaultFontColor)
        {
            Bold.Click += ChangeTextStyle;
            Italic.Click += ChangeTextStyle;
            Underlined.Click += ChangeTextStyle;
            Strikethrough.Click += ChangeTextStyle;
            Obfuscated.Click += ChangeTextStyle;
            ClearStyle.Click += ClearTextStyle;
            ChangeColor.Click += ChangeTextColor;
            if (AddSelectorText != null) AddSelectorText.Click += AddSelector;
            if (AddObjective != null) AddObjective.Click += AddObjectiveScore;
            if (AddEvent != null) AddEvent.Click += this.AddEvent;
            TextColor.SelectionChanged += SetBackground;
            Bold.Focusable = false;
            Italic.Focusable = false;
            Underlined.Focusable = false;
            Strikethrough.Focusable = false;
            Obfuscated.Focusable = false;
            DefaultFontColor = defaultFontColor;

            ContextMenu cm = new ContextMenu();
            cm.Items.Add(item);
            item.Click += EditItem;
            ContextMenu = cm;
        }
        private void SetBackground(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            cb.Background = ((ComboBoxItem)cb.SelectedItem).Background;
        }
        protected override void OnSelectionChanged(RoutedEventArgs e)
        {
            base.OnSelectionChanged(e);
            DependencyObject pointer = Selection.Start.Parent;
            if (pointer is Run)
            {
                bool underline = false, strikethrough = false;
                Run run = pointer as Run;
                foreach (TextDecoration dec in run.TextDecorations)
                {
                    if (dec.Location == TextDecorationLocation.Underline) underline = true;
                    else if (dec.Location == TextDecorationLocation.Strikethrough) strikethrough = true;
                }
                Bold.IsChecked = run.FontWeight == FontWeights.Bold;
                Italic.IsChecked = run.FontStyle == FontStyles.Italic;
                Underlined.IsChecked = underline;
                Strikethrough.IsChecked = strikethrough;
                Obfuscated.IsChecked = run.Background == Brushes.Black;
                item.IsEnabled = false;
            }
            else
            {
                Bold.IsChecked = false;
                Italic.IsChecked = false;
                Underlined.IsChecked = false;
                Strikethrough.IsChecked = false;
                Obfuscated.IsChecked = false;
                item.IsEnabled = false;
            }
            if (pointer != Selection.End.Parent) return;
            if (pointer is Span) item.IsEnabled = true; else if (pointer is Run && (pointer as Run).Parent is Span) item.IsEnabled = true; else item.IsEnabled = false;
        }
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                e.Handled = true;
            }
            else if(e.Key == Key.Delete)
            {
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }

        private void ChangeTextColor(object sender, RoutedEventArgs e)
        {
            if(!ColorPicker.IsMouseOver)Selection.ApplyPropertyValue(TextElement.ForegroundProperty, (ColorPicker.SelectedItem as ComboBoxItem).Background);
        }
        private void ChangeTextStyle(object sender, RoutedEventArgs e)
        {
            if (Selection.Text.Length == 0) return;
            if (sender == Bold) EditingCommands.ToggleBold.Execute(null, this);
            else if (sender == Italic) EditingCommands.ToggleItalic.Execute(null, this);
            else if (sender == Underlined) EditingCommands.ToggleUnderline.Execute(null, this);
            else if (sender == Strikethrough)
            {
                TextPointer s = Selection.Start;
                Span span = new Span(Selection.Start, Selection.End);
                bool isstrikethrough = true;
                foreach (Run item in span.Inlines)
                {
                    bool flag = true;
                    foreach (TextDecoration dec in item.TextDecorations) if (dec.Location == TextDecorationLocation.Strikethrough) flag = false;
                    if (flag) { isstrikethrough = false; break; }
                }
                Inline[] inlines = span.Inlines.ToArray();
                for (int i = 0; i < inlines.Length; i++)
                {
                    Run item =  inlines[i]as Run;
                    TextDecorationCollection collection = item.TextDecorations.Clone();
                    if (isstrikethrough)
                    {
                        TextDecoration dec = null;
                        foreach (TextDecoration d in collection) if (d.Location == TextDecorationLocation.Strikethrough) dec = d;
                        if (dec != null) collection.Remove(dec);
                    }
                    else
                    {
                        if (!item.TextDecorations.Contains(TextDecorations.Strikethrough[0]))collection.Add(TextDecorations.Strikethrough);
                    }
                    Run r = new Run(item.Text, Selection.Start) { TextDecorations = collection, Background = item.Background, FontWeight = item.FontWeight, FontStyle = item.FontStyle, Foreground = item.Foreground };
                }
                EditingCommands.Delete.Execute(null, this);
                Selection.Select(s,Selection.Start);
            }
            else
            {
                if (Obfuscated.IsChecked == true) Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Black);
                else Selection.ApplyPropertyValue(TextElement.BackgroundProperty, null);
            }
        }
        private void ClearTextStyle(object sender, RoutedEventArgs e)
        {
            Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
            Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
            Selection.ApplyPropertyValue(TextElement.BackgroundProperty, null);
            Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Foreground);
        }

        private void AddSelector(object sender, RoutedEventArgs e)
        {
            AddSelectorTextUI.selector.Text = EntitySelector.GetEntity();
            AddSelectorTextUI.Editor = this;
            Window.ShowDialog(AddSelectorTextUI, AddSelectorTextUI.apply, AddSelectorTextUI.cancel);
        }
        private void AddObjectiveScore(object sender, RoutedEventArgs e)
        {
            AddObjectiveTextUI.selector.Text = EntitySelector.GetEntity();
            AddObjectiveTextUI.Editor = this;
            Window.ShowDialog(AddObjectiveTextUI, AddObjectiveTextUI.apply, AddObjectiveTextUI.cancel);
        }
        private void AddEvent(object sender, RoutedEventArgs e)
        {
            AddEventUI.Editor = this;
            AddEventUI.IsEdit = false;
            Window.ShowDialog(AddEventUI, AddEventUI.apply, AddEventUI.cancel);
        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if ((Selection.Start.Parent is Span) && ((Selection.Start.Parent as Span).Inlines.FirstInline as InlineUIContainer).Child is TextBlock item)
            {
                if (item.ToolTip as string == "Selector")
                {
                    AddSelectorTextUI.selector.Text = item.Text;
                    AddSelectorTextUI.Editor = this;
                    Window.ShowDialog(AddSelectorTextUI, AddSelectorTextUI.apply, AddSelectorTextUI.cancel);
                }
                else
                {
                    AddObjectiveTextUI.selector.Text = (item.ToolTip as string).Substring(4);
                    AddObjectiveTextUI.objective.Text = item.Text;
                    Window.ShowDialog(AddObjectiveTextUI, AddObjectiveTextUI.apply, AddSelectorTextUI.cancel);
                }
            }
            else
            {
                JsonEventInfo info = ((Selection.Start.Parent as Inline).Parent as Inline).ToolTip as JsonEventInfo;
                AddEventUI.ClickEvtType.SelectedIndex = info.ClickEventName == null ? 3 : (int)info.ClickEventName;
                AddEventUI.HoverEvtType.SelectedIndex = info.HoverEventName == null ? 4 : (int)info.HoverEventName;
                AddEventUI.ClickEvtValue.Text = info.ClickEventValue;
                AddEventUI.HoverEvtValue.Text = info.HoverEventValue;
                AddEventUI.Editor = this;
                AddEventUI.IsEdit = true;
                Window.ShowDialog(AddEventUI, AddEventUI.apply, AddEventUI.cancel);
            }
        }

        public string GetJsonText()
        {
            List<MinecraftJsonText> group = new List<MinecraftJsonText>();
            foreach(Block b in Document.Blocks)
            {
                if (!(b is Paragraph)) continue;
                foreach(Inline inline in (b as Paragraph).Inlines)
                {
                    MinecraftJsonText text = new MinecraftJsonText();
                    if(inline is Run)
                    {
                        Run r = inline as Run;
                        if (r.Text == "") continue;
                        text.ImportFontStyle(r, DefaultFontColor);
                    }
                    else if(inline is Span)
                    {
                        Span s = inline as Span;
                        if(s.Inlines.FirstInline is InlineUIContainer)
                        {
                            TextBlock r = (s.Inlines.FirstInline as InlineUIContainer).Child as TextBlock;
                            if (r.Text == "") continue;
                            if (r.Background == JsonSelectorEditor.SelectorBrush)
                            {
                                text.Selector = r.Text;
                            }
                            else
                            {
                                text.Score = new Score { Objective = r.Text, Name = r.ToolTip as string };
                            }
                            text.ImportFontStyle(r, DefaultFontColor);
                            text.Text = null;
                        }
                        else
                        {
                            JsonEventInfo evt = s.ToolTip as JsonEventInfo;
                            ClickEvent click = null;HoverEvent hover = null;
                            if (evt.ClickEventName != null) click = new ClickEvent { Action = (ClickEvents)evt.ClickEventName, Value = evt.ClickEventValue };
                            if (evt.HoverEventName != null) hover = new HoverEvent { Action = (HoverEvents)evt.HoverEventName, Value = evt.HoverEventValue };

                            foreach(Inline element in s.Inlines)
                            {
                                MinecraftJsonText t = new MinecraftJsonText { ClickEvent = click, HoverEvent = hover };
                                if (element is Run)
                                {
                                    Run r = element as Run;
                                    if (r.Text == "") continue;
                                    t.ImportFontStyle(r, DefaultFontColor);
                                }
                                else
                                {
                                    TextBlock r = ((element as Span).Inlines.FirstInline as InlineUIContainer).Child as TextBlock;
                                    if (r.Text == "") continue;
                                    t.ImportFontStyle(r, DefaultFontColor);
                                }
                                group.Add(t);
                            }
                        }
                    }
                    group.Add(text);
                }
                if (b != Document.Blocks.LastBlock) group.Add(new MinecraftJsonText { Text = "\n" });
            }
            if (group.Count == 0) return "";
            else if (group.Count == 1) return group[0].GetJsonText();
            else return (new MinecraftJsonText { Text = "", Extra = group.ToArray() }).GetJsonText();
        }
    }
}
//选取在不同段落时无法添加事件
//特殊文本字体格式
//优化输出结果