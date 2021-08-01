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
    public class DeimosteelHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 8);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases shriek of horror's use speed");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.ShriekSpeed *= 0.65f;
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 22;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 1;
            item.defense = 5;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<DeimosteelChainMail>() && legs.type == ModContent.ItemType<DeimosteelGreaves>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Drains terror over time to increase your defense by 8 and item use speed by 10%" +
                "\nIf you have no terror left, your shriek of horror use speed will be increased instead";
            float drainSpeed = 0.65f;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.TerrorPercent >= drainSpeed / 60)
            {
                modPlayer.TerrorPercent -= drainSpeed / 60;
                modPlayer.allUseSpeed *= 1.10f;
                player.statDefense += 8;
            }
            else
            {
                modPlayer.ShriekSpeed *= 0.5f;
            }
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class DeimosteelChainMail : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases critical strike chance by 6%");
        }

        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 6;
            player.rangedCrit += 6;
            player.meleeCrit += 6;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 10);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 1;
            item.defense = 6;
        }

        //public override bool DrawBody()
        //{
        //    return false;
        //}
    }
    [AutoloadEquip(EquipType.Legs)]
    public class DeimosteelGreaves : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.DeimosteelBar>(), 7);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases all item use speeds by 6%");
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.allUseSpeed *= 1.06f;
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = Item.sellPrice(0, 0, 50, 0);
            item.rare = 1;
            item.defense = 5;
        }
    }
}
