using MinecraftToolsBoxSDK;
using HelixToolkit.Wpf;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace MinecraftToolsBox.Commands
{
    /// <summary>
    /// EntityNBT.xaml 的交互逻辑
    /// </summary>
    public partial class EntityNBT : HamburgerMenu, ICommandGenerator
    {
        CommandsGeneratorTemplate CmdGenerator;
        EntityNeutral C1;
        EntityHostile C2;
        EntityFriend C3;
        EntityBoss C4;
        EntityThrow C5;
        EntityTraffic C6;
        EntityOther C7;

        public EntityNBT(CommandsGeneratorTemplate cmdGenerator)
        {
            Application.Current.Dispatcher.Invoke(new Action(delegate {
                C1 = new EntityNeutral();
                C2 = new EntityHostile();
                C3 = new EntityFriend();
                C4 = new EntityBoss();
                C5 = new EntityThrow();
                C6 = new EntityTraffic();
                C7 = new EntityOther();
            }));
            InitializeComponent();potion.BeginInit();
            e1.IsSelected = true;
            CmdGenerator = cmdGenerator;

            ObjReader CurrentHelixObjReader = new ObjReader();
            Model3DGroup MyModel = CurrentHelixObjReader.Read(Environment.CurrentDirectory + "/3D/direction.obj");
            foo.Content = MyModel;
            IList<Point3D> a = new List<Point3D>
            {
                new Point3D(5, 0, 1),
                new Point3D(0, 0, 1)
            };
            tube.Path = new Point3DCollection(a);
        }
        private void Properties(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < pro.Children.Count; i++)
            {
                object obj = pro.Children[i];
                if (obj is NumericUpDown) ((NumericUpDown)obj).Value = 0;
                else if (obj is CheckBox) ((CheckBox)obj).IsChecked = false;
            }
        }
        public string GenerateCommand()
        {
            string Tag = GetTag();
            if (Tag == "请输入拴绳坐标") return Tag;
            if (Tag != "") Tag = "{" + Tag.Substring(0, Tag.Length - 1) + "}";
            return "/summon " + ((TreeViewItem)view.SelectedItem).ToolTip + " ~ ~ ~ " + Tag;
        }
        string GetTag()
        {
            string tag = "";
            if (fire.Value != 0) tag += "Fire:" + Convert.ToInt16(fire.Value * 20) + ",";
            if (air.Value != 15) tag += "Air:" + Convert.ToInt16(air.Value * 20) + ",";
            if (onground.IsChecked == true) tag += "OnGround:true,";
            if (inu.IsChecked == true) tag += "Invulnerable:true,";
            if (name.Text != "") tag += "CustomName:" + name.Text + ",";
            if (NVisible.IsChecked == true) tag += "CustomNameVisible:true,";
            if (silent.IsChecked == true) tag += "Silent:true,";
            if (glow.IsChecked == true) tag += "Glowing:true,";
            if (fall.Value != 0) tag += "FallDistance:" + Convert.ToSingle(fall.Value) + ",";
            if (NoG.IsChecked == true) tag += "NoGravity:true,";
            if (mob.IsEnabled)
            {
                if (canP.IsChecked == true) tag += "CanPickUpLoot:" + pick.IsChecked + ",";
                if (noAI.IsChecked == true) tag += "NoAI:true,";
                if (dis.IsChecked == true) tag += "PersistenceRequired:true,";
                if (left.IsChecked == true) tag += "LeftHanded:true,";
                if (health.Value != null) tag += "Health:" + health.Value + ",";
                if (abs.Value != 0) tag += "AbsorptionAmount:" + abs.Value + ",";
                if (hurt.Value != 0) tag += "HurtByTimestamp:" + Convert.ToInt32(hurt.Value * 20) + ",";
                if (getH.Value != 0) tag += "HurtTime:" + Convert.ToInt16(getH.Value * 20) + ",";
                if (leashed.IsChecked == true)
                {
                    tag += "Leashed:true,";
                    if (LocX.Value == null || LocY.Value == null || LocZ.Value == null) return "请输入拴绳坐标";
                    tag += "Leash:{X:" + LocX.Value + ",Y:" + LocY.Value + ",Z:" + LocZ.Value + "},";
                }

                string handItem = "0:" + (H1.Text == "" ? "{}" : H1.Text) + "," + "1:" + (H2.Text == "" ? "{}" : H2.Text);
                if (handItem != "0:{},1:{}") tag += "HandItems:[" + handItem + "],";

                string handDrop = "0:" + (H1C.Value == null ? "0.085f" : Convert.ToSingle(H1C.Value / 100) + "f") + ",1:" + (H2C.Value == null ? "0.085f" : Convert.ToSingle(H2C.Value / 100) + "f");
                if (handDrop != "0:0.085f,1:0.085f") tag += "HandDropChances:[" + handDrop + "],";

                string items = "0:" + (A4.Text == "" ? "{}" : A4.Text) + ",1:" + (A3.Text == "" ? "{}" : A3.Text) + ",2:" + (A2.Text == "" ? "{}" : A2.Text) + ",3:" + (A1.Text == "" ? "{}" : A1.Text);
                if (items != "0:{},1:{},2:{},3:{}") tag += "ArmorItems:[" + items + "],";

                string chances = "0:" + (A4C.Value == null ? "0.085f" : (A4C.Value / 100) + "f") + ",1:" + (A3C.Value == null ? "0.085f" : (A3C.Value / 100) + "f") + ",2:" + (A2C.Value == null ? "0.085f" : (A2C.Value / 100) + "f") + ",3:" + (A1C.Value == null ? "0.085f" : (A1C.Value / 100) + "f");
                if (chances != "3:0.085f,2:0.085f,1:0.085f,0:0.085f") tag += "ArmorDropChances:[" + chances + "],";

                string Attributes = "";
                if (value1.Value != 32) Attributes += "{Name:generic.followRange,Base:" + value1.Value + "d},";
                if (value2.Value != 0.7) Attributes += "{Name:generic.movementSpeed,Base:" + value2.Value + "d}";
                if (value3.Value != 0) Attributes += "{Name:generic.armorToughness,Base:" + value3.Value + "d}";
                if (value4.Value != 0) Attributes += "{Name:generic.armor,Base:" + value4.Value + "d}";
                if (value5.Value != 20) Attributes += "{Name:generic.maxHealth,Base:" + value5.Value + "d}";
                if (value6.Value != 2 && (string)((TreeViewItem)view.SelectedItem).ToolTip != "VillagerGolem") Attributes += "{Name:generic.attackDamage,Base:" + value6.Value + "d}";
                if (value7.Value != 0) Attributes += "{Name:generic.knockbackResistance,Base:" + value7.Value + "d}";
            }
            string p = potion.GenerateCommand();
            if (p != "") tag += "ActiveEffects:[" + p + "],";
            if (Motion.IsEnabled) tag += "Motion:[" + X.Value + "d," + Y.Value + "d," + Z.Value + "d" + "],";

            string child = "";
            if (C1 != null && C1.IsLoaded) child = C1.getNBT();
            else if (C3 != null && C3.IsLoaded) child = C3.getNBT();
            else if (C5 != null && C5.IsLoaded) child = C5.getNBT();
            else if (C2 != null && C2.IsLoaded) child = C2.getNBT();
            else if (C4 != null && C4.IsLoaded) child = C4.getNBT();
            else if (C6 != null && C6.IsLoaded) child = C6.getNBT();
            else if (C7 != null && C7.IsLoaded) child = C7.getNBT();

            tag = child + tag;
            return tag;
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (C1 == null) return;
            TreeViewItem t = (TreeViewItem)view.SelectedItem;
            string id = (string)t.ToolTip;
            if (t != neutral && t != hostile && t != friend && t != BOSS && t != _throw && t != traffic && t != other) t = (TreeViewItem)t.Parent;
            if (t == neutral) { EntityData.Content = C1; C1.Enable(id); goto isMob; }
            if (t == hostile) { EntityData.Content = C2; C2.Enable(id); goto isMob; }
            if (t == friend) { EntityData.Content = C3; C3.Enable(id); goto isMob; }
            if (t == BOSS) { EntityData.Content = C4; C4.Enable(id); goto isMob; }
            if (t == _throw) { EntityData.Content = C5; C5.Enable(id); goto isMob; }
            if (t == traffic) { EntityData.Content = C6; C6.Enable(id); goto notMob; }
            if (t == other) { EntityData.Content = C7; C7.Enable(id); goto notMob; }
            isMob:; mob.IsEnabled = true; show.Visibility = Visibility.Hidden; return;
            notMob:; mob.IsEnabled = false; show.Visibility = Visibility.Visible; return;
        }

        private void Location(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (X == null || Y == null || Z == null || tube == null) return;
            IList<Point3D> a = new List<Point3D>
            {
                new Point3D((double)X.Value, (double)Z.Value, (double)Y.Value + 1),
                new Point3D(0, 0, 1)
            };
            tube.Path = new Point3DCollection(a);
        }

        private void Motion_Checked(object sender, RoutedEventArgs e)
        {
            Motion.IsEnabled = (bool)motion.IsChecked;
        }

        private void Get_Click(object sender, RoutedEventArgs e)
        {
            string Tag = GetTag();
            if (Tag != "") Tag = "," + Tag.Substring(0, Tag.Length - 1);
            CmdGenerator.AddCommand("{id:" + ((TreeViewItem)view.SelectedItem).ToolTip + Tag + "}");
        }
        
        private void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            Content = e.ClickedItem;
        }
    }
}