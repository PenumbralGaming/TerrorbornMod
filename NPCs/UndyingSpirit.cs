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
    class UndyingSpirit : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 6;
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 48;
            npc.height = 42;
            npc.damage = 45;
            npc.defense = 6;
            npc.lifeMax = 1500;
            npc.HitSound = SoundID.NPCHit36;
            npc.DeathSound = SoundID.NPCDeath39;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofNight, Main.rand.Next(16, 23));
            TerrorbornWorld.downedUndyingSpirit = true;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = new SpriteEffects();
            effects = SpriteEffects.FlipHorizontally;
            Vector2 drawOrigin = new Vector2(npc.width / 2, npc.height / 2);
            for (int i = 0; i < npc.oldPos.Length; i++)
            {
                Vector2 drawPos = npc.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY) + new Vector2(0, 4);
                Color color = npc.GetAlpha(Color.White) * ((float)(npc.oldPos.Length - i) / (float)npc.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/UndyingSpirit_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }
        int frame = 0;
        Vector2 velocity = Vector2.Zero;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 3;
            }
            if (frame >= 6)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode && !NPC.AnyNPCs(npc.type))
            {
                float multiplier = 0.0075f;
                if (WorldGen.crimson)
                {
                    return SpawnCondition.Crimson.Chance * multiplier;
                }
                else
                {
                    return SpawnCondition.Corruption.Chance * multiplier;
                }
            }
            else
            {
                return 0f;
            }
        }

        float offsetDirection = 0f;
        bool angered = false;
        public override void AI()
        {
            npc.spriteDirection = 1;
            Player player = Main.player[npc.target];
            npc.rotation = npc.DirectionTo(player.Center).ToRotation();

            if (!angered && npc.life < npc.lifeMax)
            {
                angered = true;
                offsetDirection = npc.DirectionFrom(player.Center).ToRotation();
                CombatText.NewText(npc.getRect(), Color.PaleVioletRed, "!!!", true);
            }

            offsetDirection += MathHelper.ToRadians(5);

            if (angered)
            {
                int offset = 500;
                moveTowardsPosition(0.85f, 0.975f, offsetDirection.ToRotationVector2() * offset + player.Center);
            }
            else
            {
                int offset = 750;
                moveTowardsPosition(0.3f, 0.97f, offsetDirection.ToRotationVector2() * offset + player.Center);
            }

            npc.velocity = velocity;
        }

        public void moveTowardsPosition(float speed, float velocityMultiplier, Vector2 position)
        {
            Vector2 direction = npc.DirectionTo(position);
            velocity += speed * direction;
            velocity *= velocityMultiplier;
        }
    }
}

