using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class TidalCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tidal Cooldown");
            Description.SetDefault("'You have to regain the power of the tides'");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}