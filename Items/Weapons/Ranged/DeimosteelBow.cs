using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DeimosteelBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Converts wooden arrows into piercing dark bolts");
        }

        public override void SetDefaults()
        {
            item.damage = 13;
            item.ranged = true;
            item.width = 18;
            item.height = 36;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = 1;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
        }

        //public override Vector2? HoldoutOffset()
        //{
        //    return new Vector2(2f, 0);
        //}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (type == ProjectileID.WoodenArrowFriendly)
            {
                type = ModContent.ProjectileType<DarkBeam>();
            }
            return true;
        }
    }

    class DarkBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.extraUpdates = 2;
            projectile.timeLeft = 30 * (projectile.extraUpdates + 1);
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.width = 4;
            projectile.height = 4;
        }

        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

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


        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.damage = (int)(projectile.damage * 0.8);
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(projectile.Center, 54);
            dust.noGravity = true;
        }
    }
}
