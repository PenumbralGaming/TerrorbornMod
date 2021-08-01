using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.ID;
using System;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Accessories
{
    class IntimidationAura : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Intimidation Aura");
            Tooltip.SetDefault("Closely dodging hostile enemies and projectiles will grant you terror" +
                "\nGetting hit will cause you to lose a third of the terror you have");
        }

        public override void SetDefaults()
        {
            item.width = 40;
            item.height = 34;
            item.accessory = true;
            item.rare = 1;
            item.value = Item.sellPrice(0, 4, 0, 0);
        }
        int Cooldown = 60 / 6;
        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.IntimidationAura = true;

            if (Cooldown > 0)
            {
                Cooldown--;
            }
            else
            {
                bool CreateDust = false;
                float range = 16 * 8;

                for (int i = 0; i < 200; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && npc.damage > 0 && npc.Distance(player.Center) <= range + (npc.height + npc.width) / 4 && npc.active)
                    {
                        CreateDust = true;
                    }
                }
                for (int i = 0; i < Main.projectile.GetUpperBound(0); i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.hostile && projectile.Distance(player.Center) <= range + (projectile.height + projectile.width) / 4 && projectile.active && projectile.damage > 0)
                    {
                        CreateDust = true;
                    }
                }

                if (CreateDust)
                {
                    modPlayer.TerrorPercent += 1.5f;
                    Cooldown = 60 / 6;
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.MaxMana, player.Center);
                    DustCircle(player.Center, 180, range, 63, -5, 3f);
                }
            }
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
                dust.alpha = 125;
            }
        }
    }
}
