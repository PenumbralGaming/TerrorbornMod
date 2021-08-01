using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Events;
using Terraria.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.UI;

namespace TerrorbornMod.Items.Weapons.Summons.Other
{
    class Armagrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A grenade that releases little mothrons upon exploding" +
                "\nThese mothrons take up no summon slots but disappear after a short amount of time" +
                "\nYour minions will target enemies hit by the main grenade");
        }
        public override void SetDefaults()
        {
            item.damage = 36;
            item.summon = true;
            item.useTime = 26;
            item.useAnimation = 26;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 65);
            item.rare = 5;
            item.UseSound = SoundID.Item106;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 22;
            item.mana = 10;
            item.shoot = ModContent.ProjectileType<ArmagrenadeProj>();
        }
    }

    class ArmagrenadeProj : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Summons/Other/Armagrenade";
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.75f);
        }
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 16;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
            projectile.velocity.Y += 0.18f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.player[projectile.owner].MinionAttackTargetNPC = target.whoAmI;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath46, projectile.Center);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(projectile.Center, ProjectileSpeed, ModContent.ProjectileType<LittleMothron>(), (int)(projectile.damage * 0.75f), projectile.knockBack, projectile.owner);
            }
        }
    }
    class LittleMothron : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 16;
            projectile.tileCollide = true;
            projectile.friendly = false;
            projectile.penetrate = 4;
            projectile.hostile = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 1000;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 3;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        int timeUntilDeadly = 60;
        int trueTimeLeft = 300;
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
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            timeUntilDeadly = 45;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (trueTimeLeft > 0)
            {
                trueTimeLeft--;
            }
            else
            {
                projectile.alpha += 255 / 60;
                if (projectile.alpha >= 255)
                {
                    projectile.timeLeft = 0;
                }
                timeUntilDeadly = 30;
            }

            FindFrame(projectile.height);
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = 1;
            }
            else
            {
                projectile.spriteDirection = -1;
            }

            if (timeUntilDeadly > 0)
            {
                projectile.velocity *= 0.98f;
                projectile.friendly = false;
                timeUntilDeadly--;
            }
            else
            {
                projectile.friendly = true;
                NPC targetNPC = Main.npc[0];
                float Distance = 1000; //max distance away
                bool Targeted = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && projectile.CanHit(Main.npc[i]) && projectile.localNPCImmunity[i] == 0)
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
                    Vector2 direction = projectile.DirectionTo(targetNPC.Center);
                    projectile.velocity += speed * direction;
                    projectile.velocity *= 0.96f;
                }
            }
        }
    }
}

