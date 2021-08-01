using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Projectiles
{
    class SoulLightning : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

        public override bool? CanHitNPC(NPC target)
        {
            return target.whoAmI == (int)projectile.ai[0];
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.extraUpdates = 200;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1;
            projectile.hide = true;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(projectile.position - (projectile.velocity * i / 4), 1, 1, 62, 0, 0, Scale: 2, newColor: Color.White);
                Main.dust[dust].noGravity = true;

                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.NextFloat(1.5f, 3f);

                Main.dust[dust].velocity = direction * speed;
            }

            NPC target = Main.npc[(int)projectile.ai[0]];
            if (target.active)
            {
                Vector2 direction = projectile.DirectionTo(target.Center);
                float speed = 2f;
                projectile.velocity += speed * direction;

                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(target.Center).ToRotation(), MathHelper.ToRadians(0.75f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                projectile.velocity *= 0.98f;
            }
            else
            {
                projectile.active = false;
            }
        }
    }
}
