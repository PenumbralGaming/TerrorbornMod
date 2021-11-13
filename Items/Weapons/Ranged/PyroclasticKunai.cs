﻿using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using TerrorbornMod.Items.Materials;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class PyroclasticKunai : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 15;
            item.ranged = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 21;
            item.useAnimation = 21;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item39;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 25;
            item.shoot = ModContent.ProjectileType<PyroclasticKunaiProjectile>();
        }

        float maxRotation = MathHelper.ToRadians(10f);
        int kunaiCount = 5;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = -2; i <= 2; i++)
            {
                Vector2 velocity = new Vector2(speedX, speedY).RotatedBy((maxRotation / kunaiCount) * i);
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<PyroclasticGemstone>(), 15);
            recipe.AddIngredient(ModContent.ItemType<IncendiusAlloy>(), 10);
            recipe.AddIngredient(ItemID.ThrowingKnife, 50);
            recipe.AddTile(ModContent.TileType<Tiles.Incendiary.IncendiaryAltar>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class PyroclasticKunaiProjectile : ModProjectile
    {
        int FallWait = 40;
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 38;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                bool spawnPortal = true;
                foreach (Projectile projectile in Main.projectile)
                {
                    if (projectile.type == ModContent.ProjectileType<PyroclasticPortal>() && projectile.active)
                    {
                        spawnPortal = false;
                        break;
                    }
                }

                if (spawnPortal)
                {
                    Player player = Main.player[projectile.owner];
                    Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<PyroclasticPortal>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);
            }
            else
            {
                projectile.velocity *= 0.9f;
                projectile.alpha += 255 / 20;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Main.PlaySound(0, projectile.position);
        }
    }

    class PyroclasticPortal : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.timeLeft = trueTimeLeft + 300;
        }

        float maxRotation = MathHelper.ToRadians(10f);
        int kunaiCount = 3;
        int trueTimeLeft = 120;
        int projectileWait = 30;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.rotation -= MathHelper.ToRadians(5f);

            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
                if (player.itemTime > 0)
                {
                    projectileWait--;
                    if (projectileWait <= 0)
                    {
                        projectileWait = 30;
                        for (int i = -1; i <= 1; i++)
                        {
                            Vector2 velocity = (projectile.DirectionTo(Main.MouseWorld) * 25).RotatedBy((maxRotation / kunaiCount) * i);
                            Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<PyroclasticKunaiProjectile>(), projectile.damage, projectile.knockBack, player.whoAmI);
                        }
                    }
                }
            }
            else
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
    }
}
