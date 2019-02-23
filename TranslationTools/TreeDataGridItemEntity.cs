using MinecraftToolsBoxSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationTools
{
    class TreeDataGridItemEntity : TreeDataGridItem
    {
        public string id;
        public string[] uuid;

        public TreeDataGridItemEntity(XmlNode item, int index)
        {
            Type = "实体" + index;
            id = item.Attributes["Id"].Value;
            foreach (XmlNode node in item.ChildNodes)
            {
                if (node.Name == "CustomName") Children.Add(new TreeDataGridItem { Type = "名称", Node = node });
                else
                {
                    if (node.Name == "Item")
                    {
                        TreeDataGridItemItem i = new TreeDataGridItemItem { Type = node.Attributes["Slot"].Value };
                        foreach (XmlNode data in node.ChildNodes)
                            i.Children.Add(new TreeDataGridItem { Type = data.Name == "Name" ? "名字" : "说明", Node = data });
                        Children.Add(i);
                    }
                    else
                    {
                        TreeDataGridItem i2 = new TreeDataGridItem { Type = "书" };
                        foreach (XmlNode data in node.ChildNodes)
                            i2.Children.Add(new TreeDataGridItem { Type = data.Name == "Title" ? "标题" : "内容", Node = data });
                        Children.Add(i2);
                    }
                }
            }
        }
    }

    class TreeDataGridItemItem : TreeDataGridItem
    {
        public string slot;
    }
}
