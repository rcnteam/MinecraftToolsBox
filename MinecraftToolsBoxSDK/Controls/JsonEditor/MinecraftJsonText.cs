using Newtonsoft.Json;
using MinecraftToolsBoxSDK;
using System.Windows;
using System;
using System.Windows.Media;
using MinecraftToolsBoxSDK.Json;

namespace MinecraftToolsBoxSDK
{
    public class MinecraftJsonText
    {
        public TextFormat Format;

        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("selector")]
        public string Selector { get; set; }
        [JsonProperty("score")]
        public Score Score { get; set; }

        [JsonProperty("bold")]
        public bool Bold { get; set; }
        [JsonProperty("italic")]
        public bool Italic { get; set; }
        [JsonProperty("underlined")]
        public bool Underlined { get; set; }
        [JsonProperty("strikthrough")]
        public bool Strikethrough { get; set; }
        [JsonProperty("obfuscated")]
        public bool Obfuscated { get; set; }
        [JsonConverter(typeof(JsonTextColorConverter))]
        [JsonProperty("color")]
        public SolidColorBrush Color { get; set; }

        [JsonProperty("extra")]
        public MinecraftJsonText[] Extra { get; set; }

        [JsonProperty("clickEvent")]
        public ClickEvent ClickEvent { get; set; }
        [JsonProperty("hoverEvent")]
        public HoverEvent HoverEvent { get; set; }

        public string GetJsonText()
        {
            JsonSerializerSettings jsetting = new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            return JsonConvert.SerializeObject(this, Formatting.None, jsetting);
        }
        public void ImportFontStyle(dynamic r, Brush DefaultFontColor)
        {
            Text = r.Text;
            if (r.FontWeight == FontWeights.Bold) Bold = true;
            if (r.FontStyle == FontStyles.Italic) Italic = true;
            if ((r.Background is SolidColorBrush) && r.Background == Brushes.Black) Obfuscated = true;
            if (r.Foreground != DefaultFontColor) Color = r.Foreground;
            foreach (TextDecoration dec in r.TextDecorations) if (dec == TextDecorations.Underline[0]) Underlined = true; else if (dec == TextDecorations.Strikethrough[0]) Strikethrough = true;
        }
    }

    internal class JsonTextColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Color c = (value as SolidColorBrush).Color;
            string val = c.R + "," + c.G + "," + c.B;
            switch (val)
            {
                case "255,255,255": val = "white"; break;
                case "0,0,170": val = "dark_blue"; break;
                case "0,170,0": val = "dark_green"; break;
                case "0,170,170": val = "dark_aqua"; break;
                case "170,0,0": val = "dark_red"; break;
                case "170,0,170": val = "dark_purple"; break;
                case "255,170,0": val = "gold"; break;
                case "170,170,170": val = "gray"; break;
                case "85,85,85": val = "dark_gray"; break;
                case "85,85,255": val = "blue"; break;
                case "85,255,85": val = "green"; break;
                case "85,255,255": val = "aqua"; break;
                case "255,85,85": val = "red"; break;
                case "255,85,255": val = "light_purple"; break;
                case "255,255,85": val = "yellow"; break;
                case "0,0,0": val = "black"; break;
            }
            writer.WriteValue(val);
        }
    }

    public class Score
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("objective")]
        public string Objective { get; set; }
    }

    public class ClickEvent
    {
        [JsonConverter(typeof(StringToJsonFormateConverter))]
        [JsonProperty("action")]
        public ClickEvents? Action { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }
    public class HoverEvent
    {
        [JsonConverter(typeof(StringToJsonFormateConverter))]
        [JsonProperty("action")]
        public HoverEvents? Action { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public enum TextFormat { Text, Selector, Scoreboard }
}
