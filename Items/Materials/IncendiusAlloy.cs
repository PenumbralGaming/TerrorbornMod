using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Materials
{
    class IncendiusAlloy : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incendiary Alloy");
            Tooltip.SetDefault("'A shattered fragment of the seal, brought back to life by some unholy curse'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 69, 0);
            item.rare = 3;
            item.width = 18;
            item.height = 18;
        }
    }
}

