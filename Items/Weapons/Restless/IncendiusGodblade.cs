using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class IncendiusGodblade : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/IncendiusGodblade";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Sinful Saber");
        }

        public override string defaultTooltip()
        {
            return "Fires an inaccurate slash projectile by default";
        }

        public override string altTooltip()
        {
            return "Throws the sword at the cursor, causing it to leave a trail of slashes while returning" +
                "\nIf the sword hits an enemy while not returning, it will create multiple slashes";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 46;
            item.melee = true;
            item.width = 58;
            item.height = 62;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 1;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.shoot = mod.ProjectileType("IncendiusSlash");
            item.shootSpeed = 20;
            item.crit = 7;
            item.autoReuse = true;
            modItem.restlessTerrorDrain = 12f;
            modItem.restlessChargeUpUses = 5;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                item.noMelee = true;
                item.noUseGraphic = true;
            }
            else
            {
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                type = mod.ProjectileType("IncendiarySwordThrown");
                speedX *= 2.5f;
                speedY *= 2.5f;
                item.noMelee = true;
                item.noUseGraphic = true;
            }
            else
            {
                Vector2 rotatedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(50));
                rotatedSpeed *= Main.rand.NextFloat(0.65f, 1.35f);
                speedX = rotatedSpeed.X;
                speedY = rotatedSpeed.Y;
                item.noUseGraphic = false;
                item.noMelee = false;
            }
            return base.RestlessShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe.AddIngredient(ItemID.PalladiumBar, 12);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 10);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 35);
            recipe2.AddIngredient(ItemID.CobaltBar, 12);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 10);
            recipe2.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }

    class IncendiarySwordThrown : ModProjectile
    {
        int timeUntilReturn = 60;
        int penetrateUntilReturn = 3;

        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/IncendiusGodblade";
        public override void SetDefaults()
        {
            projectile.width = 58;
            projectile.height = 62;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.timeLeft = 600;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (timeUntilReturn > 0)
            {
                for (int i = 0; i < Main.rand.Next(4, 6); i++)
                {
                    float speed = Main.rand.Next(8, 22);
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<IncendiusSlash>(), projectile.damage / 2, 1, projectile.owner);
                }
            }
            penetrateUntilReturn--;
            if (penetrateUntilReturn <= 0)
            {
                timeUntilReturn = 0;
                speed = projectile.velocity.Length();
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 10;
            height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

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
            speed = projectile.velocity.Length();
            timeUntilReturn = 0;
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position);
            return false;
        }

        float speed;
        int projectileWait = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (timeUntilReturn <= 0)
            {
                projectile.rotation += 0.5f * player.direction;
                projectile.tileCollide = false;
                Vector2 direction = projectile.DirectionTo(player.Center);
                projectile.velocity = direction * speed;

                if (Main.player[projectile.owner].Distance(projectile.Center) <= speed)
                {
                    projectile.active = false;
                }

                int d = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.Fire);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = projectile.velocity;
                Main.dust[d].noLight = true;

                projectileWait--;
                if (projectileWait <= 0)
                {
                    projectileWait = 20;
                    float speed = 15f;
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<IncendiusSlash>(), projectile.damage, 1, projectile.owner);
                }
            }
            else
            {
                projectile.direction = player.direction;
                projectile.spriteDirection = player.direction;
                projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135);
                if (projectile.spriteDirection == 1)
                {
                    projectile.rotation -= MathHelper.ToRadians(90f);
                }
                timeUntilReturn--;
                if (timeUntilReturn <= 0)
                {
                    speed = projectile.velocity.Length();
                }
            }
        }
    }

    class IncendiusSlash : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 26;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.timeLeft = 300;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            projectile.alpha += 255 / 60;
            if (projectile.alpha >= 255)
            {
                projectile.active = false;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, 0, projectile.width, projectile.height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), SpriteEffects.None, 0);
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }
}
