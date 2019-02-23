using Newtonsoft.Json;
using MinecraftToolsBoxSDK;
using System.Collections.Generic;
using System;

namespace MinecraftToolsBoxSDK.Json.Advancements
{
    /// <summary>
    /// 所有条件的抽象基类
    /// </summary>
    public abstract class Condition { }

    public class BredAnimalsConditions : Condition
    {
        [JsonProperty("parent")]
        /// <summary>
        /// 父或母
        /// </summary>
        public EntityData Parent { get; set; }

        [JsonProperty("partner")]
        /// <summary>
        /// 配偶（实体父母的繁殖对象，可用于检查繁殖的骡） 
        /// </summary>
        public ExtendedEntityData Partner { get; set; }

        [JsonProperty("child")]
        /// <summary>
        /// 孩子
        /// </summary>
        public ExtendedEntityData Child { get; set; }
    }
    public class BrewedPotionConditions : Condition
    {
        [JsonConverter(typeof(StringToJsonFormateConverter))]
        [JsonProperty("potion")]
        /// <summary>
        /// 药水的ID
        /// </summary>
        public PotionID Potion { get; set; }
    }
    public class ChangedDimensionConditions : Condition
    {
        [JsonProperty("from")]
        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        /// <summary>
        /// 实体出发的维度
        /// </summary>
        public DimensionName From { get; set; }
        [JsonProperty("to")]
        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        /// <summary>
        /// 实体前往的维度
        /// </summary>
        public DimensionName To { get; set; }
    }
    public class ConstructBeaconConditions : Condition
    {
        [JsonProperty("level")]
        /// <summary>
        /// 信标层级。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Level { get; set; }
    }
    public class ConsumeItemConditions : Condition
    {
        [JsonProperty("item")]
        /// <summary>
        /// 消耗物品的数据
        /// </summary>
        public ItemData Item { get; set; }
    }
    public class CuredZombieVillagerConditions : Condition
    {
        [JsonProperty("zombie")]
        /// <summary>
        /// 储存僵尸变为村民的对话信息。 
        /// </summary>
        public EntityData Zombie { get; set; }
        [JsonProperty("villager")]
        /// <summary>
        /// 储存僵尸变为村民的对话信息。 
        /// </summary>
        public EntityData Villager { get; set; }
    }
    public class EffectsChangedConditions : Condition
    {
        [JsonProperty("effects")]
        [JsonConverter(typeof(EffectNameConverter))]
        /// <summary>
        /// 状态效果列表。 
        /// </summary>
        public Effect[] Effects { get; set; }
    }
    public class EnchantedItemConditions : Condition
    {
        [JsonProperty("item")]
        /// <summary>
        /// 附魔的物品的数据
        /// </summary>
        public ItemData Item { get; set; }

        [JsonProperty("levels")]
        /// <summary>
        /// 附魔花费的经验等级。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Levels { get; set; }
    }
    public class EnterBlockConditions : Condition
    {
        [JsonProperty("block")]
        /// <summary>
        /// 进入的方块的ID
        /// </summary>
        public string Block { get; set; }

        [JsonConverter(typeof(BlockStateConverter))]
        [JsonProperty("state")]
        public Dictionary<string, string> State { get; set; }
    }
    public class EntityHurtPlayerConditions : Condition
    {
        [JsonProperty("damage")]
        /// <summary>
        /// 实体受到伤害的数据
        /// </summary>
        public Damage Damage { get; set; }
    }
    public class EntityKilledPlayerConditions : Condition
    {
        [JsonProperty("entity")]
        /// <summary>
        /// 杀死玩家的实体数据
        /// </summary>
        public ExtendedEntityData Entity { get; set; }
    }
    public class InventoryChangedConditions
    {
        [JsonProperty("items")]
        /// <summary>
        /// 物品列表。 
        /// </summary>
        public ItemData[] Items { get; set; }

        [JsonProperty("slots")]
        /// <summary>
        /// 物品栏槽位数据
        /// </summary>
        public Slots Slots { get; set; }
    }
    public class ItemDuribilityChangedConditions : Condition
    {
        [JsonProperty("item")]
        /// <summary>
        /// 发生变化的物品
        /// </summary>
        public ItemData Item { get; set; }

        [JsonProperty("durability")]
        /// <summary>
        /// 物品的耐久度
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Durability { get; set; }

