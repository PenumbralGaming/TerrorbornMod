using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System;

namespace TerrorbornMod.Items.Shadowcrawler
{
    public class BladeOfShade : ModItem
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
            Tooltip.SetDefault("Hitting enemies causes Nightmare flames to erupt from them towards your cursor");
        }

        public override void SetDefaults()
        {
            item.damage = 61;
            item.melee = true;
            item.width = 52;
            item.height = 52;
            item.useTime = 8;
            item.useAnimation = 8;
            item.useStyle = 1;
            item.knockBack = 1;
            item.value = Item.buyPrice(gold: 2, silver: 70);
            item.rare = 5;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(target.Center, target.DirectionTo(mousePosition).RotatedByRandom(MathHelper.ToRadians(5)), ModContent.ProjectileType<NightmareBoilMelee>(), item.damage / 2, knockBack, Owner: item.owner);
            }
        }
    }

    class NightmareBoilMelee : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.melee = true;
            //projectile.extraUpdates = 100;
            projectile.timeLeft = 60;
            projectile.penetrate = 5;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hide = true;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 74, 0f, 0f, 100, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            float rotation = projectile.velocity.ToRotation() - MathHelper.ToRadians(180);
            float Speed = 1f;
            projectile.velocity += new Vector2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
        }
    }
}

