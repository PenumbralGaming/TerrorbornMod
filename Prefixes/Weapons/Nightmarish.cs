using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Prefixes.Weapons
{
    class Nightmarish : ModPrefix
    {
        public override bool Autoload(ref string name)
        {
            name = "Nightmarish";
            return base.Autoload(ref name);
        }

        public override PrefixCategory Category => PrefixCategory.AnyWeapon;

        public override void SetDefaults()
        {

        }


        public override void Apply(Item item)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            modItem.TerrorCost += item.useAnimation * 0.22f;
        }

        public override bool CanRoll(Item item)
        {
            if (TerrorbornWorld.obtainedShriekOfHorror)
            {
                return base.CanRoll(item);
            }
            return false;
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult = 1.3f;
            useTimeMult = 0.7f;
            critBonus = 20;
        }

        public override void ModifyValue(ref float valueMult)
        {
            valueMult = 1.5f;
            base.ModifyValue(ref valueMult);
        }
    }
}