        [JsonProperty("delta")]
        /// <summary>
        /// 变化量
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Delta { get; set; }
    }
    public class LevitationConditions : Condition
    {
        [JsonProperty("duration")]
        /// <summary>
        /// 效果持续时间，以Tick为单位
        /// 设置为整数（int）表示确定的数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Duration { get; set; }
        [JsonProperty("distance")]
        /// <summary>
        /// 可以使用以下方式表示实体漂浮的距离参数
        /// 一个整数（int）或 实例化IntegerRange表示范围 或 实例化HorizontalDistance表示水平距离 或 实例化Distance表示3D空间中的距离参数
        /// </summary>
        public object Distance { get; set; }
    }
    public class LocationConditions : Condition
    {
        [JsonProperty("position")]
        /// <summary>
        /// 实体的坐标信息
        /// </summary>
        public Position Position { get; set; }

        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        [JsonProperty("biome")]
        /// <summary>
        /// 实体所处的生物群系
        /// </summary>
        public BiomeName Biome { get; set; }

        [JsonProperty("feature")]
        /// <summary>
        /// 实体所处的结构（structure）名称
        /// </summary>
        public string Feature { get; set; }
    }
    public class NetherTravelConditions : Condition
    {
        [JsonProperty("distance")]
        /// <summary>
        /// 穿越的距离
        /// </summary>
        public Distance Distance { get; set; }
    }
    public class PlacedBlockConditions : Condition
    {
        [JsonProperty("location")]
        /// <summary>
        /// 放置方块的玩家的位置信息
        /// </summary>
        public Location Location { get; set; }

        [JsonProperty("block")]
        /// <summary>
        /// 进入的方块的ID
        /// </summary>
        public string Block { get; set; }

        [JsonConverter(typeof(BlockStateConverter))]
        [JsonProperty("state")]
        public Dictionary<string, string> State { get; set; }
    }
    public class PlayerHurtEntityConditions : Condition
    {
        [JsonProperty("damage")]
        /// <summary>
        /// 实体受到伤害的数据
        /// </summary>
        public Damage Damage { get; set; }
    }
    public class PlayerKilledEntityConditions : Condition
    {
        [JsonProperty("entity")]
        /// <summary>
        /// 杀死实体的玩家数据
        /// </summary>
        public ExtendedEntityData Entity { get; set; }
    }
    public class RecipeUnlockedConditions : Condition
    {
        [JsonProperty("recipe")]
        /// <summary>
        /// 解锁的配方名称
        /// </summary>
        public string Recipe { get; set; }
    }
    public class SleptInBedConditions : Condition
    {
        [JsonProperty("location")]
        /// <summary>
        /// 玩家的位置信息
        /// </summary>
        public Location Location { get; set; }
    }
    public class SummonedEntityConditions : Condition
    {
        [JsonProperty("entity")]
        /// <summary>
        /// 生成的实体数据
        /// </summary>
        public ExtendedEntityData Entity { get; set; }
    }
    public class TameAnimalConditions : Condition
    {
        [JsonProperty("entity")]
        /// <summary>
        /// 驯服动物的数据
        /// </summary>
        public ExtendedEntityData Entity { get; set; }
    }//待确认
    public class UsedEnderEyeConditions : Condition
    {
        [JsonProperty("distance")]
        /// <summary>
        /// 使用末影之眼到消失的过程中玩家移动的距离
        /// </summary>
        public Distance Distance { get; set; }
    }
    public class UsedTotemConditions : Condition
    {
        [JsonProperty("item")]
        /// <summary>
        /// 图腾的数据
        /// </summary>
        public ItemData Item { get; set; }

    }//待确认:参数作用
    public class VillagerTradeConditions : Condition
    {
        [JsonProperty("item")]
        /// <summary>
        /// 交易的物品数据
        /// </summary>
        public ItemData Item { get; set; }

        [JsonProperty("villager")]
        /// <summary>
        /// 村民的数据
        /// </summary>
        public EntityData Villager { get; set; }
    }//待确认:参数作用


    /// <summary>
    /// 转换Effect枚举
    /// </summary>
    public class EffectNameConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Effect[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            foreach (Effect e in value as Effect[])
            {
                writer.WritePropertyName(EnumNameToJsonFormateConverter.EnumNameToJsonFormate(e.EffectName));
                writer.WriteRawValue(JsonConvert.SerializeObject(e, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            }
            writer.WriteEndObject();
        }
    }
}