using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class HorrificCharm : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Spawns a horrific eye above you that cries damaging tears when enemies are nearby");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.summon = true;
            item.rare = 1;
            item.damage = 10;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.HorrificCharm = true;

            bool AntlionShellShieldActive = true;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.type == ModContent.ProjectileType<HorrificEye>() && projectile.active)
                {
                    AntlionShellShieldActive = false;
                }
            }
            if (AntlionShellShieldActive)
            {
                Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<HorrificEye>(), item.damage, 0, player.whoAmI);
            }
        }
    }

    class HorrificEye : ModProjectile
    {
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 position = projectile.Center - Main.screenPosition;
            position.Y += 4;
            Main.spriteBatch.Draw(texture, position, new Rectangle(0, 0, projectile.width, projectile.height), Color.White, projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), projectile.scale, SpriteEffects.None, 0);
            return false;
        }

        public override void SetDefaults()
        {
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.width = 52;
            projectile.height = 30;
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

        int scaleDirection = 1;
        int projectileCounter = 60;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer superiorPlayer = TerrorbornPlayer.modPlayer(player);
            if (!superiorPlayer.HorrificCharm)
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

            projectile.position = player.Center + new Vector2(0, -55);
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;

            if (projectileCounter > 0)
            {
                projectileCounter--;
            }

            NPC targetNPC = Main.npc[0];
            float Distance = 1000; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && projectile.CanHit(Main.npc[i]))
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                if (projectileCounter <= 0)
                {
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item8, projectile.Center);
                    projectileCounter = 40;
                    Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
                    float speed = Main.rand.Next(5, 8);
                    Vector2 velocity = direction * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<HorrificTear>(), projectile.damage, 1f, projectile.owner);
                }
            }

            //pulse
            if (scaleDirection == 1)
            {
                projectile.scale += 0.3f / 60;
                if (projectile.scale >= 1.1f)
                {
                    scaleDirection = -1;
                }
            }
            else
            {
                projectile.scale -= 0.3f / 60;
                if (projectile.scale <= 0.9f)
                {
                    scaleDirection = 1;
                }
            }
        }
    }

    class HorrificTear : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Magic/TarSwarm";
        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = 180;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight, Scale: 1.35f, newColor: Color.White);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity / 3;
            Main.dust[dust].noLight = true;

            projectile.velocity.Y += 0.15f;
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
                projectile.velocity.Y = -oldVelocity.Y * 0.9f;
            }
            return false;
        }
    }
}


