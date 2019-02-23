using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;


namespace MinecraftToolsBoxSDK
{
    class IPTextBox : TextBox
    {
        IPTextBox leftBox = null;
        IPTextBox rightBox = null;

        //设置邻居
        public void SetNeighbour(IPTextBox left, IPTextBox right)
        {
            leftBox = left;
            rightBox = right;
        }

        // 按下键
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            // 删除键
            if (e.Key == Key.Back)
            {
                if ((CaretIndex == 0) && (leftBox != null) && SelectionLength == 0)
                {
                    leftBox.Focus();
                    leftBox.CaretIndex = leftBox.Text.Length;
                    e.Handled = true;
                }
            }

            // 光标左移
            if (e.Key == Key.Left)
            {
                if ((CaretIndex == 0) && (leftBox != null))
                {
                    leftBox.Focus();
                    leftBox.CaretIndex = leftBox.Text.Length;
                    e.Handled = true;
                }
            }

            // 光标右移
            if (e.Key == Key.Right)
            {
                if ((CaretIndex == Text.Length) && (rightBox != null))
                {
                    rightBox.Focus();
                    rightBox.CaretIndex = 0;
                    e.Handled = true;
                }
            }
            if (e.Key == Key.Subtract)
            {
                e.Handled = true;
                return;
            }
        }

        // 文本输入时
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);

            char ch = char.Parse(e.Text);

            if (ch == '.')
            {
                if ((CaretIndex == Text.Length) && (rightBox != null) && Text.Length > 0)
                {
                    rightBox.Focus();
                    rightBox.SelectAll();
                    e.Handled = true;
                    return;
                }
            }
            if (ch >= 19968 && ch <= 40869)
            {
                Text = Text.Replace(e.Text, string.Empty);
                SelectionStart = Text.Length;
                e.Handled = true;
                return;
            }
            if (ch < '0' || ch > '9')
            {
                e.Handled = true;
                return;
            }
            if ((Text.Length >= 3) && SelectionLength == 0)
            {
                e.Handled = true;
                return;
            }
        }
        
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            Int16.TryParse(Text, out short ip);
            if (ip > 255) Text = "255";

            //01 -> 去掉首个0
            string pattern = @"^0\d+$";
            if (Regex.IsMatch(Text, pattern))
            {
                Text = Text.Substring(1, Text.Length - 1);
            }
            SelectionStart = Text.Length;

            if (Text.Length == 3)
            {
                if ((CaretIndex == Text.Length) && (rightBox != null))
                {
                    rightBox.Focus();
                    rightBox.SelectAll();
                }
            }
        }
    }

}
