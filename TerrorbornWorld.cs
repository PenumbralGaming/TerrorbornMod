using System.IO;
using System.Collections.Generic;
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

namespace TerrorbornMod
{
    public class TerrorbornWorld : ModWorld
    {
        public static bool downedShadowcrawler;
        public static bool downedPrototypeI;
        public static bool downedTidalTitan;
        public static bool downedDunestock;
        public static bool downedSangrune;
        public static bool downedSangrune2;
        public static bool downedUndyingSpirit;
        public static bool obtainedShriekOfHorror;
        public static bool downedTerrorRain;
        public static bool downedFrightcrawler;
        public static bool terrorRain;
        public static int timeSinceFrightcrawlerSpawn = 0;
        public static int ShadowTiles = 0;
        public static int CurrentBountyBiome = 69; //You can't stop me from keeping it like this
        public static bool UnaliveInvasionUp;
        public static string SkeletonSheriffName;
        public static int TerrorMasterDialogue;
        public static int deimostoneTiles;
        public static int wormExtraSegmentCount = 0;

        public static Vector2 ShriekOfHorror;
        public static Vector2 HorrificAdaptation;
        public static Vector2 VoidBlink;
        public static Vector2 TerrorWarp;

        int BountyBiomeCount = 7;

        bool WasNight = false;

        public override void Initialize()
        {
            downedShadowcrawler = false;
            downedPrototypeI = false;
            downedTidalTitan = false;
            downedDunestock = false;
            timeSinceFrightcrawlerSpawn = 0;
            downedSangrune = false;
            terrorRain = false;
            downedTerrorRain = false;
            downedFrightcrawler = false;
            downedUndyingSpirit = false;
            obtainedShriekOfHorror = false;
            TerrorMasterDialogue = 0;
            SkeletonSheriffName = getSkeletonSheriffName();
            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
        }

        public override void PreUpdate()
        {
            wormExtraSegmentCount = 0;
            foreach (NPC npc in Main.npc)
            {
                if (npc.active)
                {
                    TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(npc);
                    if (globalNPC.extraWormSegment)
                    {
                        wormExtraSegmentCount++;
                    }
                }
            }
        }

        public override void ResetNearbyTileEffects()
        {
            TerrorbornPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<TerrorbornPlayer>();
            deimostoneTiles = 0;
        }
        public override void TileCountsAvailable(int[] tileCounts)
        {
            deimostoneTiles = tileCounts[ModContent.TileType<Tiles.Deimostone>()];
        }

        public string getSkeletonSheriffName()
        {
            switch (WorldGen.genRand.Next(7))
            {
                case 0:
                    return "Femurandus";
                case 1:
                    return "Cartage";
                case 2:
                    return "Pelven";
                case 3:
                    return "Chronius";
                case 4:
                    return "Scapulan";
                case 5:
                    return "Chad";
                case 6:
                    return "Raster";
                default:
                    return "Liven't";
            }
        }

