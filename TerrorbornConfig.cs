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
using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria.ModLoader.Config.UI;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;

namespace TerrorbornMod
{
    class TerrorbornConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        public static TerrorbornConfig Instance;

        [Header("QoL features")]

        [DefaultValue(true)]
        [Label("Starting Items")]
        [Tooltip("Whether or not the player will spawn with extra items to make early game a more enjoyable experience")]
        public bool startingItems;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return true;
        }

        public override void OnLoaded()
        {
            //alwaysShowSoulChargeMeter = false;
            //soulChargeMeterX = 0.8f;
            //soulChargeMeterY = 0.02f;
            //soulChargeMeterScale = 10;
        }

        public override void OnChanged()
        {
            TerrorbornMod.StartingItems = startingItems;
        }
    }
}
