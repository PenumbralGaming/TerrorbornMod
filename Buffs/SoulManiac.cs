using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class SoulManiac : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Soul Maniac");
            Description.SetDefault("Increased restless use speed while not fully charged and increased restless damage");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.restlessNonChargedUseSpeed *= 1.3f;
            modPlayer.restlessDamage += 0.25f;
        }
    }
}
