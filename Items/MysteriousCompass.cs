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
    class MysteriousCompass : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("You feel emotionally attached to it somehow..." +
                "\n[c/FF1919:Points to something important]");
        }

        public override void SetDefaults()
        {
            item.rare = -11;
            item.accessory = true;
        }

        public override void HoldItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.MysteriousCompass = true;
            bool arrowExists = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<CompassPointer>() && projectile.active)
                {
                    arrowExists = true;
                    break;
                }
            }
            if (!arrowExists)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<CompassPointer>(), 0, 0, player.whoAmI);
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.MysteriousCompass = true;
            bool arrowExists = false;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<CompassPointer>() && projectile.active)
                {
                    arrowExists = true;
                    break;
                }
            }
            if (!arrowExists)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<CompassPointer>(), 0, 0, player.whoAmI);
            }
        }
    }

    class CompassPointer : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 36;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.timeLeft = 500;
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
            return TerrorbornWorld.ShriekOfHorror;
        }


        public override void AI()
        {
            projectile.timeLeft = 500;

            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (!modPlayer.MysteriousCompass)
            {
                projectile.active = false;
            }

            projectile.position = player.Center + new Vector2(0, -65);
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
