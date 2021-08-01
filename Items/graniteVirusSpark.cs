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
    class graniteVirusSpark : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Granite, 65);
            recipe.AddIngredient(ItemID.Wire, 175);
            recipe.AddIngredient(ItemID.Actuator, 50);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Granite Virus");
            Tooltip.SetDefault("Turns you into a precise granite spark for 5 seconds\n" +
                "While transformed, you can't use items and you'll take 50% more damage\n" +
                "You can also press the 'Quick Spark' hotkey to transform as long as the item is in your inventory\n" +
                "30 second cooldown");
        }
        public override void SetDefaults()
        {
            item.rare = 3;
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
