using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MinecraftToolsBoxSDK
{
    public class ChildWindow : Grid
    {
        private ImageSource icon;
        private string title;
        private TaskBarButton task;

        static ChildWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChildWindow), new FrameworkPropertyMetadata(typeof(ChildWindow)));
        }

        public ChildWindow(ImageSource icon, object content, string title, TaskBarButton task)
        {
            this.icon = icon;
            //Content = content;
            this.title = title;
            this.task = task;
        }
        public ChildWindow()
        {
            
        }

        internal void Close()
        {
            throw new NotImplementedException();
        }

        public static readonly RoutedEvent WindowClosedEvent
            = EventManager.RegisterRoutedEvent("WindowClosedEvent", RoutingStrategy.Bubble, typeof(WindowClosedEventHandler), typeof(ChildWindow));

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
    }
}
