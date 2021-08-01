using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class AstralSparkCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Spark Cooldown");
            Description.SetDefault("You cannot transform into an astral spark again");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}

