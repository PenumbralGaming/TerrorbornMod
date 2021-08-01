using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Terraria.DataStructures;

namespace TerrorbornMod.Items.MiscConsumables
{
    public class TerrorTaco : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Causes you to generate terror over time" +
                "\nWhile this is active, being at 100% terror will cause you to die" +
                "\nYour movement speed will be greatly decreased while over 50% terror");
        }
        public override void SetDefaults()
        {
            item.useTime = 10;
            item.useAnimation = 10;
            item.useStyle = 2;
            item.maxStack = 30;
            item.consumable = true;
            item.rare = 1;
            item.autoReuse = false;
            item.UseSound = SoundID.Item2;
            item.useTurn = true;
            item.maxStack = 30;
            item.buffType = ModContent.BuffType<HorificallyStuffed>();
            item.buffTime = 3600 * 3;
        }
    }

    class HorificallyStuffed : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Horifically Stuffed");
            Description.SetDefault("You generate terror over time... but be sure not to get too full");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TerrorPercent += 1.5f / 60f;
            if (modPlayer.TerrorPercent >= 100)
            {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " was so full they blew up (literally)"), 10000, 0);
                modPlayer.TerrorPercent = 0f;
            }
            if (modPlayer.TerrorPercent >= 50)
            {
                float maxSpeed = 4.5f;
                if (player.velocity.X > maxSpeed)
                {
                    player.velocity.X = maxSpeed;
                }
                if (player.velocity.X < -maxSpeed)
                {
                    player.velocity.X = -maxSpeed;
                }
            }
        }
    }
}

