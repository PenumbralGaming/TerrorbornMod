using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DualpipeDartgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires two darts at once in an even spread");
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.noMelee = true;
            item.width = 44;
            item.height = 16;
            item.useTime = 13;
            item.useAnimation = 13;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 0.6f;
            item.value = 0;
            item.shoot = 10;
            item.rare = 2;
            item.UseSound = SoundID.Item98;
            item.autoReuse = false;
            item.shootSpeed = 20f;
            item.scale = 0.85f;
            item.useAmmo = AmmoID.Dart;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int spread = 4;
            Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-spread)), type, damage, knockBack, player.whoAmI);
            Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(spread)), type, damage, knockBack, player.whoAmI);
            return false;
        }
        public override bool UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            player.itemAnimation = 2;
            return true;
        }
    }
}