        public override TagCompound Save()
        {
            var downed = new List<string>();
            if (downedShadowcrawler) downed.Add("Shadowcrawler");
            if (downedPrototypeI) downed.Add("PrototypeI");
            if (downedTidalTitan) downed.Add("TidalTitan");
            if (downedDunestock) downed.Add("Dunestock");
            if (downedSangrune) downed.Add("Sangrune");
            if (downedSangrune2) downed.Add("Sangrune2");
            if (downedUndyingSpirit) downed.Add("UndyingSpirit");
            if (obtainedShriekOfHorror) downed.Add("ShriekOfHorror");
            if (downedTerrorRain) downed.Add("downedTerrorRain");
            if (downedFrightcrawler) downed.Add("downedFrightcrawler");

            return new TagCompound {
                {"downed", downed},
                {"CurrentBountyBiome", CurrentBountyBiome},
                {"SkeletonSheriffName", SkeletonSheriffName},
                {"TerrorMasterDialogue", TerrorMasterDialogue},
                {"VoidBlink", VoidBlink},
                {"TerrorWarp", TerrorWarp},
                {"terrorRain", terrorRain},
                {"timeSinceFrightcrawlerSpawn", timeSinceFrightcrawlerSpawn}
            };
            
        }
        public override void Load(TagCompound tag)
        {
            var downed = tag.GetList<string>("downed");
            downedShadowcrawler = downed.Contains("Shadowcrawler");
            downedPrototypeI = downed.Contains("PrototypeI");
            downedTidalTitan = downed.Contains("TidalTitan");
            downedDunestock = downed.Contains("Dunestock");
            downedSangrune = downed.Contains("Sangrune");
            downedSangrune2 = downed.Contains("Sangrune2");
            downedUndyingSpirit = downed.Contains("UndyingSpirit");
            obtainedShriekOfHorror = downed.Contains("ShriekOfHorror");
            downedTerrorRain = downed.Contains("downedTerrorRain");
            downedFrightcrawler = downed.Contains("downedFrightcrawler");
            CurrentBountyBiome = tag.GetInt("CurrentBountyBiome");
            SkeletonSheriffName = tag.GetString("SkeletonSheriffName");
            TerrorMasterDialogue = tag.GetInt("TerrorMasterDialogue");
            VoidBlink = tag.Get<Vector2>("VoidBlink");
            TerrorWarp = tag.Get<Vector2>("TerrorWarp");
            terrorRain = tag.GetBool("terrorRain");
            timeSinceFrightcrawlerSpawn = tag.GetInt("timeSinceFrightcrawlerSpawn");
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
            if (loadVersion == 0)
            {
                BitsByte flags = reader.ReadByte();
                downedShadowcrawler = flags[0];
            }
            else
            {
                ErrorLogger.Log("Terrorborn Mod: Unknown loadVersion: " + loadVersion);
            }
        }

