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

namespace TerrorbornMod.UI.TerrorMeter
{
    class TerrorMeterUI : UIState
    {
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!TerrorbornWorld.obtainedShriekOfHorror)
            {
                return;
            }

            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            Vector2 position;
            position.X = (int)(Main.screenWidth * Main.UIScale * 0.5f);
            position.Y = (int)(60.05882352941f * Main.UIScale);

            int frameCount = 7;
            int meterFrame = (int)((modPlayer.TerrorPercent / 100) * frameCount);

            if (meterFrame > frameCount)
            {
                meterFrame = frameCount;
            }

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            Texture2D texture = ModContent.GetTexture("TerrorbornMod/UI/TerrorMeter/Frames/meterpiece_" + meterFrame);
            float scale = Main.UIScale * 1.75f;
            spriteBatch.Draw(texture, position, texture.Bounds, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);


            scale = Main.UIScale * 0.65f;
            string meterText = (int)modPlayer.TerrorPercent + "%";
            position.X -= (meterText.Length) * scale * 8;
            position.Y -= scale * 14;
            
            spriteBatch.DrawString(Main.fontDeathText, meterText, position, Color.FromNonPremultiplied(209, 233, 246, 255), 0f, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
        }
    }
}
