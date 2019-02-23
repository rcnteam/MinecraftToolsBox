using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationTools
{
    class TreeDataGridItemContainer : TreeDataGridItem
    {
        public string[] pos;
        public string id;

        public TreeDataGridItemContainer(XmlNode item, int index)
        {
            Type = "容器" + index;
            pos = item.Attributes["Pos"].Value.Split(';');
            id = item.Attributes["Id"].Value;

            foreach (XmlNode node in item.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Item":
                        TreeDataGridItemItem i = new TreeDataGridItemItem { Type = "物品" };
                        foreach (XmlNode data in node.ChildNodes)
                            i.Children.Add(new TreeDataGridItem { Type = data.Name == "Name" ? "名字" : "说明", Node = data });
                        Children.Add(i);
                        break;
                    case "Book":
                        TreeDataGridItemItem i2 = new TreeDataGridItemItem { Type = "书" };
                        foreach (XmlNode data in node.ChildNodes)
                            i2.Children.Add(new TreeDataGridItem { Type = data.Name == "Title" ? "标题" : "内容", Node = data });
                        Children.Add(i2);
                        break;
                    case "CustomName": Children.Add(new TreeDataGridItem { Type = "名称", Node = node }); break;
                    case "Lock": Children.Add(new TreeDataGridItem { Type = "密码", Node = node }); break;
                }
            }
        }
    }
}
