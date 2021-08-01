using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class CrimtaneBlowgun : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bloodied Shotpipe");
            Tooltip.SetDefault("Costs 0.5% terror to use" +
                "\nUses darts as ammo and fires 3-4 darts at once");
        }
        public override void SetDefaults()
        {
            item.damage = 12;
            item.ranged = true;
            item.noMelee = true;
            item.width = 44;
            item.height = 16;
            item.useTime = 45;
            item.useAnimation = 45;
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

        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return modPlayer.TerrorPercent >= 0.5f;
        }

        Vector2 offset = new Vector2(0, 1);
        public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TerrorPercent -= 0.5f;
            position = player.Center + offset;
            position.Y -= 3;
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            int maxRotation = 15;
            for (int i = 0; i < Main.rand.Next(3, 5); i++)
            {
                Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(maxRotation)), type, damage, knockBack, player.whoAmI);
            }
            return false;
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
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}



