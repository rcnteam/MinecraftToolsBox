using MinecraftToolsBox.Commands;
using MinecraftToolsBoxSDK;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MTB.Commands
{
    /// <summary>
    /// VillagerTrade.xaml 的交互逻辑
    /// </summary>
    public partial class VillagerTrade : Grid, ICommandGenerator
    {
        public CommandsGeneratorTemplate Tmp;
        public ObservableCollection<Trade> Trades = new ObservableCollection<Trade>();
        public VillagerTrade(CommandsGeneratorTemplate cmd)
        {
            InitializeComponent();
            Tmp = cmd;
            trades.DataContext = Trades;
        }

        private void profession_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (profession == null || career == null) return;
            career.Items.Clear();
            switch (profession.SelectedIndex)
            {
                case 0: career.Items.Add("<随机>"); break;
                case 1: career.Items.Add("<随机>"); career.Items.Add("农民"); career.Items.Add("渔夫"); career.Items.Add("牧羊人"); career.Items.Add("造箭师"); break;
                case 2: career.Items.Add("<随机>"); career.Items.Add("图书管理员"); career.Items.Add("制图师"); break;
                case 3: career.Items.Add("<随机>"); career.Items.Add("牧师"); break;
                case 4: career.Items.Add("<随机>"); career.Items.Add("盔甲商"); career.Items.Add("武器商"); career.Items.Add("工具商"); break;
                case 5: career.Items.Add("<随机>"); career.Items.Add("屠夫"); career.Items.Add("皮匠"); break;
                case 6: career.Items.Add("<随机>"); career.Items.Add("傻子"); break;
            }
            career.SelectedIndex = 1;
        }

        private void trades_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            trades.Items.Refresh();
        }

        private void trades_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            trades.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (trades.SelectedIndex < 1) return;
            Trades.Move(trades.SelectedIndex, trades.SelectedIndex - 1);
            trades.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (trades.SelectedIndex > Trades.Count - 2) return;
            Trades.Move(trades.SelectedIndex, trades.SelectedIndex + 1);
            trades.Items.Refresh();
        }

        public string GenerateCommand()
        {
            return "/summon Villager ~ ~ ~ " + GetNBT();
        }
        string GetNBT()
        {
            string nbt = "";
            if (I1.Text != "" || I2.Text != "" || I3.Text != "" || I4.Text != "" || I5.Text != "" || I6.Text != "" || I7.Text != "" || I8.Text != "")
                nbt += "Inventory:[0:" + I1.Text + ",1:" + I2.Text + ",2:" + I3.Text + ",3:" + I4.Text + "4:" + I5.Text + ",5:" + I6.Text + "6:" + I7.Text + "7:" + I8.Text + "],";
            if (profession.SelectedIndex != 0) nbt += "Profession:" + (profession.SelectedIndex - 1) + ",";
            if (career.SelectedIndex != 0) nbt += "Career:" + (career.SelectedIndex - 1) + ",";
            if (riches.Value != null) nbt += "Riches:" + riches.Value + ",";
            if (careerlevel.Value != null) nbt += "CareerLevel:" + careerlevel.Value + ",";
            if (setwilling.IsChecked == true) nbt += "Willing:" + willing.IsChecked.ToString().ToLower() + ",";
            if (trades.Items.Count != 0)
            {
                nbt += "Offers:{Recipes:[";
                foreach (Trade item in Trades)
                {
                    if (item.BuyA == "" || item.Sell == "") return "请完整填写交易项目，优先填写购入A";
                    nbt += "{rewardExp:" + item.RewardExp.ToString().ToLower() + ",maxUses:" + item.MaxUses + ",uses:" + item.Uses + ",buy:" + item.BuyA + ",sell:" + item.Sell;
                    if (item.BuyB != "") nbt += ",buyB:" + item.BuyB;
                    nbt += "},";
                }
                nbt = nbt.Substring(0, nbt.Length - 1);
                nbt += "]},";
            }
            if (nbt != "") nbt = "{" + nbt.Substring(0, nbt.Length - 1) + "}";
            return nbt;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            if (nbt != "") nbt = "," + nbt;
            Tmp.AddCommand("{id:Villager" + nbt + "}");
        }
    }
    public class Trade
    {
        public int MaxUses { get; set; }
        public int Uses { get; set; } = 0;
        public string BuyA { get; set; } = "";
        public string BuyB { get; set; } = "";
        public string Sell { get; set; } = "";
        public bool RewardExp { get; set; }
    }
    public class RowToIndexConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            return row.GetIndex() + 1 + "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
