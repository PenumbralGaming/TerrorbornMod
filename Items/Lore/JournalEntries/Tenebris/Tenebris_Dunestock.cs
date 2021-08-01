using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;

namespace TerrorbornMod.Items.Lore.JournalEntries.Tenebris
{
    class Tenebris_Dunestock : ModItem
    {
        public override string Texture => "TerrorbornMod/TornPage";
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Torn Page");
            Tooltip.SetDefault("Written in fiery, royal handwriting it reads:" +
                "\nMore and more of my men fall to, well, whatever that strange beast in the desert is. The beast must die, one way or another. That" +
                "\nbeing said, I'm rather unsure how hostile this thing actually is. They say it brings strong winds along with it, however winds that" +
                "\nmy warriors say don't push- a hallucination, perhaps? If I could express a sigh with letters, I would. Maybe this entire thing is a" +
                "\nstrange, hungry hallucination my warriors keep having while travelling the desert. What's the point of all this, anyway? It's not like" +
                "\nthat cog-driven madland is worth visiting for trade. All they ever offer us is the stupidest of gadgets. Who needs to defend their" +
                "\ncity with tech when you can use the power of mind games to overwhelm your foe. Maybe I should reconsider my allyship with them." +
                "\n                                                                   -Tenebris, king of Anekronyx");
        }
        public override void SetDefaults()
        {
            item.value = 0;
            item.rare = -11;
        }
    }
}
