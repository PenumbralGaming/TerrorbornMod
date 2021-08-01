//using System;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria;
//using Terraria.ID;
//using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

//namespace TerrorbornMod.Items.Ammo
//{
//    class AzuriteArrow : ModItem
//    {
//        public override void SetDefaults()
//        {
//            item.damage = 12;
//            item.ranged = true;
//            item.width = 14;
//            item.height = 32;
//            item.maxStack = 9999;
//            item.consumable = true;
//            item.knockBack = 2;
//            item.value = Item.sellPrice(0, 0, 0, 5);
//            item.shootSpeed = 15;
//            item.rare = 2;
//            item.shoot = mod.ProjectileType("AzuriteArrowProjectile");
//            item.ammo = AmmoID.Arrow;
//        }
//        public override void SetStaticDefaults()
//        {
//            Tooltip.SetDefault("A high speed arrow that rains water as it travels.\nThe arrow's speed is unaffected by water.");
//        }
//        //public override bool HoldItemFrame(Player player)
//        //{
//        //    player.bodyFrame.Y = 56 * 2;
//        //    return true;
//        //}
//        public override void AddRecipes()
//        {
//            ModRecipe recipe = new ModRecipe(mod);
//            recipe.AddIngredient(mod.ItemType("AzuriteBar"));
//            recipe.AddIngredient(ItemID.WoodenArrow, 111);
//            recipe.AddTile(TileID.Anvils);
//            recipe.SetResult(this, 111);
//            recipe.AddRecipe();
//        }
//    }
//    class AzuriteArrowProjectile : ModProjectile
//    {
//        public override void SetStaticDefaults()
//        {
//            DisplayName.SetDefault("Azurite Arrow");
//        }
//        public override void SetDefaults()
//        {
//            projectile.width = 32;
//            projectile.height = 32;
//            projectile.ranged = true;
//            projectile.timeLeft = 600;
//            projectile.tileCollide = true;
//            projectile.friendly = true;
//            projectile.hostile = false;
//            projectile.arrow = true;
//            projectile.ignoreWater = true;
//        }
//        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
//        {
//            width = 14;
//            height = 14;
//            return true;
//        }
//        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
//        {
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21, projectile.Center);
//        }
//        bool Start = true;
//        int RainWait = 10;
//        public override void AI()
//        {
//            if (Start)
//            {
//                RainWait = Main.rand.Next(5, 11);
//                Start = false;
//            }
//            RainWait--;
//            if (RainWait <= 0)
//            {
//                RainWait = Main.rand.Next(5, 11);
//                Projectile.NewProjectile(projectile.Center, new Vector2(0, 15), mod.ProjectileType("AzuriteRain"), projectile.damage / 4, 0, projectile.owner);
//            }
//            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(90);
//        }
//        public override void Kill(int timeLeft)
//        {
//            if (timeLeft > 0)
//            {
//                for (int i = 0; i < Main.rand.Next(3, 5); i++)
//                {
//                    int Num54 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33, 0, 0, Scale: 1.5f, newColor: Color.Blue);
//                    Main.dust[Num54].noGravity = true;
//                    Main.dust[Num54].velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(30)) * Main.rand.NextFloat(0.9f, 1.1f);
//                }
//                Collision.HitTiles(projectile.position, projectile.velocity, projectile.width, projectile.height);
//                Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
//            }
//        }
//    }

//    class AzuriteRain : ModProjectile
//    {
//        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.SapphireBolt; } }
//        public override void SetStaticDefaults()
//        {
//            DisplayName.SetDefault("Azurite Rainfall");
//        }
//        public override void SetDefaults()
//        {
//            projectile.width = 6;
//            projectile.height = 6;
//            projectile.ranged = true;
//            projectile.timeLeft = 1000;
//            projectile.tileCollide = true;
//            projectile.friendly = true;
//            projectile.hostile = false;
//            projectile.hide = true;
//            projectile.ignoreWater = true;
//        }
//        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
//        {
//            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item21, projectile.Center);
//        }
//        public override void AI()
//        {
//            int Num54 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33, 0, 0, Scale: 2, newColor: Color.Blue);
//            Main.dust[Num54].noGravity = true;
//            Main.dust[Num54].velocity = projectile.velocity;
//        }
//    }
//}
