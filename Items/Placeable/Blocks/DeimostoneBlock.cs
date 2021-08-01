using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace TerrorbornMod.Items.Placeable.Blocks
{
    public class DeimostoneBlock : ModItem
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
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.Deimostone>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(this);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.SetResult(ModContent.ItemType<Walls.DeimostoneWall>(), 4);
            recipe2.AddRecipe();
            ModRecipe recipe3 = new ModRecipe(mod);
            recipe3.AddIngredient(ModContent.ItemType<Walls.DeimostoneWall>(), 4);
            recipe3.AddTile(TileID.WorkBenches);
            recipe3.SetResult(this);
            recipe3.AddRecipe();
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

