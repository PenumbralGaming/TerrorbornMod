using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.NPCs
{
    class Sangoon : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 5;
            DisplayName.SetDefault("Sangoon");
        }
        public override void SetDefaults()
        {
            npc.noGravity = true;
            npc.noTileCollide = false;
            npc.width = 46;
            npc.height = 30;
            npc.damage = 50;
            npc.defense = 7;
            npc.lifeMax = 75;
            npc.HitSound = SoundID.NPCHit18;
            npc.DeathSound = SoundID.NPCDeath32;
            npc.value = 250;
            npc.knockBackResist = 0f;
            npc.aiStyle = 14;
            aiType = NPCID.CaveBat;
            if (Main.hardMode)
            {
                npc.lifeMax = 285;
                npc.damage = 80;
                npc.defense = 17;
            }
        }
        int Frame = 0;
        int FrameWait = 4;
        public override void FindFrame(int frameHeight)
        {
            FrameWait--;
            if (FrameWait <= 0)
            {
                FrameWait = 6;
                Frame++;
                if (Frame >= 5)
                {
                    Frame = 0;
                }
            }
            npc.frame.Y = Frame * frameHeight;
            if (Main.player[npc.target].Center.X > npc.Center.X)
            {
                npc.spriteDirection = 1;
            }
            if (Main.player[npc.target].Center.X < npc.Center.X)
            {
                npc.spriteDirection = -1;
            }
            npc.rotation = MathHelper.ToRadians(npc.velocity.X * 2);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedBoss1 && Main.bloodMoon)
            {
                if (NPC.AnyNPCs(mod.NPCType("Sangrune")))
                {
                    return SpawnCondition.OverworldNightMonster.Chance * 0.5f;
                }
                return SpawnCondition.OverworldNightMonster.Chance * 0.25f;
            }
            else
            {
                return SpawnCondition.OverworldNightMonster.Chance * 0;
            }
        }
        public override void NPCLoot()
        {
            if (Main.rand.Next(5) == 0)
            {
                Item.NewItem(npc.position, npc.width, npc.height, ItemID.Heart);
            }
            if (NPC.downedBoss2 && Main.rand.Next(30) == 0)
            {
                Item.NewItem(npc.position, npc.width, npc.height, mod.ItemType("SanguineFang"));
            }
        }
    }
}
