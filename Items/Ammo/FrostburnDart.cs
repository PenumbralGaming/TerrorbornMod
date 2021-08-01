using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class FrostburnDart : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 5;
            item.ranged = true;
            item.width = 14;
            item.height = 22;
            item.maxStack = 999;
            item.consumable = true;
            item.knockBack = 1;
            item.shootSpeed = 0;
            item.rare = 0;
            item.shoot = mod.ProjectileType("FrostburnDartProjectile");
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
            recipe.AddIngredient(ModContent.ItemType<WoodDart>(), 10);
            recipe.AddIngredient(ItemID.IceTorch);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
    class FrostburnDartProjectile : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Ammo/FrostburnDart";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming dart");
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
        int DustCooldown = 69;
        public override void AI()
        {
            if (DustCooldown == 69)
            {
                DustCooldown = Main.rand.Next(5, 10);
            }
            projectile.velocity.Y += 0.1f;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            DustCooldown--;
            if (DustCooldown <= 0)
            {
                DustCooldown = Main.rand.Next(5, 10);
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 135);
                Main.dust[dust].velocity = projectile.velocity;
                Main.dust[dust].noGravity = true;
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(3) == 0)
            {
                target.AddBuff(BuffID.Frostburn, 60 * 3);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (Main.rand.Next(101) <= 20)
            {
                Item.NewItem((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height, mod.ItemType("FrostburnDart"));
            }
            Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
        }
    }
}

