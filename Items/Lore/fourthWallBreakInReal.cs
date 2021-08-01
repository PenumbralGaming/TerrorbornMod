using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore
{
    class fourthWallBreakInReal : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("A message from the devs");
            Tooltip.SetDefault("Written in neat, fancy handwriting it reads:" +
                "\nThank you for playing the Terrorborn mod! As I'm sure you can tell, it's still a WIP, but the storyline and everything else will be" +
                "\ngreatly expanded on in the future, when we've had the chance to add more story related content throughout the game. If you want" +
                "\nupdates, be sure to join our Discord server! (link on the terraria community forums page)");
        }
        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}
