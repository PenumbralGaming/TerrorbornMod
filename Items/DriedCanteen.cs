using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items
{
    class DriedCanteen : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Not consumable" +
                "\nSummons Dunestock");
        }

        public override void SetDefaults()
        {
            item.width = 12;
            item.height = 24;
            item.rare = 3;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ZoneDesert && !NPC.AnyNPCs(mod.NPCType("Dunestock"));
        }
        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("Dunestock"));
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Cobweb, 50);
            recipe.AddIngredient(ItemID.AntlionMandible, 15);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(mod.ItemType("TarOfHunger"), 75);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
