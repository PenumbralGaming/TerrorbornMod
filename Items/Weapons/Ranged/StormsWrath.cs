using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class StormsWrath : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Wrath");
            Tooltip.SetDefault("Fires a storm bolt along side it's arrows. \nStorm bolts will summon smaller bolt upon hitting foes.");
        }

        public override void SetDefaults()
        {
            item.damage = 11;
            item.ranged = true;
            item.width = 26;
            item.height = 56;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.rare = 3;
            item.UseSound = SoundID.Item75;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cloud, 15);
            recipe.AddIngredient(ItemID.RainCloud, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.AddIngredient(ItemID.Feather, 15);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 13);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX / 5, speedY / 5, mod.ProjectileType("StormsBeam"), damage, knockBack, player.whoAmI);
            return true;
        }
    }

    class BoltBallista : ModItem
    {
        int firesTilBolt = 1;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a storm bolt along side it's arrows, which are converted to Bolt Arrows.\nStorm bolts will summon a smaller bolt upon hitting foes.\n20% chance to not consume ammo");
        }
        public override bool ConsumeAmmo(Player player)
        {
            if (Main.rand.Next(101) <= 20)
            {
                return false;
            }
            return true;
        }
        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.width = 36;
            item.height = 64;
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 4, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 35f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StormsWrath"));
            recipe.AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofFlight, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            firesTilBolt--;
            if (firesTilBolt <= 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item75, position);
                firesTilBolt = 2;
                for (int i = 0; i < 2; i++)
                {
                    Projectile.NewProjectile(position.X - 50 + Main.rand.Next(100), position.Y - 50 + Main.rand.Next(100), speedX / 5, speedY / 5, mod.ProjectileType("StormsBeam"), (int)(damage * 0.75f), knockBack, player.whoAmI);
                }
            }
            type = mod.ProjectileType("ThunderArrow");
            return true;
        }
    }

    class StormsBeam : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Beam");
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 1600;
            projectile.penetrate = 1;

            projectile.width = 4;
            projectile.height = 4;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            projectile.damage = (int)(projectile.damage * 0.8);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Projectile.NewProjectile(new Vector2(target.position.X + Main.rand.Next(0, target.width), target.Center.Y - target.height * 1.5f), new Vector2(0, 10), mod.ProjectileType("StormsBolt"), (int)(projectile.damage * 0.75f), projectile.knockBack, projectile.owner);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, 1, 1, 88, 0f, 0f, 0, Scale: 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = projectile.position;
            Main.dust[dust].scale = 0.75f;
            Main.dust[dust].velocity *= 0.2f;
        }
    }

    class StormsBolt : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Storm's Bolt");
        }
        public override void SetDefaults()
        {
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 15;
            projectile.penetrate = 1;
            projectile.hide = true;

            projectile.width = 4;
            projectile.height = 4;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, 1, 1, 88, 0f, 0f, 0, Scale: 0.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].position = projectile.position;
            Main.dust[dust].scale = 0.5f;
            Main.dust[dust].velocity *= 0.2f;
        }
    }

    class ThunderArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.ranged = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.penetrate = 5;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, 0f, 0f, 0, Scale: 1f);
            Main.dust[dust].velocity = projectile.velocity;
            Main.dust[dust].noGravity = true;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - MathHelper.ToRadians(90);
        }
    }
}
