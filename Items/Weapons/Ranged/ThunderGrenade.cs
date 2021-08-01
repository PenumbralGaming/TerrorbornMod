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
    class ThunderGrenade : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("A grenade that explodes into lightning if enemies are nearby" +
                "\nThe lightning will home into enemies and try to hit more than one enemy");
        }
        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 32;
            item.damage = 36;
            item.ranged = true;
            item.useTime = 15;
            item.useAnimation = 15;
            item.noUseGraphic = true;
            item.useStyle = 1;
            item.knockBack = 2;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 5;
            item.UseSound = SoundID.Item106;
            item.autoReuse = true;
            item.noMelee = true;
            item.shootSpeed = 22;
            item.shoot = ModContent.ProjectileType<ThunderGrenadeProjectile>();
        }
    }

    class ThunderGrenadeProjectile : ModProjectile
    {
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            damage = (int)(damage * 0.75f);
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 20;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 1;
            projectile.timeLeft = 600;
        }

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(projectile.velocity.X);
            projectile.velocity.Y += 0.18f;
        }

        public void DustExplosion(Vector2 position, int RectWidth, int Streams, float DustSpeed, int DustType, float DustScale = 1f, bool NoGravity = false) //Thank you once again Seraph
        {
            float currentAngle = Main.rand.Next(360);

            //if(Main.netMode!=1){
            for (int i = 0; i < Streams; ++i)
            {

                Vector2 direction = Vector2.Normalize(new Vector2(1, 1)).RotatedBy(MathHelper.ToRadians(((360 / Streams) * i) + currentAngle));
                direction.X *= DustSpeed;
                direction.Y *= DustSpeed;

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, default(Color), DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item89, projectile.Center);
            DustExplosion(projectile.Center, 0, 12, 7, 62, 2f, true);

            List<int> npcsTargeted = new List<int>();
            for (int i = 0; i < Main.rand.Next(3, 6); i++)
            {
                bool foundSomething = false;
                bool foundAnything = false;
                NPC closest = Main.npc[0];
                NPC preferred = Main.npc[0];
                float distance = 4000;
                float preferredDistance = distance;

                foreach (NPC npc in Main.npc)
                {
                    if (!npc.friendly && npc.CanBeChasedBy() && projectile.CanHit(npc))
                    {
                        if (npcsTargeted.Contains(npc.whoAmI))
                        {
                            if (npc.Distance(projectile.Center) < distance)
                            {
                                foundAnything = true;
                                distance = npc.Distance(projectile.Center);
                                closest = npc;
                            }
                        }
                        else if (npc.Distance(projectile.Center) < preferredDistance)
                        {
                            foundAnything = true;
                            foundSomething = true;
                            preferredDistance = npc.Distance(projectile.Center);
                            preferred = npc;
                        }
                    }
                }

                if (foundAnything)
                {
                    if (foundSomething)
                    {
                        npcsTargeted.Add(preferred.whoAmI);
                        SpawnLightning(preferred.whoAmI);
                    }
                    else
                    {
                        npcsTargeted.Add(closest.whoAmI);
                        SpawnLightning(closest.whoAmI);
                    }
                }
            }
        }

        public void SpawnLightning(int target)
        {
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.NextFloat(10f, 25f);

            int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), projectile.damage / 2, 0.5f, projectile.owner);
            Main.projectile[proj].ranged = true;
            Main.projectile[proj].ai[0] = target;
        }
    }
}