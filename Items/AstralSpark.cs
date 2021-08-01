using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items
{
    class AstralSpark : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<graniteVirusSpark>());
            recipe.AddIngredient(ModContent.ItemType<Materials.NoxiousScale>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 25);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Astral Spark");
            Tooltip.SetDefault("Turns you into a precise astral spark for 5 seconds" +
                "\nWhile transformed, you can't use items" +
                "\nYou can also press the 'Quick Spark' hotkey to transform as long as the item is in your inventory" +
                "\nWhile transformed, hold JUMP to move at insanely fast speed" +
                "\n20 second cooldown");
        }
        public override void SetDefaults()
        {
            item.rare = 5;
            item.useTime = 1;
            item.useAnimation = 1;
            item.useStyle = ItemUseStyleID.Stabbing;
            item.noUseGraphic = true;
            item.autoReuse = false;
            item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override bool UseItem(Player player)
        {
            astralSparkData.Transform(player);
            return base.UseItem(player);
        }
    }
}

