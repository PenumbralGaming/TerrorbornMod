using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Materials
{
    class NoxiousScale : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("The scale of a powerful monster, likely one that consumed a large amount of souls at once");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 25, 0);
            item.rare = 5;
            item.width = 18;
            item.height = 20;
        }
    }
}
