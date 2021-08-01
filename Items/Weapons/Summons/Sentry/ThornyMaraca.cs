using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Summons.Sentry
{
    class ThornyMaraca : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a thorny companion to help you fight." +
                "\nSummoning it despawns ALL other sentries but its damage scales with sentry slots.");
        }
        public override void SetDefaults()
        {
            item.mana = 10;
            item.summon = true;
            item.damage = 22;
            item.width = 38;
            item.sentry = true;
            item.height = 42;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 2;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("MaractusSentry");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].sentry)
                    {
                        Main.projectile[i].active = false;
                    }
                }
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
            }
            return false;
        }

        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
    }
    class PinMissle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 10;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 2;
            ProjectileID.Sets.Homing[projectile.type] = true;
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
            projectile.extraUpdates = 1;
            projectile.width = 10;
            projectile.height = 12;
            projectile.penetrate = 5;
            projectile.friendly = true;
            projectile.ignoreWater = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.HasBuff(BuffID.Oiled))
            {
                damage = (int)(damage * 1.3f);
            }
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
        }
    }
    class MaractusSentry : ModProjectile
    {
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 12;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true; //This is necessary for right-click targeting
        }
        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.width = 66;
            projectile.height = 66;
            projectile.friendly = true;
            projectile.sentry = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 10;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 6;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        int PinWait = 60;
        int PinRoundsLeft = 2;
        public override void AI()
        {
            Vector2 position = new Vector2(projectile.Center.X, projectile.position.Y);
            while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                {
        new Conditions.IsSolid()
                }), out _))
            {
                position.Y++;
            }
            position.Y -= projectile.height - 4;
            projectile.position = new Vector2(position.X - projectile.width / 2, position.Y);
            FindFrame(projectile.height);
            projectile.timeLeft = 10;
            bool Targeted = false;
            //projectile.velocity.Y = 50;
            //projectile.velocity.X = 0;
            Player player = Main.player[projectile.owner];
            NPC target = Main.npc[0];
            if (player.HasMinionAttackTargetNPC && Main.npc[player.MinionAttackTargetNPC].Distance(projectile.Center) < 1500)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
                Targeted = true;
            }
            else
            {
                float Distance = 1500;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                    {
                        target = Main.npc[i];
                        Distance = Main.npc[i].Distance(projectile.Center);
                        Targeted = true;
                    }
                }
            }
            if (Targeted)
            {

                PinWait--;
                if (PinWait <= 0)
                {
                    if (PinRoundsLeft > 0)
                    {
                        PinRoundsLeft--;
                        PinWait = 10;
                    }
                    else
                    {
                        PinWait = 100;
                        PinRoundsLeft = Main.rand.Next(1, 5);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item42, projectile.Center);
                    Vector2 Rotation = projectile.DirectionTo(target.Center);
                    float Speed = 20;
                    Vector2 Velocity = Rotation * Speed;
                    Projectile.NewProjectile(projectile.Center, Velocity, ModContent.ProjectileType<PinMissle>(), projectile.damage * player.maxTurrets, projectile.knockBack, projectile.owner);
                }
            }
        }
    }
}
