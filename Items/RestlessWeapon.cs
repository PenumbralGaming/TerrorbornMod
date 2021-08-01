using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    abstract class RestlessWeapon : ModItem
    {
        public override string Texture => "TerrorbornMod/placeholder";
        public override void SetDefaults()
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.restless = true;
            item.autoReuse = true;
            restlessSetDefaults(modItem);
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(" ");
            restlessSetStaticDefaults();
        }

        public virtual void restlessSetStaticDefaults()
        {

        }

        public virtual void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 1;
            modItem.restlessTerrorDrain = 5;
            modItem.restlessChargeUpUses = 4;
        }

        public virtual string altTooltip()
        {
            return "alt";
        }

        public virtual string defaultTooltip()
        {
            return "default";
        }

        public override bool CanUseItem(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if ((player.itemAnimation <= 0 && !player.controlUseItem) || modPlayer.TerrorPercent < modItem.restlessTerrorDrain)
            {
                modItem.restlessChargeUpCounter = modItem.restlessChargeUpUses;
            }

            if (RestlessCanUseItem(player))
            {
                return true;
            }
            return false;
        }

        public virtual bool RestlessCanUseItem(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modItem.restlessChargeUpCounter--;
            if (modItem.restlessChargeUpCounter <= 0)
            {
                modPlayer.TerrorPercent -= modItem.restlessTerrorDrain;
            }
            return RestlessShoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public virtual bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
        }

        public override void UpdateInventory(Player player)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if ((player.itemAnimation <= 0 && !player.controlUseItem) || modPlayer.TerrorPercent < modItem.restlessTerrorDrain)
            {
                modItem.restlessChargeUpCounter = modItem.restlessChargeUpUses;
            }
            RestlessUpdateInventory(player);
        }

        public virtual void RestlessUpdateInventory(Player player)
        {

        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            tooltips.RemoveAt(index);

            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            tooltips.Insert(index, new TooltipLine(mod, "restlessInstructions", "Requires " + modItem.restlessChargeUpUses + " uses in a row and at least " + modItem.restlessTerrorDrain + "% terror to charge up" +
                "\nHold the Auto Select key to show charged up info"));

            if (Main.LocalPlayer.controlTorch)
            {
                tooltips.Insert(index, new TooltipLine(mod, "restlessAltTooltip", altTooltip()));
                tooltips.FirstOrDefault(x => x.Name == "restlessAltTooltip" && x.mod == "TerrorbornMod").overrideColor = Color.FromNonPremultiplied(255, 251, 168, 255);
            }
            else
            {
                tooltips.Insert(index, new TooltipLine(mod, "restlessDefaultTooltip", defaultTooltip()));
                tooltips.FirstOrDefault(x => x.Name == "restlessDefaultTooltip" && x.mod == "TerrorbornMod").overrideColor = Color.FromNonPremultiplied(255, 168, 168, 255);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
            mult = modPlayer.restlessDamage;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            base.GetWeaponKnockback(player, ref knockback);
            knockback *= modPlayer.restlessKnockback;
        }

        //public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        //{
        //    for (int i = 0; i < Main.rand.Next(1, 4); i++)
        //    {
        //        Texture2D texture = ModContent.GetTexture(Texture);
        //        Vector2 offset = new Vector2(Main.rand.Next(-2, 3), Main.rand.Next(-2, 3));
        //        spriteBatch.Draw(texture, item.Center - Main.screenPosition - new Vector2(0, texture.Height / 2 - 2) + offset, new Rectangle(0, 0, texture.Width, texture.Height), Color.FromNonPremultiplied(lightColor.R / 2, lightColor.G / 2, lightColor.B / 2, alphaColor.A / 2), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
        //    }
        //    return true;
        //}

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            for (int i = 0; i < Main.rand.Next(1, 4); i++)
            {
                Texture2D texture = ModContent.GetTexture(Texture);
                Vector2 offset = new Vector2(Main.rand.Next(-1, 2), Main.rand.Next(-1, 2));
                spriteBatch.Draw(texture, position + offset, frame, Color.FromNonPremultiplied(drawColor.R / 2, drawColor.G / 2, drawColor.B / 2, 255 / 2), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }
}
