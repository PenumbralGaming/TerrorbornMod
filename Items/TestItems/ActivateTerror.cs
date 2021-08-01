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
    class ActivateTerror : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Activate/Deactivate terror");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks Shriek of Horror in pre-made worlds" +
                "\nUse again to take it away");
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
            if (!TerrorbornWorld.obtainedShriekOfHorror)
            {
                TerrorbornWorld.obtainedShriekOfHorror = true;

                TerrorbornPlayer target = TerrorbornPlayer.modPlayer(player);
                target.TriggerAbilityAnimation("Shriek of Horror", "Hold the 'Shriek of Horror' mod hotkey to unleash a scream that generates terror while close to enemies.", "Doing so will slowly take away your health.", 0, "Special abilities and items will consume terror.");
            }
            else
            {
                TerrorbornWorld.obtainedShriekOfHorror = false;
            }
            return base.CanUseItem(player);
        }
    }
}
