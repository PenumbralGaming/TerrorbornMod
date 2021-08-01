using System.IO;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.NPCs
{
    class TerrorSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 2;
        }
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BlueSlime);
            npc.width = 32;
            npc.height = 26;
            npc.damage = 13;
            npc.defense = 2;
            npc.lifeMax = 35;
            npc.value *= 2;
            npc.knockBackResist = 0f;
            npc.lavaImmune = true;
            npc.color = Color.White;
            animationType = NPCID.BlueSlime;
            if (Main.hardMode)
            {
                npc.defense = 5;
                npc.lifeMax = 160;
                npc.damage = 34;
            }
            if (NPC.downedPlantBoss)
            {
                npc.defense = 8;
                npc.lifeMax = 190;
                npc.damage = 38;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem(npc.getRect(), ItemID.Gel, Main.rand.Next(1, 3));
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.DarkEnergy>());

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
            if (modPlayer.DeimosteelCharm)
            {
                if (Main.rand.NextFloat() <= 0.5f)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
            else
            {
                if (Main.rand.NextFloat() <= 0.25f)
                {
                    Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>());
                }
            }
        }
        //int frame = 0;
        //public override void FindFrame(int frameHeight)
        //{
        //    if (npc.velocity.Y == 0)
        //    {
        //        npc.frameCounter--;
        //        if (npc.frameCounter <= 0)
        //        {
        //            frame++;
        //            npc.frameCounter = 30;
        //        }
        //        if (frame >= 2)
        //        {
        //            frame = 0;
        //        }
        //    }
        //    else
        //    {
        //        frame = 1;
        //    }
        //    npc.frame.Y = frame * frameHeight;
        //}
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!TerrorbornWorld.obtainedShriekOfHorror)
            {
                return 0f;
            }
            return SpawnCondition.OverworldDaySlime.Chance * 0.35f;
        }
    }
}

