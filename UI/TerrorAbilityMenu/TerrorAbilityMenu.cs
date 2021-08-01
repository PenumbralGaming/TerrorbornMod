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
using Terraria.GameContent.UI.Elements;
using TerrorbornMod.Abilities;
using Terraria.GameInput;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using System.Runtime.InteropServices;
using ReLogic.Graphics;

namespace TerrorbornMod.UI.TerrorMeter
{
    class TerrorAbilityMenu : UIState
    {
        AbilityInfo ability1;
        AbilityInfo ability2;
        AbilityInfo ability3;

        UIPanel main;
        UIPanel EquippedPanel;
        UIPanel Primary1 = new UIPanel();
        UIPanel Primary2 = new UIPanel();
        UIPanel Primary3 = new UIPanel();
        UIPanel Secondary1 = new UIPanel();
        UIPanel Secondary2 = new UIPanel();
        UIPanel Secondary3 = new UIPanel();
        
        UIPanel Explanation1 = new UIPanel();
        UIPanel Explanation2 = new UIPanel();
        UIPanel Explanation3 = new UIPanel();

        UIPanel NextPage = new UIPanel();
        UIPanel PreviousPage = new UIPanel();

        UIPanel ExplanationPanel = new UIPanel();

        Vector2 mousePos;
        Rectangle mouseRectangle;

        Vector2 screenCenter;

        Player player;
        TerrorbornPlayer modPlayer;

        UIPanel abilityPanel1 = new UIPanel();
        UIPanel abilityPanel2 = new UIPanel();
        UIPanel abilityPanel3 = new UIPanel();

        int page = 0;
        const int maxPage = 1;

        string abilityDescription;
        bool showingAbilityDescription = false;

