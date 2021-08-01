using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class TenebrisWings : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 10);
            recipe.AddIngredient(ItemID.HallowedBar, 5);
            recipe.AddIngredient(ItemID.SoulofFlight, 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall" +
                "\nHold DOWN to fall faster");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 2.75f);
            player.armorEffectDrawShadow = true;
            if (player.controlDown)
            {
                player.maxFallSpeed *= 10f;
            }
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            player.armorEffectDrawShadow = true;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 1.3f;
            ascentWhenRising = 0.14f;
            maxCanAscendMultiplier = 1.7f;
            maxAscentMultiplier = 2f;
            constantAscend = 0.1f;
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            acceleration *= 1.4f;
            speed *= 1.35f;
        }
    }
}

