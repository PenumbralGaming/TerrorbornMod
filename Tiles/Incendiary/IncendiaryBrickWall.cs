﻿using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Tiles.Incendiary
{
    class IncendiaryBrickWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = ModContent.ItemType<Items.Placeable.Walls.IncendiaryBrickWall>();
            AddMapEntry(new Color(103, 57, 42));
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}