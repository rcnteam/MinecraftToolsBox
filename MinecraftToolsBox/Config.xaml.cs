using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MinecraftToolsBox
{
    /// <summary>
    /// Config.xaml 的交互逻辑
    /// </summary>
    public partial class Config : Grid
    {
        public Config()
        {
            InitializeComponent();
            theme.SelectedIndex = GetIndex();
            background.SelectedColor = GetColor(ConfigurationManager.AppSettings["Back"]);
            foreground.SelectedColor = GetColor(ConfigurationManager.AppSettings["Front"]);
        }
        public static void UpdateSettings()
        {
            Accent Accent = ThemeManager.Accents.First(x => x.Name == ConfigurationManager.AppSettings["Theme"]);
            AppTheme Theme = ThemeManager.GetAppTheme(ConfigurationManager.AppSettings["Dark"] == "true" ? "BaseDark" : "BaseLight");
            ThemeManager.ChangeAppStyle(Application.Current, Accent, Theme);
            Application.Current.MainWindow.Resources["BackgroundBrush"] = new SolidColorBrush(GetColor(ConfigurationManager.AppSettings["Back"]));
            Application.Current.MainWindow.Resources["ForegroundBrush"] = new SolidColorBrush(GetColor(ConfigurationManager.AppSettings["Front"]));
        }
        public static Color GetColor(string color)
        {
            byte a = 255, r = 255, g = 255, b = 255;
            try
            {
                color = color.Substring(1, color.Length - 1);
                a = Convert.ToByte(color[0] + "" + color[1], 16);
                r = Convert.ToByte(color[2] + "" + color[3], 16);
                g = Convert.ToByte(color[4] + "" + color[5], 16);
                b = Convert.ToByte(color[6] + "" + color[7], 16);
            }
            catch (Exception)
            {
                (Application.Current.MainWindow as MahApps.Metro.Controls.MetroWindow).ShowMessageAsync("配置文件错误","无法读取颜色设置，已重置为默认值", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
            }
            return Color.FromArgb(a, r, g, b);
        }
        private int GetIndex()
        {
            switch (ConfigurationManager.AppSettings["Theme"])
            {
                case "Red": return 0;
                case "Green": return 1;
                case "Blue": return 2;
                case "Pruple": return 3;
                case "Orange": return 4;
                case "Lime": return 5;
                case "Emerald": return 6;
                case "Teal": return 7;
                case "Cyan": return 8;
                case "Cobalt": return 9;
                case "Indigo": return 10;
                case "Violet": return 11;
                case "Pink": return 12;
                case "Magenta": return 13;
                case "Crimson": return 14;
                case "Amber": return 15;
                case "Yellow": return 16;
                case "Brown": return 17;
                case "Olive": return 18;
                case "Steel": return 19;
                case "Mauve": return 20;
                case "Taupe": return 21;
                case "Sienna": return 22;
            }
            return 2;
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            Accent Accent = ThemeManager.Accents.First(x => x.Name == (string)((StackPanel)theme.SelectedItem).ToolTip);
            AppTheme Theme = ThemeManager.GetAppTheme((bool)dark.IsChecked ? "BaseDark" : "BaseLight");
            ThemeManager.ChangeAppStyle(Application.Current, Accent, Theme);
            Application.Current.MainWindow.Resources["BackgroundBrush"] = new SolidColorBrush(background.SelectedColor);
            Application.Current.MainWindow.Resources["ForegroundBrush"] = new SolidColorBrush(foreground.SelectedColor);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Preview_Click(null, null);
            Color front = foreground.SelectedColor, back = background.SelectedColor;
            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfg.AppSettings.Settings["Theme"].Value = (string)(((StackPanel)theme.SelectedItem).ToolTip);
            cfg.AppSettings.Settings["Front"].Value = "#FF" + (Convert.ToString(front.R, 16) + Convert.ToString(front.G, 16) + Convert.ToString(front.B, 16)).ToUpper();
            cfg.AppSettings.Settings["Back"].Value = "#FF" + (Convert.ToString(back.R, 16) + Convert.ToString(back.G, 16) + Convert.ToString(back.B, 16)).ToUpper();
            cfg.AppSettings.Settings["Dark"].Value = dark.IsChecked.ToString().ToLower();
            cfg.Save();
            ConfigurationManager.RefreshSection("appSettings");
            UpdateSettings();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            theme.SelectedIndex = GetIndex();
            background.SelectedColor = GetColor(ConfigurationManager.AppSettings["Back"]);
            foreground.SelectedColor = GetColor(ConfigurationManager.AppSettings["Front"]);
            if (ConfigurationManager.AppSettings["Dark"] == "true") dark.IsChecked = true; else dark.IsChecked = false;
            Preview_Click(null, null);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\MinecraftToolsBox.exe");
            Application.Current.Shutdown();
        }
        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            theme.SelectedIndex = 2;
            background.SelectedColor = GetColor("#FF252526");
            foreground.SelectedColor = GetColor("#FFFFFFFF");
            dark.IsChecked = false;
        }
    }
}
