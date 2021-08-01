using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class BountyHunterLeatherwear : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n3% increased ranged critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 20;
            item.defense = 7;
            item.rare = 2;
            item.value = Item.sellPrice(0, 2, 0, 0);
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.04f;
            player.rangedCrit += 3;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class BountyHunterPants : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n2% increased ranged critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.defense = 5;
            item.rare = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.04f;
            player.rangedCrit += 3;
        }
    }
    [AutoloadEquip(EquipType.Head)]
    public class BountyHunterCap : ModItem
    {
        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("4% increased ranged damage" +
                "\n4% increased ranged critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.defense = 7;
            item.rare = 2;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BountyHunterLeatherwear") && legs.type == mod.ItemType("BountyHunterPants");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Pressing the <ArmorAbility> hotkey gives you the 'bounty hunter's mark' buff for\n" +
                              "5 seconds. This increases your ranged critical strike chance by 25%, ranged firing speed by" +
                              "\n25% and makes all ranged projectiles home, but at the cost of 20 defense" +
                              "\n50 second cooldown";
            if (!player.HasBuff(mod.BuffType("HuntersRecharge")) && TerrorbornMod.ArmorAbility.JustPressed)
            {
                player.AddBuff(mod.BuffType("HuntersRecharge"), 60 * 50);
                player.AddBuff(mod.BuffType("HuntersMark"), 60 * 5);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item62, player.Center);

                DustExplosion(player.Center, 0, 80, 25, DustID.AmberBolt, NoGravity: true);
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

        public override void UpdateEquip(Player player)
        {
            player.rangedDamage += 0.04f;
            player.rangedCrit += 4;
        }
    }
    class HuntersRecharge : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Hunter's recharge");
            Description.SetDefault("You cannot use 'bounty hunter's mark' again");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
    public class HuntersMark : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Bounty Hunter's Mark");
            Description.SetDefault("'None can escape a bounty hunter'");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= 20;
            player.rangedCrit += 25;
            TerrorbornPlayer mPlayer = TerrorbornPlayer.modPlayer(player);
            mPlayer.rangedUseSpeed *= 1.25f;
        }
    }
}
