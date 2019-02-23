using MahApps.Metro.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;

namespace MinecraftToolsBox.Database
{
    /// <summary>
    /// DataBase.xaml 的交互逻辑
    /// </summary>
    public partial class DataBase : MetroWindow
    {
        ObservableCollection<DATA> Data = new ObservableCollection<DATA>();
        TextBox txtb;

        public DataBase()
        {
            InitializeComponent();
            dg.DataContext = Data;
            refreshTreeView();
            updateDataGrid();
            txtb = command;
        }
        
        void refreshTreeView()
        {
            root.Items.Clear();
            FileStream data = new FileStream("dataBase/root.txt", FileMode.Open);
            StreamReader baseRoot = new StreamReader(data, Encoding.UTF8);
            baseRoot.BaseStream.Seek(0, SeekOrigin.Begin);
            string folders = baseRoot.ReadLine();
            folders = folders.Substring(4, folders.Length - 4);

            string[] children = folders.Split(',');
            if (children[0] != "")
                for (int i = 0; i < children.Length; i++)
                {
                    TreeViewItem t = new TreeViewItem();
                    t.Header = children[i];
                    root.Items.Add(t);
                    new Folder(children[i], t);
                }
            data.Close();
            data.Dispose();
            baseRoot.Close();
            baseRoot.Dispose();
        }
        /// <summary>
        /// 更新数据列表：从数据库重新读取
        /// </summary>
        private void updateDataGrid()
        {
            progressLoad.Value = 0;

            Data.Clear();
            FileStream stream = new FileStream(getFileLoc(file.Text), FileMode.Open);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            reader.ReadLine();

            string line = reader.ReadToEnd();
            string[] split = line.Split('\n');
            progressLoad.Maximum = split.LongLength;

            for (long i = 0; i < split.LongLength - 1; i++)
            {
                string[] sp = split[i].Replace("\r", "").Split('\t');
                Data.Add(new DATA { type = sp[0], data = sp[1], des = sp[2] });
                progressLoad.Value += 1;
            }
            string last = split[split.Length - 1];
            if (last != "\t\t" && last != "")
            {
                string[] sp = last.Split('\t');
                if (sp.Length > 2)Data.Add(new DATA { type = sp[0], data = sp[1], des = sp[2] });
                progressLoad.Value += 1;
            }
            stream.Close();
            dg.DataContext = Data;
        }

