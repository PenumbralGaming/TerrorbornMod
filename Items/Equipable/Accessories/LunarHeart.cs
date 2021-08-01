using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class LunarHeart : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a powerful Tide Spirit to fight for you" +
                "\nThis Tide Spirit will consume terror in order to attack enemies" +
                "\nIt won't be able to attack enemies if you don't have enough terror");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 5));
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 34;
            item.accessory = true;
            item.noMelee = true;
            item.expert = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.useAnimation = 5;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TideSpirit = true;

            bool TidalSpiritActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<TideSpirit>() && projectile.active)
                {
                    TidalSpiritActive = false;
                }
            }
            if (TidalSpiritActive)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<TideSpirit>(), 24, 0, player.whoAmI);
            }
        }
    }

    class TideSpirit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
        }
        public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.width = 36;
            projectile.height = 168 / 4;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 120;
            projectile.timeLeft = 60;
        }

        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 4;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        float direction = 0f;
        int mode = 0;
        int azuriteCounter = 0;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.TideSpirit)
            {
                projectile.active = false;
            }
            else
            {
                projectile.timeLeft = 60;
            }

            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }

            FindFrame(projectile.height);

            bool Targeted = false;
            NPC target = Main.npc[0];

            float Distance = 1000;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    target = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }

            if (!projectile.CanHit(player) || !Targeted || !projectile.CanHit(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            if (mode == 0)
            {
                if (projectile.Distance(player.Center) > 100)
                {
                    float speed = 1f;
                    projectile.velocity += projectile.DirectionTo(player.Center) * speed;
                    projectile.velocity *= 0.96f;
                }
                if (projectile.Distance(player.Center) > 5000)
                {
                    projectile.position = player.Center - new Vector2(projectile.width / 2, projectile.height / 2);
                }
            }

            if (mode == 1)
            {
                if (projectile.Distance(target.Center) > 100)
                {
                    float speed = 1f;
                    projectile.velocity += projectile.DirectionTo(target.Center) * speed;
                    projectile.velocity *= 0.96f;
                }

                if (azuriteCounter > 0)
                {
                    azuriteCounter--;
                }
                else if (superiorPlayer.TerrorPercent >= 3f)
                {
                    superiorPlayer.TerrorPercent -= 3f;
                    azuriteCounter = 40;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item110, projectile.Center);
                    for (int i = 0; i < Main.rand.Next(3, 5); i++)
                    {
                        float speed = Main.rand.Next(25, 40);
                        Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                        Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<Projectiles.AzuriteShard>(), projectile.damage, 1, projectile.owner);
                    }
                }
            }
        }
    }
}


