using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    class DarkSigil : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consumable" +
                "\nStarts a blood moon" +
                "\nCan only be used during the night");
        }

        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
            item.rare = 2;
            item.maxStack = 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShadowScale, 15);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.TissueSample, 15);
            recipe2.AddTile(TileID.DemonAltar);
            recipe2.SetResult(this, 1);
            recipe2.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            if (!Main.bloodMoon && !Main.dayTime)
            {
                Main.NewText("The sky begins to bleed...", Color.FromNonPremultiplied(175, 75, 255, 255));
                Main.bloodMoon = true;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                item.stack--;
            }
            else
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
