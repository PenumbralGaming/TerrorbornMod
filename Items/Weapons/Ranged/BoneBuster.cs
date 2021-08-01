using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class BoneBuster : ModItem
    {
        int UntilBlast;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Carpal Buster");
            Tooltip.SetDefault("Uses Cartilage Rounds as ammo" +
                "\n95% chance to not consume ammo" +
                "\n'Shoots as fast as your fingers can click'");
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(100) <= 5;
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 3;
            item.useAnimation = 3;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item41;
            item.autoReuse = false;
            item.shootSpeed = 16f;
            item.shoot = mod.ProjectileType("CartilageRoundProjectile");
            item.useAmmo = mod.ItemType("CartilageRound");
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 RotatedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6));
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
    class CartilageRound : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 0;
            item.ranged = true;
            item.width = 12;
            item.height = 14;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = 10;
            item.shootSpeed = 20;
            item.rare = 2;
            item.shoot = mod.ProjectileType("CartilageRoundProjectile");
            item.ammo = item.type;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Used by the bone buster" +
                "\nBounces off of tiles and pierces multiple enemies");
        }
    }
    class CartilageRoundProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/CartilageRound";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cartilage Round");
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 14;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.penetrate = -1;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
        }
        int BouncesLeft = 5;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
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

            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            BouncesLeft--;
            if (BouncesLeft <= 0)
            {
                projectile.active = false;
            }
            return false;
        }
        
    }
}