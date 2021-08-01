using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.DeveloperSets
{
    [AutoloadEquip(EquipType.Body)]
    public class PenumbralGamingsChestwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Chestwear");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 25;
            item.rare = 9;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
        public override bool DrawBody()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class PenumbralGamingsLegwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Legwear");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.rare = 9;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class PenumbralGamingsVisor : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penumbral's Visor");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.rare = 9;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
    }
}
