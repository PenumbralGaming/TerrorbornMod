using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class IncendiaryBullet : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 10;
            item.ranged = true;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.shootSpeed = 4;
            item.rare = 4;
            item.shoot = mod.ProjectileType("IncendiaryBulletProjectile");
            item.ammo = AmmoID.Bullet;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Incendiary Round");
            Tooltip.SetDefault("Fires at incredibly high speeds" +
                "\nLeaves a cloud of fiery gas behind on critical hits" +
                "\nHeals you for 1 HP on crits if you are under 250 HP");
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.IncendiusAlloy>(), 2);
            recipe.AddIngredient(ItemID.CobaltBar);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 222);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Materials.IncendiusAlloy>(), 2);
            recipe2.AddIngredient(ItemID.PalladiumBar);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this, 222);
            recipe2.AddRecipe();
        }
    }
    class IncendiaryBulletProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 2;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.extraUpdates = 5;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 60 * 10);
            if (crit)
            {
                Projectile.NewProjectile(projectile.Center, target.velocity, ModContent.ProjectileType<Incendius.FlameCloud>(), projectile.damage / 5, 0, projectile.owner);

                Player player = Main.player[projectile.owner];
                if (player.statLife <= 250)
                {
                    player.HealEffect(1);
                    player.statLife++;
                }
            }
        }

        int dustCooldown = 4;
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            dustCooldown--;
            if (dustCooldown <= 0)
            {
                dustCooldown = 4;
                Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Fire);
                dust.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
                Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            }
        }
    }
}