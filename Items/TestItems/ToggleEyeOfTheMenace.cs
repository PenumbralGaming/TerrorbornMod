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
    class ToggleEyeOfTheMenace : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toggle EotM");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nToggles [i/s1:" + ModContent.ItemType<PermanentUpgrades.EyeOfTheMenace>() +"]");
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
            if (!modPlayer.EyeOfTheMenace)
            {
                modPlayer.EyeOfTheMenace = true;
                Main.NewText("Eye of the Menace toggled on");

            }
            else
            {
                modPlayer.EyeOfTheMenace = false;
                Main.NewText("Eye of the Menace toggled off");
            }
            return base.CanUseItem(player);
        }
    }
}

