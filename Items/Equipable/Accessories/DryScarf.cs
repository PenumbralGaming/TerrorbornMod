using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class DryScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+1 minion slot" +
                "\n25% increased minion slot count" +
                "\n+1 defense for every two minions active");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 3;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions++;
            player.maxMinions = (int)(player.maxMinions * 1.25f);
            float minionCount = 0;
            for (int i = 0; i < 1000; i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.active) minionCount += projectile.minionSlots;
            }
            player.statDefense += (int)(minionCount / 2);
        }
    }
}

