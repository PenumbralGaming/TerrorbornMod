using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SearingDaggers : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HellstoneBar, 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires many flaming daggers in an even spread" +
                "\nThese daggers will create weaker versions of themselves on critical hits");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.width = 32;
            item.height = 38;
            item.magic = true;
            item.damage = 20;
            item.useTime = 28;
            item.useAnimation = 28;
            item.mana = 10;
            item.rare = 3;
            item.shoot = mod.ProjectileType("SearingDagger");
            item.shootSpeed = 15;
            item.UseSound = SoundID.Item20;
            item.useStyle = 5;
            item.knockBack = 0.1f;
            item.autoReuse = true;
            item.noMelee = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = new Vector2(speedX, speedY);
            for (int i = -2; i <= 2; i++)
            {
                Projectile.NewProjectile(position, velocity.RotatedBy(MathHelper.ToRadians(10 * i)), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }

    class SearingDagger : ModProjectile
    {
        int FallWait = 40;
        int penetrateUntilStop = 2;

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 3;
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
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 66;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.magic = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * 10);

            penetrateUntilStop--;
            if (penetrateUntilStop <= 0)
            {
                FallWait = 0;
            }

            if (crit && projectile.ai[0] != 1)
            {
                Projectile proj = Main.projectile[Projectile.NewProjectile(originalPosition, originalVelocity, projectile.type, projectile.damage / 2, projectile.knockBack, projectile.owner)];
                proj.ai[0] = 1;
                proj.alpha = 255 / 2;
            }
        }

        bool start = true;
        Vector2 originalVelocity;
        Vector2 originalPosition;
        public override void AI()
        {
            if (start)
            {
                start = false;
                originalVelocity = projectile.velocity;
                originalPosition = projectile.Center;
            }

            if (FallWait > 0)
            {
                FallWait--;
                projectile.spriteDirection = 1;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
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

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            Rectangle originalHitbox = hitbox;
            int newSize = 25;
            hitbox.Width = newSize;
            hitbox.Height = newSize;
            hitbox.X = originalHitbox.Center.X - newSize / 2;
            hitbox.Y = originalHitbox.Center.Y - newSize / 2;
            base.ModifyDamageHitbox(ref hitbox);
        }
    }
}


