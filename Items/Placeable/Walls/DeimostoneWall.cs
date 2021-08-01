using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Walls
{
    public class DeimostoneWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Stone infused with terror after many years of intense exposure");
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
            item.useTime = 7;
            item.useStyle = 1;
            item.consumable = true;
            item.createWall = ModContent.WallType<Tiles.DeimostoneWallTile>();
        }


        public override void ExtractinatorUse(ref int resultType, ref int resultStack)
        {
            if (Main.rand.Next(5) == 0)
            {
                resultType = ModContent.ItemType<DarkEnergy>();
                if (Main.rand.Next(5) == 0)
                {
                    resultStack = 1;
                }
            }
        }
    }
}


