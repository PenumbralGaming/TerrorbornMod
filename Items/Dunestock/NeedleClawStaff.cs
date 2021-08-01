using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Dunestock
{
    class NeedleClawStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.damage = 34;
            item.noMelee = true;
            item.width = 54;
            item.height = 56;
            item.useTime = 22;
            item.shoot = 10;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Claw");
            item.shootSpeed = 15f;
            item.mana = 4;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 mouse = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            position = player.Center + (player.DirectionTo(mouse) * 65);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner);
            return false;
        }
    }
    class Claw : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/DuneClaw";
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.timeLeft = 90;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

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
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        int CollideCounter = 0;
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
            //Terraria.Audio.SoundEngine.PlaySound(SoundID.Run, projectile.Center);
            CollideCounter += 1;
            if (CollideCounter >= 5)
            {
                projectile.timeLeft = 0;
            }
            return false;
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, projectile.Center);
                float ProjSpeed = 25f;
                Vector2 vector8 = new Vector2(projectile.position.X + (projectile.width / 2), projectile.position.Y + (projectile.height / 2));
                int damage = projectile.damage / 6; // /2 undoes the expert increase
                int type = mod.ProjectileType("MagicNeedle");
                float rotation = (float)Math.Atan2(vector8.Y - mousePosition.Y, vector8.X - mousePosition.X);
                Vector2 speed = new Vector2((float)((Math.Cos(rotation) * ProjSpeed) * -1), (float)((Math.Sin(rotation) * ProjSpeed) * -1)).RotatedByRandom(MathHelper.ToRadians(1));
                Projectile.NewProjectile(vector8.X, vector8.Y, speed.X, speed.Y, type, damage, projectile.knockBack, projectile.owner, 0, 3);
            }
            projectile.active = false;
        }
        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
    }
    class MagicNeedle : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        bool Stick = false;
        int trueTimeleft = 235;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.magic = true;
            projectile.ignoreWater = false;
            projectile.tileCollide = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 12000;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Stick = true;
            return false;
        }
        public override void AI()
        {
            if (projectile.timeLeft == 12000)
            {
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
            }
            if (projectile.ai[1] <= 0)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            if (Stick)
            {
                projectile.velocity *= 0;
                projectile.ai[1]--;
            }
            else
            {
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
                if (projectile.ai[0] == 1 && !Stick)
                {
                    projectile.velocity.Y += 0.3f;
                }
            }
        }
    }
}

