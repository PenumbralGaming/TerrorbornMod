﻿using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    class Shelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.ShellFragments>(), 12);
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("6% increased melee critical strike chance" +
                "\nHas a set bonus when paired with copper or tin armor");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 24;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.defense = 6;
            item.rare = 1;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemID.CopperChainmail && legs.type == ItemID.CopperGreaves) || (body.type == ItemID.TinChainmail && legs.type == ItemID.TinGreaves);
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "10% increased melee speed" +
                "\n4 defense";
            player.meleeSpeed += 0.1f;
            player.statDefense += 4;
        }
    }
}