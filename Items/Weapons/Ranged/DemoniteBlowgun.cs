using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DemoniteBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Vile Blowgun");
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nEvery third shot consumes 1% terror to fire 2 darts at once");
        }
        public override void SetDefaults()
        {
            item.damage = 15;
            item.ranged = true;
            item.noMelee = true;
            item.width = 44;
            item.height = 16;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 101;
            item.knockBack = 0.6f;
            item.value = 0;
            item.shoot = 10;
            item.rare = 1;
            item.UseSound = SoundID.Item64;
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

        int twoCounter = 3;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + offset;
            position.Y -= 3;
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            twoCounter--;
            int maxRotation = 10;
            if (twoCounter <= 0)
            {
                twoCounter = 3;
                float cost = 1f;
                if (modPlayer.TerrorPercent >= cost)
                {
                    Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(maxRotation)), type, damage, knockBack, player.whoAmI);
                    modPlayer.TerrorPercent -= cost;
                }
            }
            Vector2 newSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(maxRotation));
            speedX = newSpeed.X;
            speedY = newSpeed.Y;
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
            recipe.AddIngredient(ItemID.DemoniteBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}


