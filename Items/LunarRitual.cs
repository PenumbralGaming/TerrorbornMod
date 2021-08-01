using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items
{
    class LunarRitual : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Not consumable" +
                "\nSummons a Mysterious Crab");
        }

        public override void SetDefaults()
        {
            item.rare = 3;
            item.useAnimation = 30;
            item.useTime = 30;
            item.useStyle = 4;
            item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.type == ModContent.ProjectileType<LunarPortal>() && proj.active)
                {
                    return false;
                }
            }
            return player.ZoneBeach && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.TidalTitan>()) && !NPC.AnyNPCs(ModContent.NPCType<NPCs.Bosses.TidalTitanIdle>());
        }
        public override bool UseItem(Player player)
        {
            Vector2 position = player.Center + new Vector2(0, -350);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item117, position);
            Projectile.NewProjectile(position, Vector2.Zero, ModContent.ProjectileType<LunarPortal>(), 0, 0);
            return true;
        }
    }

    class LunarPortal : ModProjectile
    {
        public override string Texture => "TerrorbornMod/Portal";
        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 10000;
            projectile.damage = 1;
            projectile.hostile = false;
            projectile.friendly = false;
            projectile.alpha = 255;
            projectile.tileCollide = false;
        }

        float backRotation;
        float frontRotation;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = Color.DeepSkyBlue;
            float scaleMultiplier = 4f;

            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), projectile.GetAlpha(color), backRotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.2f * scaleMultiplier, SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), projectile.GetAlpha(color), frontRotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f * scaleMultiplier, SpriteEffects.None, 0);
            return false;
        }

        int trueTimeLeft = 120;
        bool spawned = false;
        int spawnCounter = 75;


        public override void AI()
        {
            if (trueTimeLeft > 0)
            {
                if (projectile.alpha > 0)
                {
                    projectile.alpha -= 255 / 30;
                }
                else
                {
                    projectile.alpha = 0;
                }
                trueTimeLeft--;
            }
            else
            {
                if (projectile.alpha < 255)
                {
                    projectile.alpha += 255 / 30;
                }
                else
                {
                    projectile.active = false;
                }
            }
            float rotationSpeed = 5f;
            backRotation += MathHelper.ToRadians(rotationSpeed);
            frontRotation -= MathHelper.ToRadians(rotationSpeed);

            if (spawnCounter > 0)
            {
                spawnCounter--;
            }
            else if (!spawned)
            {
                spawned = true;
                NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y, ModContent.NPCType<NPCs.Bosses.TidalTitanIdle>());
            }
        }
    }
}
