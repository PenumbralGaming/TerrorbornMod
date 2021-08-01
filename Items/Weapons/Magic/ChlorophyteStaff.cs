using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class ChlorophyteStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Fires 5 chlorophyte beams at various speeds" +
                "\nThese will home in to your mouse cursor");
        }
        public override void SetDefaults()
        {
            item.damage = 32;
            item.noMelee = true;
            item.width = 50;
            item.height = 30;
            item.useTime = 25;
            item.useAnimation = 25;
            item.shoot = 10;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 4, 80, 0);
            item.rare = 7;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ChlorophyteBeam");
            item.shootSpeed = 15f;
            item.mana = 15;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50);
            for (int i = 0; i < 5; i++)
            {
                Vector2 directionVector = player.DirectionTo(Main.MouseWorld);
                Projectile.NewProjectile(position, directionVector * Main.rand.Next(4, 33), type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    class ChlorophyteBeam : ModProjectile
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
            projectile.penetrate = 10;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 120;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(3.5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 44, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
        }
    }
}

