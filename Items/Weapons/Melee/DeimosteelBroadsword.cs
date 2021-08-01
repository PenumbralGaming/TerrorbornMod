using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class DeimosteelBroadsword : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a short ranged piercing wave when swung");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.melee = true;
            item.width = 36;
            item.height = 36;
            item.useTime = 15;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.rare = 1;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shootSpeed = 10f;
            item.shoot = mod.ProjectileType("DeimoWave");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class DeimoWave : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/ShriekWave";
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 14;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            projectile.velocity *= 0.95f;
            projectile.alpha += 255 / 30;
            if (projectile.alpha >= 255)
            {
                projectile.active = false;
            }
        }
    }
}

