using MinecraftToolsBoxSDK;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Title.xaml 的交互逻辑
    /// </summary>
    public partial class Title : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate CmdGenWindow;
        public Title(CommandsGeneratorTemplate cmdGenWindow)
        {
            InitializeComponent();
            CmdGenWindow = cmdGenWindow;
            doc.Bold = bold;
            doc.Italic = italic;
            doc.Underlined = underlined;
            doc.Strikethrough = strikethrough;
            doc.Obfuscated = obfuscated;
            doc.ColorPicker = TextColor;
            doc.Window = (cmdGenWindow.Parent as ContentControl).Parent as IMtbWindow;
            doc.EntitySelector = ES;
            doc.InitializeSettings(clearStyle, c, addSelector, addObjective, null, TextColor, doc.Foreground);
        }
        public string GenerateCommand()
        {
            if (setTime.IsChecked == true) return "/title " + ES.GetEntity() + " times " + fadeIn.Value + " " + stay.Value + " " + fadeOut.Value;
            else
            {
                string cmd = "";
                if (T.IsChecked == true) cmd = "/title " + ES.GetEntity() + " title "; else if (subT.IsChecked == true) cmd = "/title " + ES.GetEntity() + " subtitle "; else cmd = "/title " + ES.GetEntity() + " actionbar ";
                return cmd + doc.GetJsonText();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == clear) CmdGenWindow.AddCommand("/title " + ES.GetEntity() + " clear");
            else CmdGenWindow.AddCommand("/title " + ES.GetEntity() + " reset");
        }
    }
}