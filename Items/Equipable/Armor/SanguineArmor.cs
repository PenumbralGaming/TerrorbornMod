using Terraria;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SanguineHood : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 6%" +
                "\nIncreases critical strike chance by 5%" +
                "\nIncreases shriek of horror's use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed += 0.06f;
            player.magicCrit += 5;
            player.rangedCrit += 5;
            player.meleeCrit += 5;
            modPlayer.ShriekSpeed *= 0.7f;
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 5;
            item.defense = 11;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<SanguineBreastplate>() && legs.type == ModContent.ItemType<SanguineGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "The healing orbs from necromantic curse return to you much faster" +
                "\nThe projectile from necromantic curse homes into enemies and can hit multiple enemies";
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.SanguineSetBonus = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class SanguineBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 8%" +
                "\nIncreases critical strike chance by 4%" +
                "\nIncreases the amount of terror obtained from Shriek of horror by 30%" +
                "\nIncrease armor penetration by 12");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed += 0.08f;
            modPlayer.ShriekTerrorMultiplier *= 1.3f;
            player.magicCrit += 4;
            player.rangedCrit += 4;
            player.meleeCrit += 4;
            player.armorPenetration += 12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 25;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 5;
            item.defense = 19;
        }

        public override bool DrawBody()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class SanguineGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.SanguineFang>(), 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases item/weapon use speed by 5%" +
                "\nIncreases critical strike chance by 4%" +
                "\nIncreases life regen");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed += 0.05f;
            player.lifeRegen += 2;
            player.magicCrit += 4;
            player.rangedCrit += 4;
            player.meleeCrit += 4;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 5;
            item.defense = 8;
        }
    }
}