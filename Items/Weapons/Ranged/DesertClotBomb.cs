using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.GameContent.Events;
using Terraria.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.WorldBuilding;
using Terraria.UI;

namespace TerrorbornMod.Items.Weapons.Ranged
{
    class DesertClotBomb : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Explodes into homing antlion blood");
        }
        public override void SetDefaults()
        {
            item.damage = 20;
            item.ranged = true;
            item.consumable = true;
            item.maxStack = 999;
            item.useTime = 18;
            item.useAnimation = 18;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 0, 0, 20);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 22;
            item.shoot = ModContent.ProjectileType<DesertClotBombProj>();
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.TarOfHunger>(), 6);
            recipe.AddIngredient(ItemID.AntlionMandible);
            recipe.AddIngredient(ItemID.Grenade, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 111);
            recipe.AddRecipe();
        }
    }

    class DesertClotBombProj : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Ranged/DesertClotBomb";
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.75f);
        }
        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 22;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.ranged = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }
        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
            projectile.velocity.Y += 0.18f;
        }
        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath1, projectile.Center);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, projectile.Center);
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                float Speed = Main.rand.Next(7, 10);
                Vector2 ProjectileSpeed = MathHelper.ToRadians(Main.rand.Next(361)).ToRotationVector2() * Speed;
                Projectile.NewProjectile(projectile.Center, ProjectileSpeed, ModContent.ProjectileType<AntlionBlood>(), (int)(projectile.damage * 0.75f), 0, projectile.owner);
            }
        }
    }
    class AntlionBlood : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.Homing[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.ranged = true;
            projectile.timeLeft = 30;
        }
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void AI()
        {
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
            int targetPlayer = Player.FindClosest(projectile.position, projectile.width, projectile.height);
            NPC targetNPC = Main.npc[0];
            float Distance = 375; //max distance away
            bool Targeted = false;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    targetNPC = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }
            if (Targeted)
            {
                //HOME IN
                float speed = .3f;
                Vector2 move = targetNPC.Center - projectile.Center;
                float magnitude = (float)Math.Sqrt(move.X * move.X + move.Y * move.Y);
                move *= speed / magnitude;
                projectile.velocity += move;
                projectile.velocity *= 0.98f;
            }
        }
    }
}
