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
    public class AzuriteHelmet : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 10" +
                "\n4% increased magic damage" +
                "\n5% increased magic crit");
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 10;
            player.magicDamage += 0.04f;
            player.magicCrit += 5;
        }

        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 18;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.defense = 6;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<AzuriteChestplate>() && legs.type == ModContent.ItemType<AzuriteLeggings>();
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Every 3rd use of a magic weapon will release a short range aquatic shockwave." +
                            "\nThis can pierce an unlimited amount of enemies and has high knockback, but only" +
                            "\nhas one third of the weapon's base damage." +
                            "\nHitting two or more enemies with this shockwave will give you back 5 mana.";

            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.AzuriteArmorBonus = true;
        }
    }

    [AutoloadEquip(EquipType.Body)]
    public class AzuriteChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 10" +
                "\n5% increased magic damage" +
                "\nMagic weapons cast faster" +
                "\n2% increased magic crit");
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 10;
            player.magicDamage += 0.07f;
            player.magicCrit += 2;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            modPlayer.magicUseSpeed += 0.2f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 25;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.defense = 7;
        }

        public override bool DrawBody()
        {
            return false;
        }
    }
    [AutoloadEquip(EquipType.Legs)]
    public class AzuriteLeggings : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("AzuriteBar"), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
        bool WasInAir = false;
        int TilInAir = 20;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases max mana by 15" +
                "\n3% increased magic damage" +
                "\n2% increased magic crit");
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 15;
            player.magicDamage += 0.03f;
            player.magicCrit += 2;
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 12;
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.defense = 4;
        }
    }
    
    class azuriteShockwave : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Melee/AzuriteSlash";

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 28;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.timeLeft = 300;
        }

        int enemiesHit = 0;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            enemiesHit++;
            if (enemiesHit >= 2)
            {
                Main.player[projectile.owner].statMana += 5;
                Main.player[projectile.owner].ManaEffect(5);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            damage += target.defense / 2;
        }

        int trueTimeLeft = 30;
        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(90);
            projectile.velocity *= 0.95f;
            if (trueTimeLeft <= 0)
            {
                projectile.alpha += 15;
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            else
            {
                trueTimeLeft--;
            }
        }
        //public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        //{
        //    //Thanks to Seraph for afterimage code.
        //    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
        //    for (int i = 0; i < projectile.oldPos.Length; i++)
        //    {
        //        Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
        //        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
        //        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
        //    }
        //    return false;
        //}
    }
}