using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod
{
    class CombatTokenCurrency : CustomCurrencySingleCoin
    {
        public Color CombatTokenCurrencyTextColor = Color.FromNonPremultiplied(186, 75, 15, 255);

        public CombatTokenCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
        {
        }

        public override void GetPriceText(string[] lines, ref int currentLine, int price)
        {
            Color color = CombatTokenCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
            lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
                {
                    color.R,
                    color.G,
                    color.B,
                    Language.GetTextValue("LegacyTooltip.50"),
                    price,
                    "Combat Tokens"
                });
        }
    }
    class CombatToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'The reward for peacekeeping'");
        }
        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 28;
            item.height = 30;
            item.value = 100;
            item.rare = -11;
        }
    }
}
