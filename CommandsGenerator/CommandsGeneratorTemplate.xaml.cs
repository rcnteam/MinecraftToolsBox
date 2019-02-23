using MinecraftToolsBoxSDK;
using System;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock.Layout;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// CommandsGeneratorTemplate.xaml 的交互逻辑
    /// </summary>
    public partial class CommandsGeneratorTemplate : Grid
    {
        public MtbWindow win = null;
        public string nowCmd = "";
        CommandsGenerator.CommandsGenerator parent = null;

        public CommandsGeneratorTemplate(CommandsGenerator.CommandsGenerator par)
        {
            InitializeComponent();
            parent = par;
        }

        public void AddCommand(string command)
        {
            GC.Collect();
            if (command == "") { Output.Text = ""; return; }
            Output.Text = command;
            updateHistory(command);
        }
        public void updateHistory(string command)
        {
            if (History.Text == "") { History.Text = command; } else { History.Text = command + "\n" + History.Text; }
        }
        private void delete_history(object sender, RoutedEventArgs e)
        {
            History.Text = "";
            nowCmd = "command";
        }
        private void clear_control(object sender, RoutedEventArgs e)
        {
            switch ((string)clear.Content)
            {
                case "清空输出区": Output.Text = ""; break;
                case "清空历史记录": History.Text = ""; break;
            }
        }
        private void OUTPUT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (OUTPUT.SelectedIndex)
            {
                case 0: clear.Content = "清空输出区"; break;
                case 1: clear.Content = "清空历史记录"; break;
            }
        }
        private void GenerateCommand(object sender, RoutedEventArgs e)
        {
            nowCmd = "command";
            AddCommand(((ICommandGenerator)layOutPane.SelectedContent.Content).GenerateCommand());
            GC.Collect();
        }
        private void copy_Click(object sender, RoutedEventArgs e)
        {
            Output.SelectAll();
            Output.Copy();
        }
        private void saveCommand_Click(object sender, RoutedEventArgs e)
        {
            //if (w != null) w.Close();
            //w = null;
            //w = new SaveCommand(Output.Text);
            //w.Show();
            //GC.Collect();
        }
        public void SaveCommand(string cmd)
        {
            //if (w != null) w.Close();
            //w = null;
            //w = new SaveCommand(cmd);
            //w.Show();
            //GC.Collect();
        }
        public void AddPage(string title, object content)
        {
            layOutPane.Children.Add(new LayoutAnchorable { Title = title, Content = content, IsSelected = true });
        }
        private void layOutPane_ChildrenCollectionChanged(object sender, EventArgs e)
        {
            if (layOutPane.Children.Count == 0)
            {
                parent.Window = null;
                win.Close();
            }
        }
    }
}
