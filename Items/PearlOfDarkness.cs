using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class PearlOfDarkness : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Use to point to where the next terror ability is for 10 seconds");
        }

        public override void SetDefaults()
        {
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<PearlOfDarknessProjectile>();
            item.maxStack = 999;
            item.noUseGraphic = true;
            item.consumable = true;
        }

        public override bool CanUseItem(Player player)
        {
            bool canSpawn = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == item.shoot && proj.active)
                {
                    canSpawn = false;
                    break;
                }
            }
            return canSpawn;
        }
    }

    class PearlOfDarknessProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 26;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 60 * 10;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, 0, projectile.width, projectile.height), Color.White, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        Vector2 targetPosition()
        {
            Vector2 positionOfProjectile = Vector2.Zero;
            bool foundProjectile = false;

            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<Abilities.NecromanticCurse>() && proj.active)
                {
                    positionOfProjectile = proj.Center;
                    foundProjectile = true;
                    break;
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.HorrificAdaptation>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.VoidBlink>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }
            if (!foundProjectile)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.type == ModContent.ProjectileType<Abilities.TerrorWarp>() && proj.active)
                    {
                        positionOfProjectile = proj.Center;
                        foundProjectile = true;
                        break;
                    }
                }
            }

            return positionOfProjectile;
        }


        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            projectile.position = player.Center + new Vector2(0, -50);
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);

            if (targetPosition() == Vector2.Zero)
            {
                return;
            }
            else
            {
                projectile.rotation = projectile.DirectionTo(targetPosition()).ToRotation() + MathHelper.ToRadians(90);
            }
        }
    }
}

