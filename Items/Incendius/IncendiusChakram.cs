using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusChakram : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 5);
            recipe.AddIngredient(ItemID.CobaltBar, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 5);
            recipe2.AddIngredient(ItemID.PalladiumBar, 3);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Purgatory Chakram");
            Tooltip.SetDefault("Throws a chakram that returns to you" +
                "\nThe higher the stack, the more chakrams you can throw at once" +
                "\nMaximum stack of five");
        }
        public override void SetDefaults()
        {
            item.damage = 35;
            item.width = 54;
            item.height = 54;
            item.useTime = 8;
            item.useAnimation = 8;
            item.rare = 4;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 60, 0);
            item.shootSpeed = 35;
            item.shoot = mod.ProjectileType("IncendiusChakram_projectile");
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.maxStack = 5;
            item.melee = true;
        }
        public override bool CanUseItem(Player player)
        {
            int CrescentCount = 1;
            for (int i = 0; i < 300; i++)
            {
                if (Main.projectile[i].type == item.shoot && Main.projectile[i].active)
                {
                    CrescentCount++;
                }
            }
            return CrescentCount <= item.stack;
        }
    }
    class IncendiusChakram_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Incendius/IncendiusChakram"; } }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 8;
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
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, effects, 0f);
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 30;
            height = 30;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 54;
            projectile.height = 54;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 8;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(0, projectile.position); //Sound for when it hits a block

            // B O U N C E
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

        int TimeUntilReturn = 18;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            projectile.spriteDirection = player.direction * -1;
            projectile.rotation += 0.5f * player.direction;
            if (TimeUntilReturn <= 0)
            {
                projectile.tileCollide = false;
                Vector2 targetPosition = Main.player[projectile.owner].Center;
                float speed = 3.4f;
                Vector2 move = targetPosition - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
                projectile.velocity *= 0.90f;
                if (Main.player[projectile.owner].Distance(projectile.Center) <= 30)
                {
                    projectile.active = false;
                }
            }
            else
            {
                TimeUntilReturn--;
            }
        }
    }
}

