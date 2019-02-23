using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MinecraftToolsBoxSDK
{
    public class SmoothScrollViewer : ScrollViewer
    {
        private DispatcherTimer tm = new DispatcherTimer();

        private int count;

        private double delta;

        private double target;

        private bool down;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            tm.Tick += new EventHandler(Scroll);
            tm.Interval = TimeSpan.FromSeconds(0.01);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;
            count = 0;
            if (down)
            {
                target = HorizontalOffset;
            }
            if (e.Delta < 0)
            {
                target += 60.0;
            }
            else
            {
                target -= 60.0;
            }
            delta = (int)(target - HorizontalOffset) / 10;
            tm.Start();
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            down = true;
        }

        private void Scroll(object source, EventArgs e)
        {
            ScrollToHorizontalOffset(HorizontalOffset + delta);
            count++;
            if (count >= 10)
            {
                tm.Stop();
                target = HorizontalOffset;
                down = false;
            }
        }
    }
    public class VerticalSmoothScrollViewer : ScrollViewer
    {
        private DispatcherTimer tm = new DispatcherTimer();

        private int count;

        private double delta;

        private double target;

        private bool down;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            tm.Tick += new EventHandler(Scroll);
            tm.Interval = TimeSpan.FromSeconds(0.01);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            e.Handled = true;
            count = 0;
            if (down)
            {
                target = VerticalOffset;
            }
            if (e.Delta < 0)
            {
                target += 60.0;
            }
            else
            {
                target -= 60.0;
            }
            delta = (int)(target - VerticalOffset) / 10;
            tm.Start();
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            down = true;
        }

        private void Scroll(object source, EventArgs e)
        {
            ScrollToVerticalOffset(VerticalOffset + delta);
            count++;
            if (count >= 10)
            {
                tm.Stop();
                target = VerticalOffset;
                down = false;
            }
        }
    }
}
