using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class MindPiercer : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Hitting an enemy causes a spearhead to form above them" +
                "\nThis spearhead will loom above them for 3 seconds before falling and dealing 50 damage" +
                "\nOnly one spearhead can form at once per enemy");
        }
        public override void SetDefaults()
        {
            item.damage = 2;
            item.ranged = true;
            item.width = 38;
            item.height = 38;
            item.useTime = 17;
            item.noUseGraphic = true;
            item.useAnimation = 17;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 15;
            item.shoot = mod.ProjectileType("MindPiercerProjectile");
        }
    }
    class MindPiercerProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/MindPiercer";
        int FallWait = 40;
        public override void SetDefaults()
        {
            projectile.width = 38;
            projectile.height = 38;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 3000;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 15;
            height = 15;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            FallWait = 0;
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);
            if (modTarget.mindSpearheadTime <= 0)
            {
                modTarget.mindSpearheadTime = 60 * 3;
                int proj = Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<MindSpearhead>(), 50, 0, projectile.owner);
                Main.projectile[proj].ai[0] = target.whoAmI;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (FallWait <= 0)
            {
                damage = (int)(damage * 0.75f);
            }
        }

        public override void AI()
        {
            if (FallWait > 0)
            {
                FallWait--;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);

                if (projectile.velocity.X > 0)
                {
                    projectile.spriteDirection = 1;
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45f);
                }
                else
                {
                    projectile.spriteDirection = -1;
                    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
                }
            }
            else
            {
                projectile.velocity *= 0.9f;
                projectile.alpha += 255 / 20;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
        }
    }

    class MindSpearhead : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 3000;
        }

        bool fallen = false;
        int distance = 50;

        public override bool? CanHitNPC(NPC target)
        {
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);
            return target.whoAmI == projectile.ai[0] && fallen;
        }

        public override void AI()
        {
            NPC target = Main.npc[(int)projectile.ai[0]];
            TerrorbornNPC modTarget = TerrorbornNPC.modNPC(target);

            if (modTarget.mindSpearheadTime <= 0)
            {
                fallen = true;
            }

            if (!target.active)
            {
                projectile.active = false;
            }

            if (fallen)
            {
                distance -= 10;
                if (distance < 0)
                {
                    distance = 0;
                }
            }
            else
            {
                projectile.rotation = projectile.DirectionTo(target.Center).ToRotation() + MathHelper.ToRadians(45);
            }

            projectile.position = new Vector2(target.Center.X, target.position.Y - distance);
            projectile.position -= new Vector2(projectile.width, projectile.height) / 2;
        }
    }
}

