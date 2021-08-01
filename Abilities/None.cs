using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Abilities
{
    class None : AbilityInfo
    {
        public override int typeInt()
        {
            return 0;
        }

        public override float Cost()
        {
            return 0f;
        }

        public override string Name()
        {
            return "None";
        }

        public override string Description()
        {
            return " ";
        }
    }
    class NotYetUnlocked : AbilityInfo
    {
        public override int typeInt()
        {
            return 0;
        }

        public override float Cost()
        {
            return 0f;
        }

        public override string Name()
        {
            return "Not Yet Unlocked";
        }

        public override string Description()
        {
            return " ";
        }
    }
}
