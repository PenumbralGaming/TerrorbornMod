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
    class ContaminatedMarinePistol : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/ContaminatedMarinePistol";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Contaminated Marine Pistol");
        }

        public override string defaultTooltip()
        {
            return "Does nothing of interest by default";
        }

        public override string altTooltip()
        {
            return "Fires a high velocity bullet that causes nightmare flames to erupt from hit enemies";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 27;
            item.ranged = true;
            item.noMelee = true;
            item.width = 38;
            item.height = 18;
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 5;
            item.knockBack = 1;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shootSpeed = 16f;
            item.shoot = 10;
            item.useAmmo = AmmoID.Bullet;
            modItem.restlessChargeUpUses = 10;
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
                Main.projectile[proj].GetGlobalProjectile<TerrorbornProjectile>().ContaminatedMarine = true;
                return false;
            }
            return base.RestlessShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 22);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.TerrorSample>(), 6);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class NightmareBoilRanged : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.timeLeft = 60;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hide = true;
        }

        int moveCounter = 10;
        public override void AI()
        {
            if (moveCounter > 0)
            {
                moveCounter--;
                projectile.position -= projectile.velocity;
            }
            else
            {
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity = projectile.velocity;
                projectile.velocity.Y += 0.2f;
            }
        }
    }
}




