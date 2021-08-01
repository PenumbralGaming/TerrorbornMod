using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.UI;
using TerrorbornMod;
using Terraria.Map;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;

namespace TerrorbornMod
{
    class TerrorbornItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public float ShriekDrainBoost = 0f;
        public float ShriekSpeedMultiplier = 1f;

        public float TerrorCost = 0f;

        public bool restless = false;
        public bool guarenteedChargedUse = false;
        public int restlessChargeUpUses = 3;
        public float restlessTerrorDrain;
        public int restlessChargeUpCounter = 0;

        int azureCounter = 2;
        public override bool CanUseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());
            if (player.HasBuff(ModContent.BuffType<Buffs.GraniteSpark>()) || (modPlayer.MidShriek && !darkblood) || !modPlayer.canUseItems)
            {
                return false;
            }
            if (modPlayer.AzuriteArmorBonus && item.magic && base.CanUseItem(item, player) && player.statMana >= item.mana)
            {
                if (azureCounter <= 0)
                {
                    azureCounter = 2;
                    float speed = 22.5f;
                    Vector2 velocity = player.DirectionTo(Main.MouseWorld) * speed;

                    Projectile.NewProjectile(player.Center, velocity, ModContent.ProjectileType<Items.Equipable.Armor.azuriteShockwave>(), item.damage / 3, 30, player.whoAmI);
                }
                else
                {
                    azureCounter--;
                }
            }
            if (TerrorCost > 0f)
            {
                if (modPlayer.TerrorPercent <= TerrorCost)
                {
                    return false;
                }
                else if (base.CanUseItem(item, player))
                {
                    modPlayer.TerrorPercent -= TerrorCost;
                }
            }
            return base.CanUseItem(item, player);
        }

        public bool RestlessChargedUp()
        {
            return restlessChargeUpCounter <= 0;
        }

        public override void PostReforge(Item item)
        {
            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Frightening>())
            {
                ShriekDrainBoost = 0f;
            }

            if (item.prefix != ModContent.PrefixType<Prefixes.Accessories.Startling>())
            {
                ShriekSpeedMultiplier = 1f;
            }
            if (item.prefix != ModContent.PrefixType<Prefixes.Weapons.Nightmarish>())
            {
                TerrorCost = 0f;
            }
        }

        public override bool UseItem(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return base.UseItem(item, player);
        }

        public override void UpdateEquip(Item item, Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (ShriekDrainBoost != 0f)
            {
                modPlayer.ShriekTerrorMultiplier += ShriekDrainBoost;
            }
            if (ShriekSpeedMultiplier != 1f)
            {
                modPlayer.ShriekSpeed *= ShriekSpeedMultiplier;
            }
        }

        public override void SetDefaults(Item item)
        {

        }

        public static TerrorbornItem modItem(Item item)
        {
            return item.GetGlobalItem<TerrorbornItem>();
        }

        bool equippedInVanity = false;
        Color restlessColor = Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255);
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine line = tooltips.FirstOrDefault(x => x.Name == "ItemName" && x.mod == "Terraria");
            if (restless)
            {
                tooltips.Insert(1, new TooltipLine(mod, "restlessItem", "--Restless Weapon--"));
                tooltips.FirstOrDefault(x => x.Name == "restlessItem" && x.mod == "TerrorbornMod").overrideColor = Color.FromNonPremultiplied(128, 75, 135, 255);
                foreach (TooltipLine line2 in tooltips)
                {
                    if (line2.mod == "Terraria" && line2.Name == "ItemName")
                    {
                        line2.overrideColor = restlessColor;
                    }
                }
            }
            line = tooltips.FirstOrDefault(x => x.Name == "Social" && x.mod == "Terraria");
            if (ShriekDrainBoost != 0f && line == null)
            {
                tooltips.Add(new TooltipLine(mod, "shriekterrorprefix", "+" + (ShriekDrainBoost * 100f) + "% Shriek of Horror terror drain"));
                tooltips.FirstOrDefault(x => x.Name == "shriekterrorprefix" && x.mod == "TerrorbornMod").isModifier = true;
            }
            if (ShriekSpeedMultiplier != 1f && line == null)
            {
                bool isBad = true;
                string changeText = "+" + (ShriekSpeedMultiplier - 1) * 100f;
                if (ShriekSpeedMultiplier < 1f)
                {
                    changeText = ((ShriekSpeedMultiplier - 1) * 100f).ToString();
                    isBad = false;
                }

                tooltips.Add(new TooltipLine(mod, "shriekspeedprefix", changeText + "% Shriek of Horror charge up time"));
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.mod == "TerrorbornMod").isModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "shriekspeedprefix" && x.mod == "TerrorbornMod").isModifierBad = isBad;
            }
            if (TerrorCost > 0f)
            {
                string changeText = "+" + TerrorCost;
                tooltips.Add(new TooltipLine(mod, "terrorcostprefix", changeText + "% terror required per use"));
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.mod == "TerrorbornMod").isModifier = true;
                tooltips.FirstOrDefault(x => x.Name == "terrorcostprefix" && x.mod == "TerrorbornMod").isModifierBad = true;
            }
        }

        public override void OnHitNPC(Item item, Player player, NPC target, int damage, float knockBack, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (crit && modPlayer.SangoonBand && target.type != NPCID.TargetDummy)
            {
                if (modPlayer.SangoonBandCooldown <= 0)
                {
                    player.HealEffect(1);
                    player.statLife += 1;
                    modPlayer.SangoonBandCooldown = 20;
                }
            }
        }
    }
}
