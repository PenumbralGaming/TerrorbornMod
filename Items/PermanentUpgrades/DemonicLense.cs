using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class DemonicLense : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently allows you to use Shriek of Horror twice as fast" +
                "\nCan only be used once");
        }
        public override void SetDefaults()
        {
            item.rare = 4;
            item.useStyle = 4;
            item.consumable = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.UseSound = SoundID.Item29;
            item.value = Item.sellPrice(0, 10, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.DemonicLense;
        }
        public override bool UseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.DemonicLense)
            {
                item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror use speed doubled", true, false);
                modPlayer.DemonicLense = true;
            }
            return base.UseItem(player);
        }
    }
}
