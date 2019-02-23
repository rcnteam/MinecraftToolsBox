using MinecraftToolsBoxSDK;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Tellraw.xaml 的交互逻辑
    /// </summary>
    public partial class Tellraw : Grid, ICommandGenerator
    {
        public Tellraw(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            doc.Bold = bold;
            doc.Italic = italic;
            doc.Underlined = underlined;
            doc.Strikethrough = strikethrough;
            doc.Obfuscated = obfuscated;
            doc.ColorPicker = TextColor;
            doc.Window = (tmp.Parent as ContentControl).Parent as IMtbWindow;
            doc.EntitySelector = ES;
            doc.InitializeSettings(clearStyle, c, addSelector, addObjective, addEvent, TextColor, doc.Foreground);
        }

        public string GenerateCommand()
        {
            return "/tellraw " + ES.GetEntity() + " " + doc.GetJsonText();
        }
    }
}
