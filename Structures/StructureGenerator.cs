using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Structures
{
    public static class StructureGenerator
    {
        internal static void GenerateHAShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/HAShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType) // use that tile type here
                );
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/HAShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Lava"));
            //sample.SetFlag(SamplingKey.LiquidLava);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_HalfBrick"));
            //sample.SetFlag(SamplingKey.HalfBrick);
            //sample.SetPosition(point);
            //sample.Apply();
        }

        internal static void GenerateTWShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/TWShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Titanium)
                );
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/TWShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Lava"));
            //sample.SetFlag(SamplingKey.LiquidLava);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_HalfBrick"));
            //sample.SetFlag(SamplingKey.HalfBrick);
            //sample.SetPosition(point);
            //sample.Apply();
        }

        internal static void GenerateVBShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/VBShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>(); // a random tile type that doesn't have Main.frameImportant flagged
            for (int i = 0; i < Main.maxTileSets; i++) // using a for loop instead of a while loop so that if the 1 in a quadrabilliontrillion chance that you don't get a valid tile after 1,000,000,000,000,000 rolls of Main.rand doesn't lag lul
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Platforms),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.LivingDemonFire)
                );
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/VBShrine_Walls")); // creates background walls
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_FrameImportant")); // places frame important tiles
            //sample.SetFlag(SamplingKey.FrameImportantTiles);
            //sample.SetPosition(point);
            //sample.SetSamplingValues(
            //    new ValueTuple<byte, byte, byte, int, int>(253, 27, 77, TileID.DemonAltar, 1) // the extra int value is the style of the tile
            //    );
            //sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Water"));
            //sample.SetFlag(SamplingKey.LiquidWater);
            //sample.SetPosition(point);
            //sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/VBShrine_Lava"));
            sample.SetFlag(SamplingKey.LiquidLava);
            sample.SetPosition(point);
            sample.Apply();

            //sample = new GenerationSample();
            //sample.SetSample(mod.GetTexture("Structures/StructureTest_Honey"));
            //sample.SetFlag(SamplingKey.LiquidHoney);
            //sample.SetPosition(point);
            //sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/VBShrine_HalfBrick"));
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            sample.Apply();
        }

        internal static void GenerateSOHShrine(Mod mod, Point point)
        {
            GenerationSample sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/SOHShrine"));
            sample.SetFlag(SamplingKey.Placement);
            sample.SetPosition(point);
            int tileType = ModContent.TileType<Tiles.MemorialBrick>();
            for (int i = 0; i < Main.maxTileSets; i++)
            {

            }
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, tileType),
                new ValueTuple<byte, byte, byte, int>(0, 255, 0, TileID.Platforms),
                new ValueTuple<byte, byte, byte, int>(0, 0, 255, TileID.LivingDemonFire)
                );
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/SOHShrine_Walls"));
            sample.SetFlag(SamplingKey.Walls);
            sample.SetPosition(point);
            sample.SetSamplingValues(
                new ValueTuple<byte, byte, byte, int>(255, 0, 0, ModContent.WallType<Tiles.MemorialWall>())
                );
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/SOHShrine_HalfBrick"));
            sample.SetFlag(SamplingKey.HalfBrick);
            sample.SetPosition(point);
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/SOHShrine_SlopeDownRight"));
            sample.SetFlag(SamplingKey.SlopeDownLeft);
            sample.SetPosition(point);
            sample.Apply();

            sample = new GenerationSample();
            sample.SetSample(mod.GetTexture("Structures/SOHShrine_SlopeDownLeft"));
            sample.SetFlag(SamplingKey.SlopeDownRight); //Already Tried: Slope Down Left, Slope Down Right, Slope Up Left, Slope Up Right (apparently one of the last 2)
            sample.SetPosition(point);
            sample.Apply();
        }
    }
}