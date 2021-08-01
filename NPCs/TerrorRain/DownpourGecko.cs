using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class DownpourGecko : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 9;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 86;
            npc.height = 30;
            npc.damage = 45;
            npc.defense = 15;
            npc.lifeMax = 450;
            npc.HitSound = SoundID.NPCHit50;
            npc.DeathSound = SoundID.NPCDeath53;
            npc.value = 250;
            npc.knockBackResist = 0.3f;
            npc.aiStyle = 26;

            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    if (Main.rand.NextFloat() <= 0.3f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
                else
                {
                    if (Main.rand.NextFloat() <= 0.15f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
            }

            if (Main.rand.NextFloat() <= 0.65f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.ThunderShard>());
            }
        }

        int projWait = 0;
        public override void PostAI()
        {
            npc.spriteDirection = npc.direction;

            projWait--;
            if (projWait <= 0)
            {
                projWait = Main.rand.Next(15, 25);
                Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<GeckoGloop>(), 30 / 4, 0);
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/DownpourGecko_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
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
                    npc.frameCounter = 3;
                }
                if (frame >= Main.npcFrameCount[npc.type])
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 8;
            }
            npc.frame.Y = frame * frameHeight;
        }
    }

    class GeckoGloop : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 2;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 16;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 1000;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.Glooped>(), 60 * 10);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        int trueTimeLeft = 60 * 5;
        public override void AI()
        {
            projectile.velocity.Y += 0.2f;

            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frameCounter = 20;
                projectile.frame++;
                if (projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                projectile.alpha += 255 / 60;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
    }
}
