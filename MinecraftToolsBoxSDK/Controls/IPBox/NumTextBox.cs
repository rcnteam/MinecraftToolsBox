using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System;

namespace MinecraftToolsBoxSDK
{
    public class NumTextBox : TextBox
    {
        public int Max { get; set; } = int.MaxValue;
        public int Min { get; set; } = int.MinValue;
        public bool INT { get; set; } = false;

        // 文本输入时
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            char ch = char.Parse(e.Text);

            if (ch == '.' && !INT) return;

            if (ch == '-' && SelectionStart == 0 && Min < 0) return;

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
        }

        // 文本输入
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);

            int ip = 0;
            if (!Text.Contains(".") && !Text.Substring(1, Text.Length - 1).Contains("-") && Text != "-")
                try { ip = int.Parse(Text); }catch(FormatException a) { Console.WriteLine(a); }
            if (ip > Max) Text = Max + "";
            if (ip < Min) Text = Min + "";

            //01 -> 去掉首个0
            string pattern = @"^0\d+$";
            if (Regex.IsMatch(Text, pattern)) Text = Text.Substring(1, Text.Length - 1);
            SelectionStart = Text.Length;
        }
    }
    public class LocBox : NumTextBox
    {
        public LocBox leftBox { get; set; } = null;
        public LocBox rightBox { get; set; } = null;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Max = 30000000;Min = -30000000;
        }

        //设置邻居
        public void setNeighbour(LocBox left, LocBox right)
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
            if (e.Key == Key.Left || e.Key == Key.Up)
            {
                if ((CaretIndex == 0) && (leftBox != null))
                {
                    leftBox.Focus();
                    leftBox.CaretIndex = leftBox.Text.Length;
                    e.Handled = true;
                }
            }

            // 光标右移
            if (e.Key == Key.Right || e.Key == Key.Down)
            {
                if ((CaretIndex == Text.Length) && (rightBox != null))
                {
                    rightBox.Focus();
                    rightBox.CaretIndex = 0;
                    e.Handled = true;
                }
            }
            if (e.Key == Key.OemMinus || e.Key == Key.Subtract) { Text = "-"; SelectionStart = Text.Length; e.Handled = true; return; }
        }
    }
}