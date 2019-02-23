using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.VisualBasic.Devices;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace MinecraftToolsBox
{
    /// <summary>
    /// Flyout.xaml 的交互逻辑
    /// </summary>
    public partial class PerformanceWindow : Grid
    {
        static ChartValues<ObservableValue> Cpu1 = new ChartValues<ObservableValue>();
        static ChartValues<ObservableValue> Ram1 = new ChartValues<ObservableValue>();
        public SeriesCollection Cpu { get; set; } = new SeriesCollection { new LineSeries { AreaLimit = -10, Values = Cpu1, Title = "CPU" } };
        public SeriesCollection Ram { get; set; } = new SeriesCollection { new LineSeries { AreaLimit = -10, Values = Ram1, Title = "内存" } };
        SolidColorBrush red = new SolidColorBrush(Color.FromRgb(220, 0, 0));
        SolidColorBrush yellow = new SolidColorBrush(Color.FromRgb(200, 200, 50));
        SolidColorBrush green = new SolidColorBrush(Color.FromRgb(50, 200, 50));

        PerformanceCounter cpuPerformance = new PerformanceCounter() { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };
        ComputerInfo ci = new ComputerInfo(); 
        DispatcherTimer timer = new DispatcherTimer();
        public int TotalRam { get; set; }

        public PerformanceWindow()
        {
            InitializeComponent();
            c1.BeginInit();
            c2.BeginInit();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(AnimatedPlot);
            
            TotalRam = Convert.ToInt32(ci.TotalPhysicalMemory / 1024 / 1024);
            ramUsageText.Text = "内存使用率：0%\n" + (TotalRam - Convert.ToInt32(ci.AvailablePhysicalMemory / 1024 / 1024)) + "/" + TotalRam + "MB";
            DataContext = this;
        }
        private void AnimatedPlot(object sender, EventArgs e)
        {
            double y = cpuPerformance.NextValue();
            cpuUsageText.Text = string.Format("CPU使用率：{0:0}%", y);
            if (y > 85) cpuUsageText.Foreground = red; else if (y > 70) cpuUsageText.Foreground = yellow; else cpuUsageText.Foreground = green;
            
            double R = Math.Round((double)((ulong)TotalRam - ci.AvailablePhysicalMemory / 1024 / 1024));
            double Rp = (R * 100) / TotalRam;
            ramUsageText.Text = string.Format("内存使用率：{0:0}%\n" + R + "/" + TotalRam + "MB", Rp);
            if (Rp > 85) ramUsageText.Foreground = red; else if (Rp > 70) ramUsageText.Foreground = yellow; else ramUsageText.Foreground = green;

            Cpu1.Insert(0, new ObservableValue(Math.Round(y)));
            Ram1.Insert(0, new ObservableValue(R));
            if (Cpu1.Count == 62)
            {
                Cpu1.RemoveAt(61);
                Ram1.RemoveAt(61);
            }
        }
        private void start_Click(object sender, RoutedEventArgs e)
        {
            timer.IsEnabled = (bool)start.IsChecked;
        }
    }
}