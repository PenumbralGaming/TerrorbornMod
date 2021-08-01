using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore.JournalEntries.Tenebris
{
    class Rath_TidalTitan : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torn Page");
            Tooltip.SetDefault("Written in neat, fancy handwriting it reads:" +
                "\nAmongst the seas are very powerful creatures capable of abusing the terror in the minds of their prey. These fascinating" +
                "\ncreatures, I believe are like the terror incarnates of crabs. Tidal Titans, I believe the peasants call them. That being" +
                "\nsaid, I'm unsure of how much they actually have to do with the tides, but I do know they're capable of splashing massive" +
                "\nwaves, so perhaps that's where they get the name from. From my experience with them at the dock, they seem to be passive;" +
                "\nat least to my kind. Tales I've interviewed others on report of the provoking of such crabs and being attacked- seems to" +
                "\nme as though they got what they deserved. Perhaps studying the powers of these crabs could be beneficial." +
                "\n                                                                   -Rath, top Anekronian Military researcher");
        }
        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}