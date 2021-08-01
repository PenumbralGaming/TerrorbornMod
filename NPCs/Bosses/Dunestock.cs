using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Events;
using Terraria.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.UI;

namespace TerrorbornMod.NPCs.Bosses
{
    [AutoloadBossHead]
    class Dunestock : ModNPC
    {
        int FrameWait = 10;
        int frame = 1;
        bool Flying = false;
        bool start = true;

        public Vector2 findTileUnderPosition(int positionX, int positionY)
        {
            for (Vector2 v = new Vector2(positionX, positionY); Main.tile[v.ToTileCoordinates().X, v.ToTileCoordinates().Y].active(); v.Y++) { }
            return new Vector2(positionX, positionY);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dunestock");
            Main.npcFrameCount[npc.type] = 12;
            NPCID.Sets.TrailCacheLength[npc.type] = 8;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return Charging;
        }

        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.defense = 13;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/BlightOfTheDunes");
            npc.lifeMax = 7000;
            npc.damage = 40;
            npc.width = 100;
            npc.height = 96;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.boss = true;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 110000f;
            npc.alpha = 250;
            npc.knockBackResist = 0.00f;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.defense = 19;
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.GoldenTooth>());

            bool spawnSS = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(5);
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active && projectile.type == ModContent.ProjectileType<Abilities.StarvingStorm>())
                {
                    spawnSS = false;
                }
            }

            if (spawnSS)
            {
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.StarvingStorm>(), 0, 0, Main.myPlayer);
            }

            if (!TerrorbornWorld.downedDunestock)
            {
                TerrorbornWorld.downedDunestock = true;
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Lore.JournalEntries.Tenebris.Tenebris_Dunestock>());
            }
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DS_TreasureBag"));
            }
            else
            {
                int choice = Main.rand.Next(2);
                if (choice == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NeedleClawStaff"));
                }
                else if (choice == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Dunesting"), Stack: 750);
                }

                int item1;
                WeightedRandom<int> itemlist = new WeightedRandom<int>();
                itemlist.Add(ModContent.ItemType<Items.Equipable.Accessories.DryScarf>());
                itemlist.Add(ModContent.ItemType<Items.Equipable.Accessories.AntlionShell>());
                itemlist.Add(ModContent.ItemType<Items.Equipable.Accessories.AntlionClaw>());
                itemlist.Add(ModContent.ItemType<Items.Equipable.Accessories.Wings.AntlionWings>());
                item1 = itemlist.Get();
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, item1);
                int item2 = itemlist.Get();
                while (item2 == item1)
                {
                    item2 = itemlist.Get();
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, item2);
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Accessories.CloakOfTheWind>());
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.DunestockMask>());
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                frame++;
                if (!Flying)
                {
                    FrameWait = 5;
                }
                if (Flying)
                {
                    FrameWait = 5;
                }
                if (frame >= 4 && !Flying)
                {
                    frame = 0;
                }
                if (frame <= 8 && Flying)
                {
                    frame = 9;
                }
                if (frame >= 12 && Flying)
                {
                    frame = 9;
                }
                npc.frame.Y = frame * frameHeight;
            }
        }

        public override bool PreNPCLoot()
        {
            return true;
        }

        Vector2 LineEnd;
        bool DrawLine = false;
        float NextAttackAngle;
        private void SetLine(float TimeOffset)
        {
            LineEnd = npc.Center + npc.AngleTo(Main.player[npc.target].Center + (Main.player[npc.target].velocity * TimeOffset)).ToRotationVector2() * 2200f;
            DrawLine = true;
            NextAttackAngle = npc.AngleTo((Main.player[npc.target].Center + (Main.player[npc.target].velocity * TimeOffset)));
        }

        private void StopLine()
        {
            DrawLine = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (DrawLine)
            {
                Utils.DrawLine(spriteBatch, npc.Center, LineEnd, Color.LightYellow, Color.LightYellow, 3);
            }
            return base.PreDraw(spriteBatch, drawColor);
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (npc.life > npc.lifeMax * 0.40f)
            {
                return;
            }
            SpriteEffects effects = new SpriteEffects();
            if (npc.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(-2, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/DunestockEyesTrail"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        public void shootBurstOfProjectiles(int numProjectiles, float speedX, float speedY, int Type, Vector2 position, int Damage, int knockback)
        {
            for (int i = 0; i < numProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15)); // 15 degree spread.
                float scale = 1f - (Main.rand.NextFloat() * .3f);
                perturbedSpeed = perturbedSpeed * scale;
                int num54 = Projectile.NewProjectile(position, perturbedSpeed, Type, Damage, 0f, 0);
            }
        }

        float DeathAnimVelocityX = 0;
        int InvincibilityCounter = 300;

        //Grounded Variables
        int WarningParticlesCounter = 0;
        int AIPhase = 0;
        int LastAI = 0;
        int PhaseTimeLeft = 180;
        int BurstWait = 30;
        int ProjectilesLeft;
        int ProjectileWait;
        int NoGravNeedles = 5;
        int AttacksUntilFlight;
        float NeedleSpeed = 0;
        List<int> NextAttacks = new List<int>();

        //Flight variables
        int AttacksUntilLand;
        bool Charging = false;
        int Direction = 0;
        float BarageDirection = 0;
        Vector2 MovementTarget;

        float predictMultiplier = 0f;
        int phaseTimeLeftMax;

        int pullBackTime = 0;
        public override void AI()
        {
            npc.TargetClosest(true);
            if (Charging)
            {
                int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldFlame, Scale: 2.5f);
                Main.dust[dust].velocity = npc.velocity;
                Main.dust[dust].noGravity = true;
            }
            phaseTimeLeftMax = 280 - 220 * (npc.lifeMax - npc.life) / (int)(npc.lifeMax * 0.60f);
            Sandstorm.TimeLeft = 60;
            if (start)
            {
                typeof(Sandstorm).GetMethod("StartSandstorm", BindingFlags.Static | BindingFlags.NonPublic).Invoke((object)null, (object[])null);
                start = false;
                npc.position.X = Main.player[npc.target].position.X - npc.width / 2;
                npc.position.Y = Main.player[npc.target].position.Y - 200;
                AttacksUntilFlight = Main.rand.Next(3, 6);
            }
            npc.noGravity = Flying;
            npc.noTileCollide = Flying;
            if (!Main.player[npc.target].ZoneDesert)
            {
                if (InvincibilityCounter <= 0)
                {
                    npc.dontTakeDamage = true;
                }
                else
                {
                    InvincibilityCounter--;
                }
            }
            else
            {
                InvincibilityCounter = 300;
                npc.dontTakeDamage = false;
            }
            if (Main.player[npc.target].Distance(npc.Center) > 1500)
            {
                pullBackTime = 120;
            }
            if (Main.player[npc.target].Distance(npc.Center) < 500)
            {
                pullBackTime = 0;
            }
            if (pullBackTime > 0)
            {
                pullBackTime--;
                int SuckSpeed = 12;
                if (Main.player[npc.target].Center.X > npc.Center.X)
                {
                    Main.player[npc.target].position.X -= SuckSpeed;
                    int newDust = Dust.NewDust(Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height, DustID.GoldFlame, -50, 0, Scale: 1.75f);
                    Main.dust[newDust].noGravity = true;
                }
                else
                {
                    Main.player[npc.target].position.X += SuckSpeed;
                    int newDust = Dust.NewDust(Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height, DustID.GoldFlame, 50, 0, Scale: 1.75f);
                    Main.dust[newDust].noGravity = true;
                }
            }
            if (!Main.player[npc.target].active || Main.player[npc.target].dead)
            {
                Flying = true;
                npc.velocity.Y -= 0.1f;
                npc.velocity.X = DeathAnimVelocityX;
                if (npc.position.Y <= Main.player[npc.target].position.Y - 1500)
                {
                    npc.active = false;
                }
            }
            else
            {
                if (Main.player[npc.target].Center.X > npc.Center.X)
                {
                    npc.spriteDirection = 1;
                }
                else
                {
                    npc.spriteDirection = -1;
                }

                if (Flying)
                {
                    npc.ai[1] = 1;
                    if (!NPC.AnyNPCs(mod.NPCType("Dunestock_Lower")) && AIPhase < 9)
                    {
                        NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + npc.width / 4, mod.NPCType("Dunestock_Lower"));
                    }
                }
                else
                {
                    npc.ai[1] = 0;
                    npc.velocity.X *= 0.93f;
                }
                DeathAnimVelocityX = npc.velocity.X;
                if (npc.alpha > 0)
                {
                    npc.alpha -= 15;
                }
                if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
                if (AIPhase == 0)
                {
                    PhaseTimeLeft--;
                    WarningParticlesCounter--;
                    if (PhaseTimeLeft <= 31 && WarningParticlesCounter <= 0)
                    {
                        DustExplosion(npc.Center, 0, 50, 20, 60, DustScale: 2f, NoGravity: true);
                        WarningParticlesCounter = 10;
                    }
                    if (PhaseTimeLeft <= 0)
                    {
                        if (NextAttacks.Count <= 0)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                int choice = Main.rand.Next(1, 5);
                                while (NextAttacks.Contains(choice))
                                {
                                    choice = Main.rand.Next(1, 5);
                                }
                                NextAttacks.Add(choice);
                            }
                        }
                        AIPhase = NextAttacks[0];
                        NextAttacks.RemoveAt(0);
                        if (AIPhase == 1)
                        {
                            BurstWait = 32 + (npc.life / 700);
                            if (!Main.expertMode)
                            {
                                BurstWait = 30 + (npc.life / 700);
                            }
                            SetLine(BurstWait);
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item103, npc.Center);
                            ProjectilesLeft = Main.rand.Next(15, 30);
                            ProjectileWait = 2;
                        }
                        if (AIPhase == 2)
                        {
                            ProjectileWait = 5;
                            ProjectilesLeft = 24;
                            NoGravNeedles = 10;
                            NeedleSpeed = 1;
                            predictMultiplier = 0f;
                        }
                        if (AIPhase == 3)
                        {
                            ProjectileWait = 30;
                            ProjectilesLeft = 3;
                        }
                        if (AIPhase == 4)
                        {
                            ProjectileWait = 5;
                            ProjectilesLeft = 25;
                            NeedleSpeed = 1;
                        }
                        LastAI = AIPhase;
                        AttacksUntilFlight--;
                    }
                    else
                    {
                        if (AttacksUntilFlight <= 0)
                        {
                            Flying = true;
                            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + npc.width / 4, mod.NPCType("Dunestock_Lower"));
                            AttacksUntilLand = 5;
                            AIPhase = 5;
                            PhaseTimeLeft = 240;
                            ProjectileWait = 30;
                        }
                    }
                }
                if (AIPhase == 1) //Attack where it aims where the player will be before firing sand bolts.
                {
                    if (BurstWait <= 0)
                    {
                        StopLine();
                        ProjectileWait--;
                        if (ProjectileWait <= 0)
                        {
                            ProjectilesLeft--;
                            if (ProjectilesLeft <= 0)
                            {
                                AIPhase = 0;
                                PhaseTimeLeft = phaseTimeLeftMax;
                            }

                            ProjectileWait = 2;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                            float ProjSpeed = 3f;

                            int damage = 15;
                            int type = mod.ProjectileType("SandBolt");
                            float rotation = NextAttackAngle;
                            Vector2 rotationVector = rotation.ToRotationVector2() * ProjSpeed;
                            rotationVector = rotationVector.RotatedByRandom(MathHelper.ToRadians(10));
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, rotationVector.X, rotationVector.Y, type, damage, 0f, 0);
                        }
                    }
                    else
                    {
                        BurstWait--;
                    }
                }
                if (AIPhase == 2) //Needle barrage.
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 8;
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                        NeedleSpeed++;
                        float realProjSpeed = NeedleSpeed;
                        NoGravNeedles--;
                        int damage = 20;
                        int type = ModContent.ProjectileType<TumblerNeedle>();
                        float rotation = npc.DirectionTo(Main.player[npc.target].Center).ToRotation();

                        if (NoGravNeedles <= 0)
                        {
                            rotation = npc.DirectionTo(Main.player[npc.target].Center + (npc.Distance(Main.player[npc.target].Center) / realProjSpeed) * Main.player[npc.target].velocity * predictMultiplier).ToRotation();
                        }
                        else
                        {
                            realProjSpeed /= 2;
                        }
                        Vector2 speed = (rotation.ToRotationVector2() * realProjSpeed).RotatedByRandom(MathHelper.ToRadians(10));

                        if (NoGravNeedles <= 0)
                        {
                            NoGravNeedles = 3;
                            predictMultiplier += 1f / 3f;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 240);
                        }
                        else
                        {
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 1, 180);
                        }
                    }
                }
                if (AIPhase == 3) //Air blast but now
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, npc.Center);
                        ProjectilesLeft--;
                        ProjectileWait = 30;
                        float ProjSpeed = 5;
                        if (ProjectilesLeft == 1)
                        {
                            ProjSpeed = 10;
                        }
                        if (ProjectilesLeft == 0)
                        {
                            ProjSpeed = 15;
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                        Projectile.NewProjectile(npc.Center, rotation * ProjSpeed, ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(npc.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(-50)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(npc.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(50)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(npc.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(-25)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                        Projectile.NewProjectile(npc.Center, (rotation * ProjSpeed).RotatedBy(MathHelper.ToRadians(25)), ModContent.ProjectileType<WindBlast>(), 60 / 2, 0);
                    }
                }
                if (AIPhase == 4) //Blood spew
                {
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 5;
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {
                            AIPhase = 0;
                            PhaseTimeLeft = phaseTimeLeftMax;
                        }
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                        NeedleSpeed++;
                        int damage = 20;
                        int type = ModContent.ProjectileType<ClotBlood>();
                        Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                        Vector2 SpeedVector = (rotation * NeedleSpeed).RotatedByRandom(MathHelper.ToRadians(15));
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                    }
                }
                if (AIPhase == 5) //Fly towards the player
                {
                    float maxSpeed = 12;
                    if (npc.Center.X > Main.player[npc.target].Center.X && npc.velocity.X > -maxSpeed)
                    {
                        npc.velocity.X -= 0.15f + (npc.Center.X - Main.player[npc.target].Center.X) / 3500; //Moves faster the farther the player is from the boss.
                        if (npc.velocity.X > 10f)
                        {
                            npc.velocity.X -= 0.9f;
                        }
                    }
                    if (npc.Center.X < Main.player[npc.target].Center.X && npc.velocity.X < maxSpeed)
                    {
                        npc.velocity.X += 0.15f + (Main.player[npc.target].Center.X - npc.Center.X) / 3500;
                        if (npc.velocity.X < -10f)
                        {
                            npc.velocity.X += 0.9f;
                        }
                    }
                    if (npc.Center.Y > Main.player[npc.target].Center.Y)
                    {
                        npc.velocity.Y -= 0.10f + (npc.Center.Y - Main.player[npc.target].Center.Y) / 3000;
                        if (npc.velocity.Y > 7.5f)
                        {
                            npc.velocity.Y -= 0.6f;
                        }
                    }
                    if (npc.Center.Y < Main.player[npc.target].Center.Y)
                    {
                        npc.velocity.Y += 0.10f + (Main.player[npc.target].Center.Y - npc.Center.Y) / 3000;
                        if (npc.velocity.Y < -7.5f)
                        {
                            npc.velocity.Y += 0.6f;
                        }
                    }
                    npc.velocity.X = npc.velocity.X;
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    npc.velocity.Y *= 0.98f;
                    npc.velocity.X *= 0.995f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(20, 41);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                        float ProjSpeed = Main.rand.Next(15, 20);
                        int damage = 20;
                        int type = mod.ProjectileType("TumblerNeedle");
                        Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                        Vector2 speed = rotation * ProjSpeed;
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 200);
                    }
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 6;
                        PhaseTimeLeft = 50;
                        Charging = false;
                        ProjectileWait = 25;
                        if (npc.Center.X > Main.player[npc.target].Center.X)
                        {
                            Direction = -1;
                            MovementTarget = new Vector2(300, 0);
                        }
                        else
                        {
                            Direction = 1;
                            MovementTarget = new Vector2(-300, 0);
                        }
                    }
                }
                if (AIPhase == 6) //Horizontal charge
                {
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    if (Charging)
                    {
                        npc.velocity.X *= 0.95f;
                        npc.velocity.Y = 0;
                        npc.velocity.X += Direction;
                        ProjectileWait--;
                        if (npc.velocity.X > 40)
                        {
                            npc.velocity.X = 40;
                        }
                        if (npc.velocity.X < -40)
                        {
                            npc.velocity.X = -40;
                        }
                        if (ProjectileWait <= 0)
                        {
                            ProjectileWait = 25;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, npc.Center);
                            float ProjSpeed = 12;
                            int damage = 20;
                            int type = mod.ProjectileType("DuneClaw");
                            Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                            Vector2 SpeedVector = rotation * ProjSpeed;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                        }
                        PhaseTimeLeft--;
                        if (PhaseTimeLeft <= 0)
                        {
                            AIPhase = 7;
                            Charging = false;
                            ProjectileWait = 30;
                            ProjectilesLeft = Main.rand.Next(18, 32);
                            BarageDirection = npc.DirectionTo(Main.player[npc.target].Center).ToRotation();
                        }
                    }
                    else
                    {
                        npc.TargetClosest(true);
                        Vector2 targetPosition = Main.player[npc.target].Center + MovementTarget;
                        float speed = 4f;
                        Vector2 move = targetPosition - npc.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        npc.velocity.Y += move.Y;
                        npc.velocity.X += move.X;
                        npc.velocity.X *= 0.8f;
                        npc.velocity.Y *= 0.8f;
                        if (npc.Distance(targetPosition) <= 50)
                        {
                            Charging = true;
                        }
                    }
                    npc.velocity.X = npc.velocity.X;
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                }
                if (AIPhase == 7) //Airborn needle barrage
                {
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    npc.velocity.X = npc.velocity.X;
                    npc.velocity.X *= 0.93f;
                    npc.velocity.Y *= 0.93f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(1, 4);
                        ProjectilesLeft--;
                        if (ProjectilesLeft <= 0)
                        {

                            MovementTarget = Main.player[npc.target].Center;
                            while (!WorldUtils.Find(MovementTarget.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                                {
        new Conditions.IsSolid()
                                }), out _))
                            {
                                MovementTarget.Y++;
                            }
                            MovementTarget.Y -= npc.height;
                            AIPhase = 8;
                        }
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                        float ProjSpeed = Main.rand.Next(20, 29);
                        int damage = 20;
                        int type = mod.ProjectileType("TumblerNeedle");
                        float rotation = BarageDirection;
                        Vector2 speed = BarageDirection.ToRotationVector2() * ProjSpeed;
                        NoGravNeedles = Main.rand.Next(4, 7);
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 60);
                    }
                }
                if (AIPhase == 8) //Fly to the land under the player
                {
                    MovementTarget = Main.npc[NPC.FindFirstNPC(mod.NPCType("Dunestock_Lower"))].position;
                    MovementTarget.Y -= npc.height / 2;
                    Vector2 move;
                    npc.TargetClosest(true);
                    float speed = 14f;
                    move = MovementTarget - npc.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    move *= speed / magnitude;
                    npc.velocity = move;
                    float Xdif = MovementTarget.X - (npc.position.X + npc.width / 2);
                    float Ydif = MovementTarget.Y - (npc.position.Y + npc.height / 2);
                    float distanceToTarget = (float)Math.Sqrt((Xdif * Xdif) + (Ydif * Ydif));
                    if (Math.Abs(distanceToTarget) <= speed)
                    {
                        AIPhase = 0;
                        PhaseTimeLeft = phaseTimeLeftMax;
                        Flying = false;
                        AttacksUntilFlight = 5;
                    }
                }
                if (npc.life <= npc.lifeMax * 0.40f && AIPhase <= 8)
                {
                    TerrorbornMod.ScreenShake(75f);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                    DustExplosion(npc.Center, 0, 50, 40, 60, DustScale: 2f, NoGravity: true);
                    Flying = true;
                    AIPhase = 9;
                    PhaseTimeLeft = 240;
                    ProjectileWait = 30;
                    StopLine();
                    Main.NewText("The wind accelerates around you...", 238, 45, 45);
                    music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/DreadInTheDustwind");
                }
                if (AIPhase == 9) //Phase two flying
                {
                    float maxSpeed = 15;
                    if (npc.Center.X > Main.player[npc.target].Center.X && npc.velocity.X > -maxSpeed)
                    {
                        npc.velocity.X -= 0.16f + (npc.Center.X - Main.player[npc.target].Center.X) / 3500; //Moves faster the farther the player is from the boss.
                        if (npc.velocity.X > 10f)
                        {
                            npc.velocity.X -= 0.9f;
                        }
                    }
                    if (npc.Center.X < Main.player[npc.target].Center.X && npc.velocity.X < maxSpeed)
                    {
                        npc.velocity.X += 0.16f + (Main.player[npc.target].Center.X - npc.Center.X) / 3500;
                        if (npc.velocity.X < -10f)
                        {
                            npc.velocity.X += 0.9f;
                        }
                    }
                    if (npc.Center.Y > Main.player[npc.target].Center.Y)
                    {
                        npc.velocity.Y -= 0.11f + (npc.Center.Y - Main.player[npc.target].Center.Y) / 3000;
                        if (npc.velocity.Y > 7.5f)
                        {
                            npc.velocity.Y -= 0.6f;
                        }
                    }
                    if (npc.Center.Y < Main.player[npc.target].Center.Y)
                    {
                        npc.velocity.Y += 0.11f + (Main.player[npc.target].Center.Y - npc.Center.Y) / 3000;
                        if (npc.velocity.Y < -7.5f)
                        {
                            npc.velocity.Y += 0.6f;
                        }
                    }
                    npc.velocity.X = npc.velocity.X;
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    npc.velocity.Y *= 0.98f;
                    npc.velocity.X *= 0.995f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = 45;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                        float ProjSpeed = 15;
                        int damage = 20;
                        int type = mod.ProjectileType("TumblerNeedle");
                        Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center + (npc.Distance(Main.player[npc.target].Center) / ProjSpeed) * Main.player[npc.target].velocity);
                        Vector2 speed = (rotation * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(10));
                        Projectile.NewProjectile(npc.Center.X, npc.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 200);
                    }
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 10;
                        PhaseTimeLeft = 45;
                        Charging = false;
                        ProjectileWait = 15;
                        if (npc.Center.X > Main.player[npc.target].Center.X)
                        {
                            Direction = -1;
                            MovementTarget = new Vector2(400, 0);
                        }
                        else
                        {
                            Direction = 1;
                            MovementTarget = new Vector2(-400, 0);
                        }
                    }
                }
                if (AIPhase == 10) //Phase two charge
                {
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    if (Charging)
                    {
                        npc.velocity.X *= 0.95f;
                        npc.velocity.Y = 0;
                        npc.velocity.X += Direction * 1.5f;
                        ProjectileWait--;
                        if (npc.velocity.X > 55)
                        {
                            npc.velocity.X = 55;
                        }
                        if (npc.velocity.X < -55)
                        {
                            npc.velocity.X = -55;
                        }
                        if (ProjectileWait <= 0)
                        {
                            ProjectileWait = 15;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, npc.Center);
                            float ProjSpeed = 14;
                            int damage = 20;
                            int type = mod.ProjectileType("DuneClaw");
                            Vector2 rotation = npc.DirectionTo(Main.player[npc.target].Center);
                            Vector2 SpeedVector = rotation * ProjSpeed;
                            Projectile.NewProjectile(npc.Center.X, npc.Center.Y, SpeedVector.X, SpeedVector.Y, type, damage, 0f, 0);
                        }
                        PhaseTimeLeft--;
                        if (PhaseTimeLeft <= 0)
                        {
                            AIPhase = 11;
                            PhaseTimeLeft = 180;
                            Charging = false;
                        }
                    }
                    else
                    {
                        npc.TargetClosest(true);
                        Vector2 targetPosition = Main.player[npc.target].Center + MovementTarget;
                        float speed = 4f;
                        Vector2 move = targetPosition - npc.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        npc.velocity.Y += move.Y;
                        npc.velocity.X += move.X;
                        npc.velocity.X *= 0.8f;
                        npc.velocity.Y *= 0.8f;
                        if (npc.Distance(targetPosition) <= 50)
                        {
                            Charging = true;
                        }
                    }
                    npc.velocity.X = npc.velocity.X;
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                }
                if (AIPhase == 11) //Clot bomb rain
                {
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    MovementTarget = new Vector2(0, -300);
                    npc.TargetClosest(true);
                    Vector2 targetPosition = Main.player[npc.target].Center + MovementTarget;
                    float speed = 2.5f;
                    Vector2 move = targetPosition - npc.Center;
                    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                    move *= speed / magnitude;
                    if (npc.Distance(Main.player[npc.target].Center + MovementTarget) >= 45)
                    {
                        npc.velocity.Y += move.Y;
                        npc.velocity.X += move.X;
                    }
                    npc.velocity.X *= 0.9f;
                    npc.velocity.Y *= 0.9f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0)
                    {
                        ProjectileWait = Main.rand.Next(7, 27);
                        Projectile.NewProjectile(new Vector2(npc.position.X + Main.rand.Next(npc.width + 1), npc.position.Y + Main.rand.Next(npc.height + 1)), new Vector2(0, 0), ModContent.ProjectileType<ClotBomb>(), 30, 0);
                    }
                    npc.velocity.X = npc.velocity.X;
                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        PhaseTimeLeft = 420;
                        AIPhase = 12;
                        ProjectileWait = 60;
                        ProjectilesLeft = 4;
                        MovementTarget = Main.player[npc.target].Center;
                    }
                }
                if (AIPhase == 12) //Tornado
                {
                    npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
                    npc.velocity.X *= 0.9f;
                    npc.velocity.Y *= 0.9f;
                    ProjectileWait--;
                    if (ProjectileWait <= 0 && ProjectilesLeft > 0)
                    {
                        ProjectilesLeft--;
                        ProjectileWait = 15;
                        if (ProjectilesLeft == 0)
                        {
                            ProjectileWait = 45;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                            DustExplosion(npc.Center, 0, 50, 40, 60, DustScale: 2f, NoGravity: true);
                            while (!WorldUtils.Find(MovementTarget.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                                {
        new Conditions.IsSolid()
                                }), out _))
                            {
                                MovementTarget.Y++;
                            }
                            Projectile.NewProjectile(MovementTarget, Vector2.Zero, mod.ProjectileType("SandnadoSpawn"), 30, 0);
                        }
                        else
                        {
                            DustExplosion(npc.Center, 0, 50, 20, 60, DustScale: 2f, NoGravity: true);
                        }
                    }
                    if (ProjectileWait <= 0 && ProjectilesLeft <= 0)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, npc.Center);
                        ProjectileWait = 60;
                        float speed = 20;
                        Vector2 velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                        Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(npc.Center, velocity.RotatedBy(MathHelper.ToRadians(-60)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(npc.Center, velocity.RotatedBy(MathHelper.ToRadians(60)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(npc.Center, velocity.RotatedBy(MathHelper.ToRadians(-30)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                        Projectile.NewProjectile(npc.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), ModContent.ProjectileType<WindBlast>(), 60 / 4, 0);
                    }
                    npc.velocity.X = npc.velocity.X;

                    PhaseTimeLeft--;
                    if (PhaseTimeLeft <= 0)
                    {
                        AIPhase = 9;
                        PhaseTimeLeft = 240;
                        ProjectileWait = 30;
                    }
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
    }
    class Dunestock_Lower : ModNPC
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/Dunestock_Lower";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dunestock");
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.defense = 999;
            npc.lifeMax = 7000;
            npc.damage = 0;
            npc.width = 60;
            npc.height = 44;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit6;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.value = 110000f;
            npc.knockBackResist = 0.00f;
            npc.hide = false;
            npc.chaseable = false;
        }
        public override bool PreNPCLoot()
        {
            return false;
        }
        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }
        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage = 0;
            crit = false;
        }
        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            damage = 0;
            crit = false;
        }
        int frame = 0;
        int FrameWait = 0;
        private void WalkingAnimation(int FrameSpeed)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = FrameSpeed;
                frame++;
            }
            if (frame > 5 || frame < 1)
            {
                frame = 1;
            }
        }
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }
        bool Start = true;
        int AIPhase = 0;
        int PhaseCounter = 60;
        int JumpDir;
        int NeedleCounter = 60;
        public override void AI()
        {
            if (NPC.AnyNPCs(mod.NPCType("Dunestock")))
            {
                int MainBoss = NPC.FindFirstNPC(mod.NPCType("Dunestock"));
                if (Main.npc[MainBoss].life <= Main.npc[MainBoss].lifeMax * 0.40f)
                {
                    npc.active = false;
                }
                if (Main.npc[MainBoss].ai[1] == 0 || Main.player[npc.target].dead)
                {
                    npc.active = false;
                }
                if (Start)
                {
                    Start = false;
                }
                npc.life = Main.npc[MainBoss].life;
                if (AIPhase == 0) //Standing still
                {
                    npc.TargetClosest(true);
                    frame = 0;
                    PhaseCounter--;
                    if (PhaseCounter <= 0)
                    {
                        PhaseCounter = Main.rand.Next(120, 300);
                        AIPhase = 1;
                    }
                }
                if (AIPhase == 1)
                {
                    if (Main.player[npc.target].Center.Y > npc.position.Y + 30 && npc.velocity.Y == 0)
                    {
                        npc.position.Y++;
                    }
                    npc.TargetClosest(true);
                    float WalkSpeed = 0.35f;
                    if (npc.life <= npc.lifeMax * 0.40f)
                    {
                        WalkSpeed = 0.45f;
                    }
                    WalkingAnimation(4);
                    if (npc.Center.X <= Main.player[npc.target].Center.X)
                    {
                        npc.velocity.X += WalkSpeed;
                    }
                    else
                    {
                        npc.velocity.X -= WalkSpeed;
                    }
                    PhaseCounter--;
                    if (PhaseCounter <= 0 && npc.velocity.Y == 0)
                    {
                        if (Main.player[npc.target].Center.X >= npc.Center.X)
                        {
                            JumpDir = 1;
                        }
                        else
                        {
                            JumpDir = -1;
                        }
                        npc.velocity.X = 10 * JumpDir;
                        npc.velocity.Y = -14;
                        AIPhase = 2;
                    }
                }
                if (AIPhase == 2)
                {
                    frame = 6;
                    npc.TargetClosest(false);
                    float speed = 0.5f;
                    if (Main.player[npc.target].Center.X >= npc.Center.X)
                    {
                        npc.velocity.X += speed;
                        if (JumpDir == -1 && Main.player[npc.target].Center.Y > npc.position.Y + npc.height && npc.life <= npc.lifeMax * 0.75f)
                        {
                            npc.velocity.Y += 1;
                        }
                    }
                    else
                    {
                        npc.velocity.X -= speed;
                        if (JumpDir == 1 && Main.player[npc.target].Center.Y > npc.position.Y + npc.height && npc.life <= npc.lifeMax * 0.75f)
                        {
                            npc.velocity.Y += 1;
                        }
                    }
                    if (npc.velocity.Y == 0)
                    {
                        AIPhase = 0;
                        PhaseCounter = Main.rand.Next(45, 75);
                    }
                }
            }
            else
            {
                npc.active = false;
            }
        }
    }
    class SandBolt : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            projectile.width = 5;
            projectile.height = 5;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.hide = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 1500;
        }
        int DustWait = 5;
        public override void AI()
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector2 projectilePosition = projectile.position;
                    projectilePosition -= projectile.velocity * ((float)i * 0.25f);
                    projectile.alpha = 255;
                    DustWait--;
                    if (DustWait <= 0)
                    {
                        int dust = Dust.NewDust(projectilePosition, projectile.width, projectile.height, 0, 0);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = projectilePosition;
                        DustWait = 5;
                        Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                        Main.dust[dust].velocity *= 0.2f;
                    }
                }
            }
        }
    }
    class TumblerNeedle : ModProjectile
    {
        bool Stick = false;
        int trueTimeleft = 235;
        float telegraphAlpha = 0f;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (projectile.ai[0] != 1 && !Stick && projectile.ai[1] > 0)
            {
                Color color = Color.LightYellow;
                color.A = (int)(255 * 0.75f);
                Utils.DrawLine(spriteBatch, projectile.Center, projectile.Center + projectile.velocity.ToRotation().ToRotationVector2() * 2200f, color * telegraphAlpha, Color.Transparent, 3);
            }
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 12000;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            if (NPC.AnyNPCs(ModContent.NPCType<Dunestock>()))
            {
                fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(mod.NPCType("Dunestock"))].target].position.Y - 30 > projectile.position.Y || projectile.ai[0] == 0;
            }
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Stick = true;
            return false;
        }
        public override void AI()
        {
            if (telegraphAlpha < 1)
            {
                telegraphAlpha += 0.02f;
            }
            if (projectile.timeLeft == 12000)
            {
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
            }
            if (projectile.ai[1] <= 0)
            {
                projectile.alpha += 5;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            if (Stick)
            {
                projectile.velocity *= 0;
                projectile.ai[1]--;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
                if (projectile.ai[0] == 1 && !Stick)
                {
                    projectile.velocity.Y += 0.3f;
                }
            }
        }
    }
    class Tumbleweed : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 2;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        int trueTimeleft = 180;
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 12000;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(mod.NPCType("Dunestock"))].target].position.Y - 30 > projectile.position.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Run, projectile.Center);
            return false;
        }
        public override void AI()
        {
            projectile.velocity.Y += 0.25f;
            trueTimeleft--;
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
            if (trueTimeleft <= 0)
            {
                projectile.alpha += 5;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
    }
    class DuneClaw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        int CollideCounter = 0;
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 150;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            //Terraria.Audio.SoundEngine.PlaySound(SoundID.Run, projectile.Center);
            CollideCounter += 1;
            if (CollideCounter >= 5)
            {
                projectile.timeLeft = 0;
            }
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft <= 0)
            {
                Player targetPlayer = Main.player[Player.FindClosest(projectile.Center, projectile.width, projectile.height)];
                if (Main.expertMode)
                {
                    for (int i = 0; i < Main.rand.Next(4,6); i++)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, projectile.Center);
                        float ProjSpeed = 15f;
                        int damage = 35 / 2; // /2 undoes the expert increase
                        int type = mod.ProjectileType("TumblerNeedle");
                        float rotation = projectile.DirectionTo(targetPlayer.Center).ToRotation();
                        Vector2 speed = (rotation.ToRotationVector2() * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(6));
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 180);
                    }
                    projectile.active = false;
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, projectile.Center);
                        float ProjSpeed = 10f;
                        int damage = 20;
                        int type = mod.ProjectileType("TumblerNeedle");
                        float rotation = projectile.DirectionTo(targetPlayer.Center).ToRotation();
                        Vector2 speed = (rotation.ToRotationVector2() * ProjSpeed).RotatedByRandom(MathHelper.ToRadians(3));
                        Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, speed.X, speed.Y, type, damage, 0f, 0, 0, 120);
                    }
                    projectile.active = false;
                }
                
            }
        }
    }
 
    class ClotBomb : ModProjectile
    {
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.75f);
        }
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }
        int RotationDirection = 0;
        public override void AI()
        {
            if (projectile.timeLeft == 599)
            {
                if (Main.rand.Next(2) == 0)
                {
                    RotationDirection = 1;
                }
                else
                {
                    RotationDirection = -1;
                }
            }
            projectile.rotation += MathHelper.ToRadians(5 * RotationDirection);
            projectile.velocity.Y += 0.5f;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = Main.player[Main.npc[NPC.FindFirstNPC(mod.NPCType("Dunestock"))].target].position.Y - 30 > projectile.position.Y;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(projectile.Center, ProjectileSpeed, mod.ProjectileType("ClotBlood"), projectile.damage / 2, 0);
            }
        }
    }
    class ClotBlood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 30;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            int targetPlayer = Player.FindClosest(projectile.position, projectile.width, projectile.height);
            //HOME IN
            float speed = .35f;
            Vector2 move = Main.player[targetPlayer].Center - projectile.Center;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            move *= speed / magnitude;
            projectile.velocity += move;
        }
    }
    class SandnadoSpawn : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 420;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        int Counter = 0;
        int NumPlaced = 0;
        public override void AI()
        {
            Counter--;
            if (Counter <= 0)
            {
                Counter = 5;
                if (NumPlaced <= 8)
                {
                    int num54 = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.position.Y - 7 - (38 * NumPlaced/* * (1 + (NumPlaced * .18f))*/)), new Vector2(0, 0), mod.ProjectileType("Sandnado"), projectile.damage, projectile.knockBack, projectile.owner, NumPlaced);
                }
                NumPlaced++;
            }
            int SuckSpeed = 2;
            Player targetPlayer = Main.player[Player.FindClosest(projectile.Center, 0, 0)];
            if (targetPlayer.Center.X > projectile.Center.X)
            {
                targetPlayer.position.X -= SuckSpeed;
                int newDust = Dust.NewDust(targetPlayer.position, targetPlayer.width, targetPlayer.height, DustID.GoldFlame, -35, 0, Scale: 1.75f);
                Main.dust[newDust].noGravity = true;
            }
            else
            {
                targetPlayer.position.X += SuckSpeed;
                int newDust = Dust.NewDust(targetPlayer.position, targetPlayer.width, targetPlayer.height, DustID.GoldFlame, 35, 0, Scale: 1.75f);
                Main.dust[newDust].noGravity = true;
            }
        }

    }
    class Sandnado : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 10;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 1;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        public override void SetDefaults()
        {
            projectile.width = 148;
            projectile.height = 26;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.hostile = true;
            projectile.damage = 0;
            projectile.timeLeft = 10000;
            projectile.alpha = 255;
        }
        int TrueTimeLeft = 300;
        float TornadoXVelocity = 10;
        int TornadoDirection = 1;
        public override void AI()
        {
            if ((TornadoXVelocity >= 10 && TornadoDirection == 1) || (TornadoXVelocity <= -10 && TornadoDirection == -1))
            {
                TornadoDirection *= -1;
                TornadoXVelocity += TornadoDirection;
            }
            else
            {
                TornadoXVelocity += TornadoDirection;
            }
            projectile.velocity.X = TornadoXVelocity;

            FindFrame(projectile.height);
            TrueTimeLeft--;
            if (TrueTimeLeft <= 0)
            {
                projectile.alpha += 5;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            else
            {
                projectile.alpha = 140 - (int)(projectile.ai[0] * 15);
                projectile.scale = 1.5f + (projectile.ai[0] * .18f);
            }
        }
    }
    class WindBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 42;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
}
