using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Materials
{
    class SanguineFang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine fang");
            Tooltip.SetDefault("'All shall be consumed, let none survive'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 15, 0);
            item.rare = 2;
            item.width = 28;
            item.height = 24;
        }
    }
}
