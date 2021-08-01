using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.PrototypeI
{
    class PlasmaScepter : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[item.type] = true;
            DisplayName.SetDefault("Scepter of Contamination");
            Tooltip.SetDefault("Fires a stream of dark plasma");
        }
        public override void SetDefaults()
        {
            item.damage = 69;
            item.noMelee = true;
            item.width = 54;
            item.height = 54;
            item.useTime = 4;
            item.useAnimation = 4;
            item.shoot = 10;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 16, 0, 0);
            item.rare = 9;
            item.UseSound = SoundID.Item13;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlasmaSpray");
            item.shootSpeed = 55f / 5f;
            item.mana = 3;
            item.magic = true;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-20, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 74);
            return true;
        }
    }
    class PlasmaSpray : ModProjectile
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
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.hide = true;
            projectile.timeLeft = 100 * 5;
            projectile.extraUpdates = 4;
        }

        int dustWait = 0;
        public override void AI()
        {
            if (dustWait > 0)
            {
                dustWait--;
            }
            else
            {
                dustWait = 5;
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 74, Scale: 1.35f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = projectile.velocity;
            }

            projectile.velocity.Y += 0.15f;
            //projectile.velocity.X *= 0.999f;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 4;
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
