using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class DeimosteelOre : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = 21;
            soundStyle = 1;

            minPick = 0;
            mineResist = 1f;
            drop = ModContent.ItemType<Items.Placeable.Blocks.DeimosteelOreItem>();
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Deimosteel");
            AddMapEntry(new Color(138, 155, 152), name);
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

