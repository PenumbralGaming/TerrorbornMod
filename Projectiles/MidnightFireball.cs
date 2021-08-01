using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Projectiles
{
    public class MidnightFireball : ModProjectile
    {
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Debuffs.MidnightFlamesDebuff>(), 60 * 3);
            projectile.timeLeft = 1;
        }
        //private bool HasGravity = true;
        private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 40;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 69;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.timeLeft = (int)projectile.ai[0];
            }

            projectile.rotation += MathHelper.ToRadians(5);
            Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * 0.80f * Main.essScale);

            //int dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 74);
            //Main.dust[dust].velocity = projectile.velocity;

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 74);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;
            Main.dust[dust].velocity = projectile.velocity;


            if (projectile.velocity != Vector2.Zero)
            {
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)].Center).ToRotation(), MathHelper.ToRadians(projectile.ai[1] * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            }

            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frameCounter = 5;
                projectile.frame++;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 12, 7, 74, 2f, true);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}

