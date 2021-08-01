using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class Azurite : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true; 
            soundType = 21;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            minPick = 56;
            mineResist = 2;
            drop = mod.ItemType("AzuriteOre");
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Azurite");
            AddMapEntry(new Color(106, 142, 193), name);
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
            dustType = 33;
            makeDust = true;
        }

        public override bool HasWalkDust()
        {
            return true;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0;
            g = 0;
            b = 0.1f;
        }
    }
}
