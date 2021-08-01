using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class ChlorophyteBlowpipe : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses darts as ammo" +
                "\nFires two darts in a row");
        }
        public override void SetDefaults()
        {
            item.damage = 35;
            item.ranged = true;
            item.noMelee = true;
            item.width = 42;
            item.height = 14;
            item.useTime = 5;
            item.useAnimation = item.useTime * 2;
            item.reuseDelay = 20;
            item.useStyle = 101;
            item.knockBack = 0.6f;
            item.value = 0;
            item.shoot = 10;
            item.rare = ItemRarityID.Lime;
            item.autoReuse = true;
            item.shootSpeed = 16;
            item.useAmmo = AmmoID.Dart;
        }
        Vector2 offset = new Vector2(0, 0);
        public override bool UseItemFrame(Player player)
        {
            player.bodyFrame.Y = 2 * player.bodyFrame.Height;
            player.itemLocation = player.Center + offset;
            return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item63, player.Center);
            player.itemRotation = player.DirectionTo(Main.MouseWorld).ToRotation();
            if (player.direction == -1)
            {
                player.itemRotation += MathHelper.ToRadians(180);
            }
            position = player.Center + offset;
            position.Y -= 3;
            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = 10;
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
            recipe.AddIngredient(ItemID.ChlorophyteBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}



