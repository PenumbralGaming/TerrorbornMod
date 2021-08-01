using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.NPCs
{
    class TarFlyer : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 62;
            npc.height = 36;
            npc.damage = 45;
            npc.defense = 6;
            npc.lifeMax = 160;
            npc.HitSound = SoundID.NPCHit31;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.knockBackResist = 0.1f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TarOfHunger"), Main.rand.Next(5, 11));
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
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TarFlyer_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 2;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss3)
            {
                return SpawnCondition.DesertCave.Chance * 0.11f;
            }
            else
            {
                return 0f;
            }
        }
        public override void AI()
        {
            npc.TargetClosest(true);
            if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }
            if (Main.player[npc.target].dead)
            {
                float speed = -0.2f;
                Vector2 velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                npc.velocity += velocity;

                npc.velocity *= 0.99f;
                if (npc.Distance(Main.player[npc.target].Center) > 4500)
                {
                    npc.active = false;
                }
            }
            else
            {
                float speed = 0.1f;
                if (!Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
                {
                    speed = 0.04f;
                }
                else
                {
                    int dust = Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldFlame, Scale: 1f);
                    Main.dust[dust].velocity = npc.velocity;
                    Main.dust[dust].noGravity = true;
                }
                Vector2 velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
                npc.velocity += velocity;
                npc.velocity += velocity;

                npc.velocity *= 0.99f;
            }
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 1.5f);
        }
    }
}
