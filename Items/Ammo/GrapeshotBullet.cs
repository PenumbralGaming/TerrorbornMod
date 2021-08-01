using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class GrapeshotBullet : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 11;
            item.ranged = true;
            item.width = 14;
            item.height = 14;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 5);
            item.shootSpeed = 20;
            item.rare = 5;
            item.shoot = mod.ProjectileType("GrapeshotBulletSpawn");
            item.ammo = AmmoID.Bullet;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires multiple bullets at once");
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>());
            recipe.AddIngredient(ItemID.EmptyBullet, 111);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }

    class GrapeshotBulletSpawn : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/GrapeshotBullet";

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 14;
            projectile.ranged = true;
            projectile.timeLeft = 1;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.hide = true;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 velocity = projectile.velocity;
                velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                Projectile.NewProjectile(projectile.Center, velocity.RotatedByRandom(MathHelper.ToRadians(15)), ModContent.ProjectileType<GrapeshotBulletProjectile>(), (int)(projectile.damage * 0.45f), projectile.knockBack, projectile.owner);
            }
        }
    }

    class GrapeshotBulletProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 2;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.hide = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, 0, projectile.width, projectile.height), Color.White, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            projectile.hide = false;
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            }
        }
    }
}