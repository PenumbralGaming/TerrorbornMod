using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SoulReaperMask : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 15);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+10 max life" +
                "\n10% increased restless weapon damage" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 12;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("SoulReaperBreastplate") && legs.type == mod.ItemType("SoulReaperGreaves");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Killing enemies or taking away 7.5% of a boss's health causes them to drop a thunder soul" +
                "\nPicking up a thunder soul grants you the Soul Maniac buff for 5 seconds, increasing restless weapon stats" +
                "\n10% increased critical strike chance" +
                "\n+10 max life";
            player.statLifeMax2 += 10;
            player.magicCrit += 10;
            player.rangedCrit += 10;
            player.meleeCrit += 10;

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SoulReaperArmorBonus = true;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 10;
            modPlayer.restlessDamage += 0.10f;
            float otherDamageIncrease = 0.05f;
            player.magicDamage += otherDamageIncrease;
            player.rangedDamage += otherDamageIncrease;
            player.meleeDamage += otherDamageIncrease;
            player.minionDamage += otherDamageIncrease;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class SoulReaperBreastplate : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 24);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 16);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+20 max life" +
                "\n30% increased restless weapon use speed while not fully charged" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 14;
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 20;
            modPlayer.restlessNonChargedUseSpeed *= 1.3f;
            float otherDamageIncrease = 0.05f;
            player.magicDamage += otherDamageIncrease;
            player.rangedDamage += otherDamageIncrease;
            player.meleeDamage += otherDamageIncrease;
            player.minionDamage += otherDamageIncrease;
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class SoulReaperGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.ThunderShard>(), 12);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.NoxiousScale>(), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("+10 max life" +
                "\n10% increased restless weapon use speed while fully charged" +
                "\n5% increase to all damage");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.statLifeMax2 += 10;
            modPlayer.restlessChargedUseSpeed *= 1.1f;
            float otherDamageIncrease = 0.05f;
            player.magicDamage += otherDamageIncrease;
            player.rangedDamage += otherDamageIncrease;
            player.meleeDamage += otherDamageIncrease;
            player.minionDamage += otherDamageIncrease;
        }
    }

    class ThunderSoul : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 30;
        }

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
        }

        public override bool OnPickup(Player player)
        {
            player.AddBuff(ModContent.BuffType<Buffs.SoulManiac>(), 60 * 5);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item4, player.Center);
            CombatText.NewText(player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), "Soul Maniac!");
            return false;
        }
    }
}
