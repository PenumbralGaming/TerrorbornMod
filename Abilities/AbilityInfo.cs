using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Converters;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Abilities
{
    class AbilityInfo
    {
        public virtual Texture2D texture()
        {
            return ModContent.GetTexture("TerrorbornMod/Abilities/ShriekOfHorror_Icon");
        }

        public virtual int typeInt()
        {
            return 69;
        }

        public virtual bool canUse(Player player)
        {
            return true;
        }

        public virtual string Name()
        {
            return "pootis";
        }

        public virtual string Description()
        {
            return "Summons pootis on use";
        }

        public virtual bool HeldDown()
        {
            return false;
        }

        public virtual float Cost()
        {
            return 25f;
        }

        public virtual void OnUse(Player player)
        {

        }
    }
}
