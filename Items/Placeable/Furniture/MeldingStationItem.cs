using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Furniture
{
	class MeldingStationItem : ModItem
	{
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Melding Station");
            Tooltip.SetDefault("Can be used to craft with deimosteel and create restless weapons");
        }
        public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
            item.rare = 1;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<Tiles.MeldingStation>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.DeimosteelBar>(), 12);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

