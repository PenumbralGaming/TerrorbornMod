using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Projectiles
{
    class VeinBurst : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 110;
        }

        int moveCounter = 10;
        public override void AI()
        {
            if (projectile.ai[0] == 1)
            {
                moveCounter = 0;
            }
            if (moveCounter > 0)
            {
                moveCounter--;
                projectile.position -= projectile.velocity;
            }
            else
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = projectile.velocity;
                projectile.velocity.Y += 0.2f;
                Lighting.AddLight(projectile.Center, 3, 0, 0);
            }
        }
    }
}
