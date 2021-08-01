using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class PlatinumBlowpipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nRight click to fire slower, but cause your darts to move quicker and deal slightly more damage");
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 12;
            item.useTime = 35;
            item.useAnimation = 35;
            item.useStyle = 101;
            item.knockBack = 0.6f;
            item.value = 0;
            item.shoot = 10;
            item.rare = 0;
            item.UseSound = SoundID.Item63;
            item.autoReuse = true;
            item.shootSpeed = 11.25f;
            item.useAmmo = AmmoID.Dart;
        }
        Vector2 offset = new Vector2(0, 1);
        public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                item.reuseDelay = 10;
            }
            else
            {
                item.reuseDelay = 0;
            }
            return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse == 2)
            {
                speedX *= 2;
                speedY *= 2;
                damage = (int)(damage * 1.05f);
            }
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            position = player.Center + offset;
            position.Y -= 3;
            return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool UseItem(Player player)
        {
            player.bodyFrame.Y = 56 * 2;
            player.itemAnimation = 2;
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PlatinumBar, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}

