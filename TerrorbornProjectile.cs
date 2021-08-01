using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace TerrorbornMod
{
    class TerrorbornProjectile : GlobalProjectile
    {
        int BaseExtraUpdates = 0;
        bool BaseUsesIDStaticImmunity = false;
        int BaseIdStaticNPCHitCooldown = 15;
        bool Start = true;
        int crystalWait = 10;
        public override bool InstancePerEntity => true;

        public bool VeinBurster = false;
        public bool ContaminatedMarine = false;

        public bool Shadowflame = false;

        public override bool CanHitPlayer(Projectile projectile, Player target)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(projectile, target);
        }

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.JestersArrow || projectile.type == ProjectileID.UnholyArrow)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = -1;
            }
            if (projectile.type == ProjectileID.MeteorShot)
            {
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 15;
            }
        }

        public static TerrorbornProjectile modProjectile(Projectile projectile)
        {
            return projectile.GetGlobalProjectile<TerrorbornProjectile>();
        }

        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }

            if (VeinBurster)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath21, target.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    int proj = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), target.Center, direction * speed, ModContent.ProjectileType<Projectiles.VeinBurst>(), damage, 0f, player.whoAmI);
                    Main.projectile[proj].DamageType = DamageClass.Ranged;
                }
            }

            if (ContaminatedMarine)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, target.Center);
                for (int i = 0; i < Main.rand.Next(7, 9); i++)
                {
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(25, 35);
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), target.Center, direction * speed, ModContent.ProjectileType<Items.Weapons.Restless.NightmareBoilRanged>(), damage, 0f, player.whoAmI);
                }
            }

            if (Shadowflame)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 3);
            }
        }

        int hellfireCooldown = 120;
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (modPlayer.IncendiusArmorBonus && projectile.minion)
            {
                hellfireCooldown--;
                if (hellfireCooldown <= 0 && player.HeldItem.DamageType == DamageClass.Summon)
                {
                    float speed = 15f;
                    hellfireCooldown = 120;
                    Vector2 velocity = projectile.DirectionTo(Main.MouseWorld) * speed;
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.HellFire>(), (int)(18 * projectile.minionSlots), 0.01f, projectile.owner);
                }
            }

            if (Start)
            {
                Start = false;
                BaseExtraUpdates = projectile.extraUpdates;
                BaseUsesIDStaticImmunity = projectile.usesIDStaticNPCImmunity;
                BaseIdStaticNPCHitCooldown = projectile.idStaticNPCHitCooldown;
            }

            if (projectile.friendly)
            {
                if (player.HasBuff(ModContent.BuffType<Items.Equipable.Armor.HuntersMark>()) && projectile.DamageType == DamageClass.Ranged)
                {
                    NPC targetNPC = Main.npc[0];
                    float Distance = 375; //max distance away
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
                        float speed = .35f;
                        Vector2 move = targetNPC.Center - projectile.Center;
                        float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                        move *= speed / magnitude;
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2.5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                    }
                }
            }

            if (Shadowflame)
            {
                Dust dust = Dust.NewDustPerfect(projectile.Center, 27);
                dust.noGravity = true;
                dust.velocity = Vector2.Zero;
                dust.scale = 2f;
            }
        }
    }
}
