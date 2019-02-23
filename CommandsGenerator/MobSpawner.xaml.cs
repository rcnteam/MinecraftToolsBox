using MinecraftToolsBoxSDK;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// MobSpawner.xaml 的交互逻辑
    /// </summary>
    public partial class MobSpawner : Grid, ICommandGenerator
    {
        public CommandsGeneratorTemplate Tmp;
        public ObservableCollection<Potential> Data = new ObservableCollection<Potential>();
        public MobSpawner(CommandsGeneratorTemplate tmp)
        {
            InitializeComponent();
            Tmp = tmp;
            potentials.DataContext = Data;
        }

        private void Potentials_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            potentials.Items.Refresh();
        }

        private void Potentials_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            potentials.Items.Refresh();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (potentials.SelectedIndex < 1) return;
            Data.Move(potentials.SelectedIndex, potentials.SelectedIndex - 1);
            potentials.Items.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (potentials.SelectedIndex > Data.Count - 2) return;
            Data.Move(potentials.SelectedIndex, potentials.SelectedIndex + 1);
            potentials.Items.Refresh();
        }

        string GetNBT()
        {
            string nbt = "";
            if (range.Value != 4) nbt += "SpawnRange:" + range.Value + ",";
            if (maxnearby.Value != null) nbt += "MaxNearbyEntities:" + maxnearby.Value + ",";
            if (requiredrange.Value != null) nbt += "RequiredPlayerRange:" + requiredrange.Value + ",";
            if (delay.Value != null) nbt += "Delay:" + delay.Value + ",";
            if (count.Value != 0) nbt += "SpawnCount:" + count.Value + ",";
            if (mindelay.Value != null) nbt += "MinSpawnDelay:" + mindelay.Value + ",";
            if (maxdelay.Value != null) nbt += "MaxSpawnDelay:" + maxdelay.Value + ",";
            if (Data.Count != 0)
            {
                nbt += "SpawnPotentials:[";
                foreach (Potential item in Data)
                {
                    nbt += "{Weight:" + item.Weight + ",Properties:" + item.NBT + "},";
                }
                nbt = nbt.Substring(0, nbt.Length - 1);
                nbt += "],";
            }
            if (nbt != "") nbt = "{" + nbt.Substring(0, nbt.Length - 1) + "}";
            return nbt;
        }
        public string GenerateCommand()
        {
            return "/give @p minecraft:mob_spawner 1 0" + GetNBT();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            if (nbt != "") nbt = ",tag:{" + nbt + "}";
            Tmp.AddCommand("{id:\"mob_spawner\",Count:1,Damage:0" + nbt + "}");
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string nbt = GetNBT();
            if (nbt == "") nbt = "{}";
            Tmp.AddCommand("minecraft:mobspawner-1-" + nbt);
        }
    }
    public class Potential
    {
        public int Weight { get; set; }
        public string NBT { get; set; }
    }
}
