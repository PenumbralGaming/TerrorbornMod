//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using Terraria;
//using Terraria.ID;
//using Terraria.Localization;
//using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
//using Terraria.WorldBuilding;
//using Microsoft.Xna.Framework;
//using Terraria.GameContent.Generation;
//using Terraria.ModLoader.IO;
//using Terraria.DataStructures;
//using Microsoft.Xna.Framework.Graphics;
//using StructureHelper;

//namespace TerrorbornMod.Items.TestItems
//{
//    class GenerateTerrorShrines : ModItem
//    {
//        public override string Texture => "TerrorbornMod/placeholder";
//        public override void SetStaticDefaults()
//        {
//            Tooltip.SetDefault("Generates the terror shrines in a pre-made world");
//        }
//        public override void SetDefaults()
//        {
//            item.rare = -12;
//            item.autoReuse = false;
//            item.useStyle = ItemUseStyleID.HoldingUp;
//            item.useTime = 20;
//            item.useAnimation = 20;
//        }
//        public override bool CanUseItem(Player player)
//        {
//            Vector2 ShriekOfHorrorPosition = new Vector2(Main.spawnTileX, Main.spawnTileY);
//            float sizeMultiplier = 1;
//            if (Main.maxTilesX == 6400)
//            {
//                sizeMultiplier = 1.5f;
//            }
//            if (Main.maxTilesX == 8400)
//            {
//                sizeMultiplier = 2f;
//            }
//            ShriekOfHorrorPosition.Y += 285 * sizeMultiplier;
//            ShriekOfHorrorPosition.X -= 15;

//            StructureHelper.Generator.GenerateStructure("Structures/ShriekOfHorrorShrine", new Point16((int)ShriekOfHorrorPosition.X, (int)ShriekOfHorrorPosition.Y), mod);

//            int DungeonDirection = 1;
//            if (Main.dungeonX < Main.maxTilesX / 2)
//            {
//                DungeonDirection = -1;
//            }
//            Vector2 HorrificAdaptationPosition = new Vector2(Main.spawnTileX + (Main.maxTilesX / 4) * -DungeonDirection, Main.maxTilesY / 2);
//            StructureHelper.Generator.GenerateStructure("Structures/HorrificAdaptationShrine", new Point16((int)HorrificAdaptationPosition.X, (int)HorrificAdaptationPosition.Y), mod);

//            TerrorbornWorld.VoidBlink = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.95f);
//            StructureHelper.Generator.GenerateStructure("Structures/VoidBlinkShrine", new Point16((int)TerrorbornWorld.VoidBlink.X, (int)TerrorbornWorld.VoidBlink.Y), mod);

//            TerrorbornWorld.TerrorWarp = new Vector2(WorldGen.genRand.Next(50, Main.maxTilesX - 50), Main.maxTilesY * 0.66f);
//            StructureHelper.Generator.GenerateStructure("Structures/TerrorWarpShrine", new Point16((int)TerrorbornWorld.TerrorWarp.X, (int)TerrorbornWorld.TerrorWarp.Y), mod);
//            return base.CanUseItem(player);
//        }
//    }
//}


