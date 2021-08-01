using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class TheLiesOfNourishment : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Lies of Nourishment");
            Tooltip.SetDefault("The first hit on an enemy will always be critical" +
                "\nKilling enemies has an increased chance to drop hearts");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 36;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.value = Item.sellPrice(0, 3, 75, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.LiesOfNourishment = true;
        }
    }
}
