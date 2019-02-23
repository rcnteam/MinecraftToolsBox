using MinecraftToolsBoxSDK;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// Banners.xaml 的交互逻辑
    /// </summary>
    public partial class Banners : Grid, ICommandGenerator
    {
        CommandsGeneratorTemplate Tmp;
        SolidColorBrush border = new SolidColorBrush(Color.FromRgb(128, 128, 128));
        bool flag = false;

        BitmapImage b1 = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\banner\\brick.png"));
        BitmapImage b2 = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\banner\\mojang_logo.png"));
        BitmapImage b3 = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\banner\\creeper.png"));
        BitmapImage b4 = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\banner\\skull.png"));
        BitmapImage b5 = new BitmapImage(new Uri(Environment.CurrentDirectory + "\\images\\banner\\flower.png"));

        public Banners(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            Tmp = tmp;
            brick.Source = b1;
            mojang.Source = b2;
            creeper.Source = b3;
            skull.Source = b4;
            flower.Source = b5;
        }

        private void TextColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (sender as ComboBox).Background = ((sender as ComboBox).SelectedItem as ComboBoxItem).Background;
            if (sender == BgColor && view != null) view.Background = ((sender as ComboBox).SelectedItem as ComboBoxItem).Background;
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            if (layers.Items.Count >= 8 && patterns.SelectedIndex != -1) return;
            flag = true;
            StackPanel g = new StackPanel { Height = 68, Orientation = Orientation.Horizontal };
            g.Children.Add(new TextBlock { Width = 10, Text = "1" });
            Border b = new Border { Child = CopyPattern((layerColor.SelectedItem as ComboBoxItem).Background), BorderBrush = border, BorderThickness = new Thickness(1), Width = 32, Height = 62, ToolTip = (patterns.SelectedItem as Border).ToolTip };
            g.Children.Add(b);
            ComboBox cb = new ComboBox { Width = 54, Height = 29, Margin = new Thickness(5, 0, 0, 0) };
            foreach(ComboBoxItem item in layerColor.Items)
            {
                cb.Items.Add(new ComboBoxItem { BorderBrush = item.BorderBrush, BorderThickness = item.BorderThickness, Background = item.Background, Margin = item.Margin, Style = item.Style, Height = 20, Width = 20 });
            }
            cb.SelectionChanged += LayerColor_SelectionChanged;
            cb.SelectedIndex = layerColor.SelectedIndex;
            g.Children.Add(cb);
            layers.Items.Insert(0, g);
            layers.SelectedIndex = 0;
            RefershIndex();
            view.Children.Add(new Grid { Background = new VisualBrush(b) });
            flag = false;
        }

        private UIElement CopyPattern(Brush background)
        {
            UIElement element = (patterns.SelectedItem as Border).Child, result = null;
            if (element is Grid)
            {
                result = new Grid();
                (result as Grid).Children.Add(new System.Windows.Shapes.Path { Data = p1.Data, Fill = background });
                (result as Grid).Children.Add(new System.Windows.Shapes.Path { Data = p2.Data, Fill = background });
            }
            else if (element is Ellipse)
            {
                Ellipse e = element as Ellipse;
                result = new Ellipse { HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center, Width = e.Width, Height = e.Height, Fill = background };
            }
            else if(element is Image)
            {
                string path = "";
                Image img = element as Image;
                if (img.Source == b1) path = "brick.png"; else if (img.Source == b2) path = "mojang_logo.png"; else if (img.Source == b3) path = "creeper.png"; else if (img.Source == b4) path = "skull.png"; else path = "flower.png";
                System.Drawing.Bitmap curBitmap = new System.Drawing.Bitmap(Environment.CurrentDirectory + "\\images\\banner\\" + path);
                Color c = (background as SolidColorBrush).Color;
                for (int i = 0; i < curBitmap.Width; i++)
                {
                    for (int j = 0; j < curBitmap.Height; j++)
                    {
                        System.Drawing.Color curColor = curBitmap.GetPixel(i, j);
                        if (curColor.A != 0) curBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(255, c.R, c.G, c.B));
                    }
                }
                curBitmap.Save(Environment.CurrentDirectory + "\\temp\\banner.png");
                result = new Image { Source = GetImageSource(Environment.CurrentDirectory + "\\temp\\banner.png") };
                File.Delete(Environment.CurrentDirectory + "\\temp\\banner.png");
            }
            else if(element is Rectangle)
            {
                Rectangle r = element as Rectangle;
                if (r.StrokeThickness == 4)
                {
                    result = new Rectangle { Stroke = background, StrokeThickness = 4 };
                }
                else
                {
                    LinearGradientBrush b = r.Fill.Clone() as LinearGradientBrush;
                    if (b.GradientStops[0].Color == Colors.Black) b.GradientStops[0].Color = (background as SolidColorBrush).Color; else b.GradientStops[1].Color = (background as SolidColorBrush).Color;
                    result = new Rectangle { Fill = b };
                }
            }
            else
            {
                result = new System.Windows.Shapes.Path { Data = (element as System.Windows.Shapes.Path).Data, Fill = background };
            }
            return result;
        }
        public static BitmapImage GetImageSource(string filePath)
        {
            BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open));
            FileInfo info = new FileInfo(filePath);
            byte[] bytes = reader.ReadBytes((int)info.Length);
            reader.Close();
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(bytes);
            bitmap.EndInit();
            return bitmap;
        }

        private void Up(object sender, RoutedEventArgs e)
        {
            int current = layers.SelectedIndex;
            if (current == 0) return;
            UIElement v = view.Children[current];
            view.Children.RemoveAt(current);
            view.Children.Insert(current - 1, v);
            object selected = layers.Items[current];
            layers.Items.RemoveAt(current);
            layers.Items.Insert(current - 1, selected);
            layers.SelectedIndex = current - 1;
            RefershIndex();
        }
        private void Down(object sender, RoutedEventArgs e)
        {
            int current = layers.SelectedIndex;
            if (current == layers.Items.Count - 1) return;
            UIElement v = view.Children[current];
            view.Children.RemoveAt(current);
            view.Children.Insert(current + 1, v);
            object selected = layers.Items[current];
            layers.Items.RemoveAt(current);
            layers.Items.Insert(current + 1, selected);
            layers.SelectedIndex = current + 1;
            RefershIndex();
        }
        private void LayerColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            Brush background = (c.SelectedItem as ComboBoxItem).Background;
            c.Background = background;
            if (flag) return;
            UIElement element = ((c.Parent as StackPanel).Children[1] as Border).Child;
            if (element is Grid)
            {
                ((element as Grid).Children[0] as System.Windows.Shapes.Path).Fill = background;
                ((element as Grid).Children[1] as System.Windows.Shapes.Path).Fill = background;
            }
            else if (element is Ellipse) (element as Ellipse).Fill = background;
            else if (element is Image)
            {
                System.Drawing.Bitmap curBitmap = new System.Drawing.Bitmap(((element as Image).Source as BitmapImage).StreamSource);
                Color co = (background as SolidColorBrush).Color;
                for (int i = 0; i < curBitmap.Width; i++)
                {
                    for (int j = 0; j < curBitmap.Height; j++)
                    {
                        System.Drawing.Color curColor = curBitmap.GetPixel(i, j);
                        if (curColor.A != 0) curBitmap.SetPixel(i, j, System.Drawing.Color.FromArgb(255, co.R, co.G, co.B));
                    }
                }
                curBitmap.Save(Environment.CurrentDirectory + "\\temp\\banner.png");
                (element as Image).Source = GetImageSource(Environment.CurrentDirectory + "\\temp\\banner.png");
                File.Delete(Environment.CurrentDirectory + "\\temp\\banner.png");
            }
            else if (element is Rectangle)
            {
                Rectangle r = element as Rectangle;
                if (r.StrokeThickness == 4) r.Stroke = background;
                else
                {
                    LinearGradientBrush b = r.Fill.Clone() as LinearGradientBrush;
                    if (b.GradientStops[0].Color == Colors.Black) b.GradientStops[0].Color = (background as SolidColorBrush).Color; else b.GradientStops[1].Color = (background as SolidColorBrush).Color;
                }
            }
            else (element as System.Windows.Shapes.Path).Fill = background;
        }
        private void RefershIndex()
        {
            for (int i = 0; i < layers.Items.Count; i++)
            {
                StackPanel s = layers.Items[i] as StackPanel;
                (s.Children[0] as TextBlock).Text = i + 1 + "";
            }
        }
        private void layers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete && layers.SelectedIndex != -1)
            {
                layers.Items.RemoveAt(layers.SelectedIndex);
                RefershIndex();
            }
        }

        public string GenerateCommand()
        {
            string nbt = GetNBT();
            if (nbt != "") nbt = " {BlockEntityTag:" + nbt + "}";
            if (itemNBT.Text != "")
            {
                if (nbt != "") nbt = nbt.Substring(0, nbt.Length - 1) + "," + itemNBT.Text.Substring(1);
                else nbt = " " + itemNBT.Text;
            }
            return "/give @p minecraft:" + (banner.IsChecked == true ? "banner" : "shield") + " 1 " + BgColor.SelectedIndex + nbt;
        }

        private void banner_Checked(object sender, RoutedEventArgs e)
        {
            if (banner.IsChecked == true)
            {
                customName.IsEnabled = true;
                getBNBT.IsEnabled = true;
            }
            else
            {
                customName.IsEnabled = false;
                getBNBT.IsEnabled = false;
            }
        }

        private void GetINBT(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            if (nbt != "") nbt = "{BlockEntityTag:" + nbt + "}";
            if (itemNBT.Text != "")
            {
                if (nbt != "") nbt = nbt.Substring(0, nbt.Length - 1) + "," + itemNBT.Text.Substring(1);
                else nbt = " " + itemNBT.Text;
            }
            Tmp.AddCommand("{id:\"" + (banner.IsChecked == true ? "banner" : "shield") + "\",Damage:" + BgColor.SelectedIndex + ",tag:{" + nbt + "}");
        }

        private void GetBNBT(object sender, RoutedEventArgs e)
        {
            Tmp.AddCommand(GetNBT());
        }
        private string GetNBT()
        {
            string nbt = "";
            if (customName.Text != "") nbt += "CustomName:" + customName.Text + ",";
            nbt += "Base:" + BgColor.SelectedIndex + ",";
            nbt = nbt.Substring(0, nbt.Length - 1);
            if (layers.Items.Count != 0)
            {
                string patterns = "";
                foreach (StackPanel panel in layers.Items)
                {
                    patterns = "{Color:" + (panel.Children[2] as ComboBox).SelectedIndex + "," + "Pattern:" + (panel.Children[1] as Border).ToolTip + "}," + patterns;
                }
                nbt += ",Patterns:[" + patterns.Substring(0, patterns.Length - 1) + "]";
            }
            if (nbt != "") return "{" + nbt + "}"; else return nbt;
        }
    }
}
