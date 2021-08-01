using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Vanity.BossMasks
{
    [AutoloadEquip(EquipType.Head)]
    public class ShadowcrawlerMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadowcrawler Mask");
        }

        public override void SetDefaults()
        {
            item.rare = ItemRarityID.Blue;
            item.vanity = true;
        }
    }
}

