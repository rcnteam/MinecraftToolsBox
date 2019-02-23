using MinecraftToolsBoxSDK;
using Newtonsoft.Json;
using System;

namespace MinecraftToolsBoxSDK.Json.Advancements
{
    /// <summary>
    /// 基本条件（Condition）参数，表示实体的位置信息
    /// </summary>
    public class Location
    {
        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        [JsonProperty("biome")]
        /// <summary>
        /// 实体所处的生物群系
        /// </summary>
        public BiomeName Biome { get; set; }

        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        [JsonProperty("dimension")]
        /// <summary>
        /// 实体所处的维度
        /// </summary>
        public DimensionName Dimension { get; set; }

        [JsonProperty("feature")]
        /// <summary>
        /// 实体所处的结构（structure）名称
        /// </summary>
        public string Feature { get; set; }
        [JsonProperty("position")]
        /// <summary>
        /// 坐标位置
        /// </summary>
        public Position Position { get; set; }
    }
    public enum BiomeName { BIOME }
    public enum DimensionName { TheOverworld, TheNether, TheEnd }

    /// <summary>
    /// 基本条件（Condition）参数，表示实体的坐标信息
    /// </summary>
    public class Distance
    {
        [JsonProperty("x")]
        /// <summary>
        /// 到原点的x距离范围
        /// 设置为double值表示精确数据 或 实例化DoubleRange表示范围
        /// </summary>
        public object XRange { get; set; }
        [JsonProperty("y")]
        /// <summary>
        /// 到原点的y距离范围
        /// 设置为double值表示精确数据 或 实例化DoubleRange表示范围
        /// </summary>
        public object YRange { get; set; }
        [JsonProperty("z")]
        /// <summary>
        /// 到原点的z距离范围
        /// 设置为double值表示精确数据 或 实例化DoubleRange表示范围
        /// </summary>
        public object ZRange { get; set; }
    }
    public class HorizontalDistance
    {
        [JsonProperty("horizontal")]
        public IntegerRange Horizontal { get; set; }
    }

    /// <summary>
    /// 基本条件（Condition）参数，表示实体的位置信息
    /// </summary>
    public class Position
    {
        [JsonProperty("x")]
        /// <summary>
        /// 到原点的x距离范围
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object XRange { get; set; }
        [JsonProperty("y")]
        /// <summary>
        /// 到原点的y距离范围
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object YRange { get; set; }
        [JsonProperty("z")]
        /// <summary>
        /// 到原点的z距离范围
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object ZRange { get; set; }
    }

    public class Effect
    {
        [JsonIgnore]
        public EffectsName EffectName { get; set; }

        [JsonProperty("amplifer")]
        /// <summary>
        /// 效果等级
        /// 设置为整数（int）表示确定的数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Amplifer { get; set; }

        [JsonProperty("duration")]
        /// <summary>
        /// 效果持续时间，以Tick为单位
        /// 设置为整数（int）表示确定的数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Duration { get; set; }
    }
    public enum EffectsName { EFECT }

    public class ItemData
    {
        [JsonProperty("count")]
        /// <summary>
        /// 物品数量。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Count { get; set; }
        [JsonProperty("data")]
        /// <summary>
        /// 物品数据值。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Data { get; set; }
        [JsonProperty("durability")]
        /// <summary>
        /// 物品耐久度。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Durability { get; set; }
        [JsonProperty("enchantments")]
        /// <summary>
        /// 魔咒列表
        /// </summary>
        public Enchantment[] Enchantments { get; set; }
        [JsonProperty("item")]
        /// <summary>
        /// 物品ID
        /// 注意：以 minecraft: 开头
        /// </summary>
        public string Item { get; set; }
        [JsonProperty("nbt")]
        /// <summary>
        /// 物品的NBT标签
        /// </summary>
        public string Nbt { get; set; }
        [JsonConverter(typeof(StringToJsonFormateConverter))]
        [JsonProperty("potion")]
        /// <summary>
        /// 酿造药水的ID
        /// </summary>
        public PotionID Potion { get; set; }
    }
    public enum PotionID { POTION }
    
    public class Enchantment
    {
        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        [JsonProperty("enchantment")]
        public EnchantmentName EnchantmentName { get; set; }

        [JsonProperty("levels")]
        /// <summary>
        /// 魔咒等级。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Levels { get; set; }
    }
    public enum EnchantmentName { ENCHANTMENT }

