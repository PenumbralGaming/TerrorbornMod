using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Dunestock
{
    class Dunesting : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("When used with wooden arrows, it rapidly firse a barrage of piercing tumbler\nneedles." +
                "\nWhen used with any other arrow, it fires two arrows at once.");
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.width = 26;
            item.height = 56;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item42;
            item.autoReuse = true;
            item.shoot = 20;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
        }
        public override bool CanUseItem(Player player)
        {
            return base.CanUseItem(player);
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.Next(101) <= 25f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                item.reuseDelay = 0;
                Vector2 spread1 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Vector2 spread2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                int realdamage = (int)(damage * 0.5f);
                Projectile.NewProjectile(position, spread1, ModContent.ProjectileType<TumblerNeedleFriendly>(), realdamage, knockBack, item.owner);
                Projectile.NewProjectile(position, spread2, ModContent.ProjectileType<TumblerNeedleFriendly>(), realdamage, knockBack, item.owner);
            }
            else
            {
                item.reuseDelay = 25;
                float speed = item.shootSpeed;

                Vector2 velocity = (new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition - position).ToRotation().ToRotationVector2() * speed * 1.5f;
                Vector2 speed1 = velocity * 0.5f;
                Vector2 speed2 = velocity * 0.75f;
                Projectile.NewProjectile(position, speed1, type, (int)(damage * 0.9f), knockBack, item.owner);
                Projectile.NewProjectile(position, speed2, type, (int)(damage * 0.9f), knockBack, item.owner);
            }
            return false;
        }
    }
    class TumblerNeedleFriendly : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        bool Stick = false;
        int trueTimeleft = 235;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
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
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 12000;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Stick = true;
            return false;
        }
        public override void AI()
        {
            if (projectile.ai[1] <= 0)
            {
                projectile.alpha += 5;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            if (Stick)
            {
                projectile.velocity *= 0;
                projectile.ai[1]--;
            }
            else
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            }
        }
    }
}