        public override void OnInitialize()
        {
            mousePos = new Vector2(Main.mouseX, Main.mouseY);
            mouseRectangle = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
            screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.UIScale;

            main = new UIPanel();
            EquippedPanel = new UIPanel();
            Primary1 = new UIPanel();
            Primary2 = new UIPanel();
            Primary3 = new UIPanel();
            Secondary1 = new UIPanel();
            Secondary2 = new UIPanel();
            Secondary3 = new UIPanel();
            abilityPanel1 = new UIPanel();
            NextPage = new UIPanel();
            PreviousPage = new UIPanel();

            showingAbilityDescription = false;

            Append(EquippedPanel);
            Append(Primary1);
            Append(Primary2);
            Append(Primary3);
            Append(Secondary1);
            Append(Secondary2);
            Append(Secondary3);
            Append(ExplanationPanel);
            Append(main);
            Append(abilityPanel1);
            Append(abilityPanel2);
            Append(abilityPanel3);
            Append(NextPage);
            Append(PreviousPage);
            Append(Explanation1);
            Append(Explanation2);
            Append(Explanation3);
        }
        public void DrawAbilityInfo(AbilityInfo ability, Player player, Vector2 position, SpriteBatch spriteBatch, UIPanel panel)
        {
            panel.Left.Set(position.X, 0f);
            panel.Width.Set(300f, 0f);
            panel.Top.Set(position.Y, 0f);
            panel.Height.Set(75f, 0f);
            //abilityPanel.BackgroundColor = Color.FromNonPremultiplied(143, 167, 164, 0);
            
            panel.Draw(spriteBatch);

            float TextureScale = 1f;
            spriteBatch.Draw(ability.texture(), new Rectangle((int)(position.X + 8 + ability.texture().Width * TextureScale), (int)(position.Y + 45), (int)(ability.texture().Width * TextureScale), (int)(ability.texture().Height * TextureScale)), new Rectangle(0, 0, ability.texture().Width, ability.texture().Height), Color.White, 0f, new Vector2(ability.texture().Width / 2 * TextureScale, ability.texture().Height / 2 * TextureScale), SpriteEffects.None, TextureScale);

            string nameText = ability.Name() + " (Cost: " + ability.Cost().ToString() + "%)";
            spriteBatch.DrawString(Main.fontDeathText, nameText, position + new Vector2(5, 5), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);
        }
        public override void Update(GameTime gameTime)
        {
            player = Main.player[Main.myPlayer];
            modPlayer = TerrorbornPlayer.modPlayer(player);

            if (page == 0)
            {
                ability1 = new NecromanticCurseInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability1)))
                {
                    ability1 = new NotYetUnlocked();
                }
                ability2 = new GelatinArmorInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability2)))
                {
                    ability2 = new NotYetUnlocked();
                }
                ability3 = new HorrificAdaptationInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability3)))
                {
                    ability3 = new NotYetUnlocked();
                }
            }
            if (page == 1)
            {
                ability1 = new VoidBlinkInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability1)))
                {
                    ability1 = new NotYetUnlocked();
                }
                ability2 = new StarvingStormInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability2)))
                {
                    ability2 = new NotYetUnlocked();
                }
                ability3 = new TerrorWarpInfo();
                if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(ability3)))
                {
                    ability3 = new NotYetUnlocked();
                }
            }
            mousePos = new Vector2(Main.mouseX, Main.mouseY);
            mouseRectangle = new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1);
            screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.UIScale;

            main.Left.Set(screenCenter.X - 400f, 0f);
            main.Width.Set(800f, 0f);
            main.Top.Set(screenCenter.Y - 400f, 0f);
            main.Height.Set(300f, 0f);

            PreviousPage.Left.Set(screenCenter.X - 375, 0f);
            PreviousPage.Width.Set(110f, 0f);
            PreviousPage.Top.Set(screenCenter.Y - 125f, 0f);
            PreviousPage.Height.Set(20f, 0f);

            NextPage.Left.Set(screenCenter.X - 185f, 0f);
            NextPage.Width.Set(110f, 0f);
            NextPage.Top.Set(screenCenter.Y - 125f, 0f);
            NextPage.Height.Set(20f, 0f);

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!TerrorbornWorld.obtainedShriekOfHorror || !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).ShowTerrorAbilityMenu)
            {
                showingAbilityDescription = false;
                return;
            }
            //main.BackgroundColor = Color.FromNonPremultiplied(95, 111, 109, 200);

            spriteBatch.End();
            spriteBatch.Begin(default, BlendState.AlphaBlend);

            main.Draw(spriteBatch);

            DrawAbilityInfo(ability1, player, new Vector2(screenCenter.X - 375, screenCenter.Y - 375), spriteBatch, abilityPanel1);
            DrawAbilityInfo(ability2, player, new Vector2(screenCenter.X - 375, screenCenter.Y - 290), spriteBatch, abilityPanel2);
            DrawAbilityInfo(ability3, player, new Vector2(screenCenter.X - 375, screenCenter.Y - 205), spriteBatch, abilityPanel3);

            EquippedPanel.Left.Set(screenCenter.X - 66, 0f);
            EquippedPanel.Width.Set(455, 0f);
            EquippedPanel.Top.Set(screenCenter.Y - 375, 0f);
            EquippedPanel.Height.Set(75f, 0f);
            EquippedPanel.Draw(spriteBatch);

            string primaryText = "Primary: " + modPlayer.primaryAbility.Name() + " (UNBINDED) (" + (int)modPlayer.primaryAbility.Cost() + "%)";
            if (TerrorbornMod.PrimaryTerrorAbility.GetAssignedKeys().Count > 0)
            {
                primaryText = "Primary: " + modPlayer.primaryAbility.Name() + " (" + TerrorbornMod.PrimaryTerrorAbility.GetAssignedKeys()[0].ToString() + ") (" + (int)modPlayer.primaryAbility.Cost() + "%)";
            }
            spriteBatch.DrawString(Main.fontDeathText, primaryText, new Vector2(screenCenter.X - 61, screenCenter.Y - 372f), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            
            string secondaryText = "Secondary: " + modPlayer.secondaryAbility.Name() + " (UNBINDED) (" + (int)modPlayer.secondaryAbility.Cost() + "%)";
            if (TerrorbornMod.SecondaryTerrorAbility.GetAssignedKeys().Count > 0)
            {
                secondaryText = "Secondary: " + modPlayer.secondaryAbility.Name() + " (" + TerrorbornMod.SecondaryTerrorAbility.GetAssignedKeys()[0].ToString() + ") (" + (int)(modPlayer.secondaryAbility.Cost() * 1.5f) + "%)";
            }
            spriteBatch.DrawString(Main.fontDeathText, secondaryText, new Vector2(screenCenter.X - 61, screenCenter.Y - 335f), Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);

            PreviousPage.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Previous page", new Vector2(screenCenter.X - 369f, screenCenter.Y - 125f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);
            NextPage.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Next page", new Vector2(screenCenter.X - 163f, screenCenter.Y - 125f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);


            if (new Rectangle((int)(screenCenter.X - 375), (int)(screenCenter.Y - 125), 110, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed) //When previous page is clicked
            {
                page--;
                if (page < 0)
                {
                    page = maxPage;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }

            if (new Rectangle((int)(screenCenter.X - 185), (int)(screenCenter.Y - 125), 110, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed) //When next page is clicked
            {
                page++;
                if (page > maxPage)
                {
                    page = 0;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }

            //---------PRIMARY BUTTONS---------
            Primary1.Left.Set(screenCenter.X - 318, 0f);
            Primary1.Width.Set(65, 0f);
            Primary1.Top.Set(screenCenter.Y - 330, 0f);
            Primary1.Height.Set(20, 0f);
            Primary1.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Primary", new Vector2(screenCenter.X - 314f, screenCenter.Y - 330f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 318), (int)(screenCenter.Y - 330), 65, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.primaryAbility = ability1;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            Primary2.Left.Set(screenCenter.X - 318, 0f);
            Primary2.Width.Set(65, 0f);
            Primary2.Top.Set(screenCenter.Y - 245, 0f);
            Primary2.Height.Set(20, 0f);
            Primary2.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Primary", new Vector2(screenCenter.X - 314f, screenCenter.Y - 245f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 318), (int)(screenCenter.Y - 245), 65, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.primaryAbility = ability2;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            Primary3.Left.Set(screenCenter.X - 318, 0f);
            Primary3.Width.Set(65, 0f);
            Primary3.Top.Set(screenCenter.Y - 160, 0f);
            Primary3.Height.Set(20, 0f);
            Primary3.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Primary", new Vector2(screenCenter.X - 314f, screenCenter.Y - 160f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 318), (int)(screenCenter.Y - 160f), 65, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.primaryAbility = ability3;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            //---------SECONDARY BUTTONS---------
            Secondary1.Left.Set(screenCenter.X - 240, 0f);
            Secondary1.Width.Set(81, 0f);
            Secondary1.Top.Set(screenCenter.Y - 330, 0f);
            Secondary1.Height.Set(20, 0f);
            Secondary1.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Secondary", new Vector2(screenCenter.X - 237f, screenCenter.Y - 330f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 240), (int)(screenCenter.Y - 330), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.secondaryAbility = ability1;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            Secondary2.Left.Set(screenCenter.X - 240, 0f);
            Secondary2.Width.Set(81, 0f);
            Secondary2.Top.Set(screenCenter.Y - 245, 0f);
            Secondary2.Height.Set(20, 0f);
            Secondary2.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Secondary", new Vector2(screenCenter.X - 237f, screenCenter.Y - 245f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 240), (int)(screenCenter.Y - 245), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.secondaryAbility = ability2;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            Secondary3.Left.Set(screenCenter.X - 240, 0f);
            Secondary3.Width.Set(81, 0f);
            Secondary3.Top.Set(screenCenter.Y - 160, 0f);
            Secondary3.Height.Set(20, 0f);
            Secondary3.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "Secondary", new Vector2(screenCenter.X - 237f, screenCenter.Y - 160f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(screenCenter.X - 240), (int)(screenCenter.Y - 160f), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                modPlayer.secondaryAbility = ability3;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Grab);
            }

            //---------EXPLANATION BUTTONS---------
            float ExplanationLeft = screenCenter.X - 110;
            float QuestionMarkOffsetX = 6.5f;
            int ButtonWidth = 20;


            Explanation1.Left.Set(ExplanationLeft, 0f);
            Explanation1.Width.Set(ButtonWidth, 0f);
            Explanation1.Top.Set(screenCenter.Y - 330, 0f);
            Explanation1.Height.Set(20, 0f);
            Explanation1.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "?", new Vector2(ExplanationLeft + QuestionMarkOffsetX, screenCenter.Y - 328f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(ExplanationLeft), (int)(screenCenter.Y - 330), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                showingAbilityDescription = true;
                abilityDescription = ability1.Description();
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }

            Explanation2.Left.Set(ExplanationLeft, 0f);
            Explanation2.Width.Set(ButtonWidth, 0f);
            Explanation2.Top.Set(screenCenter.Y - 245, 0f);
            Explanation2.Height.Set(20, 0f);
            Explanation2.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "?", new Vector2(ExplanationLeft + QuestionMarkOffsetX, screenCenter.Y - 243f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(ExplanationLeft), (int)(screenCenter.Y - 245), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                showingAbilityDescription = true;
                abilityDescription = ability2.Description();
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }

            Explanation3.Left.Set(ExplanationLeft, 0f);
            Explanation3.Width.Set(ButtonWidth, 0f);
            Explanation3.Top.Set(screenCenter.Y - 160, 0f);
            Explanation3.Height.Set(20, 0f);
            Explanation3.Draw(spriteBatch);
            spriteBatch.DrawString(Main.fontDeathText, "?", new Vector2(ExplanationLeft + QuestionMarkOffsetX, screenCenter.Y - 158f), Color.White, 0f, Vector2.Zero, 0.35f, SpriteEffects.None, 0f);

            if (new Rectangle((int)(ExplanationLeft), (int)(screenCenter.Y - 160f), 81, 20).Intersects(mouseRectangle) && TerrorbornUtils.mouseJustPressed)
            {
                showingAbilityDescription = true;
                abilityDescription = ability3.Description();
                Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuTick);
            }

            //---------EXPLANATION PANEL---------
            ExplanationPanel.Left.Set(screenCenter.X - 66, 0f);
            ExplanationPanel.Width.Set(455, 0f);
            ExplanationPanel.Top.Set(screenCenter.Y - 290, 0f);
            ExplanationPanel.Height.Set(160, 0f);
            ExplanationPanel.Draw(spriteBatch);

            string explanation = "Secondary terror abilities will cost 1.5x as much terror" +
                               "\nas they would if they were primary. Click the '?' icon " +
                               "\nnext to one the abilities to see that ability's function.";
            if (showingAbilityDescription) explanation = abilityDescription;
            spriteBatch.DrawString(Main.fontDeathText, explanation, new Vector2(screenCenter.X - 61, screenCenter.Y - 285), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
        }
    }
}
