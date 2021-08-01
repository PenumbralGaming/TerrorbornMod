using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;
using Terraria.ModLoader.Exceptions;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class BubbleBow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Bow");
            Tooltip.SetDefault("Converts arrows into bubble arrows\nBubble arrows leave behind a trail of bubbles, and explode into them upon\nhitting enemies");
        }

        public override void SetDefaults()
        {
            item.damage = 17;
            item.ranged = true;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 20f;
            item.useAmmo = AmmoID.Arrow;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            type = mod.ProjectileType("BubbleArrow");
            return true;
        }
    }
    class BubbleArrow : ModProjectile
    {
        int BubbleWait = 3;
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.friendly = true;
            projectile.ranged = true;
            //projectile.extraUpdates = 100;
            projectile.timeLeft = 200;
            projectile.penetrate = 1;
        }
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) - 1.57f;
            BubbleWait--;
            if (BubbleWait <= 0)
            {
                BubbleWait = 3;
                Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), mod.ProjectileType("Bubble"), projectile.damage / 4, 0, projectile.owner);
            }
        }
        public override void Kill(int timeLeft)
        {
            if (timeLeft > 0)
            {
                for (int i = 0; i < Main.rand.Next(3, 6); i++)
                {
                    Projectile.NewProjectile(projectile.Center - projectile.velocity, new Vector2(Main.rand.Next(-5, 6), Main.rand.Next(-5, 6)), mod.ProjectileType("Bubble"), (int)(projectile.damage / 3), 0, projectile.owner);
                }
            }
        }
    }
    class Bubble : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Bubble";
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.ranged = true;
            //projectile.extraUpdates = 100;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.hide = false;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54, projectile.position);
        }
    }
}
