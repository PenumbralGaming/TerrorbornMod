using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Ammo
{
    class IncendiaryArrow : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 12;
            item.ranged = true;
            item.width = 14;
            item.height = 32;
            item.maxStack = 9999;
            item.consumable = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.shootSpeed = 4.5f;
            item.rare = 4;
            item.shoot = mod.ProjectileType("IncendiaryArrowProjectile");
            item.ammo = AmmoID.Arrow;
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Waits for a moment before firing" +
                "\nUpon firing it has incredibly high velocity" +
                "\nIgnores half of enemy defense" +
                "\nInflicts hit enemies with a random type of fire");
        }

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
    class IncendiaryArrowProjectile : ModProjectile
    {

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 32;
            projectile.timeLeft = 360;
            projectile.penetrate = 3;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.arrow = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 2;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return Countdown <= 0;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int choice = Main.rand.Next(4);
            if (choice == 0)
            {
                target.AddBuff(BuffID.OnFire, 60 * 5);
            }
            if (choice == 1)
            {
                target.AddBuff(BuffID.Frostburn, 60 * 5);
            }
            if (choice == 2)
            {
                target.AddBuff(BuffID.CursedInferno, 60 * 5);
            }
            if (choice == 3)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
            }
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                SpriteEffects effects = SpriteEffects.None;
                if (projectile.spriteDirection == -1)
                {
                    effects = SpriteEffects.FlipHorizontally;
                }
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            if (Countdown > 0)
            {
                Color lineColor = Color.FromNonPremultiplied(247, 201, 155, 255 / 2) * 0.25f;
                Utils.DrawLine(spriteBatch, projectile.Center, projectile.Center + (projectile.rotation + MathHelper.ToRadians(90)).ToRotationVector2() * 3000, lineColor, lineColor, 3);
            }
            return false;
        }

        int Countdown = 240;
        Vector2 originalVelocity = Vector2.Zero;
        float rotationOffset;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (Countdown == 240)
            {
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
                originalVelocity = projectile.velocity;
                //projectile.position += originalVelocity * 3;
                projectile.velocity = Vector2.Zero;

                rotationOffset = projectile.DirectionTo(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition).ToRotation() - projectile.rotation;
            }
            if (Countdown > 0)
            {
                projectile.rotation = projectile.DirectionTo(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition).ToRotation() - rotationOffset;
                Countdown--;
                if (Countdown <= 0)
                {
                    projectile.velocity = (projectile.rotation + MathHelper.ToRadians(90)).ToRotationVector2() * originalVelocity.Length() * 2;
                }
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(90);
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
