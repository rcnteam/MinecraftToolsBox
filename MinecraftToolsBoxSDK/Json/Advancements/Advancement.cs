using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace MinecraftToolsBoxSDK.Json.Advancements
{
    public class Advancement
    {
        [JsonConverter(typeof(CriteriaConverter))]
        /// <summary>
        /// 必须达成的条件。
        /// </summary>
        public Criteria criteria { get; set; }
        /// <summary>
        /// 可选的显示数据。 
        /// </summary>
        public Display display { get; set; }
        /// <summary>
        /// 此进度的可选的父目录（不适用于根成就）。
        /// </summary>
        public string parent { get; set; }
        /// <summary>
        /// 可选，所有条件（<criteriaName>）列表。如果所有条件都需要，此项可以被忽略。有多个条件：要求包含了要求条件列表（所有条件都需要被提及）。任一列表中所有条件达成后，进度会完成。（基本上是“与”、“或”的条件分组）
        /// </summary>
        public string[][] requirements { get; set; }
        /// <summary>
        /// 可选，当进度达到时获得的所有奖励。
        /// </summary>
        public Reward rewards { get; set; }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }

    public class Criteria
    {
        public CriteriaType Trigger { get; set; }
        public Condition Conditions { get; set; }
        public string CriteriaName { get; set; }
    }
    public enum CriteriaType
    {
        /// <summary>
        /// 玩家繁殖两个动物
        /// </summary>
        BredAnimals,
        /// <summary>
        /// 玩家酿造一瓶药水
        /// </summary>
        BrewedPotion,
        /// <summary>
        /// 玩家旅行到一个维度/从一个维度出发。
        /// </summary>
        ChangedDimension,
        /// <summary>
        /// 玩家更改信标结构。（当信标自我更新时）
        /// </summary>
        ConstructBeacon,
        /// <summary>
        /// 玩家损耗了物品。
        /// </summary>
        ConsumeItem,
        /// <summary>
        /// 玩家治愈僵尸村民。
        /// </summary>
        CuredZombieVillager,
        /// <summary>
        /// 玩家获得/消除状态效果。
        /// </summary>
        EffectsChanged,
        /// <summary>
        /// 玩家通过附魔台附魔物品（使用铁砧或命令时不触发）。
        /// </summary>
        EnchantedItem,
        /// <summary>
        /// 玩家进入方块（比如传送门）。玩家需要站在与方块相同的空间里。每个游戏刻都检查，但如果进度被命令剥夺，其每个成功配对游戏刻都会触发。这意味着它每个游戏刻可以触发1至8次。
        /// </summary>
        EnterBlock,
        /// <summary>
        /// 实体伤害玩家。
        /// </summary>
        EntityHurtPlayer,
        /// <summary>
        /// 实体杀死玩家。
        /// </summary>
        EntityKilledPlayer,
        /// <summary>
        /// 仅可使用命令。
        /// </summary>
        Impossible,
        /// <summary>
        /// 玩家物品栏变化。
        /// </summary>
        InventoryChanged,
        /// <summary>
        /// 任何物品以任何形式损害。
        /// </summary>
        ItemDurabilityChanged,
        /// <summary>
        /// 玩家获得漂浮效果。
        /// </summary>
        Levitation,
        /// <summary>
        /// 每个游戏刻（每秒20次）检查玩家的位置。
        /// </summary>
        Location,
        /// <summary>
        /// 玩家进入下界，然后返回主世界。
        /// </summary>
        NetherTravel,
        /// <summary>
        /// 玩家放置方块。
        /// </summary>
        PlacedBlock,
        /// <summary>
        /// 玩家伤害实体（包括自己）。
        /// </summary>
        PlayerHurtEntity,
        /// <summary>
        /// 玩家杀死实体。
        /// </summary>
        PlayerKilledEntity,
        /// <summary>
        /// 玩家解锁配方（例如用知识之书）。
        /// </summary>
        RecipeUnlocked,
        /// <summary>
        /// 玩家上床睡觉。
        /// </summary>
        SleptInBed,
        /// <summary>
        /// 玩家召唤了生物，例如用灵魂沙和凋灵骷髅头颅召唤凋灵。同样适用于铁傀儡、雪傀儡和末影龙（使用末影水晶）。使用发射器来放置凋灵骷髅头颅或南瓜也能激活此触发器。刷怪蛋、命令和刷怪箱不会激活此触发器。
        /// </summary>
        SummonedEntity,
        /// <summary>
        /// 玩家驯服动物。
        /// </summary>
        TameAnimal,
        /// <summary>
        /// 每个游戏刻触发（每秒20次）。
        /// </summary>
        Tick,
        /// <summary>
        /// 玩家使用末影之眼（在有要塞生成的世界）。
        /// </summary>
        UsedEnderEye,
        /// <summary>
        /// 玩家使用图腾。
        /// </summary>
        UsedTotem,
        /// <summary>
        /// 玩家与村民交易。
        /// </summary>
        VillagerTrade
    }

    /// <summary>
    /// 转换Criteria类型
    /// </summary>
    public class CriteriaConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Criteria);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Criteria c = value as Criteria;
            if (value == null) return;
            writer.WriteStartObject();
            
            writer.WritePropertyName(c.CriteriaName);
            writer.WriteStartObject();

            writer.WritePropertyName("trigger");
            writer.WriteValue(EnumConverter(c.Trigger));

            writer.WritePropertyName("conditions");
            writer.WriteStartObject();
            string raw = JsonConvert.SerializeObject(c.Conditions, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            writer.WriteRaw(raw.Substring(1, raw.Length - 2));
            writer.WriteEndObject();

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        public static string EnumConverter(CriteriaType type)
        {
            switch (type)
            {
                default: return "minecraft:impossible";
                case CriteriaType.BredAnimals: return "minecraft:bred_animals";
                case CriteriaType.BrewedPotion: return "minecraft:brewed_potion";
                case CriteriaType.ChangedDimension: return "minecraft:changed_dimension";
                case CriteriaType.ConstructBeacon: return "minecraft:construct_beacon";
                case CriteriaType.ConsumeItem: return "minecraft:consume_item";
                case CriteriaType.CuredZombieVillager: return "minecraft:cured_zombie_villager";
                case CriteriaType.EffectsChanged: return "minecraft:effects_changed";
                case CriteriaType.EnchantedItem: return "minecraft:enchanted_item";
                case CriteriaType.EnterBlock: return "minecraft:enter_block";
                case CriteriaType.EntityHurtPlayer: return "minecraft:entity_hurt_player";
                case CriteriaType.EntityKilledPlayer: return "minecraft:entity_killed_player";
                case CriteriaType.InventoryChanged: return "minecraft:inventory_changed";
                case CriteriaType.ItemDurabilityChanged: return "minecraft:item_durability_changed";
                case CriteriaType.Levitation: return "minecraft:levitation";
                case CriteriaType.Location: return "minecraft:location";
                case CriteriaType.NetherTravel: return "minecraft:nether_travel";
                case CriteriaType.PlacedBlock: return "minecraft:placed_block";
                case CriteriaType.PlayerHurtEntity: return "minecraft:player_hurt_entity";
                case CriteriaType.PlayerKilledEntity: return "minecraft:player_killed_entity";
                case CriteriaType.RecipeUnlocked: return "minecraft:recipe_unlocked";
                case CriteriaType.SleptInBed: return "minecraft:slept_in_bed";
                case CriteriaType.SummonedEntity: return "minecraft:summoned_entity";
                case CriteriaType.TameAnimal: return "minecraft:tame_animal";
                case CriteriaType.Tick: return "minecraft:tick";
                case CriteriaType.UsedEnderEye: return "minecraft:used_ender_eye";
                case CriteriaType.UsedTotem: return "minecraft:used_totem";
                case CriteriaType.VillagerTrade: return "minecraft:villager_trade";
            }
        }
    }
}