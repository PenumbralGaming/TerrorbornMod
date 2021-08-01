using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class GaussStriker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires eratic, but infinitely piercing, bolts of lightning");
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
        public override void SetDefaults()
        {
            item.damage = 50;
            item.noMelee = true;
            item.width = 48;
            item.height = 20;
            item.useTime = 8;
            item.shoot = 10;
            item.useAnimation = 8;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item12;
            item.autoReuse = true;
            item.shootSpeed = 25f;
            item.shoot = mod.ProjectileType("GaussBolt");
            item.mana = 3;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
    }

    class GaussBolt : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }

        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 25;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 400;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.extraUpdates = 100;
            projectile.hide = true;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        //public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    base.PostDraw(spriteBatch, lightColor);
        //    Texture2D texture = ModContent.GetTexture(Texture);
        //    Vector2 position = projectile.position - Main.screenPosition;
        //    position += new Vector2(projectile.width / 2, projectile.height / 2);
        //    //position.Y += 4;
        //    Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), SpriteEffects.None, 0);
        //}

        bool start = true;
        float rotationCounter = 15;
        public override void AI()
        {
            if (start)
            {
                start = false;
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
            }

            //FindFrame(projectile.height);
            //projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            if (Main.player[projectile.owner].Distance(projectile.Center) > 30)
            {
                for (int i = 0; i < 4; i++)
                {
                    int dust = Dust.NewDust(projectile.Center - (projectile.velocity * i / 4), 1, 1, 62, 0, 0, Scale: 2, newColor: Color.White);
                    Main.dust[dust].noGravity = true;

                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.NextFloat(1.5f, 3f);

                    Main.dust[dust].velocity = direction * speed;
                }
            }

            rotationCounter--;
            if (rotationCounter <= 0)
            {
                rotationCounter = Main.rand.Next(10, 20);
                projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
            }
        }
    }
}


