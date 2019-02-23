using System;
using System.Collections.Generic;
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
using MinecraftToolsBox;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// MtbWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MtbWindow : Grid
    {
        public static DragControlHelper Helper = (Application.Current.MainWindow as IMainWindowCommands).GetHelper();
        public TaskBarButton Task { get; set; }
        public string Title { get => title.Text; set => title.Text = value; }
        public ImageSource Icon { get => img.Source; set => img.Source = value; }
        public object Content { get => content.Content; set => content.Content = value; }
        public WindowView View;

        public MtbWindow()
        {
            InitializeComponent();
        }

        public void Close()
        {
            RaiseEvent(new RoutedEventArgs(WindowClosedEvent, this));
            (Application.Current.MainWindow as IMainWindowCommands).CloseWindow(this);
        }

        public void Close(object sender, RoutedEventArgs e) => Close();

        public MtbWindow(ImageSource icon, object content, string tit, TaskBarButton task)
        {
            InitializeComponent();
            Icon = icon;
            Content = content;
            Title = tit;
            Task = task;
            View = new WindowView(icon, tit, this);
            task.WindowItems.Children.Add(View);
            Task.Badge.Badge = Task.WindowItems.Children.Count;
        }

        public static readonly RoutedEvent WindowClosedEvent
            = EventManager.RegisterRoutedEvent("WindowClosed", RoutingStrategy.Bubble, typeof(WindowClosedEventHandler), typeof(MtbWindow));

        public event WindowClosedEventHandler WindowClosed
        {
            add
            {
                AddHandler(WindowClosedEvent, value);
            }
            remove
            {
                RemoveHandler(WindowClosedEvent, value);
            }
        }
        public delegate void WindowClosedEventHandler(object Sender, RoutedEventArgs e);

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            (Application.Current.MainWindow as IMainWindowCommands).ShowWindow(this);
            Helper.TargetElement = this;
        }
    }
}
