using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Weapons.Melee
{
    class AzuriteGlaive : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Azurite Glaive");
            Tooltip.SetDefault("Fires a piercing azure beam");
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetDefaults()
        {
			item.useStyle = 5;
            item.width = 48;
            item.height = 50;
            item.damage = 28;
            item.melee = true;
            item.noMelee = true;
            item.useTime = 24;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.useAnimation = 18;
            item.shootSpeed = 4;
            item.knockBack = 4;
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("AzuriteGlaiveProjectile");
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[item.shoot] < 1;
        }
    }
    class AzuriteGlaiveProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.aiStyle = 19;
            projectile.penetrate = -1;
            projectile.scale = 1.3f;
            projectile.alpha = 0;
            projectile.hide = true;
            projectile.ownerHitCheck = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.friendly = true;
        }
        public float movementFactor
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }
        bool HasFiredBolt = false;
        public override void AI()
        {

            Player projOwner = Main.player[projectile.owner];
            Vector2 ownerMountedCenter = projOwner.RotatedRelativePoint(projOwner.MountedCenter, true);
            projectile.direction = projOwner.direction;
            projOwner.heldProj = projectile.whoAmI;
            projOwner.itemTime = projOwner.itemAnimation;
            projectile.position.X = ownerMountedCenter.X - (float)(projectile.width / 2);
            projectile.position.Y = ownerMountedCenter.Y - (float)(projectile.height / 2);
            if (!projOwner.frozen)
            {
                if (movementFactor == 0f)
                {
                    movementFactor = 3f;
                    projectile.netUpdate = true;
                }
                if (projOwner.itemAnimation < projOwner.itemAnimationMax / 3)
                {
                    movementFactor -= 2.4f;
                    if (!HasFiredBolt)
                    {
                        float Speed = 27;
                        Vector2 ProjectileVelocity = (projectile.rotation - (MathHelper.ToRadians(135f))).ToRotationVector2() * Speed;
                        if (projectile.spriteDirection == -1)
                        {
                            ProjectileVelocity = (projectile.rotation - (MathHelper.ToRadians(45f))).ToRotationVector2() * Speed;
                        }
                        Projectile.NewProjectile(projectile.Center + (ProjectileVelocity / Speed) * 100, ProjectileVelocity, mod.ProjectileType("AquaRay"), projectile.damage, projectile.knockBack, projectile.owner);
                        HasFiredBolt = true;
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item60, projectile.Center);
                    }
                }
                else
                {
                    movementFactor += 2.1f;
                }
            }
            projectile.position += projectile.velocity * movementFactor;
            if (projOwner.itemAnimation == 0)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(135f);
            if (projOwner.direction == 1)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.spriteDirection == -1)
            {
                projectile.rotation -= MathHelper.ToRadians(90f);
            }
        }
    }
    class AquaRay : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; } }
        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.extraUpdates = 100;
            projectile.timeLeft = 1000;
            projectile.penetrate = 2;
            projectile.hide = true;
        }
        public override void AI()
        {
            projectile.velocity.Y += 0.2f;
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(projectile.position - (projectile.velocity * i / 4), 1, 1, 88, 0, 0, Scale: 2, newColor: Color.SkyBlue);
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
