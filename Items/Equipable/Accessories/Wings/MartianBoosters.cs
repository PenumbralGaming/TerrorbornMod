using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class MartianBoosters : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Allows flight and slow fall" +
                "\nCauses you to emit light" +
                "\nGreatly increased movement speed while gliding" +
                "\nHold down for 1.5 seconds to instantly get rid of your flight time, allowing you to glide");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.value = Item.sellPrice(0, 8, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
        }

        int soundDelay = 10;
        int downCounter = 90;
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.wingTimeMax = (int)(60 * 3.2f);
            Lighting.AddLight(player.Center, Color.Cyan.ToVector3());
            if (player.controlDown)
            {
                if (downCounter == 0)
                {
                    player.wingTime = 0;
                    downCounter = -1;
                    CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Cyan, "Flight fuel disposed", true);
                    Terraria.Audio.SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 72, 1.5f, -0.6f);
                }
                else
                {
                    downCounter--;
                }
            }
            else
            {
                downCounter = 90;
            }
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.velocity.Y != 0)
            {
                player.wingFrameCounter++;
            }
            if (player.velocity.Y >= 0 || player.wingTime <= 0 || !player.controlJump)
            {
                player.wingFrame = 0;
            }
            if (player.controlJump && player.wingTime > 0 && player.velocity.Y != 0)
            {
                soundDelay--;
                if (soundDelay <= 0)
                {
                    soundDelay = 10;
                    Terraria.Audio.SoundEngine.PlaySound(2, (int)player.Center.X, (int)player.Center.Y, 24, 1, 1);
                }
                int dust = Dust.NewDust(new Vector2(player.Center.X - 20 + (-17.5f * player.direction), player.Center.Y - 10), 40, 40, 226);
                Main.dust[dust].velocity = new Vector2(0, 3);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].noLight = true;
                Main.dust[dust].color = item.color;
                Main.dust[dust].scale = 0.5f;
            }
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.08f;
            maxCanAscendMultiplier = 2.5f;
            maxAscentMultiplier = 2.5f;
            constantAscend = 0.1f;
        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            if (inUse)
            {
            }
            return base.WingUpdate(player, inUse);
        }

        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        {
            if (player.controlJump &&  player.wingTime <= 0)
            {
                acceleration *= 3f;
                speed *= 2f;
            }
            else
            {
                acceleration *= 1.4f;
                speed *= 1.35f;
            }
        }

        //public override void AddRecipes()
        //{
        //    ModRecipe recipe = new ModRecipe(mod);
        //    recipe.AddIngredient(null, "EquipMaterial", 60);
        //    recipe.AddTile(null, "ExampleWorkbench");
        //    recipe.SetResult(this);
        //    recipe.AddRecipe();
        //}
    }
}
