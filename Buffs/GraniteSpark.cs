using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Buffs
{
    class GraniteSpark : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Granite Spark");
            Description.SetDefault("You are transformed into a highly manueverable spark!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.immuneAlpha = 255;
            player.wings = 0;
            int dust = Dust.NewDust(player.Center, 0, 0, DustID.Electric);
            Main.dust[dust].noGravity = true;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.graniteSpark = true;
            player.endurance -= 0.50f;
            player.noFallDmg = true;
            player.ignoreWater = true;
        }
    }
}
