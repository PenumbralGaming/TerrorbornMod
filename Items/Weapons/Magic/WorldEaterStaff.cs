using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class WorldEaterStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.DemoniteBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            DisplayName.SetDefault("Toothache staff");
            Tooltip.SetDefault("Fires multiple eater's tooth projectiles" +
                "\nIgnites hit enemies with cursed flames on critical hits");
        }
        public override void SetDefaults()
        {
            item.damage = 14;
            item.noMelee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item17;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("WorldEaterTooth");
            item.shootSpeed = 10f;
            item.mana = 6;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 3; i++)
            {
                position = player.Center + (player.DirectionTo(Main.MouseWorld) * 40);
                Vector2 velocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }

        class WorldEaterTooth : ModProjectile
        {
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
                    Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
                }
                return false;
            }

            public override void SetDefaults()
            {
                projectile.width = 14;
                projectile.height = 20;
                projectile.aiStyle = -1;
                projectile.friendly = true;
                projectile.penetrate = 3;
                projectile.magic = true;
                projectile.hide = false;
                projectile.tileCollide = true;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = -1;
            }
            public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            {
                if (crit)
                {
                    target.AddBuff(BuffID.CursedInferno, 60 * 3);
                }
            }

            public override void AI()
            {
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
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
                int newSize = 14;
                hitbox.Width = newSize;
                hitbox.Height = newSize;
                hitbox.X = originalHitbox.Center.X - newSize / 2;
                hitbox.Y = originalHitbox.Center.Y - newSize / 2;
                base.ModifyDamageHitbox(ref hitbox);
            }
        }
    }
}
