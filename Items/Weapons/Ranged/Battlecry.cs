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

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class Battlecry : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Battlecry");
            Tooltip.SetDefault("33% chance to not consume ammo" +
                "\nLeft click converts bullets into homing bullets that drain 0.5% terror per hit" +
                "\nRight click to fire much faster, but with less accuracy, consuming 1.5% terror per shot" +
                "\n'Spray n' pray!'");
        }

        public override void SetDefaults()
        {
            item.damage = 14;
            item.ranged = true;
            item.noMelee = true;
            item.width = 50;
            item.height = 30;
            item.useTime = 7;
            item.useAnimation = 7;
            item.useStyle = 5;
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.shoot = 10;
            item.autoReuse = true;
            item.scale = 0.75f;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Bullet;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        float percentCost = 1.5f;
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.altFunctionUse == 2)
            {
                item.reuseDelay = 0;
                return modPlayer.TerrorPercent > percentCost;
            }
            else
            {
                item.reuseDelay = 7;
            }
            return base.CanUseItem(player);
        }

        // 33% chance not to consume ammo
        public override bool ConsumeAmmo(Player player)
        {
            return Main.rand.NextFloat() >= .33f;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-5, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe1 = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe1.AddIngredient(ItemID.DemoniteBar, 15);
            recipe1.AddIngredient(ItemID.IllegalGunParts);
            recipe1.AddTile(TileID.Anvils);
            recipe1.SetResult(this);
            recipe1.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.DirtBlock, 10);
            recipe2.AddIngredient(ItemID.CrimtaneBar, 15);
            recipe2.AddIngredient(ItemID.IllegalGunParts);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            position = player.Center + (player.DirectionTo(Main.MouseWorld) * 50 * item.scale);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.altFunctionUse == 2)
            {
                Vector2 rotatedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                speedX = rotatedSpeed.X;
                speedY = rotatedSpeed.Y;
                modPlayer.TerrorPercent -= percentCost;
            }
            else
            {
                type = ModContent.ProjectileType<BattleBullet>();

            }
            return true;
        }
    }

    class BattleBullet : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 2;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.hide = true;
            projectile.timeLeft = 250;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustPerfect(projectile.Center, 21);
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;

            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2.5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TerrorPercent += 0.5f;
        }
    }

}