using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class aerodynamic : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Aerodynamic");
            Description.SetDefault("Wind won't push you around, you are more agile, and increased wing flight time");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.WindPushed] = true;
            player.moveSpeed += 3;
            player.maxRunSpeed += .5f;

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.Aerodynamic = true;
        }
    }
}
