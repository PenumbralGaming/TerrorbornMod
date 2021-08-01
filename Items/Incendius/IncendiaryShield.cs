using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiaryShield : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Melee damage has an 8% chance to cause an explosion" +
                "\nThis chance is boosted by half your melee critical strike chance" +
                "\nThis explosion will damage the hit enemy and all nearby hit enemies for the same amount" +
                "\nof damage the original hit did" +
                "\nThis will additionally inflict enemies with a random type of fire for 5 seconds");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe.AddIngredient(ItemID.CobaltBar, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe2.AddIngredient(ItemID.PalladiumBar, 20);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 4;
            item.defense = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.IncendiaryShield = true;
        }
    }
}

