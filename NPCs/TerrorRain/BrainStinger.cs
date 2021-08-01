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
    class BrainStinger : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 10;
            NPCID.Sets.TrailCacheLength[npc.type] = 1;
            NPCID.Sets.TrailingMode[npc.type] = 1;
        }

        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.width = 42;
            npc.height = 48;
            npc.damage = 65;
            npc.defense = 15;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath28;
            npc.value = 250;
            npc.knockBackResist = 0.25f;
            npc.lavaImmune = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornWorld.terrorRain && Main.raining && spawnInfo.player.ZoneRain)
            {
                return 0.55f;
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

            if (Main.rand.NextFloat() <= 0.5f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.ThunderShard>());
            }

            if (Main.rand.NextFloat() <= 0.065f)
            {
                Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Weapons.Summons.Minions.AnglerStaff>());
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
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/NPCs/TerrorRain/BrainStinger_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
        }

        bool pushingUp = false;
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            if (pushingUp)
            {
                if (frame < 5)
                {
                    frame = 5;
                }
                if (frame < 8)
                {
                    npc.frameCounter--;
                    if (npc.frameCounter <= 0)
                    {
                        npc.frameCounter = 4;
                        frame++;
                    }
                }
            }
            else
            {
                if (frame != 1)
                {
                    npc.frameCounter--;
                    if (npc.frameCounter <= 0)
                    {
                        npc.frameCounter = 10;
                        frame++;
                        if (frame >= 10)
                        {
                            frame = 0;
                        }
                    }
                }
            }
            npc.frame.Y = frame * frameHeight;
        }
        public override void AI()
        {
            npc.TargetClosest(false);
            Player player = Main.player[npc.target];
            if (pushingUp)
            {
                float speed = 0.5f;
                npc.velocity.X += speed * npc.ai[1];
                npc.velocity.Y -= speed;
                npc.velocity *= 0.95f;

                npc.ai[0]--;
                if (npc.ai[0] <= 0)
                {
                    pushingUp = false;
                    npc.ai[0] = 60;
                }
            }
            else
            {
                npc.velocity.Y += 0.3f;
                npc.velocity *= 0.93f;

                npc.ai[0]--;
                if (npc.ai[0] <= 0 && player.Center.Y < npc.Center.Y)
                {
                    pushingUp = true;
                    npc.ai[0] = 45;
                    npc.ai[1] = -1;
                    if (player.Center.X > npc.Center.X)
                    {
                        npc.ai[1] = 1;
                    }
                }
            }
        }
    }
}
