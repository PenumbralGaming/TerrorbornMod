using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs
{
    class TarSludge : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 32;
            npc.height = 46;
            npc.damage = 45;
            npc.defense = 6;
            npc.lifeMax = 170;
            npc.HitSound = SoundID.NPCHit8;
            npc.DeathSound = SoundID.NPCDeath13;
            npc.value = 250;
            npc.knockBackResist = 0.1f;
            npc.aiStyle = 1;
            
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TarOfHunger"), Main.rand.Next(5, 11));
            if (Main.rand.Next(35) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TheLiesOfNourishment"));
            }
        }
        int JavelinCounter = 69;
        public override void PostAI()
        {
            Player player = Main.player[npc.target];
            if (npc.velocity == Vector2.Zero && (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height) || npc.life <= npc.lifeMax * 0.30f))
            {
                if (npc.life <= npc.lifeMax * 0.30f)
                {
                    JavelinCounter -= 3;
                }
                else
                {
                    JavelinCounter--;
                }
                if (JavelinCounter <= 0)
                {
                    JavelinCounter = Main.rand.Next(60, 80);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, npc.Center);
                    Vector2 Rotation = npc.DirectionTo(player.Center);
                    float Speed = 10;
                    if (npc.life <= npc.lifeMax * 0.30f)
                    {
                        Speed = 16;
                    }
                    Vector2 Velocity = Rotation * Speed;
                    Projectile.NewProjectile(npc.Center, Velocity, ModContent.ProjectileType<TarSludgeJavelin>(), 20, 0);
                }
            }
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (npc.velocity.Y == 0)
            {
                npc.frameCounter--;
                if (npc.frameCounter <= 0)
                {
                    frame++;
                    npc.frameCounter = 30;
                }
                if (frame >= 2)
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 1;
            }
            npc.frame.Y = frame * frameHeight;
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3)
            {
                return SpawnCondition.DesertCave.Chance * 0.22f;
            }
            else
            {
                return 0f;
            }
        }
    }

    class TarSludgeJavelin : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 46;
            projectile.height = 46;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }
        public override void AI()
        {
            projectile.rotation =
                projectile.velocity.ToRotation() +
                MathHelper.ToRadians(45f);
            projectile.velocity.Y += 0.05f;
        }
    }
}
