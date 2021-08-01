using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Materials
{
    class ThunderShard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A fragment of one of the souls released by killing the Wall of Flesh" +
                "\nWhen torn apart, the soul seems to have gained an electric charge");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 20, 0);
            item.rare = 5;
            item.width = 18;
            item.height = 18;
        }
    }
}

