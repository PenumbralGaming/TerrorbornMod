using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class RubyIncense : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ruby, 5);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreased enemy defense by 4." +
                "\nEvery 3 seconds, you get healed by 1 for every 3 enemies nearby.");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 44;
            item.accessory = true;
            item.noMelee = true;
            item.rare = 2;
            item.value = Item.sellPrice(gold: 1, silver: 25);
            item.useAnimation = 5;
        }

        int HealCounter = 180;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.armorPenetration += 4;
            int enemyCount = 0;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.CanBeChasedBy() && npc.type != NPCID.EaterofWorldsBody && npc.type != NPCID.TheDestroyerBody)
                {
                    enemyCount++;
                }
            }
            HealCounter--;
            if (HealCounter <= 0 && (int)(enemyCount / 2) != 0)
            {
                player.HealEffect(enemyCount / 3);
                player.statLife += enemyCount / 3;
                HealCounter = 180;
            }
        }
    }
}




