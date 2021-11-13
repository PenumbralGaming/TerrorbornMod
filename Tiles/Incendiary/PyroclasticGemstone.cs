﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace TerrorbornMod.Tiles.Incendiary
{
    class PyroclasticGemstone : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.DrawYOffset = 2;
            TileObjectData.addTile(Type);

            soundType = 42;
            soundStyle = 165;
            drop = ModContent.ItemType<Items.Materials.PyroclasticGemstone>();

            mineResist = 5f;
            minPick = 100;

            dustType = DustID.Fire;

            Main.tileLighted[Type] = true;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Pyroclastic Gemstone");
            AddMapEntry(new Color(255, 246, 216), name);
        }

        private readonly int animationFrameWidth = 18;
        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            int uniqueAnimationFrame = Main.tileFrame[Type] + i;
            if (i % 2 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            if (i % 3 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            if (i % 4 == 0)
            {
                uniqueAnimationFrame += 3;
            }
            uniqueAnimationFrame = uniqueAnimationFrame % 3;

            frameXOffset = uniqueAnimationFrame * animationFrameWidth;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            float thingy = 3f;
            r = 1f / thingy;
            g = 0.426f / thingy;
            b = 0.384f / thingy;
        }
    }
}
