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
	internal class FrightcrawlerHead : Frightcrawler
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.width = 62;
			npc.height = 86;
			npc.aiStyle = -1;
			head = true;
		}

		float movementDirection = 0f;
		float movementSpeed = 5f;
		int AIPhase = 0;
		int phaseTimeLeft = 60 * 5;
		int projectileCounter = 60;
		public override void actualAI()
		{
			npc.TargetClosest(false);
			Player target = Main.player[npc.target];
			if (target.dead || !TerrorbornWorld.terrorRain)
            {
				npc.velocity.Y += 0.2f;
				if (npc.Center.Y >= target.Center.Y + 10000)
                {
					npc.active = false;
                }
				return;
            }
			npc.velocity = movementSpeed * movementDirection.ToRotationVector2();
			if (AIPhase == 0)
            {
				movementSpeed = MathHelper.Lerp(movementSpeed, 12f, 0.07f);
				movementDirection = movementDirection.AngleLerp(npc.DirectionTo(target.Center + (npc.Distance(target.Center) / movementSpeed) * target.velocity).ToRotation(), 0.03f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					phaseTimeLeft = 60 * 3;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0, 1, 0.5f);
					TerrorbornMod.ScreenShake(10);
					AIPhase++;
                }

				projectileCounter--;
				if (projectileCounter <= 0)
                {
					projectileCounter = 45;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, npc.Center);
					float speed = 17f;
					Vector2 velocity = (npc.rotation - MathHelper.ToRadians(90)).ToRotationVector2() * speed;
					Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
                }
            }
			if (AIPhase == 1)
			{
				movementDirection = movementDirection.AngleLerp(npc.DirectionTo(target.Center + (npc.Distance(target.Center) / movementSpeed) * target.velocity).ToRotation(), 0.01f);
				movementSpeed = MathHelper.Lerp(movementSpeed, 3f, 0.05f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					AIPhase++;
					phaseTimeLeft = 60 * 5;
					Terraria.Audio.SoundEngine.PlaySound(4, (int)npc.Center.X, (int)npc.Center.Y, 10, 1, -0.3f);
					TerrorbornMod.ScreenShake(20);
				}
			}
			if (AIPhase == 2)
			{
				movementSpeed = MathHelper.Lerp(movementSpeed, 20f, 0.05f);
				movementDirection = movementDirection.AngleLerp(npc.DirectionTo(target.Center).ToRotation(), 0.02f);
				phaseTimeLeft--;
				if (phaseTimeLeft <= 0)
                {
					AIPhase = 0;
					phaseTimeLeft = 60 * 7;
				}

				projectileCounter--;
				if (projectileCounter <= 0)
				{
					projectileCounter = 90;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath13, npc.Center);
					float speed = 20f;
					Vector2 velocity = npc.DirectionTo(target.Center + target.velocity * (npc.Distance(target.Center) / speed)) * speed;
					Projectile.NewProjectile(npc.Center, velocity, ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
					Projectile.NewProjectile(npc.Center, velocity.RotatedBy(-MathHelper.ToRadians(30)), ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
					Projectile.NewProjectile(npc.Center, velocity.RotatedBy(MathHelper.ToRadians(30)), ModContent.ProjectileType<Projectiles.FrightfulSpit>(), 75 / 4, 0);
				}
			}
		}
	}

	internal class FrightcrawlerBody : Frightcrawler
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 58;
			npc.height = 32;
			body = true;
			npc.defense = 9999;
		}

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = 1;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			damage = 1;
        }
    }

	internal class FrightcrawlerTail : Frightcrawler
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.aiStyle = -1;
			npc.width = 34;
			npc.height = 32;
			npc.defense = 9999;
			tail = true;
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = 1;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			damage = 1;
		}
	}

	public abstract class Frightcrawler : BaseWorm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frightcrawler");
			NPCID.Sets.TrailCacheLength[npc.type] = 1;
			NPCID.Sets.TrailingMode[npc.type] = 1;
		}

        public override void SetDefaults()
		{
			bodySegmentCount = 40;
			tailType = ModContent.NPCType<FrightcrawlerTail>();
			bodyType = ModContent.NPCType<FrightcrawlerBody>();
			headType = ModContent.NPCType<FrightcrawlerHead>();
			npc.lifeMax = 4500;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.damage = 40;
			npc.friendly = false;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.defense = 25;
			npc.knockBackResist = 0f;

			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
			npc.buffImmune[BuffID.Ichor] = true;
			npc.buffImmune[BuffID.ShadowFlame] = true;
			npc.buffImmune[BuffID.Poisoned] = true;
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
				spriteBatch.Draw(ModContent.GetTexture(Texture + "_Glow"), drawPos, npc.frame, color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
		}

		public override void NPCLoot()
		{
			TerrorbornWorld.downedFrightcrawler = true;

			TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.LocalPlayer);
			if (TerrorbornWorld.obtainedShriekOfHorror)
			{
				if (modPlayer.DeimosteelCharm)
				{
					Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.NoxiousScale>(), Main.rand.Next(8, 13));
				}
				else
				{
					Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.TerrorSample>(), Main.rand.Next(4, 7));
				}
			}

			Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Materials.NoxiousScale>(), Main.rand.Next(15, 21));

			if (Main.rand.NextFloat() <= 0.5f)
			{
				Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Accessories.SoulEater>());
			}
		}
    }
}
