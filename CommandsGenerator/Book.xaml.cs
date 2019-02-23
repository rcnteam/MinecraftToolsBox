using MinecraftCommandsGenerator.Json;
using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Book.xaml 的交互逻辑
    /// </summary>
    public partial class Book : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate CmdGenerator;
        JsonEditor editing;

        public Book(CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            CmdGenerator = cmdGenerator;
            JsonEditor doc = Page1.doc;
            doc.Bold = bold;
            doc.Italic = italic;
            doc.Underlined = underlined;
            doc.Strikethrough = strikethrough;
            doc.Obfuscated = obfuscated;
            doc.ColorPicker = TextColor;
            doc.Window = (cmdGenerator.Parent as ContentControl).Parent as IMtbWindow;
            doc.EntitySelector = ES;
            doc.InitializeSettings(clearStyle, c, addSelector, addObjective, addEvent, TextColor, doc.Foreground);
            Page1.doc.GotFocus += FocusedChanged;
            editing = Page1.doc;
        }
        public void FocusedChanged(object sender, RoutedEventArgs e)
        {
            editing.Selection.Select(editing.Document.ContentEnd, editing.Document.ContentEnd);
            editing = sender as JsonEditor;
        }
        private void AddPage(object sender, RoutedEventArgs e)
        {
            BookPage p = new BookPage();
            int idx = pageList.Children.Count;
            pageList.Children.Insert(idx - 1, p);
            if (idx < 10) p.pageIdx.Text = string.Format("第0{0}页", idx); else p.pageIdx.Text = string.Format("第{0}页", idx);
            if (pageList.Children.Count == 51) newPage.IsEnabled = false;
            JsonEditor doc = p.doc;
            doc.Bold = bold;
            doc.Italic = italic;
            doc.Underlined = underlined;
            doc.Strikethrough = strikethrough;
            doc.Obfuscated = obfuscated;
            doc.ColorPicker = TextColor;
            doc.Window = (CmdGenerator.Parent as ContentControl).Parent as IMtbWindow;
            doc.EntitySelector = ES;
            doc.InitializeSettings(clearStyle, c, addSelector, addObjective, addEvent, TextColor, doc.Foreground);
            doc.GotFocus += FocusedChanged;
        }

        public string GenerateCommand()
        {
            return "/give " + ES.GetEntity() + " minecraft:written_book " + count.Value + " 0 " + GetTag();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CmdGenerator.AddCommand(GetTag());
            CmdGenerator.nowCmd = "book"; 
        }
        private string GetTag()
        {
            string tit = "title:\"自定义Json书\",", aut = "author:\"author\",";
            if (title.Text != "" && title.Text != null) tit = "title:\"" + title.Text + "\",";
            if (author.Text != "" && author.Text != null) aut = "author:\"" + author.Text + "\",";
            string txt = "";

            for (int i = 0; i < pageList.Children.Count - 1; i++)
            {
                BookPage p = pageList.Children[i] as BookPage;
                txt += "\"" + JsonText.transfer(p.doc.GetJsonText()) + "\",";
            }

            return "{" + tit + aut + "pages:[" + txt.Substring(0, txt.Length - 1) + "]}";
        }

        private void DeleteSelectedPage(object sender, RoutedEventArgs e)
        {
            int index = pageList.Children.IndexOf(editing.Parent as UIElement);
            pageList.Children.RemoveAt(index);
            for (; index < pageList.Children.Count - 1; index++)
            {
                BookPage p = pageList.Children[index] as BookPage;
                if (index + 1 < 10) p.pageIdx.Text = string.Format("第0{0}页", index + 1); else p.pageIdx.Text = string.Format("第{0}页", index + 1);
            }
        }
        private void MoveSelectedPage(object sender, RoutedEventArgs e)
        {
            if (pageList.Children.Count > PageNum.Value)
            { 
                int idx = (int)PageNum.Value - 1;
                if (idx > pageList.Children.Count - 1) idx = pageList.Children.Count - 1;
                BookPage p = editing.Parent as BookPage;
                pageList.Children.Remove(p);
                pageList.Children.Insert(idx, p);
                for (int i = 0; i < pageList.Children.Count - 1; i++)
                {
                    if (i + 1 < 10) (pageList.Children[i] as BookPage).pageIdx.Text = string.Format("第0{0}页", i + 1); else (pageList.Children[i] as BookPage).pageIdx.Text = string.Format("第{0}页", i + 1);
                }
            }
        }
    }
}
