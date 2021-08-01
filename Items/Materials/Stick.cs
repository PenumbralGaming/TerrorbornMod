using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Materials
{
    class Stick : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stick");
            Tooltip.SetDefault("It's just a stick...");
        }
        public override void SetDefaults()
        {
            item.maxStack = 1;
            item.melee = true;
            item.damage = 2;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.rare = -1;
        }
    }
}

