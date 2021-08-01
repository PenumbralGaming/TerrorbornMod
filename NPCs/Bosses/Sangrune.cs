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

namespace TerrorbornMod.NPCs.Bosses
{
    class Sangrune : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 7;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 162;
            npc.height = 132;
            npc.damage = 25;
            npc.defense = 7;
            npc.lifeMax = 1500;
            npc.HitSound = SoundID.NPCHit18;
            npc.DeathSound = SoundID.NPCDeath32;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.buffImmune[BuffID.Confused] = true;
            if (Main.hardMode)
            {
                npc.lifeMax = 5000;
                npc.damage = 40;
                npc.defense = 17;
            }
        }
        int FrameWait = 0;
        int Frame = 0;
        public override void FindFrame(int frameHeight)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = 6;
                Frame++;
                if (Frame >= Main.npcFrameCount[npc.type])
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = Frame * frameHeight;
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 2000;
            if (Main.hardMode)
            {
                npc.lifeMax = 7500;
            }
        }
        int AIPhase = 0;
        int PhaseCounter = 450;
        int AttackCounter;
        public override void AI()
        {
            npc.TargetClosest(false);
            Vector2 targetPosition = Main.player[npc.target].Center;
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);
            if (Main.dayTime || Main.player[npc.target].dead || !Main.bloodMoon)
            {
                npc.velocity.Y += 0.1f;
                if (Main.player[npc.target].Distance(npc.Center) > 2000)
                {
                    npc.active = false;
                }
            }
            else
            {
                if (AIPhase == 0)
                {
                    PhaseCounter--;
                    if (PhaseCounter <= 0)
                    {
                        PhaseCounter = 3;
                        AttackCounter = 120;
                        AIPhase = 1;
                    }

                    float acceleration = 0.1f;
                    float deceleration = 0.05f;

                    if (targetPosition.X < npc.Center.X && npc.velocity.X > -8)
                    {
                        npc.velocity.X -= acceleration; // accelerate to the left
                        if (npc.velocity.X > 0)
                        {
                            npc.velocity.X -= deceleration;
                        }
                    }
                    if (targetPosition.X > npc.Center.X && npc.velocity.X < 8)
                    {
                        npc.velocity.X += acceleration; // accelerate to the right
                        if (npc.velocity.X < 0)
                        {
                            npc.velocity.X += deceleration;
                        }
                    }
                    if (targetPosition.Y < npc.Center.Y && npc.velocity.Y > -8)
                    {
                        npc.velocity.Y -= acceleration; // accelerate up
                        if (npc.velocity.Y > 0)
                        {
                            npc.velocity.Y -= deceleration;
                        }
                    }
                    if (targetPosition.Y > npc.Center.Y && npc.velocity.Y < 8)
                    {
                        npc.velocity.Y += acceleration; // accelerate down
                        if (npc.velocity.Y < 0)
                        {
                            npc.velocity.Y += deceleration;
                        }
                    }
                }
                if (AIPhase == 1)
                {
                    npc.velocity.Y *= 0.98f;
                    npc.velocity.X *= 0.98f;
                    AttackCounter--;
                    if (AttackCounter <= 0)
                    {
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath10, npc.Center);
                        AttackCounter = 10;
                        PhaseCounter--;
                        if (PhaseCounter <= 0)
                        {
                            AIPhase = 0;
                            PhaseCounter = 450;
                        }
                        DustExplosion(npc.Center, 0, 40, 25, 60, DustScale: 1.5f, NoGravity: true);
                        for (int i = 0; i < 200; i++)
                        {
                            if (!Main.npc[i].friendly)
                            {
                                if (Main.hardMode)
                                {
                                    Main.npc[i].life += 100;
                                    if (Main.npc[i].active)
                                    {
                                        Main.npc[i].HealEffect(100);
                                    }
                                    if (Main.npc[i].life > Main.npc[i].lifeMax)
                                    {
                                        Main.npc[i].life = Main.npc[i].lifeMax;
                                    }
                                }
                                else
                                {
                                    Main.npc[i].life += 20;
                                    if (Main.npc[i].active)
                                    {
                                        Main.npc[i].HealEffect(20);
                                    }
                                    if (Main.npc[i].life > Main.npc[i].lifeMax)
                                    {
                                        Main.npc[i].life = Main.npc[i].lifeMax;
                                    }
                                }
                            }
                        }
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
        public override void NPCLoot()
        {
            TerrorbornWorld.downedSangrune = true;
            for (int i = 0; i < Main.rand.Next(5, 9); i++)
            {
                Item.NewItem(npc.position, npc.width, npc.height, ItemID.Heart);
            }
            if (Main.hardMode)
            {
                TerrorbornWorld.downedSangrune2 = true;
                for (int i = 0; i < Main.rand.Next(7, 14); i++)
                {
                    Item.NewItem(npc.position, npc.width, npc.height, ItemID.SoulofNight);
                    if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    {
                        int choice = Main.rand.Next(3);
                        if (choice == 0)
                        {
                            Item.NewItem(npc.position, npc.width, npc.height, ItemID.SoulofSight);
                        }
                        if (choice == 1)
                        {
                            Item.NewItem(npc.position, npc.width, npc.height, ItemID.SoulofFright);
                        }
                        if (choice == 2)
                        {
                            Item.NewItem(npc.position, npc.width, npc.height, ItemID.SoulofMight);
                        }
                    }
                }
            }
            Item.NewItem(npc.position, npc.width, npc.height, mod.ItemType("SanguineFang"), Main.rand.Next(4, 9));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.bloodMoon && NPC.downedBoss2 && !NPC.AnyNPCs(npc.type))
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0.04f;
            }
            else
            {
                return 0f;
            }
        }
    }
}
