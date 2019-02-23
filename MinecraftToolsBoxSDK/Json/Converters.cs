using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MinecraftToolsBoxSDK.Json
{

    /// <summary>
    /// 给枚举类型添加 minecraft: 前缀并处理大小写
    /// </summary>
    public class EnumNameToJsonFormateConverter : JsonConverter
    {
        public static readonly List<char> Chars = new List<char>(new char[36] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
        public override bool CanConvert(Type objectType) { return true; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(EnumNameToJsonFormate(value));
        }
        public static string EnumNameToJsonFormate(object value)
        {
            List<char> list = new List<char>(value.ToString());
            for (int i = 0; i < list.Count; i++)
            {
                char c = list[i];
                if (Chars.Contains(c))
                {
                    list[i] = char.ToLower(c);
                    list.Insert(i, '_'); i++;
                }
            }
            return "minecraft:" + new string(list.ToArray()).Substring(1);
        }
    }
    /// <summary>
    /// 给枚举类型处理大小写
    /// </summary>
    public class StringToJsonFormateConverter : JsonConverter
    {
        public static readonly List<char> Chars = new List<char>(new char[36] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
        public override bool CanConvert(Type objectType) { return true; }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(EnumNameToJsonFormate(value));
        }
        public static string EnumNameToJsonFormate(object value)
        {
            List<char> list = new List<char>(value.ToString());
            for (int i = 0; i < list.Count; i++)
            {
                char c = list[i];
                if (Chars.Contains(c))
                {
                    list[i] = char.ToLower(c);
                    list.Insert(i, '_'); i++;
                }
            }
            return new string(list.ToArray()).Substring(1);
        }
    }
    /// <summary>
    /// 转换方块状态数据
    /// </summary>
    public class BlockStateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, string>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (KeyValuePair<string, string> item in value as Dictionary<string, string>)
            {
                writer.WritePropertyName(item.Key);
                writer.WriteValue(item.Value);
            }
            writer.WriteEndObject();
        }
    }
}
