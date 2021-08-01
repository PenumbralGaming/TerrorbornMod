using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.PrototypeI
{
    class PlasmaticVortex : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Slowly returns to you upon hitting an enemy, dealing extra hits");
        }
        public override void SetDefaults()
        {
            item.damage = 118;
            item.width = 102;
            item.height = 82;
            item.useTime = 30;
            item.useAnimation = 30;
            item.rare = 9;
            item.useStyle = 1;
            item.knockBack = 0f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 16, 0, 0);
            item.shootSpeed = 50;
            item.shoot = mod.ProjectileType("PlasmaticVortex_projectile");
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.melee = true;
        }
    }
    class PlasmaticVortex_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/PrototypeI/PlasmaticVortex"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
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
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/PrototypeI/PlasmaticVortex"), drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
                color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(ModContent.GetTexture("TerrorbornMod/Items/PrototypeI/PlasmaticVortex_Glow"), drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return true;
        }

        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 102;
            projectile.height = 82;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            TimeUntilReturn = 0;
            speed = 0;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (TimeUntilReturn > 0)
            {
                TimeUntilReturn = 0;
                speed = 0;
            }
        }

        int TimeUntilReturn = 22;
        float speed = 0;
        bool Start = true;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.spriteDirection = player.direction * -1;
            projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                projectile.tileCollide = false;
                Vector2 direction = projectile.DirectionTo(player.Center);
                speed += 0.3f;
                projectile.velocity = direction * speed;

                if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
                {
                    projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
                if (TimeUntilReturn <= 0)
                {
                    speed = projectile.velocity.Length();
                }
            }
        }
    }
}

