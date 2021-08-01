using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class HurricaneDiscs : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Creates two discs that rapidly spin around you, shredding through foes" +
                "\nThe distance at which they spin around you is controlled by the mouse cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 67;
            item.channel = true;
            item.noUseGraphic = true;
            item.noMelee = true;
            item.melee = true;
            item.width = 25;
            item.height = 25;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.rare = 5;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            item.autoReuse = true;
            item.shootSpeed = 0f;
            item.shoot = mod.ProjectileType("HurricaneDisc");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            bool spawnProjectile = true;
            foreach (Projectile projectile in Main.projectile)
            {
                if (projectile.type == type && projectile.active)
                {
                    spawnProjectile = false;
                    break;
                }
            }

            if (spawnProjectile)
            {
                Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
                int proj = Projectile.NewProjectile(position, Vector2.Zero, type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = 1;
            }

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 18);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class HurricaneDisc : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 360 / 15;
            projectile.extraUpdates = 2;
        }

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(Color.White) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        float rotationFromPlayer;
        float distance;
        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
                rotationFromPlayer = 0f;
                if (projectile.ai[0] == 1)
                {
                    rotationFromPlayer += MathHelper.ToRadians(180);
                }
                distance = 0f;
            }

            Player player = Main.player[projectile.owner];
            projectile.rotation += MathHelper.ToRadians(45) * player.direction / (projectile.extraUpdates + 1);
            rotationFromPlayer += MathHelper.ToRadians(15) * player.direction / (projectile.extraUpdates + 1) * player.meleeSpeed;
            projectile.spriteDirection = player.direction;
            projectile.active = player.channel;
            projectile.timeLeft = 300;

            float targetDistance = player.Distance(Main.MouseWorld);
            if (targetDistance < 125)
            {
                targetDistance = 125;
            }
            else if (targetDistance > 600)
            {
                targetDistance = 600;
            }
            distance = MathHelper.Lerp(distance, targetDistance, 0.02f);

            projectile.position = player.Center + distance * rotationFromPlayer.ToRotationVector2();
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);
        }
    }
}


