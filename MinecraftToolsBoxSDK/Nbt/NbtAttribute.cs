using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftToolsBoxSDK.Nbt
{
    [AttributeUsage(AttributeTargets.All)]
    public class NbtProperty : Attribute
    {
        public string Name { get; set; }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public int MaxCount { get; set; }
    }
}
