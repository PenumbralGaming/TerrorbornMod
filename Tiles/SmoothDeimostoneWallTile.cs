using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Tiles
{
    class SmoothDeimostoneWallTile : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = ModContent.ItemType<Items.Placeable.Walls.SmoothDeimostoneWall>();
            AddMapEntry(new Color(9, 19, 29));
        }

    }
}

