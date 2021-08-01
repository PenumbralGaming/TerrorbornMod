using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Projectiles
{
    class AzuriteShard : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, Scale: 1.35f, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity / 3;

            projectile.velocity.Y += 1.5f;
            projectile.velocity.X *= 0.98f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X != oldVelocity.X)
            {
                projectile.position.X = projectile.position.X + projectile.velocity.X;
                projectile.velocity.X = -oldVelocity.X;
            }
            if (projectile.velocity.Y != oldVelocity.Y)
            {
                projectile.position.Y = projectile.position.Y + projectile.velocity.Y;
                projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}
