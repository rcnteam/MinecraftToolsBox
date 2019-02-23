using MahApps.Metro.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace MinecraftToolsBoxSDK
{
    public partial class TaskBarButton : Control
    {
        public ImageSource Source { get; set; }
        public object Tip { get; set; }
        public StackPanel WindowItems { get; set; } = new StackPanel { Orientation = Orientation.Horizontal };
        public MtbPlugin Plugin;
        public Badged Badge;
        internal TextBlock Bar;

        static TaskBarButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskBarButton), new FrameworkPropertyMetadata(typeof(TaskBarButton)));
        }

        
        protected override void OnToolTipOpening(ToolTipEventArgs e)
        {
            base.OnToolTipOpening(e);
            IMainWindowCommands main = Application.Current.MainWindow as IMainWindowCommands;
            int offset = (Parent as StackPanel).Children.IndexOf(this) * 48 + 123 - WindowItems.Children.Count * 108;
            main.ShowWindowView(WindowItems, offset < 0 ? 0 : offset);
            Application.Current.MainWindow.PreviewMouseMove += TaskBarButton_MouseMove;
        }
        public void TaskBarButton_MouseMove(object sender, MouseEventArgs e)
        {
            IMainWindowCommands main = Application.Current.MainWindow as IMainWindowCommands;
            Point p1 = e.GetPosition(WindowItems), p2 = e.GetPosition(this);
            if (p1.Y < -10 || p1.X < -10 || p1.X > WindowItems.Children.Count * 216 + 10 || (p2.X < 0 || p2.X > 48) && p2.Y > 0)
            {
                Application.Current.MainWindow.PreviewMouseMove -= TaskBarButton_MouseMove;
                main.CloseWindowView();
            }
        }
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            DataContext = this;
        }
    }
    public class MyBadged : Badged
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            BadgePlacementMode = ControlzEx.BadgePlacementMode.TopRight;
            (VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this) as DependencyObject) as TaskBarButton).Badge = this;
        }
    }
    public class MyTextBlock : TextBlock
    {
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            (VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(this) as DependencyObject) as TaskBarButton).Bar = this;
        }
    }
}
