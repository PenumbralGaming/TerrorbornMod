using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class AntlionShell : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Generates a shield that rotates around you" +
                "\nThis shield can block a projectile once every 5 seconds" +
                "\nAdditionally, the shield will hit enemies it passes through dealing 50 damage" +
                "\nBlocking a projectile or hitting an enemy with the shield grants you the 'panic' buff for 5 seconds");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.defense = 4;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.AntlionShell = true;

            bool AntlionShellShieldActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<AntlionShellShield>() && projectile.active)
                {
                    AntlionShellShieldActive = false;
                }
            }
            if (AntlionShellShieldActive)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<AntlionShellShield>(), 50, 0, player.whoAmI);
            }
        }
    }
    
    class AntlionShellShield : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.width = 20;
            projectile.height = 38;
            projectile.alpha = 255;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            player.AddBuff(BuffID.Panic, 60 * 5);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return target.type != NPCID.TruffleWorm && !target.friendly;
        }

        float direction = 0f;
        int cooldown = 60 * 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.AntlionShell)
            {
                projectile.active = false;
            }
            else
            {
                projectile.timeLeft = 60;
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 5;
            }
            projectile.velocity = Vector2.Zero;

            int distance = 100;
            projectile.position = direction.ToRotationVector2() * distance + player.Center;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.rotation = direction + MathHelper.ToRadians(180);

            direction += MathHelper.ToRadians(1f);

            if (cooldown <= 0)
            {
                for (int i = 0; i < 1000; i++)
                {
                    Projectile target = Main.projectile[i];
                    if (target.hostile && target.getRect().Intersects(projectile.getRect()))
                    {
                        target.penetrate = 0;
                        player.AddBuff(BuffID.Panic, 60 * 5);
                        CombatText.NewText(projectile.getRect(), Color.LightYellow, "Projectile blocked", true, true);
                        cooldown = 60 * 5;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_SkeletonHurt);
                    }
                }
            }
            else
            {
                cooldown--;
                projectile.alpha = 125;
            }
        }
    }
}

