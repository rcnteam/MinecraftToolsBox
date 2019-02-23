using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftToolsBoxSDK.Nbt
{
    public static class NbtSerializer
    {
        public static string Serialize(object obj)
        {
            if (obj == null) return "";
            string nbt = "";
            Type t = obj.GetType();
            foreach(FieldInfo info in t.GetFields())
            {
                string value = "";
                Type type = info.FieldType;
                if (type.IsArray)
                {
                    object fieldValue = info.GetValue(obj);
                    if (fieldValue == null) continue;
                    string val = "";
                    if (type == typeof(byte[]))
                    {
                        foreach (byte b in (byte[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(short[]))
                    {
                        foreach (short b in (short[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(int[]))
                    {
                        foreach (int b in (int[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(long[]))
                    {
                        foreach (long b in (long[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(float[]))
                    {
                        foreach (float b in (float[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(double[]))
                    {
                        foreach (double b in (double[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(string[]))
                    {
                        foreach (string b in (string[])fieldValue)
                            val += b + ",";
                    }
                    else if (type == typeof(bool[]))
                    {
                        foreach (bool b in (bool[])fieldValue)
                            val += b + ",";
                    }
                    else
                    {
                        foreach (object b in (object[])fieldValue)
                            val += Serialize(b) + ",";
                    }
                    if (val.Length != 0)
                    {
                        val = val.Substring(0, val.Length - 1);
                        if (val.Length != 0) value = "[" + val + "]";
                    }
                }
                else if (type == typeof(byte?) || type == typeof(short?) || type == typeof(int?) || type == typeof(long?) || type == typeof(float?) || type == typeof(double?) || type == typeof(string) || type == typeof(bool?))
                {
                    //if(object.Equals(info.GetValue(obj),))
                    object o = info.GetValue(obj);
                    if (o != null) value = o + "";
                }
                else
                {
                    value = Serialize(info.GetValue(obj));
                }
                if (value != "" && value != null)
                {
                    Attribute attribute = Attribute.GetCustomAttribute(info, typeof(NbtProperty));
                    if (attribute != null && (attribute as NbtProperty).Name != null) nbt += (attribute as NbtProperty).Name + ":" + value + ",";
                    else nbt += info.Name + ":" + value + ",";
                }
            }
            if (nbt.Length != 0) return "{" + nbt.Substring(0, nbt.Length - 1) + "}";
            else return "{}";
        }
    }
}
