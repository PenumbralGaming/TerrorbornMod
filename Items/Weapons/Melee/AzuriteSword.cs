using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class AzuriteSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires azurite waves as it is swung");
        }
        public override void SetDefaults()
        {
            item.damage = 27;
            item.melee = true;
            item.width = 36;
            item.height = 48;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 1;
            item.knockBack = 9f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 23;
            item.shoot = mod.ProjectileType("AzuriteSlash");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            counter = 0;
            return false;
        }

        int counter = 0;
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Vector2 direction = player.itemRotation.ToRotationVector2();
            Vector2 velocity = item.shootSpeed * direction;
            velocity *= player.direction;
            counter--;
            if (counter <= 0)
            {
                Projectile.NewProjectile(player.Center + direction * 15, velocity, item.shoot, (int)(item.damage * 0.65f), item.knockBack, player.whoAmI);
                counter = 3;
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    class AzuriteSlash : ModProjectile
    {
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
            projectile.width = 30;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 1000;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }
        int timeUntilFade = 60;
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            projectile.velocity *= 0.98f;
            timeUntilFade--;
            if (timeUntilFade <= 0)
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