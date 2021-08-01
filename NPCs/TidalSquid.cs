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
    class TidalSquid : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.width = 20;
            npc.height = 40;
            npc.damage = 45;
            npc.defense = 3;
            npc.lifeMax = 130;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath25;
            npc.knockBackResist = 0f;
        }
        
        float ChargeWait = 60;
        public override void AI()
        {
            npc.rotation = MathHelper.ToRadians(npc.velocity.X);
            npc.TargetClosest(false);
            Player targetPlayer = Main.player[npc.target];
            
            if (targetPlayer.wet || !npc.wet)
            {
                npc.velocity.Y += 0.1f;
            }
            else
            {
                npc.velocity.Y *= 0.8f;
            }

            if (npc.wet && targetPlayer.wet)
            {
                npc.noTileCollide = false;
                ChargeWait--;
                if (Collision.CanHit(npc.position, npc.width, npc.height, targetPlayer.position, targetPlayer.width, targetPlayer.height) && targetPlayer.position.Y < npc.position.Y && ChargeWait <= 0)
                {
                    ChargeWait = Main.rand.Next(15, 75);
                    npc.velocity.Y -= 10;
                    float XSpeed = 6;
                    if (targetPlayer.position.X < npc.position.X)
                    {
                        npc.velocity.X -= XSpeed;
                    }
                    else
                    {
                        npc.velocity.X += XSpeed;
                    }
                }
            }
            else
            {

            }
            npc.velocity.Y *= 0.98f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(9) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrackedShell"), 1);
            }
        }

        int frame = 0;
        float TrueVelocityX = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 6;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (TerrorbornWorld.downedTidalTitan && Main.raining)
            {
                return SpawnCondition.Ocean.Chance * 0.9f;
            }
            else
            {
                return 0f;
            }
        }
    }
}
