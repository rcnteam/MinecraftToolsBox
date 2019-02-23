using Substrate.Nbt;
using System.Collections.Generic;
using System.Xml;

namespace MinecraftToolsBox.Tools
{
    public abstract class LanguageItem
    {
        public LangItemType Type { get; set; }
        public object Original { get; set; }
        public object Translated { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }

        public LanguageItem() { }
        public LanguageItem(TagNodeCompound tag, LangItemType type)
        {
            Type = type;
            if (type == LangItemType.CommandBlock)
            {
                Original = tag["Command"].ToTagString().Data;
            }
            else if (type == LangItemType.Sign)
            {
                Info = tag["x"].ToTagInt().Data + "," + tag["y"].ToTagInt().Data + "," + tag["z"].ToTagInt().Data;
                Original = new Sign { Line1 = tag["Text1"].ToTagString().Data, Line2 = tag["Text2"].ToTagString().Data, Line3 = tag["Text3"].ToTagString().Data, Line4 = tag["Text4"].ToTagString().Data };
            }
            else if (type == LangItemType.Container)
            {
                Name = tag["id"].ToTagString().Data;
                Info = tag["x"].ToTagInt().Data + "," + tag["y"].ToTagInt().Data + "," + tag["z"].ToTagInt().Data;
                Container c = new Container();
                if (tag.ContainsKey("CustomName"))
                {
                    string str = tag["CustomName"].ToTagString().Data;
                    if (Name == "Control" || Name == "minecraft:command_block")
                    {
                        if (str != "" && str != "@") c.CustomName = Name;
                    }
                    else if (str != "") c.CustomName = str;
                }
                if (tag.ContainsKey("Lock") && tag["Lock"].ToTagString().Data != "") c.Lock = tag["Lock"].ToTagString().Data;

                if (tag.Keys.Contains("RecordItem") && tag["RecordItem"].ToTagCompound().ContainsKey("tag"))
                {
                    TagNodeCompound tag1 = tag["RecordItem"].ToTagCompound()["tag"].ToTagCompound();
                    Item i = new Item(tag1);
                    if (i.Check()) c.Items.Add(i);
                }
                if (tag.ContainsKey("Items") && tag["Items"].IsCastableTo(TagType.TAG_LIST))
                    foreach (TagNodeCompound tags in tag["Items"].ToTagList())
                    {
                        if (tags["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(tags);
                            if (i.Check()) c.Items.Add(i);
                        }
                        else
                        {
                            Item i = new Item(tags);
                            if (i.Check()) c.Items.Add(i);
                        }
                    }
                Original = c;
            }
            else if (type == LangItemType.Entity)
            {
                Name = tag["id"].ToTagString().Data;
                Entity e = new Entity();
                if (tag.ContainsKey("CustomName")) e.CustomName = tag["CustomName"].ToTagString().Data;

                if (Name.ToLower().Contains("item"))
                {
                    if (tag.ContainsKey("Item"))
                    {
                        Item item = new Item(tag["Item"].ToTagCompound());
                        if (item.Check()) e.Equipments[0] = item;
                    }
                }
                else
                {
                    if (tag.ContainsKey("HandItems") || tag.ContainsKey("ArmorItems"))
                    {
                        TagNodeList HandItems = tag["HandItems"].ToTagList();
                        TagNodeList ArmorItems = tag["ArmorItems"].ToTagList();
                        try
                        {
                            if (HandItems[0].ToTagCompound().ContainsKey("id") && HandItems[0].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(HandItems[0].ToTagCompound());
                                if (i.Check()) e.Equipments[0] = i;
                            }
                            else
                            {
                                Item i = new Item(HandItems[0].ToTagCompound());
                                if (i.Check()) e.Equipments[0] = i;
                            }
                        }
                        catch { }
                        try
                        {
                            if (HandItems[1].ToTagCompound().ContainsKey("id") && HandItems[1].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(HandItems[1].ToTagCompound());
                                if (i.Check()) e.Equipments[1] = i;
                            }
                            else
                            {
                                Item i = new Item(HandItems[1].ToTagCompound());
                                if (i.Check()) e.Equipments[1] = i;
                            }
                        }
                        catch { }
                        try
                        {
                            if (ArmorItems[3].ToTagCompound().ContainsKey("id") && ArmorItems[3].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(ArmorItems[3].ToTagCompound());
                                if (i.Check()) e.Equipments[2] = i;
                            }
                            else
                            {
                                Item i = new Item(ArmorItems[3].ToTagCompound());
                                if (i.Check()) e.Equipments[2] = i;
                            }
                        }
                        catch { }
                        try
                        {
                            if (ArmorItems[2].ToTagCompound().ContainsKey("id") && ArmorItems[2].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(ArmorItems[2].ToTagCompound());
                                if (i.Check()) e.Equipments[3] = i;
                            }
                            else
                            {
                                Item i = new Item(ArmorItems[2].ToTagCompound());
                                if (i.Check()) e.Equipments[3] = i;
                            }
                        }
                        catch { }
                        try
                        {
                            if (ArmorItems[1].ToTagCompound().ContainsKey("id") && ArmorItems[1].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(ArmorItems[1].ToTagCompound());
                                if (i.Check()) e.Equipments[4] = i;
                            }
                            else
                            {
                                Item i = new Item(ArmorItems[1].ToTagCompound());
                                if (i.Check()) e.Equipments[4] = i;
                            }
                        }
                        catch { }
                        try
                        {
                            if (ArmorItems[0].ToTagCompound().ContainsKey("id") && ArmorItems[0].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                            {
                                Book i = new Book(ArmorItems[0].ToTagCompound());
                                if (i.Check()) e.Equipments[5] = i;
                            }
                            else
                            {
                                Item i = new Item(ArmorItems[0].ToTagCompound());
                                if (i.Check()) e.Equipments[5] = i;
                            }
                        }
                        catch { }
                    }
                    else if (tag.ContainsKey("Equipment"))
                    {
                        TagNodeList Equipment = tag["Equipment"].ToTagList();
                        if (Equipment[0].ToTagCompound().ContainsKey("id") && Equipment[0].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(Equipment[0].ToTagCompound());
                            if (i.Check()) e.Equipments[0] = i;
                        }
                        else
                        {
                            Item i = new Item(Equipment[0].ToTagCompound());
                            if (i.Check()) e.Equipments[0] = i;
                        }

                        if (Equipment[4].ToTagCompound().ContainsKey("id") && Equipment[4].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(Equipment[4].ToTagCompound());
                            if (i.Check()) e.Equipments[2] = i;
                        }
                        else
                        {
                            Item i = new Item(Equipment[4].ToTagCompound());
                            if (i.Check()) e.Equipments[2] = i;
                        }

                        if (Equipment[3].ToTagCompound().ContainsKey("id") && Equipment[3].ToTagCompound().ContainsKey("id") && Equipment[3].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(Equipment[3].ToTagCompound());
                            if (i.Check()) e.Equipments[3] = i;
                        }
                        else
                        {
                            Item i = new Item(Equipment[3].ToTagCompound());
                            if (i.Check()) e.Equipments[3] = i;
                        }

                        if (Equipment[2].ToTagCompound().ContainsKey("id") && Equipment[2].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(Equipment[2].ToTagCompound());
                            if (i.Check()) e.Equipments[4] = i;
                        }
                        else
                        {
                            Item i = new Item(Equipment[2].ToTagCompound());
                            if (i.Check()) e.Equipments[4] = i;
                        }

                        if (Equipment[1].ToTagCompound().ContainsKey("id") && Equipment[1].ToTagCompound()["id"].ToTagString().Data.Contains("written_book"))
                        {
                            Book i = new Book(Equipment[1].ToTagCompound());
                            if (i.Check()) e.Equipments[5] = i;
                        }
                        else
                        {
                            Item i = new Item(Equipment[1].ToTagCompound());
                            if (i.Check()) e.Equipments[5] = i;
                        }
                        Item hand = new Item(Equipment[0].ToTagCompound());
                        Item armor1 = new Item(Equipment[4].ToTagCompound());
                        Item armor2 = new Item(Equipment[3].ToTagCompound());
                        Item armor3 = new Item(Equipment[2].ToTagCompound());
                        Item armor4 = new Item(Equipment[1].ToTagCompound());
                        if (hand.Check()) e.Equipments[0] = hand;
                        if (armor1.Check()) e.Equipments[2] = armor1;
                        if (armor2.Check()) e.Equipments[3] = armor2;
                        if (armor3.Check()) e.Equipments[4] = armor3;
                        if (armor4.Check()) e.Equipments[5] = armor4;
                    }
                }
                Original = e;
            }
            else if (type == LangItemType.MobSpawner)
            {
                Info = tag["x"].ToTagInt().Data + "," + tag["y"].ToTagInt().Data + "," + tag["z"].ToTagInt().Data;
                Name = tag["SpawnData"].ToTagCompound()["id"].ToTagString().Data;
                Original = new LanguageItem(tag["SpawnData"].ToTagCompound(), LangItemType.Entity).Original;
            }
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            if (Type == LangItemType.CommandBlock)
            {
                XmlElement element = doc.CreateElement("CommandBlock");
                element.SetAttribute("Original", (string)Original);
                return element;
            }
            else if (Type == LangItemType.Sign)
            {
                Sign s = Original as Sign;
                XmlElement element = doc.CreateElement("Sign");
                XmlElement l1 = doc.CreateElement("Line1"); l1.SetAttribute("Original", s.Line1); element.AppendChild(l1);
                XmlElement l2 = doc.CreateElement("Line2"); l2.SetAttribute("Original", s.Line2); element.AppendChild(l2);
                XmlElement l3 = doc.CreateElement("Line3"); l3.SetAttribute("Original", s.Line3); element.AppendChild(l3);
                XmlElement l4 = doc.CreateElement("Line4"); l4.SetAttribute("Original", s.Line4); element.AppendChild(l4);
                element.SetAttribute("Pos", Info);
                return element;
            }
            else if(Type == LangItemType.Container)
            {
                XmlElement element = doc.CreateElement("Container");
                Container c = Original as Container;
                if (c.CustomName != "")
                {
                    XmlElement CustomName = doc.CreateElement("CustomName");
                    CustomName.SetAttribute("Original", c.CustomName);
                    element.AppendChild(CustomName);
                }
                if (c.Lock != "")
                {
                    XmlElement Lock = doc.CreateElement("Lock");
                    Lock.SetAttribute("Original", c.Lock);
                    element.AppendChild(Lock);
                }
                foreach (Item item in c.Items) element.AppendChild(item.GetXmlElement(doc));

                element.SetAttribute("Id", Name);
                element.SetAttribute("Pos", Info);
                return element;
            }
            else if(Type == LangItemType.MobSpawner)
            {
                XmlElement element = doc.CreateElement("MobSpawner");
                XmlElement element2 = (Original as Entity).GetXmlElement(doc);
                element2.SetAttribute("Id", Name);
                element.SetAttribute("Pos", Info);
                element.AppendChild(element2);
                return element;
            }
            else
            {
                XmlElement element = (Original as Entity).GetXmlElement(doc);
                element.SetAttribute("Id", Name);
                return element;
            }
        }

        public bool Check(List<LanguageItem> list)
        {
            switch (Type)
            {
                default:return false;
                case LangItemType.Sign:
                    Sign s = Original as Sign;
                    if ((s.Line1 == "\"\"" || s.Line1 == "{\"text\":\"\"}" || s.Line1 == "null" || s.Line1.Replace(" ", "") == "{\"extra\":[\"\"],\"text\":\"\"}") && (s.Line2 == "\"\"" || s.Line2 == "{\"text\":\"\"}" || s.Line2 == "null" || s.Line2.Replace(" ", "") == "{\"extra\":[\"\"],\"text\":\"\"}") && (s.Line3 == "\"\"" || s.Line3 == "{\"text\":\"\"}" || s.Line3 == "null" || s.Line3.Replace(" ", "") == "{\"extra\":[\"\"],\"text\":\"\"}") && (s.Line4 == "\"\"" || s.Line4 == "{\"text\":\"\"}" || s.Line4 == "null" || s.Line4.Replace(" ", "") == "{\"extra\":[\"\"],\"text\":\"\"}")) return false;
                    foreach (LanguageItem item in list)
                        if (item.Type == LangItemType.Sign)
                        {
                            Sign si = item.Original as Sign;
                            if (si.Line1 == s.Line1 && si.Line2 == s.Line2 && si.Line3 == s.Line3 && si.Line4 == s.Line4)
                            {
                                item.Info += ";" + Info;
                                return false;
                            }
                        }
                    return true;
                case LangItemType.Container:
                    Container c = Original as Container;
                    if (c.CustomName != "" || c.Lock != "" || c.Items.Count != 0) return true; else return false;
                case LangItemType.Entity:
                    Entity e = Original as Entity;
                    if (e.CustomName != "" || e.Equipments[0] != null || e.Equipments[1] != null || e.Equipments[2] != null || e.Equipments[3] != null || e.Equipments[4] != null || e.Equipments[5] != null)
                    {
                        foreach (LanguageItem item in list)
                            if (item.Type == LangItemType.Entity)
                                if (e.CompareWith(item.Original as Entity)) return false;
                        return true;
                    }
                    else return false;
                case LangItemType.MobSpawner:
                    Entity e1 = Original as Entity;
                    if (e1.CustomName != "" || e1.Equipments[0] != null || e1.Equipments[1] != null || e1.Equipments[2] != null || e1.Equipments[3] != null || e1.Equipments[4] != null || e1.Equipments[5] != null)
                        foreach (LanguageItem i in list)
                        {
                            if (i.Type != LangItemType.MobSpawner) continue;
                            if (e1.CompareWith(i.Original as Entity))
                            {
                                i.Info += ";" + Info;
                                i.Name += ";" + Name;
                                return false;
                            }
                        }
                    else return false;
                    return true;
            }
        }
    }

    public class LanguageItemCommandBlock : LanguageItem
    {
        public LanguageItemCommandBlock(TagNodeCompound tag, List<LanguageItemCommandBlock> list, out bool check)
        {
            check = true;
            Original = tag["Command"].ToTagString().Data;

            foreach (LanguageItem item in list)
            {
                if (item.Original as string == Original as string)
                {
                    check = false;
                    return;
                }
            }

            string[] split = (Original as string).Split(' ');
            string cmd = split[0].Replace("/", "").ToLower();
            if (cmd == "me" || cmd == "title" || cmd == "tellraw" || cmd == "say")
            {
                check = false;
                return;
            }
            else if (cmd == "execute")
            {
                if (split.Length >= 12)
                {
                    if (split[11] == "me" || split[11] == "title" || split[11] == "tellraw" || split[11] == "say") return;
                }
                else if (split.Length >= 6)
                {
                    if (split[5] == "me" || split[5] == "title" || split[5] == "tellraw" || split[5] == "say") return;
                }
            }
            //nested 'execute' command
        }

    }

    public enum LangItemType { CommandBlock, Sign, Container, Entity, MobSpawner }
    public class Sign
    {
        public string Line1 { get; set; } = "";
        public string Line2 { get; set; } = "";
        public string Line3 { get; set; } = "";
        public string Line4 { get; set; } = "";

        public string getText(int line)
        {
            switch (line)
            {
                default: return null;
                case 0: return Line1;
                case 1: return Line2;
                case 2: return Line3;
                case 3: return Line4;
            }
        }
    }
    public class Container
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public string CustomName { get; set; } = "";
        public string Lock { get; set; } = "";
    }
    public class Item
    {
        public string Name { get; set; } = "";
        public string Lore { get; set; } = "";

