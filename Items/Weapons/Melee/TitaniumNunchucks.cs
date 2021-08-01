using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class TitaniumNunchucks : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Can deflect projectiles with a 1.5 second cooldown");
        }
        public override void SetDefaults()
        {
            item.damage = 62;
            item.width = 52;
            item.height = 94;
            item.melee = true;
            item.channel = true;
            item.useTime = 8;
            item.useAnimation = 8;
            item.useStyle = 100;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.shoot = mod.ProjectileType("TitaniumNunchucksProjectile");
            item.noUseGraphic = true;
            item.noMelee = true;
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.TitaniumBar, 13);
            recipe.AddIngredient(ItemID.WhiteString, 3);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class TitaniumNunchucksProjectile : ModProjectile
    {
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.spriteDirection * -1;
        }
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 8;
        }

        public override void SetDefaults()
        {
            projectile.idStaticNPCHitCooldown = 6;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.width = 52;
            projectile.height = 60;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.melee = true;
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

        bool Start = true;
        int DeflectCounter = 120;
        public override void AI()
        {
            DeflectCounter--;
            Player player = Main.player[projectile.owner];
            for (int i = 0; i < 200; i++)
            {
                if (Main.projectile[i].Distance(projectile.Center) < 50 + (Main.projectile[i].width + Main.projectile[i].height) / 2 && Main.projectile[i].hostile && DeflectCounter <= 0)
                {
                    DeflectCounter = 90;
                    Main.projectile[i].velocity = Main.projectile[i].velocity.RotatedByRandom(MathHelper.ToRadians(5)) * -1.5f;
                    Main.projectile[i].friendly = true;
                    Main.projectile[i].hostile = false;
                    Main.projectile[i].damage = 850;
                    Main.projectile[i].melee = true;
                    Main.projectile[i].owner = projectile.owner;
                    Main.projectile[i].knockBack = projectile.knockBack;
                }
            }

            FindFrame(projectile.height);

            if (Start)
            {
                Start = false;
            }
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            Vector2 vector2000 = Main.MouseWorld - vector;
            vector2000.Normalize();
            projectile.soundDelay--;
            if (projectile.soundDelay <= 0)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item1, (int)projectile.Center.X, (int)projectile.Center.Y);
                projectile.soundDelay = 15;
            }
            if (Main.myPlayer == projectile.owner)
            {
                if (!player.channel || player.noItems || player.CCed)
                {
                    projectile.Kill();
                }
            }

            //Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0.7f);
            projectile.Center = player.MountedCenter;
            projectile.position.X += player.width / 2 * player.direction;
            projectile.spriteDirection = player.direction * -1;
            player.ChangeDir((int)(vector2000.X / (float)Math.Abs(vector2000.X)));
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));
            player.bodyFrame.Y = 3 * player.bodyFrame.Height;
        }
    }
}

