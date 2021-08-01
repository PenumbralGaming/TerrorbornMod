using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.IO;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
	public class ThunderousThrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.Yoyo[item.type] = true;
			ItemID.Sets.GamepadExtraRange[item.type] = 15;
			ItemID.Sets.GamepadSmartQuickReach[item.type] = true;
			Tooltip.SetDefault("Electrocutes nearby enemies");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
			recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.width = 26;
			item.height = 40;
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 17f;
			item.shoot = ModContent.ProjectileType<ThunderousThrowProjectile>();
			item.knockBack = 2.5f;
			item.damage = 40;
			item.rare = 5;
			item.melee = true;
			item.channel = true;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(silver: 1);
		}

		private static readonly int[] unwantedPrefixes = new int[] { PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy };

		public override bool AllowPrefix(int pre)
		{
			if (Array.IndexOf(unwantedPrefixes, pre) > -1)
			{
				return false;
			}
			return true;
		}
	}

	public class ThunderousThrowProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 12f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 350f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 22f;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
		}

		int projectileWait = 10;
        public override void PostAI()
		{
			NPC target = Main.npc[0];
			bool foundTarget = false;
			float distance = 2000;

			foreach (NPC npc in Main.npc)
			{
				if (npc.Distance(projectile.Center) <= distance && npc.CanBeChasedBy() && projectile.CanHit(npc) && !npc.friendly)
				{
					distance = npc.Distance(projectile.Center);
					target = npc;
					foundTarget = true;
				}
			}

			if (foundTarget)
			{
				projectileWait--;
				if (projectileWait <= 0)
				{
					projectileWait = Main.rand.Next(20, 30);
					Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
					float speed = Main.rand.NextFloat(10f, 25f);

					int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), projectile.damage / 2, 0.5f, projectile.owner);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].ai[0] = target.whoAmI;
				}
			}
		}
    }
}