        public Item() { }
        public Item(TagNodeCompound item)
        {
            if (!item.ContainsKey("tag")) return;
            TagNodeCompound tag = item["tag"].ToTagCompound();
            if (!tag.ContainsKey("display")) return;
            TagNodeCompound display = tag["display"].ToTagCompound();
            if (display.Keys.Contains("Name")) Name = display["Name"].ToTagString().Data;
            if (display.Keys.Contains("Lore"))
            {
                TagNodeList l = display["Lore"].ToTagList();
                for (int k = 0; k < l.Count; k++)
                {
                    Lore += l[k].ToTagString().Data.Replace("Line" + (k + 1) + ":", "") + "\n";
                }
                if (Lore.Length > 0) Lore = Lore.Substring(0, Lore.Length - 1);
            }
        }
        public bool Check()
        {
            if (Name != "" || Lore != "") return true;
            return false;
        }
        public bool CompareWith(Item i)
        {
            return Name == i.Name && Lore == i.Lore;
        }

        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement(GetType().Name);
            if (Name != "")
            {
                XmlElement name = doc.CreateElement("Name");
                element.AppendChild(name);
                name.SetAttribute("Original", Name);
            }
            if (Lore != "")
            {
                XmlElement lore = doc.CreateElement("Lore");
                lore.SetAttribute("Original", Lore);
                element.AppendChild(lore);
            }
            if (this is Book)
            {
                Book b = this as Book;
                if (b.Title != "")
                {
                    XmlElement title = doc.CreateElement("Title");
                    title.SetAttribute("Original", b.Title);
                    element.AppendChild(title);
                }
                if (b.Pages != "")
                {
                    XmlElement pages = doc.CreateElement("Pages");
                    pages.SetAttribute("Original", b.Pages);
                    element.AppendChild(pages);
                }
            }
            return element;
        }
    }
    public class Book : Item
    {
        public string Title { get; set; } = "";
        public string Pages { get; set; } = "";

