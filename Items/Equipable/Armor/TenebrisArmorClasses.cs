using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Equipable.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class TenebrisMask : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tenebris Helmet");
            Tooltip.SetDefault("11% increased damage" +
                "\nIncreased item use speed by 15%" +
                "\n6% increased critical strike chance" +
                "\n20% increased damage taken");
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 5;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TenebrisChestplate") && legs.type == mod.ItemType("TenebrisLeggings");
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Press the <ArmorAbility> hotkey to dash towards the cursor.\n" +
                "Getting hit during this dash will completely dodge the attack and give you the 'Tenebral Focus'\n" +
                "buff. This will increase your critical strike chance and item use speed by 15%, however you will\n" +
                "take 50% increased damage as well." +
                "\nYou cannot dodge an attack like this again while the Focus buff is active." +
                "\nYour maximum HP is decreased by 80.";
            player.statLifeMax2 -= 80;

            if (TerrorbornModContent.ArmorAbility.JustPressed && !player.HasBuff(ModContent.BuffType<TenebralCooldown>()))
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item72, player.Center);
                player.AddBuff(ModContent.BuffType<TenebralCooldown>(), 180);
                int Speed = 14;
                Vector2 mousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
                TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                modPlayer.TenebrisDashVelocity = player.DirectionTo(mousePosition) * Speed;
                modPlayer.TenebrisDashTime = 20;
            }
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.11f;
            int crit = 4;
            player.magicCrit += crit;
            player.meleeCrit += crit;
            player.rangedCrit += crit;
            player.endurance -= 0.2f;
            modPlayer.allUseSpeed += 0.15f;
        }

    }
    [AutoloadEquip(EquipType.Body)]
    public class TenebrisChestplate : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("10% increased damage" +
                "\nIncreased item use speed by 15%" +
                "\n4% increased critical strike chance" +
                "\n20% increased damage taken");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 24;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 8;
        }

        public override bool DrawBody()
        {
            return false;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.1f;
            int crit = 4;
            player.magicCrit += crit;
            player.meleeCrit += crit;
            player.rangedCrit += crit;
            player.endurance -= 0.2f;
            modPlayer.allUseSpeed += 0.15f;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class TenebrisLeggings : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.SoulOfPlight>(), 18);
            recipe.AddIngredient(ItemID.HallowedBar, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tenebris Greaves");
            Tooltip.SetDefault("9% increased damage" +
                "\nIncreased agility" +
                "\n5% increased critical strike chance" +
                "\n20% increased damage taken");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.09f;
            int crit = 5;
            player.magicCrit += crit;
            player.meleeCrit += crit;
            player.rangedCrit += crit;
            player.endurance -= 0.2f;
            player.moveSpeed += 3;
        }
    }
    class TenebralFocus : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tenebral Focus");
            Description.SetDefault("15% increased critical strike chance and 15% increased item use speed");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            player.allDamage += 0.08f;
            int crit = 15;
            player.magicCrit += crit;
            player.meleeCrit += crit;
            player.rangedCrit += crit;
            modPlayer.allUseSpeed += 0.15f;
        }
    }
    class TenebralCooldown : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tenebral Cooldown");
            Description.SetDefault("You cannot dash again...");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
            longerExpertDebuff = false;
        }
    }
}


