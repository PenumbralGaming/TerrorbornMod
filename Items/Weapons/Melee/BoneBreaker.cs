using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TerrorbornMod.Items.Weapons.Melee
{
    public class BoneBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Knocks enemies' bones out on hit" +
                "\nTheir bones will shatter into smaller pieces upon hitting the ground");
        }
        public override void SetDefaults()
        {
            item.damage = 36;
            item.melee = true;
            item.width = 60;
            item.height = 56;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 7.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.DD2_MonkStaffSwing;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("KnockedBone");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt, target.Center);
            if (crit)
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_SkeletonDeath);
            }

            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(10, 23);
                Vector2 velocity = direction * speed;
                Projectile.NewProjectile(target.Center, velocity, ModContent.ProjectileType<KnockedBone>(), item.damage / 2, 1f, player.whoAmI);
            }
        }
    }

    public class KnockedBone : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.melee = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.penetrate = -1;
        }
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 2);
            projectile.velocity.Y += 0.2f;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit2, projectile.Center);

            for (int i = 0; i < Main.rand.Next(2, 4); i++)
            {
                Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                float speed = Main.rand.Next(7, 16);
                Vector2 velocity = direction * speed;
                Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<KnockedBonePiece>(), projectile.damage, 1f, projectile.owner);
            }
        }
    }

    public class KnockedBonePiece : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.melee = true;
            projectile.timeLeft = 600;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
        }
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X * 2);
            projectile.velocity.Y += 0.1f;
        }
    }
}
