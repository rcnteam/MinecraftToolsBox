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
    public partial class CommandEditor : CustomDialog
    {
        public TreeDataGridItem Item;
        public MapTranslator Translator;

        public CommandEditor()
        {
            InitializeComponent();
        }
        public void SetCurrentItem(TreeDataGridItem item, MapTranslator translator)
        {
            Item = item;
            Translator = translator;
            original.Text = item.Original;
            translated.Text = item.Translated;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MetroWindow).HideMetroDialogAsync(this);
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow as MetroWindow).HideMetroDialogAsync(this);
            Item.Translated = translated.Text;
            Translator.DialogueClosed();
        }
    }
}
