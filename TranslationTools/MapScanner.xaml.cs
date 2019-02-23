using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MinecraftToolsBox.Tools;
using Substrate.Core;
using Substrate.Nbt;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml;

namespace MTB.GameHandler
{
    /// <summary>
    /// UserControl1.xaml 的交互逻辑
    /// </summary>
    public partial class MapScanner : Grid
    {
        string CMDBLOCK = "minecraft:command_block", SIGN = "minecraft:sign";
        public MapScanner()
        {
            InitializeComponent();
        }

        private void Scan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                label.Content = "开始扫描";
                Previous1.IsEnabled = false;
                scan.IsEnabled = false;
                tab1.IsEnabled = false;
                tab2.IsEnabled = false;
                string FilePath = path.Text, OutputPath = path.Text + "\\.translation", Version = "Before 1.11";
                if (!Directory.Exists(OutputPath)) Directory.CreateDirectory(OutputPath);

                progress.IsIndeterminate = false;
                totalProgress.IsIndeterminate = false;
                int len = Directory.GetFiles(FilePath + "\\region").Length;
                totalProgress.Maximum = len;

                using (Stream stream = new NBTFile(FilePath + "\\level.dat").GetDataInputStream())
                {
                    TagNodeCompound root = new NbtTree(stream).Root["Data"].ToTagCompound();
                    bool flag = false;
                    if (!root.ContainsKey("Version")) flag = true;
                    else
                    {
                        TagNodeCompound version = root["Version"].ToTagCompound();
                        Version = version["Name"].ToTagString().Data;
                        if (Version.Contains("w"))
                        {
                            string[] ver = Version.Split('w');
                            if (int.Parse(ver[0]) < 16) flag = true;
                            else
                            {
                                if (int.Parse(ver[0]) == 16)
                                {
                                    if (int.Parse(ver[1].Substring(0, ver[1].Length - 1)) < 32) flag = true;
                                }
                            }
                        }
                        else if (!Version.Contains("Pre"))
                        {
                            string[] ver = Version.Split('.');
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
                    string[] split = OutputPath.Split('\\');
                    string fileName = split[split.Length - 2];
                    while (File.Exists(OutputPath + "\\" + fileName + ".mtbl")) fileName += "-";
                    bool[] settings = new bool[] { (bool)cb.IsChecked, (bool)sign.IsChecked, (bool)container.IsChecked, (bool)entity.IsChecked, (bool)spawner.IsChecked, (bool)saveinfo.IsChecked };
                    Task.Run(new Action(delegate { Start(Directory.GetFiles(FilePath + "\\region"), OutputPath + "\\" + fileName + ".mtbl", settings, Version, root); }));
                }
            }
            catch
            {
                (System.Windows.Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法打开存档", "存档不存在或已经损坏", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }
        private void Start(string[] files, string Fpath, bool[] settings, string version, TagNodeCompound level)
        {
            //try
            //{
                XmlDocument doc = new XmlDocument();
                doc.AppendChild(doc.CreateXmlDeclaration("1.0", "UTF-8", null));
                XmlElement root = doc.CreateElement("LanguageItems");
                root.SetAttribute("SaveVersion", version);
                root.SetAttribute("CommandBlocks", settings[0].ToString());
                root.SetAttribute("Signs", settings[1].ToString());
                root.SetAttribute("Containers", settings[2].ToString());
                root.SetAttribute("Entities", settings[3].ToString());
                root.SetAttribute("Spawners", settings[4].ToString());
                root.SetAttribute("SaveInfo", settings[5].ToString());
                doc.AppendChild(root);

                if (settings[5])
                {
                    XmlElement SaveInfo = doc.CreateElement("SaveInfo");
                    SaveInfo.SetAttribute("Original", level["LevelName"].ToTagString().Data);
                    root.AppendChild(SaveInfo);
                }
                XmlElement CommandBlocks = doc.CreateElement("CommandBlocks");
                XmlElement Signs = doc.CreateElement("Signs");
                XmlElement Containers = doc.CreateElement("Containers");
                XmlElement Entities = doc.CreateElement("Entities");
                XmlElement Spawners = doc.CreateElement("Spwaners");
                if (settings[0]) root.AppendChild(CommandBlocks);
                if (settings[1]) root.AppendChild(Signs);
                if (settings[2]) root.AppendChild(Containers);
                if (settings[3]) root.AppendChild(Entities);
                if (settings[4]) root.AppendChild(Spawners);
                CommandBlocks.SetAttribute("StartIndex", "1");
                Signs.SetAttribute("StartIndex", "1");
                Containers.SetAttribute("StartIndex", "1");
                Entities.SetAttribute("StartIndex", "1");
                Spawners.SetAttribute("StartIndex", "1");

                List<LanguageItem> list = new List<LanguageItem>();
                foreach (string text in files)
                {
                    RegionFile regionFile = new RegionFile(text);
                    for (int i = 0; i < 32; i++)
                        for (int j = 0; j < 32; j++)
                        {
                            Dispatcher.BeginInvoke(new Action(delegate { progress.Value += 1; }));
                            if (regionFile.HasChunk(i, j))
                            {
                            try
                                {
                                NbtTree tree = new NbtTree();
                                tree.ReadFrom(regionFile.GetChunkDataInputStream(i, j));
                                TagNodeCompound Root = tree.Root["Level"].ToTagCompound();
                                TagNodeList TileEntityList = Root["TileEntities"].ToTagList();
                                TagNodeList EntityList = Root["Entities"].ToTagList();

                                if (EntityList != null && settings[3])
                                        foreach (TagNodeCompound tagNodeCompound in EntityList)
                                        {
                                            LanguageItem item = new LanguageItem(tagNodeCompound, LangItemType.Entity);
                                            if (item.Check(list)) Entities.AppendChild(item.GetXmlElement(doc));
                                        }
                                    if (TileEntityList != null)
                                        foreach (TagNodeCompound tagNodeCompound in TileEntityList)
                                        {
                                            string data = tagNodeCompound["id"].ToTagString().Data;
                                            if (data == CMDBLOCK)
                                            {
                                                if (settings[0])
                                                {
                                                    LanguageItem item = new LanguageItem(tagNodeCompound, LangItemType.CommandBlock);
                                                    if (item.Check(list)) CommandBlocks.AppendChild(item.GetXmlElement(doc));
                                                }
                                            }
                                            else if (data == SIGN)
                                            {
                                                if (settings[1])
                                                {
                                                    LanguageItem item = new LanguageItem(tagNodeCompound, LangItemType.Sign);
                                                    if (item.Check(list)) Signs.AppendChild(item.GetXmlElement(doc));
                                                }
                                            }
                                            else if (tagNodeCompound.Keys.Contains("Items") || tagNodeCompound.Keys.Contains("RecordItem") || tagNodeCompound.ContainsKey("CustomName") || tagNodeCompound.ContainsKey("Lock"))
                                            {
                                                if (settings[2])
                                                {
                                                    LanguageItem item = new LanguageItem(tagNodeCompound, LangItemType.Container);
                                                    if (item.Check(list)) Containers.AppendChild(item.GetXmlElement(doc));
                                                }
                                            }
                                            else if (data.Contains("mob_spawner"))
                                            {
                                                if (settings[4])
                                                {
                                                    LanguageItem item = new LanguageItem(tagNodeCompound, LangItemType.MobSpawner);
                                                    if (item.Check(list)) Spawners.AppendChild(item.GetXmlElement(doc));
                                                }
                                            }
                                        }
                                }
                                catch
                                {
                                continue;
                                }

                            }
                        }
                    Dispatcher.BeginInvoke(new Action(delegate 
                    {
                        progress.Value = 0;
                        totalProgress.Value += 1;
                        TotalProgressInfo.Content = "正在扫描第" + totalProgress.Value + "个文件，共" + totalProgress.Maximum + "个";
                        string[] split = text.Split('\\');
                        ProgressInfo.Content = "正在扫描 " + split[split.Length - 1];
                    }));
                }
                doc.Save(Fpath);
                Dispatcher.BeginInvoke(new Action(delegate 
                {
                    progress.Value = 0;
                    totalProgress.Value = 0;
                    progress.IsIndeterminate = true;
                    totalProgress.IsIndeterminate = true;
                    label.Content = "已完成扫描";
                    TotalProgressInfo.Content = "已完成扫描";
                    ProgressInfo.Content = "";
                    Next1.IsEnabled = true;
                    file.Content = "主文件已保存到 " + Fpath;
                    file.ToolTip = Fpath;
                }));
                
            //}
            //catch (Exception e)
            //{
            //    Dispatcher.BeginInvoke(new Action(delegate { (System.Windows.Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法打开文件", "文件不存在或已经损坏", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" }); }));
            //}
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog m_Dialog = new FolderBrowserDialog();
            DialogResult result = m_Dialog.ShowDialog();
            if (result == DialogResult.Cancel) return;
            string m_Dir = m_Dialog.SelectedPath.Trim();
            path.Text = m_Dir;
        }

        private void Tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tab.SelectedIndex == 2)
            {
                string info = "即将扫描文件夹 " + path.Text+" 中的";
                if (cb.IsChecked == true) info += "\n    -命令方块";
                if (sign.IsChecked == true) info += "\n    -告示牌";
                if (container.IsChecked == true) info += "\n    -容器";
                if (entity.IsChecked == true) info += "\n    -实体";
                if (spawner.IsChecked == true) info += "\n    -刷怪笼";
                if (saveinfo.IsChecked == true) info += "\n    -存档信息";
                ScanInfo.Content = info;
            }
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
        
    }
}