using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class GoldenTooth : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently doubles Shriek of Horror's range" +
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
            item.value = Item.sellPrice(0, 4, 0, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.GoldenTooth;
        }
        public override bool UseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.GoldenTooth)
            {
                item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror range increased by 100%", true, false);
                modPlayer.GoldenTooth = true;
            }
            return base.UseItem(player);
        }
    }
}
