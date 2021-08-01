using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class AzuriteDagger : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a dagger that isn't slowed by water" +
                "\nThis dagger will ricochet off of enemies and momentarily linger instead of falling");
        }
        public override void SetDefaults()
        {
            item.damage = 19;
            item.ranged = true;
            item.width = 26;
            item.height = 32;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 15;
            item.noUseGraphic = true;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 15;
            item.shoot = mod.ProjectileType("AzuriteDaggerProjectile");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }
    class AzuriteDaggerProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/AzuriteDagger";
        int FallWait = 60;
        int leaveWait = 120;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azurite Dagger");
        }
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 32;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.ranged = true;
            projectile.hide = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.timeLeft = 3000;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (FallWait <= 0)
            {
                return;
            }
            float speed = projectile.velocity.Length() / 3;
            Vector2 direction = (projectile.DirectionTo(target.Center).ToRotation() + MathHelper.ToRadians(180)).ToRotationVector2();
            projectile.velocity = speed * direction;
            FallWait = 0;
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
                projectile.velocity *= 0.98f;
                if (projectile.velocity.X > 0)
                {
                    projectile.rotation += MathHelper.ToRadians(20);
                }
                else
                {
                    projectile.rotation -= MathHelper.ToRadians(20);
                }
                if (leaveWait > 0)
                {
                    leaveWait--;
                }
                else
                {
                    projectile.alpha += 15;
                    if (projectile.alpha >= 255)
                    {
                        projectile.active = false;
                    }
                }
            }
        }
        public override void Kill(int timeLeft)
        {
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
        }
    }
}
