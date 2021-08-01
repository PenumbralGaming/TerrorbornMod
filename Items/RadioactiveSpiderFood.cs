using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items
{
    class RadioactiveSpiderFood : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("bugs", 2);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofFright);
            recipe.AddIngredient(ItemID.SoulofSight);
            recipe.AddIngredient(ItemID.SoulofMight);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        int Shadowcrawler = ModContent.NPCType<NPCs.Bosses.Shadowcrawler>();
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an ancient foe of Anekronyx");
        }
        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 26;
            item.maxStack = 20;
            item.rare = 7;
            item.useStyle = 4;
            item.consumable = true;
            item.useAnimation = 30;
            item.useTime = 30;
        }
        public override bool CanUseItem(Player player)
        {
            return !Main.dayTime && !NPC.AnyNPCs(Shadowcrawler);
        }
        public override bool UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, Shadowcrawler);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Roar, player.position, 0);
            TerrorbornMod.ScreenShake(50);
            return true;
        }
    }
}
