using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class CoreOfFear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently Allows you to move at 30% of your normal speed during Shriek of Horror" +
                "\nCan only be used once");
        }
        public override void SetDefaults()
        {
            item.rare = 3;
            item.useStyle = 4;
            item.consumable = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.UseSound = SoundID.Item29;
            item.value = Item.sellPrice(0, 1, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.CoreOfFear;
        }
        public override bool UseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.CoreOfFear)
            {
                item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror movement speed increased by 30%", true, false);
                modPlayer.CoreOfFear = true;
            }
            return base.UseItem(player);
        }
    }
}
