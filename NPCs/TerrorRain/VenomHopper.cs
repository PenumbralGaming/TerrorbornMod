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
    class VenomHopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 3;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.aiStyle = -1;
            npc.width = 38;
            npc.height = 30;
            npc.damage = 65;
            npc.defense = 15;
            npc.lifeMax = 450;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornWorld.terrorRain && Main.raining && spawnInfo.player.ZoneRain)
            {
                return 0.8f;
            }
            else
            {
                return 0f;
            }
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

            if (Main.rand.NextFloat() <= 0.4f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.ThunderShard>());
            }

            if (Main.rand.NextFloat() <= 0.05f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Hooks.VenomTongue>());
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
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/VenomHopper_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        int frame = 0;
        bool attacking = false;

        public override void FindFrame(int frameHeight)
        {
            if (attacking)
            {
                frame = 2;
            }
            else
            {
                if (npc.velocity.Y == 0)
                {
                    frame = 0;
                }
                else
                {
                    frame = 1;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override void AI()
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            else
            {
                npc.spriteDirection = -1;
            }

            attacking = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<VenomTongueTip>() && proj.ai[0] == npc.whoAmI && proj.active)
                {
                    attacking = true;
                    break;
                }
            }

            if (!attacking)
            {
                if (npc.velocity.Y == 0)
                {
                    npc.velocity.X *= 0.90f;
                    npc.ai[0]--;
                    if (npc.ai[0] <= 0)
                    {
                        npc.ai[0] = 60;
                        npc.ai[1]--;
                        if (npc.ai[1] <= 0)
                        {
                            npc.ai[1] = 3;
                            Terraria.Audio.SoundEngine.PlaySound(SoundID.Frog, (int)npc.Center.X, (int)npc.Center.Y, 0, 1.5f, -0.2f);
                            float speed = 20;
                            Vector2 velocity = npc.DirectionTo(player.Center) * speed;
                            Vector2 positionOffset = new Vector2(npc.spriteDirection * 10, 0);
                            int proj = Projectile.NewProjectile(npc.Center + positionOffset, velocity, ModContent.ProjectileType<VenomTongueTip>(), 75 / 4, 0);
                            Main.projectile[proj].ai[0] = npc.whoAmI;
                            Main.projectile[proj].hostile = true;
                        }
                        else
                        {
                            npc.velocity.Y = -10;
                            npc.velocity.X = 5 * npc.spriteDirection;
                        }
                    }
                }
                else
                {

                }
            }
        }
    }

    class VenomTongueTip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.timeLeft = 500;
            projectile.tileCollide = false;
        }

        float speed;
        Vector2 originPoint;
        int timeUntilReturn = 30;

        public override void AI()
        {
            NPC npc = Main.npc[(int)projectile.ai[0]];
            originPoint = npc.Center + new Vector2(npc.spriteDirection * 10, 0);

            projectile.active = npc.active;

            if (timeUntilReturn > 0)
            {
                speed = projectile.velocity.Length();
                timeUntilReturn--;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            }
            else
            {
                projectile.velocity = projectile.DirectionTo(originPoint) * speed;
                if (projectile.Distance(originPoint) <= speed)
                {
                    projectile.active = false;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Venom, 60 * 3);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 center = projectile.Center;
            Vector2 distToProj = originPoint - projectile.Center;
            float projRotation = distToProj.ToRotation() - 1.57f;
            float distance = distToProj.Length();
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/VenomTongueSegment");

            while (distance > texture.Height && !float.IsNaN(distance))
            {
                distToProj.Normalize();
                distToProj *= texture.Height;
                center += distToProj;
                distToProj = originPoint - center;
                distance = distToProj.Length();


                //Draw chain
                spriteBatch.Draw(texture, new Vector2(center.X - Main.screenPosition.X, center.Y - Main.screenPosition.Y),
                    new Rectangle(0, 0, texture.Width, texture.Height), Color.White, projRotation,
                    new Vector2(texture.Width * 0.5f, texture.Height * 0.5f), 1f, SpriteEffects.None, 0f);
            }

            Texture2D texture2 = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture2, new Rectangle((int)position.X, (int)position.Y, texture2.Width, texture2.Height), new Rectangle(0, 0, texture2.Width, texture2.Height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), SpriteEffects.None, 0);
            return false;
        }
    }
}
