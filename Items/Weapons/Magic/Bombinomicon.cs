using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class Bombinomicon : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a random explosive" +
                "\n[c/ff0000:These explosives CAN and WILL destroy tiles]" +
                "\n'The last person to use this lost his eye, which haunts him every October'");
        }
        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.width = 24;
            item.height = 26;
            item.magic = true;
            item.damage = 60;
            item.useTime = 30;
            item.useAnimation = 30;
            item.mana = 20;
            item.rare = 4;
            item.shoot = ProjectileID.Bomb;
            item.shootSpeed = 10;
            item.UseSound = SoundID.Item117;
            item.useStyle = 5;
            item.knockBack = 8;
            item.autoReuse = true;
        }
        public override bool CanUseItem(Player player)
        {
            int choice = Main.rand.Next(3);
            int subtypeChoice = Main.rand.Next(2);
            if (choice == 0)
            {
                item.shoot = ProjectileID.Grenade;
                if (Main.rand.Next(4) == 0)
                {
                    if (subtypeChoice == 0)
                    {
                        item.shoot = ProjectileID.StickyGrenade;
                    }
                    else
                    {
                        item.shoot = ProjectileID.BouncyGrenade;
                    }
                }
            }
            if (choice == 1)
            {
                item.shoot = ProjectileID.Bomb;
                if (Main.rand.Next(4) == 0)
                {
                    if (subtypeChoice == 0)
                    {
                        item.shoot = ProjectileID.StickyBomb;
                    }
                    else
                    {
                        item.shoot = ProjectileID.BouncyBomb;
                    }
                }
            }
            if (choice == 2)
            {
                item.shoot = ProjectileID.Dynamite;
                if (Main.rand.Next(4) == 0)
                {
                    if (subtypeChoice == 0)
                    {
                        item.shoot = ProjectileID.StickyDynamite;
                    }
                    else
                    {
                        item.shoot = ProjectileID.BouncyDynamite;
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }
}
