using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Magic
{
    class BookOfChaos : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a chaos ball that bounces between enemies" +
                "\nInflicts shadowflame for 2 seconds on critical hits");
        }

        public override void SetDefaults()
        {
            item.value = Item.sellPrice(0, 1, 0, 0);
            item.width = 32;
            item.height = 38;
            item.magic = true;
            item.damage = 27;
            item.useTime = 25;
            item.useAnimation = 25;
            item.mana = 10;
            item.rare = 2;
            item.shoot = mod.ProjectileType("ChaosBall");
            item.shootSpeed = 10;
            item.UseSound = SoundID.Item20;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0.1f;
            item.autoReuse = true;
        }
    }

    class ChaosBall : ModProjectile
    {
        public override string Texture { get { return "Terraria/NPC_" + NPCID.ChaosBall; } }

        public override void SetDefaults()
        {
            NPC npc = new NPC();
            npc.SetDefaults(NPCID.ChaosBall);
            projectile.width = npc.width;
            projectile.height = npc.height;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 6;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.hide = false;
            projectile.timeLeft = 600;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        List<NPC> alreadyHit = new List<NPC>();
        public override void AI()
        {
            Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, 27)];
            dust.noGravity = true;
            dust.velocity = Vector2.Zero;
            dust.scale = 2f;
        }

        public override void Kill(int timeLeft)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit3, projectile.Center);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            alreadyHit.Add(target);

            bool targeted = false;
            NPC launchTo = Main.npc[0];
            float distance = 2000;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && npc.Distance(projectile.Center) < distance && !alreadyHit.Contains(npc) && projectile.CanHit(npc) && !npc.friendly && npc.CanBeChasedBy())
                {
                    distance = npc.Distance(projectile.Center);
                    launchTo = npc;
                    targeted = true;
                }
            }

            if (targeted)
            {
                float speed = projectile.velocity.Length();
                Vector2 direction = projectile.DirectionTo(launchTo.Center);
                projectile.velocity = speed * direction;
            }

            if (crit)
            {
                target.AddBuff(BuffID.ShadowFlame, 60 * 2);
            }
        }
    }
}

