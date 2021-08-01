using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class SightForSoreEyes : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bubble Wand");
            Tooltip.SetDefault("Rapidly fires bubbles that vary in speed");
            Item.staff[item.type] = true;
        }
        public override void SetDefaults()
        {
            item.rare = 2;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.magic = true;
            item.UseSound = SoundID.Item13;
            item.mana = 2;
            item.damage = 8;
            item.shootSpeed = 10;
            item.knockBack = 1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shoot = mod.ProjectileType("MagicBubble");
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(position, new Vector2(speedX * Main.rand.NextFloat(1, 2.5f), speedY * Main.rand.NextFloat(1, 2.5f)).RotatedByRandom(MathHelper.ToRadians(5)), type, item.damage, knockBack, Owner: item.owner);
            }
            return false;
        }
    }
    class MagicBubble : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Bubble";
        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.hide = false;
        }
        public override void AI()
        {
            projectile.velocity *= 0.98f;
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 10;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54, projectile.position);
        }
    }
}
