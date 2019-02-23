using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MinecraftToolsBoxSDK;
using MTB.GameHandler;

namespace TranslationTools
{
    public class TranslationTools : MtbPlugin
    {
        public override bool CheckUpdate()
        {
            return false;
        }

        public override Dictionary<string, Tile[]> GetPages()
        {
            Tile t1 = new Tile
            {
                Content = new Image { },
                Title = "地图扫描",
                ToolTip = "扫描地图中需要翻译的内容"
            };
            t1.Click += T1_Click;

            Tile t2 = new Tile
            {
                Content = new Image { },
                Title = "翻译",
            };
            t2.Click += T2_Click;

            Tile t3 = new Tile
            {
                Content = new Image { },
                Title = "应用到地图",
            };
            t3.Click += T3_Click;

            return new Dictionary<string, Tile[]>
            {
                { "地图翻译", new Tile[] { t1, t2, t3 } }
            };
        }



        public override void LoadPlugin()
        {
            Name = "翻译工具";
            Author = "RCN_Team";
            Description = "Tools for translating maps";
            Version = "1.0";
            Icon = new BitmapImage(new Uri("pack://application:,,,/TranslationTools;component/translation.png"));
        }

        public override void UnloadPlugin()
        {
            
        }

        private void T1_Click(object sender, RoutedEventArgs e) => (Application.Current.MainWindow as IMainWindowCommands).AddWindow(new BitmapImage(new Uri("pack://application:,,,/TranslationTools;component/sign.png")), new MapScanner(), "地图扫描", this);

        private void T2_Click(object sender, RoutedEventArgs e) => (Application.Current.MainWindow as IMainWindowCommands).AddWindow(new BitmapImage(new Uri("pack://application:,,,/TranslationTools;component/sign.png")), new MapTranslator(), "翻译", this);

        private void T3_Click(object sender, RoutedEventArgs e) => (Application.Current.MainWindow as IMainWindowCommands).AddWindow(new BitmapImage(new Uri("pack://application:,,,/TranslationTools;component/sign.png")), new ApplyToMap(), "应用到地图", this);
    }
}
