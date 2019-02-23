using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MinecraftToolsBox.Tools;
using Substrate.Core;
using Substrate.Nbt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml;

namespace MTB.GameHandler
{
    /// <summary>
    /// ApplyToMap.xaml 的交互逻辑
    /// </summary>
    public partial class ApplyToMap : Grid
    {
        int tabSelectedIndex = 0;
        string CMDBLOCK = "minecraft:command_block", SIGN = "minecraft:sign";

        public ApplyToMap()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel) return;
            string dir = dialog.SelectedPath.Trim();
            path.Text = dir;
        }

        private void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedIndex == tabSelectedIndex) return;
            if (tab.SelectedIndex == 2)
            {
                if (Directory.Exists(path.Text + "\\.translation"))
                {
                    languageFiles.Items.Clear();
                    foreach(string file in Directory.GetFiles(path.Text + "\\.translation"))
                    {
                        if (file.Substring(file.Length - 5) == ".mtbl")
                        {
                            languageFiles.Items.Add(new CheckBox { Content = file.Split('\\').Last(), Foreground = Brushes.Black, ToolTip = file });
                        }
                    }
                }
            }
            else if (tab.SelectedIndex == 3)
            {
                string info = "即将修改存档 " + path.Text + " 中的";
                if (cb.IsChecked == true) info += "\n    -命令方块";
                if (sign.IsChecked == true) info += "\n    -告示牌";
                if (container.IsChecked == true) info += "\n    -容器";
                if (entity.IsChecked == true) info += "\n    -实体";
                if (spawner.IsChecked == true) info += "\n    -刷怪笼";
                if (saveinfo.IsChecked == true) info += "\n    -存档信息";
                ScanInfo.Content = info;
            }
            tabSelectedIndex = tab.SelectedIndex;
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            tab.SelectedIndex--;
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            tab.SelectedIndex++;
            if (tab.SelectedIndex == 3)
            {
                tab3.IsEnabled = false;
                tab4.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", path.Text + "\\.translation");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "MTB Language Files (*.mtbl)|*.mtbl",
                Multiselect = true
            };//system.win32
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string name in openFileDialog.FileNames)
                {
                    bool flag = false;
                    foreach (CheckBox cb in languageFiles.Items)
                    {
                        if (cb.ToolTip.ToString() == name)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag) continue;
                    languageFiles.Items.Add(new CheckBox { Content = name.Split('\\').Last(), Foreground = Brushes.Black, IsChecked = true, ToolTip = name });
                }
            }
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                using (Stream stream = new NBTFile(path.Text + "\\level.dat").GetDataInputStream())
                {
                    TagNodeCompound root = new NbtTree(stream).Root["Data"].ToTagCompound();
                    bool flag = false;
                    if (!root.ContainsKey("Version")) flag = true;
                    else
                    {
                        TagNodeCompound version = root["Version"].ToTagCompound();
                        string verName = version["Name"].ToTagString().Data;
                        if (verName.Contains("w"))
                        {
                            string[] ver = verName.Split('w');
                            if (int.Parse(ver[0]) < 16) flag = true;
                            else
                            {
                                if (int.Parse(ver[0]) == 16)
                                {
                                    if (int.Parse(ver[1].Substring(0, ver[1].Length - 1)) < 32) flag = true;
                                }
                            }
                        }
                        else if (!verName.Contains("Pre"))
                        {
                            string[] ver = verName.Split('.');
                            if (int.Parse(ver[0]) < 2)
                            {
                                if (int.Parse(ver[1]) < 11) flag = true;
                            }
                        }
                    }
                    if (flag)
                    {
                        CMDBLOCK = "Control";
                        SIGN = "Sign";
                    }
                }

                List<LanguageItem> ItemList = new List<LanguageItem>();
                List<LanguageItem> SignList = new List<LanguageItem>();
                List<LanguageItem> ContainerList = new List<LanguageItem>();
                List<LanguageItem> EntityList = new List<LanguageItem>();
                List<LanguageItem> MobSpawnerList = new List<LanguageItem>();
                foreach(CheckBox cb in languageFiles.Items)
                {
                    if (cb.IsChecked == false) continue;
                    XmlDocument doc = new XmlDocument();
                    doc.Load((languageFiles.Items[0] as CheckBox).ToolTip.ToString());
                    foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[1].ChildNodes)
                    {
                        ItemList.Add(new LanguageItem { Original = node.Attributes["Original"].Value, Translated = node.Attributes.Count == 2 ? node.Attributes[1].Value : "", Type = LangItemType.CommandBlock });
                    }
                    foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[2].ChildNodes)
                    {
                        Sign o = new Sign(), t = new Sign();
                        o.Line1 = node.ChildNodes[0].Attributes[0].Value;
                        o.Line2 = node.ChildNodes[1].Attributes[0].Value;
                        o.Line3 = node.ChildNodes[2].Attributes[0].Value;
                        o.Line4 = node.ChildNodes[3].Attributes[0].Value;
                        t.Line1 = node.ChildNodes[0].Attributes.Count == 2 ? node.ChildNodes[0].Attributes[1].Value : "";
                        t.Line2 = node.ChildNodes[1].Attributes.Count == 2 ? node.ChildNodes[1].Attributes[1].Value : "";
                        t.Line3 = node.ChildNodes[2].Attributes.Count == 2 ? node.ChildNodes[2].Attributes[1].Value : "";
                        t.Line4 = node.ChildNodes[3].Attributes.Count == 2 ? node.ChildNodes[3].Attributes[1].Value : "";
                        SignList.Add(new LanguageItem { Original = o, Translated = t, Type = LangItemType.Sign });
                    }
                    foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[3].ChildNodes)
                    {
                        Container o = new Container(), t = new Container();
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            if (item.Name == "CustomName")
                            {
                                o.CustomName = item.Attributes[0].Value;
                                t.CustomName = item.Attributes.Count == 2 ? item.Attributes[1].Value : "";
                            }
                            else if (item.Name == "Lock")
                            {
                                o.Lock = item.Attributes[0].Value;
                                t.Lock = item.Attributes.Count == 2 ? item.Attributes[1].Value : "";
                            }
                            else if (item.Name == "Book")
                            {
                                Book io = new Book(), it = new Book();
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Title") { io.Title = data.Attributes[0].Value; it.Title = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Pages = data.Attributes[0].Value; it.Pages = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Items.Add(io);
                                t.Items.Add(it);
                            }
                            else
                            {
                                Item io = new Item(), it = new Item();
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Name") { io.Name = data.Attributes[0].Value; it.Name = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Lore = data.Attributes[0].Value; it.Lore = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Items.Add(io);
                                t.Items.Add(it);
                            }
                        }
                        ContainerList.Add(new LanguageItem { Original = o, Translated = t, Type = LangItemType.Container, Name = node.Attributes["Id"].Value, Info = node.Attributes["Pos"].Value });
                    }
                    foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[4].ChildNodes)
                    {
                        Entity o = new Entity(), t = new Entity();
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            if (item.Name == "CustomName") { o.CustomName = item.Attributes[0].Value; t.CustomName = item.Attributes.Count == 2 ? item.Attributes[1].Value : ""; }
                            else if (item.Name == "Book")
                            {
                                Book io = new Book(), it = new Book();
                                int slot = -1;
                                switch (item.Attributes["Slot"].Value)
                                {
                                    case "主手": slot = 0; break;
                                    case "副手": slot = 1; break;
                                    case "头盔": slot = 2; break;
                                    case "胸甲": slot = 3; break;
                                    case "护腿": slot = 4; break;
                                    case "靴子": slot = 5; break;
                                }
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Title") { io.Title = data.Attributes[0].Value; it.Title = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Pages = data.Attributes[0].Value; it.Pages = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Equipments[slot] = io;
                                t.Equipments[slot] = it;
                            }
                            else
                            {
                                Item io = new Item(), it = new Item();
                                int slot = -1;
                                switch (item.Attributes["Slot"].Value)
                                {
                                    case "主手": slot = 0; break;
                                    case "副手": slot = 1; break;
                                    case "头盔": slot = 2; break;
                                    case "胸甲": slot = 3; break;
                                    case "护腿": slot = 4; break;
                                    case "靴子": slot = 5; break;
                                }
                                if (slot == -1) continue;
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Name") { io.Name = data.Attributes[0].Value; it.Name = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Lore = data.Attributes[0].Value; it.Lore = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Equipments[slot] = io;
                                t.Equipments[slot] = it;
                            }
                        }
                        EntityList.Add(new LanguageItem { Original = o, Translated = t, Type = LangItemType.Entity, Name = node.Attributes["Id"].Value });
                    }
                    foreach (XmlNode node in doc.ChildNodes[1].ChildNodes[5].ChildNodes)
                    {
                        Entity o = new Entity(), t = new Entity();
                        foreach (XmlNode item in node.ChildNodes)
                        {
                            if (item.Name == "CustomName") { o.CustomName = item.Attributes[0].Value; t.CustomName = item.Attributes.Count == 2 ? item.Attributes[1].Value : ""; }
                            else if (item.Name == "Book")
                            {
                                Book io = new Book(), it = new Book();
                                int slot = -1;
                                switch (item.Attributes["Slot"].Value)
                                {
                                    case "主手": slot = 0; break;
                                    case "副手": slot = 1; break;
                                    case "头盔": slot = 2; break;
                                    case "胸甲": slot = 3; break;
                                    case "护腿": slot = 4; break;
                                    case "靴子": slot = 5; break;
                                }
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Title") { io.Title = data.Attributes[0].Value; it.Title = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Pages = data.Attributes[0].Value; it.Pages = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Equipments[slot] = io;
                                t.Equipments[slot] = it;
                            }
                            else
                            {
                                Item io = new Item(), it = new Item();
                                int slot = -1;
                                switch (item.Attributes["Slot"].Value)
                                {
                                    case "主手": slot = 0; break;
                                    case "副手": slot = 1; break;
                                    case "头盔": slot = 2; break;
                                    case "胸甲": slot = 3; break;
                                    case "护腿": slot = 4; break;
                                    case "靴子": slot = 5; break;
                                }
                                if (slot == -1) continue;
                                foreach (XmlNode data in item.ChildNodes)
                                {
                                    if (data.Name == "Name") { io.Name = data.Attributes[0].Value; it.Name = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                    else { io.Lore = data.Attributes[0].Value; it.Lore = data.Attributes.Count == 2 ? data.Attributes[1].Value : ""; }
                                }
                                o.Equipments[slot] = io;
                                t.Equipments[slot] = it;
                            }
                        }
                        MobSpawnerList.Add(new LanguageItem { Original = o, Translated = t, Type = LangItemType.MobSpawner, Name = node.Attributes["Id"].Value, Info = node.Attributes["Pos"].Value });
                    }
                }

                totalProgress.Maximum = Directory.GetFiles(path.Text + "\\region").Length;
                totalProgress.IsIndeterminate = false;
                progress.IsIndeterminate = false;
                string p = path.Text;
                Task.Run(new Action(delegate { Apply(p, ItemList, SignList, ContainerList, EntityList, MobSpawnerList); }));
            //}
            //catch
            //{
            //    (System.Windows.Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法打开文件", "文件不存在或已经损坏", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            //}
        }
        private void Apply(string path, List<LanguageItem> items, List<LanguageItem> signs, List<LanguageItem> containers, List<LanguageItem> Entities, List<LanguageItem> MobSpawners)
        {
            //try
            //{
            string[] files = Directory.GetFiles(path + "\\region");
            for (int i = 0; i < files.Length; i++)
            {
                Dispatcher.BeginInvoke(new Action(delegate { totalProgress.Value += 1; progress.Value = 0; }));
                RegionFile region = new RegionFile(files[i]);
                for (int I = 0; I < 32; I++)
                    for (int J = 0; J < 32; J++)
                    {
                        Dispatcher.BeginInvoke(new Action(delegate { progress.Value += 1; }));
                        if (region.HasChunk(I, J))
                        {
                            try
                            {
                                NbtTree nbtTree = new NbtTree();
                                nbtTree.ReadFrom(region.GetChunkDataInputStream(I, J));
                                TagNodeList tagNodeList = nbtTree.Root["Level"].ToTagCompound()["TileEntities"].ToTagList();
                                TagNodeList entityList = nbtTree.Root["Level"].ToTagCompound()["Entities"].ToTagList();
                                bool flag = false;
                                if (entityList != null)
                                {
                                    foreach (TagNodeCompound tagNodeCompound in entityList)
                                    {
                                        if (LangTranslator.EntityTranslator(tagNodeCompound, Entities)) flag = true;
                                    }
                                }
                                if (tagNodeList != null)
                                    foreach (TagNodeCompound tagNodeCompound in tagNodeList)
                                    {
                                        string data = tagNodeCompound["id"].ToTagString().Data;
                                        if (data == CMDBLOCK) { if (LangTranslator.CommandBlockTranslator(tagNodeCompound, items)) flag = true; }
                                        else if (data == SIGN) { if (LangTranslator.SignTranslator(tagNodeCompound, signs)) flag = true; }
                                        else if (tagNodeCompound.ContainsKey("Items") || tagNodeCompound.ContainsKey("RecordItem")) { if (LangTranslator.ContainerTranslator(tagNodeCompound, containers)) flag = true; }
                                        else if (tagNodeCompound.ContainsKey("SpawnData")) { if (LangTranslator.EntityTranslator(tagNodeCompound["SpawnData"].ToTagCompound(), MobSpawners)) flag = true; }
                                    }
                                if (flag)
                                {
                                    using (Stream chunkDataOutputStream = region.GetChunkDataOutputStream(I, J))
                                    {
                                        nbtTree.WriteTo(chunkDataOutputStream);
                                    }
                                }
                            }
                            catch { }
                        }
                    }
            }
            Dispatcher.BeginInvoke(new Action(delegate { progress.Value = 0; totalProgress.Value = 0; progress.IsIndeterminate = true; totalProgress.IsIndeterminate = true; }));
            //}
            //catch
            //{
            //    Dispatcher.BeginInvoke(new Action(delegate { (System.Windows.Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法完成操作", "存档可能已经损坏", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" }); }));
            //}
        }

    }
}
