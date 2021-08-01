using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Incendius
{
    class IncendiusShreddage : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a slightly homing swarm of incendius shards" +
                "\nIgnites hit enemies");
        }
        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.width = 48;
            item.height = 56;
            item.magic = true;
            item.damage = 16;
            item.useTime = 16;
            item.useAnimation = 16;
            item.mana = 8;
            item.rare = 4;
            item.shoot = mod.ProjectileType("IncendiusShard");
            item.shootSpeed = 1;
            item.UseSound = SoundID.Item20;
            item.useStyle = 5;
            item.knockBack = 0.1f;
            item.autoReuse = true;
            item.noMelee = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(4, 6); i++)
            {
                Projectile.NewProjectile(new Vector2(player.Center.X + Main.rand.Next(-50, 51), player.Center.Y + Main.rand.Next(-50, 51)), new Vector2(speedX, speedY), type, damage, knockBack, Owner: item.owner);
            }
            return false;
        }
    }
    class IncendiusShard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
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
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 24;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 180;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
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
                float speed = .5f;
                Vector2 move = targetNPC.Center - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
            }
        }
        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 45, 10, DustID.Fire, DustScale: 0.5f, NoGravity: true);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 180);
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
