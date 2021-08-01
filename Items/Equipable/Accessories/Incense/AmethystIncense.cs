using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class AmethystIncense : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Amethyst, 5);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreased enemy defense by 2." +
                "\nYou get an extra defense for every 3 enemies nearby.");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 44;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.value = Item.sellPrice(gold: 1, silver: 25);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 2;
            int enemyCount = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.CanBeChasedBy())
                {
                    enemyCount++;
                }
            }
            player.statDefense += enemyCount / 3;
        }
    }
}
