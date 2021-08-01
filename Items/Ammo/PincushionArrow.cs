using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class PincushionArrow : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 17;
            item.ranged = true;
            item.width = 14;
            item.height = 32;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.shootSpeed = 3;
            item.rare = 5;
            item.shoot = mod.ProjectileType("PincushionArrowProjectile");
            item.ammo = AmmoID.Arrow;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Sticks onto enemies and then breaks shortly after, dealing extra damage" +
                "\nAdditionally ignores half of an enemy's defense" +
                "\nIf an enemy they're stuck into dies they'll launch into another enemy");
        }
    }
    class PincushionArrowProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/PincushionArrow";
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.ranged = true;
            projectile.timeLeft = 3600;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!stuck)
            {
                Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
                stuck = true;
                wasCrit = crit;
                stuckNPC = target;
                offset = target.position - projectile.position;
            }
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            if (stuck)
            {
                stuckNPC.StrikeNPC(projectile.damage / 10 + stuckNPC.defense / 2, 0, 0, wasCrit);
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (stuck)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox.Width = 14;
            hitbox.Height = 14;
            hitbox.Y += (int)(32f * (14f / 32f) / 2f);
            base.ModifyDamageHitbox(ref hitbox);
        }

        bool stuck = false;
        bool wasCrit;
        NPC stuckNPC;
        int stuckTimeLeft = 300;
        Vector2 offset;
        bool start = true;
        float originalVelocity;
        NPC target;
        public override void AI()
        {
            if (start)
            {
                start = false;
                originalVelocity = projectile.velocity.Length();
            }
            if (stuck)
            {
                projectile.position = stuckNPC.position - offset;
                stuckNPC.lifeRegen -= 5;
                if (!stuckNPC.active)
                {
                    bool foundTarget = false;
                    float range = 600;
                    for (int i = 0; i < 200; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (projectile.Distance(npc.Center) < range && projectile.CanHit(npc) && npc.active && !npc.friendly && !npc.dontTakeDamage && npc.chaseable)
                        {
                            range = projectile.Distance(npc.Center);
                            target = npc;
                            foundTarget = true;
                        }
                    }
                    if (foundTarget)
                    {
                        Vector2 newVelocity = projectile.DirectionTo(target.Center) * originalVelocity;
                        projectile.velocity = newVelocity;
                        stuck = false;
                        stuckTimeLeft = 300;
                    }
                    else
                    {
                        projectile.timeLeft = 1;
                    }
                }
                stuckTimeLeft--;
                if (stuckTimeLeft <= 0)
                {
                    projectile.timeLeft = 1;
                }
            }
            else
            {
                projectile.velocity.Y += 0.05f;
                projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(90);
            }
        }
    }
}

