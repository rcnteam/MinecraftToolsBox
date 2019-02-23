using MahApps.Metro.Controls;
using MinecraftToolsBox.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace MinecraftToolsBox.Database
{
    /// <summary>
    /// SaveCommand.xaml 的交互逻辑
    /// </summary>
    public partial class SaveCommand : MetroWindow
    {
        CommandsGeneratorTemplate CmdGenerator;
        public SaveCommand(string cmd, CommandsGeneratorTemplate cmdGenerator)
        {
            InitializeComponent();
            refreshTreeView();
            root.IsExpanded = true;
            Data.Text = cmd;
            CmdGenerator = cmdGenerator;

            switch (CmdGenerator.nowCmd)
            {
                case "command":break;
                case "Pselector": CmdType.SelectedIndex = 1; break;
                case "Eselector": CmdType.SelectedIndex = 2; break;
                case "book":CmdType.SelectedIndex = 5;break;
                case "sign_item": CmdType.SelectedIndex = 6; break;
                case "sign_block": CmdType.SelectedIndex = 7; break;
            }
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
        string getFileLoc(TreeViewItem parent)
        {
            if (parent == root) return "database/root.txt";

            ObservableCollection<string> loc = new ObservableCollection<string>();
            loc.Add((string)parent.Header);

            while (parent != root)
            {
                parent = (TreeViewItem)parent.Parent;
                loc.Insert(0, (string)parent.Header);
            }

            string location = "ROOT://";
            for (int i = 1; i < loc.Count; i++) location += loc[i] + ">";
            return getFileLoc(location);
        }
        string getFileLoc(string path)
        {
            path = path.Replace("ROOT://", "dataBase/").Replace(">", "/");
            string[] split = path.Split('/'); path = "";
            for (int i = 0; i < split.Length - 1; i++) path += "/" + split[i];
            path = path.Substring(1, path.Length - 1) + "/root.txt";
            return path;
        }

        private void save_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (MainWindow.db != null) MainWindow.db.Close();
            string path = getFileLoc((TreeViewItem)folder.SelectedItem);
            Console.WriteLine(path);
            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            stream.Seek(0, SeekOrigin.Begin);
            string data = reader.ReadToEnd();
            data += "\r\n" + ((ComboBoxItem)CmdType.SelectedItem).Content + "\t" + Data.Text + "\t" + Des.Text;
            stream.Close();
            stream = new FileStream(path, FileMode.Create);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);            
            writer.Write(data);
            writer.Flush();
            writer.Close();
            Close();
        }
    }
}