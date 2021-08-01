using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;

namespace TerrorbornMod.Items.TestItems
{
    class ToggleGoldenTooth : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toggle Golden Tooth");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.GoldenTooth>() + "]");
        }
        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.GoldenTooth)
            {
                modPlayer.GoldenTooth = true;
                Main.NewText("Golden Tooth toggled on");

            }
            else
            {
                modPlayer.GoldenTooth = false;
                Main.NewText("Golden Tooth toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}


