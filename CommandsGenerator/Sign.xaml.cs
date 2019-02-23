using MinecraftCommandsGenerator.Json;
using MinecraftToolsBoxSDK;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Sign.xaml 的交互逻辑
    /// </summary>
    public partial class Sign : Grid, ICommandGenerator
    {
        private string state="standing_sign";//挂在墙上，立在地上
        CommandsGeneratorTemplate CmdGenerator;
        static BitmapImage wallSign = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/Wall_Sign.png"));
        static BitmapImage standingSign = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/Standing_Sign.png"));
        JsonEditor editing;

        public Sign(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            CmdGenerator = tmp;
            line1.Bold = bold;
            line1.Italic = italic;
            line1.Underlined = underlined;
            line1.Strikethrough = strikethrough;
            line1.Obfuscated = obfuscated;
            line1.ColorPicker = TextColor;
            line1.Window = (tmp.Parent as ContentControl).Parent as IMtbWindow;
            line1.EntitySelector = ES;
            line1.InitializeSettings(clearStyle, c, null, null, null, TextColor, line1.Foreground);
            line2.Bold = bold;
            line2.Italic = italic;
            line2.Underlined = underlined;
            line2.Strikethrough = strikethrough;
            line2.Obfuscated = obfuscated;
            line2.ColorPicker = TextColor;
            line2.Window = (tmp.Parent as ContentControl).Parent as IMtbWindow;
            line2.EntitySelector = ES;
            line2.InitializeSettings(clearStyle, c, null, null, null, TextColor, line2.Foreground);
            line3.Bold = bold;
            line3.Italic = italic;
            line3.Underlined = underlined;
            line3.Strikethrough = strikethrough;
            line3.Obfuscated = obfuscated;
            line3.ColorPicker = TextColor;
            line3.Window = (tmp.Parent as ContentControl).Parent as IMtbWindow;
            line3.EntitySelector = ES;
            line3.InitializeSettings(clearStyle, c, null, null, null, TextColor, line3.Foreground);
            line4.Bold = bold;
            line4.Italic = italic;
            line4.Underlined = underlined;
            line4.Strikethrough = strikethrough;
            line4.Obfuscated = obfuscated;
            line4.ColorPicker = TextColor;
            line4.Window = (tmp.Parent as ContentControl).Parent as IMtbWindow;
            line4.EntitySelector = ES;
            line4.InitializeSettings(clearStyle, c, null, null, null, TextColor, line4.Foreground);
            EditingCommands.AlignCenter.Execute(null, line1);
            EditingCommands.AlignCenter.Execute(null, line2);
            EditingCommands.AlignCenter.Execute(null, line3);
            EditingCommands.AlignCenter.Execute(null, line4);
            editing = line1;
            addSelector.Click += AddSelector_Click;
            addObjective.Click += AddObjective_Click;
        }

        private void AddObjective_Click(object sender, RoutedEventArgs e)
        {
            JsonEditor.AddObjectiveTextUI.Selector.Text = ES.GetEntity();
            JsonEditor.AddObjectiveTextUI.Editor = editing;
            editing.Window.ShowDialog(JsonEditor.AddObjectiveTextUI, JsonEditor.AddObjectiveTextUI.Apply, JsonEditor.AddObjectiveTextUI.Cancel);
        }
        private void AddSelector_Click(object sender, RoutedEventArgs e)
        {
            JsonEditor.AddSelectorTextUI.Selector.Text = ES.GetEntity();
            JsonEditor.AddSelectorTextUI.Editor = editing;
            editing.Window.ShowDialog(JsonEditor.AddSelectorTextUI, JsonEditor.AddSelectorTextUI.Apply, JsonEditor.AddSelectorTextUI.Cancel);
        }

        private void SignChanged(object sender, RoutedEventArgs e)
        {
            if (wallsign.IsChecked == true)
            {
                state = "wall_sign";
                stand.Source = wallSign;

                if (NW != null) NW.Visibility = Visibility.Hidden;
                if (NWN != null) NWN.Visibility = Visibility.Hidden;
                if (NWW != null) NWW.Visibility = Visibility.Hidden;
                if (NE != null) NE.Visibility = Visibility.Hidden;
                if (NEN != null) NEN.Visibility = Visibility.Hidden;
                if (NEE != null) NEE.Visibility = Visibility.Hidden;
                if (SE != null) SE.Visibility = Visibility.Hidden;
                if (SES != null) SES.Visibility = Visibility.Hidden;
                if (SEE != null) SEE.Visibility = Visibility.Hidden;
                if (SW != null) SW.Visibility = Visibility.Hidden;
                if (SWS != null) SWS.Visibility = Visibility.Hidden;
                if (SWW != null) SWW.Visibility = Visibility.Hidden;
                S.IsChecked = true;
            }
            else{
                state = "standing_sign";
                stand.Source = standingSign;

                if (NW != null) NW.Visibility = Visibility.Visible;
                if (NWN != null) NWN.Visibility = Visibility.Visible;
                if (NWW != null) NWW.Visibility = Visibility.Visible;
                if (NE != null) NE.Visibility = Visibility.Visible;
                if (NEN != null) NEN.Visibility = Visibility.Visible;
                if (NEE != null) NEE.Visibility = Visibility.Visible;
                if (SE != null) SE.Visibility = Visibility.Visible;
                if (SES != null) SES.Visibility = Visibility.Visible;
                if (SEE != null) SEE.Visibility = Visibility.Visible;
                if (SW != null) SW.Visibility = Visibility.Visible;
                if (SWS != null) SWS.Visibility = Visibility.Visible;
                if (SWW != null) SWW.Visibility = Visibility.Visible;
            }
        }

        public string GenerateCommand()
        {
            string info = "";
            if (give.IsChecked == true) info = "/give " + ES.GetEntity() + " minecraft:sign " + amount.Value + " 0 ";
            if (block.IsChecked == true)
            {
                if (isData.IsChecked == true) info = "/blockdata "; else info = "/setblock ";
                if (tilde.IsChecked == true) info += "~" + LocX.Text + " ~" + LocY.Text + " ~" + LocZ.Text;
                else
                {
                    if (LocX.Text == "" || LocY.Text == "" || LocZ.Text == "") return "请填写坐标";
                    else info += LocX.Text + " " + LocY.Text + " " + LocZ.Text;
                }
                if (isData.IsChecked == false)
                {
                    int face = 5;
                    if (state == "standing_sign")
                    {
                        if (S.IsChecked == true) face = 0;
                        if (SWS.IsChecked == true) face = 1;
                        if (SW.IsChecked == true) face = 2;
                        if (SWW.IsChecked == true) face = 3;
                        if (W.IsChecked == true) face = 4;
                        if (NWW.IsChecked == true) face = 5;
                        if (NW.IsChecked == true) face = 6;
                        if (NWN.IsChecked == true) face = 7;
                        if (N.IsChecked == true) face = 8;
                        if (NEN.IsChecked == true) face = 9;
                        if (NE.IsChecked == true) face = 10;
                        if (NEE.IsChecked == true) face = 11;
                        if (E.IsChecked == true) face = 12;
                        if (NEE.IsChecked == true) face = 13;
                        if (NE.IsChecked == true) face = 14;
                        if (NEN.IsChecked == true) face = 15;
                    }
                    else
                    {
                        if (E.IsChecked == true) face = 5;
                        if (S.IsChecked == true) face = 3;
                        if (W.IsChecked == true) face = 4;
                        if (N.IsChecked == true) face = 2;
                    }
                    info += " minecraft:" + state + " " + face + " ";
                    if (isData.IsChecked != true) info += "replace ";
                }
                else info += " ";
            }
            if (item.IsChecked == true) info = "/replaceitem entity " + ES.GetEntity() + " " + " slot.inventory." + ivt.Text + " minecraft:sign 1 0 ";
            return info + GetTag();
        }
        private string GetTag()
        {
            string text1 = JsonText.transfer(line1.GetJsonText());
            string text2 = JsonText.transfer(line2.GetJsonText());
            string text3 = JsonText.transfer(line3.GetJsonText());
            string text4 = JsonText.transfer(line4.GetJsonText());

            if (Cmd1.Text != "") text1 = text1.Substring(0, text1.Length - 1) + ",\\\"clickEvent\\\":{\\\"action\\\":\\\"run_command\\\",\\\"value\\\":\\\"" + Cmd1.Text + "\\\"}}";
            if (Cmd2.Text != "") text2 = text2.Substring(0, text2.Length - 1) + ",\\\"clickEvent\\\":{\\\"action\\\":\\\"run_command\\\",\\\"value\\\":\\\"" + Cmd2.Text + "\\\"}}";
            if (Cmd3.Text != "") text3 = text3.Substring(0, text3.Length - 1) + ",\\\"clickEvent\\\":{\\\"action\\\":\\\"run_command\\\",\\\"value\\\":\\\"" + Cmd3.Text + "\\\"}}";
            if (Cmd4.Text != "") text4 = text4.Substring(0, text4.Length - 1) + ",\\\"clickEvent\\\":{\\\"action\\\":\\\"run_command\\\",\\\"value\\\":\\\"" + Cmd4.Text + "\\\"}}";

            if (give.IsChecked == true || item.IsChecked == true) return "{BlockEntityTag:{Text1:\"" + text1 + "\",Text2:\"" + text2 + "\",Text3:\"" + text3 + "\",Text4:\"" + text4 + "\"}}";
            else return "{Text1:\"" + text1 + "\",Text2:\"" + text2 + "\",Text3:\"" + text3 + "\",Text4:\"" + text4 + "\"}";
        }
        private void GetTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender == getTag_Item) { give.IsChecked = true; CmdGenerator.AddCommand(GetTag()); CmdGenerator.nowCmd = "sign_item"; }
            else { block.IsChecked = true; CmdGenerator.AddCommand(GetTag()); CmdGenerator.nowCmd = "sign_block"; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isData.IsChecked == true) direction.IsEnabled = false; else direction.IsEnabled = true;
        }
        private void FocusedChanged(object sender, RoutedEventArgs e)
        {
            editing.Selection.Select(editing.Document.ContentEnd, editing.Document.ContentEnd);
            editing = sender as JsonEditor;
        }
        private void EnterPressed(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (sender == line1) line2.Focus();
                else if (sender == line2) line3.Focus();
                else if (sender == line3) line4.Focus();
                else line1.Focus();
            }
        }
    }
}