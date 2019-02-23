using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MinecraftToolsBoxSDK.Json.Advancements
{
    public class Display
    {
        /// <summary>
        /// 一个物品或方块ID，用于显示进度窗口。
        /// </summary>
        public Icon icon { get; set; }
        /// <summary>
        /// 进度的名称（string）
        /// 或JSON文本（包含文本，格式和/tellraw等命令中使用的类似）
        /// </summary>
        public string title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        /// <summary>
        /// 图标边框的可选种类。默认为task
        /// </summary>
        public FrameType frame { get; set; } = FrameType.task;
        /// <summary>
        /// 用于此进度标签的背景的可选目录（仅根进度）。
        /// </summary>
        public string background { get; set; }
        /// <summary>
        /// 进度的描述（string）
        /// 或JSON文本（包含文本，格式和/tellraw等命令中使用的类似）
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 是否在完成此进度后显示提示信息。默认为true。
        /// </summary>
        public bool show_toast { get; set; } = true;
        /// <summary>
        /// 是否在完成此进度后广播此信息。默认为true。
        /// </summary>
        public bool announce_to_chat { get; set; } = true;
        /// <summary>
        /// 是否在进度屏幕隐藏进度，对根进度无效。默认为false。
        /// </summary>
        public bool hidden { get; set; } = false;
    }
    public enum FrameType
    {
        /// <summary>
        /// 挑战进度。
        /// </summary>
        challenge,
        /// <summary>
        /// 目标进度。
        /// </summary>
        goal,
        /// <summary>
        /// 普通的进度。
        /// </summary>
        task
    }
    public class Icon
    {
        /// <summary>
        /// 物品的ID
        /// </summary>
        public string item { get; set; }
        /// <summary>
        /// 物品的损害值
        /// </summary>
        public int data { get; set; }
    }
}