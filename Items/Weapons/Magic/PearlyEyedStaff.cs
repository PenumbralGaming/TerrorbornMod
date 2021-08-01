using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class PearlyEyedStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            Tooltip.SetDefault("Fires a ray of light that splits into many smaller shards upon getting close to an enemy");
        }
        public override void SetDefaults()
        {
            item.damage = 25;
            item.noMelee = true;
            item.width = 46;
            item.height = 46;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item72;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PearlLightRay");
            item.shootSpeed = 15f;
            item.mana = 4;
            item.magic = true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 46);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, item.owner, 1);
            return false;
        }
    }

    class PearlLightRay : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 350;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 63, 0f, 0f, 100, Color.White, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            int RotatationSpeed = 2;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * Direction * projectile.ai[0]));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }


            Rectangle closeRectangle = projectile.getRect();
            int extraWidth = 160;
            int extraHeight = 160;
            closeRectangle.Width += extraWidth;
            closeRectangle.Height += extraHeight;
            closeRectangle.X -= extraWidth / 2;
            closeRectangle.Y -= extraHeight / 2;

            bool intersects = false;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.getRect().Intersects(closeRectangle) && npc.active && !npc.dontTakeDamage)
                {
                    intersects = true;
                }
            }

            if (intersects)
            {
                projectile.timeLeft = 0;
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item110, projectile.Center);
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    float speed = 1f;
                    Vector2 velocity = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<ShardOfLight>(), projectile.damage, 1, projectile.owner);
                }
            }
        }
    }
    class ShardOfLight : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }

        public override void SetDefaults()
        {
            projectile.width = 5;
            projectile.height = 5;
            projectile.magic = true;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 60;
            projectile.hide = true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 63, 0f, 0f, 100, Color.White, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;

            Vector2 rotation = (projectile.velocity.ToRotation()).ToRotationVector2();
            float Speed = 1f;
            projectile.velocity += rotation * Speed;
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
                Vector2 direction = projectile.DirectionTo(targetNPC.Center);
                projectile.velocity += direction * speed;
            }
        }
    }
}

