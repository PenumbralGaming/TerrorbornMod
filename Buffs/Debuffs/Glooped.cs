using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs.Debuffs
{
    class Glooped : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Glooped");
            Description.SetDefault("Significantly decreased wing flight time");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.Glooped = true;
        }
    }
}
