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
    class NecromanticCurseInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 4;
        }

        public override Texture2D texture()
        {
            return ModContent.GetTexture("TerrorbornMod/Abilities/NecromanticCurse_Icon");
        }

        public override float Cost()
        {
            return 30f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Necromantic Curse";
        }

        public override string Description()
        {
            return "Fires out a dungeon spirit that does 1 damage, but" +
                 "\nmakes your attacks lifesteal from the hit enemy.";
        }

        public override bool canUse(Player player)
        {
            return true;
        }

        public override void OnUse(Player player)
        {
            float speed = 20;
            Vector2 velocity = player.DirectionTo(Main.MouseWorld) * speed;
            int proj = Projectile.NewProjectile(player.Center, velocity, ModContent.ProjectileType<DungeonSpirit>(), 1, 5, player.whoAmI);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath52, player.Center);

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.SanguineSetBonus)
            {
                Main.projectile[proj].penetrate = 4;
            }
        }
    }

    class DungeonSpirit : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 20;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.player[projectile.owner]);
            int type = 132;
            if (modPlayer.SanguineSetBonus) type = 130;
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, type);
            Main.dust[dust].velocity = projectile.velocity / 4;
            if (modPlayer.SanguineSetBonus) Main.dust[dust].velocity = projectile.velocity / 10;
            Main.dust[dust].scale = 1.5f;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;

            if (modPlayer.SanguineSetBonus)
            {
                NPC targetNPC = Main.npc[0];
                float Distance = 1000; //max distance away
                bool Targeted = false;
                for (int i = 0; i < 200; i++)
                {
                    if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy() && projectile.localNPCImmunity[i] == 0)
                    {
                        targetNPC = Main.npc[i];
                        Distance = Main.npc[i].Distance(projectile.Center);
                        Targeted = true;
                    }
                }
                if (Targeted)
                {
                    //HOME IN
                    projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
                }
            }
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            TerrorbornNPC modNPC = TerrorbornNPC.modNPC(target);
            modNPC.soulSplitTime = 60 * 5;
            CombatText.NewText(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height), Color.LightCyan, "Soul Split");
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item103, target.Center);
        }
    }

    class ObtainNecromanticCurse : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Necromantic Curse");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Necromantic Curse'" +
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
                if (tPlayer.unlockedAbilities.Contains(4))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(4);
                    Main.NewText("Unlocked 'Necromantic Curse'");
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
    class NecromanticCurse : TerrorAbility
    {
        public override string TexturePath => "Abilities/NecromanticCurse_Icon";

        public override Vector2 lockedPosition()
        {
            return new Vector2(Main.dungeonX * 16, Main.dungeonY * 16);
        }

        public override Vector2 dimensions()
        {
            return new Vector2(24, 24);
        }

        public override Vector2 baseOffsets()
        {
            return new Vector2(0, -100);
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
            target.unlockedAbilities.Add(4);
            target.TriggerAbilityAnimation("Necromantic Curse", "Fires out a dungeon spirit that does 1 damage, but makes your attacks lifesteal from the hit enemy", "Costs 30% terror to use", 0, visibilityTime: 800);
        }
    }
}


