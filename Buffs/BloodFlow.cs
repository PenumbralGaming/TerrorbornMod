﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Buffs
{
    class BloodFlow : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blood Flow");
            Description.SetDefault("Increased life regen while moving");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.velocity.X != 0)
            {
                player.lifeRegen += 3;
            }
        }
    }
}