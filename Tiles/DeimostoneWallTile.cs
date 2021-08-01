using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Tiles
{
    class DeimostoneWallTile : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            drop = ModContent.ItemType<Items.Placeable.Walls.DeimostoneWall>();
            AddMapEntry(new Color(9, 19, 29));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }

    //class DeimostoneWallTileFriendly : ModWall
    //{
    //    public override void SetDefaults()
    //    {
    //        Main.wallHouse[Type] = false;
    //        drop = ModContent.ItemType<Items.Placeable.Walls.DeimostoneWall>();
    //        AddMapEntry(new Color(9, 19, 29));
    //    }
    //    public override bool CanExplode(int i, int j)
    //    {
    //        return false;
    //    }
    //}
}
