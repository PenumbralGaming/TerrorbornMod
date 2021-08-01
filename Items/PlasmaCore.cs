using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items
{
    class PlasmaCore : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 30);
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddIngredient(ItemID.MartianConduitPlating, 100);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a machine controlled by conflicting forces" +
                "\nNot consumable");
        }
        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 40;
            item.value = 0;
            item.rare = 8;
            item.useStyle = 4;
            item.consumable = false;
            item.useAnimation = 30;
            item.useTime = 30;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(mod.NPCType("PrototypeI"));
        }
        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, mod.NPCType("PrototypeI"));
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            return true;
        }
    }
}