        public void CreateLineOfBlocksHorizontal(int LineX, int LineY, int Length, int Type)
        {
            for (int i = 0; i < Length; i++)
            {
                WorldGen.PlaceTile(LineX + i, LineY, Type);
            }
        }
        public void CreateLineOfBlocksVerticle(int LineX, int LineY, int Length, int Type)
        {
            for (int i = 0; i < Length; i++)
            {
                WorldGen.PlaceTile(LineX, LineY + i, Type);
            }
        }
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimostone Chests", delegate (GenerationProgress progress)
            {
                progress.Message = "Generating Deimostone Chests";
                GenerateDeimostoneChests();
            }));
            tasks.Insert(genIndex + 2, new PassLegacy("Terror Shrines", delegate (GenerationProgress progress)
            {
                progress.Message = "Building the shrines";
                GenerateShrineStructures();
            }));
            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Granite"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimostone caves", delegate (GenerationProgress progress)
            {
                deimostoneCaves.Clear();
                progress.Message = "Darkening Deimostone";
                GenerateDeimostoneCaves();
            }));
            genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            tasks.Insert(genIndex + 1, new PassLegacy("Deimosteel Ore", delegate (GenerationProgress progress)
            {
                progress.Message = "Generating Deimosteel";
                GenerateDeimosteel();
            }));

        }

        List<Point16> deimostoneChests = new List<Point16>(500);
        List<Rectangle> deimostoneCaves = new List<Rectangle>();
        public override void PostWorldGen()
        {
            for (int i = 0; i < 1000; i++)
            {
                Chest chest = Main.chest[i];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 11 * 36)
                {
                    if (Main.rand.Next(101) <= 25)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Summons.Minions.FrigidStaff>());
                                break;
                            }
                        }
                    }
                }
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 2 * 36)
                {
                    if (Main.rand.Next(101) <= 20)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Ranged.DualpipeDartgun>());
                                break;
                            }
                        }
                    }
                }
                if (chest != null && Main.tile[chest.x, chest.y].type == ModContent.TileType<Tiles.DeimostoneChestTile>())
                {
                    List<int> mainItems = new List<int>();
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Magic.ShriekersLung>());
                    mainItems.Add(ModContent.ItemType<Items.Equipable.Accessories.HorrificCharm>());
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Melee.NightLight>());
                    mainItems.Add(ModContent.ItemType<Items.Weapons.Ranged.MindPiercer>());

                    List<int> secondaryMainItems = new List<int>();
                    secondaryMainItems.Add(ItemID.BandofRegeneration);
                    secondaryMainItems.Add(ItemID.MagicMirror);
                    secondaryMainItems.Add(ItemID.CloudinaBottle);
                    secondaryMainItems.Add(ItemID.HermesBoots);
                    secondaryMainItems.Add(ItemID.ShoeSpikes);

                    List<int> ammosAndThrowables = new List<int>();
                    ammosAndThrowables.Add(ItemID.FlamingArrow);
                    ammosAndThrowables.Add(ItemID.ThrowingKnife);

                    List<int> bars = new List<int>();
                    bars.Add(ItemID.SilverBar);
                    bars.Add(ItemID.TungstenBar);
                    bars.Add(ItemID.GoldBar);
                    bars.Add(ItemID.PlatinumBar);

                    List<int> commonPotions = new List<int>();
                    commonPotions.Add(ItemID.SpelunkerPotion);
                    commonPotions.Add(ItemID.FeatherfallPotion);
                    commonPotions.Add(ItemID.NightOwlPotion);
                    commonPotions.Add(ItemID.WaterWalkingPotion);
                    commonPotions.Add(ItemID.ArcheryPotion);
                    commonPotions.Add(ItemID.GravitationPotion);

                    List<int> uncommonPotions = new List<int>();
                    uncommonPotions.Add(ItemID.ThornsPotion);
                    uncommonPotions.Add(ItemID.HunterPotion);
                    uncommonPotions.Add(ItemID.TrapsightPotion);
                    uncommonPotions.Add(ItemID.TeleportationPotion);

                    List<int> lightItems = new List<int>();
                    lightItems.Add(ItemID.Torch);
                    lightItems.Add(ItemID.SpelunkerGlowstick);

                    int item = 0;
                    chest.item[item].SetDefaults(WorldGen.genRand.Next(mainItems));
                    item++;
                    chest.item[item].SetDefaults(WorldGen.genRand.Next(secondaryMainItems));
                    item++;
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.Extractinator);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.4f)
                    {
                        chest.item[item].SetDefaults(ItemID.SuspiciousLookingEye);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.33f)
                    {
                        chest.item[item].SetDefaults(ItemID.Dynamite);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.25f)
                    {
                        chest.item[item].SetDefaults(ItemID.JestersArrow);
                        chest.item[item].stack = WorldGen.genRand.Next(25, 51);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(bars));
                        chest.item[item].stack = WorldGen.genRand.Next(5, 15);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(ammosAndThrowables));
                        chest.item[item].stack = WorldGen.genRand.Next(25, 51);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.HealingPotion);
                        chest.item[item].stack = WorldGen.genRand.Next(6, 11);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.666f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(commonPotions));
                        chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.333f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(uncommonPotions));
                        chest.item[item].stack = WorldGen.genRand.Next(2, 4);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.RecallPotion);
                        chest.item[item].stack = WorldGen.genRand.Next(3, 6);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(WorldGen.genRand.Next(lightItems));
                        chest.item[item].stack = WorldGen.genRand.Next(20, 40);
                        item++;
                    }
                    if (WorldGen.genRand.NextFloat() <= 0.5f)
                    {
                        chest.item[item].SetDefaults(ItemID.GoldCoin);
                        chest.item[item].stack = WorldGen.genRand.Next(2, 5);
                        item++;
                    }
                }
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36)
                {
                    if (Main.rand.NextFloat() <= 0.25f)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.Nunchucks>());
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void GenerateDeimosteel()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05 / 3); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(3, 5), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
            }

            foreach(Rectangle element in deimostoneCaves)
            {
                for (int i = 0; i < WorldGen.genRand.Next(5, 8); i++)
                {
                    int x = WorldGen.genRand.Next(element.Left, element.Right);
                    int y = WorldGen.genRand.Next(element.Top, element.Bottom);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
                }
                for (int i = 0; i < WorldGen.genRand.Next(4, 7); i++)
                {
                    int x = WorldGen.genRand.Next(element.Left - 3, element.Left + 10);
                    int y = WorldGen.genRand.Next(element.Top, element.Bottom);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
                }
                for (int i = 0; i < WorldGen.genRand.Next(4, 7); i++)
                {
                    int x = WorldGen.genRand.Next(element.Right - 10, element.Right + 3);
                    int y = WorldGen.genRand.Next(element.Top, element.Bottom);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 6), WorldGen.genRand.Next(3, 6), ModContent.TileType<Tiles.DeimosteelOre>(), false, 0f, 0f, false, true);
                }
            }
        }

        public void GenerateShrineStructures()
        {
            Vector2 ShriekOfHorrorPosition = new Vector2(Main.spawnTileX, Main.spawnTileY);
            float sizeMultiplier = 1;
            if (Main.maxTilesX == 6400)
            {
                sizeMultiplier = 1.5f;
            }
            if (Main.maxTilesX == 8400)
            {
                sizeMultiplier = 2f;
            }
            ShriekOfHorrorPosition.Y += 285 * sizeMultiplier;
            ShriekOfHorrorPosition.X -= 15;

            Structures.StructureGenerator.GenerateSOHShrine(mod, new Point((int)ShriekOfHorrorPosition.X, (int)ShriekOfHorrorPosition.Y));

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            Vector2 HorrificAdaptationPosition = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            Structures.StructureGenerator.GenerateHAShrine(mod, new Point((int)HorrificAdaptationPosition.X, (int)HorrificAdaptationPosition.Y));

            VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
            Structures.StructureGenerator.GenerateVBShrine(mod, new Point((int)VoidBlink.X, (int)VoidBlink.Y));

            TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
            Structures.StructureGenerator.GenerateTWShrine(mod, new Point((int)TerrorWarp.X, (int)TerrorWarp.Y));
        }

        public void GenerateDeimostoneCaves()
        {
            float amountMultiplier = 1f;
            GenerateDeimostoneCave(new Point16(WorldGen.genRand.Next(Main.maxTilesX / 2 - 350, Main.maxTilesX / 2 + 350), (int)(WorldGen.rockLayerHigh + 25)), WorldGen.genRand.NextFloat(1.2f, 1.6f));
            for (int i = 0; i < WorldGen.genRand.Next(30, 45) * amountMultiplier; i++)
            {
                GenerateDeimostoneCave(new Point16(WorldGen.genRand.Next(500, Main.maxTilesX - 500), WorldGen.genRand.Next((int)WorldGen.rockLayerHigh, (int)Main.maxTilesY - 300)), WorldGen.genRand.NextFloat(1.2f, 1.6f));
            }
        }

        public void GenerateDeimostoneCave(Point16 position, float sizeMultiplier)
        {
            int caveHeight = (int)(85 * sizeMultiplier);
            int caveWidth = (int)(45 * sizeMultiplier);
            int sideWidth = (int)(10 * sizeMultiplier);
            int currentSideOffset = 0;
            float offsetChance = 0.4f;
            int maxOffsetRange = 4;
            int maxSizeOffset = 2;
            int distortRange = 2;
            float distortChance = 0.3f;
            int caveCenterX = position.X + caveWidth / 2;


            deimostoneCaves.Add(new Rectangle(position.X, position.Y, caveWidth, caveHeight));

            bool dontGenerate = false;
            for (int i = 0; i < caveWidth; i++)
            {
                for (int j = 0; j < caveHeight; j++)
                {
                    Point16 tilePosition = new Point16(position.X + i, position.Y + j);
                    if (Main.tile[tilePosition.X, tilePosition.Y].type == ModContent.TileType<Tiles.Deimostone>())
                    {
                        dontGenerate = true;
                    }
                }
            }

            if (dontGenerate)
            {
                return;
            }

            for (int i = 5; i < caveWidth - 5; i++)
            {
                for (int j = 5; j < caveHeight - 5; j++)
                {
                    if ((j < distortRange + 5 || j > caveHeight - distortRange - 5) && WorldGen.genRand.NextFloat() <= distortChance)
                    {

                    }
                    else
                    {
                        Point16 tilePosition = new Point16(position.X + i, position.Y + j);
                        WorldGen.KillTile(tilePosition.X, tilePosition.Y);
                        WorldGen.KillWall(tilePosition.X, tilePosition.Y);
                        WorldGen.PlaceWall(tilePosition.X, tilePosition.Y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                }
            }

            for (int y = 0; y < caveHeight; y++)
            {
                if (WorldGen.genRand.NextFloat() <= offsetChance)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        currentSideOffset++;
                    }
                    else
                    {
                        currentSideOffset--;
                    }

                    if (currentSideOffset < -maxOffsetRange)
                    {
                        currentSideOffset = -maxOffsetRange;
                    }
                    if (currentSideOffset > maxOffsetRange)
                    {
                        currentSideOffset = maxOffsetRange;
                    }
                }
                for (int x = 0; x < sideWidth + WorldGen.genRand.Next(-maxSizeOffset, maxSizeOffset + 1); x++)
                {
                    Point16 tilePosition = position + new Point16(x + currentSideOffset, y);

                    if ((y > 1 && y < caveHeight - 2) || WorldGen.genRand.NextBool())
                    {
                        WorldGen.KillTile(tilePosition.X, tilePosition.Y);
                        WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                        WorldGen.PlaceWall(tilePosition.X, tilePosition.Y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                }
            }

            currentSideOffset = 0;
            for (int y = 0; y < caveHeight; y++)
            {
                if (WorldGen.genRand.NextFloat() <= offsetChance)
                {
                    if (WorldGen.genRand.NextBool())
                    {
                        currentSideOffset++;
                    }
                    else
                    {
                        currentSideOffset--;
                    }

                    if (currentSideOffset < -maxOffsetRange)
                    {
                        currentSideOffset = -maxOffsetRange;
                    }
                    if (currentSideOffset > maxOffsetRange)
                    {
                        currentSideOffset = maxOffsetRange;
                    }
                }

                for (int x = 0; x < sideWidth + WorldGen.genRand.Next(-maxSizeOffset, maxSizeOffset + 1); x++)
                {
                    Point16 newPosition = position + new Point16(caveWidth, 0);
                    Point16 tilePosition = newPosition + new Point16(-x - currentSideOffset, y);

                    if ((y > 1 && y < caveHeight - 2) || WorldGen.genRand.NextBool())
                    {
                        WorldGen.KillTile(tilePosition.X, tilePosition.Y);
                        WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                        WorldGen.PlaceWall(tilePosition.X, tilePosition.Y, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                }
            }

            for (int i = 0; i < WorldGen.genRand.Next(5, 8) * sizeMultiplier; i++)
            {
                Point16 platformPosition = new Point16(position.X, position.Y + WorldGen.genRand.Next((int)(15 * sizeMultiplier), caveHeight - (int)(15 * sizeMultiplier)));
                int platformLength = (int)(WorldGen.genRand.Next(20, 45) * sizeMultiplier);

                for (int i2 = 0; i2 < platformLength; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(i2, 0);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }

                for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(i2, -1);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }

                for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(i2, 1);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }
            }

            for (int i = 0; i < WorldGen.genRand.Next(5, 8) * sizeMultiplier; i++)
            {
                Point16 platformPosition = new Point16(position.X + caveWidth, position.Y + WorldGen.genRand.Next((int)(15 * sizeMultiplier), caveHeight - (int)(15 * sizeMultiplier)));
                int platformLength = (int)(WorldGen.genRand.Next(20, 35) * sizeMultiplier);

                for (int i2 = 0; i2 < platformLength; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(-i2, 0);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }

                for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(-i2, -1);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }

                for (int i2 = 0; i2 < platformLength - WorldGen.genRand.Next(5, 15) * sizeMultiplier; i2++)
                {
                    Point16 tilePosition = platformPosition + new Point16(-i2, 1);
                    WorldGen.PlaceTile(tilePosition.X, tilePosition.Y, ModContent.TileType<Tiles.Deimostone>());
                }
            }

            int side = 1;
            if (WorldGen.genRand.NextBool())
            {
                side = -1;
            }

            Point16 chestPosition = new Point16(caveCenterX + (caveWidth / 2 - 10) * side, position.Y + WorldGen.genRand.Next(10, caveHeight - 10));
            deimostoneChests.Add(chestPosition);

            //generate second chest
            side *= -1;
            chestPosition = new Point16(caveCenterX + (caveWidth / 2 - 10) * side, position.Y + WorldGen.genRand.Next(10, caveHeight - 10));
            deimostoneChests.Add(chestPosition);

            //add tunnels to the sides
            int tunnelY;
            int tunnelHeight;
            int tunnelCeilingWidth;

            for (int i = 0; i < WorldGen.genRand.Next(4); i++)
            {
                tunnelY = position.Y + WorldGen.genRand.Next(25, caveHeight - 25);
                tunnelHeight = (int)(WorldGen.genRand.Next(4, 7) * sizeMultiplier);
                tunnelCeilingWidth = (int)(2 * sizeMultiplier);
                int startingX = (int)(position.X + sideWidth + Main.rand.Next(2, 4) * sizeMultiplier);
                int yOffset = 0;

                for (int x = 0; x < sideWidth + WorldGen.genRand.Next(4, 9) * sizeMultiplier; x++)
                {
                    int currentX = startingX - x;
                    int currentY = tunnelY - tunnelHeight / 2;
                    yOffset += WorldGen.genRand.Next(-1, 2);
                    if (yOffset > 3)
                    {
                        yOffset = 3;
                    }
                    if (yOffset < -3)
                    {
                        yOffset = -3;
                    }

                    for (int i2 = 0; i2 < tunnelHeight; i2++)
                    {
                        WorldGen.KillTile(currentX, currentY + i2 + yOffset);
                        if (i2 <= tunnelCeilingWidth || i2 >= tunnelHeight - tunnelCeilingWidth)
                        {
                            WorldGen.PlaceTile(currentX, currentY + i2 + yOffset, ModContent.TileType<Tiles.Deimostone>());
                        }
                        WorldGen.PlaceWall(currentX, currentY + i2 + yOffset, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                }
            }

            for (int i = 0; i < WorldGen.genRand.Next(4); i++)
            {
                tunnelY = position.Y + WorldGen.genRand.Next(25, caveHeight - 25);
                tunnelHeight = (int)(WorldGen.genRand.Next(4, 7) * sizeMultiplier);
                tunnelCeilingWidth = (int)(2 * sizeMultiplier);
                int startingX = (int)(position.X + caveWidth - sideWidth - Main.rand.Next(2, 4) * sizeMultiplier);
                int yOffset = 0;

                for (int x = 0; x < sideWidth + WorldGen.genRand.Next(4, 9) * sizeMultiplier; x++)
                {
                    int currentX = startingX + x;
                    int currentY = tunnelY - tunnelHeight / 2;
                    yOffset += WorldGen.genRand.Next(-1, 2);
                    if (yOffset > 3)
                    {
                        yOffset = 3;
                    }
                    if (yOffset < -3)
                    {
                        yOffset = -3;
                    }

                    for (int i2 = 0; i2 < tunnelHeight; i2++)
                    {
                        WorldGen.KillTile(currentX, currentY + i2 + yOffset);
                        if (i2 <= tunnelCeilingWidth || i2 >= tunnelHeight - tunnelCeilingWidth)
                        {
                            WorldGen.PlaceTile(currentX, currentY + i2 + yOffset, ModContent.TileType<Tiles.Deimostone>());
                        }
                        WorldGen.PlaceWall(currentX, currentY + i2 + yOffset, ModContent.WallType<Tiles.DeimostoneWallTile>());
                    }
                }
            }
        }

        public void GenerateDeimostoneChests()
        {
            while (deimostoneChests.Count > 0)
            {
                GenerateDeimostoneChest(deimostoneChests[0].X, deimostoneChests[0].Y);
                deimostoneChests.RemoveAt(0);
            }
        }

        public void GenerateDeimostoneChest(int x, int y)
        {
            WorldGen.KillTile(x, y);
            WorldGen.KillTile(x + 1, y);
            WorldGen.KillTile(x + 1, y + 1);
            WorldGen.KillTile(x, y + 1);
            WorldGen.KillTile(x, y + 2);
            WorldGen.KillTile(x + 1, y + 2);
            WorldGen.PlaceTile(x, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x + 1, y + 2, ModContent.TileType<Tiles.Deimostone>());
            WorldGen.PlaceTile(x, y + 1, (ushort)ModContent.TileType<Tiles.DeimostoneChestTile>());
            int chest = Chest.FindChest(x, y);
        }

        public void SelectBountyBiome()
        {
            CurrentBountyBiome = Main.rand.Next(BountyBiomeCount);
            //0 = Underground desert
            //1 = Underground snow
            //2 = Underworld
            //3 = Jungle
            //4 = Underground Jungle;
            //5 = World Evil
            //6 = Snow

            //HARDMODE ONLY:
            //7 = Hallowed
            //8 = Underground Hallowed
            //9 = Underground Evil
        }

        public void NewDayActions()
        {
            SelectBountyBiome();
        }

        public void OnRainStart()
        {
            if (!terrorRain && Main.rand.NextFloat() <= 0.2f && Main.hardMode)
            {
                terrorRain = true;
                Main.NewText("Dark rain begins to fall from the sky!", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
            }
        }

        void SetTerrorAbilityPositions()
        {
            ShriekOfHorror = new Vector2(Main.spawnTileX, Main.spawnTileY);
            float sizeMultiplier = 1;
            if (Main.maxTilesX == 6400)
            {
                sizeMultiplier = 1.5f;
            }
            if (Main.maxTilesX == 8400)
            {
                sizeMultiplier = 2f;
            }
            ShriekOfHorror.Y += 285 * sizeMultiplier;
            ShriekOfHorror.X -= 15;
            ShriekOfHorror.X += 35;
            ShriekOfHorror.Y += 11.5f;
            ShriekOfHorror *= 16;

            int DungeonDirection = 1;
            if (Main.dungeonX < Main.spawnTileX)
            {
                DungeonDirection = -1;
            }
            HorrificAdaptation = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
            HorrificAdaptation *= 16;
        }

        bool wasRaining = true;
        int thunderCounter = -69;
        public override void PostUpdate()
        {
            if (thunderCounter == -69)
            {
                thunderCounter = Main.rand.Next(60 * 4, 60 * 45);
            }

            if (terrorRain)
            {
                Main.rainTexture = ModContent.GetTexture("TerrorbornMod/TerrorRain");

                if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>()))
                {
                    timeSinceFrightcrawlerSpawn = 0;
                }

                if (!Main.LocalPlayer.dead && Main.LocalPlayer.ZoneRain)
                {
                    timeSinceFrightcrawlerSpawn++;
                    if (timeSinceFrightcrawlerSpawn >= 3600 * 2)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 0, 1, -0.3f);
                        NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
                    }
                    else if (timeSinceFrightcrawlerSpawn >= 3600f * 1f && Main.rand.NextFloat() <= 0.0002f)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)Main.LocalPlayer.Center.X, (int)Main.LocalPlayer.Center.Y, 0, 1, -0.3f);
                        NPC.SpawnOnPlayer(Main.myPlayer, ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
                    }
                }

                thunderCounter--;
                if (thunderCounter <= 0)
                {
                    thunderCounter = Main.rand.Next(60 * 4, 60 * 45);
                    TerrorbornMod.TerrorThunder();
                }
            }
            else
            {
                Main.rainTexture = ModContent.GetTexture("Terraria/Rain");
            }

            //Vector2 textVector = VoidBlink * 16 - Main.player[Main.myPlayer].position;
            //Main.NewText(textVector.X + ", " + textVector.Y);
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            SetTerrorAbilityPositions();

            bool SpawnShriek = !obtainedShriekOfHorror;
            bool SpawnHA = !modPlayer.unlockedAbilities.Contains(1);
            bool SpawnNC = !modPlayer.unlockedAbilities.Contains(4);
            bool SpawnVB = !modPlayer.unlockedAbilities.Contains(3);
            bool SpawnTW = !modPlayer.unlockedAbilities.Contains(2);
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active)
                {
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.ShriekOfHorror>())
                    {
                        SpawnShriek = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.HorrificAdaptation>())
                    {
                        SpawnHA = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.NecromanticCurse>())
                    {
                        SpawnNC = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.VoidBlink>())
                    {
                        SpawnVB = false;
                    }
                    if (Main.projectile[i].type == ModContent.ProjectileType<Abilities.TerrorWarp>())
                    {
                        SpawnTW = false;
                    }
                }
            }
            if (!obtainedShriekOfHorror)
            {
                SpawnHA = false;
                SpawnNC = false;
                SpawnVB = false;
                SpawnTW = false;
            }
            if (SpawnShriek)
            {
                Projectile.NewProjectile(ShriekOfHorror, Vector2.Zero, ModContent.ProjectileType<Abilities.ShriekOfHorror>(), 0, 0);
            }
            if (SpawnHA)
            {
                Projectile.NewProjectile(HorrificAdaptation, Vector2.Zero, ModContent.ProjectileType<Abilities.HorrificAdaptation>(), 0, 0);
            }
            if (SpawnNC)
            {
                Projectile.NewProjectile(HorrificAdaptation, Vector2.Zero, ModContent.ProjectileType<Abilities.NecromanticCurse>(), 0, 0);
            }
            if (SpawnVB)
            {
                Projectile.NewProjectile(VoidBlink, Vector2.Zero, ModContent.ProjectileType<Abilities.VoidBlink>(), 0, 0);
            }
            if (SpawnTW)
            {
                Projectile.NewProjectile(TerrorWarp, Vector2.Zero, ModContent.ProjectileType<Abilities.TerrorWarp>(), 0, 0);
            }

            if (CurrentBountyBiome == 69)
            {
                SelectBountyBiome();
            }
            if (Main.hardMode)
            {
                BountyBiomeCount = 10;
            }
            if (Main.dayTime && WasNight)
            {
                WasNight = false;
                NewDayActions();
            }
            if (!Main.dayTime)
            {
                WasNight = true;
            }

            if (Main.raining && !wasRaining)
            {
                wasRaining = true;
                OnRainStart();
            }
            if (!Main.raining)
            {
                if (wasRaining)
                {
                    wasRaining = false;
                }
                if (terrorRain)
                {
                    Main.NewText("The sky begins to clear...", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    downedTerrorRain = true;
                    terrorRain = false;
                }
            }

            if (SkeletonSheriffName == "")
            {
                SkeletonSheriffName = getSkeletonSheriffName();
            }
        }

        public void GenerateAzuriteOres()
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05); k++)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);
                WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(4, 7), WorldGen.genRand.Next(3, 6), mod.TileType("Azurite"), false, 0f, 0f, false, true);
            }
        }
    }
}
