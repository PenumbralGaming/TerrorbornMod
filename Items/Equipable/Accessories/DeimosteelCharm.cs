using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DeimosteelCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases the amount of bleaks that spawn in deimostone caves" +
                "\nDoubles the chance that certain enemies will drop terror samples");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.useAnimation = 5;
            item.rare = 1;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.DeimosteelCharm = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 4);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 1);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}


