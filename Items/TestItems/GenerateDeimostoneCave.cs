using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Generation;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.TestItems
{
    class GenerateDeimostoneCave : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }

        public override bool CanUseItem(Player player)
        {
            Vector2 position = Main.MouseWorld / 16;
            GenerateDeimostoneCaveMethod(new Point16((int)position.X, (int)position.Y), Main.rand.NextFloat(1f, 1.35f));
            return base.CanUseItem(player);
        }

        public void GenerateDeimostoneCaveMethod(Point16 position, float sizeMultiplier)
        {
            //int chest = WorldGen.PlaceChest((int)Main.MouseWorld.X / 16, (int)Main.MouseWorld.Y / 16, (ushort)ModContent.TileType<Tiles.DeimostoneChestTile>());
            //Main.NewText(chest);
            int x = (int)Main.MouseWorld.X / 16;
            int y = (int)Main.MouseWorld.Y / 16;
            //WorldGen.PlaceTile(x, y, (ushort)ModContent.TileType<Tiles.DeimostoneChestTile>());
            Main.NewText(Chest.FindChest(x, y));

            //    //WorldGen.TileRunner(position.X, position.Y, 200 * sizeMultiplier, 10, ModContent.TileType<Tiles.Deimostone>());
            //    int caveHeight = (int)(85 * sizeMultiplier);
            //    int caveWidth = (int)(45 * sizeMultiplier);
            //    int sideWidth = (int)(10 * sizeMultiplier);
            //    int currentSideOffset = 0;
            //    float offsetChance = 0.4f;
            //    int maxOffsetRange = 4;
            //    int maxSizeOffset = 2;
            //    int distortRange = 10;
            //    float distortChance = 0.3f;

            //    for (int i = 0; i < caveWidth; i++)
            //    {
            //        for (int j = 0; j < caveHeight; j++)
            //        {
            //            if ((j < distortRange || j > caveHeight - distortRange) && WorldGen.genRand.NextFloat() <= distortChance)
            //            {

            //            }
            //            else
            //            {
            //                Point16 tilePosition = new Point16(position.X + i, position.Y + j);
            //                WorldGen.KillTile(tilePosition.X, tilePosition.Y);
            //                WorldGen.KillWall(tilePosition.X, tilePosition.Y);
            //                WorldGen.PlaceWall(tilePosition.X, tilePosition.Y, ModContent.WallType<Tiles.DeimostoneWallTile>());
            //            }
            //        }
            //    }

            //    for (int y = 0; y < caveHeight; y++)
            //    {
            //        if (WorldGen.genRand.NextFloat() <= offsetChance)
            //        {
            //            if (WorldGen.genRand.NextBool())
            //            {
            //                currentSideOffset++;
            //            }
            //            else
            //            {
            //                currentSideOffset--;
            //            }

            //            if (currentSideOffset < -maxOffsetRange)
            //            {
            //                currentSideOffset = -maxOffsetRange;
            //            }
            //            if (currentSideOffset > maxOffsetRange)
            //            {
            //                currentSideOffset = maxOffsetRange;
            //            }
            //        }
            //        for (int x = 0; x < sideWidth + WorldGen.genRand.Next(-maxSizeOffset, maxSizeOffset + 1); x++)
            //        {
            //            Point16 tilePosition = position + new Point16(x + currentSideOffset, y);
            //            WorldGen.KillTile(tilePosition.X, tilePosition.Y);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }
            //    }

            //    currentSideOffset = 0;
            //    for (int y = 0; y < caveHeight; y++)
            //    {
            //        if (WorldGen.genRand.NextFloat() <= offsetChance)
            //        {
            //            if (WorldGen.genRand.NextBool())
            //            {
            //                currentSideOffset++;
            //            }
            //            else
            //            {
            //                currentSideOffset--;
            //            }

            //            if (currentSideOffset < -maxOffsetRange)
            //            {
            //                currentSideOffset = -maxOffsetRange;
            //            }
            //            if (currentSideOffset > maxOffsetRange)
            //            {
            //                currentSideOffset = maxOffsetRange;
            //            }
            //        }
            //        for (int x = 0; x < sideWidth + WorldGen.genRand.Next(-maxSizeOffset, maxSizeOffset + 1); x++)
            //        {
            //            Point16 newPosition = position + new Point16(caveWidth, 0);
            //            Point16 tilePosition = newPosition + new Point16(-x - currentSideOffset, y);
            //            WorldGen.KillTile(tilePosition.X, tilePosition.Y);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }
            //    }

            //    for (int i = 0; i < WorldGen.genRand.Next(5, 8) * sizeMultiplier; i++)
            //    {
            //        Point16 platformPosition = new Point16(position.X, position.Y + WorldGen.genRand.Next((int)(15 * sizeMultiplier), caveHeight - (int)(15 * sizeMultiplier)));
            //        int platformLength = (int)(WorldGen.genRand.Next(20, 45) * sizeMultiplier);

            //        for (int i2 = 0; i2 < platformLength; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(i2, 0);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }

            //        for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(i2, -1);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }

            //        for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(i2, 1);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }
            //    }

            //    for (int i = 0; i < WorldGen.genRand.Next(5, 8) * sizeMultiplier; i++)
            //    {
            //        Point16 platformPosition = new Point16(position.X + caveWidth, position.Y + WorldGen.genRand.Next((int)(15 * sizeMultiplier), caveHeight - (int)(15 * sizeMultiplier)));
            //        int platformLength = (int)(WorldGen.genRand.Next(20, 35) * sizeMultiplier);

            //        for (int i2 = 0; i2 < platformLength; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(-i2, 0);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }

            //        for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(-i2, -1);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }

            //        for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
            //        {
            //            Point16 tilePosition = platformPosition + new Point16(-i2, 1);
            //            WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
            //        }
            //    }


        }
    }
}
