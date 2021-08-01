using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HermesFeather : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermes' Feather");
            Tooltip.SetDefault("Allows you to use Shriek of Horror quicker");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = 1;
            item.value = Item.sellPrice(0, 0, 75, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(this);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 3);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(ItemID.HermesBoots);
            recipe.AddRecipe();
        }
    }
}
