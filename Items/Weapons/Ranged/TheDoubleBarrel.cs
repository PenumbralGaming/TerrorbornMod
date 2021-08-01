using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class TheDoubleBarrel : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Quadruple Barrel");
            Tooltip.SetDefault("Fires many bullets at once" +
                "\nCan be fired as quick as you want for 4 shots before needing to be reloaded" +
                "\nReloads automatically when not firing");
        }
        public override void SetDefaults()
        {
            item.damage = 18;
            item.ranged = true;
            item.noMelee = true;
            item.width = 60;
            item.height = 22;
            item.useTime = 8;
            item.useAnimation = 8;
            item.shoot = 10;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 5;
            item.autoReuse = false;
            item.shootSpeed = 16f;
            item.useAmmo = AmmoID.Bullet;
            item.holdStyle = ItemHoldStyleID.HoldingOut;
        }

        public override void HoldStyle(Player player)
        {
            player.itemLocation = player.position + new Vector2(-20, 10);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        int useCounter = 0;
        int shotsLeft = 0;
        public override bool CanUseItem(Player player)
        {
            return shotsLeft > 0;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            useCounter = 35;
            shotsLeft--;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item36, player.position);
            for (int i = 0; i < Main.rand.Next(4, 7); i++)
            {
                Vector2 EditedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(13)) * Main.rand.NextFloat(0.7f, 1.3f);
                Projectile.NewProjectile(position, EditedSpeed, type, damage, knockBack, item.owner);
            }
            return false;
        }

        public override void UpdateInventory(Player player)
        {
            if (useCounter > 0)
            {
                useCounter--;
                if (useCounter <= 0)
                {
                    CombatText.NewText(player.getRect(), Color.White, "Reloaded");
                }
            }
            else
            {
                shotsLeft = 4;
            }
        }
    }
}