    public class IntegerRange
    {
        [JsonProperty("max")]
        public int? Max { get; set; }
        [JsonProperty("min")]
        public int? Min { get; set; }
    }
    public class DoubleRange
    {
        [JsonProperty("max")]
        public double? Max { get; set; }
        [JsonProperty("min")]
        public double? Min { get; set; }
    }

    /// <summary>
    /// 二级条件（Condition）参数，包含实体数据
    /// </summary>
    public class EntityData
    {
        [JsonProperty("distance")]
        /// <summary>
        /// 可以使用以下方式表示实体到原点的距离参数
        /// 一个整数（int）或 实例化IntegerRange表示范围 或 实例化HorizontalDistance表示水平距离 或 实例化Distance表示3D空间中的距离参数
        /// </summary>
        public object Distance { get; set; }

        [JsonProperty("effects")]
        [JsonConverter(typeof(EffectNameConverter))]
        /// <summary>
        /// 状态效果列表。 
        /// </summary>
        public Effect[] Effects { get; set; }

        [JsonProperty("location")]
        /// <summary>
        /// 实体的位置信息
        /// </summary>
        public Location Location { get; set; }

        [JsonProperty("nbt")]
        /// <summary>
        /// 匹配实体的NBT
        /// </summary>
        public string Nbt { get; set; }
    }
    /// <summary>
    /// EntityData的扩展类，增加了实体ID属性
    /// </summary>
    public class ExtendedEntityData : EntityData
    {
        [JsonConverter(typeof(EnumNameToJsonFormateConverter))]
        [JsonProperty("type")]
        /// <summary>
        /// 实体ID
        /// </summary>
        public EntityName Type { get; set; }
    }
    public enum EntityName { ENTITY }

    public class Damage
    {
        [JsonProperty("blocked")]
        /// <summary>
        /// 检查伤害是否被成功阻挡。
        /// </summary>
        public bool? Blocked { get; set; }

        [JsonProperty("dealt")]
        /// <summary>
        /// 在减少伤害前检查实体即将受到的伤害。
        /// 设置为double值表示精确数据 或 实例化DoubleRange表示范围
        /// </summary>
        public object Dealt { get; set; }

        [JsonProperty("source_entity")]
        /// <summary>
        /// 伤害来源
        /// </summary>
        public ExtendedEntityData SourceEntity { get; set; }

        [JsonProperty("taken")]
        /// <summary>
        /// 在减少伤害前检查实体即将受到的伤害。
        /// 设置为double值表示精确数据 或 实例化DoubleRange表示范围
        /// </summary>
        public object Taken { get; set; }

        [JsonProperty("type")]
        /// <summary>
        /// 
        /// </summary>
        public DamageType Type { get; set; }
    }
    public class DamageType
    {
        [JsonProperty("bypasses_armor")]
        /// <summary>
        /// 
        /// </summary>
        public bool? BypassesArmor { get; set; }
        [JsonProperty("bypasses_invulnerability")]
        /// <summary>
        /// 
        /// </summary>
        public bool? BypassesInvulnerability { get; set; }
        [JsonProperty("bypasses_magic")]
        /// <summary>
        /// 
        /// </summary>
        public bool? BypassesMagic { get; set; }

        [JsonProperty("is_explosion")]
        /// <summary>
        /// 
        /// </summary>
        public bool? IsExplosion { get; set; }
        [JsonProperty("is_fire")]
        /// <summary>
        /// 
        /// </summary>
        public bool? IsFire { get; set; }
        [JsonProperty("is_magic")]
        /// <summary>
        /// 
        /// </summary>
        public bool? IsMagic { get; set; }
        [JsonProperty("is_projectile")]
        /// <summary>
        /// 
        /// </summary>
        public bool? IsProjectile { get; set; }
    }//待确认

    public class Slots
    {
        [JsonProperty("empty")]
        /// <summary>
        /// 物品栏中空槽位数量。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Empty { get; set; }
        [JsonProperty("full")]
        /// <summary>
        /// 物品栏中已被完全使用（填充了一组物品）槽位数量。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Full { get; set; }
        [JsonProperty("occupied")]
        /// <summary>
        /// 物品栏中已使用槽位数量。
        /// 设置为int值表示精确数据 或 实例化IntegerRange表示范围
        /// </summary>
        public object Occupied { get; set; }

    }
}
