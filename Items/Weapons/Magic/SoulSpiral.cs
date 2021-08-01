using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SoulSpiral : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates a spiral of souls around you");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.width = 32;
            item.height = 38;
            item.magic = true;
            item.damage = 106;
            item.useTime = 32;
            item.useAnimation = 32;
            item.mana = 10;
            item.rare = 5;
            item.shoot = mod.ProjectileType("SpiralSoul");
            item.shootSpeed = 35;
            item.UseSound = SoundID.Item20;
            item.useStyle = 5;
            item.knockBack = 0.1f;
            item.autoReuse = true;
        }

        int rotationDirection = 1;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int soulAmount = 9;
            rotationDirection *= -1;

            for (int i = 0; i < soulAmount; i++)
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity = velocity.RotatedBy(MathHelper.ToRadians(i * (360 / soulAmount)));
                int proj = Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = rotationDirection;
            }
            return false;
        }
    }

    class SpiralSoul : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = 25;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = (int)(360 / rotationSpeed);
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 10;
        }

        bool start = true;
        float rotationSpeed = 10;
        public override void AI()
        {
            if (start)
            {
                start = false;
            }

            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(rotationSpeed) * projectile.ai[0]);

            Dust dust = Dust.NewDustPerfect(projectile.Center, 21);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
        }
    }
}

