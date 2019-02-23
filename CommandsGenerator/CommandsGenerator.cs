using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MinecraftToolsBox;
using MinecraftToolsBox.Commands;
using MinecraftToolsBoxSDK;
using MTB.Commands;

namespace CommandsGenerator
{
    public class CommandsGenerator : MtbPlugin
    {
        public CommandsGeneratorTemplate Window = null;

        public override bool CheckUpdate()
        {
            return false;
        }

        public override Dictionary<string, Tile[]> GetPages()
        {
            Tile t1 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/basic.png")), Height = 80, Width = 80 },
                Title = "基础命令",
                ToolTip = "基本的原版命令"
            };
            t1.Click += Tile_Click;

            Tile t2 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/server.png")), Height = 80, Width = 80 },
                Title = "服务器命令",
                ToolTip = "原版服务器命令"
            };
            t2.Click += Tile_Click;

            Tile t3 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/entity.png")), Height = 80, Width = 80 },
                Title = "实体命令",
                ToolTip = "适用于所有实体的命令"
            };
            t3.Click += Tile_Click;

            Tile t4 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/player.png")), Height = 80, Width = 80 },
                Title = "玩家命令",
                ToolTip = "仅适用于玩家的命令"
            };
            t4.Click += Tile_Click;

            Tile t5 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/book.png")), Height = 80, Width = 80 },
                Title = "Json书",
                ToolTip = "制作一本含有Json文本的书"
            };
            t5.Click += Tile_Click;

            Tile t6 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/sign.png")), Height = 80, Width = 80 },
                Title = "告示牌",
                ToolTip = "自定义告示牌"
            };
            t6.Click += Tile_Click;

            Tile t7 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/tellraw.png")), Height = 80, Width = 80 },
                Title = "消息文本",
                ToolTip = "自定义在聊天框出现的提示文本"
            };
            t7.Click += Tile_Click;

            Tile t8 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/title.png")), Height = 80, Width = 80 },
                Title = "显示标题",
                ToolTip = "自定义出现在屏幕上的标题"
            };
            t8.Click += Tile_Click;

            Tile t9 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/scoreboard_obj.png")), Height = 80, Width = 80 },
                Title = "记分板目标",
                ToolTip = "创建/修改记分板"
            };
            t9.Click += Tile_Click;

            Tile t10 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/scoreboard_Player.png")), Height = 80, Width = 80 },
                Title = "记分板玩家",
                ToolTip = "管理玩家记分板分数"
            };
            t10.Click += Tile_Click;

            Tile t11 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/scoreboard_Team.png")), Height = 80, Width = 80 },
                Title = "记分板队伍",
                ToolTip = "管理记分板队伍"
            };
            t11.Click += Tile_Click;

            Tile t12 = new Tile
            {
                Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/tileIcons/ItemNBT.png")), Height = 80, Width = 80 },
                Title = "物品NBT",
                ToolTip = "生成自定义的物品"
            };
            t12.Click += Tile_Click;

            Tile t13 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "实体NBT",
                ToolTip = "生成自定义的实体"
            };
            t13.Click += Tile_Click;

            Tile t14 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "物品与生成",
                ToolTip = "获取物品"
            };
            t14.Click += Tile_Click;

            Tile t15 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "检测与执行",
                ToolTip = "在特定条件下执行命令"
            };
            t15.Click += Tile_Click;

            Tile t16 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "方块NBT/放置填充方块",
                ToolTip = "生成自定义方块"
            };
            t16.Click += Tile_Click;

            Tile t17 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "村民交易",
                ToolTip = "自定义村民交易内容"
            };
            t17.Click += Tile_Click;

            Tile t18 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "刷怪笼",
                ToolTip = "自定义刷怪笼生成的实体"
            };
            t18.Click += Tile_Click;

            Tile t19 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "烟花",
                ToolTip = "自定义烟花效果"
            };
            t19.Click += Tile_Click;

            Tile t20 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "旗帜/盾牌",
                ToolTip = "自定义旗帜/盾牌的图案"
            };
            t20.Click += Tile_Click;

            Tile t21 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "药水/药水箭",
                ToolTip = "自定义药水/药水箭效果"
            };
            t21.Click += Tile_Click;

            Tile t22 = new Tile
            {
                Content = new Image { Height = 80, Width = 80 },
                Title = "盔甲架",
                ToolTip = "自定义盔甲架动作",
                IsEnabled = false
            };
            t22.Click += Tile_Click;

            Dictionary<string, Tile[]> groups = new Dictionary<string, Tile[]>
            {
                { "简单命令", new Tile[] { t1, t2, t3, t4 } },
                { "Json文本", new Tile[] { t5, t6, t7, t8 } },
                { "记分板", new Tile[] { t9, t10, t11 } },
                { "高级命令", new Tile[] {t12, t13, t14, t15, t16 } },
                { "自定义", new Tile[] { t17, t18, t19, t20, t21, t22 } }
            };
            return groups;
        }


        public override void LoadPlugin()
        {
            Name = "命令生成器";
            Author = "RCN_Team";
            Description = "生成Minecraft原版命令";
            Version = "0.1";
            Icon = new BitmapImage(new Uri("pack://application:,,,/CommandsGenerator;component/images/cmd_block.png"));
        }

        public override void UnloadPlugin()
        {
            
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            if (Window == null)
            {
                Window = new CommandsGeneratorTemplate(this);
                Window.win = (Application.Current.MainWindow as IMainWindowCommands).AddWindow(Icon, Window, "命令生成器", this);
                Window.win.WindowClosed += Win_WindowClosed;
            }
            object content = null;
            string title = (sender as Tile).Title;
            switch (title)
            {
                case "基础命令": content = new BasicCommands(Window); break;
                case "服务器命令": content = new ServerCommands(Window); break;
                case "实体命令": content = new EntityCommands(Window); break;
                case "玩家命令": content = new PlayerCommands(Window); break;
                case "Json书": content = new Book(Window); break;
                case "告示牌": content = new Sign(Window); break;
                case "消息文本": content = new Tellraw(Window); break;
                case "显示标题": content = new Title(Window); break;
                case "记分板目标": content = new ScoreboardObjective(Window); break;
                case "记分板玩家": content = new ScoreboardPlayers(Window); break;
                case "记分板队伍": content = new ScoreboardTeam(Window); break;
                case "物品NBT": content = new ItemNBT(Window);break;
                case "实体NBT":content = new EntityNBT(Window);break;
                case "物品与生成":content = new GetElement();break;
                case "检测与执行":content = new ExecuteAndDetect();break;
                case "方块NBT/放置填充方块": content = new SetBlocks(Window); break;
                case "村民交易": content = new VillagerTrade(Window); break;
                case "刷怪笼": content = new MobSpawner(Window); break;
                case "烟花": content = new Firework(Window); break;
                case "旗帜/盾牌": content = new Banners(Window); break;
                case "药水/药水箭": content = new Potion(Window); break;
                case "盔甲架": content = new ArmorStand(Window); break;
            }
            Window.AddPage(title, content);
        }

        private void Win_WindowClosed(object Sender, RoutedEventArgs e) => Window = null;
    }
}
