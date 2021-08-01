using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Converters;
using Steamworks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Abilities
{
    class TerrorAbility : ModProjectile
    {
        public override string Texture => "TerrorbornMod/" + TexturePath;
        public virtual string TexturePath => "Abilities/ShriekOfHorror_Icon";
        public override void SetDefaults()
        {
            projectile.damage = 0;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.timeLeft = 500;
            projectile.penetrate = -1;
            projectile.light = 0.2f;
            projectile.width = (int)dimensions().X;
            projectile.height = (int)dimensions().Y;
            projectile.scale = getScale();
        }

        public virtual float getScale()
        {
            return 2f;
        }

        public virtual Vector2 dimensions()
        {
            return new Vector2(20, 24);
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;
        }

        public virtual bool HasLockedPosition()
        {
            return true;
        }

        public virtual Vector2 lockedPosition()
        {
            return Vector2.Zero;
        }

        public virtual Vector2 baseOffsets()
        {
            return new Vector2(12, 0);
        }

        public virtual void ActualAI()
        {

        }

        Vector2 positionOffset = Vector2.Zero;
        int offsetDirection = 1;
        float offsetSpeed = 0;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture(TexturePath);
            spriteBatch.Draw(texture, projectile.Center + positionOffset + baseOffsets() - Main.screenPosition, new Rectangle(0, 0, (int)(texture.Width), (int)(texture.Height)), Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public virtual void UpdateObtainablity(float maxDistance = 30)
        {
            if (Main.player[Player.FindClosest(projectile.Center + positionOffset + baseOffsets(), 0, 0)].Distance(projectile.Center + positionOffset + baseOffsets()) <= maxDistance)
            {
                ObtainAbility();
            }
        }

        public virtual void ObtainAbility()
        {

        }

        public virtual void Float(float maxSpeed = 3, float accelerationSpeed = 0.25f)
        {
            offsetSpeed += offsetDirection * accelerationSpeed;
            if ((offsetSpeed > maxSpeed && offsetDirection == 1) || (offsetSpeed < -maxSpeed && offsetDirection == -1))
            {
                offsetDirection *= -1;
            }
            positionOffset.Y += offsetSpeed;
        }

        public override void AI()
        {
            projectile.timeLeft = 500;
            ActualAI();
            if (HasLockedPosition())
            {
                projectile.position = (lockedPosition()) - new Vector2(projectile.width, projectile.height);
            }
            //projectile.position += positionOffset;

            int dust = Dust.NewDust(projectile.position + positionOffset + baseOffsets(), (int)(projectile.width), (int)(projectile.height), 91, 0, 0);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = Vector2.Zero;
        }
    }
}
