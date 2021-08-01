using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.PermanentUpgrades
{
    class EyeOfTheMenace : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Permanently increases the Terror gained from using Shriek of Horror by 50%" +
                "\nCan only be used once");
        }
        public override void SetDefaults()
        {
            item.rare = 2;
            item.useStyle = 4;
            item.consumable = true;
            item.useAnimation = 30;
            item.useTime = 30;
            item.UseSound = SoundID.Item29;
            item.value = Item.sellPrice(0, 0, 50, 0);
        }
        public override bool CanUseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            return !modPlayer.EyeOfTheMenace;
        }
        public override bool UseItem(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!modPlayer.EyeOfTheMenace)
            {
                item.stack--;
                CombatText.NewText(new Rectangle((int)(player.Center.X - 50), (int)(player.Center.Y - 50), 100, 10), Color.Red, "Shriek of Horror terror drain increased by 50%", true, false);
                modPlayer.EyeOfTheMenace = true;
            }
            return base.UseItem(player);
        }
    }
}
