using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MinecraftToolsBoxSDK;
using TranslationTools;

namespace MTB.GameHandler
{
    /// <summary>
    /// MapTranslator.xaml 的交互逻辑
    /// </summary>
    public partial class MapTranslator : Grid
    {
        int changes;
        DispatcherTimer timer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 30) };
        XmlDocument doc = new XmlDocument();
        XmlNode root;
        string filePath;
        DateTime lastSave;
        List<TreeDataGridItem> results = new List<TreeDataGridItem>();
        CommandEditor cmdDialog = new CommandEditor();
        SignEditor signDialog = new SignEditor();

        public MapTranslator()
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();
            InitializeComponent();
            if (result == System.Windows.Forms.DialogResult.Cancel) return;
            string directory = m_Dialog.SelectedPath.Trim();
            filePath = directory;

            string[] split = filePath.Split('\\');
            filePath= filePath + "\\.translation\\" + split[split.Length - 1] + ".mtbl";
            
            doc.Load(filePath);
            root = doc.SelectSingleNode("LanguageItems");
            if (root.Attributes["CommandBlocks"].Value == "True")
            {
                int.TryParse(root.ChildNodes[1].Attributes["StartIndex"].Value, out int index);
                TreeDataGridItem ldi = new TreeDataGridItem { Type = "命令" };
                foreach (XmlNode item in root.ChildNodes[1].ChildNodes)
                    ldi.Children.Add(new TreeDataGridItemCommandBlock(item, index++));
                list.Children.Add(ldi);
            }
            if (root.Attributes["Signs"].Value == "True")
            {
                int.TryParse(root.ChildNodes[2].Attributes["StartIndex"].Value, out int index);
                TreeDataGridItem ldi = new TreeDataGridItem { Type = "告示牌" };
                foreach (XmlNode item in root.ChildNodes[2].ChildNodes)
                    ldi.Children.Add(new TreeDataGridItemSign(item, index++));
                list.Children.Add(ldi);
            }
            if (root.Attributes["Containers"].Value == "True")
            {
                int.TryParse(root.ChildNodes[3].Attributes["StartIndex"].Value, out int index);
                TreeDataGridItem ldi = new TreeDataGridItem { Type = "容器" };
                foreach (XmlNode item in root.ChildNodes[3].ChildNodes)
                    ldi.Children.Add(new TreeDataGridItemContainer(item, index++));
                list.Children.Add(ldi);
            }
            if (root.Attributes["Entities"].Value == "True")
            {
                int.TryParse(root.ChildNodes[4].Attributes["StartIndex"].Value, out int index);
                TreeDataGridItem ldi = new TreeDataGridItem { Type = "实体" };
                foreach (XmlNode item in root.ChildNodes[4].ChildNodes)
                    ldi.Children.Add(new TreeDataGridItemEntity(item, index++));
                list.Children.Add(ldi);
            }
            if (root.Attributes["Spawners"].Value == "True")
            {
                int.TryParse(root.ChildNodes[5].Attributes["StartIndex"].Value, out int index);
                TreeDataGridItem ldi = new TreeDataGridItem { Type = "刷怪笼" };
                foreach (XmlNode item in root.ChildNodes[5].ChildNodes)
                    ldi.Children.Add(new TreeDataGridItemMobSpawner(item, index++));
                list.Children.Add(ldi);
            }

            timer.Tick += delegate 
            {
                doc.Save(filePath);
                Dispatcher.Invoke(() => 
                {
                    Save(true);
                });
            };
            timer.Start();
        }

        private void Save(bool isAuto = false)
        {
            doc.Save(filePath);
            changes = 0;
            saveInfo.Text = "已在" + (lastSave = DateTime.Now).ToLongTimeString() + (isAuto ? "自动保存" : "保存");
        }

        private void List_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //LanguageDataItem item = list.SelectedItem as LanguageDataItem;
            //if (list.editing != null)
            //{
            //    if (list.editing.Type == "告示牌")
            //    {
            //        if ((TL1.Text != "" || TL2.Text != "" || TL3.Text != "" || TL4.Text != "") && (TL1.Text != OL1.Text || TL2.Text != OL2.Text || TL3.Text != OL3.Text || TL4.Text != OL4.Text))
            //        {
            //            ObservableCollection<LanguageDataItem> sign = list.editing.Children;
            //            (sign[0].Node as XmlElement).SetAttribute("Translated", TL1.Text);
            //            (sign[1].Node as XmlElement).SetAttribute("Translated", TL2.Text);
            //            (sign[2].Node as XmlElement).SetAttribute("Translated", TL3.Text);
            //            (sign[3].Node as XmlElement).SetAttribute("Translated", TL4.Text);
            //            (sign[0].Translated as TextBlock).Text = TL1.Text;
            //            (sign[1].Translated as TextBlock).Text = TL2.Text;
            //            (sign[2].Translated as TextBlock).Text = TL3.Text;
            //            (sign[3].Translated as TextBlock).Text = TL4.Text;
            //            (Application.Current.MainWindow as IMainWindowCommands).ChangeWindowTitle("[*]");
            //        }
            //    }
            //    else if (list.editing.Children.Count != 0)
            //    {
                    
            //    }
            //    else
            //    {
            //        string val = tra.Text;
            //        if (val != "" && val != ori.Text) { (list.editing.Node as XmlElement).SetAttribute("Translated", val); (Application.Current.MainWindow as IMainWindowCommands).ChangeWindowTitle("[*]"); }
            //        (list.editing.Translated as TextBlock).Text = val;
            //    }
            //}
            //if (item.Type == "告示牌")
            //{
            //    ObservableCollection<LanguageDataItem> sign = item.Children;
            //    OL1.Text = sign[0].Original as string;
            //    OL2.Text = sign[1].Original as string;
            //    OL3.Text = sign[2].Original as string;
            //    OL4.Text = sign[3].Original as string;

            //    TL1.Text = (sign[0].Translated as TextBlock).Text;
            //    TL2.Text = (sign[1].Translated as TextBlock).Text;
            //    TL3.Text = (sign[2].Translated as TextBlock).Text;
            //    TL4.Text = (sign[3].Translated as TextBlock).Text;
            //    editor.SelectedIndex = 2;
            //    if (TL1.Text == "" && TL2.Text == "" && TL3.Text == "" && TL4.Text == "" && (OL1.Text != "" || OL2.Text != "" || OL3.Text != "" || OL4.Text != ""))
            //    {
            //        TL1.Text = OL1.Text;
            //        TL2.Text = OL2.Text;
            //        TL3.Text = OL3.Text;
            //        TL4.Text = OL4.Text;
            //    }
            //}
            //else if (item.Children.Count != 0)
            //{
            //    editor.SelectedIndex = 3;
            //}
            //else
            //{
            //    editor.SelectedIndex = 0;
            //    ori.Text = item.Original as string;
            //    tra.Text = (item.Translated as TextBlock).Text;
            //    if (tra.Text == "" && ori.Text != "") tra.Text = ori.Text;
            //}
        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeDataGridItem item = list.SelectedItem as TreeDataGridItem;
            if (item.Parent != null && (item.Parent as TreeDataGridItem).Type == "命令")
            {
                cmdDialog.SetCurrentItem(item, this);
                (Application.Current.MainWindow as MetroWindow).ShowMetroDialogAsync(cmdDialog);
            }
            else if(item.Parent != null && (item.Parent as TreeDataGridItem).Type == "告示牌")
            {
                signDialog.SetCurrentItem(item, this);
                (Application.Current.MainWindow as MetroWindow).ShowMetroDialogAsync(signDialog);
            }
            else if(!item.HasChildren)
            {
                DataGridRow row = (DataGridRow)list.ItemContainerGenerator.ContainerFromIndex(list.SelectedIndex);
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(row);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(2);
                cell.Focus();
                list.BeginEdit();
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S) Save();
            else if (e.Key == Key.Enter)
            {
                /*if (list.SelectedItem != null) (list.SelectedItem as LanguageDataItem).Start();*/
            }
        }

        private void DockPanel_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        public void list_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            changes++;
            if (saveInfo.Text == "无更改" || !saveInfo.Text.Contains("保存")) saveInfo.Text = changes + "处更改";
            else
            {
                string[] split = saveInfo.Text.Split('，');
                saveInfo.Text = changes + "处更改，" + split[split.Length - 1];
            }
        }

        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T childContent = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                childContent = v as T;
                if (childContent == null)
                {
                    childContent = GetVisualChild<T>(v);
                }
                if (childContent != null)
                { break; }
            }
            return childContent;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.SelectedIndex != searchResults.Items.Count - 1)searchResults.SelectedIndex++;
        }

        private void Last_Click(object sender, RoutedEventArgs e)
        {
            if (searchResults.SelectedIndex != 0) searchResults.SelectedIndex--;
        }

        private void searchResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TreeDataGridItem item = results[searchResults.SelectedIndex];
            TreeDataGridItem tmp = item;
            while ((item = item.Parent as TreeDataGridItem) != null)
            {
                item.IsExpanded = true;
            }
            int i = list.Items.IndexOf(tmp);
            list.SelectedIndex = i;
            list.ScrollIntoView(tmp);
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            results.Clear();
            searchResults.Items.Clear();
            foreach (TreeDataGridItem item in list.Children)
            {
                GetSearchResult(item, KeyWord.Text);
            }
        }

        private void GetSearchResult(TreeDataGridItem item, string keyword)
        {
            if (item.HasChildren)
            {
                foreach (TreeDataGridItem i in item.Children)
                {
                    GetSearchResult(i, keyword);
                }
            }
            else
            {
                if ((item.Original != null && item.Original.Contains(keyword) || item.Translated != null && item.Translated.Contains(keyword)) && item.Parent != null)
                {
                    string str = item.Type;
                    TreeDataGridItem parent = item;
                    while ((parent = parent.Parent as TreeDataGridItem).Parent != null)
                    {
                        str = parent.Type + ">" + str;
                    }
                    searchResults.Items.Add(str + item.Original + "\n" + item.Translated);
                    results.Add(item);
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e) => searchWindow.Visibility = Visibility.Visible;

        private void closeSearch(object sender, RoutedEventArgs e) => searchWindow.Visibility = Visibility.Hidden;

        public void DialogueClosed()
        {
            list_CellEditEnding(null, null);
            list.Items.Refresh();
            list.Focus();
        }
    }
}