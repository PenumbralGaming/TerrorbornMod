using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.TreasureBags
{
    class TT_TreasureBag : ModItem
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
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.TidalTitanMask>());
            }
            player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Accessories.LunarHeart>());
            player.QuickSpawnItem(mod.ItemType("CrackedShell"), Main.rand.Next(8, 12));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(mod.ItemType("BubbleBow"));
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(mod.ItemType("TidalClaw"), 750);
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(mod.ItemType("SightForSoreEyes"));
            }
        }

        public override int BossBagNPC => mod.NPCType("TidalTitan");
    }
}

