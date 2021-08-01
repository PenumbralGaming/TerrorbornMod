using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.DeveloperSets
{
    [AutoloadEquip(EquipType.Body)]
    public class CrimsanesRobes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CR1MS4NE's Robes");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
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
    public class CrimsanesLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CR1MS4NE's Leg Robes");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            item.rare = 9;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override bool DrawLegs()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class CrimsanesHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("CR1MS4NE's Hood");
            Tooltip.SetDefault("[c/8A9B98:Great for impersonating Terrorborn devs!]");
        }

        public override void SetDefaults()
        {
            item.rare = 9;
            item.vanity = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override bool DrawHead()
        {
            return false;
        }
    }
}

