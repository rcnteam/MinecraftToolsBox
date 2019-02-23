using MahApps.Metro.Controls;
using System;

namespace MinecraftToolsBox
{
    /// <summary>
    /// ShowException.xaml 的交互逻辑
    /// </summary>
    public partial class ShowException : MetroWindow
    {
        public ShowException(Exception e)
        {
            InitializeComponent();
            message.Text = e.StackTrace + "\n";
            message.Text += e.Message;
            method.Text += e.Source + "/";
            help.Text = e.HelpLink;
            method.Text += e.TargetSite.Name;
            id.Text += e.HResult;
        }
    }
}
