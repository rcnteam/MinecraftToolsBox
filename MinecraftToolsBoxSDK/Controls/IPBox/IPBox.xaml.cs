using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// IPBox.xaml 的交互逻辑
    /// </summary>
    public partial class IPBox : UserControl
    {
        //
        // 摘要：
        //     构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public IPBox()
        {
            InitializeComponent();

            // 处理粘贴
            DataObject.AddPastingHandler(this, new DataObjectPastingEventHandler(IPTextBox_Paste));

            // 设置textBox次序¨°
            ipTextBox1.SetNeighbour(null, ipTextBox2);
            ipTextBox2.SetNeighbour(ipTextBox1, ipTextBox3);
            ipTextBox3.SetNeighbour(ipTextBox2, ipTextBox4);
            ipTextBox4.SetNeighbour(ipTextBox3, null);
        }
        public String GetIP()
        {
            return ipTextBox1.Text + "." + ipTextBox2.Text + "." + ipTextBox3.Text + "." + ipTextBox4.Text ;
        }

        // 处理粘贴 类似IP格式的才能粘贴
        private void IPTextBox_Paste(object sender, DataObjectPastingEventArgs e)
        {

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string value = e.DataObject.GetData(typeof(string)).ToString();
                Text = value;
            }
            e.CancelCommand();
        }

        #region DP

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(String), typeof(IPBox),
            new FrameworkPropertyMetadata(String.Empty, new PropertyChangedCallback(TextPropertyChangedCallback)));

        [Description("IPAddress IP地址")]
        [Category("Common Properties")]
        public String Text
        {
            get
            {
                return (String)GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        private static void TextPropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs arg)
        {
            if (sender != null && sender is IPBox)
            {
                IPBox ipbox = sender as IPBox;
                ipbox.OnTextUpdated((String)arg.OldValue, (String)arg.NewValue);
            }
        }

        #endregion

        #region Event

        public static readonly RoutedEvent TextUpdatedEvent =
            EventManager.RegisterRoutedEvent("TextUpdated",
             RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<String>), typeof(IPBox));

        [Description("IP地址被更新后发生")]
        public event RoutedPropertyChangedEventHandler<String> TextUpdated
        {
            add
            {
                AddHandler(TextUpdatedEvent, value);
            }
            remove
            {
                RemoveHandler(TextUpdatedEvent, value);
            }
        }

        protected virtual void OnTextUpdated(String oldValue, String newValue)
        {
            RoutedPropertyChangedEventArgs<String> arg =
                new RoutedPropertyChangedEventArgs<String>(oldValue, newValue, TextUpdatedEvent);
            try
            {
                string[] ipnum = newValue.Split('.');
                if (ipnum.Length == 4)
                {
                    ipTextBox1.Text = ipnum[0];
                    ipTextBox2.Text = ipnum[1];
                    ipTextBox3.Text = ipnum[2];
                    ipTextBox4.Text = ipnum[3];
                }
                RaiseEvent(arg);
            }
#pragma warning disable CS0168 // 声明了变量，但从未使用过
            catch (Exception ex)
#pragma warning restore CS0168 // 声明了变量，但从未使用过
            {

            }

        }

        #endregion

        #region Validate

        /// <summary>
        /// IP地址格式正确返回 true
        /// </summary>
        /// <returns></returns>
        public bool ValidateIPAddress()
        {
            string pattern = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";

            //判断IP地址格式
            return Regex.IsMatch(Text, pattern);
        }

        #endregion

        private void IPTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (
                !string.IsNullOrWhiteSpace(ipTextBox1.Text)
                || !string.IsNullOrWhiteSpace(ipTextBox2.Text)
                || !string.IsNullOrWhiteSpace(ipTextBox3.Text)
                || !string.IsNullOrWhiteSpace(ipTextBox4.Text)
                )
            {
                Text = ipTextBox1.Text + "." + ipTextBox2.Text + "." + ipTextBox3.Text + "." + ipTextBox4.Text;
            }
            else
            {
                Text = string.Empty;
            }
        }
    }
}
