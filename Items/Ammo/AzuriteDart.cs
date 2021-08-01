using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class AzuriteDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a weaker but piercing clone of itself upon hitting an enemy");
        }
        public override void SetDefaults()
        {
            item.damage = 11;
            item.ranged = true;
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1;
            item.shootSpeed = 2;
            item.rare = 2;
            item.shoot = mod.ProjectileType("AzuriteDartProjectile");
            item.ammo = AmmoID.Dart;
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.AzuriteBar>());
            recipe.AddIngredient(ModContent.ItemType<WoodDart>(), 111);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
    class AzuriteDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/AzuriteDart";
        public override void SetStaticDefaults()
        {
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
            projectile.width = 14;
            projectile.height = 22;
            projectile.ranged = true;
            projectile.timeLeft = 1000;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.extraUpdates = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }
        int DustCooldown = 69;
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
            projectile.velocity.Y += 0.03f;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            if (projectile.ai[0] > 0)
            {
                projectile.alpha = 255 / 2;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.ai[0] > 0)
            {
                return;
            }
            int proj = Projectile.NewProjectile(originalPosition, originalVelocity, projectile.type, projectile.damage / 4, projectile.knockBack / 4, projectile.owner);
            Main.projectile[proj].ai[0] = 1;
            Main.projectile[proj].penetrate = 3;
        }
    }
}

