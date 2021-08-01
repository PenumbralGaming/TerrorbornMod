using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class SangoonBand : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sangoon Band");
            Tooltip.SetDefault("Increases life regen" +
                "\nDealing crits heals you for 1 hp");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.useAnimation = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(mod.ItemType("SanguineFang"), 10);
            recipe1.AddIngredient(ItemID.TissueSample, 5);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(mod.ItemType("SanguineFang"), 10);
            recipe2.AddIngredient(ItemID.ShadowScale, 5);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 3;
            modPlayer.SangoonBand = true;
        }
    }
}
