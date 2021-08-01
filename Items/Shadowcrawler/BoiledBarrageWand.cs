using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Shadowcrawler
{
    class BoiledBarrageWand : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Midnight Barrage Staff");
            Tooltip.SetDefault("Rains midnight fireballs from above");
            Item.staff[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.rare = 5;
            item.width = 44;
            item.height = 54;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 1;
            item.magic = true;
            item.UseSound = SoundID.Item20;
            item.mana = 12;
            item.damage = 76;
            item.shootSpeed = 25;
            item.knockBack = 1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shoot = ModContent.ProjectileType<MidnightFireballMagic>();
            item.value = Item.sellPrice(0, 2, 70, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 newPosition = new Vector2(Main.MouseWorld.X, player.Center.Y - 1000);
                newPosition.X += Main.rand.Next(-200, 201);
                newPosition.Y += Main.rand.Next(-100, 50);
                Vector2 direction = Main.MouseWorld - newPosition;
                direction.Normalize();
                Projectile.NewProjectile(newPosition, direction * item.shootSpeed, type, damage, knockBack, player.whoAmI);
            }
            return false;
        }
    }
    public class MidnightFireballMagic : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Projectiles/MidnightFireball";
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.CursedInferno, 60 * 2);
            projectile.timeLeft = 1;
        }
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 40;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Color.White;
            return true;
        }

        bool start = true;
        public override void AI()
        {
            if (start)
            {
                start = false;
            }

            projectile.tileCollide = projectile.Center.Y > Main.player[projectile.owner].Center.Y;

            projectile.rotation += MathHelper.ToRadians(5);
            Lighting.AddLight(projectile.Center, Color.Green.ToVector3() * 0.80f * Main.essScale);

            //int dust = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, 74);
            //Main.dust[dust].velocity = projectile.velocity;

            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 74);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].noLight = true;
            Main.dust[dust].velocity = projectile.velocity;

            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;

            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(1f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            }

            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frameCounter = 5;
                projectile.frame++;
                if (projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 12, 7, 74, 2f, true);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
}

