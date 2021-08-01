using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.NPCs
{
    class IncendiaryWheel : ModNPC
    {
        public override void SetDefaults()
        {
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.width = 56;
            npc.height = 56;
            npc.damage = 65;
            npc.defense = 12;
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = 250;
            npc.knockBackResist = 0.1f;
            npc.aiStyle = 26;
            npc.lavaImmune = true;
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Confused] = true;
            npc.buffImmune[BuffID.Ichor] = true;

            //aiType = NPCID.WalkingAntlion;
        }
        public override void PostAI()
        {
            npc.rotation += MathHelper.ToRadians(npc.velocity.X * 2);
            Lighting.AddLight(npc.Center, 214 / 200, 114 / 200, 80 / 200);
        }
        public override void NPCLoot()
        {
            Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Items.Materials.IncendiusAlloy>(), Main.rand.Next(6, 9));
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.hardMode)
            {
                return SpawnCondition.Cavern.Chance * .2f;
            }
            else
            {
                return SpawnCondition.Underground.Chance * 0f;
            }
        }
    }
}
