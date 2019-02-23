using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MTB.GameHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TranslationTools
{
    /// <summary>
    /// CommandEditor.xaml 的交互逻辑
    /// </summary>
    public partial class SignEditor : CustomDialog
    {
        public TreeDataGridItem Item;
        public MapTranslator Translator;

        public SignEditor()
        {
            InitializeComponent();
        }
        public void SetCurrentItem(TreeDataGridItem item, MapTranslator translator)
        {
            Item = item;
            Translator = translator;

            O1.Text = (item.Children[0] as TreeDataGridItem).Original;
            O2.Text = (item.Children[1] as TreeDataGridItem).Original;
            O3.Text = (item.Children[2] as TreeDataGridItem).Original;
            O4.Text = (item.Children[3] as TreeDataGridItem).Original;

            T1.Text = (item.Children[0] as TreeDataGridItem).Translated;
            T2.Text = (item.Children[1] as TreeDataGridItem).Translated;
            T3.Text = (item.Children[2] as TreeDataGridItem).Translated;
            T4.Text = (item.Children[3] as TreeDataGridItem).Translated;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MetroWindow).HideMetroDialogAsync(this);
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MetroWindow).HideMetroDialogAsync(this);
            (Item.Children[0] as TreeDataGridItem).Translated = T1.Text;
            (Item.Children[1] as TreeDataGridItem).Translated = T2.Text;
            (Item.Children[2] as TreeDataGridItem).Translated = T3.Text;
            (Item.Children[3] as TreeDataGridItem).Translated = T4.Text;
            Translator.DialogueClosed();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            Grid g = (sender as TextBox).Parent as Grid;
            int i = g.Children.IndexOf(sender as UIElement) + 1;
            Keyboard.Focus(g.Children[i > 3 ? 0 : i] as TextBox);
        }
    }
}
//sign click command