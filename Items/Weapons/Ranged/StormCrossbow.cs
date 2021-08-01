using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class StormCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires faster and faster as you use it" +
                "\nAt maximum firing rate it will additionally fire powerful, infinitely piercing lightning bolts" +
                "\nFires multiple arrows at once");
        }

        public override void SetDefaults()
        {
            item.reuseDelay = 45;
            item.damage = 9;
            item.ranged = true;
            item.width = 64;
            item.height = 36;
            item.channel = true;
            item.useTime = 9;
            item.useAnimation = 9;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = 5;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-3f, -3f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (item.reuseDelay > 0)
            {
                item.reuseDelay -= 2;
                if (item.reuseDelay < 0)
                {
                    item.reuseDelay = 0;
                }
            }
            else
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity.Normalize();
                velocity *= 50;
                Projectile.NewProjectile(position, velocity, ModContent.ProjectileType<LightningBolt>(), damage * 2, knockBack, player.whoAmI);
            }

            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity *= Main.rand.NextFloat(0.8f, 1.2f);
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                Projectile.NewProjectile(position, velocity, type, (int)(damage * 0.4f), knockBack, player.whoAmI);
            }

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                item.reuseDelay = 45;
            }
        }
    }

    class LightningBolt : ModProjectile
    {

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
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 8;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.penetrate = -1;
            projectile.extraUpdates = 4;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity /= projectile.extraUpdates + 1;
            }
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
        }
    }
}

