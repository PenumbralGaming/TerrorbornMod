using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MandibleNecklace : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SharkToothNecklace);
            recipe.AddIngredient(ModContent.ItemType<AntsMandible>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mandible Necklace");
            Tooltip.SetDefault("Grants 8 armor penetration and increased movement speed");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = 3;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 8;
            player.runAcceleration += 0.065f;
        }
    }
}
