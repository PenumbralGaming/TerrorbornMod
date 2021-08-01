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
using TerrorbornMod.UI.TerrorMeter;
using TerrorbornMod.UI.TerrorAbilityUnlock;

namespace TerrorbornMod
{
    class TerrorbornMod : Mod
    {

        internal UserInterface terrorMeterInterface;
        internal TerrorMeterUI terrorMeterUI;

        internal UserInterface unlockInterface;
        internal UnlockUI unlockUI;

        internal UserInterface terrorMenuInterface;
        internal TerrorAbilityMenu terrorAbilityMenu;

        public static ModHotKey ArmorAbility;
        public static ModHotKey quickVirus;
        public static ModHotKey ShriekOfHorror;
        public static ModHotKey PrimaryTerrorAbility;
        public static ModHotKey SecondaryTerrorAbility;
        public static ModHotKey OpenTerrorAbilityMenu;

        public static int CombatTokenCustomCurrencyId;

        public static float screenShaking = 0f;

        public static bool StartingItems = true;

        public static void ScreenShake(float Intensity)
        {
            screenShaking = Intensity;
        }

        public override void Unload()
        {
            Main.rainTexture = ModContent.GetTexture("Terraria/Rain");
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (!Main.gameMenu)
            {
                Player player = Main.LocalPlayer;
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

                if (modPlayer.ZoneDeimostone)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/CreepyCaverns");
                    priority = MusicPriority.BiomeHigh;
                }

                if (TerrorbornWorld.terrorRain && Main.raining && player.ZoneRain)
                {
                    music = GetSoundSlot(SoundType.Music, "Sounds/Music/DarkRain");
                    priority = MusicPriority.Event;
                }
            }
        }

        public TerrorbornMod()
        {
            //Utils.DrawBorderString()
        }

