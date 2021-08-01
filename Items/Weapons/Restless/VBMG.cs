using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class VBMG : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/VBMG";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Vein Buster");
        }

        public override string defaultTooltip()
        {
            return "Does nothing of interest by default";
        }

        public override string altTooltip()
        {
            return "Fires a high velocity bullet that causes blood to erupt from hit enemies";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 10;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.shoot = 10;
            item.useAmmo = AmmoID.Bullet;
            modItem.restlessChargeUpUses = 5;
            modItem.restlessTerrorDrain = 8;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }
        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].extraUpdates = Main.projectile[proj].extraUpdates * 2 + 1;
                Main.projectile[proj].GetGlobalProjectile<TerrorbornProjectile>().VeinBurster = true;
                return false;
            }
            return base.RestlessShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.CrimtaneBar, 10);
            recipe.AddIngredient(ItemID.TissueSample, 5);
            recipe.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemID.DemoniteBar, 10);
            recipe2.AddIngredient(ItemID.ShadowScale, 5);
            recipe2.AddIngredient(mod.ItemType("SanguineFang"), 12);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 5);
            recipe2.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}

