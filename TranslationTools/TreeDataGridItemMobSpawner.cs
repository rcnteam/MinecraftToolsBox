using MinecraftToolsBoxSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationTools
{
    class TreeDataGridItemMobSpawner : TreeDataGridItem
    {
        public string id;
        public string[] pos;

        public TreeDataGridItemMobSpawner(XmlNode item, int index)
        {
            Type = "刷怪笼" + index;
            pos = item.Attributes["Pos"].Value.Split(';');
            id = item.Attributes["Id"].Value;
            foreach (XmlNode node in item.ChildNodes)
            {
                if (node.Name == "CustomName") Children.Add(new TreeDataGridItem { Type = "名称", Original = node.Attributes[0].Value, Translated = node.Attributes["Translated"] != null ? node.Attributes["Translated"].Value : "" });
                else
                {
                    if (node.Name == "Item")
                    {
                        TreeDataGridItemItem i = new TreeDataGridItemItem { Type = node.Attributes["Slot"].Value };
                        foreach (XmlNode data in node.ChildNodes)
                            i.Children.Add(new TreeDataGridItem { Type = data.Name == "Name" ? "名字" : "说明", Original = data.Attributes[0].Value, Translated = data.Attributes["Translated"] != null ? data.Attributes["Translated"].Value : "" });
                        Children.Add(i);
                    }
                    else
                    {
                        TreeDataGridItemItem i2 = new TreeDataGridItemItem { Type = "书" };
                        foreach (XmlNode data in node.ChildNodes)
                            i2.Children.Add(new TreeDataGridItem { Type = data.Name == "Title" ? "标题" : "内容", Original = data.Attributes[0].Value, Translated = data.Attributes["Translated"] != null ? data.Attributes["Translated"].Value : "" });
                        Children.Add(i2);
                    }
                }
            }
        }
    }
}
