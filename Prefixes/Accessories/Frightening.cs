using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Prefixes.Accessories
{
    class Frightening : ModPrefix
    {
        public override bool Autoload(ref string name)
        {
            name = "Frightening";
            return base.Autoload(ref name);
        }

        public override PrefixCategory Category => PrefixCategory.Accessory;

        public override void SetDefaults()
        {

        }


        public override void Apply(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.ShriekDrainBoost = 0.08f;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.4982f;
            base.ModifyValue(ref valueMult);
        }
    }
}
