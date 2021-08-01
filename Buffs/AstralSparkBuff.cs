using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Buffs
{
    class AstralSparkBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Astral Spark");
            Description.SetDefault("You are transformed into a highly manueverable spark!" +
                "\nHold JUMP to move at insanely fast speeds");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.immuneAlpha = 255;
            player.wings = 0;
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(player.Center, 0, 0, 62, Scale: 2f);
                Main.dust[dust].noGravity = true;
            }
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.astralSpark = true;
            player.noFallDmg = true;
            player.ignoreWater = true;

            if (player.buffTime[buffIndex] == 1)
            {
                modPlayer.iFrames = 60 * 3;
            }
        }
    }
}
