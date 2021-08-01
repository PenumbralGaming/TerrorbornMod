using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class AnekronianApple : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently increases Shriek of Horror movement speed by 20%" +
                "\nCan only be used once");
        }
        public override void SetDefaults()
        {
            item.rare = 5;
            item.useStyle = ItemUseStyleID.EatingUsing;
            item.consumable = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.UseSound = SoundID.Item2;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.AnekronianApple;
        }
        public override bool UseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.AnekronianApple)
            {
                item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror movement speed increased by 20%", true, false);
                modPlayer.AnekronianApple = true;
            }
            return base.UseItem(player);
        }
    }
}

