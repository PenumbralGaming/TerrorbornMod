using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Materials
{
    class CrackedShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Fragment of a tidal titan's shell, capable of withstanding much of the strongest aquatic pressures'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.value = Item.sellPrice(0, 0, 10, 0);
            item.rare = 2;
        }
        public override void AddRecipes()
        {
            ModRecipe chest = new ModRecipe(mod);
            chest.AddIngredient(this, 3);
            chest.AddTile(TileID.Anvils);
            chest.SetResult(mod.ItemType("TidalShellChestplate"));
            chest.AddRecipe();
            ModRecipe helm = new ModRecipe(mod);
            helm.AddIngredient(this, 1);
            helm.AddTile(TileID.Anvils);
            helm.SetResult(mod.ItemType("TidalShellHeadplate"));
            helm.AddRecipe();
            ModRecipe legs = new ModRecipe(mod);
            legs.AddIngredient(this, 2);
            legs.AddTile(TileID.Anvils);
            legs.SetResult(mod.ItemType("TidalShellLegwear"));
            legs.AddRecipe();
            ModRecipe claws = new ModRecipe(mod);
            claws.AddIngredient(this, 1);
            claws.AddTile(TileID.Anvils);
            claws.SetResult(mod.ItemType("TidalClaw"), 250);
            claws.AddRecipe();
            ModRecipe Bow = new ModRecipe(mod);
            Bow.AddIngredient(this, 3);
            Bow.AddTile(TileID.Anvils);
            Bow.SetResult(mod.ItemType("BubbleBow"));
            Bow.AddRecipe();
            ModRecipe Wand = new ModRecipe(mod);
            Wand.AddIngredient(this, 3);
            Wand.AddTile(TileID.Anvils);
            Wand.SetResult(mod.ItemType("SightForSoreEyes"));
            Wand.AddRecipe();
        }
    }
}