        public Book() { }
        public Book(TagNodeCompound item)
        {
            if (!item.ContainsKey("tag")) return;
            TagNodeCompound tag = item["tag"].ToTagCompound();
            if (!tag.ContainsKey("display")) goto Next;
            TagNodeCompound display = tag["display"].ToTagCompound();
            if (display.Keys.Contains("Lore"))
            {
                TagNodeList l = display["Lore"].ToTagList();
                for (int k = 0; k < l.Count; k++)
                {
                    Lore += l[k].ToTagString().Data.Replace("Line" + (k + 1) + ":", "") + "\n";
                }
                if (Lore.Length > 0) Lore = Lore.Substring(0, Lore.Length - 1);
            }
            Next:;
            if (tag.ContainsKey("title")) Title = tag["title"].ToTagString().Data;
            if (tag.ContainsKey("pages")) foreach (TagNode node in tag["pages"].ToTagList()) Pages += node.ToTagString().Data + "\n";
            if (Pages.Length != 0) Pages = Pages.Substring(0, Pages.Length - 1);
        }
        public new bool Check()
        {
            if (Title != "" || Pages != "" || Lore != "") return true;
            return false;
        }
        public bool CompareWith(Book i)
        {
            return Title == i.Title && Pages == i.Pages && Lore==i.Lore;
        }
    }
    public class Entity
    {
        public string CustomName { get; set; } = "";
        public Item[] Equipments { get; set; } = new Item[6];

