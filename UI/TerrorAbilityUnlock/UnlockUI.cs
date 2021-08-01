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
using System.Runtime.InteropServices;
using ReLogic.Graphics;

namespace TerrorbornMod.UI.TerrorAbilityUnlock
{
    class UnlockUI : UIState
    {
        public static float abilityUnlockAlpha = 0f;
        public static string abilityUnlockName = "Shriek of Horror";
        public static string abilityUnlockDescription1 = "Hold the 'Shriek of Horror' mod hotkey to unleash a scream and collect the terror of enemies.";
        public static string abilityUnlockDescription2 = "Doing so will slowly take away your health.";
        public static string abilityUnlockDescription3 = "Special abilities and items will consume terror.";
        public static int abilityUILifetimeCounter = 0;

        int fadeInTime = 90;
        int fadeOutTime = 45;


        public void updateTerrorAbilityUI()
        {
            if (abilityUnlockAlpha == 0f && abilityUILifetimeCounter <= 0)
            {

            }
            if (abilityUILifetimeCounter > 0 && abilityUnlockAlpha < 1f)
            {
                abilityUnlockAlpha += 1f / fadeInTime;
            }
            if (abilityUILifetimeCounter > 0 && abilityUnlockAlpha >= 1f)
            {
                abilityUILifetimeCounter--;
            }
            if (abilityUILifetimeCounter <= 0 && abilityUnlockAlpha > 0f)
            {
                abilityUnlockAlpha -= 1f / fadeOutTime;
            }
        }

        public override void Update(GameTime gameTime)
        {
            updateTerrorAbilityUI();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (abilityUnlockAlpha == 0)
            {
                return;
            }

            float scale = 1.75f;

            DynamicSpriteFont font = Main.fontDeathText;

            Vector2 textPosition1;
            textPosition1.X = Main.screenWidth* 0.5f;
            textPosition1.Y = Main.screenHeight * 0.2f;
            string text1 = "New ability unlocked:";
            Color color1 = new Color(209, 233, 246);

            Vector2 textPosition2;
            textPosition2.X = Main.screenWidth * 0.5f;
            textPosition2.Y = Main.screenHeight * 0.25f;
            string text2 = abilityUnlockName;
            Color color2 = new Color(255, 255, 255);

            Vector2 textPosition3;
            textPosition3.X = Main.screenWidth * 0.5f;
            textPosition3.Y = Main.screenHeight * 0.65f;
            string text3 = abilityUnlockDescription1;
            Color color3 = new Color(225, 225, 225);

            Vector2 textPosition4;
            textPosition4.X = Main.screenWidth * 0.5f;
            textPosition4.Y = Main.screenHeight * 0.69f;
            string text4 = abilityUnlockDescription2;
            Color color4 = new Color(210, 210, 210);

            Vector2 textPosition5;
            textPosition5.X = Main.screenWidth * 0.5f;
            textPosition5.Y = Main.screenHeight * 0.78f;
            string text5 = abilityUnlockDescription3;
            Color color5 = new Color(220, 220, 220);

            float offsetFactor = 11f;

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            scale = 0.65f;
            textPosition1.X -= text1.Length * scale * offsetFactor;
            spriteBatch.DrawString(font, text1, textPosition1, color1 * abilityUnlockAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            scale = 0.8f;
            textPosition2.X -= text2.Length * scale * offsetFactor;
            spriteBatch.DrawString(font, text2, textPosition2, color2 * abilityUnlockAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            scale = 0.65f;
            textPosition3.X -= text3.Length * scale * offsetFactor;
            spriteBatch.DrawString(font, text3, textPosition3, color3 * abilityUnlockAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            scale = 0.55f;
            textPosition4.X -= text4.Length * scale * offsetFactor;
            spriteBatch.DrawString(font, text4, textPosition4, color4 * abilityUnlockAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);

            scale = 0.62f;
            textPosition5.X -= text5.Length * scale * offsetFactor;
            spriteBatch.DrawString(font, text5, textPosition5, color5 * abilityUnlockAlpha, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0f);
        }
    }
}
