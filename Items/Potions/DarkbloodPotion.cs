using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Potions
{
    public class DarkbloodPotion : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows you to use items while using Shriek of Horror" +
                "\nDamaging enemies grants you a small amount of terror over time");
        }
        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 2;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 1;
            item.autoReuse = false;
            item.UseSound = SoundID.Item3;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<Buffs.Darkblood>();
            item.buffTime = 3600 * 10;
        }
    }
}
