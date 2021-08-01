using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;


namespace TerrorbornMod.Items.Equipable.Accessories
{
    class MidnightCloak : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<CloakOfTheWind>());
            recipe.AddIngredient(ItemID.MasterNinjaGear);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 10);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Double tap left or right to dash" +
                "\nDashing right as a projectile or enemy is near you increases your weapon" +
                "\nuse speed by 10% for 3 seconds" +
                "\nEvery 15 seconds, this can also grant you immunity frames" +
                "\nGrants a chance to dodge attacks that would have hit you");
        }

        public override void SetDefaults()
        {
            item.accessory = true;
            item.rare = 9;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.defense = 8;
        }

        bool heldRight = false;
        bool heldLeft = false;

        int cancleCounterLeft = 0;
        int cancleCounterRight = 0;
        int rightTaps = 0;
        int leftTaps = 0;

        int dashDelay = 0;
        int dustTime = 0;

        public void OnCloseCall(Player player)
        {
            DustExplosion(player.Center, 0, 10, 25, 74, 2f, NoGravity: true);
            player.AddBuff(ModContent.BuffType<Windspeed>(), 60 * 3);
            CombatText.NewText(player.getRect(), Color.LightGreen, "Close dodge!", true, false);
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (!player.HasBuff(ModContent.BuffType<MidnightDodgeCooldown>()))
            {
                player.AddBuff(ModContent.BuffType<MidnightDodgeCooldown>(), 60 * 15);
                modPlayer.iFrames = 40;
            }
        }

        public void Dash(int direction, Player player)
        {
            player.velocity.X = 14.5f * direction;
            Terraria.Audio.SoundEngine.PlaySound(SoundID.DD2_FlameburstTowerShot, player.Center);

            Rectangle closeRectangle = player.getRect();
            int extraWidth = 320;
            int extraHeight = 320;
            closeRectangle.Width += extraWidth;
            closeRectangle.Height += extraHeight;
            closeRectangle.X -= extraWidth / 2;
            closeRectangle.Y -= extraHeight / 2;

            bool intersects = false;
            for (int i = 0; i < 200; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.friendly && npc.damage > 0 && npc.getRect().Intersects(closeRectangle) && npc.active)
                {
                    intersects = true;
                }
            }
            for (int i = 0; i < Main.projectile.GetUpperBound(0); i++)
            {
                Projectile projectile = Main.projectile[i];
                if (projectile.hostile && projectile.getRect().Intersects(closeRectangle) && projectile.active && projectile.damage > 0)
                {
                    intersects = true;
                }
            }

            if (intersects)
            {
                OnCloseCall(player);
            }
        }

        public override void UpdateEquip(Player player)
        {
            if (player.dash > 0)
            {
                return;
            }

            player.blackBelt = true;

            if (dashDelay > 0)
            {
                dashDelay--;
            }

            if (dustTime > 0)
            {
                dustTime--;
                int dust = Dust.NewDust(player.position, player.width, player.height, 74, Scale: 1f);
                Main.dust[dust].velocity = player.velocity;
                Main.dust[dust].noGravity = true;
            }

            if (!player.controlRight)
            {
                heldRight = false;
            }
            if (player.controlRight & !heldRight & dashDelay <= 0)
            {
                heldRight = true;
                cancleCounterRight = 15;
                rightTaps++;
                if (rightTaps >= 2)
                {
                    Dash(1, player);
                    dashDelay = 40;
                    dustTime = 40;
                }
            }

            if (!player.controlLeft)
            {
                heldLeft = false;
            }
            if (player.controlLeft & !heldLeft & dashDelay <= 0)
            {
                heldLeft = true;
                cancleCounterLeft = 15;
                leftTaps++;
                if (leftTaps >= 2)
                {
                    Dash(-1, player);
                    dashDelay = 40;
                    dustTime = 40;
                }
            }

            if (cancleCounterRight > 0)
            {
                cancleCounterRight--;
            }
            if (cancleCounterRight <= 0)
            {
                rightTaps = 0;
            }

            if (cancleCounterLeft > 0)
            {
                cancleCounterLeft--;
            }
            if (cancleCounterLeft <= 0)
            {
                leftTaps = 0;
            }
        }
        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }
    }
    public class MidnightDodgeCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Midnight Dodge Cooldown");
            Description.SetDefault("You cannot get iframes from close dashes");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.buffTime[buffIndex] == 1)
            {
                CombatText.NewText(player.getRect(), Color.LightGreen, "Midnight dodge regained!", true, false);
            }
        }
    }
}
