using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftToolsBoxSDK.Nbt.Entity
{
    public class Entity
    {
        [NbtProperty(Name = "id")]
        public string Id;
        [NbtProperty(MaxCount = 3)]
        public double[] Pos;
        [NbtProperty(MaxCount = 3)]
        public double[] Motion;
        [NbtProperty(MaxCount = 2)]
        public float[] Rotation;
        public float? FallDistance;
        public short? Fire;
        public short? Air;
        public bool? OnGround;
        public int? Dimension;
        public bool? Invulnerable;
        public int? ProtalCooldown;
        public long? UUIDMost;
        public long? UUIDLeast;
        /// <summary>
        /// Removed after 1.9
        /// </summary>
        public string UUID;
        public string CustomName;
        public bool? CustomNameVisible;
        public bool? Silent;
        /// <summary>
        /// Removed after 15w41a
        /// </summary>
        public Entity Riding;
        public Entity[] Passengers = new Entity[1];
        public bool? Glowing;
        public string[] Tags;
        public CommandStats CommandStats;
    }
    public class CommandStats
    {
        public bool SucessCountObjective;
        public string SucessCountName;
        public int AffectedBlocksObjective;
        public string AffectedBlocksName;
        public int AffectedEntitiesObjective;
        public string AffectedEntitiesName;
        public int AffectedItemsObjective;
        public string AffectedItemsName;
        public string QueryResultObjective;
        public string QueryResultName;
    }
}
