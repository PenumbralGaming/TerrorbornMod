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
    class DeputyBag_hm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Peacekeeper's bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}" +
                "\nContains useful hardmode loot to help you along your journey");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 20;
            item.height = 32;
            item.rare = 4;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            player.QuickSpawnItem(ItemID.HealingPotion, Main.rand.Next(11, 19));
            player.QuickSpawnItem(ItemID.GreaterHealingPotion, Main.rand.Next(8, 12));
            player.QuickSpawnItem(ItemID.GreaterManaPotion, Main.rand.Next(15, 26));

            //ores
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.CobaltOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.PalladiumOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.MythrilOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.OrichalcumOre, Main.rand.Next(35, 60));
            }
            if (Main.rand.Next(2) == 0)
            {
                player.QuickSpawnItem(ItemID.TitaniumOre, Main.rand.Next(35, 60));
            }
            else
            {
                player.QuickSpawnItem(ItemID.AdamantiteOre, Main.rand.Next(35, 60));
            }

            player.QuickSpawnItem(ItemID.SoulofNight, Main.rand.Next(7, 11));
            player.QuickSpawnItem(ItemID.SoulofLight, Main.rand.Next(7, 11));
            player.QuickSpawnItem(ItemID.SoulofFlight, Main.rand.Next(18, 28));
            if (WorldGen.crimson)
            {
                player.QuickSpawnItem(ItemID.CursedArrow, Main.rand.Next(250, 501));
                player.QuickSpawnItem(ItemID.CursedBullet, Main.rand.Next(250, 501));
                player.QuickSpawnItem(ItemID.CursedFlame, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(ItemID.DartRifle);
                        player.QuickSpawnItem(ItemID.CursedDart, 150);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(ItemID.WormHook);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(ItemID.ChainGuillotines);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(ItemID.ClingerStaff);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(ItemID.PutridScent);
                    }
                }
            }
            else
            {
                player.QuickSpawnItem(ItemID.IchorArrow, Main.rand.Next(250, 501));
                player.QuickSpawnItem(ItemID.IchorBullet, Main.rand.Next(250, 501));
                player.QuickSpawnItem(ItemID.Ichor, Main.rand.Next(5, 11));
                if (Main.rand.Next(4) == 0)
                {
                    int choice = Main.rand.Next(5);
                    if (choice == 0)
                    {
                        player.QuickSpawnItem(ItemID.DartPistol);
                        player.QuickSpawnItem(ItemID.IchorDart, 150);
                    }
                    if (choice == 1)
                    {
                        player.QuickSpawnItem(ItemID.TendonHook);
                    }
                    if (choice == 2)
                    {
                        player.QuickSpawnItem(ItemID.FetidBaghnakhs);
                    }
                    if (choice == 3)
                    {
                        player.QuickSpawnItem(ItemID.SoulDrain);
                    }
                    if (choice == 4)
                    {
                        player.QuickSpawnItem(ItemID.FleshKnuckles);
                    }
                }
            }
            if (Main.rand.Next(4) == 0)
            {
                int choice = Main.rand.Next(5);
                if (choice == 0)
                {
                    player.QuickSpawnItem(ItemID.DaedalusStormbow);
                    player.QuickSpawnItem(ItemID.HolyArrow, 200);
                }
                if (choice == 1)
                {
                    player.QuickSpawnItem(ItemID.IlluminantHook);
                }
                if (choice == 2)
                {
                    player.QuickSpawnItem(ItemID.FlyingKnife);
                }
                if (choice == 3)
                {
                    player.QuickSpawnItem(ItemID.CrystalVileShard);
                }
                if (choice == 4)
                {
                    player.QuickSpawnItem(ItemID.CrystalSerpent);
                }
            }
        }
    }
}
