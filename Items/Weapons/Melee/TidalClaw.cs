using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TidalClaw : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a claw that will remain on the ground after landing\nClaws will stick into enemies they hit for repeated damage\nOnly the first hit will do full damage, repeated damage does only\n1/4 of the base damage");
        }
        public override void SetDefaults()
        {
            item.shootSpeed = 20f;
            item.damage = 28;
            item.knockBack = 5f;
            item.useStyle = 1;
            item.useAnimation = 25;
            item.useTime = 25;
            item.width = 16;
            item.height = 18;
            item.maxStack = 9999;
            item.rare = 2;
            item.consumable = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.melee = true;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 0, 5);
            item.shoot = mod.ProjectileType("TidalClawProjectile");
        }
    }
    class TidalClawProjectile : ModProjectile
    {
        bool HasStuck = false;
        int TrueTimeleft = 420;
        NPC stuckTo;
        Vector2 RelativePosition;
        int StrikeWait = 120;
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/TidalClaw";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Claw");
        }
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 18;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = 3;
            projectile.timeLeft = 10000;
            projectile.hide = false;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.velocity *= 0f;
            if (HasStuck)
            {
                projectile.active = false;
            }
            return HasStuck;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!HasStuck)
            {
                projectile.damage /= 4;
                HasStuck = true;
                stuckTo = target;
                RelativePosition = new Vector2(target.position.X - projectile.position.X, target.position.Y - projectile.position.Y);
            }
        }
        public override bool? CanHitNPC(NPC target)
        {
            return !HasStuck;
        }
        public override void AI()
        {
            TrueTimeleft--;
            if (TrueTimeleft <= 0)
            {
                projectile.alpha += 10;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            if (!HasStuck)
            {
                projectile.velocity.X *= 0.95f;
                projectile.velocity.Y *= 0.98f;
                projectile.velocity.Y += 0.25f;
                projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 2);
            }
            else
            {
                StrikeWait--;
                if (StrikeWait <= 0)
                {
                    StrikeWait = 120;
                    stuckTo.StrikeNPC(projectile.damage, 0, 0);
                }
                projectile.position.X = stuckTo.position.X - RelativePosition.X;
                projectile.position.Y = stuckTo.position.Y - RelativePosition.Y;
                if (stuckTo.life <= 0 || !stuckTo.active)
                {
                    projectile.active = false;
                }
            }
        }
    }
}

