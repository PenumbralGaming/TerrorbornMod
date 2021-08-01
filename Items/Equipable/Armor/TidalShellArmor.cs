using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class TidalShellChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Tidal shell chestplate");
            Tooltip.SetDefault("Melee damage increased by 4%");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.defense = 8;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.04f;
        }
        public override bool DrawBody()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class TidalShellLegwear : ModItem
    {
        bool WasInAir = false;
        int TilInAir = 20;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Melee damage increased by 4%");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.04f;
        }
        
    }
    [AutoloadEquip(EquipType.Head)]
    public class TidalShellHeadplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Melee damage increased by 4%" +
                "\nMelee crit chance increased by 5%");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.rare = 2;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TidalShellChestplate") && legs.type == mod.ItemType("TidalShellLegwear");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Hitting enemies with melee weapon has a chance to cause geysers to erupt from the" +
                "\nground beneath them" +
                "\nKilling an enemy guarentees this effect";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.TidalShellArmorBonus = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.meleeDamage += 0.04f;
        }
    }
    class TideFireFriendly : ModProjectile
    {
        public override string Texture => "TerrorbornMod/NPCs/Bosses/TumblerNeedle";
        public override void SetDefaults()
        {
            projectile.width = 50;
            projectile.height = 50;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.friendly = true;
            projectile.hide = true;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 30;
            projectile.alpha = 255;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }
        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 88, 0, 0, Scale: 2, newColor: Color.SkyBlue);
            Main.dust[dust].noGravity = true;
        }
    }
}
