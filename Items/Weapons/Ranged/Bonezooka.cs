using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Bonezooka : ModItem
    {
        int UntilBlast;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses Rockets as ammo" +
                "\nFires homing skulls that explode on contact with enemies, dealing 70 extra damage to the hit enemy and" +
                "\nnearby enemies");
        }
        public override void SetDefaults()
        {
            item.damage = 63;
            item.ranged = true;
            item.noMelee = true;
            item.width = 58;
            item.height = 26;
            item.useTime = 25;
            item.shoot = 10;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Rocket;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = ModContent.ProjectileType<BonezookaSkull>();
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
    }
    class BonezookaSkull : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 36;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 45, 15, 172, DustScale: 1f, NoGravity: true);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && projectile.Distance(npc.Center) <= 100 + ((npc.width + npc.height) / 2) && !npc.dontTakeDamage)
                {
                    npc.StrikeNPC(70, 0, 0, Main.rand.Next(101) <= Main.player[projectile.owner].rangedCrit);
                }
            }
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
        public override void AI()
        {
            if (projectile.velocity.X <= 0)
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            NPC targetNPC = Main.npc[0];
            float Distance = 500; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = .6f;
                Vector2 move = targetNPC.Center - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
                projectile.velocity *= 0.98f;
            }
            Dust dust = Dust.NewDustPerfect(projectile.Center, 172, Vector2.Zero, 0, Scale: 0.8f);
            dust.noGravity = true;
        }
        int bouncesLeft = 3;
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
                projectile.velocity.Y = -oldVelocity.Y;
            }

            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            bouncesLeft--;
            if (bouncesLeft <= 0)
            {
                projectile.timeLeft = 0;
            }
            return false;
        }
    }
}