        string getFileLoc(string path)
        {
            path = path.Replace("ROOT://", "dataBase/").Replace(">", "/");
            string[] split = path.Split('/'); path = "";
            for (int i = 0; i < split.Length - 1; i++) path += "/" + split[i];
            path = path.Substring(1, path.Length - 1) + "/root.txt";
            return path;
        }
        /// <summary>
        /// 写入数据库
        /// </summary>
        private void updateDataBase()
        {
            progressSave.Value = 0;
            progressSave.Maximum = Data.Count;

            string path = getFileLoc(file.Text);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string index = reader.ReadLine();

            for (int i = 0; i < Data.Count; i++) { index += "\r\n"+Data[i].getDataBaseText(); progressSave.Value += 1; }
            
            stream.Close();reader.Dispose();reader = null;
            stream = new FileStream(path, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(index);
            writer.Flush();
            writer.Close();
            stream.Close();
        }

        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            for (int i = 0; i < Data.Count; i++) Data[i].idx = i + 1 + "";
            dg.RowHeight = 30;
        }
        private void folder_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (file == null) return;
            updateDataBase();
            file.Text = getFileLoc((TreeViewItem)folder.SelectedItem);
            updateDataGrid();
        }
        string getFileLoc(TreeViewItem parent)
        {
            if (parent == root) return "ROOT://";

            ObservableCollection<string> loc = new ObservableCollection<string>();
            loc.Add((string)parent.Header);

            while (parent != root)
            {
                parent = (TreeViewItem)parent.Parent;
                loc.Insert(0, (string)parent.Header);
            }
                
            string location = "ROOT://";
            for (int i = 1; i < loc.Count; i++) location += loc[i] + ">";
            return location;
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            updateDataBase();
            MainWindow.db = null;
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem t = (TreeViewItem)folder.SelectedItem;
            if (t == root) return;
            string path = getFileLoc(t);
            ((TreeViewItem)t.Parent).IsSelected = true;
            string[] p = path.Split('>'); path = "";
            for(int i = 0; i < p.Length - 2; i++) path += ">"+p[i];
            path = path.Replace(">ROOT://","ROOT://");
            if (path == "") path = "ROOT://"; else path += ">";
            file.Text = path;
        }
        private void folderName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) e.Handled = true;
        }
        private void folderName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string t = e.Text;
            if (t == "/" || t == "\\" || t == "," || t == "<" || t == ">" || t == "?" || t == "*" || t == "\"") e.Handled = true;
        }
        private void newFolder_Click(object sender, RoutedEventArgs e)
        {
            if (folderName.Text == "") { this.ShowMessageAsync("无法创建文件夹", "文件夹名称不能为空", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" }); return; }
            string fileLoc = getFileLoc((TreeViewItem)folder.SelectedItem).Replace("ROOT://", "dataBase/").Replace(">", "/") + "root.txt";

            FileStream file = new FileStream(fileLoc, FileMode.Open);
            StreamReader reader = new StreamReader(file, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            string index = reader.ReadLine();
            string index1 = index.Substring(4);
            string[] folders = index1.Split(','); index1 = null;

            string newFolderName = folderName.Text;
            StreamWriter writer = new StreamWriter(file, Encoding.UTF8);

            string allTxt = reader.ReadToEnd();
            if (allTxt == "\n") allTxt = "";
            file.Close();
            FileStream f;
            if (folders.Length == 1 && folders[0] == "") writer.Write("文件夹：" + newFolderName + "\r\n" + allTxt);
            else
            {
                for (int i = 0; i < folders.Length; i++) if (folders[i] == newFolderName) { this.ShowMessageAsync("该名称已被使用", "已存在名为 " + folderName.Text + " 的文件夹", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" }); file.Close(); file.Dispose(); return; }
                f = new FileStream(fileLoc, FileMode.Create);
                writer = new StreamWriter(f, Encoding.UTF8);
                writer.Write(index + "," + newFolderName + "\r\n" + allTxt);
            }
            writer.Flush(); writer.Close(); writer.Dispose();
            reader.Close(); reader.Dispose();
            file.Close(); file.Dispose();
            allTxt = null; folders = null;

            fileLoc = fileLoc.Substring(0, fileLoc.Length - 8) + newFolderName;

            Directory.CreateDirectory(fileLoc);//创建子文件夹
            fileLoc += "/root.txt";
            f = new FileStream(fileLoc, FileMode.Create);
            writer = new StreamWriter(f, Encoding.UTF8);
            writer.Write("文件夹：");
            writer.Flush();
            writer.Close();

            TreeViewItem t = new TreeViewItem();
            t.Header = newFolderName;
            ((TreeViewItem)folder.SelectedItem).Items.Add(t);
            newFolderName = null;
            ((TreeViewItem)folder.SelectedItem).IsExpanded = true;
            t.IsSelected = true;
        }
        private void deleteFolder_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem t = (TreeViewItem)folder.SelectedItem;
            if (t == root) return;
            string name = (string)t.Header;
            string path = file.Text;
            //删除TreeViewItem
            ((TreeViewItem)t.Parent).IsSelected = true;
            ((TreeViewItem)t.Parent).Items.Remove(t);
            t = null;
            //删除数据库目录
            path = path.Replace("ROOT://", "dataBase/").Replace(">", "/");
            string[] split = path.Split('/'); path = "";
            for (int i = 0; i < split.Length - 1; i++) path += "/" + split[i];
            path = path.Substring(1, path.Length - 1);
            Directory.Delete(path + "/" + split[split.Length - 1], true);
            //重写索引文件
            path = "";
            for (int i = 0; i < split.Length - 2; i++) path += "/" + split[i];
            path = path.Substring(1, path.Length - 1) + "/root.txt";
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            string index = reader.ReadLine();
            index = index.Replace(name, "").Replace(",,", ",").Replace(",\n", "\n").Replace("：,", "：");
            index += reader.ReadToEnd();
            reader = null;
            stream.Close();
            stream = new FileStream(path, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(index);
            writer.Flush();
            writer.Close();
            stream.Close();
            stream.Dispose();
            GC.Collect();
        }

        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            if (selectAll.IsChecked == true) {
                for (int i = 0; i < dg.Items.Count; i++)
                {
                    var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dg.Columns[4].GetCellContent(ObjROw);
                        if (objElement != null)
                        {
                            CheckBox objChk = (CheckBox)objElement;
                            if (objChk.IsChecked == false) objChk.IsChecked = true;
                        }
                    }
                }
            }
            else if (selectAll.IsChecked == false)
            {
                for (int i = 0; i < dg.Items.Count; i++)
                {
                    var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dg.Columns[4].GetCellContent(ObjROw);
                        if (objElement != null)
                        {
                            CheckBox objChk = (CheckBox)objElement;
                            if (objChk.IsChecked == true) objChk.IsChecked = false;
                        }
                    }
                }
            }
        }
        private void selectChange_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dg.Items.Count; i++)
            {
                var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dg.Columns[4].GetCellContent(ObjROw);
                    if (objElement != null)
                    {
                        CheckBox objChk = (CheckBox)objElement;
                        if (objChk.IsChecked == false) objChk.IsChecked = true;
                        else objChk.IsChecked = false;
                    }
                }
            }
            bool che = true;
            for (int i = 0; i < dg.Items.Count; i++)
            {
                var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dg.Columns[4].GetCellContent(ObjROw);
                    if (objElement != null)
                    {
                        CheckBox objChk = (CheckBox)objElement;
                        if (objChk.IsChecked == false) che = false;
                    }
                }
            }
            selectAll.IsChecked = che;
        }
        private void deleteData_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedItem is DATA) Data.Remove((DATA)dg.SelectedItem);

            for (int i = 0; i < dg.Items.Count - 1; i++)
                if (dg.Items[i] is DATA)
                {
                    var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                    DataGridRow ObjROw = (DataGridRow)cntr;
                    if (ObjROw != null)
                    {
                        FrameworkElement objElement = dg.Columns[4].GetCellContent(ObjROw);
                        if (objElement != null)
                        {
                            CheckBox Chk = (CheckBox)objElement;
                            if (Chk.IsChecked == true) { Data.RemoveAt(i); i--; }
                            }
                    }
                }
            updateIndex();
        }
        private void moveFolder_Click(object sender, RoutedEventArgs e)
        {
            string folderLoc = getFileLoc(file.Text);
            folderLoc = folderLoc.Substring(0, folderLoc.Length - 9);
            string newPath = getFileLoc(targetLoc.Text);
            newPath = newPath.Substring(0, newPath.Length - 9);
            CopyDirectory(folderLoc, newPath);
            refreshTreeView();
        }
        void CopyDirectory(string fromDir, string toDir)
        {
            if (!Directory.Exists(fromDir)) return;

            string[] name = fromDir.Split('/');
            
            if (Directory.Exists(toDir + "/" + name[name.LongLength - 1]))
            {
                FileStream stream = new FileStream(fromDir + "/root.txt", FileMode.Open);
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                reader.ReadLine();
                string AData = reader.ReadToEnd();
                stream.Close();
                toDir += "/" + name[name.LongLength - 1];
                stream = new FileStream(toDir + "/root.txt", FileMode.Open);
                reader = new StreamReader(stream, Encoding.UTF8);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                AData = reader.ReadToEnd() + "\n" + AData;
                stream.Close();

                stream = new FileStream(toDir + "/root.txt", FileMode.Create);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.Write(AData);
                writer.Flush();
                stream.Close();
                
                int length = name[name.LongLength - 1].Length;
                string[] Directories = Directory.GetDirectories(fromDir);
                for (int i = 0; i < Directories.Length; i++) CopyDirectory(Directories[i].Replace('\\', '/'), toDir);
            } else
            {
                FileStream stream = new FileStream(toDir + "/root.txt", FileMode.Open);
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                string index = reader.ReadLine();
                index = index.Substring(4, index.Length - 4);
                string[] spl = index.Split(',');
                if (spl.LongLength == 1 && spl[0] == "") index = "文件夹：" + index + name[name.LongLength - 1];
                else index = "文件夹：" + index + "," + name[name.LongLength - 1];
                string toEnd = reader.ReadToEnd();
                if (toEnd != "") index += "\n" + toEnd;

                stream.Close();
                stream = new FileStream(toDir + "/root.txt", FileMode.Create);
                StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                writer.Write(index);
                writer.Flush();
                stream.Close();

                directCopy(fromDir, toDir + "/" + name[name.LongLength - 1]);
            }
            
        }
        void directCopy(string fromDir,string toDir)
        {
            if (!Directory.Exists(fromDir)) return;
            
            if (!Directory.Exists(toDir))
            {
                Directory.CreateDirectory(toDir);
            }
            
            string[] files = Directory.GetFiles(fromDir);
            foreach (string formFileName in files)
            {
                string fileName = Path.GetFileName(formFileName);
                string toFileName = Path.Combine(toDir, fileName);
                File.Copy(formFileName, toFileName);
            }
            
            string[] fromDirs = Directory.GetDirectories(fromDir);
            foreach (string fromDirName in fromDirs)
            {
                string dirName = Path.GetFileName(fromDirName);
                string toDirName = Path.Combine(toDir, dirName);
                directCopy(fromDirName, toDirName);
            }
        }
        private void getLocation_Click(object sender, RoutedEventArgs e)
        {
            targetLoc.Text = file.Text;
        }
        
        void updateIndex()
        {
            for (int i = 0; i < dg.Items.Count - 1; i++)
            {
                var cntr = dg.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = dg.Columns[0].GetCellContent(ObjROw);
                    if (objElement != null)
                    {
                        TextBlock objChk = (TextBlock)objElement;
                        objChk.Text = i + 1 + "";
                    }
                }
            }
        }

        private void MetroWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && e.Key == Key.S)
            {
                e.Handled = true;
                updateDataBase();
            }
        }
        private void dg_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            progressSave.Value=0;
        }
        private void dg_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            dg_SelectionChanged(null, null);
        }
        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dg.SelectedIndex == dg.Items.Count - 1) { CmdType.SelectedIndex = 0; description.Text = ""; return; }
            if (!(dg.SelectedItem is DATA)) return;
            DATA d = (DATA)dg.SelectedItem;
            switch (d.type)
            {
                case "原版命令":CmdType.SelectedIndex = 0; txtb.Text = d.data;
                    break;
                case "绝对坐标":CmdType.SelectedIndex = 3; ((LocationEditor)dataEditor.Children[0]).importData(d.data);
                    break;
                case "相对坐标":CmdType.SelectedIndex = 4; ((TLocationEditor)dataEditor.Children[0]).importData(d.data);
                    break;

                case "玩家选择器":CmdType.SelectedIndex = 1;
                    break;
                case "实体选择器":CmdType.SelectedIndex = 2;
                    break;
                case "物品_Json书":CmdType.SelectedIndex = 5;
                    break;
                case "物品_告示牌":CmdType.SelectedIndex = 6;
                    break;
                case "方块_告示牌":CmdType.SelectedIndex = 7;
                    break;
                case "物品NBT":CmdType.SelectedIndex = 8;
                    break;
                case "实体NBT":CmdType.SelectedIndex = 9;
                    break;
            }
            UIElement u = dataEditor.Children[0];
            if(u is Template) ((Template)u).importData(d.data);
            description.Text = d.des;
        }

        private void CmdType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataEditor == null) return;
            dataEditor.Children.Clear();
            switch (CmdType.SelectedIndex)
            {
                case 0:txtb = new TextBox(); dataEditor.Children.Add(txtb);
                    break;
                case 3:dataEditor.Children.Add(new LocationEditor());
                    break;
                case 4:dataEditor.Children.Add(new TLocationEditor());
                    break;
                default:dataEditor.Children.Add(new Template(CmdType.SelectedIndex));
                    break;
            }
        }

        private void insert_Click(object sender, RoutedEventArgs e)
        {
            if (dg.SelectedIndex == -1) return;
            int idx = int.Parse(index.Text) - 1;
            index.Text = idx + 1 + "";
            if (idx < 0) { idx = 0; index.Text = idx + 1 + ""; }
            if (idx > dg.Items.Count) { idx = dg.Items.Count - 2; index.Text = dg.Items.Count - 2 + ""; }
            DATA d = Data[dg.SelectedIndex];
            Data.RemoveAt(dg.SelectedIndex);
            Data.Insert(idx, d);
            updateIndex();
            dg.SelectedIndex = idx;
        }
        string getEditingData()
        {
            string d = "";
            switch (CmdType.SelectedIndex)
            {
                default: d = ((Template)dataEditor.Children[0]).getData(); break;
                case 0: d = txtb.Text; break;
                case 3: d = ((LocationEditor)dataEditor.Children[0]).getData(); break;
                case 4: d = ((TLocationEditor)dataEditor.Children[0]).getData(); break;
            }
            return d;
        }
        private void addData_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            if (dg.SelectedIndex != -1) index = dg.SelectedIndex;
            Data.Insert(index, new DATA() { type = (string)((ComboBoxItem)CmdType.SelectedItem).Content, data = getEditingData(), des = description.Text, idx = index+1 + "" });
            updateIndex();
        }
        private void editData_Click(object sender, RoutedEventArgs e)
        {
            int i = dg.SelectedIndex;
            if (i < Data.Count) Data.RemoveAt(i);
            Data.Insert(i, new DATA { type = (string)((ComboBoxItem)CmdType.SelectedItem).Content, data = getEditingData(), des = description.Text, idx = i + 1 + "" });
        }
    }

    class Folder
    {
        public Folder(string name, TreeViewItem t)
        {
            FileStream file = new FileStream("dataBase/" + name + "/root.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file, Encoding.UTF8);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            string folders = reader.ReadLine();
            folders = folders.Substring(4, folders.Length - 4);

            string[] children = folders.Split(',');
            if (children[0] != "")
                for (int i = 0; i < children.Length; i++)
                {
                    TreeViewItem ti = new TreeViewItem();
                    ti.Header = children[i];
                    t.Items.Add(ti);
                    new Folder(name + "/" + children[i], ti);
                }
            file.Close();
        }
    }
    class DATA
    {
        public string idx { get; set; }
        public string data { get; set; }
        public string type { get; set; } = "原版命令";
        public string des { get; set; }

        public string getDataBaseText()
        {
            Console.WriteLine(des);
            return type + "\t" + data + "\t" + des;
        }
    }
}