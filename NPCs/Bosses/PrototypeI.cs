using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Cryptography.X509Certificates;
using log4net.DateFormatter;
using Terraria.Graphics.Effects;

namespace TerrorbornMod.NPCs.Bosses
{
    [AutoloadBossHead]
    public class PrototypeI : ModNPC
    {
        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void NPCLoot()
        {
            TerrorbornWorld.downedPrototypeI = true;
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PI_TreasureBag"));
            }
            else
            {
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmaScepter>());
                }
                if (choice == 1)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmoditeShotgun>());
                }
                if (choice == 2)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.PrototypeI.PlasmaticVortex>());
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.PrototypeIMask>());
                }
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.Distance(target.Center) <= 240;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.type == ProjectileID.HallowStar)
            {
                damage /= 4;
            }
        }

        bool showPortal = false;
        Vector2 portalPos = Vector2.Zero;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Prototype I");
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 240;
            npc.height = 240;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DarkMatter");
            npc.damage = 75;
            npc.defense = 48;
            npc.takenDamageMultiplier = 0.85f;
            npc.lifeMax = 45000;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 0f;
            npc.knockBackResist = 0.00f;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 60000;
            npc.defense = 60;
        }

        int coreFrame = 0;
        int coreFrameCounter = 0;
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Player target = Main.player[npc.target];
            Texture2D texture = mod.GetTexture("NPCs/Bosses/PrototypeI_core");
            coreFrameCounter--;
            if (coreFrameCounter <= 0)
            {
                coreFrameCounter = 7;
                coreFrame++;
                if (coreFrame >= 4)
                {
                    coreFrame = 0;
                }
            }
            int frameHeight = texture.Height / 4;
            Color color = npc.GetAlpha(Color.White);
            //Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight/ 2 - 4), drawColor);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);
            SpriteEffects effect = SpriteEffects.None;
            if (npc.spriteDirection == 1)
            {
                effect = SpriteEffects.FlipHorizontally;
            }
            texture = mod.GetTexture("NPCs/Bosses/PrototypeI_glow");
            Vector2 position = npc.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0);

            if (AIPhase == 0 && overallPhase == 3)
            {
                texture = mod.GetTexture("NPCs/Bosses/PrototypeI_core");
                Vector2 mirageOffset;
                mirageOffset = target.Center - npc.Center;
                Main.spriteBatch.Draw(texture, npc.Center + mirageOffset * 2 - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);
                mirageOffset = new Vector2(0, target.Center.Y - npc.Center.Y);
                Main.spriteBatch.Draw(texture, npc.Center + mirageOffset * 2 - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);
                mirageOffset = new Vector2(target.Center.X - npc.Center.X, 0);
                Main.spriteBatch.Draw(texture, npc.Center + mirageOffset * 2 - Main.screenPosition - new Vector2(texture.Width / 2, frameHeight / 2 - 4), new Rectangle(0, coreFrame * frameHeight, texture.Width, frameHeight), color);


                texture = Main.npcTexture[npc.type];
                mirageOffset = target.Center - npc.Center;
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];
                mirageOffset = new Vector2(0, target.Center.Y - npc.Center.Y);
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];
                mirageOffset = new Vector2(target.Center.X - npc.Center.X, 0);
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];


                texture = mod.GetTexture("NPCs/Bosses/PrototypeI_glow");
                mirageOffset = target.Center - npc.Center;
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];
                mirageOffset = new Vector2(0, target.Center.Y - npc.Center.Y);
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];
                mirageOffset = new Vector2(target.Center.X - npc.Center.X, 0);
                position = npc.Center - Main.screenPosition;
                position.Y += 4;
                position += mirageOffset * 2;
                Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, npc.width, npc.height), new Rectangle(0, 0, npc.width, npc.height), color, npc.rotation, new Vector2(npc.width / 2, npc.height / 2), effect, 0); texture = Main.npcTexture[npc.type];
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 6);
            if (AIPhase == 0)
            {
                phase0MaxSpeed = -10;
            }
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            if (AIPhase == 0)
            {
                if (phase0MaxSpeed > -3)
                {
                    phase0MaxSpeed = -3;
                }
            }
        }

        bool autoDirection = true;
        int dirOverride = 1;
        int AIPhase = 0;
        int nextAIPhase = -1;
        int phaseCounter = 300;
        int attackCounter1 = 0;
        int attackCounter2 = 0;
        int attackCounter3 = 0;
        bool charging = false;
        float spinSpeed = 5;
        int attackDirection = 0;
        int bulletHellAttackChoice = 0;
        float spinAttackRotation = 0;
        float spinAttackRotationSpeed = 0;
        bool spinAttackTelegraph = false;

        bool vertical = false;
        int telegraphCounter = 8;
        bool dashTelegraph = false;

        float phase0MaxSpeed = 0;

        int overallPhase = 1;

        void PreAIPhaseSetup(int phase, Player target)
        {
            if (phase == 0)
            {
                //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, target.Center);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)target.Center.X, (int)target.Center.Y, 0, pitchOffset: 0.5f);
                phase0MaxSpeed = 0;
                phaseCounter = 300;
                attackCounter1 = 90;
                teleportToPortal();
                if (overallPhase == 3)
                {
                    laserCounter = 60;
                    int choice = Main.rand.Next(4);
                    if (choice == 0)
                    {
                        nextAIPhase = 1;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 2;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 3;
                    }
                    if (choice == 3)
                    {
                        nextAIPhase = 4;
                    }
                }
                else
                {
                    int choice = Main.rand.Next(3);
                    if (choice == 0)
                    {
                        nextAIPhase = 1;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 2;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 4;
                    }
                }
            }
            if (phase == 1)
            {
                charging = false;
                teleportToPortal();
                phaseCounter = 3;
                attackCounter1 = 90;
                if (!(overallPhase == 2))
                {
                    attackCounter1 = 120;
                }
                if (overallPhase == 3)
                {
                    int choice = Main.rand.Next(4);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 2;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 3;
                    }
                    if (choice == 3)
                    {
                        nextAIPhase = 4;
                    }
                }
                else
                {
                    int choice = Main.rand.Next(3);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 2;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 4;
                    }
                }
            }
            if (phase == 2)
            {
                charging = false;
                if (overallPhase == 3)
                {
                    int choice = Main.rand.Next(4);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 1;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 3;
                    }
                    if (choice == 3)
                    {
                        nextAIPhase = 4;
                    }
                }
                else
                {
                    int choice = Main.rand.Next(3);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 1;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 4;
                    }
                }
                attackCounter1 = 75;
                attackCounter2 = 0;
                if (overallPhase == 3 && vertical)
                {
                    if (npc.Center.Y > target.Center.Y)
                    {
                        attackDirection = 1;
                    }
                    else
                    {
                        attackDirection = -1;
                    }
                }
                else
                {
                    if (npc.Center.X > target.Center.X)
                    {
                        attackDirection = 1;
                    }
                    else
                    {
                        attackDirection = -1;
                    }
                }

                DustExplosion(npc.Center, 0, 360, 30, 74, 1.5f, true);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
            }
            if (phase == 3)
            {
                spinAttackTelegraph = false;
                autoDirection = false;
                spinSpeed = 15;
                phaseCounter = 360;
                attackCounter1 = 30;
                attackCounter2 = 90;
                spinAttackRotationSpeed = 0;
                npc.position = target.Center - new Vector2(npc.width / 2, npc.height / 2);
                npc.position.Y -= 600;
                spinAttackRotation = 0 - MathHelper.ToRadians(90);
                int choice = Main.rand.Next(4);
                if (choice == 0)
                {
                    nextAIPhase = 0;
                }
                if (choice == 1)
                {
                    nextAIPhase = 1;
                }
                if (choice == 2)
                {
                    nextAIPhase = 2;
                }
                if (choice == 3)
                {
                    nextAIPhase = 4;
                }

                int choice2 = Main.rand.Next(2);
                if (choice2 == 0)
                {
                    attackDirection = 1;
                }
                else
                {
                    attackDirection = -1;
                }
                dirOverride = attackDirection;
                DustExplosion(npc.Center, 0, 360, 30, 74, 1.5f, true);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
            }
            if (phase == 4)
            {
                //Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)target.Center.X, (int)target.Center.Y, 0, pitchOffset: -0.5f);
                teleportToPortal();
                attackCounter1 = 0;
                phaseCounter = 4;
                if (overallPhase == 3)
                {
                    int choice = Main.rand.Next(3);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 1;
                    }
                    if (choice == 2)
                    {
                        nextAIPhase = 2;
                    }
                    if (choice == 3)
                    {
                        nextAIPhase = 3;
                    }
                }
                else
                {
                    int choice = Main.rand.Next(2);
                    if (choice == 0)
                    {
                        nextAIPhase = 0;
                    }
                    if (choice == 1)
                    {
                        nextAIPhase = 2;
                    }
                    //if (choice == 2)
                    //{
                    //    nextAIPhase = 4;
                    //}
                }
            }
        }

        bool ShockwaveActivated = false;
        public override void AI()
        {
            portalEffect();
            if (nextAIPhase == -1)
            {
                nextAIPhase = 1;
                int proj = Projectile.NewProjectile(Main.player[npc.target].Center, Vector2.Zero, ModContent.ProjectileType<TelegraphArrowP1>(), 0, 0);
                Main.projectile[proj].ai[0] = npc.whoAmI;
            }
            npc.TargetClosest(false);
            Player target = Main.player[npc.target];
            if (dashTelegraph)
            {
                telegraphCounter--;
                if (telegraphCounter <= 0)
                {
                    telegraphCounter = 8;
                    if (vertical && overallPhase > 1)
                    {
                        Projectile.NewProjectile(new Vector2(target.Center.X, target.Center.Y + 2000), new Vector2(0, -10), ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
                    }
                    else
                    {
                        Projectile.NewProjectile(new Vector2(target.Center.X - 2000, target.Center.Y), new Vector2(10, 0), ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
                    }
                }
            }
            if (!Main.dayTime && !target.dead)
            {



                //-------------------PHASE ONE-------------------
                if (overallPhase == 1)
                {
                    if (AIPhase == 0) // Spin Attack
                    {
                        spinTowardsPlayer(1.1f, 5f);
                        spinSpeed = 10;
                        autoDirection = true;
                        phaseCounter--;
                        if (phaseCounter == 60)
                        {
                            setUpNextAIPhasePortals(nextAIPhase);
                        }
                        if (phaseCounter <= 0)
                        {
                            AIPhase = nextAIPhase;
                            PreAIPhaseSetup(AIPhase, target);
                        }
                    }
                    if (AIPhase == 1) //Charge attack
                    {
                        float chargeSpeed = 20 + (3 - phaseCounter) * 8;
                        if (attackCounter1 > 0)
                        {
                            if (npc.Distance(target.Center) > 500)
                            {
                                preChargeLineup(npc.DirectionTo(target.Center), 1.2f, 0.88f);
                            }
                            else
                            {
                                preChargeLineup(npc.DirectionTo(target.Center), -1.2f, 0.88f);
                            }
                            attackCounter1--;
                            spinSpeed = 4;
                            autoDirection = true;

                            if (attackCounter1 == (int)(attackCounter1 / 8) * 8)
                            {
                                Vector2 telegraphVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity)) * 5;
                                Projectile.NewProjectile(npc.Center, telegraphVelocity, ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
                            }
                        }
                        else if (!charging)
                        {
                            setCharging(chargeSpeed, npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity)));
                            attackCounter2 = 60;
                            if (phaseCounter == 1)
                            {
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                            Terraria.Audio.SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                            Vector2 telegraphVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity)) * 5;
                            Projectile.NewProjectile(npc.Center, telegraphVelocity, ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
                        }
                        else if (charging)
                        {
                            autoDirection = false;
                            spinSpeed = 8;
                            attackCounter2--;
                            if (attackCounter2 <= 0)
                            {
                                attackCounter1 = 120;
                                phaseCounter--;
                                if (phaseCounter <= 0)
                                {
                                    AIPhase = nextAIPhase;
                                    PreAIPhaseSetup(AIPhase, target);
                                }
                                charging = false;
                            }
                        }
                    }
                    if (AIPhase == 2) //Horizontal charge
                    {
                        if (!charging)
                        {
                            Vector2 targetPosition = target.Center + new Vector2(750 * attackDirection, 0);
                            preChargeLineup(npc.DirectionTo(targetPosition), 3, 0.89f);
                            autoDirection = true;
                            spinSpeed = 5;
                            if (npc.Distance(targetPosition) <= 25)
                            {
                                npc.velocity.X = -25 * attackDirection;
                                npc.velocity.Y = 0;
                                charging = true;
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                            attackCounter2--;
                            if (attackCounter2 <= 0)
                            {
                                attackCounter2 = 5;
                                Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                                float speed = 22;
                                int laserDamage = 95;
                                laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                                Vector2 velocity = rotation * speed;
                                Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, npc.position);
                            }
                        }
                        if (charging)
                        {
                            dashTelegraph = false;
                            autoDirection = false;
                            spinSpeed = 10;
                            attackCounter1--;
                            if (attackCounter1 <= 0)
                            {
                                AIPhase = nextAIPhase;
                                PreAIPhaseSetup(AIPhase, target);
                            }
                        }
                    }
                    //if (AIPhase == 3) //Spin around the player
                    //{
                    //    velocity = Vector2.Zero;

                    //    if (Math.Abs(spinAttackRotationSpeed) < MathHelper.ToRadians(2.5f))
                    //    {
                    //        spinAttackRotationSpeed += MathHelper.ToRadians(0.025f * attackDirection);
                    //    }
                    //    spinAttackRotation += spinAttackRotationSpeed;
                    //    npc.position = target.Center + (spinAttackRotation.ToRotationVector2() * 600) - new Vector2(npc.width / 2, npc.height / 2);

                    //    attackCounter1--;
                    //    if (attackCounter1 <= 0)
                    //    {
                    //        attackCounter1 = 5;
                    //        Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                    //        float speed = 15;
                    //        int laserDamage = 90;
                    //        laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                    //        Vector2 velocity = rotation * speed;
                    //        Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                    //        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, npc.position);
                    //    }

                    //    phaseCounter--;
                    //    if (phaseCounter == 60)
                    //    {
                    //        setUpNextAIPhasePortals(nextAIPhase);
                    //    }
                    //    if (phaseCounter <= 0)
                    //    {
                    //        AIPhase = nextAIPhase;
                    //        PreAIPhaseSetup(AIPhase, target);
                    //    }
                    //}
                    if (AIPhase == 4) // 'Bullet Hell'
                    {
                        npc.velocity = Vector2.Zero;
                        attackCounter1--;
                        if (attackCounter1 <= 0)
                        {
                            if (phaseCounter == 4)
                            {
                                bulletHellAttackChoice = 0;
                            }
                            else
                            {
                                bulletHellAttackChoice++;
                            }
                            teleportToPortal();
                            phaseCounter--;
                            if (phaseCounter == 1)
                            {
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                            else if (phaseCounter != 0)
                            {
                                setPortal(target.Center + new Vector2(Main.rand.Next(-800, 800), Main.rand.Next(-600, 600)));
                            }
                            attackCounter1 = 95;

                            if (bulletHellAttackChoice == 0)
                            {
                                float rotation = MathHelper.ToRadians(Main.rand.Next(360));
                                int laserDamage = 100;
                                laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                                int laserCount = 13;
                                float speed = 5;
                                for (int i = 0; i < laserCount; i++)
                                {
                                    rotation += MathHelper.ToRadians(360 / laserCount);
                                    Vector2 velocity = rotation.ToRotationVector2() * speed;
                                    int type = ModContent.ProjectileType<NightmareShockwave>();
                                    Projectile.NewProjectile(npc.Center, velocity, type, laserDamage, 0);
                                }
                            }
                            if (bulletHellAttackChoice == 1)
                            {
                                int Direction2 = 1;
                                if (Main.rand.Next(2) == 0)
                                {
                                    Direction2 = -1;
                                }
                                float Speed2 = 14f;
                                Vector2 vector82 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                                int damage2 = 90;

                                damage2 = Main.expertMode ? (damage2 / 4) : (damage2 / 2);
                                int type2 = ModContent.ProjectileType<LargeNightmareFlame>();
                                float rotation2 = MathHelper.ToRadians(Main.rand.Next(360));
                                for (int i = 0; i < 15; i++)
                                {
                                    rotation2 += MathHelper.ToRadians(360 / 15);
                                    Vector2 actualSpeed = new Vector2((float)((Math.Cos(rotation2) * Speed2) * -1), (float)((Math.Sin(rotation2) * Speed2) * -1));
                                    actualSpeed += (actualSpeed / 2).RotatedBy(MathHelper.ToRadians(90 * Direction2));
                                    int num54 = Projectile.NewProjectile(vector82.X, vector82.Y, actualSpeed.X, actualSpeed.Y, type2, damage2, 0f, 0, Direction2);
                                }
                            }
                            if (bulletHellAttackChoice == 2)
                            {
                                Vector2 direction = npc.DirectionTo(target.Center);
                                float speed = 20;
                                Vector2 initialVelocity = speed * direction;
                                int type = ModContent.ProjectileType<HomingPlasma>();
                                int damage = 90;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);

                                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                                {
                                    Vector2 projectileVelocity = initialVelocity.RotatedByRandom(MathHelper.ToRadians(45)) * Main.rand.NextFloat(0.8f, 1.2f);
                                    Projectile.NewProjectile(npc.Center, projectileVelocity, type, damage, 0, ai0: 1f);
                                }
                            }
                        }

                        if (phaseCounter <= 0)
                        {
                            AIPhase = nextAIPhase;
                            PreAIPhaseSetup(AIPhase, target);
                        }
                    }
                }
                if (overallPhase == 2) //In between phases
                {
                    spinSpeed /= 2;
                    if (Math.Round(spinSpeed) == 0)
                    {
                        spinSpeed = 0;
                    }
                    if (!ShockwaveActivated)
                    {
                        phaseCounter--;
                        if (phaseCounter <= 0)
                        {
                            ShockwaveActivated = true;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                            if (Main.netMode != NetmodeID.Server && !Filters.Scene["Shockwave"].IsActive())
                            {
                                Filters.Scene.Activate("Shockwave", npc.Center).GetShader().UseColor(1, 3, 45).UseTargetPosition(npc.Center);
                            }
                            setUpNextAIPhasePortals(nextAIPhase);
                        }
                    }
                    else
                    {
                        effectCounter--;
                        if (effectCounter <= 0)
                        {
                            effectCounter = 15;
                            DustExplosion(npc.Center, 0, 360, 60, 74, 1.5f, true);
                        }
                        TerrorbornMod.ScreenShake(10f);
                        attackCounter1--;
                        if (attackCounter1 <= 0)
                        {
                            if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                            {
                                Filters.Scene["Shockwave"].Deactivate();
                            }
                            overallPhase = 3;
                            ShockwaveActivated = false;
                            npc.dontTakeDamage = false;
                            AIPhase = nextAIPhase;
                            PreAIPhaseSetup(AIPhase, target);
                        }
                    }
                    if (Main.netMode != NetmodeID.Server && Filters.Scene["Shockwave"].IsActive())
                    {
                        float progress = (90f - attackCounter1) / 60f;
                        Filters.Scene["Shockwave"].GetShader().UseProgress(progress).UseOpacity(100 * (1 - progress / 3f));
                    }
                    npc.velocity *= 0.95f;
                }





                //-------------------PHASE TWO-------------------
                if (overallPhase == 3)
                {
                    if (AIPhase == 0) // Spin Attack
                    {
                        spinSpeed = 10;
                        autoDirection = true;
                        if (phaseCounter == 30)
                        {
                            setUpNextAIPhasePortals(nextAIPhase);
                        }
                        if (phaseCounter <= 0)
                        {
                            npc.velocity *= 0.93f;
                            attackCounter1--;
                            if (attackCounter1 <= 0)
                            {
                                AIPhase = nextAIPhase;
                                PreAIPhaseSetup(AIPhase, target);
                            }
                        }
                        else
                        {
                            phaseCounter--;
                            spinTowardsPlayer(1.1f, 5f, 23, 38);
                        }
                    }
                    if (AIPhase == 1) //Charge attack
                    {
                        float chargeSpeed = 20 + (3 - phaseCounter) * 8;
                        if (attackCounter1 > 0)
                        {
                            if (phaseCounter <= 0)
                            {
                                if (npc.Distance(target.Center) > 500)
                                {
                                    preChargeLineup(npc.DirectionTo(target.Center), 1f, 0.92f);
                                }
                                else
                                {
                                    preChargeLineup(npc.DirectionTo(target.Center), -3f, 0.88f);
                                }
                                attackCounter1--;
                                if (attackCounter1 <= 0)
                                {
                                    AIPhase = nextAIPhase;
                                    PreAIPhaseSetup(AIPhase, target);
                                }
                            }
                            else
                            {
                                if (npc.Distance(target.Center) > 500)
                                {
                                    if (attackCounter1 == 1)
                                    {
                                        preChargeLineup(npc.DirectionTo(target.Center), 0.8f, 0.88f);
                                    }
                                    else
                                    {
                                        preChargeLineup(npc.DirectionTo(target.Center), 1.2f, 0.88f);
                                    }
                                }
                                else
                                {
                                    preChargeLineup(npc.DirectionTo(target.Center), -1.2f, 0.88f);
                                }
                                attackCounter1--;
                                spinSpeed = 4;
                                autoDirection = true;
                                if (attackCounter1 == (int)(attackCounter1 / 8) * 8)
                                {
                                    Vector2 telegraphVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity)) * 5;
                                    if (phaseCounter == 1)
                                    {
                                        telegraphVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity * 0.75f)) * 5;
                                    }
                                    Projectile.NewProjectile(npc.Center, telegraphVelocity, ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
                                }
                            }
                        }
                        else if (!charging)
                        {
                            if (phaseCounter == 1)
                            {
                                setCharging(chargeSpeed, npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity * 0.75f)));
                            }
                            else
                            {
                                setCharging(chargeSpeed, npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity)));
                            }
                            if (phaseCounter == 3)
                            {
                                int damage = 100;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                                Vector2 projVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity));
                                float offset = 50;
                                Projectile.NewProjectile(npc.Center + projVelocity * npc.width * 0.75f, projVelocity * 5, ModContent.ProjectileType<NightmareShockwave>(), damage, 0);
                                
                                Projectile.NewProjectile(npc.Center + projVelocity.RotatedBy(MathHelper.ToRadians(offset)) * npc.width * 0.75f, projVelocity.RotatedBy(MathHelper.ToRadians(offset)) * 5, ModContent.ProjectileType<NightmareShockwave>(), damage, 0);

                                Projectile.NewProjectile(npc.Center + projVelocity.RotatedBy(MathHelper.ToRadians(-offset)) * npc.width * 0.75f, projVelocity.RotatedBy(MathHelper.ToRadians(-offset)) * 5, ModContent.ProjectileType<NightmareShockwave>(), damage, 0);
                            }
                            if (phaseCounter == 2)
                            {
                                int damage = 100;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                                Vector2 projVelocity = npc.DirectionTo(target.Center + (npc.Distance(target.Center) / chargeSpeed * target.velocity));
                                float shockwaveCount = 11;
                                for (int i = 0; i < shockwaveCount + 1; i++)
                                {
                                    projVelocity = projVelocity.RotatedBy(MathHelper.ToRadians(360 / (shockwaveCount + 1)));
                                    Projectile.NewProjectile(npc.Center + projVelocity * npc.width * 0.75f, projVelocity * 5, ModContent.ProjectileType<NightmareShockwave>(), damage, 0);
                                }
                            }
                            attackCounter2 = 60;
                            if (phaseCounter == 1)
                            {
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                            Terraria.Audio.SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                        }
                        else if (charging)
                        {
                            autoDirection = false;
                            spinSpeed = 8;
                            attackCounter2--;
                            if (attackCounter2 == (int)(attackCounter2 / 12) * 12 && phaseCounter == 1)
                            {
                                int damage = 100;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                                Vector2 projVelocity = npc.velocity.ToRotation().ToRotationVector2() * 5;
                                Projectile.NewProjectile(npc.Center, projVelocity.RotatedBy(MathHelper.ToRadians(90)), ModContent.ProjectileType<NightmareShockwave>(), damage, 0);
                                Projectile.NewProjectile(npc.Center, projVelocity.RotatedBy(MathHelper.ToRadians(-90)), ModContent.ProjectileType<NightmareShockwave>(), damage, 0);
                            }
                            if (attackCounter2 <= 0)
                            {
                                attackCounter1 = 90;
                                phaseCounter--;
                                if (attackCounter1 == 1)
                                {
                                    attackCounter1 = 120;
                                }
                                if (attackCounter1 == 0)
                                {
                                    attackCounter1 = 85;
                                }
                                charging = false;
                            }
                        }
                    }
                    if (AIPhase == 2) //Horizontal/Vertical charge
                    {
                        if (!charging && attackCounter1 > 0)
                        {
                            Vector2 targetPosition = target.Center + new Vector2(750 * attackDirection, 0);
                            if (vertical)
                            {
                                targetPosition = target.Center + new Vector2(0, 750 * attackDirection);
                            }

                            preChargeLineup(npc.DirectionTo(targetPosition), 3, 0.89f);

                            autoDirection = true;
                            spinSpeed = 5;
                            if (npc.Distance(targetPosition) <= 25)
                            {
                                if (vertical)
                                {
                                    npc.velocity.Y = -25 * attackDirection;
                                    npc.velocity.X = 0;
                                }
                                else
                                {
                                    npc.velocity.X = -25 * attackDirection;
                                    npc.velocity.Y = 0;
                                }
                                charging = true;
                                setUpNextAIPhasePortals(nextAIPhase);
                                attackCounter2 = 0;
                            }

                            attackCounter2--;
                            if (attackCounter2 <= 0)
                            {
                                attackCounter2 = 5;
                                Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                                float speed = 26;
                                int laserDamage = 120;
                                laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                                Vector2 velocity = rotation * speed;
                                Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, npc.position);
                            }
                        }
                        else if (!charging)
                        {
                            attackCounter3--;
                            if (attackCounter3 <= 0)
                            {
                                AIPhase = nextAIPhase;
                                PreAIPhaseSetup(AIPhase, target);
                            }
                            npc.velocity *= 0.95f;
                        }
                        if (charging)
                        {
                            dashTelegraph = false;
                            attackCounter2--;
                            if (attackCounter2 <= 0)
                            {
                                int damage = 135;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                                attackCounter2 = 5;
                                Vector2 offset = new Vector2(Main.rand.Next(0, npc.width), Main.rand.Next(0, npc.height));
                                int projectile = Projectile.NewProjectile(npc.position + offset, Vector2.Zero, ModContent.ProjectileType<PlasmaticVision>(), damage, 0);
                                Main.projectile[projectile].ai[0] = attackCounter1 + 30;
                            }
                            autoDirection = false;
                            spinSpeed = 10;
                            attackCounter1--;
                            if (attackCounter1 <= 0)
                            {
                                charging = false;
                                attackCounter3 = 60;
                            }
                        }
                    }
                    if (AIPhase == 3) //Spin around the player
                    {
                        if (target.wingTime < 5)
                        {
                            target.wingTime = 5;
                        }
                        if (phaseCounter <= 0)
                        {
                            if (Math.Abs(spinAttackRotationSpeed) < MathHelper.ToRadians(8f))
                            {
                                spinAttackRotationSpeed -= MathHelper.ToRadians(0.050f * attackDirection);
                            }
                            spinAttackRotation += spinAttackRotationSpeed;
                            npc.position = target.Center + (spinAttackRotation.ToRotationVector2() * 600) - new Vector2(npc.width / 2, npc.height / 2);
                            attackCounter2--;
                            if (attackCounter2 <= 0)
                            {
                                AIPhase = nextAIPhase;
                                PreAIPhaseSetup(AIPhase, target);
                            }
                        }
                        else
                        {
                            npc.velocity = Vector2.Zero;

                            if (Math.Abs(spinAttackRotationSpeed) < MathHelper.ToRadians(8f))
                            {
                                spinAttackRotationSpeed += MathHelper.ToRadians(0.020f * attackDirection);
                            }
                            spinAttackRotation += spinAttackRotationSpeed;
                            npc.position = target.Center + (spinAttackRotation.ToRotationVector2() * 600) - new Vector2(npc.width / 2, npc.height / 2);

                            attackCounter1--;
                            if (attackCounter1 <= 0)
                            {
                                attackCounter1 = 6;
                                Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                                float speed = 6;
                                int laserDamage = 120;
                                laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                                Vector2 velocity = rotation * speed;
                                int projID = Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                                Main.projectile[projID].ai[0] = 1;
                                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, npc.position);
                            }

                            phaseCounter--;
                            if (phaseCounter == 180)
                            {
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                        }
                    }
                    if (AIPhase == 4) // 'Bullet Hell'
                    {
                        npc.velocity = Vector2.Zero;
                        attackCounter1--;
                        if (attackCounter1 <= 0)
                        {
                            teleportToPortal();
                            if (phaseCounter == 4)
                            {
                                bulletHellAttackChoice = 0;
                            }
                            else
                            {
                                bulletHellAttackChoice++;
                            }
                            phaseCounter--;
                            if (phaseCounter == 1)
                            {
                                setUpNextAIPhasePortals(nextAIPhase);
                            }
                            else if (phaseCounter != 0)
                            {
                                setPortal(target.Center + new Vector2(Main.rand.Next(-800, 800), Main.rand.Next(-600, 600)));
                            }
                            attackCounter1 = 110;
                            if (bulletHellAttackChoice == 0)
                            {
                                float rotation = MathHelper.ToRadians(Main.rand.Next(360));
                                int laserDamage = 120;
                                laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
                                int laserCount = 16;
                                float speed = 5;
                                for (int i = 0; i < laserCount; i++)
                                {
                                    rotation += MathHelper.ToRadians(360 / laserCount);
                                    Vector2 velocity = rotation.ToRotationVector2() * speed;
                                    int type = ModContent.ProjectileType<NightmareShockwave>();
                                    Projectile.NewProjectile(npc.Center, velocity, type, laserDamage, 0);
                                }
                            }
                            if (bulletHellAttackChoice == 1)
                            {
                                int Direction2 = 1;
                                if (Main.rand.Next(2) == 0)
                                {
                                    Direction2 = -1;
                                }
                                float Speed2 = 14f;
                                Vector2 vector82 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                                int damage2 = 130;

                                damage2 = Main.expertMode ? (damage2 / 4) : (damage2 / 2);
                                int type2 = ModContent.ProjectileType<LargeNightmareFlame>();
                                float rotation2 = MathHelper.ToRadians(Main.rand.Next(360));
                                for (int i = 0; i < 16; i++)
                                {
                                    rotation2 += MathHelper.ToRadians(360 / 16);
                                    Vector2 actualSpeed = new Vector2((float)((Math.Cos(rotation2) * Speed2) * -1), (float)((Math.Sin(rotation2) * Speed2) * -1));
                                    actualSpeed += (actualSpeed / 2).RotatedBy(MathHelper.ToRadians(90 * Direction2));
                                    int num54 = Projectile.NewProjectile(vector82.X, vector82.Y, actualSpeed.X, actualSpeed.Y, type2, damage2, 0f, 0, Direction2);
                                }
                                Direction2 *= -1;
                                for (int i = 0; i < 16; i++)
                                {
                                    rotation2 += MathHelper.ToRadians(360 / 16);
                                    Vector2 actualSpeed = new Vector2((float)((Math.Cos(rotation2) * Speed2) * -1), (float)((Math.Sin(rotation2) * Speed2) * -1));
                                    actualSpeed += (actualSpeed / 2).RotatedBy(MathHelper.ToRadians(90 * Direction2));
                                    int num54 = Projectile.NewProjectile(vector82.X, vector82.Y, actualSpeed.X, actualSpeed.Y, type2, damage2, 0f, 0, Direction2);
                                }
                            }
                            if (bulletHellAttackChoice == 2)
                            {
                                Vector2 direction = npc.DirectionTo(target.Center);
                                float speed = 26;
                                Vector2 initialVelocity = speed * direction;
                                int type = ModContent.ProjectileType<HomingPlasma>();
                                int damage = 110;
                                damage = Main.expertMode ? (damage / 4) : (damage / 2);

                                for (int i = 0; i < Main.rand.Next(5, 8); i++)
                                {
                                    Vector2 projectileVelocity = initialVelocity.RotatedByRandom(MathHelper.ToRadians(45)) * Main.rand.NextFloat(0.8f, 1.2f);
                                    Projectile.NewProjectile(npc.Center, projectileVelocity, type, damage, 0, ai0: 1f);
                                }
                            }
                        }

                        if (phaseCounter <= 0)
                        {
                            AIPhase = nextAIPhase;
                            PreAIPhaseSetup(AIPhase, target);
                        }
                    }
                }
                if (autoDirection == true)
                {
                    if (npc.Center.X > target.Center.X)
                    {
                        dirOverride = -1;
                    }
                    else
                    {
                        dirOverride = 1;
                    }
                }
            }
            else
            {
                npc.velocity.Y -= 0.5f;
                if (npc.position.Y <= target.position.Y - 1200)
                {
                    npc.active = false;
                }
            }
            npc.velocity = npc.velocity;
            npc.direction = dirOverride;
            npc.spriteDirection = -dirOverride;
            npc.rotation += MathHelper.ToRadians(spinSpeed * npc.direction);
        }

        void setUpNextAIPhasePortals(int phase)
        {
            Player target = Main.player[npc.target];
            if (phase == 4)
            {
                setPortal(target.Center + new Vector2(Main.rand.Next(-800, 800), Main.rand.Next(-600, 600)));
            }
            if (phase == 0)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float distance = Main.rand.Next(800, 1000);
                setPortal(direction * distance + target.Center);
            }
            if (phase == 2)
            {
                dashTelegraph = true;
                if (vertical)
                {
                    vertical = false;
                }
                else
                {
                    vertical = true;
                }
            }
            if (phase == 3)
            {
                spinAttackTelegraph = true;
            }
            if (phase == 1)
            {
                setPortal(target.Center);
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0 && overallPhase == 1)
            {
                npc.lifeMax = 60000;
                if (!Main.expertMode)
                {
                    npc.lifeMax = 45000;
                }
                npc.life = npc.lifeMax;
                showPortal = false;
                overallPhase = 2;
                phaseCounter = 60;
                attackCounter1 = 90;
                npc.dontTakeDamage = true;
                npc.damage += 20;
                nextAIPhase = Main.rand.Next(5);
                vertical = Main.rand.NextBool();
                dashTelegraph = false;
            }
        }

        int laserCounter = 0;
        void spinTowardsPlayer(float distanceMultiplier = 1, float speed = 10, float laserSpeed = 25, int laserCooldown = 30, float cloneAlpha = 0)
        {
            Player target = Main.player[npc.target];
            Vector2 rotation = npc.DirectionTo(target.Center);
            Vector2 addedFromDistance = rotation * (npc.Distance(target.Center) / 100) * distanceMultiplier;
            Vector2 actualVelocity = rotation * speed + addedFromDistance;
            if (actualVelocity.Length() > phase0MaxSpeed)
            {
                actualVelocity = actualVelocity.ToRotation().ToRotationVector2() * phase0MaxSpeed;
            }
            phase0MaxSpeed += 0.25f;
            npc.velocity = actualVelocity;

            Vector2 laserVelocity = rotation * laserSpeed;
            int laserDamage = 100;
            laserDamage = Main.expertMode ? (laserDamage / 4) : (laserDamage / 2);
            laserCounter--;
            if (laserCounter <= 0)
            {
                laserCounter = laserCooldown;
                Vector2 mirageOffset = Vector2.Zero;
                int proj = Projectile.NewProjectile(npc.Center + mirageOffset * 2, laserVelocity * new Vector2(1, 1), ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                if (overallPhase == 3)
                {
                    Main.projectile[proj].ai[0] = 2;
                }
                if (overallPhase == 3)
                {
                    mirageOffset = target.Center - npc.Center;
                    proj = Projectile.NewProjectile(npc.Center + mirageOffset * 2, laserVelocity * new Vector2(-1, -1), ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                    Main.projectile[proj].ai[0] = 2;
                    mirageOffset = new Vector2(0, target.Center.Y - npc.Center.Y);
                    proj = Projectile.NewProjectile(npc.Center + mirageOffset * 2, laserVelocity * new Vector2(1, -1), ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                    Main.projectile[proj].ai[0] = 2;
                    mirageOffset = new Vector2(target.Center.X - npc.Center.X, 0);
                    proj = Projectile.NewProjectile(npc.Center + mirageOffset * 2, laserVelocity * new Vector2(-1, 1), ModContent.ProjectileType<Projectiles.CursedBeam>(), laserDamage, 0);
                    Main.projectile[proj].ai[0] = 2;
                }
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item33, npc.position);
            }
        }

        int effectCounter = 0;
        void portalEffect()
        {
            if (showPortal)
            {
                effectCounter--;
                if (effectCounter <= 0)
                {
                    effectCounter = 15;
                    DustExplosion(portalPos, 0, 360, 30, 74, 1.5f, true);
                }
            }
            if (spinAttackTelegraph)
            {
                effectCounter--;
                if (effectCounter <= 0)
                {
                    effectCounter = 15; 
                    Vector2 position = Main.player[npc.target].Center;
                    position.Y -= 600;
                    DustExplosion(position, 0, 360, 60, 74, 1.5f, true);
                }
            }
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

        void setCharging(float speed, Vector2 direction)
        {
            npc.velocity = speed * direction;
            charging = true;
        }

        void setPortal(Vector2 position)
        {
            showPortal = true;
            portalPos = position;
        }

        void teleportToPortal()
        {
            showPortal = false;
            npc.position = portalPos - new Vector2(npc.width / 2, npc.height / 2);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item60, portalPos);
            int proj = Projectile.NewProjectile(Main.player[npc.target].Center, Vector2.Zero, ModContent.ProjectileType<TelegraphArrowP1>(), 0, 0);
            Main.projectile[proj].ai[0] = npc.whoAmI;
        }

        void preChargeLineup(Vector2 direction, float speed, float drag = 0.92f)
        {
            npc.velocity += direction * speed;
            npc.velocity *= drag;
        }
    }

    class CursedSpiritFlames : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        int TimeUntilSpeedUp = 50;
        bool HasSpedUp = false;
        bool Spawn = true;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 250;
        }
        private void OnSpawn()
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20);
            Spawn = false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 2);
            projectile.timeLeft = 1;
        }
        public override void AI()
        {
            if (Spawn)
            {
                OnSpawn();
            }
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74);
            Main.dust[dust].velocity = Vector2.Zero;
            TimeUntilSpeedUp--;
            if (TimeUntilSpeedUp <= 0 && !HasSpedUp)
            {
                projectile.velocity *= 3;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item45);
                HasSpedUp = true;
            }
        }
    }

    class HomingPlasma : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 2);
            projectile.timeLeft = 1;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 25;
            projectile.height = 25;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 2.25f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.AngleTo(target.Center), MathHelper.ToRadians(1f)).ToRotationVector2() * projectile.velocity.Length();
        }
    }


    class PlasmaticVision : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 2);
            projectile.timeLeft = 1;
        }
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 34;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 360;
        }

        float homingSpeed = 0;
        bool launched = false;

        public override void AI()
        {
            if (homingSpeed == 0)
            {
                homingSpeed = Main.rand.NextFloat(0.5f, 0.8f);
            }

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            if (projectile.ai[0] <= 0)
            {
                if (!launched)
                {
                    Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                    Vector2 direction = projectile.DirectionTo(target.Center);
                    Vector2 speed = direction * 35;
                    projectile.velocity = speed;
                    launched = true;
                }
            }
            else
            {
                Player target = Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)];
                Vector2 direction = projectile.DirectionTo(target.Center);
                projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(90);
                if (target.Center.X <= projectile.Center.X)
                {
                    projectile.spriteDirection = -1;
                }
                //Vector2 speed = direction * homingSpeed;
                //projectile.velocity += speed;
                //projectile.velocity *= 0.98f;
                projectile.ai[0]--;
            }
        }
    }

    class TelegraphArrowP1 : ModProjectile
    {
        int timeUntilLeave = 45;
        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 36;
            projectile.tileCollide = false;
            projectile.timeLeft = 1000;
            projectile.damage = 0;
        }

        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    lightColor = projectile.GetAlpha(Color.White);
        //    return true;
        //}

        public override void AI()
        {
            NPC target = Main.npc[(int)projectile.ai[0]];
            Player player = Main.player[Main.myPlayer];
            Vector2 direction = player.DirectionTo(target.Center);
            int distance = 100;
            projectile.position = direction * distance + player.Center;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.rotation = direction.ToRotation() + MathHelper.ToRadians(90);

            if (timeUntilLeave > 0)
            {
                timeUntilLeave--;
            }
            else
            {
                DustExplosion(projectile.Center, 0, 12, 7, 74, 2f, true);
                projectile.timeLeft = 1;
                projectile.active = false;
            }
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
                dust.noLight = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
    }
}