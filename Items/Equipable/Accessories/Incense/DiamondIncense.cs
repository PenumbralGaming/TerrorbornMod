using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Incense
{
    class DiamondIncense : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Diamond, 5);
            recipe.AddIngredient(ItemID.Bottle);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Decreased enemy defense by 4." +
                "\nYou emit an aura that damages nearby enemies every 3 seconds." +
                "\nThe range and damage of this aura is increased by the amount of enemies nearby.");
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

        public void DustCircle(Vector2 position, int Dusts, float Radius, int DustType, float DustSpeed, float DustScale = 1f) //Thanks to seraph for this code
        {
            float currentAngle = Main.rand.Next(360);
            for (int i = 0; i < Dusts; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Dusts) * i) + currentAngle));
                direction.X *= Radius;
                direction.Y *= Radius;

                Dust dust = Dust.NewDustPerfect(position + direction, DustType, (direction / Radius) * DustSpeed, 0, default(Color), DustScale);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        int damageCounter = 1;
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
            damageCounter--;
            if (damageCounter <= 0)
            {
                int range = 275 + (20 * enemyCount);
                bool CreateDust = false;
                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && npc.Distance(player.Center) <= range && npc.active)
                    {
                        CreateDust = true;
                        npc.StrikeNPC(12 + enemyCount / 2, 0, 0);
                    }
                }
                damageCounter = 180;
                if (CreateDust)
                {
                    DustCircle(player.Center, 180, range, 63, -10, 3f);
                }
            }
        }
    }
}
