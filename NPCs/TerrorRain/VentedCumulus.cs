using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Events;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.UI;

namespace TerrorbornMod.NPCs.TerrorRain
{
    class VentedCumulus : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 56;
            npc.height = 260 / 5;
            npc.damage = 0;
            npc.defense = 25;
            npc.lifeMax = 750;
            npc.HitSound = SoundID.NPCHit30;
            npc.DeathSound = SoundID.NPCDeath33;
            npc.value = 250;
            npc.knockBackResist = 0.25f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    if (Main.rand.NextFloat() <= 0.4f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
                else
                {
                    if (Main.rand.NextFloat() <= 0.2f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
            }

            if (Main.rand.NextFloat() <= 0.75f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.ThunderShard>());
            }

            if (Main.rand.NextFloat() <= 0.125f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Ranged.ThunderGrenade>());
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            if (npc.direction == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/VentedCumulus_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 3;
            }
            if (frame >= 5)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornWorld.terrorRain && Main.raining && spawnInfo.player.ZoneRain)
            {
                return 0.35f;
            }
            else
            {
                return 0f;
            }
        }

        int projectileWait = 5;
        NPC targetNPC;
        public override void AI()
        {
            npc.TargetClosest(true);
            npc.spriteDirection = npc.direction;
            Player player = Main.player[npc.target];
            if (npc.ai[0] == 0)
            {
                npc.velocity *= 0.98f;

                Vector2 target = player.Center + new Vector2(0, -200);
                float speed = 0.3f;
                Vector2 velocityIncrease = speed * npc.DirectionTo(target);
                npc.velocity += velocityIncrease;

                npc.ai[1]++;
                if (npc.ai[1] > 420)
                {
                    npc.ai[1] = 0;
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<VentedCumulusRaincloud>(), 75 / 4, 0);

                    float Distance = 3500; //max distance away
                    bool Targeted = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].Distance(npc.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].type != ModContent.NPCType<FrightcrawlerBody>() && Main.npc[i].type != ModContent.NPCType<FrightcrawlerTail>() && Main.npc[i].active && Main.npc[i].type != npc.type)
                        {
                            targetNPC = Main.npc[i];
                            Distance = Main.npc[i].Distance(npc.Center);
                            Targeted = true;
                        }
                    }
                    if (Targeted)
                    {
                        npc.ai[0] = 1;
                    }
                }
            }
            if (npc.ai[0] == 1)
            {
                Vector2 target = new Vector2(targetNPC.Center.X, targetNPC.position.Y - 50);
                target.X -= npc.width / 2;
                target.Y -= npc.height / 2;

                npc.velocity = (target - npc.position) / 10 + targetNPC.velocity;

                projectileWait--;
                if (projectileWait <= 0)
                {
                    projectileWait = 7;
                    Vector2 position = new Vector2(Main.rand.Next((int)npc.position.X, (int)npc.position.X + npc.width), npc.position.Y + npc.height);
                    Projectile.NewProjectile(position, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.VentRain>(), 50 / 4, 0);
                }

                TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(targetNPC);
                globalNPC.CumulusEmpowermentTime = 600;

                npc.ai[1]++;
                if (npc.ai[1] > 180)
                {
                    npc.ai[1] = 0;
                    npc.ai[0] = 0;
                }
            }
        }
    }

    class VentedCumulusRaincloud : ModProjectile
    {
        int trueTimeLeft = 60 * 15;

        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 10000;
            projectile.tileCollide = false;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
        }

        void FindFrame()
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 4;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            base.PostDraw(spriteBatch, lightColor);
            Texture2D texture = ModContent.GetTexture(Texture + "_Glow");
            Vector2 position = projectile.position - Main.screenPosition;
            //position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(0, 0), SpriteEffects.None, 0);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 60 * 4);
        }

        int projectileWait = 5;
        public override void AI()
        {
            FindFrame();

            projectileWait--;
            if (projectileWait <= 0)
            {
                projectileWait = 7;
                Vector2 position = new Vector2(Main.rand.Next((int)projectile.position.X, (int)projectile.position.X + projectile.width), projectile.position.Y + projectile.height);
                Projectile.NewProjectile(position, new Vector2(0, 10), ModContent.ProjectileType<Projectiles.VentRain>(), projectile.damage, 0);
            }

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                projectileWait = 10;
                projectile.alpha += 255 / 60;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
    }
}
