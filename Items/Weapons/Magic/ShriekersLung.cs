using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class ShriekersLung : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shrieker's Lung");
            Tooltip.SetDefault("Fires bouncing sound waves that home into confused enemies" +
                "\nHas a chance to confuse enemies for 5 seconds");
        }

        public override void SetDefaults()
        {
            item.damage = 12;
            item.noMelee = true;
            item.width = 44;
            item.height = 26;
            item.useTime = 24;
            item.rare = 1;
            item.useAnimation = 24;
            item.useStyle = 5;
            item.knockBack = 0.0001f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.autoReuse = true;
            item.shootSpeed = 10f;
            item.shoot = ModContent.ProjectileType<ShriekWave>();
            item.mana = 3;
            item.magic = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Audio.SoundEngine.PlaySound(4, (int)player.Center.X, (int)player.Center.Y, 4, 1, -0.6f);
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class ShriekWave : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i += 2)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 60;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        int collideCounter = 4;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < projectile.localNPCImmunity.Length; i++)
            {
                if (projectile.localNPCImmunity[i] < 0 || projectile.localNPCImmunity[i] > 5)
                {
                    projectile.localNPCImmunity[i] = 5;
                }
            }
            collideCounter--;
            if (collideCounter <= 0)
            {
                projectile.timeLeft = 0;
            }
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
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextFloat() <= 0.15f)
            {
                target.AddBuff(BuffID.Confused, 60 * 5);
            }
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            NPC targetNPC = Main.npc[0];
            float Distance = 700; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && Main.npc[i].HasBuff(BuffID.Confused))
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(3.5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            }
        }
    }
}