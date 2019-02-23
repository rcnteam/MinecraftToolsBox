using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MinecraftToolsBoxSDK
{
    /// <summary>
    /// PlayerSelector.xaml 的交互逻辑
    /// </summary>
    public partial class PlayerSelector : DockPanel
    {
        private List<string> NAME = new List<string>(), TEAM = new List<string>(), SCOREB = new List<string>();
        bool stop = false;

        public PlayerSelector()
        {
            InitializeComponent();
            stop = true;
            team_null.IsChecked = true;
            stop = false;
        }
        public string GetPlayer()
        {
            if (UseTar.IsChecked == true) return TARGET.Text;
            else { GenerateTarget(); return TARGET.Text; }
        }
        public void GenerateTarget()
        {
            if (direct.IsChecked == true) { TARGET.Text = PlayerName.Text; return; }
            if (arguements.IsChecked == true)
            {//初始化所有参数
                string tar = null;
                string COUNT = "c=";
                string LOC = "";
                string MODE = "";
                string TEAM = "";
                string R = "r=";
                string RM = "rm=";
                string L = "l=";
                string LM = "lm=";
                string RX = "rx=";
                string RXM = "rxm=";
                string RY = "ry=";
                string RYM = "rym=";
                string DLOC = "";
                string NAME = "";
                string SCORE = "";

                if (mode_else.IsChecked == true)
                {
                    if (M.Items.Count > 2) goto Next;
                    string str = "";
                    for (int i = 0; i < 4; i++)
                    {
                        M.SelectedIndex = i;
                        str = "选择游戏模式不是" + ((ComboBoxItem)mode.SelectedItem).Content + "模式的玩家(m=!" + mode.SelectedIndex + ")";
                        if ((string)M.SelectedItem == str) break;
                    }
                    M.Items.Add("选择游戏模式不是" + ((ComboBoxItem)mode.SelectedItem).Content + "模式的玩家(m=!" + mode.SelectedIndex + ")");
                Next:;
                }
                if (team_not.IsChecked == true && team.Text != "")
                {
                    string Team = team.Text;
                    if (this.TEAM.Count == 0) goto add;
                    for (int i = 0; i < this.TEAM.Count; i++)
                    {
                        if (this.TEAM[i] == Team) goto Next;
                    }
                    add:
                    T.Items.Add("选择不在队伍  " + Team + "  的玩家");
                    this.TEAM.Add(Team);
                Next:;
                }
                if (name_else.IsChecked == true && name.Text != "")
                {
                    string Name = name.Text;
                    if (this.NAME.Count == 0) goto add;
                    for (int i = 0; i < this.NAME.Count; i++)
                    {
                        if (this.NAME[i] == Name) goto Next;
                    }
                    add:
                    N.Items.Add("选择名字不是  " + Name + "  的玩家");
                    this.NAME.Add(Name);
                Next:;
                }
                if (score_name.Text != "" && (score_max.Value != null || score_min.Value != null))
                {
                    string Board = score_name.Text;
                    int score;string v = "", t = "";
                    if (score_min.Value != null) { score = (int)score_min.Value; t = "≤"; } else { score = (int)score_max.Value; v = "_min"; t = "≥"; }
                    string cmd = "score_" + Board + v + "=" + score;

                    if (SCOREB.Count == 0) goto add;
                    for (int i = 0; i < SCOREB.Count; i++)
                    {
                        if (SCOREB[i] == cmd) goto Next;
                    }
                    add:
                    S.Items.Add("选择计分板 " + Board + " 分数" + t + score + "  的玩家");
                    SCOREB.Add(cmd);
                Next:;
                }

                switch (target.SelectedIndex)//获取参数：@a|@p|@r
                {
                    case 0:tar = "@a"; break;
                    case 1:tar = "@p"; break;
                    case 2:tar = "@r"; break;
                }
                if (count.Value == null) COUNT = ""; else COUNT = "c=" + count.Value + ",";//获取参数：数量（c=）
                if (loc.GetLoc() == "") LOC = ""; else LOC = loc.GetLoc() + ",";//获取参数：坐标x=X,y=Y,z=Z,
                if (mode.SelectedIndex == 4) MODE = "";
                    else {
                    if (mode_else.IsChecked == false) MODE = "m=" + mode.SelectedIndex + ",";
                    else
                        for (int i = 0; i < M.Items.Count; i++)
                        {
                            string[] m = ((string)M.Items[i]).Split('(', ')');
                            MODE += m[1] + ",";
                        }
                }//获取参数：游戏模式（m=）
                if (rmax.Value == null) R = ""; else R += rmax.Value + ",";//获取参数：最大半径（r=）
                if (rmin.Value == null) RM = ""; else RM += rmin.Value + ",";//获取参数：最小半径（rm=）
                if (lmax.Value == null) L = ""; else L += lmax.Value + ",";//获取参数：最高等级（l=）
                if (lmin.Value == null) LM = ""; else LM += lmin.Value + ","; //获取参数：最低等级（lm=）
                if (rxmax.Value == null) RX = ""; else RX += rxmax.Value + ",";//获取参数：最高x旋转（rx=）
                if (rxmin.Value == null) RXM = ""; else RXM += rxmin.Value + ",";//获取参数：最低x旋转（rxm=）
                if (rymax.Value == null) RY = ""; else RY += rymax.Value + ",";//获取参数：最高y旋转（ry=）
                if (rymin.Value == null) RYM = ""; else RYM += rymin.Value + ",";//获取参数：最低y旋转（rym=）
                if (dloc.GetDLoc() == "") DLOC = ""; else DLOC = dloc.GetDLoc()+",";//获取参数：立方体范围（dx=X,dy=Y,dz=Z）
                if (name_else.IsChecked == false) { if (name.Text == "") NAME = ""; else NAME = "name=" + name.Text + ","; }
                else//获取参数：名字（name=）
                    for (int i = 0; i < this.NAME.Count; i++)
                        NAME += "name=!" + this.NAME[i] + ",";
                //获取参数：计分板
                for (int i = 0; i < SCOREB.Count; i++)
                    SCORE += SCOREB[i] + ",";
                if (team_null.IsChecked == true) TEAM = "";//获取参数：队伍（team=）
                else
                {
                    if (team_only.IsChecked == true) TEAM = "team=" + team_name.Text + ",";
                    if (team_not.IsChecked == true)
                        for (int i = 0; i < this.TEAM.Count; i++)
                            TEAM += "team=!" + this.TEAM[i] + ",";
                    if (team_no.IsChecked == true) TEAM = "team=,";
                    if (team_all.IsChecked == true) TEAM = "team=!,";
                }
                string ARG = COUNT + LOC + DLOC + MODE + TEAM + NAME + R + RM + L + LM + RX + RXM + RY + RYM + SCORE;
                if (ARG.Length == 0) { TARGET.Text = tar; return; }
                TARGET.Text = tar+"["+ ARG.Substring(0, ARG.Length - 1) + "]";
            }
        }

        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mode.SelectedIndex == 4)
            {
                if (mode_else != null) mode_else.IsEnabled = false;
                if (addM != null) addM.IsEnabled = false;
                if (addM != null) delM.IsEnabled = false;
                if (addM != null) M.IsEnabled = false;
                if (mode_else != null) mode_else.IsChecked = false;
            }
            else if (mode_else != null) mode_else.IsEnabled = true;
        }
        private void Enable(object sender, RoutedEventArgs e)
        {
            if (stop) return;
            if (sender == mode_else)
            {
                if (mode_else.IsChecked == true)
                {
                    addM.IsEnabled = true;
                    delM.IsEnabled = true;
                    M.IsEnabled = true;
                }
                else
                {
                    addM.IsEnabled = false;
                    delM.IsEnabled = false;
                    M.IsEnabled = false;
                }
            }
            else if (sender == name_else)
            {
                if (name_else.IsChecked == true)
                {
                    addN.IsEnabled = true;
                    delN.IsEnabled = true;
                    N.IsEnabled = true;
                }
                else
                {
                    addN.IsEnabled = false;
                    delN.IsEnabled = false;
                    N.IsEnabled = false;
                }
            }
            else
            {
                stop = true;
                team_only.IsChecked = false;
                team_all.IsChecked = false;
                team_no.IsChecked = false;
                team_not.IsChecked = false;
                team_null.IsChecked = false;
                (sender as RadioButton).IsChecked = true;
                if (team_not.IsChecked == true)
                {
                    team.IsEnabled = true;
                    addT.IsEnabled = true;
                    delT.IsEnabled = true;
                    T.IsEnabled = true;
                }
                else
                {
                    team.IsEnabled = false;
                    addT.IsEnabled = false;
                    delT.IsEnabled = false;
                    T.IsEnabled = false;
                }
                stop = false;
            }
        }

        private void DataControl(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            
            if (b == addM)
            {
                if (M.Items.Count > 2) return;
                string str = "";
                for (int i = 0; i < 4; i++)
                {
                    M.SelectedIndex = i;
                    str = "选择游戏模式不是" + ((ComboBoxItem)mode.SelectedItem).Content + "模式的玩家(m=!" + mode.SelectedIndex + ")";
                    if ((string)M.SelectedItem == str) return;
                }
                M.Items.Add("选择游戏模式不是" + ((ComboBoxItem)mode.SelectedItem).Content + "模式的玩家(m=!" + mode.SelectedIndex + ")");
            }
            if (b == delM) M.Items.Remove(M.SelectedItem);
            
            if (b == addN && name.Text != "")
            {
                string Name = name.Text;
                if (NAME.Count == 0) goto add;
                for (int i = 0; i < NAME.Count; i++)
                {
                    if (NAME[i] == Name) return;
                }
                add:
                N.Items.Add("选择名字不是  " + Name + "  的玩家");
                NAME.Add(Name);
                name.Text = "";
            }
            if (b == delN) { if (N.SelectedIndex == -1) return; NAME.RemoveAt(N.SelectedIndex); N.Items.Remove(N.SelectedItem); }

            if (b == addT && team.Text != "")
            {
                string Team = team.Text;
                if (TEAM.Count == 0) goto add;
                for (int i = 0; i < TEAM.Count; i++)
                {
                    if (TEAM[i] == Team) return;
                }
                add:
                T.Items.Add("选择不在队伍  " + Team + "  的玩家");
                TEAM.Add(Team);
                team.Text = "";
            }
            if (b == delT) { if (T.SelectedIndex == -1) return; TEAM.RemoveAt(T.SelectedIndex); T.Items.Remove(T.SelectedItem); }

            if (score_name.Text != "" && ((b == addSA && score_min.Value != null) || (b==addSI&&score_max.Value != null)))
            {
                string Board = score_name.Text;
                int score;string v = "", t = "";
                if (b == addSA) { score = (int)score_min.Value; t = "≤"; score_min.Value = null; } else { score = (int)score_max.Value; v = "_min"; t = "≥"; score_max.Value = null; }
                string cmd = "score_" + Board + v + "=" + score;

                if (SCOREB.Count == 0) goto add;
                for (int i = 0; i < SCOREB.Count; i++)
                {
                    if (SCOREB[i] == cmd) return;
                }
                add:
                S.Items.Add("选择计分板 "+ Board + " 分数"+ t + score + "  的玩家");
                SCOREB.Add(cmd);
            }
            if (b == delS) { if (S.SelectedIndex == -1) return; SCOREB.RemoveAt(S.SelectedIndex); S.Items.Remove(S.SelectedItem); }
        }
    }
}