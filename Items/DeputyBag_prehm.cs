using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items
{
    class DeputyBag_prehm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deputy's bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}" +
                "\nContains useful pre-hardmode loot to help you along your journey");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 20;
            item.height = 32;
            item.rare = 2;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        
        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ItemID.LesserHealingPotion, Main.rand.Next(11, 19));
            player.QuickSpawnItem(ItemID.HealingPotion, Main.rand.Next(8, 12));
            player.QuickSpawnItem(ItemID.WoodenArrow, Main.rand.Next(250, 501));
            player.QuickSpawnItem(ItemID.MusketBall, Main.rand.Next(150, 301));
            player.QuickSpawnItem(ItemID.SilverBullet, Main.rand.Next(50, 150));
            player.QuickSpawnItem(ItemID.ManaPotion, Main.rand.Next(15, 26));

            //ores
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.CopperOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.TinOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.IronOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.LeadOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.SilverOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.TungstenOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.GoldOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.PlatinumOre, Main.rand.Next(35, 60));
            }

            if (WorldGen.crimson)
            {
                player.QuickSpawnItem(ItemID.DemoniteOre, Main.rand.Next(35, 60));
                player.QuickSpawnItem(ItemID.ShadowScale, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(ItemID.Musket);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(ItemID.ShadowOrb);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(ItemID.Vilethorn);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(ItemID.BallOHurt);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(ItemID.BandofStarpower);
                    }
                }
            }
            else
            {
                player.QuickSpawnItem(ItemID.CrimtaneOre, Main.rand.Next(35, 60));
                player.QuickSpawnItem(ItemID.TissueSample, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(ItemID.TheUndertaker);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(ItemID.CrimsonHeart);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(ItemID.PanicNecklace);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(ItemID.CrimsonRod);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(ItemID.TheRottedFork);
                    }
                }
            }
        }
    }
}
