using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AntsMandible : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Royal Mandible");
            Tooltip.SetDefault("Grants 3 armor penetration and increased movement speed");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 3;
            player.runAcceleration += 0.065f;
        }
    }
}
