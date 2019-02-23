using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MinecraftToolsBoxSDK;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Firework.xaml 的交互逻辑
    /// </summary>
    public partial class Firework : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate Tmp;
        public ObservableCollection<FireworkItem> Data = new ObservableCollection<FireworkItem>();
        public static ColorEditor CE = new ColorEditor();
        public Firework(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            Tmp = tmp;
            dg.DataContext = Data;
            cbc.ItemsSource = new List<string> { "小球", "大球", "星形", "苦力怕", "散射" };
        }
        private void Fireworks_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            dg.Items.Refresh();
        }

        private void Fireworks_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            dg.Items.Refresh();
        }
        public string GenerateCommand()
        {
            return "";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CE.ColorPicker.SelectedColor = Colors.White;
            CE.DG = dg;CE.Mode = "Color";
            dg.CancelEdit(DataGridEditingUnit.Row);
            if (Data.Count == 0 || (!(dg.SelectedItem is FireworkItem) && dg.SelectedItem != null)) { Data.Add(new FireworkItem()); CE.TargetItem = Data.Last(); }
            else if (dg.SelectedItem != null) CE.TargetItem = dg.SelectedItem;
            ((Tmp.Parent as ContentControl).Parent as IMtbWindow).ShowDialog(CE, CE.Apply, CE.Cancel);
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            CE.ColorPicker.SelectedColor = Colors.White;
            CE.DG = dg; CE.Mode = "FadeColor";
            dg.CancelEdit(DataGridEditingUnit.Row);
            if (Data.Count == 0 || (!(dg.SelectedItem is FireworkItem) && dg.SelectedItem != null)) { Data.Add(new FireworkItem()); CE.TargetItem = Data.Last(); }
            else if (dg.SelectedItem != null) CE.TargetItem = dg.SelectedItem;
            ((Tmp.Parent as ContentControl).Parent as IMtbWindow).ShowDialog(CE, CE.Apply, CE.Cancel);
        }
    }
    public class FireworkItem
    {
        public string Type { get; set; } = "小球";
        public bool Trail { get; set; }
        public bool Flicker { get; set; }
        public SolidColorBrush Color { get; set; } = new SolidColorBrush();
        public SolidColorBrush FadeColor { get; set; } = new SolidColorBrush();
    }
}
