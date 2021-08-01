using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.PrototypeI
{
    class PlasmoditeShotgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Rapidly fires bursts of plasma crystals" +
                "\nPartially ignores defense");
        }
        public override void SetDefaults()
        {
            item.damage = 35;
            item.ranged = true;
            item.noMelee = true;
            item.width = 60;
            item.height = 22;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 16, 0, 0);
            item.rare = 9;
            item.autoReuse = true;
            item.shootSpeed = 4f;
            item.UseSound = SoundID.Item61;
            item.shoot = ModContent.ProjectileType<PlasmaCrystal>();
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(5, 7); i++)
            {
                Vector2 EditedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(13)) * Main.rand.NextFloat(0.8f, 1.3f);
                Projectile.NewProjectile(position, EditedSpeed, type, damage, knockBack, item.owner);
            }
            return false;
        }
    }
    class PlasmaCrystal : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 18;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = false;
            projectile.extraUpdates = 8;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 16;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(mod.GetTexture("Items/PrototypeI/PlasmaCrystal_Glow"), drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            //spriteBatch.Draw(glowTexture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(glowTexture.Width / 2, glowTexture.Height / 2), 1f, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return false;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item27, projectile.position);
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(180);
        }
    }
}

