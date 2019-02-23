namespace MinecraftToolsBoxSDK.Json.Advancements
{
    /// <summary>
    /// advancement奖励
    /// </summary>
    public class Reward
    {
        /// <summary>
        /// 配方列表（标签）。
        /// </summary>
        public string[] recipes { get; set; }
        /// <summary>
        /// 可抢夺物品列表（标签）。
        /// </summary>
        public string[] loot { get; set; }
        /// <summary>
        /// 经验值总数
        /// </summary>
        int experience { get; set; }
        /// <summary>
        /// 运行的函数。函数为.minecraft\saves\XXXX\data\functions\中的纯文本文件，包含运行的命令列表。
        /// </summary>
        public string function { get; set; }
    }
}
