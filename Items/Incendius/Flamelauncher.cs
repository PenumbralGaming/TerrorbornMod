using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Incendius
{
    class Flamelauncher : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Uses Gel as ammo, creating flames" +
                "\nHas a chance to create a lingering flame cloud" +
                "\n95% chance to not consume ammo");
        }
        public override void SetDefaults()
        {
            item.damage = 34;
            item.ranged = true;
            item.noMelee = true;
            item.scale = 0.8f;
            item.width = 58;
            item.height = 32;
            item.useTime = 4;
            item.useAnimation = 12;
            item.reuseDelay = 15;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item61;
            item.shoot = ProjectileID.Flames;
            item.autoReuse = true;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Gel;
        }
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .95f;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (Main.rand.NextFloat() <= .20f)
            {
                float Multiplier = Main.rand.NextFloat(.4f, .6f);
                Projectile.NewProjectile(position, new Vector2(speedX * Multiplier, speedY * Multiplier).RotatedByRandom(MathHelper.ToRadians(10)), mod.ProjectileType("FlameCloud"), damage / 2, 0, item.owner);
            }
            if (speedX > 0)
            {
                position += Vector2.Normalize(new Vector2(speedX, speedY)).RotatedBy(60) * 15f;
            }
            else
            {
                position += Vector2.Normalize(new Vector2(speedX, speedY)).RotatedBy(-60) * 15f;
            }

            int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
            Main.projectile[proj].usesLocalNPCImmunity = true;
            Main.projectile[proj].localNPCHitCooldown = -1;
            Main.projectile[proj].penetrate = -1;
            return false;
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }
    }
    class FlameCloud : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 40;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.timeLeft = 600;
            projectile.ranged = true;
            projectile.hide = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 15;
            projectile.tileCollide = false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, 25, 25, DustID.Fire, Scale: 1.5f);
            Main.dust[dust].noGravity = true;
            projectile.velocity *= 0.96f;
        }
    }
}
