using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteBullet : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.width = 8;
            item.height = 16;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 5);
            item.shootSpeed = 20;
            item.rare = 2;
            item.shoot = mod.ProjectileType("AzuriteBulletProjectile");
            item.ammo = AmmoID.Bullet;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A high speed bullet that has a 20% chance to rain azurite shards on hit foes\nThe bullet's speed is unaffected by water");
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"));
            recipe.AddIngredient(ItemID.MusketBall, 111);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
    class AzuriteBulletProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azurite Bullet");
        }
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 2;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Vector2 position = new Vector2(target.position.X + Main.rand.Next(-200, 200), target.position.Y - Main.rand.Next(600, 800));
                float speed = 5f;
                Vector2 velocity = target.DirectionFrom(position) * speed;

                int proj = Projectile.NewProjectile(position.X, position.Y, velocity.X, velocity.Y, ModContent.ProjectileType<Projectiles.AzuriteShard>(), projectile.damage / 2, 0, Main.myPlayer, 0f, 0f);
                Main.projectile[proj].ranged = true;
            }
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
                for (int i = 0; i < Main.rand.Next(2, 4); i++)
                {
                    int Num54 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33, 0, 0, Scale: 1f, newColor: Color.Blue);
                    Main.dust[Num54].noGravity = true;
                    Main.dust[Num54].velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.9f, 1.1f);
                }
            }
        }
    }
}