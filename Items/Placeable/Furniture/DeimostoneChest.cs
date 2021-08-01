using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Furniture
{
    class DeimostoneChest : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.consumable = true;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.createTile = ModContent.TileType<Tiles.DeimostoneChestTile>();
		}
	}
}
