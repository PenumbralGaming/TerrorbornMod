﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace TerrorbornMod.Tiles.Incendiary
{
    public class IncendiaryAlloyTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = SoundID.Tink;
            soundStyle = 1;
            dustType = 6;
            minPick = 150;
            mineResist = 6f;
            drop = ModContent.ItemType<Items.Materials.IncendiusAlloy>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Incendiary Alloy");
            AddMapEntry(new Color(208, 102, 102), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.3f;
            g = 0;
            b = 0f;
        }
    }
}
