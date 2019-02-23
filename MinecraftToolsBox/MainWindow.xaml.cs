using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MinecraftToolsBoxSDK;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace MinecraftToolsBox
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow, IMainWindowCommands
    {
        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr proc, int min, int max);
        SolidColorBrush tileBrush = new SolidColorBrush(Color.FromArgb(229, 17, 150, 240));
        SolidColorBrush groupBrush = new SolidColorBrush(Color.FromArgb(190, 255, 255, 255));

        public MainWindow()
        {
            SplashScreen splash = new SplashScreen("images/SplashScreen.png");
            splash.Show(true, true);
            //load configuration
            NameValueCollection cfg = ConfigurationManager.AppSettings;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            KeyValueConfigurationCollection c = configuration.AppSettings.Settings;
            List<string> list = new List<string>();list.AddRange(c.AllKeys);
            if (!list.Contains("Height")) c.Add(new KeyValueConfigurationElement("Height", "720"));
            if (!list.Contains("Width")) c.Add(new KeyValueConfigurationElement("Width", "1280"));
            if (!list.Contains("Account")) c.Add(new KeyValueConfigurationElement("Account", "Steve"));
            if (!list.Contains("Memory")) c.Add(new KeyValueConfigurationElement("Memory", "1024"));
            if (!list.Contains("JavaPath")) c.Add(new KeyValueConfigurationElement("JavaPath", ""));
            if (!list.Contains("MCPath")) c.Add(new KeyValueConfigurationElement("MCPath", ""));
            if (!list.Contains("DataShare")) c.Add(new KeyValueConfigurationElement("DataShare", "true"));
            if (!list.Contains("Online")) c.Add(new KeyValueConfigurationElement("Online", "true"));
            if (!list.Contains("FirstRun")) c.Add(new KeyValueConfigurationElement("FirstRun", "true"));
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
            Config.UpdateSettings();
            //initialize UI
            InitializeComponent();
            DataContext = this;
            (About.Content as About).ver.Content = "Version: 3.0";
            //load plugins
            if (!Directory.Exists(Environment.CurrentDirectory + "\\plugins")) Directory.CreateDirectory(Environment.CurrentDirectory + "\\plugins");
            foreach(string file in Directory.GetFiles(Environment.CurrentDirectory + "\\plugins"))
            {
                if (file.Substring(file.Length - 3) != "dll") continue;
                Assembly asm = Assembly.LoadFile(file);
                Type[] t = asm.GetExportedTypes();
                foreach (Type type in t)
                {
                    if (type.BaseType == typeof(MtbPlugin))
                    {
                        MtbPlugin plugin = (MtbPlugin)Activator.CreateInstance(type);
                        plugin.LoadPlugin();
                        if (plugin.CheckUpdate()) Console.WriteLine(plugin.Name + ":" + plugin.UpdateMessage);

                        foreach(KeyValuePair<string, Tile[]> item in plugin.GetPages())
                        {
                            WrapPanel wp = new WrapPanel { Orientation = Orientation.Vertical, Height=252 };
                            foreach(Tile page in item.Value)
                            {
                                page.Height = page.Width = 120;
                                page.Background = tileBrush;
                                wp.Children.Add(page);
                            }
                            GroupBox grop = new GroupBox
                            {
                                Header = item.Key,
                                Content = wp,
                                Margin = new Thickness(5, 5, 10, 10),
                                HorizontalAlignment = HorizontalAlignment.Left,
                                Background = Brushes.Transparent,
                                BorderBrush = groupBrush
                            };
                            ControlsHelper.SetHeaderFontSize(grop, 14);
                            GroupBoxHelper.SetHeaderForeground(grop, Brushes.White);
                            panel.Children.Add(grop);
                        }
                        tasks.Children.Add(new TaskBarButton { Tip = plugin.Name, Source = plugin.Icon, Plugin = plugin });
                    }
                }

            }
            Task.Run(new Action(CheckUpdate));
        }

        public static void CheckUpdate()
        {
            WebClient wc = new WebClient();
            try
            {
                //download version info
            }
            catch
            {
                (Application.Current.MainWindow as MetroWindow).Dispatcher.Invoke(delegate 
                {
                    (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法检查更新", "无法获取更新信息，请检查网络连接", MessageDialogStyle.Affirmative, new MetroDialogSettings { AffirmativeButtonText = "确定" });
                });
                return;
            }
            try
            {
                //check update
                if (false)
                {
                    Application.Current.MainWindow.Dispatcher.Invoke(delegate
                    {
                        (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("检测到新版本", "点击立即更新将关闭主程序，请保存您的工作进度", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings { AffirmativeButtonText = "立即更新", NegativeButtonText = "暂不更新" });
                    });
                }
            }
            catch(Exception e)
            {
                (Application.Current.MainWindow as MetroWindow).ShowMessageAsync("无法完成更新", "文件错误" + e.HelpLink, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
        }

        #region 窗口控制
        public MtbWindow AddWindow(ImageSource Icon, object Content, string Title, MtbPlugin Plugin)
        {
            foreach (TaskBarButton task in tasks.Children)
            {
                if (task.Plugin == Plugin)
                {
                    MtbWindow window = new MtbWindow(Icon, Content, Title, task);
                    ShowWindow(window);
                    windows.Children.Add(window);
                    window.Height = windows.ActualHeight;
                    window.Width = windows.ActualWidth;
                    return window;
                }
            }
            return null;
        }
        public void ShowWindow(MtbWindow Window)
        {
            Window.Visibility = Visibility.Visible;
        }
        public void CloseWindow(MtbWindow window)
        { 
            window.Task.WindowItems.Children.Remove(window.View);
            int count = window.Task.WindowItems.Children.Count;
            window.Task.Badge.Badge = count != 0 ? count + "" : null;
            windows.Children.Remove(window);

            SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            GC.Collect();
        }
        #endregion

        #region 打开页面
        private void OpenPerformanceWindow(object sender, RoutedEventArgs e)
        {
            config.IsOpen = false;
            About.IsOpen = false;
            performance.IsOpen = !performance.IsOpen;
        }
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            About.IsOpen = false;
            performance.IsOpen = false;
            config.IsOpen = !config.IsOpen;
        }
        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            performance.IsOpen = false;
            config.IsOpen = false;
            About.IsOpen = !About.IsOpen;
        }
        private void OpenNoticeCenter(object sender, MouseButtonEventArgs e) => Notice.IsOpen = !Notice.IsOpen;
        private void ShowDesktop(object sender, MouseButtonEventArgs e)
        {
            foreach(UIElement item in windows.Children)
            {
                item.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region API
        public void ShowWindowView(StackPanel Panel, int Offset)
        {
            view.Visibility = Visibility.Visible;
            view.Content = Panel;
            Panel.Margin = new Thickness(Offset, 0, 0, 0);
        }
        public void CloseWindowView() => view.Visibility = Visibility.Collapsed;
        public DragControlHelper GetHelper() => h;
        #endregion
    }
}
/* BookPage剪贴板错误
 * /enchant 等级限制
 * e.Source优化
 * NBT强制匹配
 * EntityCommands NumericalUpDown数值转换
 * 优化：药水效果界面采用DataGrid
 * 所有页面实现接口
 * 实体NBT：rotation旋转标签,可养殖动物NBT
 * 颜色设置：透明色
 * gamerule maxentitycramming
 * //entity nbt:rotation,pos
 * json编辑器：添加事件：背景
 */