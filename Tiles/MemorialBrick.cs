using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Tiles
{
    public class MemorialBrick : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            //Main.tileShine[Type] = 1;
            Main.tileLighted[Type] = true;
            Main.tileSpelunker[Type] = true;
            soundType = 21;
            soundStyle = 1;
            //Main.soundDig[Type] =  21;

            minPick = int.MaxValue;
            mineResist = int.MaxValue;
            AddMapEntry(new Color(20, 30, 40));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override bool HasWalkDust()
        {
            return true;
        }

    }
    class MemorialWall : ModWall
    {
        public override void SetDefaults()
        {
            Main.wallHouse[Type] = false;
            AddMapEntry(new Color(20, 30, 40));
        }
        public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override void KillWall(int i, int j, ref bool fail)
        {
            fail = true;
        }
    }
    public class MemorialBrickItem : ModItem
    {
        public override string Texture { get { return "Terraria/Item_" + ItemID.GreenBrick; } }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("DEVELOPER TOOL");
            ItemID.Sets.ExtractinatorMode[item.type] = item.type;
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createTile = mod.TileType("MemorialBrick");
        }
    }
    public class MemorialWallItem : ModItem
    {
        public override string Texture { get { return "Terraria/Item_" + ItemID.GreenBrickWall; } }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("DEVELOPER TOOL");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 18;
            item.maxStack = 999;
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.consumable = true;
            item.createWall = mod.WallType("MemorialWall");
        }
    }
}

