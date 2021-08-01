using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Materials
{
    public class AzuriteOre : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Water condensed by the constantly shifting world, merged into the other minerals'");
            ItemID.Sets.ExtractinatorMode[item.type] = item.type;
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("Azurite");
            item.value = Item.sellPrice(0, 0, 8, 0);
        }


        public override void ExtractinatorUse(ref int resultType, ref int resultStack)
        {
            if (Main.rand.Next(100) == 0)
            {
                resultType = ItemID.FrozenKey;
                if (Main.rand.Next(5) == 0)
                {
                    resultStack = 1;
                }
            }
        }
    }
}