        public bool CompareWith(Entity e)
        {
            bool result = true;
            if (CustomName != e.CustomName) return false;
            if (Equipments[0] == null || e.Equipments[0] == null) return Equipments[0] == null && e.Equipments[0] == null; else if (!Equipments[0].CompareWith(e.Equipments[0])) result = false;
            if (Equipments[1] == null || e.Equipments[1] == null) return Equipments[1] == null && e.Equipments[1] == null; else if (!Equipments[1].CompareWith(e.Equipments[1])) result = false;
            if (Equipments[2] == null || e.Equipments[2] == null) return Equipments[2] == null && e.Equipments[2] == null; else if (!Equipments[2].CompareWith(e.Equipments[2])) result = false;
            if (Equipments[3] == null || e.Equipments[3] == null) return Equipments[3] == null && e.Equipments[3] == null; else if (!Equipments[3].CompareWith(e.Equipments[3])) result = false;
            if (Equipments[4] == null || e.Equipments[4] == null) return Equipments[4] == null && e.Equipments[4] == null; else if (!Equipments[4].CompareWith(e.Equipments[4])) result = false;
            if (Equipments[5] == null || e.Equipments[5] == null) return Equipments[5] == null && e.Equipments[5] == null; else if (!Equipments[5].CompareWith(e.Equipments[5])) result = false;
            return result;
        }
        public XmlElement GetXmlElement(XmlDocument doc)
        {
            XmlElement element = doc.CreateElement("Entity");
            if (CustomName != "")
            {
                XmlElement customName = doc.CreateElement("CustomName");
                customName.SetAttribute("Original", CustomName);
                element.AppendChild(customName);
            }
            string[] name1 = new string[6] { "主手", "副手", "头盔", "胸甲", "护腿", "靴子" };
            for (int k = 0; k < 6; k++)
            {
                Item Item = Equipments[k]; if (Item == null) continue;
                XmlElement item = Item.GetXmlElement(doc);
                item.SetAttribute("Slot", name1[k]);
                element.AppendChild(item);
            }
            return element;
        }
    }

