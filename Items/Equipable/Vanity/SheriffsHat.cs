using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Vanity
{
    [AutoloadEquip(EquipType.Head)]
    public class SheriffsHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sheriff's Hat");
            Tooltip.SetDefault("Don't worry, he has another one");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }
    }
}

