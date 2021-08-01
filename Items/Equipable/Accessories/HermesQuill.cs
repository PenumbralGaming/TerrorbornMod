using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HermesQuill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hermes' Quill");
            Tooltip.SetDefault("Allows you to use Shriek of Horror quicker" +
                "\nAllows you to move at 50% speed while using Shriek of Horror, but doing so will hurt you 25% more than usual");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = 2;
            item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
            modPlayer.ShriekOfHorrorMovement += 0.5f;
            modPlayer.ShriekOfHorrorExtraDamageMultiplier *= 1.25f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<HermesFeather>());
            recipe.AddIngredient(ModContent.ItemType<DarkQuill>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
