using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class GraniteSparkCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Virus Cooldown");
            Description.SetDefault("You cannot transform into a virus spark again");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}
