using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class DeimosteelOreItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deimosteel Ore");
            Tooltip.SetDefault("Usually found near Deimostone, but can occasionally be found throughout the caverns");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 14;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.rare = 1;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.DeimosteelOre>();
        }
    }
}

