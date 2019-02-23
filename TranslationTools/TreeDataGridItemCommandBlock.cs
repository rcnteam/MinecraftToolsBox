using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TranslationTools
{
    class TreeDataGridItemCommandBlock : TreeDataGridItem
    {
        public string[] pos;

        public TreeDataGridItemCommandBlock(XmlNode item, int index)
        {
            Node = item;
            Type = "命令" + index;
            Node = item;
        }
    }
}
