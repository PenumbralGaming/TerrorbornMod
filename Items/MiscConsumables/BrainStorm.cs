using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    class BrainStorm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Consumable" +
                "\nStarts the astraphobia event, or ends it if it's already going on" +
                "\nCan only be used during rain");
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
            recipe.AddIngredient(ItemID.SoulofNight, 3);
            recipe.AddIngredient(ItemID.SoulofLight, 3);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 2);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            if (Main.raining)
            {
                if (TerrorbornWorld.terrorRain)
                {
                    Main.NewText("The sky begins to brighten up again...", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    TerrorbornWorld.terrorRain = false;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                }
                else
                {
                    Main.NewText("Dark rain begins to fall from the sky!", Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255));
                    TerrorbornWorld.terrorRain = true;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)player.Center.X, (int)player.Center.Y, 0);
                }
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
