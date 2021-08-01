using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class AzurePearl : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires an azure beam that explodes into smaller bouncing projectiles");
        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.noMelee = true;
            item.width = 32;
            item.height = 32;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shootSpeed = 30f;
            item.shoot = ModContent.ProjectileType<AzureBurst>();
            item.mana = 10;
            item.magic = true;
            item.noUseGraphic = true;
        }
    }
    class AzureBurst : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = 400;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item110, projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                float speed = 35f;
                Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<AzureSpray>(), projectile.damage / 3, 1, projectile.owner);
            }
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, Scale: 2, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
        }
    }

    class AzureSpray : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, Scale: 1.35f, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity / 4;

            projectile.velocity.Y += 1.5f;
            projectile.velocity.X *= 0.98f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }

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
                projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}



