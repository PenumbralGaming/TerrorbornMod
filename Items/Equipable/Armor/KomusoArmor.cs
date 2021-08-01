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
    public class KomusoBaskethelm : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Hay, 30);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 8);
            recipe.AddIngredient(ItemID.Silk, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 4%");
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.04f;
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;
            item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<KomusoBreastplate>() && legs.type == ModContent.ItemType<KomusoGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Increases summon damage by 12%";
            player.minionDamage += 0.12f;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class KomusoBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 5%" +
                "\nIncreases your maximum number of minions by 1");
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.05f;
            player.maxMinions++;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Hay, 25);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 13);
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 25;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;
            item.defense = 7;
        }

        public override bool DrawBody()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class KomusoGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Hay, 25);
            recipe.AddRecipeGroup(RecipeGroupID.IronBar, 6);
            recipe.AddIngredient(ItemID.Silk, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases summon damage by 3%");
        }

        public override void UpdateEquip(Player player)
        {
            player.minionDamage += 0.03f;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 2;
            item.defense = 4;
        }
    }
}