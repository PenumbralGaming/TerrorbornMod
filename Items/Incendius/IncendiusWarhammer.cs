using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusWarhammer : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe.AddIngredient(ItemID.CobaltBar, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe2.AddIngredient(ItemID.PalladiumBar, 20);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetDefaults()
        {
            item.damage = 33;
            item.melee = true;
            item.width = 64;
            item.height = 64;
            item.useTime = 4;
            item.useAnimation = 17;
            item.hammer = 75;
            item.useStyle = 1;
            item.knockBack = 11;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
    }
}
