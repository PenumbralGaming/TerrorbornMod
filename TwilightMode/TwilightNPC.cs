﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using System;
using TerrorbornMod.UI.TitleCard;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.TwilightMode
{
	public class TwilightNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public bool twilight = false;
		bool start = true;

		public static TwilightNPC modNPC(NPC npc)
		{
			if (npc != null)
			{
				return npc.GetGlobalNPC<TwilightNPC>();
			}
			else
			{
				return null;
			}
		}

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
		{
			if (npc.type == NPCID.CultistBossClone)
			{
				NPC npc2 = Main.npc[(int)npc.ai[3]];
				if (modNPC(npc2).twilight)
				{
					int healAmount = npc2.lifeMax / 15 + ((npc2.lifeMax - npc2.life) / 10);
					npc2.HealEffect(healAmount);
					npc2.life += healAmount;
					if (npc2.life >= npc2.lifeMax)
					{
						npc2.life = npc2.lifeMax;
					}
				}
			}
		}

        public override void SetDefaults(NPC npc)
        {
			if (!TerrorbornWorld.TwilightMode || npc.friendly || npc.lifeMax >= int.MaxValue / 2)
            {
				return;
            }

			if (npc.boss)
			{
				npc.lifeMax = (int)(npc.lifeMax * 1.5f);
				npc.value *= 1.3f;
			}
            else
			{
				npc.lifeMax = (int)(npc.lifeMax * 1.35f);
				npc.knockBackResist *= 0.5f;
				npc.value *= 2f;
			}
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
			if (TerrorbornWorld.TwilightMode)
            {
				spawnRate = (int)(spawnRate * 0.75f);
				maxSpawns = (int)(maxSpawns * 1.3f);
			}
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
		{
			if (npc.type == NPCID.CultistBossClone)
			{
				NPC npc2 = Main.npc[(int)npc.ai[3]];
				if (modNPC(npc2).twilight)
				{
					int healAmount = npc2.lifeMax / 15 + ((npc2.lifeMax - npc2.life) / 10);
					npc2.HealEffect(healAmount);
					npc2.life += healAmount;
					if (npc2.life >= npc2.lifeMax)
					{
						npc2.life = npc2.lifeMax;
					}
				}
			}
		}

		bool postAIStart = true;
		bool ballStoppedHoming = false;
        public override void PostAI(NPC npc)
        {
			if (!TerrorbornWorld.TwilightMode)
            {
				return;
            }

			if (postAIStart)
            {
				postAIStart = false;
				ballStoppedHoming = false;
			}

			if (npc.aiStyle == 9)
            {
				if (!ballStoppedHoming)
                {
					npc.velocity = npc.DirectionTo(Main.LocalPlayer.Center) * (npc.velocity.Length() + 0.07f);
					if (npc.Distance(Main.LocalPlayer.Center) <= 200f)
                    {
						ballStoppedHoming = true;
                    }
                }
            }
        }

        public override bool PreAI(NPC npc)
		{
			if (start)
			{
				twilight = TerrorbornWorld.TwilightMode;
			}
			if (twilight)
			{
				if (npc.aiStyle == 4)
				{
					EyeOfCthulhuAI(npc);
					return false;
				}
				if (npc.aiStyle == 15)
				{
					KingSlimeAI(npc);
					return false;
				}
				if (npc.aiStyle == 43)
				{
					QueenBeeAI(npc);
					return false;
				}
				if (npc.aiStyle == 84)
				{
					LunaticCultistAI(npc);
					return false;
				}
				if (npc.aiStyle == 54)
				{
					BrainOfCthulhuAI(npc);
					return false;
				}
				if (npc.aiStyle == 27)
				{
					WoFMouthAI(npc);
					return false;
				}
				if (npc.aiStyle == 28)
				{
					WoFEyeAI(npc);
					return false;
				}
			}
			start = false;
			return base.PreAI(npc);
		}

		int EoCFlameDir = 0;
		public void SetEoCFlamethrowerDirection(NPC npc)
        {
			if (Main.player[npc.target].Center.X > npc.Center.X) EoCFlameDir = 1;
			else EoCFlameDir = -1;

			if (Main.player[npc.target].Center.Y < npc.Center.Y + 100) EoCFlameDir *= -1;

		}

		bool EoCAutoRotate = true;
		int EoCFlamethrowerWait = 0;
		int EoCAttackCounter = 0;
		bool EoCThirdPhaseJustStarted = true;
		public void EyeOfCthulhuAI(NPC npc)
		{
			bool flag2 = false;
			if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.12)
			{
				flag2 = true;
			}
			bool flag3 = false;
			if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.04)
			{
				flag3 = true;
			}
			float num4 = 20f;
			if (flag3)
			{
				num4 = 10f;
			}
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest();
			}
			bool dead = Main.player[npc.target].dead;
			float num5 = npc.position.X + (float)(npc.width / 2) - Main.player[npc.target].position.X - (float)(Main.player[npc.target].width / 2);
			float num6 = npc.position.Y + (float)npc.height - 59f - Main.player[npc.target].position.Y - (float)(Main.player[npc.target].height / 2);
			float num7 = (float)Math.Atan2(num6, num5) + 1.57f;
			if (num7 < 0f)
			{
				num7 += 6.283f;
			}
			else if ((double)num7 > 6.283)
			{
				num7 -= 6.283f;
			}
			float num8 = 0f;
			if (npc.ai[0] == 0f && npc.ai[1] == 0f)
			{
				num8 = 0.02f;
			}
			if (npc.ai[0] == 0f && npc.ai[1] == 2f && npc.ai[2] > 40f)
			{
				num8 = 0.05f;
			}
			if (npc.ai[0] == 3f && npc.ai[1] == 0f)
			{
				num8 = 0.05f;
			}
			if (npc.ai[0] == 3f && npc.ai[1] == 2f && npc.ai[2] > 40f)
			{
				num8 = 0.08f;
			}
			if (npc.ai[0] == 3f && npc.ai[1] == 4f && npc.ai[2] > num4)
			{
				num8 = 0.15f;
			}
			if (npc.ai[0] == 3f && npc.ai[1] == 5f)
			{
				num8 = 0.05f;
			}
			if (Main.expertMode)
			{
				num8 *= 1.5f;
			}
			if (flag3 && Main.expertMode)
			{
				num8 = 0f;
			}
			if (EoCAutoRotate)
			{
				if (npc.rotation < num7)
				{
					if ((double)(num7 - npc.rotation) > 3.1415)
					{
						npc.rotation -= num8;
					}
					else
					{
						npc.rotation += num8;
					}
				}
				else if (npc.rotation > num7)
				{
					if ((double)(npc.rotation - num7) > 3.1415)
					{
						npc.rotation += num8;
					}
					else
					{
						npc.rotation -= num8;
					}
				}
				if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				{
					npc.rotation = num7;
				}
				if (npc.rotation < 0f)
				{
					npc.rotation += 6.283f;
				}
				else if ((double)npc.rotation > 6.283)
				{
					npc.rotation -= 6.283f;
				}
				if (npc.rotation > num7 - num8 && npc.rotation < num7 + num8)
				{
					npc.rotation = num7;
				}
			}
			if (Main.rand.Next(5) == 0)
			{
				int num9 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y + (float)npc.height * 0.25f), npc.width, (int)((float)npc.height * 0.5f), 5, npc.velocity.X, 2f);
				Main.dust[num9].velocity.X *= 0.5f;
				Main.dust[num9].velocity.Y *= 0.1f;
			}
			if (Main.dayTime || dead)
			{
				npc.velocity.Y -= 0.04f;
				if (npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
				return;
			}
			if (npc.life <= npc.lifeMax * 0.33f && npc.ai[0] > 1f)
            {
				Player player = Main.player[npc.target];
				if (EoCThirdPhaseJustStarted)
                {
					EoCThirdPhaseJustStarted = false;
					EoCAttackCounter = -30;
					EoCAutoRotate = false;
					npc.ai[1] = 3;
					npc.damage = (int)(npc.damage * 1.5f);
				}
				else if (npc.ai[1] == 0)
				{
					npc.velocity *= 0.9f;
					if (EoCFlamethrowerWait > 0)
					{
						npc.rotation += MathHelper.ToRadians(7f) * EoCFlameDir;
						EoCFlamethrowerWait--;
						if (EoCFlamethrowerWait <= 0)
                        {
							Main.PlaySound(SoundID.NPCDeath10, npc.Center);
						}
					}
					else
					{
						npc.rotation += MathHelper.ToRadians(MathHelper.Lerp(1.1f, 0.8f, (float)npc.life / (float)npc.lifeMax / 0.33f)) * EoCFlameDir;
						EoCAttackCounter++;
						if (EoCAttackCounter % 3 == 2)
						{
							Main.PlaySound(SoundID.Item34, npc.Center);
							Projectile.NewProjectile(npc.Center, npc.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90f)) * 24f, ModContent.ProjectileType<EoCFlameThrower>(), 60 / 4, 0f);
                        }
						TerrorbornMod.ScreenShake(1f);
						if (EoCAttackCounter > 120)
                        {
							npc.ai[1] = 1;
							EoCAttackCounter = 0;
							EoCFlameDir = -Math.Sign(npc.Center.X - player.Center.X);
						}
					}
				}
				else if (npc.ai[1] == 1)
				{
					npc.rotation = npc.rotation.AngleTowards(num7, MathHelper.ToRadians(10f));
					npc.velocity += npc.DirectionTo(player.Center + new Vector2(500 * EoCFlameDir, -100f)) * 2f;
					npc.velocity *= 0.93f;
					EoCAttackCounter++;
					if (EoCAttackCounter % 30 == 29)
					{
						int num22 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(25f, 40f, (float)npc.life / (float)npc.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						Main.PlaySound(SoundID.NPCDeath13, npc.Center);
					}
					if (EoCAttackCounter > 90)
                    {
						npc.velocity = npc.DirectionTo(player.Center) * 25f;
						npc.rotation = npc.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(90f);
						EoCAutoRotate = false;
						EoCAttackCounter = 0;
						Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, -1);
						npc.ai[1] = 2;
					}
                }
				else if (npc.ai[1] == 2)
                {
					EoCAttackCounter++;
					if (EoCAttackCounter == 29)
					{
						int num22 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
						Main.npc[num22].lifeMax = (int)MathHelper.Lerp(35f, 55f, (float)npc.life / (float)npc.lifeMax / 0.33f);
						Main.npc[num22].life = Main.npc[num22].lifeMax;
						Main.PlaySound(SoundID.NPCDeath13, npc.Center);
					}
					if (EoCAttackCounter > 65)
                    {
						EoCAttackCounter = 0;
						EoCAutoRotate = true;
						npc.ai[1] = 3;
                    }

				}
				else if (npc.ai[1] == 3)
				{
					npc.rotation = npc.rotation.AngleTowards(num7, MathHelper.ToRadians(5f));
					npc.velocity += npc.DirectionTo(player.Center + new Vector2(600 * Math.Sign(npc.Center.X - player.Center.X), -300)) * 2f;
					npc.velocity *= 0.93f;
					EoCAttackCounter++;
					if (EoCAttackCounter > 90)
					{
						EoCFlamethrowerWait = 45;
						EoCAttackCounter = 0;
						SetEoCFlamethrowerDirection(npc);
						EoCAutoRotate = false;
						npc.rotation = npc.DirectionTo(player.Center).ToRotation() - MathHelper.ToRadians(90f);
						npc.ai[1] = 0;
					}
				}
				return;
            }
			if (npc.ai[0] == 0f)
			{
				if (npc.ai[1] == 0f) //Hovering
				{
					float num10 = 22.5f; //hover speed (phase one)
					float num11 = 0.1f;
					Vector2 vector = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num12 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector.X;
					float num13 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector.Y;
					float num14 = (float)Math.Sqrt(num12 * num12 + num13 * num13);
					float num15 = num14;
					num14 = num10 / num14;
					num12 *= num14;
					num13 *= num14;
					if (npc.velocity.X < num12)
					{
						npc.velocity.X += num11;
						if (npc.velocity.X < 0f && num12 > 0f)
						{
							npc.velocity.X += num11;
						}
					}
					else if (npc.velocity.X > num12)
					{
						npc.velocity.X -= num11;
						if (npc.velocity.X > 0f && num12 < 0f)
						{
							npc.velocity.X -= num11;
						}
					}
					if (npc.velocity.Y < num13)
					{
						npc.velocity.Y += num11;
						if (npc.velocity.Y < 0f && num13 > 0f)
						{
							npc.velocity.Y += num11;
						}
					}
					else if (npc.velocity.Y > num13)
					{
						npc.velocity.Y -= num11;
						if (npc.velocity.Y > 0f && num13 < 0f)
						{
							npc.velocity.Y -= num11;
						}
					}
					npc.ai[2] += 1f;
					float num16 = 600f;
					if (Main.expertMode)
					{
						num16 *= 0.35f;
					}
					if (npc.ai[2] >= num16)
					{
						npc.ai[1] = 1f;
						npc.ai[2] = 0f;
						npc.ai[3] = 0f;
						npc.target = 255;
					}
					else if ((npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y && num15 < 500f) || (Main.expertMode && num15 < 500f))
					{
						if (!Main.player[npc.target].dead)
						{
							npc.ai[3] += 1f;
						}
						float num17 = 110f;
						if (Main.expertMode)
						{
							num17 *= 0.4f;
						}
						if (npc.ai[3] >= num17)
						{
							npc.ai[3] = 0f;
							npc.rotation = num7;
							float num18 = 5f;
							if (Main.expertMode)
							{
								num18 = 6f;
							}
							float num19 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector.X;
							float num20 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector.Y;
							float num21 = (float)Math.Sqrt(num19 * num19 + num20 * num20);
							num21 = num18 / num21;
							Vector2 vector2 = vector;
							Vector2 vector3 = default(Vector2);
							vector3.X = num19 * num21;
							vector3.Y = num20 * num21;
							vector2.X += vector3.X * 10f;
							vector2.Y += vector3.Y * 10f;
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								int num22 = NPC.NewNPC((int)vector2.X, (int)vector2.Y, 5);
								Main.npc[num22].velocity.X = vector3.X;
								Main.npc[num22].velocity.Y = vector3.Y;
								if (Main.netMode == NetmodeID.Server && num22 < 200)
								{
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num22);
								}
							}
							Main.PlaySound(SoundID.NPCHit, (int)vector2.X, (int)vector2.Y);
							for (int m = 0; m < 10; m++)
							{
								Dust.NewDust(vector2, 20, 20, 5, vector3.X * 0.4f, vector3.Y * 0.4f);
							}
						}
					}
				}
				else if (npc.ai[1] == 1f) //Start dash
				{
					npc.rotation = num7;
					float num23 = 8f; //charge speed (phase one)
					Vector2 vector4 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num24 = Main.player[npc.target].Center.X - vector4.X + Main.player[npc.target].velocity.X * (npc.Distance(Main.player[npc.target].Center) / num23);
					float num25 = Main.player[npc.target].Center.Y - vector4.Y + Main.player[npc.target].velocity.Y * (npc.Distance(Main.player[npc.target].Center) / num23);
					float num26 = (float)Math.Sqrt(num24 * num24 + num25 * num25);
					num26 = num23 / num26;
					npc.velocity.X = num24 * num26;
					npc.velocity.Y = num25 * num26;
					npc.ai[1] = 2f;
				}
				else if (npc.ai[1] == 2f) //Mid dash
				{
					npc.ai[2] += 1f;
					if (npc.ai[2] >= 40f)
					{
						npc.velocity *= 0.98f;
						if (Main.expertMode)
						{
							npc.velocity *= 0.985f;
						}
						if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
						{
							npc.velocity.X = 0f;
						}
						if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
						{
							npc.velocity.Y = 0f;
						}
					}
					else
					{
						npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
					}
					int num27 = 150;
					if (Main.expertMode)
					{
						num27 = 100;
					}
					if (npc.ai[2] >= (float)num27)
					{
						npc.ai[3] += 1f;
						npc.ai[2] = 0f;
						npc.target = 255;
						npc.rotation = num7;
						if (npc.ai[3] >= 3f)
						{
							npc.ai[1] = 0f;
							npc.ai[3] = 0f;
						}
						else
						{
							npc.ai[1] = 1f;
						}
					}
				}
				float num28 = 0.66f;
				if ((float)npc.life < (float)npc.lifeMax * num28)
				{
					npc.ai[0] = 1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
				}
				return;
			}
			if (npc.ai[0] == 1f || npc.ai[0] == 2f)
			{
				if (npc.ai[0] == 1f)
				{
					npc.ai[2] += 0.005f;
					if ((double)npc.ai[2] > 0.5)
					{
						npc.ai[2] = 0.5f;
					}
				}
				else
				{
					npc.ai[2] -= 0.005f;
					if (npc.ai[2] < 0f)
					{
						npc.ai[2] = 0f;
					}
				}
				npc.rotation += npc.ai[2];
				npc.ai[1] += 1f;
				if (Main.expertMode && npc.ai[1] % 20f == 0f)
				{
					float num29 = 5f;
					Vector2 vector5 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					float num30 = Main.rand.Next(-200, 200);
					float num31 = Main.rand.Next(-200, 200);
					float num32 = (float)Math.Sqrt(num30 * num30 + num31 * num31);
					num32 = num29 / num32;
					Vector2 vector6 = vector5;
					Vector2 vector7 = default(Vector2);
					vector7.X = num30 * num32;
					vector7.Y = num31 * num32;
					vector6.X += vector7.X * 10f;
					vector6.Y += vector7.Y * 10f;
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num33 = NPC.NewNPC((int)vector6.X, (int)vector6.Y, 5);
						Main.npc[num33].velocity.X = vector7.X;
						Main.npc[num33].velocity.Y = vector7.Y;
						if (Main.netMode == NetmodeID.Server && num33 < 200)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num33);
						}
					}
					for (int n = 0; n < 10; n++)
					{
						Dust.NewDust(vector6, 20, 20, 5, vector7.X * 0.4f, vector7.Y * 0.4f);
					}
				}
				if (npc.ai[1] == 100f)
				{
					npc.ai[0] += 1f;
					npc.ai[1] = 0f;
					if (npc.ai[0] == 3f)
					{
						npc.ai[2] = 0f;
					}
					else
					{
						Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y);
						for (int num34 = 0; num34 < 2; num34++)
						{
							Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 8);
							Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 7);
							Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 6);
						}
						for (int num35 = 0; num35 < 20; num35++)
						{
							Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
						}
						Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
					}
				}
				Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
				npc.velocity.X *= 0.98f;
				npc.velocity.Y *= 0.98f;
				if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
				{
					npc.velocity.X = 0f;
				}
				if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
				{
					npc.velocity.Y = 0f;
				}
				return;
			}
			npc.defense = 0;
			npc.damage = 23;
			if (Main.expertMode)
			{
				if (flag2)
				{
					npc.defense = -15;
				}
				if (flag3)
				{
					npc.damage = (int)(20f * Main.expertDamage);
					npc.defense = -30;
				}
				else
				{
					npc.damage = (int)(18f * Main.expertDamage);
				}
			}
			if (npc.ai[1] == 0f && flag2)
			{
				npc.ai[1] = 5f;
			}
			if (npc.ai[1] == 0f) //Hovering
			{
				float num36 = 12f; //hover speed (phase two)
				float num37 = 0.07f;
				Vector2 vector8 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				int direction = 1;
				if (Main.player[npc.target].Center.X >= npc.Center.X)
				{
					direction = -1;
				}
				float num38 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) + (120f * direction) - vector8.X;
				float num39 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector8.Y;
				float num40 = (float)Math.Sqrt(num38 * num38 + num39 * num39);
				if (num40 > 400f)
				{
					num36 += 1f;
					num37 += 0.05f;
					if (num40 > 600f)
					{
						num36 += 1f;
						num37 += 0.05f;
						if (num40 > 800f)
						{
							num36 += 1f;
							num37 += 0.05f;
						}
					}
				}
				num40 = num36 / num40;
				num38 *= num40;
				num39 *= num40;
				if (npc.velocity.X < num38)
				{
					npc.velocity.X += num37;
					if (npc.velocity.X < 0f && num38 > 0f)
					{
						npc.velocity.X += num37;
					}
				}
				else if (npc.velocity.X > num38)
				{
					npc.velocity.X -= num37;
					if (npc.velocity.X > 0f && num38 < 0f)
					{
						npc.velocity.X -= num37;
					}
				}
				if (npc.velocity.Y < num39)
				{
					npc.velocity.Y += num37;
					if (npc.velocity.Y < 0f && num39 > 0f)
					{
						npc.velocity.Y += num37;
					}
				}
				else if (npc.velocity.Y > num39)
				{
					npc.velocity.Y -= num37;
					if (npc.velocity.Y > 0f && num39 < 0f)
					{
						npc.velocity.Y -= num37;
					}
				}
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 200f)
				{
					npc.ai[1] = 1f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
					if (Main.expertMode && (double)npc.life < (double)npc.lifeMax * 0.35)
					{
						npc.ai[1] = 3f;
					}
					npc.target = 255;
				}
				if (Main.expertMode && flag3)
				{
					npc.TargetClosest();
					npc.ai[1] = 3f;
					npc.ai[2] = 0f;
					npc.ai[3] -= 1000f;
				}
			}
			else if (npc.ai[1] == 1f) //Frame of dash
			{
				Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, 0);
				npc.rotation = num7;
				float num41 = 11f; //charge speed (phase two)
				if (npc.ai[3] == 1f)
				{
					num41 *= 1.15f;
				}
				if (npc.ai[3] == 2f)
				{
					num41 *= 1.3f;
				}
				Vector2 vector9 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num42 = Main.player[npc.target].Center.X - vector9.X + Main.player[npc.target].velocity.X * (npc.Distance(Main.player[npc.target].Center) / num41);
				float num43 = Main.player[npc.target].Center.Y - vector9.Y + Main.player[npc.target].velocity.Y * (npc.Distance(Main.player[npc.target].Center) / num41);
				float num44 = (float)Math.Sqrt(num42 * num42 + num43 * num43);
				num44 = num41 / num44;
				npc.velocity.X = num42 * num44;
				npc.velocity.Y = num43 * num44;
				npc.ai[1] = 2f;
			}
			else if (npc.ai[1] == 2f) //Mid normal dash
			{
				float num45 = 40f;
				npc.ai[2] += 1f;
				if (Main.expertMode)
				{
					num45 = 50f;
				}
				if (npc.ai[2] >= num45)
				{
					npc.velocity *= 0.97f;
					if (Main.expertMode)
					{
						npc.velocity *= 0.98f;
					}
					if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
					{
						npc.velocity.X = 0f;
					}
					if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
					{
						npc.velocity.Y = 0f;
					}
				}
				else
				{
					npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
				}
				int num46 = 130;
				if (Main.expertMode)
				{
					num46 = 90;
				}
				if (npc.ai[2] >= (float)num46)
				{
					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					npc.target = 255;
					npc.rotation = num7;
					if (npc.ai[3] >= 3f)
					{
						npc.ai[1] = 0f;
						npc.ai[3] = 0f;
						if (Main.expertMode && Main.netMode != NetmodeID.MultiplayerClient && (double)npc.life < (double)npc.lifeMax * 0.5)
						{
							npc.ai[1] = 3f;
							npc.ai[3] += Main.rand.Next(1, 4);
						}
					}
					else
					{
						npc.ai[1] = 1f;
					}
				}
			}
			else if (npc.ai[1] == 3f) //Frame of fast dash
			{
				if (npc.ai[3] == 4f && flag2 && npc.Center.Y > Main.player[npc.target].Center.Y)
				{
					npc.TargetClosest();
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					npc.ai[3] = 0f;
				}
				else if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					//	npc.TargetClosest();
					//	float num47 = 30f;

					//	//int num22 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.ServantofCthulhu);
					//	//Main.PlaySound(SoundID.NPCDeath13, npc.Center);

					//	Vector2 vector10 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					//	float num48 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector10.X;
					//	float num49 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector10.Y;
					//	float num50 = Math.Abs(Main.player[npc.target].velocity.X) + Math.Abs(Main.player[npc.target].velocity.Y) / 4f;
					//	num50 += 10f - num50;
					//	if (num50 < 5f)
					//	{
					//		num50 = 5f;
					//	}
					//	if (num50 > 15f)
					//	{
					//		num50 = 15f;
					//	}
					//	if (npc.ai[2] == -1f && !flag3)
					//	{
					//		num50 *= 4f;
					//		num47 *= 1.3f;
					//	}
					//	if (flag3)
					//	{
					//		num50 *= 2f;
					//	}
					//	num48 -= Main.player[npc.target].velocity.X * num50;
					//	num49 -= Main.player[npc.target].velocity.Y * num50 / 4f;
					//	num48 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	num49 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	if (flag3)
					//	{
					//		num48 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//		num49 *= 1f + (float)Main.rand.Next(-10, 11) * 0.01f;
					//	}
					//	float num51 = (float)Math.Sqrt(num48 * num48 + num49 * num49);
					//	float num52 = num51;
					//	num51 = num47 / num51;
					//	npc.velocity.X = num48 * num51;
					//	npc.velocity.Y = num49 * num51;
					//	npc.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	npc.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	if (flag3)
					//	{
					//		npc.velocity.X += (float)Main.rand.Next(-50, 51) * 0.1f;
					//		npc.velocity.Y += (float)Main.rand.Next(-50, 51) * 0.1f;
					//		float num53 = Math.Abs(npc.velocity.X);
					//		float num54 = Math.Abs(npc.velocity.Y);
					//		if (npc.Center.X > Main.player[npc.target].Center.X)
					//		{
					//			num54 *= -1f;
					//		}
					//		if (npc.Center.Y > Main.player[npc.target].Center.Y)
					//		{
					//			num53 *= -1f;
					//		}
					//		npc.velocity.X = num54 + npc.velocity.X;
					//		npc.velocity.Y = num53 + npc.velocity.Y;
					//		npc.velocity.Normalize();
					//		npc.velocity *= num47;
					//		npc.velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
					//		npc.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.1f;
					//	}
					//	else if (num52 < 100f)
					//	{
					//		if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
					//		{
					//			float num55 = Math.Abs(npc.velocity.X);
					//			float num56 = Math.Abs(npc.velocity.Y);
					//			if (npc.Center.X > Main.player[npc.target].Center.X)
					//			{
					//				num56 *= -1f;
					//			}
					//			if (npc.Center.Y > Main.player[npc.target].Center.Y)
					//			{
					//				num55 *= -1f;
					//			}
					//			npc.velocity.X = num56;
					//			npc.velocity.Y = num55;
					//		}
					//	}
					//	else if (Math.Abs(npc.velocity.X) > Math.Abs(npc.velocity.Y))
					//	{
					//		float num57 = (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y)) / 2f;
					//		float num58 = num57;
					//		if (npc.Center.X > Main.player[npc.target].Center.X)
					//		{
					//			num58 *= -1f;
					//		}
					//		if (npc.Center.Y > Main.player[npc.target].Center.Y)
					//		{
					//			num57 *= -1f;
					//		}
					//		npc.velocity.X = num58;
					//		npc.velocity.Y = num57;
					//	}
					npc.velocity = npc.DirectionTo(Main.player[npc.target].Center) * 15f;
					npc.ai[1] = 4f;
				}
			}
			else if (npc.ai[1] == 4f) //Mid fast dash
			{
				if (npc.ai[2] == 0f)
				{
					Main.PlaySound(SoundID.ForceRoar, (int)npc.position.X, (int)npc.position.Y, -1);
				}
				float num59 = num4;
				npc.ai[2] += 1f;
				if (npc.ai[2] == num59 && Vector2.Distance(npc.position, Main.player[npc.target].position) < 200f)
				{
					npc.ai[2] -= 1f;
				}
				if (npc.ai[2] >= num59)
				{
					npc.velocity *= 0.95f;
					if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
					{
						npc.velocity.X = 0f;
					}
					if ((double)npc.velocity.Y > -0.1 && (double)npc.velocity.Y < 0.1)
					{
						npc.velocity.Y = 0f;
					}
				}
				else
				{
					npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) - 1.57f;
				}
				float num60 = num59 + 13f;
				if (npc.ai[2] >= num60)
				{
					npc.ai[3] += 1f;
					npc.ai[2] = 0f;
					if (npc.ai[3] >= 5f)
					{
						npc.ai[1] = 0f;
						npc.ai[3] = 0f;
					}
					else
					{
						npc.ai[1] = 3f;
					}
				}
			}
			else if (npc.ai[1] == 5f)
			{
				float num61 = 600f;
				float num62 = 9f;
				float num63 = 0.3f;
				Vector2 vector11 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num64 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector11.X;
				float num65 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) + num61 - vector11.Y;
				float num66 = (float)Math.Sqrt(num64 * num64 + num65 * num65);
				num66 = num62 / num66;
				num64 *= num66;
				num65 *= num66;
				if (npc.velocity.X < num64)
				{
					npc.velocity.X += num63;
					if (npc.velocity.X < 0f && num64 > 0f)
					{
						npc.velocity.X += num63;
					}
				}
				else if (npc.velocity.X > num64)
				{
					npc.velocity.X -= num63;
					if (npc.velocity.X > 0f && num64 < 0f)
					{
						npc.velocity.X -= num63;
					}
				}
				if (npc.velocity.Y < num65)
				{
					npc.velocity.Y += num63;
					if (npc.velocity.Y < 0f && num65 > 0f)
					{
						npc.velocity.Y += num63;
					}
				}
				else if (npc.velocity.Y > num65)
				{
					npc.velocity.Y -= num63;
					if (npc.velocity.Y > 0f && num65 < 0f)
					{
						npc.velocity.Y -= num63;
					}
				}
				npc.ai[2] += 1f;
				if (npc.ai[2] >= 70f)
				{
					npc.TargetClosest();
					npc.ai[1] = 3f;
					npc.ai[2] = -1f;
					npc.ai[3] = Main.rand.Next(-3, 1);
				}
			}
			if (flag3 && npc.ai[1] == 5f)
			{
				npc.ai[1] = 3f;
			}
		}

		int queenBeeLaserCounter = 0;
		public void QueenBeeAI(NPC npc)
		{
			int num598 = 0;
			for (int num599 = 0; num599 < 255; num599++)
			{
				if (Main.player[num599].active && !Main.player[num599].dead && (npc.Center - Main.player[num599].Center).Length() < 1000f)
				{
					num598++;
				}
			}
			if (Main.expertMode)
			{
				int num600 = (int)(20f * (1f - (float)npc.life / (float)npc.lifeMax));
				npc.defense = npc.defDefense + num600;
			}
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest();
			}
			if (Main.player[npc.target].dead && Main.expertMode)
			{
				if ((double)npc.position.Y < Main.worldSurface * 16.0 + 2000.0)
				{
					npc.velocity.Y += 0.04f;
				}
				if (npc.position.X < (float)(Main.maxTilesX * 8))
				{
					npc.velocity.X -= 0.04f;
				}
				else
				{
					npc.velocity.X += 0.04f;
				}
				if (npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
			}
			else if (npc.ai[0] == -1f)
			{
				if (Main.netMode == NetmodeID.MultiplayerClient)
				{
					return;
				}
				float num601 = npc.ai[1];
				int num602;
				do
				{
					num602 = Main.rand.Next(3);
					switch (num602)
					{
						case 1:
							num602 = 2;
							break;
						case 2:
							num602 = 3;
							break;
					}
				}
				while ((float)num602 == num601);
				npc.ai[0] = num602;
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
			}
			else if (npc.ai[0] == 0f)
			{
				int num603 = 2;
				if (Main.expertMode)
				{
					if (npc.life < npc.lifeMax / 2)
					{
						num603++;
					}
					if (npc.life < npc.lifeMax / 3)
					{
						num603++;
					}
					if (npc.life < npc.lifeMax / 5)
					{
						num603++;
					}
				}
				if (npc.ai[1] > (float)(2 * num603) && npc.ai[1] % 2f == 0f)
				{
					npc.ai[0] = -1f;
					npc.ai[1] = 0f;
					npc.ai[2] = 0f;
					return;
				}
				if (npc.ai[1] % 2f == 0f)
				{
					npc.TargetClosest();
					if (Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 20f)
					{
						npc.localAI[0] = 1f;
						npc.ai[1] += 1f;
						npc.ai[2] = 0f;
						float num604 = 22f;
						Vector2 vector76 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
						float num605 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector76.X;
						float num606 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector76.Y;
						float num607 = (float)Math.Sqrt(num605 * num605 + num606 * num606);
						num607 = num604 / num607;
						npc.velocity.X = num605 * num607;
						npc.velocity.Y = num606 * num607;
						npc.velocity.Y += Main.player[npc.target].velocity.Y;
						npc.spriteDirection = npc.direction;
						Main.PlaySound(SoundID.Roar, (int)npc.position.X, (int)npc.position.Y, 0);
						Main.PlaySound(SoundID.Item33, Main.player[npc.target].Center);
						Projectile proj = Main.projectile[Projectile.NewProjectile(Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X * 67f, 0) + new Vector2(0, 1000), new Vector2(0, -15 + Main.player[npc.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
						proj.tileCollide = false;
						proj.timeLeft = 300;
						proj = Main.projectile[Projectile.NewProjectile(Main.player[npc.target].Center + new Vector2(Main.player[npc.target].velocity.X * 67f, 0) + new Vector2(0, -1000), new Vector2(0, 15 + Main.player[npc.target].velocity.Y), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
						proj.tileCollide = false;
						proj.timeLeft = 300;
						return;
					}
					npc.localAI[0] = 0f;
					float num608 = 12f;
					float num609 = 0.15f;
					if (Main.expertMode)
					{
						if ((double)npc.life < (double)npc.lifeMax * 0.75)
						{
							num608 += 1f;
							num609 += 0.05f;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.5)
						{
							num608 += 1f;
							num609 += 0.05f;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.25)
						{
							num608 += 2f;
							num609 += 0.05f;
						}
						if ((double)npc.life < (double)npc.lifeMax * 0.1)
						{
							num608 += 2f;
							num609 += 0.1f;
						}
					}
					if (npc.position.Y + (float)(npc.height / 2) < Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))
					{
						npc.velocity.Y += num609;
					}
					else
					{
						npc.velocity.Y -= num609;
					}
					if (npc.velocity.Y < -12f)
					{
						npc.velocity.Y = 0f - num608;
					}
					if (npc.velocity.Y > 12f)
					{
						npc.velocity.Y = num608;
					}
					if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) > 600f)
					{
						npc.velocity.X += 0.15f * (float)npc.direction;
					}
					else if (Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 300f)
					{
						npc.velocity.X -= 0.15f * (float)npc.direction;
					}
					else
					{
						npc.velocity.X *= 0.8f;
					}
					if (npc.velocity.X < -20f)
					{
						npc.velocity.X = -20f;
					}
					if (npc.velocity.X > 20f)
					{
						npc.velocity.X = 20f;
					}
					npc.spriteDirection = npc.direction;
					return;
				}
				if (npc.velocity.X < 0f)
				{
					npc.direction = -1;
				}
				else
				{
					npc.direction = 1;
				}
				npc.spriteDirection = npc.direction;
				int num610 = 600;
				if (Main.expertMode)
				{
					if ((double)npc.life < (double)npc.lifeMax * 0.1)
					{
						num610 = 300;
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						num610 = 450;
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						num610 = 500;
					}
					else if ((double)npc.life < (double)npc.lifeMax * 0.75)
					{
						num610 = 550;
					}
				}
				int num611 = 1;
				if (npc.position.X + (float)(npc.width / 2) < Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))
				{
					num611 = -1;
				}
				if (npc.direction == num611 && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) > (float)num610)
				{
					npc.ai[2] = 1f;
				}
				if (npc.ai[2] == 1f)
				{
					npc.TargetClosest();
					npc.spriteDirection = npc.direction;
					npc.localAI[0] = 0f;
					npc.velocity *= 0.9f;
					float num612 = 0.1f;
					if (Main.expertMode)
					{
						if (npc.life < npc.lifeMax / 2)
						{
							npc.velocity *= 0.9f;
							num612 += 0.05f;
						}
						if (npc.life < npc.lifeMax / 3)
						{
							npc.velocity *= 0.9f;
							num612 += 0.05f;
						}
						if (npc.life < npc.lifeMax / 5)
						{
							npc.velocity *= 0.9f;
							num612 += 0.05f;
						}
					}
					if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < num612)
					{
						npc.ai[2] = 0f;
						npc.ai[1] += 1f;
					}
				}
				else
				{
					npc.localAI[0] = 1f;
				}
			}
			else if (npc.ai[0] == 2f)
			{
				npc.TargetClosest();
				npc.spriteDirection = npc.direction;
				float num613 = 12f;
				float num614 = 0.07f;
				if (Main.expertMode)
				{
					num614 = 0.1f;
				}
				Vector2 vector77 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num615 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector77.X;
				float num616 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 200f - vector77.Y;
				float num617 = (float)Math.Sqrt(num615 * num615 + num616 * num616);
				if (num617 < 200f)
				{
					npc.ai[0] = 1f;
					npc.ai[1] = 0f;
					npc.netUpdate = true;
					return;
				}
				num617 = num613 / num617;
				if (npc.velocity.X < num615)
				{
					npc.velocity.X += num614;
					if (npc.velocity.X < 0f && num615 > 0f)
					{
						npc.velocity.X += num614;
					}
				}
				else if (npc.velocity.X > num615)
				{
					npc.velocity.X -= num614;
					if (npc.velocity.X > 0f && num615 < 0f)
					{
						npc.velocity.X -= num614;
					}
				}
				if (npc.velocity.Y < num616)
				{
					npc.velocity.Y += num614;
					if (npc.velocity.Y < 0f && num616 > 0f)
					{
						npc.velocity.Y += num614;
					}
				}
				else if (npc.velocity.Y > num616)
				{
					npc.velocity.Y -= num614;
					if (npc.velocity.Y > 0f && num616 < 0f)
					{
						npc.velocity.Y -= num614;
					}
				}
			}
			else if (npc.ai[0] == 1f)
			{
				npc.localAI[0] = 0f;
				npc.TargetClosest();
				Vector2 vector78 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
				Vector2 vector79 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num618 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector79.X;
				float num619 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector79.Y;
				float num620 = (float)Math.Sqrt(num618 * num618 + num619 * num619);

				npc.ai[1] += 1f;
				if (Main.expertMode)
				{
					npc.ai[1] += num598 / 2;
					if ((double)npc.life < (double)npc.lifeMax * 0.75)
					{
						npc.ai[1] += 0.25f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.5)
					{
						npc.ai[1] += 0.25f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.25)
					{
						npc.ai[1] += 0.25f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.1)
					{
						npc.ai[1] += 0.25f;
					}
				}

				queenBeeLaserCounter++;
				if (queenBeeLaserCounter % 60 == 59)
				{
					Player player = Main.player[npc.target];
					float speed = 10f;
					Vector2 velocity = npc.DirectionTo(player.Center + (player.velocity * npc.Distance(player.Center) / speed)) * speed;
					Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<QueenBeeLaser>(), 10, 0f);
				}

				bool flag36 = false;
				if (npc.ai[1] > 40f)
				{
					npc.ai[1] = 0f;
					npc.ai[2]++;
					flag36 = true;
				}
				if (Collision.CanHit(vector78, 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height) && flag36)
				{
					Main.PlaySound(SoundID.NPCHit, (int)npc.position.X, (int)npc.position.Y);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int num621 = Main.rand.Next(210, 212);
						int num622 = NPC.NewNPC((int)vector78.X, (int)vector78.Y, num621);
						Main.npc[num622].velocity.X = (float)Main.rand.Next(-200, 201) * 0.002f;
						Main.npc[num622].velocity.Y = (float)Main.rand.Next(-200, 201) * 0.002f;
						Main.npc[num622].localAI[0] = 60f;
						Main.npc[num622].netUpdate = true;
					}
				}
				if (num620 > 400f || !Collision.CanHit(new Vector2(vector78.X, vector78.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
				{
					float num623 = 14f;
					float num624 = 0.1f;
					vector79 = vector78;
					num618 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector79.X;
					num619 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector79.Y;
					num620 = (float)Math.Sqrt(num618 * num618 + num619 * num619);
					num620 = num623 / num620;
					if (npc.velocity.X < num618)
					{
						npc.velocity.X += num624;
						if (npc.velocity.X < 0f && num618 > 0f)
						{
							npc.velocity.X += num624;
						}
					}
					else if (npc.velocity.X > num618)
					{
						npc.velocity.X -= num624;
						if (npc.velocity.X > 0f && num618 < 0f)
						{
							npc.velocity.X -= num624;
						}
					}
					if (npc.velocity.Y < num619)
					{
						npc.velocity.Y += num624;
						if (npc.velocity.Y < 0f && num619 > 0f)
						{
							npc.velocity.Y += num624;
						}
					}
					else if (npc.velocity.Y > num619)
					{
						npc.velocity.Y -= num624;
						if (npc.velocity.Y > 0f && num619 < 0f)
						{
							npc.velocity.Y -= num624;
						}
					}
				}
				else
				{
					npc.velocity *= 0.9f;
				}
				npc.spriteDirection = npc.direction;
				if (npc.ai[2] > 5f)
				{
					npc.ai[0] = -1f;
					npc.ai[1] = 1f;
					npc.netUpdate = true;
				}
			}
			else
			{
				if (npc.ai[0] != 3f)
				{
					return;
				}
				float num625 = 4f;
				float num626 = 0.05f;
				if (Main.expertMode)
				{
					num626 = 0.075f;
					num625 = 6f;
				}
				Vector2 vector80 = new Vector2(npc.position.X + (float)(npc.width / 2) + (float)(Main.rand.Next(20) * npc.direction), npc.position.Y + (float)npc.height * 0.8f);
				Vector2 vector81 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
				float num627 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector81.X;
				float num628 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - 300f - vector81.Y;
				float num629 = (float)Math.Sqrt(num627 * num627 + num628 * num628);
				npc.ai[1] += 1f;
				bool flag37 = false;
				if (npc.ai[1] % 120f == 119f)
				{
					//Main.PlaySound(SoundID.Item33, Main.player[npc.target].Center);
					//Projectile proj = Main.projectile[Projectile.NewProjectile(Main.player[npc.target].Center + new Vector2(1000, 0), new Vector2(-15, 0), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
					//proj.tileCollide = false;
					//proj.timeLeft = 300;
					//proj = Main.projectile[Projectile.NewProjectile(Main.player[npc.target].Center + new Vector2(-1000, 0), new Vector2(15, 0), ModContent.ProjectileType<QueenBeeLaser>(), 11, 0)];
					//proj.tileCollide = false;
					//proj.timeLeft = 300;
				}
				if (npc.life < npc.lifeMax / 3)
				{
					if (npc.ai[1] % 12f == 11f)
					{
						flag37 = true;
					}
				}
				else if (npc.life < npc.lifeMax / 2)
				{
					if (npc.ai[1] % 20f == 19f)
					{
						flag37 = true;
					}
				}
				else if (npc.ai[1] % 25f == 24f)
				{
					flag37 = true;
				}
				if (flag37 && npc.position.Y + (float)npc.height < Main.player[npc.target].position.Y)
				{
					Main.PlaySound(SoundID.Item17, npc.position);
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						float num630 = 15f;
						if ((double)npc.life < (double)npc.lifeMax * 0.1)
						{
							num630 += 3f;
						}
						float num631 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector80.X;
						float num632 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector80.Y;
						float num633 = (float)Math.Sqrt(num631 * num631 + num632 * num632);
						num633 = num630 / num633;
						num631 *= num633;
						num632 *= num633;
						int num634 = 11;
						int num635 = 55;
						float lightAmount = 0.5f;
						float rotationAmount = 35;//adds 2 extra projectiles with a set offset
						int num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						Main.projectile[num636].velocity = Main.projectile[num636].velocity.RotatedBy(MathHelper.ToRadians(rotationAmount));
						num636 = Projectile.NewProjectile(vector80.X, vector80.Y, num631, num632, num635, num634, 0f, Main.myPlayer);
						Main.projectile[num636].timeLeft = 300;
						Main.projectile[num636].light = lightAmount;
						Main.projectile[num636].tileCollide = false;
						Main.projectile[num636].velocity = Main.projectile[num636].velocity.RotatedBy(MathHelper.ToRadians(-rotationAmount));
					}
				}
				if (!Collision.CanHit(new Vector2(vector80.X, vector80.Y - 30f), 1, 1, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
				{
					num625 = 14f;
					num626 = 0.1f;
					vector81 = vector80;
					num627 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector81.X;
					num628 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector81.Y;
					num629 = (float)Math.Sqrt(num627 * num627 + num628 * num628);
					num629 = num625 / num629;
					if (npc.velocity.X < num627)
					{
						npc.velocity.X += num626;
						if (npc.velocity.X < 0f && num627 > 0f)
						{
							npc.velocity.X += num626;
						}
					}
					else if (npc.velocity.X > num627)
					{
						npc.velocity.X -= num626;
						if (npc.velocity.X > 0f && num627 < 0f)
						{
							npc.velocity.X -= num626;
						}
					}
					if (npc.velocity.Y < num628)
					{
						npc.velocity.Y += num626;
						if (npc.velocity.Y < 0f && num628 > 0f)
						{
							npc.velocity.Y += num626;
						}
					}
					else if (npc.velocity.Y > num628)
					{
						npc.velocity.Y -= num626;
						if (npc.velocity.Y > 0f && num628 < 0f)
						{
							npc.velocity.Y -= num626;
						}
					}
				}
				else if (num629 > 100f)
				{
					npc.TargetClosest();
					npc.spriteDirection = npc.direction;
					num629 = num625 / num629;
					if (npc.velocity.X < num627)
					{
						npc.velocity.X += num626;
						if (npc.velocity.X < 0f && num627 > 0f)
						{
							npc.velocity.X += num626 * 2f;
						}
					}
					else if (npc.velocity.X > num627)
					{
						npc.velocity.X -= num626;
						if (npc.velocity.X > 0f && num627 < 0f)
						{
							npc.velocity.X -= num626 * 2f;
						}
					}
					if (npc.velocity.Y < num628)
					{
						npc.velocity.Y += num626;
						if (npc.velocity.Y < 0f && num628 > 0f)
						{
							npc.velocity.Y += num626 * 2f;
						}
					}
					else if (npc.velocity.Y > num628)
					{
						npc.velocity.Y -= num626;
						if (npc.velocity.Y > 0f && num628 < 0f)
						{
							npc.velocity.Y -= num626 * 2f;
						}
					}
				}
				if (npc.ai[1] > 800f)
				{
					npc.ai[0] = -1f;
					npc.ai[1] = 3f;
					npc.netUpdate = true;
				}
			}
		}

		public void LunaticCultistAI(NPC npc)
        {
			if (NPC.AnyNPCs(NPCID.CultistDragonHead))
            {
				npc.life += npc.lifeMax / (60 * 30);
				if (npc.life >= npc.lifeMax)
				{
					npc.life = npc.lifeMax;
				}
			}
			if (npc.ai[0] != -1f && Main.rand.Next(1000) == 0)
			{
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, Main.rand.Next(88, 92));
			}
			Lighting.AddLight(npc.Center, new Vector3(0.5f, 0.5f, 0.5f));
			bool expertMode = Main.expertMode;
			bool flag = npc.life <= npc.lifeMax / 2;
			int num = 120;
			int num2 = 35;
			if (expertMode)
			{
				num = 90;
				num2 = 25;
			}
			int num3 = 18;
			int num4 = 3;
			int num5 = 30;
			if (expertMode)
			{
				num3 = 12;
				num4 = 4;
				num5 = 20;
			}
			int num6 = 80;
			int num7 = 45;
			if (expertMode)
			{
				num6 = 40;
				num7 = 30;
			}
			int num8 = 20;
			int num9 = 2;
			if (expertMode)
			{
				num8 = 30;
				num9 = 2;
			}
			int num10 = 20;
			int num11 = 3;
			bool flag2 = npc.type == NPCID.CultistBoss;
			bool flag3 = false;
			bool flag4 = false;
			if (flag)
			{
				npc.defense = (int)((float)npc.defDefense * 0.65f);
			}
			if (!flag2)
			{
				if (npc.ai[3] < 0f || !Main.npc[(int)npc.ai[3]].active || Main.npc[(int)npc.ai[3]].type != NPCID.CultistBoss)
				{
					npc.life = 0;
					npc.HitEffect();
					npc.active = false;
					return;
				}
				npc.ai[0] = Main.npc[(int)npc.ai[3]].ai[0];
				npc.ai[1] = Main.npc[(int)npc.ai[3]].ai[1];
				if (npc.ai[0] == 5f)
				{
					if (npc.justHit)
					{
						npc.life = 0;
						npc.HitEffect();
						npc.active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI);
						}
						NPC obj = Main.npc[(int)npc.ai[3]];
						obj.ai[0] = 6f;
						obj.ai[1] = 0f;
						obj.netUpdate = true;
					}
				}
				else
				{
					flag3 = true;
					flag4 = true;
				}
			}
			else if (npc.ai[0] == 5f && npc.ai[1] >= 120f && npc.ai[1] < 420f && npc.justHit)
			{
				npc.ai[0] = 0f;
				npc.ai[1] = 0f;
				npc.ai[3] += 1f;
				npc.velocity = Vector2.Zero;
				npc.netUpdate = true;
				List<int> list = new List<int>();
				for (int i = 0; i < 200; i++)
				{
					if (Main.npc[i].active && Main.npc[i].type == NPCID.CultistBossClone && Main.npc[i].ai[3] == (float)npc.whoAmI)
					{
						list.Add(i);
					}
				}
				int num12 = 10;
				if (Main.expertMode)
				{
					num12 = 3;
				}
				foreach (int item in list)
				{
					NPC nPC = Main.npc[item];
					if (nPC.localAI[1] == npc.localAI[1] && num12 > 0)
					{
						num12--;
						nPC.life = 0;
						nPC.HitEffect();
						nPC.active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, item);
						}
					}
					else if (num12 > 0)
					{
						num12--;
						nPC.life = 0;
						nPC.HitEffect();
						nPC.active = false;
					}
				}
				Main.projectile[(int)npc.ai[2]].ai[1] = -1f;
				Main.projectile[(int)npc.ai[2]].netUpdate = true;
			}
			Vector2 center = npc.Center;
			Player player = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || player.dead || !player.active)
			{
				npc.TargetClosest(faceTarget: false);
				player = Main.player[npc.target];
				npc.netUpdate = true;
			}
			if (player.dead || Vector2.Distance(player.Center, center) > 5600f)
			{
				npc.life = 0;
				npc.HitEffect();
				npc.active = false;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f);
				}
				new List<int>().Add(npc.whoAmI);
				for (int j = 0; j < 200; j++)
				{
					if (Main.npc[j].active && Main.npc[j].type == NPCID.CultistBossClone && Main.npc[j].ai[3] == (float)npc.whoAmI)
					{
						Main.npc[j].life = 0;
						Main.npc[j].HitEffect();
						Main.npc[j].active = false;
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f);
						}
					}
				}
			}
			float num13 = npc.ai[3];
			if (npc.localAI[0] == 0f)
			{
				Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 89);
				npc.localAI[0] = 1f;
				npc.alpha = 255;
				npc.rotation = 0f;
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					npc.ai[0] = -1f;
					npc.netUpdate = true;
				}
			}
			if (npc.ai[0] == -1f)
			{
				npc.alpha -= 5;
				if (npc.alpha < 0)
				{
					npc.alpha = 0;
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 420f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.netUpdate = true;
				}
				else if (npc.ai[1] > 360f)
				{
					npc.velocity *= 0.95f;
					if (npc.localAI[2] != 13f)
					{
						Main.PlaySound(SoundID.Zombie, (int)npc.position.X, (int)npc.position.Y, 105);
					}
					npc.localAI[2] = 13f;
				}
				else if (npc.ai[1] > 300f)
				{
					npc.velocity = -Vector2.UnitY;
					npc.localAI[2] = 10f;
				}
				else if (npc.ai[1] > 120f)
				{
					npc.localAI[2] = 1f;
				}
				else
				{
					npc.localAI[2] = 0f;
				}
				flag3 = true;
				flag4 = true;
			}
			if (npc.ai[0] == 0f)
			{
				if (npc.ai[1] == 0f)
				{
					npc.TargetClosest(faceTarget: false);
				}
				npc.localAI[2] = 10f;
				int num14 = Math.Sign(player.Center.X - center.X);
				if (num14 != 0)
				{
					npc.direction = (npc.spriteDirection = num14);
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 40f && flag2)
				{
					int num15 = 0;
					if (flag)
					{
						switch ((int)npc.ai[3])
						{
							case 0:
								num15 = 0;
								break;
							case 1:
								num15 = 1;
								break;
							case 2:
								num15 = 0;
								break;
							case 3:
								num15 = 5;
								break;
							case 4:
								num15 = 0;
								break;
							case 5:
								num15 = 3;
								break;
							case 6:
								num15 = 0;
								break;
							case 7:
								num15 = 5;
								break;
							case 8:
								num15 = 0;
								break;
							case 9:
								num15 = 2;
								break;
							case 10:
								num15 = 0;
								break;
							case 11:
								num15 = 3;
								break;
							case 12:
								num15 = 0;
								break;
							case 13:
								num15 = 4;
								npc.ai[3] = -1f;
								break;
							default:
								npc.ai[3] = -1f;
								break;
						}
					}
					else
					{
						switch ((int)npc.ai[3])
						{
							case 0:
								num15 = 0;
								break;
							case 1:
								num15 = 1;
								break;
							case 2:
								num15 = 0;
								break;
							case 3:
								num15 = 2;
								break;
							case 4:
								num15 = 0;
								break;
							case 5:
								num15 = 3;
								break;
							case 6:
								num15 = 0;
								break;
							case 7:
								num15 = 1;
								break;
							case 8:
								num15 = 0;
								break;
							case 9:
								num15 = 2;
								break;
							case 10:
								num15 = 0;
								break;
							case 11:
								num15 = 4;
								npc.ai[3] = -1f;
								break;
							default:
								npc.ai[3] = -1f;
								break;
						}
					}
					int maxValue = 6;
					if (npc.life < npc.lifeMax / 3)
					{
						maxValue = 4;
					}
					if (npc.life < npc.lifeMax / 4)
					{
						maxValue = 3;
					}
					if (expertMode && flag && Main.rand.Next(maxValue) == 0 && num15 != 0 && num15 != 4 && num15 != 5 && NPC.CountNPCS(523) < 10)
					{
						num15 = 6;
					}
					if (num15 == 0)
					{
						float num16 = (float)Math.Ceiling((player.Center + new Vector2(0f, -100f) - center).Length() / 50f);
						if (num16 == 0f)
						{
							num16 = 1f;
						}
						List<int> list2 = new List<int>();
						int num17 = 0;
						list2.Add(npc.whoAmI);
						for (int k = 0; k < 200; k++)
						{
							if (Main.npc[k].active && Main.npc[k].type == NPCID.CultistBossClone && Main.npc[k].ai[3] == (float)npc.whoAmI)
							{
								list2.Add(k);
							}
						}
						bool flag5 = list2.Count % 2 == 0;
						foreach (int item2 in list2)
						{
							NPC nPC2 = Main.npc[item2];
							Vector2 center2 = nPC2.Center;
							float num18 = (float)((num17 + flag5.ToInt() + 1) / 2) * ((float)Math.PI * 2f) * 0.4f / (float)list2.Count;
							if (num17 % 2 == 1)
							{
								num18 *= -1f;
							}
							if (list2.Count == 1)
							{
								num18 = 0f;
							}
							Vector2 vector = new Vector2(0f, -1f).RotatedBy(num18) * new Vector2(300f, 200f);
							Vector2 vector2 = player.Center + vector - center2;
							nPC2.ai[0] = 1f;
							nPC2.ai[1] = num16 * 2f;
							nPC2.velocity = vector2 / num16;
							if (npc.whoAmI >= nPC2.whoAmI)
							{
								nPC2.position -= nPC2.velocity;
							}
							nPC2.netUpdate = true;
							num17++;
						}
					}
					switch (num15)
					{
						case 1:
							npc.ai[0] = 3f;
							npc.ai[1] = 0f;
							break;
						case 2:
							npc.ai[0] = 2f;
							npc.ai[1] = 0f;
							break;
						case 3:
							npc.ai[0] = 4f;
							npc.ai[1] = 0f;
							break;
						case 4:
							npc.ai[0] = 5f;
							npc.ai[1] = 0f;
							break;
					}
					if (num15 == 5)
					{
						npc.ai[0] = 7f;
						npc.ai[1] = 0f;
					}
					if (num15 == 6)
					{
						npc.ai[0] = 8f;
						npc.ai[1] = 0f;
					}
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 1f)
			{
				flag3 = true;
				npc.localAI[2] = 10f;
				if ((float)(int)npc.ai[1] % 2f != 0f && npc.ai[1] != 1f)
				{
					npc.position -= npc.velocity;
				}
				npc.ai[1] -= 1f;
				if (npc.ai[1] <= 0f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 2f)
			{
				npc.localAI[2] = 11f;
				Vector2 vec = Vector2.Normalize(player.Center - center);
				if (vec.HasNaNs())
				{
					vec = new Vector2(npc.direction, 0f);
				}
				if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num == 0)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						List<int> list3 = new List<int>();
						for (int l = 0; l < 200; l++)
						{
							if (Main.npc[l].active && Main.npc[l].type == NPCID.CultistBossClone && Main.npc[l].ai[3] == (float)npc.whoAmI)
							{
								list3.Add(l);
							}
						}
						foreach (int item3 in list3)
						{
							NPC nPC3 = Main.npc[item3];
							Vector2 center3 = nPC3.Center;
							int num19 = Math.Sign(player.Center.X - center3.X);
							if (num19 != 0)
							{
								nPC3.direction = (nPC3.spriteDirection = num19);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								vec = Vector2.Normalize(player.Center - center3 + player.velocity * 20f);
								if (vec.HasNaNs())
								{
									vec = new Vector2(npc.direction, 0f);
								}
								Vector2 vector3 = center3 + new Vector2(npc.direction * 30, 12f);
								for (int m = 0; m < 1; m++)
								{
									Vector2 spinninpoint = vec * (6f + (float)Main.rand.NextDouble() * 4f);
									for (int i = 0; i <  Main.rand.Next(3, 5); i++)
									{
										spinninpoint = spinninpoint.RotatedByRandom(MathHelper.ToRadians(360));
										Projectile.NewProjectile(vector3.X, vector3.Y, spinninpoint.X / 2, spinninpoint.Y / 2, ProjectileID.FrostWave, 18, 0f, Main.myPlayer);
									}
								}
							}
						}
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec.HasNaNs())
						{
							vec = new Vector2(npc.direction, 0f);
						}
						Vector2 vector4 = npc.Center + new Vector2(npc.direction * 30, 12f);
						for (int n = 0; n < 1; n++)
						{
							Vector2 vector5 = vec * 4f;
							Projectile proj = Main.projectile[Projectile.NewProjectile(vector4.X, vector4.Y, vector5.X, vector5.Y, ProjectileID.CultistBossIceMist, num2, 0f, Main.myPlayer, 0f, 1f)];
							proj.extraUpdates += 1;
						}
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= (float)(4 + num))
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 3f)
			{
				npc.localAI[2] = 11f;
				Vector2 vec2 = Vector2.Normalize(player.Center - center);
				if (vec2.HasNaNs())
				{
					vec2 = new Vector2(npc.direction, 0f);
				}
				if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num3 == 0)
				{
					if ((int)(npc.ai[1] - 4f) / num3 == 2)
					{
						List<int> list4 = new List<int>();
						for (int num20 = 0; num20 < 200; num20++)
						{
							if (Main.npc[num20].active && Main.npc[num20].type == NPCID.CultistBossClone && Main.npc[num20].ai[3] == (float)npc.whoAmI)
							{
								list4.Add(num20);
							}
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							foreach (int item4 in list4)
							{
								NPC nPC4 = Main.npc[item4];
								Vector2 center4 = nPC4.Center;
								int num21 = Math.Sign(player.Center.X - center4.X);
								if (num21 != 0)
								{
									nPC4.direction = (nPC4.spriteDirection = num21);
								}
								if (Main.netMode != NetmodeID.MultiplayerClient)
								{
									vec2 = Vector2.Normalize(player.Center - center4 + player.velocity * 20f);
									if (vec2.HasNaNs())
									{
										vec2 = new Vector2(npc.direction, 0f);
									}
									Vector2 vector6 = center4 + new Vector2(npc.direction * 30, 12f);
									for (int num22 = 0; num22 < 1; num22++)
									{
										Vector2 spinninpoint2 = vec2 * (6f + (float)Main.rand.NextDouble() * 4f);
										spinninpoint2 = spinninpoint2.RotatedByRandom(0.52359879016876221);
										float speed = 15f;
										Vector2 actualVelocity = nPC4.DirectionTo(player.Center + player.velocity * (nPC4.Distance(player.Center) / speed)) * speed;
										Projectile.NewProjectile(vector6.X, vector6.Y, actualVelocity.X, actualVelocity.Y, ProjectileID.Fireball, num7, 0f, Main.myPlayer);
									}
								}
							}
						}
					}
					int num23 = Math.Sign(player.Center.X - center.X);
					if (num23 != 0)
					{
						npc.direction = (npc.spriteDirection = num23);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec2 = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec2.HasNaNs())
						{
							vec2 = new Vector2(npc.direction, 0f);
						}
						Vector2 vector7 = npc.Center + new Vector2(npc.direction * 30, 12f);
						for (int num24 = 0; num24 < 1; num24++)
						{
							Vector2 spinninpoint3 = vec2 * (6f + (float)Main.rand.NextDouble() * 4f);
							spinninpoint3 = spinninpoint3.RotatedByRandom(0.52359879016876221);
							Projectile.NewProjectile(vector7.X, vector7.Y, spinninpoint3.X, spinninpoint3.Y, ProjectileID.CultistBossFireBall, num5, 0f, Main.myPlayer);
						}
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= (float)(4 + num3 * num4))
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 4f)
			{
				if (flag2)
				{
					npc.localAI[2] = 12f;
				}
				else
				{
					npc.localAI[2] = 11f;
				}
				if (npc.ai[1] == 20f && flag2 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					List<int> list5 = new List<int>();
					for (int num25 = 0; num25 < 200; num25++)
					{
						if (Main.npc[num25].active && Main.npc[num25].type == NPCID.CultistBossClone && Main.npc[num25].ai[3] == (float)npc.whoAmI)
						{
							list5.Add(num25);
						}
					}
					foreach (int item5 in list5)
					{
						NPC nPC5 = Main.npc[item5];
						Vector2 center5 = nPC5.Center;
						int num26 = Math.Sign(player.Center.X - center5.X);
						if (num26 != 0)
						{
							nPC5.direction = (nPC5.spriteDirection = num26);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							Vector2 vector8 = Vector2.Normalize(player.Center - center5 + player.velocity * 20f);
							if (vector8.HasNaNs())
							{
								vector8 = new Vector2(npc.direction, 0f);
							}
							Vector2 vector9 = center5 + new Vector2(npc.direction * 30, 12f);
							for (int num27 = 0; num27 < 1; num27++)
							{
								Vector2 spinninpoint4 = vector8 * (6f + (float)Main.rand.NextDouble() * 4f);
								spinninpoint4 = spinninpoint4.RotatedByRandom(0.52359879016876221);
								Projectile.NewProjectile(vector9.X, vector9.Y - 100f, spinninpoint4.X, spinninpoint4.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
							}
						}
					}
					if ((int)(npc.ai[1] - 20f) % num6 == 0)
					{
						float speed = 8.5f;
						Vector2 velocity = player.DirectionFrom(new Vector2(npc.Center.X, npc.Center.Y - 100f)) * speed;
						Projectile.NewProjectile(npc.Center.X, npc.Center.Y - 100f, velocity.X, velocity.Y, ProjectileID.CultistBossLightningOrb, num7, 0f, Main.myPlayer);
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= (float)(20 + num6))
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 5f)
			{
				npc.localAI[2] = 10f;
				if (Vector2.Normalize(player.Center - center).HasNaNs())
				{
					new Vector2(npc.direction, 0f);
				}
				if (npc.ai[1] >= 0f && npc.ai[1] < 30f)
				{
					flag3 = true;
					flag4 = true;
					float num28 = (npc.ai[1] - 0f) / 30f;
					npc.alpha = (int)(num28 * 255f);
				}
				else if (npc.ai[1] >= 30f && npc.ai[1] < 90f)
				{
					if (npc.ai[1] == 30f && Main.netMode != NetmodeID.MultiplayerClient && flag2)
					{
						npc.localAI[1] += 1f;
						Vector2 spinningpoint = new Vector2(180f, 0f);
						List<int> list6 = new List<int>();
						for (int num29 = 0; num29 < 200; num29++)
						{
							if (Main.npc[num29].active && Main.npc[num29].type == NPCID.CultistBossClone && Main.npc[num29].ai[3] == (float)npc.whoAmI)
							{
								list6.Add(num29);
							}
						}
						int num30 = 6 - list6.Count;
						if (num30 > 4)
						{
							num30 = 4;
						}
						int num31 = list6.Count + num30 + 1; //Number of clones
						float[] array = new float[num31];
						for (int num32 = 0; num32 < array.Length; num32++)
						{
							array[num32] = Vector2.Distance(npc.Center + spinningpoint.RotatedBy((float)num32 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f), player.Center);
						}
						int num33 = 0;
						for (int num34 = 1; num34 < array.Length; num34++)
						{
							if (array[num33] > array[num34])
							{
								num33 = num34;
							}
						}
						num33 = ((num33 >= num31 / 2) ? (num33 - num31 / 2) : (num33 + num31 / 2));
						int num35 = num30;
						for (int num36 = 0; num36 < array.Length; num36++)
						{
							if (num33 != num36)
							{
								Vector2 center6 = npc.Center + spinningpoint.RotatedBy((float)num36 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f);
								if (num35-- > 0)
								{
									int num37 = NPC.NewNPC((int)center6.X, (int)center6.Y + npc.height / 2, NPCID.CultistBossClone, npc.whoAmI);
									Main.npc[num37].ai[3] = npc.whoAmI;
									Main.npc[num37].netUpdate = true;
									Main.npc[num37].localAI[1] = npc.localAI[1];
								}
								else
								{
									int num38 = list6[-num35 - 1];
									Main.npc[num38].Center = center6;
									NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num38);
								}
							}
						}
						npc.ai[2] = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ProjectileID.CultistRitual, 0, 0f, Main.myPlayer, 0f, npc.whoAmI);
						npc.Center += spinningpoint.RotatedBy((float)num33 * ((float)Math.PI * 2f) / (float)num31 - (float)Math.PI / 2f);
						npc.netUpdate = true;
						list6.Clear();
					}
					flag3 = true;
					flag4 = true;
					npc.alpha = 255;
					if (flag2)
					{
						Vector2 vector10 = Main.projectile[(int)npc.ai[2]].Center;
						vector10 -= npc.Center;
						if (vector10 == Vector2.Zero)
						{
							vector10 = -Vector2.UnitY;
						}
						vector10.Normalize();
						if (Math.Abs(vector10.Y) < 0.77f)
						{
							npc.localAI[2] = 11f;
						}
						else if (vector10.Y < 0f)
						{
							npc.localAI[2] = 12f;
						}
						else
						{
							npc.localAI[2] = 10f;
						}
						int num39 = Math.Sign(vector10.X);
						if (num39 != 0)
						{
							npc.direction = (npc.spriteDirection = num39);
						}
					}
					else
					{
						Vector2 vector11 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
						vector11 -= npc.Center;
						if (vector11 == Vector2.Zero)
						{
							vector11 = -Vector2.UnitY;
						}
						vector11.Normalize();
						if (Math.Abs(vector11.Y) < 0.77f)
						{
							npc.localAI[2] = 11f;
						}
						else if (vector11.Y < 0f)
						{
							npc.localAI[2] = 12f;
						}
						else
						{
							npc.localAI[2] = 10f;
						}
						int num40 = Math.Sign(vector11.X);
						if (num40 != 0)
						{
							npc.direction = (npc.spriteDirection = num40);
						}
					}
				}
				else if (npc.ai[1] >= 90f && npc.ai[1] < 120f)
				{
					flag3 = true;
					flag4 = true;
					float num41 = (npc.ai[1] - 90f) / 30f;
					npc.alpha = 255 - (int)(num41 * 255f);
				}
				else if (npc.ai[1] >= 120f && npc.ai[1] < 420f)
				{
					flag4 = true;
					npc.alpha = 0;
					if (flag2)
					{
						Vector2 vector12 = Main.projectile[(int)npc.ai[2]].Center;
						vector12 -= npc.Center;
						if (vector12 == Vector2.Zero)
						{
							vector12 = -Vector2.UnitY;
						}
						vector12.Normalize();
						if (Math.Abs(vector12.Y) < 0.77f)
						{
							npc.localAI[2] = 11f;
						}
						else if (vector12.Y < 0f)
						{
							npc.localAI[2] = 12f;
						}
						else
						{
							npc.localAI[2] = 10f;
						}
						int num42 = Math.Sign(vector12.X);
						if (num42 != 0)
						{
							npc.direction = (npc.spriteDirection = num42);
						}
					}
					else
					{
						Vector2 vector13 = Main.projectile[(int)Main.npc[(int)npc.ai[3]].ai[2]].Center;
						vector13 -= npc.Center;
						if (vector13 == Vector2.Zero)
						{
							vector13 = -Vector2.UnitY;
						}
						vector13.Normalize();
						if (Math.Abs(vector13.Y) < 0.77f)
						{
							npc.localAI[2] = 11f;
						}
						else if (vector13.Y < 0f)
						{
							npc.localAI[2] = 12f;
						}
						else
						{
							npc.localAI[2] = 10f;
						}
						int num43 = Math.Sign(vector13.X);
						if (num43 != 0)
						{
							npc.direction = (npc.spriteDirection = num43);
						}
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 420f)
				{
					flag4 = true;
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 6f)
			{
				npc.localAI[2] = 13f;
				npc.ai[1] += 1f;
				if (npc.ai[1] >= 120f)
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 7f)
			{
				npc.localAI[2] = 11f;
				Vector2 vec3 = Vector2.Normalize(player.Center - center);
				if (vec3.HasNaNs())
				{
					vec3 = new Vector2(npc.direction, 0f);
				}
				if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num8 == 0)
				{
					if ((int)(npc.ai[1] - 4f) / num8 == 2)
					{
						List<int> list7 = new List<int>();
						for (int num44 = 0; num44 < 200; num44++)
						{
							if (Main.npc[num44].active && Main.npc[num44].type == NPCID.CultistBossClone && Main.npc[num44].ai[3] == (float)npc.whoAmI)
							{
								list7.Add(num44);
							}
						}
						foreach (int item6 in list7)
						{
							NPC nPC6 = Main.npc[item6];
							Vector2 center7 = nPC6.Center;
							int num45 = Math.Sign(player.Center.X - center7.X);
							if (num45 != 0)
							{
								nPC6.direction = (nPC6.spriteDirection = num45);
							}
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								vec3 = Vector2.Normalize(player.Center - center7 + player.velocity * 20f);
								if (vec3.HasNaNs())
								{
									vec3 = new Vector2(npc.direction, 0f);
								}
								Vector2 vector14 = center7 + new Vector2(npc.direction * 30, 12f);
								for (int num46 = 0; (float)num46 < 5f; num46++)
								{
									Vector2 spinninpoint5 = vec3 * (6f + (float)Main.rand.NextDouble() * 4f);
									spinninpoint5 = spinninpoint5.RotatedByRandom(1.2566370964050293);
									Projectile.NewProjectile(vector14.X, vector14.Y, spinninpoint5.X, spinninpoint5.Y, ProjectileID.CultistBossFireBallClone, 18, 0f, Main.myPlayer);
								}
							}
						}
					}
					int num47 = Math.Sign(player.Center.X - center.X);
					if (num47 != 0)
					{
						npc.direction = (npc.spriteDirection = num47);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						vec3 = Vector2.Normalize(player.Center - center + player.velocity * 20f);
						if (vec3.HasNaNs())
						{
							vec3 = new Vector2(npc.direction, 0f);
						}
						Vector2 vector15 = npc.Center + new Vector2(npc.direction * 30, 12f);
						float num48 = 8f;
						float num49 = (float)Math.PI * 2f / 25f;
						for (int num50 = 0; (float)num50 < 5f; num50++)
						{
							Vector2 spinningpoint2 = vec3 * num48;
							spinningpoint2 = spinningpoint2.RotatedBy(num49 * (float)num50 - ((float)Math.PI * 2f / 5f - num49) / 2f);
							float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * ((float)Math.PI * 2f) / 60f;
							int num51 = NPC.NewNPC((int)vector15.X, (int)vector15.Y + 7, 522, 0, 0f, ai, spinningpoint2.X, spinningpoint2.Y);
							Main.npc[num51].velocity = spinningpoint2;
							Main.npc[num51].dontTakeDamage = true;
						}
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= (float)(4 + num8 * num9))
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			else if (npc.ai[0] == 8f)
			{
				npc.localAI[2] = 13f;
				if (npc.ai[1] >= 4f && flag2 && (int)(npc.ai[1] - 4f) % num10 == 0)
				{
					List<int> list8 = new List<int>();
					for (int num52 = 0; num52 < 200; num52++)
					{
						if (Main.npc[num52].active && Main.npc[num52].type == NPCID.CultistBossClone && Main.npc[num52].ai[3] == (float)npc.whoAmI)
						{
							list8.Add(num52);
						}
					}
					int num53 = list8.Count + 1;
					if (num53 > 3)
					{
						num53 = 3;
					}
					int num54 = Math.Sign(player.Center.X - center.X);
					if (num54 != 0)
					{
						npc.direction = (npc.spriteDirection = num54);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						for (int num55 = 0; num55 < num53; num55++)
						{
							Point point = npc.Center.ToTileCoordinates();
							Point point2 = Main.player[npc.target].Center.ToTileCoordinates();
							Vector2 vector16 = Main.player[npc.target].Center - npc.Center;
							int num56 = 20;
							int num57 = 3;
							int num58 = 7;
							int num59 = 2;
							int num60 = 0;
							bool flag6 = false;
							if (vector16.Length() > 2000f)
							{
								flag6 = true;
							}
							while (!flag6 && num60 < 100)
							{
								num60++;
								int num61 = Main.rand.Next(point2.X - num56, point2.X + num56 + 1);
								int num62 = Main.rand.Next(point2.Y - num56, point2.Y + num56 + 1);
								if ((num62 < point2.Y - num58 || num62 > point2.Y + num58 || num61 < point2.X - num58 || num61 > point2.X + num58) && (num62 < point.Y - num57 || num62 > point.Y + num57 || num61 < point.X - num57 || num61 > point.X + num57) && !Main.tile[num61, num62].active())
								{
									bool flag7 = true;
									if (flag7 && Collision.SolidTiles(num61 - num59, num61 + num59, num62 - num59, num62 + num59))
									{
										flag7 = false;
									}
									if (flag7)
									{
										int newNPC = NPC.NewNPC(num61 * 16 + 8, num62 * 16 + 8, 523, 0, npc.whoAmI);
										Main.npc[newNPC].dontTakeDamage = true;
										flag6 = true;
										break;
									}
								}
							}
						}
					}
				}
				npc.ai[1] += 1f;
				if (npc.ai[1] >= (float)(4 + num10 * num11))
				{
					npc.ai[0] = 0f;
					npc.ai[1] = 0f;
					npc.ai[3] += 1f;
					npc.velocity = Vector2.Zero;
					npc.netUpdate = true;
				}
			}
			if (!flag2)
			{
				npc.ai[3] = num13;
			}
			npc.dontTakeDamage = flag3;
			npc.chaseable = !flag4;
		}

		int invincible = 1;
		bool BoCPartTwo = false;
		bool BoCSpawnedNewCreepers = false;
		int BoCIchorCounter = 0;
		int BoCIchorAmount = 0;
		int BoCDustTelegraphCounter = 0;
		bool BoCHealed = false;
		public void BrainOfCthulhuAI(NPC npc)
        {
			float teleportDistance = 350f;
			NPC.crimsonBoss = npc.whoAmI;
			if (Main.netMode != 1 && npc.localAI[0] == 0f)
			{
				npc.localAI[0] = 1f;
				for (int num796 = 0; num796 < 20; num796++)
				{
					float x2 = npc.Center.X;
					float y3 = npc.Center.Y;
					x2 += (float)Main.rand.Next(-npc.width, npc.width);
					y3 += (float)Main.rand.Next(-npc.height, npc.height);
					int num797 = NPC.NewNPC((int)x2, (int)y3, NPCID.Creeper);
					Main.npc[num797].velocity = new Vector2((float)Main.rand.Next(-30, 31) * 0.1f, (float)Main.rand.Next(-30, 31) * 0.1f);
					Main.npc[num797].netUpdate = true;
					invincible *= -1;
					if (invincible == 1)
                    {
						Main.npc[num797].dontTakeDamage = true;
						Main.npc[num797].alpha = 120;
						Main.npc[num797].knockBackResist = 0f;
					}
				}
			}

			bool allInvincible = true;
			List<NPC> creepers = new List<NPC>();

			foreach (NPC creeper in Main.npc)
            {
				if (creeper.type == NPCID.Creeper && creeper.active)
                {
					creepers.Add(creeper);
					if (!creeper.dontTakeDamage)
                    {
						allInvincible = false;
                    }
                }
            }

			if (allInvincible)
            {
				BoCPartTwo = true;
				foreach (NPC creeper in creepers)
				{
					invincible *= -1;
					if (invincible == 1 || creepers.Count == 1)
					{
						creeper.dontTakeDamage = false;
						creeper.alpha = 0;
					}
					creeper.defense += 2;
                }
            }

			if (BoCSpawnedNewCreepers && NPC.AnyNPCs(NPCID.Creeper))
            {
				BoCIchorCounter++;
				if (BoCIchorAmount > 0)
                {
					if (BoCIchorCounter > 20)
					{
						BoCIchorAmount--;
						BoCIchorCounter = 0;

						NPC creeper = Main.rand.Next(creepers);
						Vector2 position = creeper.Center;
						float speed = 9f;
						Vector2 velocity = creeper.DirectionTo(Main.player[npc.target].Center) * speed;
						int proj = Projectile.NewProjectile(position, velocity, ProjectileID.GoldenShowerHostile, 25 / 4, 0f);
						Main.projectile[proj].tileCollide = false;
					}
                }
                else
				{
					if (BoCIchorCounter > 250)
					{
						Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0, 1, 1);
						TerrorbornMod.ScreenShake(5f);
						BoCIchorCounter = 0;
						BoCIchorAmount = 13;
					}
				}
            }

			if (Main.netMode != 1)
			{
				npc.TargetClosest();
				int num798 = 6000;
				if (Math.Abs(npc.Center.X - Main.player[npc.target].Center.X) + Math.Abs(npc.Center.Y - Main.player[npc.target].Center.Y) > (float)num798)
				{
					npc.active = false;
					npc.life = 0;
					if (Main.netMode == 2)
					{
						NetMessage.SendData(23, -1, -1, null, npc.whoAmI);
					}
				}
			}
			if (npc.ai[0] < 0f)
			{
				if (npc.localAI[2] == 0f)
				{
					Main.PlaySound(3, (int)npc.position.X, (int)npc.position.Y);
					npc.localAI[2] = 1f;
					Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 392);
					Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 393);
					Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 394);
					Gore.NewGore(npc.position, new Vector2((float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f), 395);
					for (int num799 = 0; num799 < 20; num799++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, 5, (float)Main.rand.Next(-30, 31) * 0.2f, (float)Main.rand.Next(-30, 31) * 0.2f);
					}
					Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 0);
				}

				npc.TargetClosest();
				float speed = 5f;
				if (BoCSpawnedNewCreepers && !NPC.AnyNPCs(NPCID.Creeper))
				{
					speed = 7.5f;
					if (!BoCHealed)
                    {
						BoCHealed = true;
						npc.life = npc.lifeMax;
					}
				}
				npc.velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;

				if (npc.life <= npc.lifeMax / 4 && !BoCSpawnedNewCreepers)
				{
					BoCSpawnedNewCreepers = true;
					for (int num796 = 0; num796 < 10; num796++)
					{
						float x2 = npc.Center.X;
						float y3 = npc.Center.Y;
						x2 += (float)Main.rand.Next(-npc.width, npc.width);
						y3 += (float)Main.rand.Next(-npc.height, npc.height);
						int num797 = NPC.NewNPC((int)x2, (int)y3, NPCID.Creeper);
						Main.npc[num797].velocity = new Vector2((float)Main.rand.Next(-30, 31) * 0.1f, (float)Main.rand.Next(-30, 31) * 0.1f);
						Main.npc[num797].netUpdate = true;
						invincible *= -1;
						if (invincible == 1)
						{
							Main.npc[num797].dontTakeDamage = true;
							Main.npc[num797].alpha = 120;
							Main.npc[num797].knockBackResist = 0f;
						}
						Main.npc[num797].defense += 4;
					}
				}

				npc.dontTakeDamage = NPC.AnyNPCs(NPCID.Creeper);

				if (npc.ai[0] == -1f)
				{
					if (Main.netMode != 1)
					{
						npc.localAI[1] += 1f;
						if (npc.justHit)
						{
							npc.localAI[1] -= Main.rand.Next(5);
						}
						int num804 = 60 + Main.rand.Next(120);
						if (Main.netMode != 0)
						{
							num804 += Main.rand.Next(30, 90);
						}
						if (npc.localAI[1] >= (float)num804)
						{
							npc.localAI[1] = 0f;
							npc.TargetClosest();
							int num814 = 0;
							Vector2 vector = npc.DirectionTo(Main.player[npc.target].Center) * teleportDistance + Main.player[npc.target].Center;
							float num815 = vector.X / 16f;
							float num816 = vector.Y / 16f;
							npc.ai[0] = 1f;
							npc.ai[1] = num815;
							npc.ai[0] = 1f;
							npc.ai[2] = num816;
							npc.netUpdate = true;
						}
					}
				}
				else if (npc.ai[0] == -2f)
				{
					npc.velocity *= 0.9f;
					if (Main.netMode != 0)
					{
						npc.ai[3] += 15f;
					}
					else
					{
						npc.ai[3] += 25f;
					}
					BoCDustTelegraphCounter++;
					if (BoCDustTelegraphCounter >= 5)
                    {
						BoCDustTelegraphCounter = 0;
						DustExplosion(new Vector2(npc.ai[1] * 16f, npc.ai[2] * 16f), 0, 20, 20f, DustID.GoldFlame, 1.5f, true);
                    }
					if (npc.ai[3] >= 255f)
					{
						npc.ai[3] = 255f;
						npc.position.X = npc.ai[1] * 16f - (float)(npc.width / 2);
						npc.position.Y = npc.ai[2] * 16f - (float)(npc.height / 2);
						Main.PlaySound(SoundID.Item8, npc.Center);
						npc.ai[0] = -3f;
						npc.netUpdate = true;
						npc.netSpam = 0;
					}
					npc.alpha = (int)npc.ai[3];
				}
				else if (npc.ai[0] == -3f)
				{
					if (Main.netMode != 0)
					{
						npc.ai[3] -= 15f;
					}
					else
					{
						npc.ai[3] -= 25f;
					}
					if (npc.ai[3] <= 0f)
					{
						npc.ai[3] = 0f;
						npc.ai[0] = -1f;
						npc.netUpdate = true;
						npc.netSpam = 0;
					}
					npc.alpha = (int)npc.ai[3];
				}
			}
			else
			{
				npc.TargetClosest();
				float speed = 1f;
				if (BoCPartTwo)
                {
					speed = 3f;
                }
				if (BoCSpawnedNewCreepers)
				{
					speed = 5f;
				}
				npc.velocity = npc.DirectionTo(Main.player[npc.target].Center) * speed;
				if (npc.ai[0] == 0f)
				{
					if (Main.netMode != 1)
					{
						int num812 = 0;
						for (int num813 = 0; num813 < 200; num813++)
						{
							if (Main.npc[num813].active && Main.npc[num813].type == 267)
							{
								num812++;
							}
						}
						if (num812 == 0)
						{
							npc.ai[0] = -1f;
							npc.localAI[1] = 0f;
							npc.alpha = 0;
							npc.netUpdate = true;
						}
						npc.localAI[1] += 1f;
						if (npc.localAI[1] >= (float)(120 + Main.rand.Next(300)))
						{
							npc.localAI[1] = 0f;
							npc.TargetClosest();
							int num814 = 0;
							Vector2 vector = npc.DirectionTo(Main.player[npc.target].Center) * teleportDistance + Main.player[npc.target].Center;
							float num815 = vector.X / 16f;
							float num816 = vector.Y / 16f;
							npc.ai[0] = 1f;
							npc.ai[1] = num815;
							npc.ai[0] = 1f;
							npc.ai[2] = num816;
							npc.netUpdate = true;
						}
					}
				}
				else if (npc.ai[0] == 1f)
				{
					BoCDustTelegraphCounter++;
					if (BoCDustTelegraphCounter >= 5)
					{
						BoCDustTelegraphCounter = 0;
						DustExplosion(new Vector2(npc.ai[1] * 16f, npc.ai[2] * 16f), 0, 20, 20f, DustID.GoldFlame, 1.5f, true);
					}
					npc.alpha += 5;
					if (npc.alpha >= 255)
					{
						Main.PlaySound(SoundID.Item8, npc.Center);
						npc.alpha = 255;
						npc.position.X = npc.ai[1] * 16f - (float)(npc.width / 2);
						npc.position.Y = npc.ai[2] * 16f - (float)(npc.height / 2);
						npc.ai[0] = 2f;
					}
				}
				else if (npc.ai[0] == 2f)
				{
					npc.alpha -= 5;
					if (npc.alpha <= 0)
					{
						npc.alpha = 0;
						npc.ai[0] = 0f;
					}
				}
			}
			if (Main.player[npc.target].dead || !Main.player[npc.target].ZoneCrimson)
			{
				if (npc.localAI[3] < 120f)
				{
					npc.localAI[3]++;
				}
				if (npc.localAI[3] > 60f)
				{
					npc.velocity.Y += (npc.localAI[3] - 60f) * 0.25f;
				}
				npc.ai[0] = 2f;
				npc.alpha = 10;
			}
			else if (npc.localAI[3] > 0f)
			{
				npc.localAI[3]--;
			}
		}

		NPC topEye;
		NPC bottomEye;
		int timeAlive = 0;
		float eyeRotOffest = 0f;
		int WoFAttackCounter = 0;
		bool WoFDoingSpread = false;
		public void WoFMouthAI(NPC npc)
		{
			timeAlive++;
			if (npc.position.X < 160f || npc.position.X > (float)((Main.maxTilesX - 10) * 16))
			{
				npc.active = false;
			}
			if (npc.localAI[0] == 0f)
			{
				npc.localAI[0] = 1f;
				Main.wofB = -1;
				Main.wofT = -1;
			}
			npc.ai[1] += 1f;
			if (npc.ai[2] == 0f)
			{
				if ((double)npc.life < (double)npc.lifeMax * 0.5)
				{
					npc.ai[1] += 1f;
				}
				if ((double)npc.life < (double)npc.lifeMax * 0.2)
				{
					npc.ai[1] += 1f;
				}
				if (npc.ai[1] > 2700f)
				{
					npc.ai[2] = 1f;
				}
			}
			if (npc.ai[2] > 0f && npc.ai[1] > 60f)
			{
				int num333 = 3;
				if ((double)npc.life < (double)npc.lifeMax * 0.3)
				{
					num333++;
				}
				npc.ai[2] += 1f;
				npc.ai[1] = 0f;
				if (npc.ai[2] > (float)num333)
				{
					npc.ai[2] = 0f;
				}
				if (Main.netMode != 1)
				{
					int num334 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)(npc.height / 2) + 20f), 117, 1);
					Main.npc[num334].velocity.X = npc.direction * 8;
				}
			}
			npc.localAI[3] += 1f;
			if (npc.localAI[3] >= (float)(600 + Main.rand.Next(1000)))
			{
				npc.localAI[3] = -Main.rand.Next(200);
				Main.PlaySound(4, (int)npc.position.X, (int)npc.position.Y, 10);
			}
			Main.wof = npc.whoAmI;
			int num335 = (int)(npc.position.X / 16f);
			int num336 = (int)((npc.position.X + (float)npc.width) / 16f);
			int num337 = (int)((npc.position.Y + (float)(npc.height / 2)) / 16f);
			int num338 = 0;
			int num339 = num337 + 7;
			while (num338 < 15 && num339 > Main.maxTilesY - 200)
			{
				num339++;
				for (int num340 = num335; num340 <= num336; num340++)
				{
					try
					{
						if (WorldGen.SolidTile(num340, num339) || Main.tile[num340, num339].liquid > 0)
						{
							num338++;
						}
					}
					catch
					{
						num338 += 15;
					}
				}
			}
			num339 += 4;
			if (Main.wofB == -1)
			{
				Main.wofB = num339 * 16;
			}
			else if (Main.wofB > num339 * 16)
			{
				Main.wofB--;
				if (Main.wofB < num339 * 16)
				{
					Main.wofB = num339 * 16;
				}
			}
			else if (Main.wofB < num339 * 16)
			{
				Main.wofB++;
				if (Main.wofB > num339 * 16)
				{
					Main.wofB = num339 * 16;
				}
			}
			num338 = 0;
			num339 = num337 - 7;
			while (num338 < 15 && num339 < Main.maxTilesY - 10)
			{
				num339--;
				for (int num341 = num335; num341 <= num336; num341++)
				{
					try
					{
						if (WorldGen.SolidTile(num341, num339) || Main.tile[num341, num339].liquid > 0)
						{
							num338++;
						}
					}
					catch
					{
						num338 += 15;
					}
				}
			}
			num339 -= 4;
			if (Main.wofT == -1)
			{
				Main.wofT = num339 * 16;
			}
			else if (Main.wofT > num339 * 16)
			{
				Main.wofT--;
				if (Main.wofT < num339 * 16)
				{
					Main.wofT = num339 * 16;
				}
			}
			else if (Main.wofT < num339 * 16)
			{
				Main.wofT++;
				if (Main.wofT > num339 * 16)
				{
					Main.wofT = num339 * 16;
				}
			}
			float num342 = (Main.wofB + Main.wofT) / 2 - npc.height / 2;
			if (npc.position.Y > num342 + 1f)
			{
				npc.velocity.Y = -1f;
			}
			else if (npc.position.Y < num342 - 1f)
			{
				npc.velocity.Y = 1f;
			}
			npc.velocity.Y = 0f;
			int num343 = (Main.maxTilesY - 180) * 16;
			if (num342 < (float)num343)
			{
				num342 = num343;
			}
			npc.position.Y = num342;

			float num344 = MathHelper.Lerp(6.5f, 4f, (float)npc.life / (float)npc.lifeMax);
			if (Main.player[npc.target].dead || npc.Distance(Main.player[npc.target].Center) > 3000f)
            {
				num344 *= 25f;
            }

			if (npc.velocity.X == 0f)
			{
				npc.TargetClosest();
				npc.velocity.X = npc.direction;
			}
			if (npc.velocity.X < 0f)
			{
				npc.velocity.X = -num344;
				npc.direction = -1;
			}
			else
			{
				npc.velocity.X = num344;
				npc.direction = 1;
			}
			npc.spriteDirection = npc.direction;
			Vector2 vector37 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num345 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector37.X;
			float num346 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector37.Y;
			float num347 = (float)Math.Sqrt(num345 * num345 + num346 * num346);
			float num348 = num347;
			num345 *= num347;
			num346 *= num347;
			if (npc.direction > 0)
			{
				if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) > npc.position.X + (float)(npc.width / 2))
				{
					npc.rotation = (float)Math.Atan2(0f - num346, 0f - num345) + 3.14f;
				}
				else
				{
					npc.rotation = 0f;
				}
			}
			else if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) < npc.position.X + (float)(npc.width / 2))
			{
				npc.rotation = (float)Math.Atan2(num346, num345) + 3.14f;
			}
			else
			{
				npc.rotation = 0f;
			}
			if (Main.expertMode && Main.netMode != 1)
			{
				int num349 = (int)(1f + (float)npc.life / (float)npc.lifeMax * 10f);
				num349 *= num349;
				if (num349 < 400)
				{
					num349 = (num349 * 19 + 400) / 20;
				}
				if (num349 < 60)
				{
					num349 = (num349 * 3 + 60) / 4;
				}
				if (num349 < 20)
				{
					num349 = (num349 + 20) / 2;
				}
				num349 = (int)((double)num349 * 0.7);
				if (Main.rand.Next(num349) == 0)
				{
					int num350 = 0;
					float[] array = new float[10];
					for (int num351 = 0; num351 < 200; num351++)
					{
						if (num350 < 10 && Main.npc[num351].active && Main.npc[num351].type == 115)
						{
							array[num350] = Main.npc[num351].ai[0];
							num350++;
						}
					}
					int maxValue = 1 + num350 * 2;
					if (num350 < 10 && Main.rand.Next(maxValue) <= 1)
					{
						int num352 = -1;
						for (int num353 = 0; num353 < 1000; num353++)
						{
							int num354 = Main.rand.Next(10);
							float num355 = (float)num354 * 0.1f - 0.05f;
							bool flag27 = true;
							for (int num356 = 0; num356 < num350; num356++)
							{
								if (num355 == array[num356])
								{
									flag27 = false;
									break;
								}
							}
							if (flag27)
							{
								num352 = num354;
								break;
							}
						}
						if (num352 >= 0)
						{
							int num357 = NPC.NewNPC((int)npc.position.X, (int)num342, 115, npc.whoAmI);
							Main.npc[num357].ai[0] = (float)num352 * 0.1f - 0.05f;
						}
					}
				}
			}
			if (npc.localAI[0] == 1f && Main.netMode != 1)
			{
				npc.localAI[0] = 2f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofT) / 2f;

				int num358 = NPC.NewNPC((int)npc.position.X, (int)num342, 114, npc.whoAmI);
				Main.npc[num358].ai[0] = 1f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofB) / 2f;

				topEye = Main.npc[num358];
				topEye.realLife = npc.whoAmI;

				num358 = NPC.NewNPC((int)npc.position.X, (int)num342, 114, npc.whoAmI);
				Main.npc[num358].ai[0] = -1f;
				num342 = (Main.wofB + Main.wofT) / 2;
				num342 = (num342 + (float)Main.wofB) / 2f;

				bottomEye = Main.npc[num358];
				bottomEye.realLife = npc.whoAmI;

				for (int num359 = 0; num359 < 11; num359++)
				{
					num358 = NPC.NewNPC((int)npc.position.X, (int)num342, 115, npc.whoAmI);
					Main.npc[num358].ai[0] = (float)num359 * 0.1f - 0.05f;
				}
			}
			Player player = Main.player[npc.target];
			topEye.rotation = topEye.DirectionTo(player.Center).ToRotation();
			bottomEye.rotation = bottomEye.DirectionTo(player.Center).ToRotation();
			if (topEye.spriteDirection == -1)
            {
				topEye.rotation += MathHelper.ToRadians(180f);
				bottomEye.rotation += MathHelper.ToRadians(180f);
				topEye.rotation += MathHelper.ToRadians(eyeRotOffest);
				bottomEye.rotation -= MathHelper.ToRadians(eyeRotOffest);
			}
            else
			{
				topEye.rotation -= MathHelper.ToRadians(eyeRotOffest);
				bottomEye.rotation += MathHelper.ToRadians(eyeRotOffest);
			}
			float topEyeRot = topEye.rotation;
			float bottomEyeRot = bottomEye.rotation;
			if (topEye.spriteDirection == -1)
			{
				topEyeRot -= MathHelper.ToRadians(180f);
				bottomEyeRot -= MathHelper.ToRadians(180f);
			}

			if (WoFDoingSpread)
            {
				eyeRotOffest = MathHelper.Lerp(eyeRotOffest, (float)Math.Sin((float)timeAlive / 25f) * 60f, 0.5f);
				WoFAttackCounter++;
				if (WoFAttackCounter > 180)
                {
					WoFAttackCounter = 0;
					WoFDoingSpread = !WoFDoingSpread;
				}
				if (timeAlive % 7 == 6)
				{
					int proj = Projectile.NewProjectile(topEye.Center, topEyeRot.ToRotationVector2() * 7.5f, ProjectileID.GoldenShowerHostile, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(bottomEye.Center, bottomEyeRot.ToRotationVector2() * 15f, ProjectileID.CursedFlameHostile, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
			}
            else
			{
				eyeRotOffest *= 0.94f;

				WoFAttackCounter++;
				if (WoFAttackCounter > (int)MathHelper.Lerp(120f, 500f, (float)npc.life / (float)npc.lifeMax))
				{
					WoFAttackCounter = 0;
					WoFDoingSpread = !WoFDoingSpread;
				}

				int timeBetweenProjectiles = (int)MathHelper.Lerp(45f, 120f, (float)npc.life / (float)npc.lifeMax);
				if (timeAlive % timeBetweenProjectiles == timeBetweenProjectiles - 1)
				{
					int proj = Projectile.NewProjectile(topEye.Center, topEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
					proj = Projectile.NewProjectile(bottomEye.Center, bottomEyeRot.ToRotationVector2() * 10f, ProjectileID.EyeLaser, 60 / 4, 0f, player.whoAmI);
					Main.projectile[proj].tileCollide = false;
				}
			}
		}

		public void WoFEyeAI(NPC npc)
        {
			if (Main.wof < 0)
			{
				npc.active = false;
				return;
			}
			npc.realLife = Main.wof;
			if (Main.npc[Main.wof].life > 0)
			{
				npc.life = Main.npc[Main.wof].life;
			}
			npc.TargetClosest();
			npc.position.X = Main.npc[Main.wof].position.X;
			npc.direction = Main.npc[Main.wof].direction;
			npc.spriteDirection = npc.direction;
			float num360 = (Main.wofB + Main.wofT) / 2;
			num360 = ((!(npc.ai[0] > 0f)) ? ((num360 + (float)Main.wofB) / 2f) : ((num360 + (float)Main.wofT) / 2f));
			num360 -= (float)(npc.height / 2);
			if (npc.position.Y > num360 + 1f)
			{
				npc.velocity.Y = -1f;
			}
			else if (npc.position.Y < num360 - 1f)
			{
				npc.velocity.Y = 1f;
			}
			else
			{
				npc.velocity.Y = 0f;
				npc.position.Y = num360;
			}
			if (npc.velocity.Y > 5f)
			{
				npc.velocity.Y = 5f;
			}
			if (npc.velocity.Y < -5f)
			{
				npc.velocity.Y = -5f;
			}
			Vector2 vector38 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num361 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector38.X;
			float num362 = Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2) - vector38.Y;
			float num363 = (float)Math.Sqrt(num361 * num361 + num362 * num362);
			float num364 = num363;
			num361 *= num363;
			num362 *= num363;
			bool flag28 = true;
			if (npc.direction > 0)
			{
				if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) > npc.position.X + (float)(npc.width / 2))
				{

				}
				else
				{

					flag28 = false;
				}
			}
			else if (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) < npc.position.X + (float)(npc.width / 2))
			{

			}
			else
			{

				flag28 = false;
			}
			if (Main.netMode == 1)
			{
				return;
			}
			int num365 = 4;
			npc.localAI[1] += 1f;
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.75)
			{
				npc.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.5)
			{
				npc.localAI[1] += 1f;
				num365++;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.25)
			{
				npc.localAI[1] += 1f;
				num365 += 2;
			}
			if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
			{
				npc.localAI[1] += 2f;
				num365 += 3;
			}
			if (Main.expertMode)
			{
				npc.localAI[1] += 0.5f;
				num365++;
				if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
				{
					npc.localAI[1] += 2f;
					num365 += 3;
				}
			}
			if (npc.localAI[2] == 0f)
			{
				if (npc.localAI[1] > 600f)
				{
					npc.localAI[2] = 1f;
					npc.localAI[1] = 0f;
				}
			}
			else
			{
				if (!(npc.localAI[1] > 45f) || !Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height))
				{
					return;
				}
				npc.localAI[1] = 0f;
				npc.localAI[2] += 1f;
				if (npc.localAI[2] >= (float)num365)
				{
					npc.localAI[2] = 0f;
				}
				if (flag28)
				{
					float num366 = 9f;
					int num367 = 11;
					int num368 = 83;
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.5)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.25)
					{
						num367++;
						num366 += 1f;
					}
					if ((double)Main.npc[Main.wof].life < (double)Main.npc[Main.wof].lifeMax * 0.1)
					{
						num367 += 2;
						num366 += 2f;
					}
					vector38 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
					num361 = Main.player[npc.target].position.X + (float)Main.player[npc.target].width * 0.5f - vector38.X;
					num362 = Main.player[npc.target].position.Y + (float)Main.player[npc.target].height * 0.5f - vector38.Y;
					num363 = (float)Math.Sqrt(num361 * num361 + num362 * num362);
					num363 = num366 / num363;
					num361 *= num363;
					num362 *= num363;
					vector38.X += num361;
					vector38.Y += num362;
				}
			}
		}

		public void KingSlimeAI(NPC npc)
        {
			float num234 = 1f;
			bool flag8 = false;
			bool flag9 = false;
			npc.aiAction = 0;
			if (npc.ai[3] == 0f && npc.life > 0)
			{
				npc.ai[3] = npc.lifeMax;
			}
			if (npc.localAI[3] == 0f && Main.netMode != 1)
			{
				npc.ai[0] = -100f;
				npc.localAI[3] = 1f;
				npc.TargetClosest();
				npc.netUpdate = true;
			}
			if (Main.player[npc.target].dead)
			{
				npc.TargetClosest();
				if (Main.player[npc.target].dead)
				{
					npc.timeLeft = 0;
					if (Main.player[npc.target].Center.X < npc.Center.X)
					{
						npc.direction = 1;
					}
					else
					{
						npc.direction = -1;
					}
				}
			}
			if (!Main.player[npc.target].dead && npc.ai[2] >= 400f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
			{
				npc.ai[2] = 0f;
				npc.ai[0] = 0f;
				npc.ai[1] = 5f;
				if (Main.netMode != 1)
				{
					npc.TargetClosest(false);
					Point point3 = npc.Center.ToTileCoordinates();
					Point point4 = Main.player[npc.target].Center.ToTileCoordinates();
					Vector2 vector30 = Main.player[npc.target].Center - npc.Center;
					int num235 = 10;
					int num236 = 0;
					int num237 = 7;
					int num238 = 0;
					bool flag10 = false;
					if (vector30.Length() > 500f)
					{
						flag10 = true;
						num238 = 100;
					}
					while (!flag10 && num238 < 100)
					{
						num238++;
						int num239 = Main.rand.Next(point4.X - num235, point4.X + num235 + 1);
						int num240 = Main.rand.Next(point4.Y - num235, point4.Y + 1);
						if ((num240 >= point4.Y - num237 && num240 <= point4.Y + num237 && num239 >= point4.X - num237 && num239 <= point4.X + num237) || (num240 >= point3.Y - num236 && num240 <= point3.Y + num236 && num239 >= point3.X - num236 && num239 <= point3.X + num236) || Main.tile[num239, num240].nactive())
						{
							continue;
						}
						int num241 = num240;
						int num242 = 0;
						if (Main.tile[num239, num241].nactive() && Main.tileSolid[Main.tile[num239, num241].type] && !Main.tileSolidTop[Main.tile[num239, num241].type])
						{
							num242 = 1;
						}
						else
						{
							for (; num242 < 150 && num241 + num242 < Main.maxTilesY; num242++)
							{
								int num243 = num241 + num242;
								if (Main.tile[num239, num243].nactive() && Main.tileSolid[Main.tile[num239, num243].type] && !Main.tileSolidTop[Main.tile[num239, num243].type])
								{
									num242--;
									break;
								}
							}
						}
						num240 += num242;
						bool flag11 = true;
						if (flag11 && Main.tile[num239, num240].lava())
						{
							flag11 = false;
						}
						if (flag11 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
						{
							flag11 = false;
						}
						if (flag11)
						{
							npc.localAI[1] = num239 * 16 + 8;
							npc.localAI[2] = num240 * 16 + 16;
							flag10 = true;
							break;
						}
					}
					Player player = Main.player[npc.target];
					Vector2 bottom = new Vector2(player.Center.X + Math.Sign(player.Center.X - npc.Center.X) * 400f, player.Center.Y - 500f).findGroundUnder(); //Set teleport position
					npc.localAI[1] = bottom.X;
					npc.localAI[2] = bottom.Y;
					DustExplosion(bottom, 0, 40, 25f, DustID.t_Slime, Color.Azure, 2f, true);
					if (num238 >= 100)
					{
					}
				}
			}
			npc.ai[2]++;
			if (Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 320f)
			{
				npc.ai[2]++;
			}
			Dust dust28;
			Dust dust2;
			if (npc.ai[1] == 5f) //Entering teleport
			{
				flag8 = true;
				npc.aiAction = 1;
				npc.ai[0]++;
				num234 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
				num234 = 0.5f + num234 * 0.5f;
				if (npc.ai[0] >= 60f)
				{
					flag9 = true;
				}
				if (npc.ai[0] == 60f)
				{
					Gore.NewGore(npc.Center + new Vector2(-40f, -npc.height / 2), npc.velocity, 734);
				}
				if (npc.ai[0] >= 60f && Main.netMode != 1)
				{
					npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
				}
				if (Main.netMode == 1 && npc.ai[0] >= 120f)
				{
					npc.ai[1] = 6f;
					npc.ai[0] = 0f;
				}
				if (!flag9)
				{
					for (int num244 = 0; num244 < 10; num244++)
					{
						int num245 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
						Main.dust[num245].noGravity = true;
						dust28 = Main.dust[num245];
						dust2 = dust28;
						dust2.velocity *= 0.5f;
					}
				}
			}
			else if (npc.ai[1] == 6f) //Exiting Teleport
			{
				flag8 = true;
				npc.aiAction = 0;
				npc.ai[0]++;
				num234 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
				num234 = 0.5f + num234 * 0.5f;
				if (npc.ai[0] >= 30f && Main.netMode != 1)
				{
					npc.ai[1] = 0f;
					npc.ai[0] = 0f;
					npc.netUpdate = true;
					npc.TargetClosest();
				}
				if (Main.netMode == 1 && npc.ai[0] >= 60f)
				{
					npc.ai[1] = 0f;
					npc.ai[0] = 0f;
					npc.TargetClosest();
				}
				for (int num246 = 0; num246 < 10; num246++)
				{
					int num247 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 4, npc.velocity.X, npc.velocity.Y, 150, new Color(78, 136, 255, 80), 2f);
					Main.dust[num247].noGravity = true;
					dust28 = Main.dust[num247];
					dust2 = dust28;
					dust2.velocity *= 2f;
				}
			}
			npc.dontTakeDamage = (npc.hide = flag9);
			if (npc.velocity.Y == 0f)
			{
				npc.velocity.X *= 0.8f;
				if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
				{
					npc.velocity.X = 0f;
				}
				if (!flag8)
				{
					npc.ai[0] += 4f;
					if ((double)npc.life < (double)npc.lifeMax * 0.8)
					{
						npc.ai[0] += 1f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.6)
					{
						npc.ai[0] += 1f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.4)
					{
						npc.ai[0] += 2f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.2)
					{
						npc.ai[0] += 3f;
					}
					if ((double)npc.life < (double)npc.lifeMax * 0.1)
					{
						npc.ai[0] += 4f;
					}
					if (npc.ai[0] >= 0f)
					{
						npc.netUpdate = true;
						npc.TargetClosest();
						if (npc.ai[1] == 3f) //Big jump
						{
							npc.velocity.Y = -10f;
							npc.velocity.X += 4.5f * (float)npc.direction;
							npc.ai[0] = -200f;
							npc.ai[1] = 0f;

							float intervals = 0.1f;
							float ySpeed = 7.5f;
							if (npc.life <= npc.lifeMax * 0.75f)
                            {
								intervals = 0.075f;
							}
							if (npc.life <= npc.lifeMax * 0.5f)
							{
								intervals = 0.05f;
							}
							if (npc.life <= npc.lifeMax * 0.25f)
							{
								intervals = 0.035f;
								ySpeed = 11f;
							}
							Player player = Main.player[npc.target];
							if (player.Center.Y < npc.Top.Y)
                            {
								ySpeed *= 1.4f;
                            }
							for (float i = -0.5f; i <= 0.5f; i += intervals)
                            {
								float maxSpeedX = 15f;
								Vector2 position = npc.Center + new Vector2(npc.width * i, -npc.height / 2);
								Projectile.NewProjectile(position, new Vector2(maxSpeedX * i * 2f, -ySpeed), ProjectileID.SpikedSlimeSpike, 60 / 4, 0f);
                            }
						}
						else if (npc.ai[1] == 2f) //Horizontal jump/short jump
						{
							npc.velocity.Y = -6f;
							npc.velocity.X += 6f * (float)npc.direction;
							npc.ai[0] = -120f;
							npc.ai[1] += 1f;
						}
						else //Regular jump
						{
							npc.velocity.Y = -8f;
							npc.velocity.X += 5f * (float)npc.direction;
							npc.ai[0] = -120f;
							npc.ai[1] += 1f;
						}
					}
					else if (npc.ai[0] >= -30f)
					{
						npc.aiAction = 1;
					}
				}
			}
			else if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
			{
				if ((npc.direction == -1 && (double)npc.velocity.X < 0.1) || (npc.direction == 1 && (double)npc.velocity.X > -0.1))
				{
					npc.velocity.X += 0.2f * (float)npc.direction;
				}
				else
				{
					npc.velocity.X *= 0.93f;
				}
			}
			int num248 = Dust.NewDust(npc.position, npc.width, npc.height, 4, npc.velocity.X, npc.velocity.Y, 255, new Color(0, 80, 255, 80), npc.scale * 1.2f);
			Main.dust[num248].noGravity = true;
			dust28 = Main.dust[num248];
			dust2 = dust28;
			dust2.velocity *= 0.5f;
			if (npc.life <= 0)
			{
				return;
			}
			float num249 = (float)npc.life / (float)npc.lifeMax;
			num249 = num249 * 0.5f + 0.75f;
			num249 *= num234;
			if (num249 != npc.scale)
			{
				npc.position.X += npc.width / 2;
				npc.position.Y += npc.height;
				npc.scale = num249;
				npc.width = (int)(98f * npc.scale);
				npc.height = (int)(92f * npc.scale);
				npc.position.X -= npc.width / 2;
				npc.position.Y -= npc.height;
			}
			if (Main.netMode == 1)
			{
				return;
			}
			int num250 = (int)((double)npc.lifeMax * 0.05);
			if (!((float)(npc.life + num250) < npc.ai[3]))
			{
				return;
			}
			npc.ai[3] = npc.life;
			int num251 = Main.rand.Next(1, 4);
			for (int num252 = 0; num252 < num251; num252++)
			{
				int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
				int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
				int num253 = 1;
				if (Main.expertMode && Main.rand.Next(4) == 0)
				{
					num253 = 535;
				}
				if (Main.rand.Next(6) == 0)
				{
					num253 = ModContent.NPCType<NPCs.TerrorSlime>();
				}
				int num254 = NPC.NewNPC(x, y, num253);
				Main.npc[num254].SetDefaults(num253);
				Main.npc[num254].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
				Main.npc[num254].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
				Main.npc[num254].ai[0] = -1000 * Main.rand.Next(3);
				Main.npc[num254].ai[1] = 0f;
				if (num253 != ModContent.NPCType<NPCs.TerrorSlime>())
				{
					Main.npc[num254].lifeMax /= 2;
					Main.npc[num254].life = Main.npc[num254].lifeMax;
				}
				if (Main.netMode == 2 && num254 < 200)
				{
					NetMessage.SendData(23, -1, -1, null, num254);
				}
			}
		}

		public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
		{
			float currentAngle = Main.rand.Next(360);

			//if(Main.netMode!=1){
			for (int i = 0; i < Streams; ++i)
			{

				Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
				direction.X *= DustSpeed;
				direction.Y *= DustSpeed;

				Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
				if (NoGravity)
				{
					dust.noGravity = true;
				}
			}
		}

		public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, Color color, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
		{
			float currentAngle = Main.rand.Next(360);

			//if(Main.netMode!=1){
			for (int i = 0; i < Streams; ++i)
			{

				Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
				direction.X *= DustSpeed;
				direction.Y *= DustSpeed;

				Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
				if (NoGravity)
				{
					dust.noGravity = true;
				}
				dust.color = color;
			}
		}

		public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
			if (npc.type == NPCID.BrainofCthulhu && twilight && !BoCSpawnedNewCreepers)
            {
				if (npc.life <= 0)
                {
					npc.life = 1;
                }
            }
        }
    }

	class EoCFlameThrower : ModProjectile
    {
		//259, 270, 271, 6 = potential dust
		public override string Texture => "TerrorbornMod/WhitePixel";

		public override void SetDefaults()
		{
			projectile.width = 35;
			projectile.height = 35;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = false;
			projectile.penetrate = 1;
			projectile.alpha = 255;
			projectile.timeLeft = 70;
		}

		float dustScale = 0f;
        public override void AI()
        {
			if (dustScale < 1f)
            {
				dustScale += 0.05f;
            }
			for (int i = 0; i < 3; i++)
			{
				int type = 0;
				switch (Main.rand.Next(4))
				{
					case 0:
						type = 6;
						break;
					case 1:
						type = 259;
						break;
					case 2:
						type = 270;
						break;
					case 3:
						type = 271;
						break;
				}
				Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, type)];
				dust.noGravity = true;
				dust.color = Color.Red;
				dust.velocity = projectile.velocity * Main.rand.NextFloat();
				dust.scale = Main.rand.NextFloat(2f, 2.5f) * dustScale;
				//if (Main.rand.NextFloat() <= 0.05f) dust.noGravity = false;
			}
        }
    }

	class QueenBeeLaser : ModProjectile
	{
		public override string Texture => "TerrorbornMod/WhitePixel";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			BezierCurve bezier = new BezierCurve();
			bezier.Controls.Clear();
			foreach (Vector2 pos in projectile.oldPos)
			{
				if (pos != Vector2.Zero && pos != null)
				{
					bezier.Controls.Add(pos);
				}
			}

			if (bezier.Controls.Count > 1)
			{
				List<Vector2> positions = bezier.GetPoints(15);
				for (int i = 0; i < positions.Count; i++)
				{
					float mult = (float)(positions.Count - i) / (float)positions.Count;
					Vector2 drawPos = positions[i] - Main.screenPosition + projectile.Size / 2;
					Color color = projectile.GetAlpha(Color.Lerp(Color.DarkSlateBlue, Color.Yellow, mult)) * mult;
					TBUtils.Graphics.DrawGlow_1(spriteBatch, drawPos, (int)(25f * mult), color);
				}
			}
			return false;
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int newDimensions = 15;
			Rectangle oldHitbox = hitbox;
			hitbox.Width = newDimensions;
			hitbox.Height = newDimensions;
			hitbox.X = oldHitbox.X - newDimensions / 2;
			hitbox.Y = oldHitbox.Y - newDimensions / 2;
		}

		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation();
		}
	}
}