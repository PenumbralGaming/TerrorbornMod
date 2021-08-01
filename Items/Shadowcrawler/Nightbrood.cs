using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Shadowcrawler
{
    class Nightbrood : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires eight arrows at once" +
                "\nThe accuracy of the arrows increases as you fire but resets when you stop firing");
        }

        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.width = 26;
            item.height = 56;
            item.useTime = 40;
            item.useAnimation = 40;
            item.useStyle = 5;
            item.noMelee = true;
            item.channel = true;
            item.knockBack = 2;
            item.rare = 5;
            item.UseSound = SoundID.DD2_BallistaTowerShot;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 18f;
            item.useAmmo = AmmoID.Arrow;
            item.value = Item.sellPrice(0, 5, 0, 0);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(2f, 0);
        }

        int accuracy = 45;
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity = velocity.RotatedByRandom(MathHelper.ToRadians(accuracy));
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            }

            if (accuracy > 10)
            {
                accuracy -= 5;
            }

            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.channel)
            {
                accuracy = 45;
            }
        }

    }
}
