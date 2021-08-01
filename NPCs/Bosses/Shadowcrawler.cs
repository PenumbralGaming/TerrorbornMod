using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerrorbornMod.Items;
using System.Runtime;
using System;
using Microsoft.Xna.Framework;
using TerrorbornMod;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using Terraria.Utilities;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs.Bosses
{
    [AutoloadBossHead]
    class Shadowcrawler : ModNPC
    {
        public override void NPCLoot()
        {
            TerrorbornWorld.downedShadowcrawler = true;
            Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.PermanentUpgrades.AnekronianApple>());
            if (Main.expertMode)
            {
                Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.TreasureBags.SC_TreasureBag>());
            }
            else
            {
                Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Materials.SoulOfPlight>(), Stack: Main.rand.Next(25, 41));
                int choice = Main.rand.Next(3);
                if (choice == 0)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Shadowcrawler.BladeOfShade>());
                }
                else if (choice == 1)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Shadowcrawler.Nightbrood>());
                }
                else if (choice == 2)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Shadowcrawler.BoiledBarrageWand>());
                }
                int armorChoice = Main.rand.Next(3);
                if (armorChoice == 0)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
                }
                if (armorChoice == 1)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisChestplate>());
                }
                if (armorChoice == 2)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisMask>());
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Armor.TenebrisLeggings>());
                }
                if (Main.rand.Next(7) == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Vanity.BossMasks.ShadowcrawlerMask>());
                }
            }
        }

        public override bool CheckActive()
        {
            return false;
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

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !Jumping;
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 15;
            DisplayName.SetDefault("Shadowcrawler");
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 158;
            npc.height = 126;
            npc.damage = 60;
            npc.HitSound = SoundID.NPCHit29;
            npc.defense = 15;
            npc.DeathSound = SoundID.NPCDeath31;
            npc.frame.Width = 388;
            music = mod.GetSoundSlot(Terraria.ModLoader.SoundType.Music, "Sounds/Music/CrepuscularCrawler");
            npc.boss = true;
            npc.frame.Height = 254;
            npc.lifeMax = 27000;
            npc.knockBackResist = 0;
        }
        bool Attacking = false;
        int Frame = 0;
        int FrameWait = 6;
        //|--------Frames--------|
        //Jump = 8
        //Standing Still = 0-7
        //Attacking = 9-12
        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = Frame * frameHeight;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);

            Vector2 drawPos = npc.position - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
            Color color = npc.GetAlpha(drawColor);
            Texture2D Texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler");
            if (phase == 2)
            {
                Texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2");
            }
            if (phase == 3)
            {
                Texture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3");
            }
            spriteBatch.Draw(Texture, drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);

            if (phase == 3)
            {
                for (int i = 0; i < npc.oldPos.Length; i++)
                {
                    drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                    color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                    Texture2D glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_Glow");
                    if (phase == 2)
                    {
                        glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2_Glow");
                    }
                    if (phase == 3)
                    {
                        glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3_Glow");
                    }
                    spriteBatch.Draw(glowTexture, drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            else
            {
                drawPos = npc.position - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                color = npc.GetAlpha(Color.White);
                Texture2D glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_Glow");
                if (phase == 2)
                {
                    glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_2_Glow");
                }
                if (phase == 3)
                {
                    glowTexture = ModContent.GetTexture("TerrorbornMod/NPCs/Bosses/Shadowcrawler_3_Glow");
                }
                spriteBatch.Draw(glowTexture, drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //if (!Jumping)
            //{
            //    Color mainColor = Color.LightGreen;
            //    mainColor *= 0.25f;
            //    float fadeOffset = 2f;
            //    Utils.DrawLine(spriteBatch, npc.position + new Vector2(0, npc.height), npc.position + new Vector2(npc.width, npc.height), mainColor, mainColor, 3f);
            //    Utils.DrawLine(spriteBatch, npc.position + new Vector2(-20, npc.height), npc.position + new Vector2(fadeOffset, npc.height), Color.Transparent, mainColor, 3f);
            //    Utils.DrawLine(spriteBatch, npc.position + new Vector2(npc.width - fadeOffset, npc.height), npc.position + new Vector2(npc.width + 20, npc.height), mainColor, Color.Transparent, 3f);
            //}
        }

        public void WalkingAnimation(int frameCooldown)
        {
            FrameWait--;
            if (Frame < 7)
            {
                Frame = 7;
            }

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame++;
            }

            if (Frame > 13)
            {
                Frame = 7;
            }
        }
        public void BackwardsWalkingAnimation(int frameCooldown)
        {
            FrameWait--;
            if (Frame < 7)
            {
                Frame = 13;
            }

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame--;
            }

            if (Frame > 13)
            {
                Frame = 13;
            }
        }

        public void StillAnimation(int frameCooldown)
        {
            FrameWait--;

            if (FrameWait <= 0)
            {
                FrameWait = frameCooldown;
                Frame++;
            }

            if (Frame > 6)
            {
                Frame = 0;
            }
        }

        public void JumpAnimation()
        {
            Frame = 14;
        }

        public void ResetAnimations()
        {
            FrameWait = 1;
        }

        public void lockToScreen()
        {
            lockedToScreen = true;
            positionOnScreen = npc.position - Main.screenPosition;
        }

        const float fallingSpeed = 1f;
        Vector2 jumpTarget;
        public void SetJump(Vector2 targetPosition, bool screenLocked)
        {
            jumpTarget = targetPosition;
            Jumping = true;
            jumpCounter = 60;

            if (screenLocked)
            {
                velocity.X = (targetPosition.X - positionOnScreen.X) / 60;
                velocity.Y = (targetPosition.Y - (460f * 4f * fallingSpeed) - positionOnScreen.Y) / 60 + fallingSpeed;

            }
            else
            {
                velocity.X = (targetPosition.X - npc.position.X) / 60;
                velocity.Y = (targetPosition.Y - (460f * 4f * fallingSpeed) - npc.position.Y) / 60 + fallingSpeed;
            }
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.85);
            npc.defense = 25;
        }

        int beepCounter;
        int beepCount;
        public void BeepingSounds(int numberOfBeeps)
        {
            beepCount = numberOfBeeps;
            beepCounter = 0;
        }

        public void DiagonalCharge(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                npc.ai[1] = 1;
                phaseCounter1 = 60;
                phaseCounter2 = 75;
                projectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    npc.ai[1] = -1;
                }
                int xOffset = 400;
                SetJump(centerOfScreen + new Vector2(xOffset * npc.ai[1] - npc.width / 2, xOffset - npc.height / 2), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    velocity.Y = -18;
                    velocity.X = -npc.ai[1] * 18;
                    if (phase == 3)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, npc.Center);
                        Vector2 direction = velocity;
                        direction.Normalize();
                        float speed = 12;
                        int damage = 65 / 4;
                        int proj1 = Projectile.NewProjectile(npc.Center, direction.RotatedBy(MathHelper.ToRadians(90)) * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj1].ai[0] = 120;
                        Main.projectile[proj1].ai[1] = 3f;
                        int proj2 = Projectile.NewProjectile(npc.Center, direction.RotatedBy(MathHelper.ToRadians(-90)) * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj2].ai[0] = 120;
                        Main.projectile[proj2].ai[1] = 3f;
                    }
                    autofacePlayer = false;
                }
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {
                lockedToScreen = false;
                WalkingAnimation(2);
                spriteDirection = (int)-npc.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 60;
                    ResetAnimations();
                }
                projectileCounter1--;
                if (projectileCounter1 <= 0)
                {
                    projectileCounter1 = 8;
                    if (phase >= 2)
                    {
                        projectileCounter1 = 7;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, npc.Center);
                    Vector2 direction = npc.velocity;
                    direction.Normalize();
                    float speed = 1;
                    int damage = 65 / 4;
                    int proj1 = Projectile.NewProjectile(npc.Center, direction.RotatedBy(MathHelper.ToRadians(90)) * speed, ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                    int proj2 = Projectile.NewProjectile(npc.Center, direction.RotatedBy(MathHelper.ToRadians(-90)) * speed, ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                    //Main.projectile[proj].ai[0] = 200f;
                    //Main.projectile[proj].ai[1] = 0.5f;
                    //if (phase == 2)
                    //{
                    //    Main.projectile[proj].ai[1] = 0.65f;
                    //}
                    //if (phase == 3)
                    //{
                    //    Main.projectile[proj].ai[1] = 1.5f;
                    //}
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void EggSpew(Player player, int numberOfEggs, int delayBetweenEggs)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockedToScreen = false;
                ResetAnimations();
                phaseCounter1 = numberOfEggs;
                phaseCounter2 = delayBetweenEggs;
                phaseCounter3 = 110;
                int direction1 = 0;
                if (Main.rand.NextBool())
                {
                    direction1 = 1;
                }
                else
                {
                    direction1 = -1;
                }
                int direction2 = 0;
                if (Main.rand.NextBool())
                {
                    direction2 = 1;
                }
                else
                {
                    direction2 = -1;
                }
                SetJump(player.Center + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), false);
                autofacePlayer = true;
            }
            else if (phaseCounter1 > 0)
            {
                if (phaseCounter2 > 0)
                {
                    phaseCounter2--;
                    StillAnimation(3);
                }
                else
                {
                    phaseCounter1--;
                    phaseCounter2 = delayBetweenEggs;
                    float distance = Main.rand.Next(100, 350);
                    Vector2 position = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * distance + player.Center + player.velocity * 90;
                    int proj = Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<ShadowEgg>(), 0, 0);
                    Main.projectile[proj].ai[0] = position.X;
                    Main.projectile[proj].ai[1] = position.Y;

                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, npc.Center);
                }
            }
            else
            {
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void MultipleJumpsWithFireballs(Player player, int jumpCount)
        {
            int projectileCount = 8;
            if (phase >= 3)
            {
                projectileCount = 10;
            }
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                ResetAnimations();
                phaseCounter1 = jumpCount;
                phaseCounter2 = 45;
                if (phase >= 2)
                {
                    phaseCounter2 = 32;
                }
                int direction1 = 0;
                if (Main.rand.NextBool())
                {
                    direction1 = 1;
                }
                else
                {
                    direction1 = -1;
                }
                int direction2 = 0;
                if (Main.rand.NextBool())
                {
                    direction2 = 1;
                }
                else
                {
                    direction2 = -1;
                }
                SetJump(centerOfScreen + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), true);
                autofacePlayer = true;
            }
            else
            {
                if (phaseCounter2 > 0)
                {
                    phaseCounter2--;
                    StillAnimation(3);
                    float direction = 0;
                    float speed = 30;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / projectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        Dust dust = Dust.NewDustPerfect(npc.Center, 74);
                        dust.velocity = directionVector * speed + player.velocity;
                        dust.noLight = true;
                        dust.noGravity = true;
                    }
                }
                else
                {
                    phaseCounter1--;
                    phaseCounter2 = 45;
                    if (phase >= 2)
                    {
                        phaseCounter2 = 32;
                    }
                    float direction = 0;
                    float speed = 17;
                    if (phase >= 2)
                    {
                        speed = 23;
                    }
                    int timeLeft = 120;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / projectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        int proj = Projectile.NewProjectile(npc.Center, directionVector * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), 90 / 4, 0);
                        Main.projectile[proj].ai[0] = timeLeft;
                        if (phase == 3)
                        {
                            Main.projectile[proj].ai[1] = 0.45f;
                        }
                        else
                        {
                            Main.projectile[proj].ai[1] = 0.65f;
                        }
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, npc.Center);
                    if (phaseCounter1 <= 0)
                    {
                        DecideNextAttack();
                    }
                    else
                    {
                        int direction1 = 0;
                        if (Main.rand.NextBool())
                        {
                            direction1 = 1;
                        }
                        else
                        {
                            direction1 = -1;
                        }
                        int direction2 = 0;
                        if (Main.rand.NextBool())
                        {
                            direction2 = 1;
                        }
                        else
                        {
                            direction2 = -1;
                        }
                        SetJump(centerOfScreen + new Vector2(Main.rand.Next(300, 600) * direction1, Main.rand.Next(200, 400) * direction2), true);
                    }
                }
            }
        }

        public void PredictiveJump(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                ResetAnimations();
                phaseCounter1 = 30;
                phaseCounter2 = 60;
                if (phase >= 2)
                {
                    phaseCounter2 = 30;
                }
                phaseCounter3 = 45;
                SetJump(centerOfScreen + new Vector2(0 - npc.width / 2, 300 - npc.height / 2), true);
                autofacePlayer = true;
            }
            else if (phaseCounter1 > 0)
            {
                StillAnimation(3);
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    BeepingSounds(3);
                    ResetAnimations();
                }
            }
            else if (phaseCounter2 > 0)
            {
                lockedToScreen = false;
                if (phase >= 2)
                {
                    BackwardsWalkingAnimation(2);
                }
                else
                {
                    BackwardsWalkingAnimation(5);
                }
                if (phase >= 2)
                {
                    velocity.X = spriteDirection * -6f;
                }
                else
                {
                    velocity.X = spriteDirection * -3f;
                }
                phaseCounter2--;
                if (phaseCounter2 == 1)
                {
                    SetJump(player.Center + player.velocity * 60 + new Vector2(0 - npc.width / 2, 0 - npc.height / 2), false);
                }
                if (phaseCounter2 == 0)
                {
                    int projectileCount = 12;
                    float direction = 0;
                    float speed = 25;
                    int timeLeft = 45;
                    if (phase == 3)
                    {
                        timeLeft *= 4;
                    }
                    int rotationMultiplier = 15;
                    for (int i = 0; i < projectileCount; i++)
                    {
                        direction += MathHelper.ToRadians(360 / projectileCount);
                        Vector2 directionVector = direction.ToRotationVector2();
                        int proj = Projectile.NewProjectile(npc.Center, directionVector * speed, ModContent.ProjectileType<LargeNightmareFlame>(), 90 / 4, 0);
                        Main.projectile[proj].timeLeft = timeLeft;
                        Main.projectile[proj].ai[0] = rotationMultiplier;
                    }

                    if (phase == 3)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, npc.Center);
                        Vector2 direction2 = npc.DirectionTo(player.Center);
                        float speed2 = 22;
                        int damage = 65 / 4;
                        int proj2 = Projectile.NewProjectile(npc.Center, direction2 * speed2, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                        Main.projectile[proj2].ai[0] = 200f;
                        Main.projectile[proj2].ai[1] = 0.5f;
                    }
                }
            }
            else
            {
                velocity.X = 0;
                StillAnimation(5);
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void HorizontalCharge(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                npc.ai[1] = 1;
                phaseCounter1 = 60;
                phaseCounter2 = 70;
                projectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    npc.ai[1] = -1;
                }
                int xOffset = 525;
                SetJump(centerOfScreen + new Vector2(xOffset * npc.ai[1] - npc.width / 2, 0 - npc.height / 2), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                if (phaseCounter1 == 1)
                {
                    velocity.Y = player.velocity.Y;
                    autofacePlayer = false;
                }
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {
                lockedToScreen = false;
                WalkingAnimation(2);
                velocity.X = -npc.ai[1] * 18;
                spriteDirection = (int)-npc.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 120;
                    ResetAnimations();
                }
                projectileCounter1--;
                if (projectileCounter1 <= 0)
                {
                    projectileCounter1 = 15;
                    if (phase >= 2)
                    {
                        projectileCounter1 = 10;
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, npc.Center);
                    Vector2 direction = npc.DirectionTo(player.Center);
                    float speed = 15;
                    int damage = 65 / 4;
                    int proj = Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                    Main.projectile[proj].ai[0] = 200f;
                    Main.projectile[proj].ai[1] = 0.5f;
                    if (phase == 2)
                    {
                        Main.projectile[proj].ai[1] = 0.65f;
                    }
                    if (phase == 3)
                    {
                        Main.projectile[proj].ai[1] = 1.5f;
                    }
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        public void OverheadFireballDropping(Player player)
        {
            if (phaseStart)
            {
                phaseStart = false;
                lockToScreen();
                npc.ai[1] = 1;
                phaseCounter1 = 70;
                if (phase == 3)
                {
                    phaseCounter1 = 50;
                }
                phaseCounter2 = 45;
                projectileCounter1 = 0;
                phaseCounter3 = 0;
                autofacePlayer = true;
                if (Main.rand.NextBool())
                {
                    npc.ai[1] = -1;
                }
                SetJump(centerOfScreen + new Vector2(350 * npc.ai[1] - npc.width / 2, -350), true);
            }
            if (phaseCounter1 > 0)
            {
                phaseCounter1--;
                StillAnimation(3);
            }
            else if (phaseCounter3 == 0)
            {

                autofacePlayer = false;
                WalkingAnimation(2);
                velocity.X = -npc.ai[1] * 15;
                spriteDirection = (int)-npc.ai[1];
                phaseCounter2--;
                if (phaseCounter2 <= 0)
                {
                    phaseCounter3 = 105;
                    ResetAnimations();
                }
                projectileCounter1--;
                if (projectileCounter1 <= 0)
                {
                    projectileCounter1 = 5;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot, npc.Center);
                    Vector2 direction = npc.DirectionTo(player.Center);
                    float speed = 14;
                    if (phase >= 2)
                    {
                        speed = 16;
                    }
                    int damage = 65 / 4;
                    int proj = Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<Projectiles.MidnightFireball>(), damage, 0);
                    Main.projectile[proj].ai[0] = 120f;
                    Main.projectile[proj].ai[1] = 2.5f;
                }
            }
            else
            {
                StillAnimation(5);
                velocity = Vector2.Zero;
                lockedToScreen = false;
                phaseCounter3--;
                if (phaseCounter3 <= 0)
                {
                    DecideNextAttack();
                }
            }
        }

        const float secondPhaseHealth = 0.60f;
        const float desparationPhaseHealth = 0.2f;

        public void DecideNextAttack()
        {
            if (NextAttacks.Count <= 0)
            {
                WeightedRandom<int> listOfAttacks = new WeightedRandom<int>();
                listOfAttacks.Add(0);
                listOfAttacks.Add(1);
                listOfAttacks.Add(2);
                listOfAttacks.Add(3);
                if (phase >= 2)
                {
                    listOfAttacks.Add(4);
                }
                listOfAttacks.Add(5);
                for (int i = 0; i < listOfAttacks.elements.Count; i++)
                {
                    int choice = listOfAttacks.Get();
                    while (NextAttacks.Contains(choice) || (choice == LastAttack && NextAttacks.Count == 0))
                    {
                        choice = listOfAttacks.Get();
                    }
                    NextAttacks.Add(choice);
                }
            }
            if (phase == 1 && npc.life <= secondPhaseHealth * npc.lifeMax)
            {
                phaseTransitionCounter = 90;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                ResetAnimations();
                NextAttacks.Add(4);
            }
            else if (phase == 2 && npc.life <= desparationPhaseHealth * npc.lifeMax)
            {
                phaseTransitionCounter = 90;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                ResetAnimations();
            }
            else
            {
                AIPhase = NextAttacks[0];
                LastAttack = AIPhase;
                NextAttacks.RemoveAt(0);
            }
            phaseStart = true;
        }

        List<int> NextAttacks = new List<int>();
        int AIPhase = 69;
        int LastAttack = -1;
        bool lockedToScreen = false;
        bool Jumping = false;
        int jumpCounter = 0;
        bool phaseStart;
        int phaseCounter1;
        int phaseCounter2;
        int phaseCounter3;
        int projectileCounter1;
        int spriteDirection;
        bool autofacePlayer = true;
        int phase = 1;
        int phaseTransitionCounter = 0;

        Vector2 positionOnScreen;
        Vector2 velocity = Vector2.Zero;
        Vector2 centerOfScreen = new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);

        public override void AI()
        {
            Player player = Main.player[npc.target];

            if (player.dead || Main.dayTime)
            {
                npc.active = false;
                DustExplosion(npc.Center, 0, 25, 25, 74, 1.5f, true);
                return;
            }

            if ((phase == 1 && npc.life <= secondPhaseHealth * npc.lifeMax) || phaseTransitionCounter > 0 || (phase == 2 && npc.life <= desparationPhaseHealth * npc.lifeMax))
            {
                npc.dontTakeDamage = true;
            }
            else
            {
                npc.dontTakeDamage = false;
            }

            if (autofacePlayer)
            {
                if (player.Center.X > npc.Center.X)
                {
                    spriteDirection = 1;
                }
                else
                {
                    spriteDirection = -1;
                }
            }
            npc.TargetClosest(false);
            npc.direction = spriteDirection;

            if (beepCount > 0)
            {
                if (beepCounter > 0)
                {
                    beepCounter--;
                }
                else
                {
                    beepCount--;
                    beepCounter = 5;
                    ModContent.GetSound("TerrorbornMod/Sounds/Effects/undertalewarning").Play(Main.soundVolume, 0f, 0f);
                }
            }

            if (Jumping)
            {
                JumpAnimation();
                velocity.Y += fallingSpeed;
                jumpCounter--;
                if (jumpCounter <= 0)
                {
                    Jumping = false;
                    velocity = Vector2.Zero;
                    if (lockedToScreen)
                    {
                        positionOnScreen = jumpTarget;
                    }
                    else
                    {
                        npc.position = jumpTarget;
                    }
                    ResetAnimations();
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                    for (int i = 0; i < 30; i++)
                    {
                        int dust = Dust.NewDust(npc.position + new Vector2(0, npc.height), npc.width, 10, 74);
                        if (lockedToScreen)
                        {
                            Main.dust[dust].velocity = player.velocity;
                        }
                        Main.dust[dust].noGravity = true;
                    }
                }
            }
            else
            {
                if (phaseTransitionCounter > 0)
                {
                    phaseTransitionCounter--;
                    StillAnimation(2);
                    TerrorbornMod.ScreenShake(10);
                    if (phaseTransitionCounter <= 0)
                    {
                        phase++;
                        DecideNextAttack();
                    }
                }
                else
                {
                    if (AIPhase == 69)
                    {
                        DecideNextAttack();
                    }
                    if (AIPhase == 0)
                    {
                        OverheadFireballDropping(player);
                    }
                    if (AIPhase == 1)
                    {
                        HorizontalCharge(player);
                    }
                    if (AIPhase == 2)
                    {
                        PredictiveJump(player);
                    }
                    if (AIPhase == 3)
                    {
                        MultipleJumpsWithFireballs(player, 3);
                    }
                    if (AIPhase == 4)
                    {
                        if (phase == 3)
                        {
                            EggSpew(player, 12, 7);
                        }
                        else
                        {
                            EggSpew(player, 5, 10);
                        }
                    }
                    if (AIPhase == 5)
                    {
                        DiagonalCharge(player);
                    }
                }
            }

            if (lockedToScreen)
            {
                positionOnScreen += velocity;
                npc.velocity = Vector2.Zero;
                Vector2 relativeToPlayer = centerOfScreen - Main.screenPosition;
                npc.position = player.Center - centerOfScreen + positionOnScreen;
            }
            else
            {
                npc.velocity = velocity;
            }
        }
    }

    class ShadowEgg : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 46;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 90;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(5);
            Vector2 targetPosition = new Vector2(projectile.ai[0], projectile.ai[1]);
            projectile.velocity = (targetPosition - projectile.Center) / 10;
        }

        public override void Kill(int timeLeft)
        {
            Vector2 rotation = projectile.DirectionTo(Main.player[Main.myPlayer].Center);
            float speed = 10f;
            Projectile.NewProjectile(projectile.Center, rotation * speed, ModContent.ProjectileType<GhostHatchling>(), 95 / 4, 0);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, projectile.Center);
            DustExplosion(projectile.Center, 0, 12, 7, 74, 2f, true);
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

    class GhostHatchling : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 2);
            projectile.timeLeft = 1;
        }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 34;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 120;
        }

        public override void AI()
        {
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            projectile.rotation = rotation - MathHelper.ToRadians(90);
            float Speed = 0.2f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center).ToRotation(), MathHelper.ToRadians(0.8f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 12, 7, 74, 2f, true);
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
    
    class NightmareFlame : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            projectile.timeLeft = 1;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 200;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 0.5f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
            //NPC targetNPC = Main.npc[0];
            //float Distance = 500; //max distance away
            //bool Targeted = false;
            //for (int i = 0; i < 200; i++)
            //{
            //    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && projectile.CanHit(Main.npc[i]))
            //    {
            //        targetNPC = Main.npc[i];
            //        Distance = Main.npc[i].Distance(projectile.Center);
            //        Targeted = true;
            //    }
            //}
            //if (Targeted)
            //{
            //    //HOME IN
            //    float speed = .5f;
            //    Vector2 move = targetNPC.Center - projectile.Center;
            //    float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
            //    move *= speed / magnitude;
            //    projectile.velocity += move;
            //}
        }
    }
    class LargeNightmareFlame : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
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
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 360;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 2.25f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(0.4f * projectile.ai[0]));
        }
    }
    class NightmareSpawn : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.hide = true;
            projectile.timeLeft = 200;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        int projectileWait = 3;
        public override void AI()
        {
            projectileWait--;
            if (projectileWait <= 0)
            {
                projectileWait = 9;
                int damage = 80;
                damage = Main.expertMode ? (damage / 4) : (damage / 2);
                Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.position.Y), new Vector2(0, -1), ModContent.ProjectileType<NightmareFlame>(), damage, 0);
                Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.position.Y), new Vector2(0, 1), ModContent.ProjectileType<NightmareFlame>(), damage, 0);
            }
        }
    }
    class NightmareShockwave : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            projectile.timeLeft = 1;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Color.White;
            return base.PreDraw(spriteBatch, lightColor);
        }
        public override void SetDefaults()
        {
            projectile.width = 92;
            projectile.height = 144 / Main.projFrames[projectile.type];
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 180;
            projectile.alpha = 255;
            projectile.light = 0.5f;
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
            return false;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1f);
            if (projectile.alpha == 255)
            {
                Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<ShockWaveTelegraph>(), 0, 0);
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
            }

            FindFrame(projectile.height);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = .75f;
            if (NPC.AnyNPCs(ModContent.NPCType<PrototypeI>()))
            {
                Speed = 0.5f;
            }
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }
    }

    class ShockWaveTelegraph : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/PlasmaCore";
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
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }
        int DustWait = 5;
        public override void AI()
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] > 9f)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 61, Vector2.Zero);
                dust.noGravity = true;
                dust.noLight = true;
                dust.scale = 1.25f;
                dust.alpha = 255 / 2;

                //for (int i = 0; i < 4; i++)
                //{
                //    Vector2 projectilePosition = projectile.position;
                //    projectilePosition -= projectile.velocity * ((float)i * 0.25f);
                //    projectile.alpha = 255;
                //    DustWait--;
                //    if (DustWait <= 0)
                //    {
                //        int dust = Dust.NewDust(projectilePosition, projectile.width, projectile.height, 0, 0);
                //        Main.dust[dust].noGravity = true;
                //        Main.dust[dust].position = projectilePosition;
                //        DustWait = 5;
                //        Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                //        Main.dust[dust].velocity *= 0.2f;
                //    }
                //}
            }
        }
    }
}