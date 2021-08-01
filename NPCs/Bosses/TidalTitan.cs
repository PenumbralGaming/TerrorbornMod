using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;

namespace TerrorbornMod.NPCs.Bosses
{
    class TidalTitanIdle : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 17;
            DisplayName.SetDefault("Mysterious Crab");
        }
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TidalTitan";
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 128;
            npc.height = 78;
            npc.damage = 0;
            npc.HitSound = SoundID.NPCHit38;
            npc.defense = 9;
            npc.DeathSound = SoundID.NPCDeath14;
            Main.raining = true;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 500;
            npc.rarity = 10;
            npc.knockBackResist = 0;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 300;
            
        }
        public override void AI()
        {
            if (npc.wet)
            {
                if (npc.velocity.Y > -10)
                {
                    npc.velocity.Y -= 0.1f;
                }
                npc.noGravity = true;
            }
            else
            {
                npc.noGravity = false;
            }
            if (npc.velocity.Y == 0)
            {
                npc.frame.Y = 0;
            }
            else
            {
                npc.frame.Y = 7 * 78;
            }
        }
        public override bool PreNPCLoot()
        {
            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y + 37, mod.NPCType("TidalTitan"));
            Terraria.Audio.SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
            TerrorbornMod.ScreenShake(40f);
            return false;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime && !NPC.AnyNPCs(mod.NPCType("TidalTitan")) && !NPC.AnyNPCs(mod.NPCType("TidalTitanIdle")))
            {
                if (NPC.downedBoss2 && !TerrorbornWorld.downedTidalTitan)
                {
                    return SpawnCondition.Ocean.Chance * 0.085f;
                }
                else
                {
                    return SpawnCondition.Ocean.Chance * 0.05f;
                }
            }
            else
            {
                return 0f;
            }
        }
    }
    [AutoloadBossHead]
    class TidalTitan : ModNPC
    {
        int bubbleCooldown = 10;
        private void SpawnBubble(float CrabChance, float Speed)
        {
            bubbleCooldown--;
            if (bubbleCooldown <= 0)
            {
                bubbleCooldown = 5;
                int captive = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TidalCrabBubble"));
            }
            else
            {
                int captive = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TidalBubble"));
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item85, npc.Center);
            //float rotation = (float)Math.Atan2(vector8.Y - (Main.player[npc.target].position.Y + (Main.player[npc.target].height * 0.5f)), vector8.X - (Main.player[npc.target].position.X + (Main.player[npc.target].width * 0.5f)));
            //Main.npc[captive].velocity = new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (AIPhase == 3 && PhaseTimeLeft > 0)
            {
                return false;
            }
            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 17;
        }

        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.boss = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/CrustaceanCrackdown");
            npc.width = 128;
            npc.height = 78;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit38;
            npc.defense = 9;
            npc.DeathSound = SoundID.NPCDeath14;
            Main.raining = true;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 4500;
            npc.knockBackResist = 0;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void NPCLoot()
        {
            if (!TerrorbornWorld.downedTidalTitan)
            {
                TerrorbornWorld.downedTidalTitan = true;
                Main.NewText("Azurite Ore forms in caverns below the ocean", 37, 173, 255);
                Main.NewText("Lunar energy blesses the rain", 75, 253, 248);

                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05) / 12; k++)
                {
                    int x = WorldGen.genRand.Next(20, 380);
                    int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.3f), Main.maxTilesY);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(3, 6), mod.TileType("Azurite"), false, 0f, 0f, false, true);
                }

                for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 6E-05) / 12; k++)
                {
                    int x = WorldGen.genRand.Next(Main.maxTilesX - 380, Main.maxTilesX - 20);
                    int y = WorldGen.genRand.Next((int)(Main.maxTilesY * 0.3f), Main.maxTilesY);
                    WorldGen.TileRunner(x, y, (double)WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(3, 6), mod.TileType("Azurite"), false, 0f, 0f, false, true);
                }
            }
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AzuriteOre"), Main.rand.Next(15, 26));
            if (Main.expertMode)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TT_TreasureBag"));
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AzuriteOre"), Main.rand.Next(15, 26));
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrackedShell"), Main.rand.Next(7, 11));
                int choice = Main.rand.Next(4);
                if (choice == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BubbleBow"));
                }
                else if (choice == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TidalClaw"), Stack: 750);
                }
                else if (choice == 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SightForSoreEyes"));
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.TidalTitanMask>());
                }
            }
        }
        int frame = 0;
        //Stand = 0;
        //Walking = 1 - 6
        //Slamdown = 6-11
        //Jump = 7
        //Mess with crabbo you get stabbo = 12-16
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }
        int FrameWait = 6;
        private void WalkingAnimation(int FrameSpeed)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = FrameSpeed;
                frame++;
            }
            if (frame > 6)
            {
                frame = 0;
            }
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax -= npc.lifeMax / 3;
            npc.defense = 19;
        }
        int FollowingFlameThingWait = 0;
        void SpawnFlame()
        {
            Vector2 position = Main.player[npc.target].Center;
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                position.Y++;
            }

            //for (position = Main.player[npc.target].Center; Main.tileSolid[Main.tile[(int)position.X / 16, (int)position.Y / 16].type]; position.Y += 0.25f) ;
            Projectile.NewProjectile(position, new Vector2(0, 0), mod.ProjectileType("TideFireWait"), 30, 0, ai0: 100);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item117, position);
        }

        void DecideNextAIPhase()
        {
            PlannedAIPhase = Main.rand.Next(1, 6);
            while (PlannedAIPhase == AIPhase || (PlannedAIPhase == 3 && JumpCooldown > 0) || (PlannedAIPhase == 5 && bubbleAttackCooldown > 0))
            {
                PlannedAIPhase = Main.rand.Next(1, 6);
            }
        }

        int AIPhase = 0;
        int PlannedAIPhase = 1;
        int waitingTime = 50;
        int PhaseTimeLeft = 120;
        int BubbleWait = 60;
        int BubblesToFire = 4;
        int JumpCooldown = 0;
        int requiredJumpWait = 600;
        int bubbleAttackCooldown = 0;
        Vector2 jumpTarget;

        public override void AI()
        {
            if (bubbleAttackCooldown > 0)
            {
                bubbleAttackCooldown--;
            }
            if (requiredJumpWait > 900 - ((npc.lifeMax - npc.life) / npc.lifeMax) * 450)
            {
                requiredJumpWait = 900 - ((npc.lifeMax - npc.life) / npc.lifeMax) * 450;
            }
            if (JumpCooldown > 0)
            {
                JumpCooldown--;
                requiredJumpWait = 900 - ((npc.lifeMax - npc.life) / npc.lifeMax) * 450;
            }
            else if (requiredJumpWait > 0)
            {
                requiredJumpWait--;
            }
            if (npc.life <= npc.lifeMax * .4f)
            {
                FollowingFlameThingWait--;
                if (FollowingFlameThingWait <= 0)
                {
                    FollowingFlameThingWait = 720;
                    SpawnFlame();
                }
            }
            if (npc.wet || !Main.player[npc.target].active || Main.player[npc.target].dead || !Main.player[npc.target].ZoneBeach)
            {
                npc.alpha += 2;
                if (npc.alpha >= 255)
                {
                    npc.active = false;
                    if (npc.wet)
                    {
                        Main.NewText("The mysterious crab plunges into the depths. Perhaps you should try and keep it from doing that somehow....", new Color(0, 0, 225));
                    }
                }
            }
            else
            {
                if (npc.alpha > 0)
                {
                    npc.alpha -= 5;
                }
                else if (npc.alpha < 0)
                {
                    npc.alpha = 0;
                }
            }
            Player target = Main.player[npc.target];
            Vector2 targetPosition = target.Center;
            if (AIPhase == 0) //Standing still. Will keep waiting until it is told to stahp waiting
            {
                if (targetPosition.Y >= npc.position.Y + 150)
                {
                    npc.velocity.Y += 0.2f;
                    npc.position.Y += 0.1f;
                }
                frame = 0;
                if (!(PlannedAIPhase == 3 && npc.velocity.Y != 0) || !(PlannedAIPhase == 2 && npc.velocity.Y != 0) || !(PlannedAIPhase == 4 && npc.velocity.Y != 0))
                {
                    waitingTime--;
                }
                if (waitingTime <= 0)
                {
                    if (requiredJumpWait <= 0)
                    {
                        PlannedAIPhase = 3;
                    }
                    AIPhase = PlannedAIPhase;
                    if (PlannedAIPhase == 1)
                    {
                        BubbleWait = 120;
                    }
                    if (PlannedAIPhase == 2)
                    {
                        frame = 12;
                    }
                    if (PlannedAIPhase == 3)
                    {
                        frame = 7;
                        PhaseTimeLeft = 90;
                        Terraria.Audio.SoundEngine.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
                        jumpTarget = targetPosition + new Vector2(0, -300);
                    }
                    if (PlannedAIPhase == 4)
                    {
                        frame = 6;
                        PhaseTimeLeft = 3;
                        if (npc.life <= npc.lifeMax * 0.66f)
                        {
                            PhaseTimeLeft = 4;
                        }
                        if (npc.life <= npc.lifeMax * 0.33f)
                        {
                            PhaseTimeLeft = 5;
                        }
                    }
                    if (PlannedAIPhase == 5)
                    {
                        BubbleWait = 10;
                        BubblesToFire = 3;
                        bubbleAttackCooldown = 500;
                    }
                }
            }
            if (AIPhase == 1) //walking
            {
                //BubbleWait--;
                //if (BubbleWait <= 0)
                //{
                //    BubbleWait = 120;
                //    float speed = 7f;
                //    Vector2 velocity = npc.DirectionTo(targetPosition) * speed;
                //    Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.MoonlightOrb>(), 50 / 4, 0);
                //}
                if (targetPosition.Y >= npc.position.Y + 150)
                {
                    npc.velocity.Y += 0.2f;
                    npc.position.Y += 0.1f;
                }
                float WalkSpeed = 0.35f;
                WalkingAnimation(4);
                if (npc.Center.X <= targetPosition.X)
                {
                    npc.velocity.X += WalkSpeed;
                }
                else
                {
                    npc.velocity.X -= WalkSpeed;
                }
                PhaseTimeLeft--;
                if (PhaseTimeLeft <= 0)
                {
                    DecideNextAIPhase();
                    AIPhase = 0;
                    waitingTime = 40;
                    FrameWait = 6;
                }
            }
            if (AIPhase == 2) //Slam Towards player
            {
                FrameWait--;
                if (FrameWait <= 0)
                {
                    frame++;
                    FrameWait = 6;
                    if (frame == 14)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                        if (npc.Center.X <= targetPosition.X)
                        {
                            Projectile.NewProjectile(new Vector2(npc.Center.X, npc.position.Y), new Vector2(12.5f, 0), mod.ProjectileType("SlamOut"), 50 / 4, 15);
                        }
                        else
                        {
                            Projectile.NewProjectile(new Vector2(npc.Center.X, npc.position.Y), new Vector2(-12.5f, 0), mod.ProjectileType("SlamOut"), 50 / 4, 15);
                        }

                    }
                    if (frame >= 16)
                    {
                        DecideNextAIPhase();
                        AIPhase = 0;
                        waitingTime = 40;
                    }
                }
            }
            if (AIPhase == 3) //Jump
            {
                if (PhaseTimeLeft > 0)
                {
                    frame = 7;
                    PhaseTimeLeft--;
                    npc.noTileCollide = true;
                    npc.velocity = npc.DirectionTo(jumpTarget) * npc.Distance(jumpTarget) / 10;

                    if (PhaseTimeLeft <= 0)
                    {
                        npc.velocity.Y += 15;
                    }
                }
                else
                {
                    frame = 7;
                    npc.noTileCollide = targetPosition.Y > npc.position.Y + npc.height;
                    if (npc.velocity.Y == 0)
                    {
                        frame = 0;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                        npc.velocity.X = 0;

                        int geyserCount = 45;
                        float geyserSpace = 200;

                        for (int i = -geyserCount / 2; i < geyserCount / 2; i++)
                        {
                            Vector2 position = new Vector2(npc.Center.X - i * geyserSpace, npc.position.Y);
                            int proj = Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<JumpGeyserTelegraph>(), 17, 0);
                            Main.projectile[proj].ai[0] = 50 - ((npc.lifeMax - npc.life) / npc.lifeMax) * 45;
                        }

                        DecideNextAIPhase();
                        AIPhase = 0;
                        waitingTime = 90;
                        JumpCooldown = 120;
                        PhaseTimeLeft = 240;
                    }
                    frame = 0;
                }
            }
            if (AIPhase == 4) //Predictive Geysers
            {
                FrameWait--;
                if (FrameWait <= 0)
                {
                    frame++;
                    FrameWait = 6;
                    if (npc.life <= npc.lifeMax * 0.66f)
                    {
                        FrameWait = 4;
                    }
                    if (frame == 10)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                        Vector2 position = new Vector2(target.Center.X + target.velocity.X * 60, target.Center.Y);
                        int proj = Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<JumpGeyserTelegraph>(), 17, 0);
                        Main.projectile[proj].ai[0] = 60;
                    }
                    if (frame >= 11)
                    {
                        PhaseTimeLeft--;
                        if (PhaseTimeLeft <= 0)
                        {
                            DecideNextAIPhase();
                            AIPhase = 0;
                            waitingTime = 80;
                        }
                        else
                        {
                            frame = 6;
                        }
                    }
                }
            }
            if (AIPhase == 5) //Bubble Barrage of bubbling fear and ultimate doom
            {
                BubbleWait--;
                if (BubbleWait <= 0)
                {
                    if (Main.expertMode)
                    {
                        SpawnBubble(10, 10);
                    }
                    else
                    {
                        SpawnBubble(10, 10);
                    }
                    BubbleWait = 30;
                    BubblesToFire--;
                    if (BubblesToFire <= 0)
                    {
                        DecideNextAIPhase();
                        AIPhase = 0;
                        waitingTime = 40;
                    }
                }
            }
        }
        class SlamOut : ModProjectile
        {
            public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
            public override void SetDefaults()
            {
                projectile.width = 30;
                projectile.height = 30;
                projectile.hostile = true;
                projectile.friendly = false;
                projectile.hide = true;
                projectile.tileCollide = false;
                projectile.penetrate = 1;
                projectile.timeLeft = 120;
                projectile.alpha = 255;
            }
            float FireWait = 0;
            public override bool CanHitPlayer(Player target)
            {
                return false;
            }
            public override void AI()
            {
                FireWait--;
                if (FireWait <= 0)
                {
                    FireWait = Math.Abs(20 / projectile.velocity.X);
                    //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20, projectile.Center);
                    Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y + 80), new Vector2(0, -10), mod.ProjectileType("TideFire"), projectile.damage, projectile.knockBack);
                }
            }
        }
        class SlamOut2 : ModProjectile
        {
            public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
            public override void SetDefaults()
            {
                projectile.width = 30;
                projectile.height = 30;
                projectile.hostile = true;
                projectile.friendly = false;
                projectile.hide = true;
                projectile.tileCollide = false;
                projectile.penetrate = 1;
                projectile.timeLeft = 420;
                projectile.alpha = 255;
            }
            float FireWait = 0;
            public override bool CanHitPlayer(Player target)
            {
                return false;
            }
            public override void AI()
            {
                FireWait--;
                if (FireWait <= 0)
                {
                    FireWait = Math.Abs(20 / projectile.velocity.X);
                    //Terraria.Audio.SoundEngine.PlaySound(SoundID.Item20, projectile.Center);
                    Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y + 80), new Vector2(0, -15), mod.ProjectileType("TideFire"), projectile.damage, projectile.knockBack);
                }
            }
        }
        class TideFire : ModProjectile
        {
            public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
            public override void SetDefaults()
            {
                projectile.width = 50;
                projectile.height = 50;
                projectile.hostile = true;
                projectile.friendly = false;
                projectile.hide = true;
                projectile.tileCollide = false;
                projectile.penetrate = 1;
                projectile.timeLeft = 30;
                projectile.alpha = 255;
            }
            public override void AI()
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, 0, 0, Scale: 2, newColor: Color.SkyBlue);
                Main.dust[dust].noGravity = true;
            }
        }
        class TidalBubble : ModNPC
        {
            Vector2 trueVelocity = new Vector2(0, 0);
            public override void SetStaticDefaults()
            {
                Main.npcFrameCount[npc.type] = 4;
                DisplayName.SetDefault("Bubble");
            }
            public override bool PreNPCLoot()
            {
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), mod.ProjectileType("TidalBubbleSmall"), npc.damage / 3, 0);
                }
                if (Main.rand.Next(10) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
                }
                return false;
            }
            public override void SetDefaults()
            {
                npc.noGravity = true;
                npc.noTileCollide = true;
                npc.width = 30;
                npc.height = 30;
                npc.damage = 30;
                npc.HitSound = SoundID.Item54;
                npc.defense = 0;
                npc.DeathSound = SoundID.Item54;
                npc.frame.Width = 388;
                npc.frame.Height = 254;
                npc.lifeMax = 20;
                npc.knockBackResist = 0;
                npc.dontTakeDamage = true;
            }
            int frame = 0;
            int frameWait = 10;
            public override void FindFrame(int frameHeight)
            {

                frameWait--;
                if (frameWait <= 0)
                {
                    frameWait = 10;
                    frame++;
                    if (frame >= 4)
                    {
                        frame = 0;
                    }
                }
                npc.frame.Y = frame * 30;
            }
            public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
            {
                npc.lifeMax = 55;
            }
            bool start = true;
            int invincTime = 30;
            public override void AI()
            {
                if (invincTime <= 0)
                {
                    npc.dontTakeDamage = false;
                }
                else
                {
                    invincTime--;
                }
                npc.TargetClosest(true);
                Vector2 targetPosition = Main.player[npc.target].Center;
                if (start)
                {
                    start = false;
                    Vector2 direction = npc.DirectionTo(targetPosition);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(30));
                    float speed = Main.rand.NextFloat(10f, 16f);
                    trueVelocity = speed * direction;
                }
                trueVelocity *= 0.98f;
                npc.velocity = trueVelocity;
            }
        }
        class TidalBubbleSmall : ModProjectile
        {
            public override string Texture => "TerrorbornMod/Items/Bubble";
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("Bubble");
            }
            public override void SetDefaults()
            {
                projectile.width = 12;
                projectile.height = 12;
                projectile.friendly = false;
                projectile.hostile = true;
                //projectile.extraUpdates = 100;
                projectile.timeLeft = 70;
                projectile.penetrate = 1;
                projectile.light = 0.25f;
                projectile.hide = false;
            }
            public override void Kill(int timeLeft)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54, projectile.position);
            }
        }
    }
    class TidalCrabBubble : ModNPC
    {
        Vector2 trueVelocity = new Vector2(0, 0);
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
            DisplayName.SetDefault("Bubble");
        }
        public override bool PreNPCLoot()
        {
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), mod.ProjectileType("TidalBubbleSmall"), npc.damage / 3, 0);
            }
            NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("TidalCrab"));
            return false;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 30;
            npc.height = 30;
            npc.damage = 32;
            npc.HitSound = SoundID.Item54;
            npc.defense = 0;
            npc.DeathSound = SoundID.Item54;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 12;
            npc.knockBackResist = 0;
        }
        int frame = 0;
        int frameWait = 10;
        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 10;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * 30;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 65;
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            Vector2 targetPosition = Main.player[npc.target].position;
            float speed = 0.4f; //Charging is fast.
            Vector2 move = targetPosition - npc.Center;
            //move.Y -= 200;
            float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            move *= speed / magnitude;
            trueVelocity += move;
            trueVelocity *= 0.98f;
            npc.velocity = trueVelocity;
        }
    }
    class TidalCrab : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 26;
            npc.height = 24;
            npc.damage = 35;
            npc.HitSound = SoundID.NPCHit1;
            npc.defense = 0;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.frame.Width = 388;
            npc.frame.Height = 254;
            npc.lifeMax = 37;
            npc.knockBackResist = 0.01f;
        }
        int frame = 0;
        int frameWait = 8;
        public override void NPCLoot()
        {
            if (Main.rand.Next(7) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
            }
        }
        public override void FindFrame(int frameHeight)
        {
            frameWait--;
            if (frameWait <= 0)
            {
                frameWait = 8;
                frame++;
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }
        bool MovingFoward = true;
        int ChangeDirectionWait = 0;
        public override void AI()
        {
            ChangeDirectionWait--;
            if (ChangeDirectionWait <= 0)
            {
                if (MovingFoward)
                {
                    MovingFoward = false;
                    ChangeDirectionWait = Main.rand.Next(10, 60);
                }
                else
                {
                    MovingFoward = true;
                    ChangeDirectionWait = Main.rand.Next(80, 150);
                }
            }
            if (npc.Center.X < Main.player[npc.target].position.X)
            {
                if (MovingFoward)
                {
                    npc.velocity.X += 0.25f;
                }
                else
                {
                    npc.velocity.X -= 0.25f;
                }
            }
            if (npc.Center.X > Main.player[npc.target].position.X)
            {
                
                if (!MovingFoward)
                {
                    npc.velocity.X += 0.25f;
                }
                else
                {
                    npc.velocity.X -= 0.25f;
                }
            }
        }
    }
    class TideFireWait : ModProjectile
    {
        float FireWait = 120;
        int FireWait2 = 6;
        int FiresLeft = 20;
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
        }
        bool start = true;
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (FireWait > 0)
            {
                Utils.DrawLine(spriteBatch, projectile.Center, new Vector2(projectile.Center.X, projectile.Center.Y - 300), Color.LightYellow, Color.LightYellow, 3);
            }
            base.PostDraw(spriteBatch, lightColor);
        }
        public override void AI()
        {
            Vector2 position = new Vector2(projectile.Center.X, projectile.position.Y - 60);
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                position.Y++;
            }
            projectile.position.Y = position.Y;
            if (start)
            {
                start = false;
                //FireWait = projectile.ai[0];
            }
            FireWait--;
            if (FireWait <= 0)
            {
                FireWait2--;
                if (FireWait2 <= 0)
                {
                    FireWait2 = 3;
                    FiresLeft--;
                    if (FiresLeft <= 0)
                    {
                        projectile.active = false;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item34, projectile.position);
                    Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y - 20), new Vector2(0, -20), mod.ProjectileType("TideFire"), 18, projectile.knockBack);
                    projectile.velocity.X = 0;
                }
            }
            else
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, Main.rand.Next(-4, 5), -15, Scale: 2, newColor: Color.SkyBlue);
                Main.dust[dust].noGravity = true;
                if (Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center.X > projectile.Center.X + 10)
                {
                    projectile.velocity.X = 4;
                }
                else if (Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center.X < projectile.Center.X - 10)
                {
                    projectile.velocity.X = -4;
                }
                else
                {
                    projectile.velocity.X = 0;
                }
            }
        }
    }

    class JumpGeyserTelegraph : ModProjectile
    {
        float FireWait = 90;
        int FireWait2 = 6;
        int FiresLeft = 5;

        int telegraphLength = 60;
        float telegraphAlpha = 0;
        int telegraphAlphaDirection = 1;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Color color = Color.LightSkyBlue;
            color.A = (int)(255 * 0.75f);
            Utils.DrawLine(spriteBatch, projectile.Center, projectile.Center + new Vector2(0, -telegraphLength), color * telegraphAlpha, Color.Transparent, 3);
            return false;
        }

        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            projectile.width = 2;
            projectile.height = 2;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
            projectile.alpha = 255;
        }
        bool start = true;
        //public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    if (FireWait > 0)
        //    {
        //        Utils.DrawLine(spriteBatch, projectile.Center, new Vector2(projectile.Center.X, projectile.Center.Y - 300), Color.LightYellow, Color.LightYellow, 3);
        //    }
        //    base.PostDraw(spriteBatch, lightColor);
        //}

        public override void AI()
        {
            Vector2 position = new Vector2(projectile.Center.X, projectile.position.Y - 60);
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                position.Y++;
            }
            projectile.position.Y = position.Y;
            if (start)
            {
                start = false;
                FireWait = projectile.ai[0];
            }
            FireWait--;
            if (FireWait == 1)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item88, Main.player[Main.myPlayer].Center);
                int proj = Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y - 20), new Vector2(0, 10), mod.ProjectileType("TideFire"), 18, projectile.knockBack);
                Main.projectile[proj].tileCollide = true;
                Main.projectile[proj].ignoreWater = true;
            }
            if (FireWait <= 0)
            {
                FireWait2--;
                if (FireWait2 <= 0)
                {
                    FireWait2 = 3;
                    FiresLeft--;
                    if (FiresLeft <= 0)
                    {
                        projectile.active = false;
                    }
                    Projectile.NewProjectile(new Vector2(projectile.position.X, projectile.position.Y - 20), new Vector2(0, -20), mod.ProjectileType("TideFire"), 18, projectile.knockBack);
                    projectile.velocity.X = 0;
                }
            }
            else
            {
                telegraphLength += 6;
                if (telegraphAlphaDirection == 1)
                {
                    telegraphAlpha += 2f / projectile.ai[0];
                    if (telegraphAlpha >= 1)
                    {
                        telegraphAlphaDirection = -1;
                    }
                }
                else
                {
                    telegraphAlpha -= 2f / projectile.ai[0];
                }
            }
        }
    }
}
