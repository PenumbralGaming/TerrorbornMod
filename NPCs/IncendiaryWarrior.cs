using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.NPCs
{
    class IncendiaryWarrior : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 4;
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.width = 58;
            npc.height = 272 / 4;
            npc.damage = 65;
            npc.defense = 12;
            npc.lifeMax = 375;
            npc.HitSound = SoundID.NPCHit41;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.value = 250;
            npc.knockBackResist = 0.1f;
            npc.aiStyle = 22;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Ichor] = true;

            //aiType = NPCID.WalkingAntlion;
        }
        int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            npc.frameCounter--;
            if (npc.frameCounter <= 0)
            {
                frame++;
                npc.frameCounter = 4;
            }
            if (frame >= 3)
            {
                frame = 0;
            }
            npc.frame.Y = frame * frameHeight;
        }
        public override void PostAI()
        {
            if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.spriteDirection = -1;
            }
            else
            {
                npc.spriteDirection = 1;
            }
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);
            Lighting.AddLight(npc.Center, 214 / 200, 114 / 200, 80 / 200);
        }
        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Materials.IncendiusAlloy>(), Main.rand.Next(8, 13));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Cavern.Chance * .07f;
            }
            else
            {
                return SpawnCondition.Underground.Chance * 0f;
            }
        }
    }
}

