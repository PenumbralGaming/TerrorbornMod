using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using TerrorbornMod;
using Terraria.ID;

namespace TerrorbornMod.Abilities
{
    class StarvingStormInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 5;
        }

        public override Texture2D texture()
        {
            return ModContent.GetTexture("TerrorbornMod/Abilities/StarvingStorm_Icon");
        }

        public override float Cost()
        {
            return 50f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Starving Storm";
        }

        public override string Description()
        {
            return "Summons a vortex at your cursor that pulls nearby" +
                 "\nenemies.";
        }

        public override bool canUse(Player player)
        {
            return true;
        }

        public override void OnUse(Player player)
        {
            Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<StormVortex>(), 0, 0, player.whoAmI);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath52, player.Center);
        }
    }

    class StormVortex : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 1;
            projectile.height = 1;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 300;
        }
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(15);
            for (int i = 0; i < 4; i++)
            {
                int dust = Dust.NewDust(projectile.Center, 0, 0, DustID.GoldFlame);
                float dustSpeed = 10f;
                Vector2 rotation = projectile.rotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(90 * i));
                Main.dust[dust].velocity = dustSpeed * rotation;
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].noGravity = true;
                Main.dust[dust].color = Color.White;
            }

            float range = 400f;
            float speedVelocity = 0.75f;
            float speedPosition = 10f;
            for (int i = 0; i < 200; i++)
            {
                NPC target = Main.npc[i];
                if (target.Distance(projectile.Center) < range && !target.friendly && target.active && projectile.CanHit(target))
                {
                    target.velocity += target.DirectionTo(projectile.Center) * speedVelocity * target.knockBackResist;
                    target.position += target.DirectionTo(projectile.Center) * speedPosition * target.knockBackResist;
                }
            }
        }
    }

    class ObtainStarvingStorm : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Starving Storm");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Starving Storm'" +
                "\nRight click to get a list of unlocked abilities");
        }
        public override void SetDefaults()
        {
            item.rare = -12;
            item.autoReuse = false;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = 20;
            item.useAnimation = 20;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer tPlayer = TerrorbornPlayer.modPlayer(player);
            if (player.altFunctionUse != 2)
            {
                if (tPlayer.unlockedAbilities.Contains(5))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(5);
                    Main.NewText("Unlocked 'Starving Storm'");
                }
            }
            else
            {
                if (tPlayer.unlockedAbilities.Count < 1)
                {
                    Main.NewText("No abilities currently unlocked!");
                }
                else
                {
                    for (int i = 0; i < tPlayer.unlockedAbilities.Count; i++)
                    {
                        Main.NewText(TerrorbornUtils.intToAbility(tPlayer.unlockedAbilities[i]).Name());
                    }
                }
            }
            return base.CanUseItem(player);
        }
    }

    class StarvingStorm : TerrorAbility
    {
        public override string TexturePath => "Abilities/StarvingStorm_Icon";

        public override bool HasLockedPosition()
        {
            return false;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(24, 20);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(0, 0);
        }

        public override float getScale()
        {
            return 1.5f;
        }

        public override void ActualAI()
        {
            Float(1.5f, 0.1f);
            UpdateObtainablity(32);
        }

        public override void ObtainAbility()
        {
            projectile.active = false;

            TerrorbornPlayer target = TerrorbornPlayer.modPlayer(Main.player[Player.FindClosest(projectile.position, projectile.width, projectile.height)]);
            target.unlockedAbilities.Add(5);
            target.TriggerAbilityAnimation("Starving Storm", "Summons a vortex at your cursor that pulls nearby enemies", "Costs 50% terror to use", 0, visibilityTime: 800);
        }
    }
}