        string BugString()
        {
            return "Any Bug";
        }
        string MythrilString()
        {
            return "Any Mythril Bar";
        }
        public override void AddRecipeGroups()
        {
            //Any Bug
            RecipeGroup bugs = new RecipeGroup(new System.Func<string>(BugString));
            bugs.ValidItems.Add(ItemID.JuliaButterfly);
            bugs.ValidItems.Add(ItemID.MonarchButterfly);
            bugs.ValidItems.Add(ItemID.PurpleEmperorButterfly);
            bugs.ValidItems.Add(ItemID.RedAdmiralButterfly);
            bugs.ValidItems.Add(ItemID.SulphurButterfly);
            bugs.ValidItems.Add(ItemID.TreeNymphButterfly);
            bugs.ValidItems.Add(ItemID.UlyssesButterfly);
            bugs.ValidItems.Add(ItemID.ZebraSwallowtailButterfly);
            bugs.ValidItems.Add(ItemID.Firefly);
            bugs.ValidItems.Add(ItemID.Buggy);
            bugs.ValidItems.Add(ItemID.Grasshopper);
            bugs.ValidItems.Add(ItemID.Grubby);
            bugs.ValidItems.Add(ItemID.LightningBug);
            RecipeGroup.RegisterGroup("bugs", bugs);

            //Any Mythril Bar
            RecipeGroup mythril = new RecipeGroup(new System.Func<string>(MythrilString));
            mythril.ValidItems.Add(ItemID.MythrilBar);
            mythril.ValidItems.Add(ItemID.OrichalcumBar);
            RecipeGroup.RegisterGroup("mythril", mythril);
        }
        public override void Load()
        {
            ArmorAbility = RegisterHotKey("ArmorAbility", "Z");
            ShriekOfHorror = RegisterHotKey("Shriek of Horror", "Q");
            PrimaryTerrorAbility = RegisterHotKey("Primary Terror Ability", "F");
            SecondaryTerrorAbility = RegisterHotKey("Secondary Terror Ability", "X");
            quickVirus = RegisterHotKey("Quick Spark", "T");
            OpenTerrorAbilityMenu = RegisterHotKey("Open/Close Terror Ability Menu", "P");
            CombatTokenCustomCurrencyId = CustomCurrencyManager.RegisterCurrency(new CombatTokenCurrency(ItemType("CombatToken"), 999L));

            if (Main.netMode != NetmodeID.Server)
            {
                Ref<Effect> screenRef = new Ref<Effect>(GetEffect("Effects/ShockwaveEffect")); // The path to the compiled shader file.
                Filters.Scene["Shockwave"] = new Filter(new ScreenShaderData(screenRef, "Shockwave"), EffectPriority.VeryHigh);
                Filters.Scene["Shockwave"].Load();
            }
            if (!Main.dedServ)
            {
                terrorMeterInterface = new UserInterface();
                unlockInterface = new UserInterface();
                terrorMenuInterface = new UserInterface();

                terrorMeterUI = new TerrorMeterUI();
                terrorMeterUI.Activate();
                
                unlockUI = new UnlockUI();
                unlockUI.Activate();

                terrorAbilityMenu = new TerrorAbilityMenu();
                terrorAbilityMenu.Activate();
            }
        }
        private GameTime _lastUpdateUiGameTime;
        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (terrorMeterInterface?.CurrentState != null)
            {
                terrorMeterInterface.Update(gameTime);
            }
            if (unlockInterface?.CurrentState != null)
            {
                unlockInterface.Update(gameTime);
            }
            if (terrorMenuInterface?.CurrentState != null)
            {
                terrorMenuInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: terrorMeterInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && terrorMeterInterface?.CurrentState != null)
                        {
                            terrorMeterInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: unlockInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && unlockInterface?.CurrentState != null)
                        {
                            unlockInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));

                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "TerrorbornMod: terrorMenuInterface",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && terrorMenuInterface?.CurrentState != null)
                        {
                            terrorMenuInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                       InterfaceScaleType.UI));
            }
        }

        public static void TerrorThunder()
        {
            positionLightning = 1f;
            //transitionColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
            ScreenShake(10);
            ModContent.GetSound("TerrorbornMod/Sounds/Effects/ThunderAmbience").Play(Main.ambientVolume, Main.rand.NextFloat(-0.25f, 0.25f), Main.rand.NextFloat(-0.3f, 0.3f));
        }

        public static Color darkRainColor = Color.FromNonPremultiplied((int)(40f * 0.7f), (int)(55f * 0.7f), (int)(70f * 0.7f), 255);
        public static Color transitionColor = Color.White;
        public static Color lightningColor = Color.FromNonPremultiplied((int)(209f), (int)(138f), (int)(255f), 255);
        public static float positionForward = 0f;
        public static float positionBackward = 0f;
        public static float positionLightning = 0f;
        public static float transitionTime = 600f;

        public override void ModifyLightingBrightness(ref float scale)
        {
            if (TerrorbornWorld.terrorRain && Main.raining && Main.LocalPlayer.ZoneRain && !Main.dayTime)
            {
                scale *= 0.92f;
            }
        }

        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
        {
            if (TerrorbornWorld.terrorRain && Main.raining)
            {
                positionBackward = 0f;
                if (positionForward < 1f)
                {
                    positionForward += 1f / transitionTime;
                }
                else
                {
                    positionForward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, darkRainColor, positionForward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
                if (positionLightning > 0f)
                {
                    positionLightning -= 1f / 30f;
                    backgroundColor = Color.Lerp(backgroundColor, lightningColor, positionLightning);
                }
            }
            else if (transitionColor != Color.White)
            {
                positionForward = 0f;
                if (positionBackward < 1f)
                {
                    positionBackward += 1f / transitionTime;
                }
                else
                {
                    positionBackward = 1f;
                }
                transitionColor = Color.Lerp(transitionColor, Color.White, positionBackward);
                tileColor = tileColor.MultiplyRGBA(transitionColor);
                backgroundColor = backgroundColor.MultiplyRGBA(transitionColor);
            }
        }

        public override void PostUpdateEverything()
        {
            TerrorbornUtils.Update();

            ShowUI();

            screenShaking *= 0.95f;
            if ((int)Math.Round(screenShaking) == 0)
            {
                screenShaking = 0;
            }
        }

        internal void ShowUI()
        {
            unlockInterface?.SetState(unlockUI);
            terrorMenuInterface?.SetState(terrorAbilityMenu);
            terrorMeterInterface?.SetState(terrorMeterUI);
        }

        internal void HideUI()
        {
            unlockInterface?.SetState(null);
            terrorMenuInterface?.SetState(null);
            terrorMeterInterface?.SetState(null);
        }

        public static void DrawTextureEasy(Vector2 position, Texture2D texture)
        {
            Main.spriteBatch.Begin();
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, Color.White, 0f, texture.Size() / 2f, 10f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
        }

        public override void PostSetupContent()
        {
            Mod yabhb = ModLoader.GetMod("FKBossHealthBar");
            if (yabhb != null)
            {
                yabhb.Call("RegisterHealthBar", ModContent.NPCType<NPCs.Bosses.Sangrune>());
                yabhb.Call("RegisterHealthBarMini", ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());
            }

            Mod fargos = ModLoader.GetMod("Fargowiltas");
            if (fargos != null)
            {
                fargos.Call("AddEventSummon", 1f, "TerrorbornMod", "BrainStorm", (Func<bool>)(() => TerrorbornWorld.downedTerrorRain), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 3.5f, "TerrorbornMod", "LunarRitual", (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 5.5f, "TerrorbornMod", "DriedCanteen", (Func<bool>)(() => TerrorbornWorld.downedDunestock), Item.buyPrice(0, 25, 0, 0));
                fargos.Call("AddSummon", 9.5f, "TerrorbornMod", "RadioactiveSpiderFood", (Func<bool>)(() => TerrorbornWorld.downedShadowcrawler), Item.buyPrice(0, 15, 0, 0));
                fargos.Call("AddSummon", 11.35f, "TerrorbornMod", "PlasmaCore", (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), Item.buyPrice(0, 50, 0, 0));
            }

            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call("AddBossWithInfo", "Tidal Titan", 3.5f, (Func<bool>)(() => TerrorbornWorld.downedTidalTitan), "Kill a mysterious crab, which rarely spawns in the ocean biome during the night. Despawns if it sinks back into the water (a layer of platforms over the ocean is recommended). Note: doesn't despawn when it becomes day");
                bossChecklist.Call("AddBossWithInfo", "Dunestock", 5.5f, (Func<bool>)(() => TerrorbornWorld.downedDunestock), "Use a [i:" + ModContent.ItemType<Items.DriedCanteen>() + "] in the desert.");
                bossChecklist.Call("AddBossWithInfo", "Shadowcrawler", 9.5f, (Func<bool>)(() => TerrorbornWorld.downedShadowcrawler), "Use a [i:" + ModContent.ItemType<Items.RadioactiveSpiderFood>() + "] during the night.");
                bossChecklist.Call("AddBossWithInfo", "Prototype I", 11.35f, (Func<bool>)(() => TerrorbornWorld.downedPrototypeI), "Use a [i:" + ModContent.ItemType<Items.PlasmaCore>() + "] during the night.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune", 3.25f, (Func<bool>)(() => TerrorbornWorld.downedSangrune), "Spawns during a blood moon after the eater of worlds/brain of cthulhu have been defeated.");
                bossChecklist.Call("AddMiniBossWithInfo", "Sangrune (hardmode)", 7.5f, (Func<bool>)(() => TerrorbornWorld.downedSangrune2), "Re-fight Sangrune after the Wall of Flesh has been defeated.");
                bossChecklist.Call("AddMiniBossWithInfo", "Undying Spirit", 6.05f, (Func<bool>)(() => TerrorbornWorld.downedUndyingSpirit), "A strange eratic ghost that 'died' long ago. Spawns occasionally in the corruption: be wary.");
                bossChecklist.Call("AddEventWithInfo", "???", -5f, (Func<bool>)(() => TerrorbornWorld.obtainedShriekOfHorror), "Follow the [i:" + ModContent.ItemType<Items.MysteriousCompass>() + "]'s guidance");
                bossChecklist.Call("AddEventWithInfo", "Astraphobia", 6.06f, (Func<bool>)(() => TerrorbornWorld.downedTerrorRain), "Has a chance to occur instead of rain. Can be manually summoned by using a [i:" + ModContent.ItemType<Items.MiscConsumables.BrainStorm>() + "] during rain.");
                bossChecklist.Call("AddMiniBossWithInfo", "Frightcrawler", 6.07f, (Func<bool>)(() => TerrorbornWorld.downedFrightcrawler), "Spawns during the Astraphobia event (see above).");
            }
        }
    }
}
