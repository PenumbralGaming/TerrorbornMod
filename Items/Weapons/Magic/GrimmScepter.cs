using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class GrimmScepter : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.AddIngredient(ItemID.TissueSample, 5);
            recipe.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.DemoniteBar, 8);
            recipe2.AddIngredient(ItemID.ShadowScale, 5);
            recipe2.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("On crits enemies will release collectable Grimm Orbs that heal you for 2 health");
        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.noMelee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 16;
            item.shoot = 10;
            item.useAnimation = 16;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GrimmRay");
            item.shootSpeed = 15f;
            item.mana = 4;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 40);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner, 1);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner, -1);
            return false;
        }
    }
    class GrimmRay : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 350;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            int RotatationSpeed = 2;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * Direction * projectile.ai[0]));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (crit)
            {
                Projectile.NewProjectile(target.Center, Vector2.Zero, ModContent.ProjectileType<GrimmOrb>(), 0, 0, projectile.owner);
            }
        }
    }
    class GrimmOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            Player player = Main.player[projectile.owner];
            int speed = 8;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                int healAmount = 2;
                player.HealEffect(healAmount);
                player.statLife += healAmount;
                projectile.active = false;
            }
        }
    }
}
