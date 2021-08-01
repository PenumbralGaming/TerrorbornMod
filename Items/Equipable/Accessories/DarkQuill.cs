using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DarkQuill : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dark Quill");
            Tooltip.SetDefault("The quill of a scholar who studied the properties of terror" +
                "\nAllows you to move at 50% speed while using Shriek of Horror, but doing so will hurt you 50% more");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = 1;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekOfHorrorMovement += 0.5f;
            modPlayer.ShriekPain += 0.5f;
        }
    }
}

