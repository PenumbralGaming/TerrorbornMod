using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace TerrorbornMod.Items.Materials
{
    class TerrorSample : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'It seems to run from you...'");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 8));
            ItemID.Sets.ItemNoGravity[item.type] = true;
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 18;
            item.height = 26;
            item.rare = 1;
        }

        Color restlessColor = Color.FromNonPremultiplied(40 * 2, 55 * 2, 70 * 2, 255);
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line2 in tooltips)
            {
                if (line2.mod == "Terraria" && line2.Name == "ItemName")
                {
                    line2.overrideColor = restlessColor;
                }
            }
        }

        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange *= 0;
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            Player player = Main.player[Player.FindClosest(item.position, item.width, item.height)];
            if (item.Distance(player.Center) <= 75)
            {
                item.velocity += item.DirectionTo(player.Center) * -0.135f;
            }
            item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
        }
    }
}

