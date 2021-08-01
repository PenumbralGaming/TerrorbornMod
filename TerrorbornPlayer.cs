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
    class TerrorbornPlayer : ModPlayer
    {
        public bool graniteSpark = false;
        public bool astralSpark = false;

        public int CombatPoints = 0;
        
        //Terror things
        public float TerrorPercent = 0;
        public float ShriekOfHorrorMovement = 0;
        public bool MidShriek = false;
        public float ShriekSpeed = 1;
        public float ShriekKnockback = 1;
        public float ShriekOfHorrorExtraDamageMultiplier = 1.5f;
        public float ShriekTerrorMultiplier = 1f;
        public float ShriekPain = 1f;
        public float ShriekRangeMultiplier = 1f;
        public bool ShowTerrorAbilityMenu = false;
        public int primaryAbilityInt = 0;
        public int secondaryAbilityInt = 0;
        public AbilityInfo primaryAbility = new None();
        public AbilityInfo secondaryAbility = new None();

        int abilityAnimationCounter1 = 0;
        int abilityAnimationCounter2 = 0;
        int abilityAnimationType = 0;

        public int TenebrisDashTime = 0;
        public Vector2 TenebrisDashVelocity = new Vector2(0, 0);
        public int iFrames = 120;

        //Extra generic stats
        public float magicUseSpeed = 1f;
        public float rangedUseSpeed = 1f;
        public float toolUseSpeed = 1f;
        public float placeSpeed = 1f;
        public float allUseSpeed = 1f;
        public bool canUseItems = true;

        //Restless stats
        public float restlessDamage = 1f;
        public float restlessKnockback = 1f;
        public float restlessUseSpeed = 1f;
        public float restlessChargedUseSpeed = 1f;
        public float restlessNonChargedUseSpeed = 1f;

        //Accessory/equipment fields
        public bool IncendiaryShield = false;
        public bool DeimosteelCharm = false;
        public bool MysteriousCompass = false;
        public bool AzuriteBrooch = false;
        public bool SoulEater = false;
        public bool cloakOfTheWind = false;
        public bool PlasmaArmorBonus = false;
        public bool VampiricPendant = false;
        public bool IncendiusArmorBonus = false;
        public bool SoulReaperArmorBonus = false;
        public bool Aerodynamic = false;
        public bool Glooped = false;
        public bool SanguineSetBonus;
        public bool AzuriteArmorBonus = false;
        public bool SangoonBand = false;
        public int SangoonBandCooldown = 0;
        public bool LiesOfNourishment = false;
        public bool IntimidationAura = false;
        public bool AntlionShell = false;
        public bool TideSpirit = false;
        public bool TidalShellArmorBonus = false;
        public bool HorrificCharm = false;

        //Permanent Upgrades
        public bool EyeOfTheMenace = false;
        public bool GoldenTooth = false;
        public bool CoreOfFear = false;
        public bool AnekronianApple = false;
        public bool DemonicLense = false;

        //Terror ability fields
        public int VoidBlinkTime = 0;
        public int GelatinArmorTime = 0;
        public int GelatinPunishmentDamage = 0;
        public IList<int> unlockedAbilities = new List<int>();

        //Biome fields
        public bool ZoneDeimostone;

        //Misc stuff
        public int terrorDrainCounter = 0;

        public void TriggerAbilityAnimation(string name, string description1, string description2, int animationType, string description3 = "You can equip terror abilities by using the 'Open/Close Terror Abilities Menu' hotkey.", int visibilityTime = 600)
        {
            UI.TerrorAbilityUnlock.UnlockUI.abilityUILifetimeCounter = visibilityTime;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockName = name;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription1 = description1;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription2 = description2;
            UI.TerrorAbilityUnlock.UnlockUI.abilityUnlockDescription3 = description3;

            abilityAnimationType = animationType;
            if (animationType == 0) //Shriek of Horror/Default
            {
                abilityAnimationCounter1 = 9;
            }
        }

        public override void UpdateBiomes()
        {
            ZoneDeimostone = TerrorbornWorld.deimostoneTiles > 75;
        }

        public override void SetupStartInventory(IList<Item> items, bool mediumcoreDeath)
        {
            Item compass = new Item();
            compass.SetDefaults(ModContent.ItemType<Items.MysteriousCompass>());
            items.Add(compass);

            if (TerrorbornMod.StartingItems)
            {
                //Extra starting weapons, accessories, etc.

                Item item = new Item();

                //staff
                item = new Item();
                item.SetDefaults(ItemID.AmethystStaff);
                items.Add(item);

                //torch
                item = new Item();
                item.SetDefaults(ItemID.Torch);
                item.stack = 50;
                items.Add(item);

                //torch
                item = new Item();
                item.SetDefaults(ItemID.Glowstick);
                item.stack = 50;
                items.Add(item);

                //bombs
                item = new Item();
                item.SetDefaults(ItemID.Bomb);
                item.stack = 15;
                items.Add(item);

                //ropes
                item = new Item();
                item.SetDefaults(ItemID.Rope);
                item.stack = 200;
                items.Add(item);

                //crown
                item = new Item();
                item.SetDefaults(ItemID.SlimeCrown);
                items.Add(item);

                //mining potions
                item = new Item();
                item.SetDefaults(ItemID.MiningPotion);
                item.stack = 3;
                items.Add(item);

                //spelunker potions
                item = new Item();
                item.SetDefaults(ItemID.SpelunkerPotion);
                item.stack = 3;
                items.Add(item);

                //shackle
                item = new Item();
                item.SetDefaults(ItemID.GrapplingHook);
                items.Add(item);
            }
        }

        public override void ResetEffects()
        {
            DeimosteelCharm = false;
            restlessDamage = 1f;
            restlessKnockback = 1f;
            restlessUseSpeed = 1f;
            restlessChargedUseSpeed = 1f;
            restlessNonChargedUseSpeed = 1f;
            HorrificCharm = false;
            TideSpirit = false;
            TidalShellArmorBonus = false;
            SoulReaperArmorBonus = false;
            MysteriousCompass = false;
            IntimidationAura = false;
            SanguineSetBonus = false;
            AntlionShell = false;
            SoulEater = false;
            AzuriteBrooch = false;
            astralSpark = false;
            graniteSpark = false;
            ShriekOfHorrorMovement = 0f;
            if (CoreOfFear)
            {
                ShriekOfHorrorMovement += 0.3f;
            }
            if (AnekronianApple)
            {
                ShriekOfHorrorMovement += 0.2f;
            }
            ShriekSpeed = 1;
            if (DemonicLense)
            {
                ShriekSpeed = 0.5f;
            }
            ShriekKnockback = 1;
            ShriekOfHorrorExtraDamageMultiplier = 2f;
            ShriekTerrorMultiplier = 1f;
            if (EyeOfTheMenace)
            {
                ShriekTerrorMultiplier += 0.5f;
            }
            ShriekPain = 1f;
            ShriekRangeMultiplier = 1f;
            if (GoldenTooth)
            {
                ShriekRangeMultiplier += 1 / 3;
            }
            rangedUseSpeed = 1f;
            toolUseSpeed = 1f;
            placeSpeed = 1f;
            SangoonBand = false;
            LiesOfNourishment = false;
            magicUseSpeed = 1f;
            allUseSpeed = 1f;
            IncendiaryShield = false;
            Aerodynamic = false;
            Glooped = false;
            PlasmaArmorBonus = false;
            IncendiusArmorBonus = false;
            AzuriteArmorBonus = false;
            VampiricPendant = false;

            Vector2 screenCenter = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2) * Main.UIScale;
            Rectangle mainRect = new Rectangle((int)screenCenter.X - 400, (int)screenCenter.Y - 400, 800, 300);
            Rectangle mouseRectangle = new Rectangle((int)Main.mouseX, (int)Main.mouseY, 1, 1);
            if (mainRect.Intersects(mouseRectangle) && ShowTerrorAbilityMenu)
            {
                canUseItems = false;
            }
            else
            {
                canUseItems = true;
            }
        }

        public override void PreUpdateMovement()
        {
            if (graniteSpark)
            {
                if (player.controlLeft) player.velocity.X = -graniteSparkData.speed;
                else if (player.controlRight) player.velocity.X = graniteSparkData.speed;
                else player.velocity.X = 0;

                if (player.controlUp) player.velocity.Y = -graniteSparkData.speed;
                else if (player.controlDown) player.velocity.Y = graniteSparkData.speed;
                else player.velocity.Y = 0;
            }

            if (astralSpark)
            {
                float speedMultiplier = 1f;

                if (player.controlJump) speedMultiplier = 5f;

                if (player.controlLeft) player.velocity.X = -astralSparkData.speed * speedMultiplier;
                else if (player.controlRight) player.velocity.X = astralSparkData.speed * speedMultiplier;
                else player.velocity.X = 0;

                if (player.controlUp) player.velocity.Y = -astralSparkData.speed * speedMultiplier;
                else if (player.controlDown) player.velocity.Y = astralSparkData.speed * speedMultiplier;
                else player.velocity.Y = 0;
            }

            if (ShriekOfHorrorMovement == 0 && MidShriek)
            {
                player.velocity = Vector2.Zero;
            }
        }

        public override float UseTimeMultiplier(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            float finalMult = allUseSpeed;
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
            {
                finalMult *= toolUseSpeed;
            }
            Item dummyItem = new Item();
            dummyItem.SetDefaults(ItemID.UglySweater);
            if (item.createTile != dummyItem.createTile || item.createWall != dummyItem.createWall)
            {
                finalMult *= placeSpeed;
            }
            if (item.magic)
            {
                finalMult *= magicUseSpeed;
            }
            if (item.ranged)
            {
                finalMult *= rangedUseSpeed;
            }
            if (modItem.restless)
            {
                finalMult *= restlessUseSpeed;
                if (modItem.RestlessChargedUp())
                {
                    finalMult *= restlessChargedUseSpeed;
                }
                else
                {
                    finalMult *= restlessNonChargedUseSpeed;
                }
            }
            return finalMult;
        }

        public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
        {

        }

        public void OnHitByAnything(Entity entity, int damage, bool crit)
        {
            if (IntimidationAura)
            {
                TerrorPercent -= TerrorPercent / 3;
                if (TerrorPercent < 0)
                {
                    TerrorPercent = 0;
                }
            }
        }

        public override void OnHitByNPC(NPC npc, int damage, bool crit)
        {
            OnHitByAnything(npc, damage, crit);
        }

        public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
        {
            OnHitByAnything(proj, damage, crit);
        }
        public override void PostUpdateEquips()
        {
            if (Aerodynamic)
            {
                player.wingTimeMax *= 2;
            }

            if (Glooped)
            {
                player.wingTimeMax /= 4;
            }
        }
        public override void ModifyScreenPosition()
        {
            Vector2 ScreenOffset = new Vector2(Main.rand.NextFloat(-TerrorbornMod.screenShaking, TerrorbornMod.screenShaking), Main.rand.NextFloat(-TerrorbornMod.screenShaking, TerrorbornMod.screenShaking));
            Main.screenPosition += ScreenOffset;
        }

        int ShriekCounter = 0;
        int SoundCounter = 0;
        public void UpdateShriekOfHorror()
        {
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());

            if (TerrorbornMod.ShriekOfHorror.Current && (player.velocity.Y == 0 || ShriekOfHorrorMovement > 0) && (player.itemTime <= 0 || darkblood))
            {
                MidShriek = true;
            }
            if (!TerrorbornMod.ShriekOfHorror.Current || player.dead)
            {
                MidShriek = false;
            }


            if (MidShriek)
            {
                if (!(ShriekOfHorrorMovement == 0))
                {
                    if ((int)player.velocity.Y != 0)
                    {
                        player.position -= player.velocity * (1 - ShriekOfHorrorMovement);
                    }
                    else
                    {
                        player.position.X -= player.velocity.X * (1 - ShriekOfHorrorMovement);
                    }
                }

                if (ShriekCounter > 0)
                {
                    ShriekCounter--;
                    if (!darkblood)
                    {
                        player.bodyFrame.Y = 6 * player.bodyFrame.Height;
                    }
                }
                else
                {
                    Shriek();
                    if (!darkblood)
                    {
                        player.bodyFrame.Y = 5 * player.bodyFrame.Height;
                    }
                    SoundCounter--;
                    if (SoundCounter <= 0)
                    {
                        SoundCounter = 22;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item103, player.Center);
                    }
                    Vector2 dustpos = player.Center;
                    dustpos.Y -= 13;
                    dustpos.X -= 3;

                    dustpos.X += player.direction * 1;

                    int dust = Dust.NewDust(dustpos, 0, 0, 54);
                    Main.dust[dust].velocity = player.velocity / 3;
                    TerrorbornMod.ScreenShake(5);
                }
            }
            else
            {
                SoundCounter = 0;
                ShriekCounter = (int)(80 * ShriekSpeed);
            }
        }

        int dustCounter = 15;
        public void Shriek()
        {
            dustCounter--;
            if (dustCounter <= 0)
            {
                dustCounter = 15;
                DustExplosion(player.Center, 0, 360, 45 * ShriekRangeMultiplier, 54, 1, true);
            }

            float range = 375 * ShriekRangeMultiplier;
            float NPCsInRange = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.Distance(player.Center) < range && !target.friendly && target.active && target.lifeMax > 5)
                {
                    NPCsInRange++;
                    //target.velocity -= target.DirectionTo(player.Center) * ((range - target.Distance(player.Center)) / 400) * target.knockBackResist * ShriekKnockback;
                }
            }
            if (NPCsInRange > 0)
            {
                TerrorPercent = TerrorPercent + 12f / 60f * ShriekTerrorMultiplier;
            }
        }

        void UpdateAbilityAnimations()
        {
            if (abilityAnimationType == 0)
            {
                player.velocity = Vector2.Zero;
                player.velocity.Y -= 1;
                abilityAnimationCounter2--;
                if (abilityAnimationCounter2 <= 0)
                {
                    abilityAnimationCounter1--;
                    if (abilityAnimationCounter1 <= 0)
                    {
                        DustExplosion(player.Center, 0, 360, 40, 66, 1, true);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item103, player.Center);
                        TerrorbornMod.ScreenShake(30);
                    }
                    else
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                        TerrorbornMod.ScreenShake(5);
                        DustExplosion(player.Center, 0, 360, 10, 66, 0.5f, true);
                        abilityAnimationCounter2 = 4;
                    }
                }
            }
        }

        int dashTimer = 0;
        int downCounter = 60;
        bool downCounterCounter = false;

        bool usingPrimary;
        bool usingSecondary;

        public override void UpdateAutopause()
        {
            if (TerrorbornMod.OpenTerrorAbilityMenu.JustPressed)
            {
                if (ShowTerrorAbilityMenu)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
                    ShowTerrorAbilityMenu = true;
                }
            }
        }

        public override void PostUpdate()
        {
            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime--;
                int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
                Main.dust[dust].color = Color.LightBlue;
                Main.dust[dust].velocity /= 2;
                Main.dust[dust].alpha = 255 / 2;
            }

            if (terrorDrainCounter > 0)
            {
                terrorDrainCounter--;
                TerrorPercent += 1f / 60f;
            }

            if (VoidBlinkTime > 0)
            {
                VoidBlinkTime--;
                if (VoidBlinkTime == 0)
                {
                    DustExplosion(player.Center, 0, 15, 15, 27, 1.5f, true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item72, player.Center);
                }
                if (VoidBlinkTime == 60)
                {
                    DustExplosion(player.Center, 0, 15, 7.5f, 27, 1.5f, true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                }
                player.immuneAlpha = 255 / 2;
            }

            if (TerrorbornMod.OpenTerrorAbilityMenu.JustPressed)
            {
                if (ShowTerrorAbilityMenu)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuClose);
                    ShowTerrorAbilityMenu = false;
                }
                else
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MenuOpen);
                    ShowTerrorAbilityMenu = true;
                }
            }

            primaryAbilityInt = TerrorbornUtils.abilityToInt(primaryAbility);
            secondaryAbilityInt = TerrorbornUtils.abilityToInt(secondaryAbility);

            if (TerrorbornMod.PrimaryTerrorAbility.JustPressed)
            {
                usingPrimary = true;
            }
            if (TerrorbornMod.PrimaryTerrorAbility.JustReleased)
            {
                usingPrimary = false;
            }
            if (TerrorbornMod.SecondaryTerrorAbility.JustPressed)
            {
                usingSecondary = true;
            }
            if (TerrorbornMod.SecondaryTerrorAbility.JustReleased)
            {
                usingSecondary = false;
            }

            if (primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() / 60 && primaryAbility.canUse(player) && TerrorbornMod.PrimaryTerrorAbility.Current)
            {
                primaryAbility.OnUse(player);
                TerrorPercent -= primaryAbility.Cost() / 60;

            }

            if (!primaryAbility.HeldDown() && TerrorPercent >= primaryAbility.Cost() && primaryAbility.canUse(player) && TerrorbornMod.PrimaryTerrorAbility.JustPressed)
            {
                primaryAbility.OnUse(player);
                TerrorPercent -= primaryAbility.Cost();
            }

            if (secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f / 60 && secondaryAbility.canUse(player) && TerrorbornMod.SecondaryTerrorAbility.Current)
            {
                secondaryAbility.OnUse(player);
                TerrorPercent -= (secondaryAbility.Cost() / 60) * 1.5f;

            }

            if (!secondaryAbility.HeldDown() && TerrorPercent >= secondaryAbility.Cost() * 1.5f && secondaryAbility.canUse(player) && TerrorbornMod.SecondaryTerrorAbility.JustPressed)
            {
                secondaryAbility.OnUse(player);
                TerrorPercent -= secondaryAbility.Cost() * 1.5f;
            }

            if (SangoonBandCooldown > 0)
            {
                SangoonBandCooldown--;
            }

            if (abilityAnimationCounter1 > 0)
            {
                UpdateAbilityAnimations();
            }

            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                UpdateShriekOfHorror();
            }
            if (TerrorPercent > 100)
            {
                TerrorPercent = 100;
            }
            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.Dunestock>())) player.buffImmune[BuffID.WindPushed] = true;

            if (TerrorbornModContent.quickVirus.JustPressed && player.HasItem(ModContent.ItemType<Items.AstralSpark>()))
            {
                astralSparkData.Transform(player);
            }

            if (TerrorbornModContent.quickVirus.JustPressed && player.HasItem(ModContent.ItemType<Items.graniteVirusSpark>()))
            {
                graniteSparkData.Transform(player);
            }

            if (graniteSpark || astralSpark)
            {
                player.wings = 0;
            }

            if (iFrames > 0)
            {
                iFrames--;
                player.immuneAlpha = 125;
            }

            if (TenebrisDashTime > 0)
            {
                TenebrisDashTime--;
                int dust = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 74, 0f, 0f, 100, Scale: 1.5f);
                player.velocity = TenebrisDashVelocity;
            }

            if (!NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()) && CombatPoints > 0)
            {
                CombatPoints = 0;
            }
        }

        public override void OnRespawn(Player player)
        {
            usingPrimary = false;
        }

        public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
        {
            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit1, player.Center);
                damage = 0;
            }

            if (MidShriek)
            {
                damage = (int)(damage * ShriekOfHorrorExtraDamageMultiplier);
            }
            if (player.HasBuff(ModContent.BuffType<Items.Equipable.Armor.TenebralFocus>()))
            {
                damage = (int)(damage * 1.5f);
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
        {
            if (GelatinPunishmentDamage > 0)
            {
                damage += GelatinPunishmentDamage;
                GelatinPunishmentDamage = 0;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, player.Center);
                for (int i = 0; i < 15; i++)
                {
                    int dust = Dust.NewDust(player.position, player.width, player.height, DustID.t_Slime);
                    Main.dust[dust].color = Color.LightBlue;
                    Main.dust[dust].velocity /= 2;
                    Main.dust[dust].alpha = 255 / 2;
                }
            }

            if (GelatinArmorTime > 0)
            {
                GelatinArmorTime = 0;
                GelatinPunishmentDamage = damage / 3;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit1, player.Center);
                damage = 0;
            }

            if (MidShriek)
            {
                damage = (int)(damage * ShriekOfHorrorExtraDamageMultiplier);
            }
        }

        public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (TenebrisDashTime > 0 && !player.HasBuff(ModContent.BuffType<Items.Equipable.Armor.TenebralFocus>()))
            {
                player.immuneTime = 120;
                DustExplosion(player.Center, 0, 360, 50, 74, 1.5f, true);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item60, player.Center);
                player.AddBuff(ModContent.BuffType<Items.Equipable.Armor.TenebralFocus>(), 60 * 13);
                iFrames = 120;
                return false;
            }
            if (iFrames > 0 || VoidBlinkTime > 0)
            {
                return false;
            }
            return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void UpdateBadLifeRegen()
        {
            if (player.HasBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>()))
            {
                player.lifeRegen = 0;
                player.lifeRegen -= 10 + (player.statDefense / 100) * 18;
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (MidShriek)
            {
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " was overloaded with fear.");
                }
                if (choice == 1)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " drained their own life.");
                }
                if (choice == 2)
                {
                    damageSource = PlayerDeathReason.ByCustomReason(player.name + " couldn't handle their power.");
                }
            }

            TerrorPercent = 0f;
            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = mod.GetPacket();
            packet.Write(CombatPoints);
            packet.Write(EyeOfTheMenace);
            packet.Write(GoldenTooth);
            packet.Write(CoreOfFear);
            packet.Write(AnekronianApple);
            packet.Write(primaryAbilityInt);
            packet.Write(secondaryAbilityInt);
            packet.Send(toWho, fromWho);
        }

        public override void clientClone(ModPlayer clientClone)
        {
            TerrorbornPlayer clone = clientClone as TerrorbornPlayer;
        }

        public override TagCompound Save()
        {
            return new TagCompound {
                {"CombatPoints", CombatPoints},
                {"EyeOfTheMenace", EyeOfTheMenace},
                {"GoldenTooth", GoldenTooth},
                {"CoreOfFear", CoreOfFear},
                {"AnekronianApple", AnekronianApple},
                {"DemonicLense", DemonicLense},
                {"PrimaryAbility", primaryAbilityInt},
                {"SecondaryAbility", secondaryAbilityInt},
                {"unlockedAbilities", unlockedAbilities},
                {"TerrorPercent", TerrorPercent}
            };
        }

        public override void Load(TagCompound tag)
        {
            CombatPoints = tag.GetInt("CombatPoints");
            EyeOfTheMenace = tag.GetBool("EyeOfTheMenace");
            GoldenTooth = tag.GetBool("GoldenTooth");
            CoreOfFear = tag.GetBool("CoreOfFear");
            DemonicLense = tag.GetBool("DemonicLense");
            AnekronianApple = tag.GetBool("AnekronianApple");
            primaryAbilityInt = tag.GetInt("PrimaryAbility");
            secondaryAbilityInt = tag.GetInt("SecondaryAbility");
            unlockedAbilities = tag.GetList<int>("unlockedAbilities");
            TerrorPercent = tag.GetFloat("TerrorPercent");

            primaryAbility = TerrorbornUtils.intToAbility(primaryAbilityInt);
            secondaryAbility = TerrorbornUtils.intToAbility(secondaryAbilityInt);
        }

        public override void LoadLegacy(BinaryReader reader)
        {
            int loadVersion = reader.ReadInt32();
        }

        public static TerrorbornPlayer modPlayer(Player player)
        {
            return player.GetModPlayer<TerrorbornPlayer>();
        }
    }
}
