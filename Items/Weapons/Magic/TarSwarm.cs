using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class TarSwarm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Raplidly fires antlion larvae." +
                "\nSlightly ignores enemy defense.");
        }
        public override void SetDefaults()
        {
            item.damage = 8;
            item.noMelee = true;
            item.width = 52;
            item.height = 40;
            item.useTime = 5;
            item.scale = 0.8f;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("AntlionLarva");
            item.shootSpeed = 15f;
            item.mana = 4;
            item.magic = true;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector2 rotatedVelocity = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
                Projectile.NewProjectile(position, rotatedVelocity, type, damage, knockBack, item.owner);
            }
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            recipe1.AddIngredient(ItemID.BeeGun);
            recipe1.AddIngredient(ItemID.BeeWax, 10);
            recipe1.AddIngredient(mod.ItemType("TarOfHunger"), 85);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
        }
    }
    class AntlionLarva : ModProjectile
    {
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 5;
        }
        public override void SetStaticDefaults()
        {
            //ProjectileID.Sets.TrailCacheLength[this.projectile.type] = 4;
            //ProjectileID.Sets.TrailingMode[this.projectile.type] = 1;
            Main.projFrames[projectile.type] = 3;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 1;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    //Thanks to Seraph for afterimage code.
        //    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
        //    for (int i = 0; i < projectile.oldPos.Length; i++)
        //    {
        //        Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
        //        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
        //        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return false;
        //}
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 300;
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
            return false;
        }
        public override void AI()
        {
            FindFrame(projectile.height);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
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
                float speed = .6f;
                Vector2 direction = projectile.DirectionTo(targetNPC.Center);
                projectile.velocity += speed * direction;
                projectile.velocity *= 0.98f;
            }
        }
    }
}
