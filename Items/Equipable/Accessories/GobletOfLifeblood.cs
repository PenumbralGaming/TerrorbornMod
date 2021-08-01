using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class GobletOfLifeblood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Goblet Of Lifeblood");
            Tooltip.SetDefault("The first hit on an enemy will always be critical" +
                "\nKilling enemies has an increased chance to drop hearts" +
                "\nDealing crits heals you for 1 hp" + 
                "\nIncreases life regen");
        }

        public override void SetDefaults()
        {
            item.width = 36;
            item.height = 36;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 4;
            item.value = Item.sellPrice(gold: 4, silver: 25);
            item.useAnimation = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(mod.ItemType("SanguineFang"), 4);
            recipe1.AddIngredient(ModContent.ItemType<Items.Equipable.Accessories.SangoonBand>());
            recipe1.AddIngredient(ModContent.ItemType<Items.Equipable.Accessories.TheLiesOfNourishment>());
            recipe1.AddIngredient(ItemID.SoulofNight, 5);
            recipe1.AddTile(TileID.TinkerersWorkbench);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.lifeRegen += 3;
            modPlayer.SangoonBand = true;
            modPlayer.LiesOfNourishment = true;
        }
    }
}
