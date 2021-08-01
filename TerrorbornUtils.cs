using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader.IO;
using TerrorbornMod.Abilities;
using Terraria.GameInput;
using Microsoft.Xna.Framework.Input;
using Extensions;

namespace TerrorbornMod
{
    static class TerrorbornUtils
    {
        public static AbilityInfo intToAbility(int integerValue)
        {
            if (integerValue == 0)
            {
                return new None();
            }
            if (integerValue == 1)
            {
                return new HorrificAdaptationInfo();
            }
            if (integerValue == 2)
            {
                return new TerrorWarpInfo();
            }
            if (integerValue == 3)
            {
                return new VoidBlinkInfo();
            }
            if (integerValue == 4)
            {
                return new NecromanticCurseInfo();
            }
            if (integerValue == 5)
            {
                return new StarvingStormInfo();
            }
            if (integerValue == 6)
            {
                return new GelatinArmorInfo();
            }
            return new None();
        }

        public static int abilityToInt(AbilityInfo abilityType)
        {
            return abilityType.typeInt();
        }

        public static bool mouseDown;
        public static bool mouseJustPressed;
        static bool canPressMouseAgain = false;
        public static void Update()
        {
            Player player = Main.player[Main.myPlayer];
            if (player.controlUseItem)
            {
                mouseDown = true;
            }
            else
            {
                mouseDown = false;
                canPressMouseAgain = true;
            }
            if (mouseJustPressed)
            {
                mouseJustPressed = false;
            }
            if (canPressMouseAgain && mouseDown)
            {
                mouseJustPressed = true;
                canPressMouseAgain = false;
            }
        }
    }
}
