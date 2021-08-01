using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class HemorrhageStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Fires a crimson heart that bounces around" +
                "\nThis heart will explode into blood after 4 seconds or after hitting an enemy");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.noMelee = true;
            item.width = 34;
            item.height = 42;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.NPCDeath13;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("CrimsonHeartBomb");
            item.shootSpeed = 15f;
            item.mana = 6;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 42);
            return true;
        }
    }

    class CrimsonHeartBomb : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 240;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 18;
            height = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < projectile.localNPCImmunity.Length; i++)
            {
                if (projectile.localNPCImmunity[i] < 0 || projectile.localNPCImmunity[i] > 5)
                {
                    projectile.localNPCImmunity[i] = 5;
                }
            }
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 15)
            {
                projectile.velocity.Y = 15;
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath21, projectile.Center);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 15);
                int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), projectile.damage / 2, 0f, projectile.owner);
                Main.projectile[proj].magic = true;
                Main.projectile[proj].ai[0] = 1;
            }
        }
    }
}

