using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;

namespace TerrorbornMod.Items.Weapons.Summons.Minions
{
    class FrigidStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a frigid soul to fight for you");
        }
        public override void SetDefaults()
        {
            item.mana = 5;
            item.summon = true;
            item.damage = 5;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 2;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("FrigidSoul");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 0, 20, 0);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool UseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                player.MinionNPCTargetAim();
            }
            return base.UseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.altFunctionUse != 2)
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (Main.projectile[i].minion && player.slotsMinions > player.maxMinions)
                    {
                        Main.projectile[i].active = false;
                    }
                }
                Projectile.NewProjectile(new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition, Vector2.Zero, type, damage, knockBack, item.owner);
                if (player.slotsMinions <= player.maxMinions)
                {
                    player.AddBuff(ModContent.BuffType<FrigidSoulBuff>(), 60);
                }
            }
            return false;
        }
    }

    class FrigidSoul : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 52;
            projectile.height = 52;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 60;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 20;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 10;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 7, DustID.Ice, DustScale: 1f, NoGravity: true);
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

        int mode = 0;
        int rotationDirection = 0;
        int dashCounter = 30;
        public override void AI()
        {
            projectile.timeLeft = 60;
            if (projectile.velocity.X > 0)
            {
                rotationDirection = 1;
            }
            else
            {
                rotationDirection = -1;
            }

            FindFrame(projectile.height);

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<FrigidSoulBuff>()))
            {
                projectile.active = false;
            }

            bool Targeted = false;
            NPC target = Main.npc[0];

            float Distance = 1000;
            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Distance(projectile.Center) < Distance && !Main.npc[i].friendly && Main.npc[i].CanBeChasedBy())
                {
                    target = Main.npc[i];
                    Distance = Main.npc[i].Distance(projectile.Center);
                    Targeted = true;
                }
            }

            if (player.HasMinionAttackTargetNPC)
            {
                target = Main.npc[player.MinionAttackTargetNPC];
            }

            if (!projectile.CanHit(player) || !Targeted || !projectile.CanHit(target))
            {
                mode = 0;
            }
            else
            {
                mode = 1;
            }

            if (mode == 0)
            {
                if (projectile.Distance(player.Center) > 80)
                {
                    float speed = 0.4f;
                    projectile.velocity += projectile.DirectionTo(player.Center) * speed;
                    projectile.velocity *= 0.98f;
                }
                if (projectile.Distance(player.Center) > 5000)
                {
                    projectile.position = player.Center - new Vector2(projectile.width / 2, projectile.height / 2);
                }
                projectile.rotation += MathHelper.ToRadians(projectile.velocity.Length()) * rotationDirection;
            }

            if (mode == 1)
            {
                float speed = 20f;
                dashCounter--;
                if (dashCounter <= 0)
                {
                    dashCounter = 30;
                    projectile.velocity = projectile.DirectionTo(target.Center) * speed;
                }

                speed = 0.2f;
                projectile.velocity += projectile.DirectionTo(target.Center) * speed;

                projectile.velocity *= 0.96f;
                projectile.rotation += MathHelper.ToRadians(projectile.velocity.Length()) * rotationDirection;
            }
        }
    }

    class FrigidSoulBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Frigid Soul");
            Description.SetDefault("A spirit of the cold follows to ensure your safety");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            longerExpertDebuff = false;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].type == ModContent.ProjectileType<FrigidSoul>())
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

