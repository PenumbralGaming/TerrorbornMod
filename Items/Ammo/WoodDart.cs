using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class WoodDart : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden Dart");
        }
        public override void SetDefaults()
        {
            item.damage = 4;
            item.ranged = true;
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1;
            item.shootSpeed = 0;
            item.rare = 0;
            item.shoot = mod.ProjectileType("WoodDartProjectile");
            item.ammo = AmmoID.Dart;
        }
        //public override bool HoldItemFrame(Player player)
        //{
        //    player.bodyFrame.Y = 56 * 2;
        //    return true;
        //}
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("Wood", 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 15);
            recipe.AddRecipe();
        }
    }
    class WoodDartProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wooden dart");
        }
        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 22;
            projectile.ranged = true;
            projectile.timeLeft = 1000;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
        }
        public override void AI()
        {
            projectile.velocity.Y += 0.1f;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
        }
        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("WoodDart"));
            }
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
        }
    }
}
