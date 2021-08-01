using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using System.Collections.Generic;
using Terraria.Utilities;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.TreasureBags
{
    class DS_TreasureBag : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treasure Bag");
            Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.consumable = true;
            item.width = 24;
            item.height = 24;
            item.expert = true;
        }

        public override bool CanRightClick()
        {
            return true;
        }
        public override void OpenBossBag(Player player)
        {
            if (Main.rand.Next(7) == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.DunestockMask>());
            }
            int choice = Main.rand.Next(2);
            if (choice == 0)
            {
                player.QuickSpawnItem(mod.ItemType("Dunesting"));
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(mod.ItemType("NeedleClawStaff"));
            }
            choice = Main.rand.Next(2);
            int item1;
            WeightedRandom<int> itemlist = new WeightedRandom<int>();
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.DryScarf>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.AntlionShell>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.AntlionClaw>());
            itemlist.Add(ModContent.ItemType<Equipable.Accessories.Wings.AntlionWings>(), 1.5f);
            item1 = itemlist.Get();
            player.QuickSpawnItem(item1);
            int item2 = itemlist.Get();
            while (item2 == item1)
            {
                item2 = itemlist.Get();
            }
            player.QuickSpawnItem(item2);
            player.QuickSpawnItem(ModContent.ItemType<Equipable.Accessories.CloakOfTheWind>());
            player.QuickSpawnItem(ModContent.ItemType<DustwindRod>());
        }

        public override int BossBagNPC => mod.NPCType("Dunestock");
    }
}


