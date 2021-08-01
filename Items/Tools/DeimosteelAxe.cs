using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Tools
{
    class DeimosteelAxe : ModItem
    {
        float terrorRequired = 1f;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Requires " + terrorRequired + "% terror per use" +
                "\nCuts insanely fast");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 8);
            recipe.AddRecipeGroup(RecipeGroupID.Wood, 3);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.damage = 4;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useAnimation = 15;
            item.useTime = 5;
            item.axe = 60 / 5;
            item.useStyle = 1;
            item.knockBack = 6;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorPercent >= terrorRequired)
            {
                modPlayer.TerrorPercent -= terrorRequired;
                return base.CanUseItem(player);
            }
            return false;
        }
    }
}

