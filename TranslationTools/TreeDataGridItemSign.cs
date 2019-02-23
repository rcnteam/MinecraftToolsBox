using MinecraftToolsBoxSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationTools
{
    class TreeDataGridItemSign : TreeDataGridItem
    {
        public string[] pos;

        public TreeDataGridItemSign(XmlNode item, int index)
        {
            Type = "告示牌" + index;
            pos = item.Attributes["Pos"].Value.Split(';');
            Children.Add(new TreeDataGridItem { Type = "第一行", Node = item.ChildNodes[0] });
            Children.Add(new TreeDataGridItem { Type = "第二行", Node = item.ChildNodes[1] });
            Children.Add(new TreeDataGridItem { Type = "第三行", Node = item.ChildNodes[2] });
            Children.Add(new TreeDataGridItem { Type = "第四行", Node = item.ChildNodes[3] });
        }
    }
}
