using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class Darkblood : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Darkblood");
            Description.SetDefault("Allows you to drain terror from enemies while attacking, also allowing you to attack while using Shriek of Horror");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}

