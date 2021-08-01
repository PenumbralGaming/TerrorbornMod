using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SpiderNecklace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("-10 defense" +
                "\nIncreases max minions by 1" +
                "\n10% increased minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.noMelee = true;
            item.lifeRegen = 5;
            item.rare = 5;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.useAnimation = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SpiderFang, 8);
            recipe.AddIngredient(ItemID.SoulofFright, 10);
            recipe.AddIngredient(ItemID.Cobweb, 100);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense -= 10;
            player.maxMinions++;
            player.minionDamage += 0.1f;
        }
    }
}
