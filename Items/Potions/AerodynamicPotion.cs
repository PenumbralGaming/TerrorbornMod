using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class AerodynamicPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Grants immunity to heavy wind, and increases movement speed" +
                "\nIncreases wing flight time by 100%");
        }
        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 2;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 1;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.aerodynamic>();
            item.buffTime = 18000;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(mod.ItemType("AzuriteOre"), 3);
            recipe1.AddIngredient(ItemID.Blinkroot);
            recipe1.AddIngredient(ItemID.Feather, 2);
            recipe1.AddIngredient(ItemID.Cloud, 3);
            recipe1.AddIngredient(ItemID.BottledWater);
            recipe1.AddTile(TileID.Bottles);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
}
