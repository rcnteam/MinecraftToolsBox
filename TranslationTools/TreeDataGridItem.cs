using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;
using MinecraftToolsBoxSDK;

namespace TranslationTools
{
    public class TreeDataGridItem : TreeDataGridElement
    {
        private const string subItem = "M0,0 L0,29 M1,19 L12,19";
        private const string subItemLast = "M0,0 L0,23 M1,22 L12,22";

        public string Type { get; set; }
        public string Original { get; set; }
        public string Translated
        {
            get => _translated;
            set
            {
                _translated = value;
                if (Node != null && value != "") (Node as XmlElement).SetAttribute("Translated", value);
            }
        }
        private string _translated;

        public string Data { get; set; }
        public XmlNode Node
        {
            get => _node;
            set
            {
                if (value.Attributes["Original"] != null) Original = value.Attributes["Original"].Value;
                if (value.Attributes["Translated"] != null)Translated = value.Attributes["Translated"].Value;
                _node = value;
            }
        }
        private XmlNode _node;

        public TreeDataGridItem() { }

        public TreeDataGridItem(string type, string original, string translated)
        {
            Type = type;
            Original = original;
            Translated = translated;
        }
    }
}
