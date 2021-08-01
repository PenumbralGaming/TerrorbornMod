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

namespace TerrorbornMod.NPCs
{
    class Bleak : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 42;
            npc.height = 48;
            npc.damage = 20;
            npc.lifeMax = 52;
            npc.HitSound = SoundID.NPCHit36;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.knockBackResist = 0.2f;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.Confused] = false;
            npc.aiStyle = 22;
        }

        public override void NPCLoot()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkEnergy>());
                if (modPlayer.DeimosteelCharm)
                {
                    if (Main.rand.NextFloat() <= 0.666f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
                else
                {
                    if (Main.rand.NextFloat() <= 0.333f)
                    {
                        Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                    }
                }
            }
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (invulnerable)
            {
                damage = 1;
            }
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (invulnerable)
            {
                damage = 1;
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
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/Bleak_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }
        int frame = 0;
        //float TrueVelocityX = 0;
        public override void FindFrame(int frameHeight)
        {
            if (invulnerable)
            {
                frame = 4;
                npc.frameCounter = 0;
            }
            else
            {
                npc.frameCounter--;
                if (npc.frameCounter <= 0)
                {
                    frame++;
                    npc.frameCounter = 4;
                }
                if (frame >= 4)
                {
                    frame = 0;
                }
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(spawnInfo.player);
            if (modPlayer.ZoneDeimostone)
            {
                if (modPlayer.DeimosteelCharm)
                {
                    return SpawnCondition.Cavern.Chance * 0.22f;
                }
                else
                {
                    return SpawnCondition.Cavern.Chance * 0.08f;
                }
            }
            else
            {
                return 0f;
            }
        }

        bool invulnerable = false;
        int invulCounter = 90;
        public override void AI()
        {
            Player player = Main.player[npc.target];
            npc.TargetClosest();
            if (invulnerable)
            {
                npc.noTileCollide = false;
                npc.knockBackResist = 0.8f;
                invulCounter--;
                if (invulCounter <= 0)
                {
                    invulnerable = false;
                    invulCounter = (int)(60 * Main.rand.NextFloat(4f, 8f));
                }
            }
            else
            {
                npc.noTileCollide = true;
                npc.knockBackResist = 0.01f;
                base.AI();
                if (Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                {
                    invulCounter--;
                }
                if (invulCounter <= 0)
                {
                    invulnerable = true;
                    invulCounter = 120;
                }
            }
        }
    }
}

