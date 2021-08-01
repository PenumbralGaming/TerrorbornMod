using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.TreasureBags
{
    class SC_TreasureBag : ModItem
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
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Vanity.BossMasks.ShadowcrawlerMask>());
            }
            player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Accessories.DarkAbdomen>());
            player.QuickSpawnItem(ModContent.ItemType<Items.Materials.SoulOfPlight>(), Main.rand.Next(25, 41));
            int choice = Main.rand.Next(3);
            if (choice == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Shadowcrawler.BladeOfShade>());
            }
            else if (choice == 1)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Shadowcrawler.Nightbrood>());
            }
            else if (choice == 2)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Shadowcrawler.BoiledBarrageWand>());
            }
            int armorChoice = Main.rand.Next(3);
            if (armorChoice == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
            }
            if (armorChoice == 1)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
            }
            if (armorChoice == 2)
            {
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                player.QuickSpawnItem(ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
            }
        }

        public override int BossBagNPC => mod.NPCType("Shadowcrawler");
    }
}