    public static class LangTranslator
    {
        public static bool CommandBlockTranslator(TagNodeCompound Target, List<LanguageItem> Source)
        {
            string cmd = Target["Command"].ToTagString().Data;
            string c = cmd.Split(' ')[0].Replace("/", "");
            if (c == "me" || c == "title" || c == "tellraw" || c == "say")
            {
                LanguageItem languageItem = null;
                foreach (LanguageItem item in Source) if (cmd == item.Original as string) { languageItem = item; break; }
                if (languageItem != null && languageItem.Translated as string != "")
                {
                    Target["Command"] = new TagNodeString(languageItem.Translated as string);
                    return true;
                }
            }
            return false;
        }
        public static bool SignTranslator(TagNodeCompound Target, List<LanguageItem> Source)
        {
            string data1 = Target["Text1"].ToTagString().Data;
            string data2 = Target["Text2"].ToTagString().Data;
            string data3 = Target["Text3"].ToTagString().Data;
            string data4 = Target["Text4"].ToTagString().Data;

            Sign sign = null;
            foreach (LanguageItem item in Source)
            {
                Sign ori = item.Original as Sign;
                if (ori.Line1 == data1 && ori.Line2 == data2 && ori.Line3 == data3 && ori.Line4 == data4)
                {
                    sign = item.Translated as Sign;
                    break;
                }
            }
            if (sign != null)
            {
                if (sign.Line1 != "" || sign.Line2 != "" || sign.Line3 != "" || sign.Line4 != "")
                {
                    if (sign.Line1 == "") sign.Line1 = "{\"text\":\"\"}";
                    if (sign.Line2 == "") sign.Line2 = "{\"text\":\"\"}";
                    if (sign.Line3 == "") sign.Line3 = "{\"text\":\"\"}";
                    if (sign.Line4 == "") sign.Line4 = "{\"text\":\"\"}";
                    Target["Text1"] = new TagNodeString(sign.Line1);
                    Target["Text2"] = new TagNodeString(sign.Line2);
                    Target["Text3"] = new TagNodeString(sign.Line3);
                    Target["Text4"] = new TagNodeString(sign.Line4);
                    return true;
                }
            }
            return false;
        }
        public static bool ItemTranslator(TagNodeCompound Target, Item O, Item T)
        {
            if (!Target.ContainsKey("tag") || O == null || T == null) return false;
            TagNodeCompound tag = Target["tag"].ToTagCompound();

            string name = "", lore = "";
            bool t = false;
            if (O is Book && T is Book)
            {
                Book bo = O as Book, bt = T as Book;
                string title = "";string[] pages;
                lore = T.Lore == "" ? O.Lore : T.Lore;
                title = bt.Title == "" ? bo.Title : bt.Title;
                pages = (bt.Pages == "" ? bo.Pages : bt.Pages).Split('\n');
                if (tag.ContainsKey("display"))
                {
                    TagNodeCompound display = tag["display"].ToTagCompound();
                    if (display.ContainsKey("Lore"))
                    {
                        if (!CheckLore(display, O)) goto Next;
                        List<TagNode> nodes = new List<TagNode>();
                        foreach (string str in lore.Split('\n')) nodes.Add(new TagNodeString(str.Replace("\r", "")));
                        display["Lore"] = new TagNodeList(TagType.TAG_STRING, nodes);
                        t = true;
                    }
                }
                Next:
                if (tag.ContainsKey("title") && tag["title"].ToTagString().Data == bo.Title) { tag["title"] = new TagNodeString(bt.Title); t = true; }
                if (tag.ContainsKey("pages"))
                {
                    TagNodeList Pages = tag["pages"].ToTagList();
                    string pagestr = "";
                    foreach (TagNodeString s in Pages) pagestr += "\n" + s.Data;
                    if (pagestr.Length != 0) pagestr = pagestr.Substring(1);
                    if (pagestr == bo.Pages)
                    {
                        List<TagNode> result = new List<TagNode>();
                        foreach (string s in pages) result.Add(new TagNodeString(s));
                        tag["pages"] = new TagNodeList(TagType.TAG_STRING, result);
                        t = true;
                    }
                }
            }
            else
            {
                if (!tag.ContainsKey("display")) return false;
                TagNodeCompound compound = tag["display"].ToTagCompound();
                if (T.Name == "") name = O.Name; else name = T.Name;
                if (T.Lore == "") lore = O.Lore; else lore = T.Lore;
                if (compound.ContainsKey("Name") || compound.ContainsKey("Lore"))
                {
                    if (compound.ContainsKey("Lore") && !CheckLore(compound, O)) return false;
                    if (compound.ContainsKey("Name")) { if (compound["Name"].ToTagString().Data == O.Name) { compound["Name"] = new TagNodeString(name); t = true; } else return false; }
                    if (compound.ContainsKey("Lore"))
                    {
                        if (CheckLore(compound, O))
                        {
                            List<TagNode> nodes = new List<TagNode>();
                            foreach (string str in lore.Split('\n')) nodes.Add(new TagNodeString(str.Replace("\r", "")));
                            compound["Lore"] = new TagNodeList(TagType.TAG_STRING, nodes);
                            t = true;
                        }
                        else return false;
                    }
                    t = true;
                }
            }
            return t;
        }
        public static bool ContainerTranslator(TagNodeCompound Target, List<LanguageItem> Source)
        {
            LanguageItem target = null;bool flag = false;
            foreach (LanguageItem c in Source)
                if (c.Info == Target["x"].ToTagInt().Data +","+ Target["y"].ToTagInt().Data + "," + Target["z"].ToTagInt().Data) { target = c; break; }
            if (target == null) return false;
            Container o = target.Original as Container;
            Container t = target.Translated as Container;

            if (Target.ContainsKey("RecordItem")) return ItemTranslator(Target["RecordItem"].ToTagCompound(), o.Items[0], t.Items[0]);
            else
            {
                TagNodeList Items = Target["Items"].ToTagList();
                foreach(TagNode item in Items)
                {
                    for(int i = 0; i < o.Items.Count; i++)
                    {
                        if (ItemTranslator(item.ToTagCompound(), o.Items[i], t.Items[i])) { flag = true; goto Next; }
                    }
                }
                Next:
                if (Target.ContainsKey("CustomName")&&t.CustomName != "") { Target["CustomName"] = new TagNodeString(t.CustomName); flag = true; }
                if (Target.ContainsKey("Lock") && t.Lock != "") { Target["Lock"] = new TagNodeString(t.Lock); flag = true; }
            }
            return flag;
        }
        public static bool EntityTranslator(TagNodeCompound Target, List<LanguageItem> Source)
        {
            bool result = false;
            if (Target.ContainsKey("CustomName"))
            {
                string name = Target["CustomName"].ToTagString().Data;
                foreach (LanguageItem item in Source)
                {
                    if ((item.Original as Entity).CustomName == name && name != "" && name != null)
                    {
                        Entity tra = item.Translated as Entity;
                        if (tra.CustomName == "") break;
                        Target["CustomName"] = new TagNodeString(tra.CustomName);
                        result = true;
                    }
                }
            }
            if (Target["id"].ToTagString().Data.ToLower().Contains("item"))
            {
                foreach (LanguageItem e in Source)
                    for (int i = 0; i < 6; i++)
                        if (Target.ContainsKey("Item") && ItemTranslator(Target["Item"].ToTagCompound(), (e.Original as Entity).Equipments[i], (e.Translated as Entity).Equipments[i]))
                        {
                            result = true;
                            goto Next;
                        }
                Next:;
            }
            else if (Target.ContainsKey("ArmorItems") || Target.ContainsKey("HandItems"))
            {
                TagNodeList list = Target["ArmorItems"].ToTagList();
                bool b1 = false, b2 = false, b3 = false, b4 = false;
                foreach (LanguageItem e in Source)
                {
                    if (list.Count > 0 && ItemTranslator(list[0].ToTagCompound(), (e.Original as Entity).Equipments[5], (e.Translated as Entity).Equipments[0])) { b1 = true; }
                    if (list.Count > 1 && ItemTranslator(list[1].ToTagCompound(), (e.Original as Entity).Equipments[4], (e.Translated as Entity).Equipments[1])) { b2 = true; }
                    if (list.Count > 2 && ItemTranslator(list[2].ToTagCompound(), (e.Original as Entity).Equipments[3], (e.Translated as Entity).Equipments[2])) { b3 = true; }
                    if (list.Count > 3 && ItemTranslator(list[3].ToTagCompound(), (e.Original as Entity).Equipments[2], (e.Translated as Entity).Equipments[3])) { b4 = true; }
                    if (b1 && b2 && b3 && b4) goto Next;
                }
                Next:
                TagNodeList list2 = Target["HandItems"].ToTagList();
                bool b5 = false, b6 = false;
                foreach (LanguageItem e in Source)
                {
                    if (list2.Count > 0 && ItemTranslator(list2[0].ToTagCompound(), (e.Original as Entity).Equipments[0], (e.Translated as Entity).Equipments[0])) { b5 = true; }
                    if (list2.Count > 1 && ItemTranslator(list2[1].ToTagCompound(), (e.Original as Entity).Equipments[1], (e.Translated as Entity).Equipments[1])) { b6 = true; }
                    if (b5 && b6) goto End;
                }
                End: result = result ? true : b1 || b2 || b3 || b4 || b5 || b6;
            }
            else if (Target.ContainsKey("Equipment"))
            {
                TagNodeList list = Target["Equipment"].ToTagList();
                bool b1 = false, b2 = false, b3 = false, b4 = false, b5 = false;
                foreach (LanguageItem e in Source)
                {
                    if (list.Count > 0 && ItemTranslator(list[0].ToTagCompound(), (e.Original as Entity).Equipments[0], (e.Translated as Entity).Equipments[0])) { b1 = true; }
                    if (list.Count > 1 && ItemTranslator(list[1].ToTagCompound(), (e.Original as Entity).Equipments[2], (e.Translated as Entity).Equipments[1])) { b2 = true; }
                    if (list.Count > 2 && ItemTranslator(list[2].ToTagCompound(), (e.Original as Entity).Equipments[3], (e.Translated as Entity).Equipments[1])) { b3 = true; }
                    if (list.Count > 3 && ItemTranslator(list[3].ToTagCompound(), (e.Original as Entity).Equipments[4], (e.Translated as Entity).Equipments[1])) { b4 = true; }
                    if (list.Count > 4 && ItemTranslator(list[4].ToTagCompound(), (e.Original as Entity).Equipments[5], (e.Translated as Entity).Equipments[1])) { b5 = true; }
                    if (b1 && b2 && b3 && b4 && b5) goto End;
                }
                End: result = result ? true : b1 || b2 || b3 || b4 || b5;
            }
            return result;
        }
        public static bool CheckLore(TagNodeCompound compound, Item O)
        {
            TagNodeList list = compound["Lore"].ToTagList();
            string[] split = O.Lore.Split('\n');
            for (int i = 0; i < list.Count && i < split.Length; i++)
            {
                if (list[i].ToTagString().Data != split[i].Replace("\r", "")) return false;
            }
            return true;
        }
    }
}