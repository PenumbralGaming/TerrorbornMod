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
    class GelatinArmorInfo : AbilityInfo
    {
        public override int typeInt()
        {
            return 6;
        }

        public override Texture2D texture()
        {
            return ModContent.GetTexture("TerrorbornMod/Abilities/GelatinArmor_Icon");
        }

        public override float Cost()
        {
            return 60f;
        }

        public override bool HeldDown()
        {
            return false;
        }

        public override string Name()
        {
            return "Gelatin Armor";
        }

        public override string Description()
        {
            return "Forms a shield around you for 25 seconds that can" +
                 "\nblock a single attack." +
                 "\nIf it blocks an attack, the next hit you take will deal" +
                 "\nextra damage.";
        }

        public override bool canUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return modPlayer.GelatinArmorTime <= 0;
        }

        public override void OnUse(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.GelatinArmorTime = 60 * 25;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item117);
        }
    }

    class ObtainGelatinArmor : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Get Gelatin Armor");
            Tooltip.SetDefault("--UNOBTAINABLE TESTING ITEM--" +
                "\nUnlocks 'Gelatin Armor'" +
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
                if (tPlayer.unlockedAbilities.Contains(6))
                {
                    Main.NewText("You already have that ability, silly!");
                }
                else
                {
                    tPlayer.unlockedAbilities.Add(6);
                    Main.NewText("Unlocked 'Gelatin Armor'");
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

    class GelatinArmor : TerrorAbility
    {
        public override string TexturePath => "Abilities/GelatinArmor_Icon";

        public override bool HasLockedPosition()
        {
            return false;
        }

        public override Vector2 dimensions()
        {
            return new Vector2(24, 26);
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
            target.unlockedAbilities.Add(6);
            target.TriggerAbilityAnimation("Gelatin Armor", "Forms a shield around you for 15 seconds that can block a single attack, consuming 60% terror", "If it blocks an attack, the next hit you take will deal extra damage", 0, visibilityTime: 800);
        }
    }
}

