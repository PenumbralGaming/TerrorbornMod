using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class CrescentOfBloodlust : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 2);
            recipe.AddIngredient(ItemID.TissueSample, 1);
            recipe.AddIngredient(mod.ItemType("SanguineFang"), 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.DemoniteBar, 2);
            recipe2.AddIngredient(ItemID.ShadowScale, 1);
            recipe2.AddIngredient(mod.ItemType("SanguineFang"), 4);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Throws a boomerang that returns to you and leaves a trail of homing blood" +
                "\nThe higher the stack, the more boomerangs you can throw at once" +
                "\nMaximum stack of three");
        }
        public override void SetDefaults()
        {
            item.damage = 13;
            item.width = 20;
            item.height = 34;
            item.useTime = 8;
            item.useAnimation = 8;
            item.rare = 2;
            item.useStyle = 1;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 0, 35, 0);
            item.shootSpeed = 35;
            item.shoot = mod.ProjectileType("CrescentOfBloodlust_projectile");
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.maxStack = 3;
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
    class CrescentOfBloodlust_projectile : ModProjectile
    {
        public override string Texture { get { return "TerrorbornMod/Items/Weapons/Melee/CrescentOfBloodlust"; } }

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
        public override void SetDefaults()
        {
            projectile.melee = true;
            projectile.width = 20;
            projectile.height = 34;
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

        int TimeUntilReturn = 9;
        int BloodWait = 10;
        bool Start = true;
        public override void AI()
        {
            if (Start)
            {
                Start = false;
                BloodWait = Main.rand.Next(8, 15);
            }
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

            BloodWait--;
            if (BloodWait <= 0)
            {
                BloodWait = Main.rand.Next(8, 15);
                Projectile.NewProjectile(projectile.position + new Vector2(Main.rand.Next(projectile.width), Main.rand.Next(projectile.height)), projectile.velocity / 9, mod.ProjectileType("Sanguine"), projectile.damage / 2, 0, projectile.owner);
            }
        }
    }
    public class Sanguine : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 100;
            projectile.melee = true;
        }

        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
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
                float speed = .6f;
                Vector2 move = targetNPC.Center - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
            }
        }
    }
}
