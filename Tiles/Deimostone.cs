using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Deimostone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = false;
            soundType = 21;
            soundStyle = 1;

            minPick = 0;
            mineResist = 0.5f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.DeimostoneBlock>();
            AddMapEntry(new Color(40, 55, 70));
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
