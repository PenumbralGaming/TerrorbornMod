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
using Terraria.UI;

namespace TerrorbornMod
{
	public abstract class BaseWorm : ModNPC
    {
        public override bool CheckActive()
        {
            return !(body || tail);
        }

        public bool head;
		public bool body;
		public bool tail;

		public int headType;
		public int bodyType;
		public int tailType;

		public int bodySegmentCount;

		List<int> bodySegments = new List<int>();

		public bool touchingTiles;

        public override void AI()
		{
			touchingTiles = Collision.SolidCollision(npc.position, npc.width, npc.height);
			actualAI();

			if (head)
			{
				npc.rotation = npc.velocity.ToRotation() + MathHelper.ToRadians(90f);
				bool hasSegments = false;
				for (int i = 0; i < Main.npc.Length; i++)
				{
					bool isPart = false;
					NPC thingy = Main.npc[i];
					if (thingy.type == bodyType && bodySegments.Contains(thingy.whoAmI) && thingy.ai[0] == npc.whoAmI && thingy.active)
					{
						isPart = true;
						thingy.velocity = Vector2.Zero;
						int index = bodySegments.IndexOf(thingy.whoAmI);
						NPC latchedNPC = Main.npc[0];
						if (index == 0)
                        {
							latchedNPC = npc;
                        }
                        else
                        {
							latchedNPC = Main.npc[bodySegments[index - 1]];
                        }
                        if (latchedNPC == npc)
						{
							thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2) + npc.velocity.Length()) * thingy.DirectionTo(latchedNPC.Center);
						}
                        else
                        {
							thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2)) * thingy.DirectionTo(latchedNPC.Center);
						}
                        thingy.rotation = thingy.DirectionTo(latchedNPC.Center).ToRotation() + MathHelper.ToRadians(90f);
						thingy.life = npc.life;
					}

					if (thingy.type == tailType && thingy.ai[0] == npc.whoAmI && thingy.active)
					{
						isPart = true;
						hasSegments = true;
						thingy.velocity = Vector2.Zero;
						int index = bodySegments.IndexOf(thingy.whoAmI);
						NPC latchedNPC = Main.npc[bodySegments[bodySegments.Count - 1]];
						thingy.position += (thingy.Distance(latchedNPC.Center) - (thingy.height / 2 + latchedNPC.height / 2)) * thingy.DirectionTo(latchedNPC.Center);
						thingy.rotation = thingy.DirectionTo(latchedNPC.Center).ToRotation() + MathHelper.ToRadians(90f);
						thingy.life = npc.life;
					}
				}

				if (!hasSegments)
                {
					for (int i = 0; i < bodySegmentCount; i++)
                    {
						int thingy = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, bodyType);
						Main.npc[thingy].ai[0] = npc.whoAmI;
						Main.npc[thingy].ai[1] = i;
						Main.npc[thingy].realLife = npc.whoAmI;
						Main.npc[thingy].dontCountMe = true;
						bodySegments.Add(thingy);
					}
					int tail = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, tailType);
					Main.npc[tail].ai[0] = npc.whoAmI;
					Main.npc[tail].ai[1] = bodySegmentCount;
					Main.npc[tail].realLife = npc.whoAmI;
					Main.npc[tail].dontCountMe = true;
				}
			}
            else
            {
				NPC headNPC = Main.npc[(int)npc.ai[0]];
				if (headNPC.type != headType || !headNPC.active)
                {
					npc.active = false;
				}
				TerrorbornNPC globalNPC = TerrorbornNPC.modNPC(npc);
				globalNPC.extraWormSegment = true;
			}
        }

        public virtual void actualAI()
        {

		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return head ? (bool?)null : false;
		}
	}
}
