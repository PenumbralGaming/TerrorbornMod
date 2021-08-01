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
    class OpticCane : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an optic demons that swarms enemies and spews blood");
        }
        public override void SetDefaults()
        {
            item.mana = 5;
            item.summon = true;
            item.damage = 8;
            item.width = 38;
            item.height = 40;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 2;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("OpticDemon");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 0, 50, 0);
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
                    player.AddBuff(ModContent.BuffType<OpticDemonBuff>(), 60);
                }
            }
            return false;
        }
    }

    class OpticDemon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            Main.projFrames[projectile.type] = 7;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 60;
            projectile.height = 42;
            projectile.tileCollide = false;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.timeLeft = 360;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
            projectile.minion = true;
            projectile.minionSlots = 1;
        }
        void FindFrame(int FrameHeight)
        {
            projectile.frameCounter--;
            if (projectile.frameCounter <= 0)
            {
                projectile.frame++;
                projectile.frameCounter = 5;
            }
            if (projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }

        public override void Kill(int timeLeft)
        {
            DustExplosion(projectile.Center, 0, 25, 7, DustID.Blood, DustScale: 1f, NoGravity: true);
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

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, Color.Red, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        int mode = 0;
        int bloodCounter = 10;
        public override void AI()
        {
            projectile.timeLeft = 500;
            if (projectile.velocity.X > 0)
            {
                projectile.spriteDirection = -1;
            }
            else
            {
                projectile.spriteDirection = 1;
            }

            FindFrame(projectile.height);

            Player player = Main.player[projectile.owner];
            if (!player.HasBuff(ModContent.BuffType<OpticDemonBuff>()))
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
                if (projectile.Distance(player.Center) > 100)
                {
                    float speed = 0.6f;
                    projectile.velocity += projectile.DirectionTo(player.Center) * speed;
                    projectile.velocity *= 0.98f;
                }
                if (projectile.Distance(player.Center) > 5000)
                {
                    projectile.position = player.Center - new Vector2(projectile.width / 2, projectile.height / 2);
                }
            }

            if (mode == 1)
            {
                if (projectile.Distance(target.Center) > 100)
                {
                    float speed = 0.6f;
                    projectile.velocity += projectile.DirectionTo(target.Center) * speed;
                    projectile.velocity *= 0.985f;
                }

                bloodCounter--;
                if (bloodCounter <= 0)
                {
                    bloodCounter = 30;
                    float speed = 25;
                    Vector2 velocity = projectile.DirectionTo(target.Center) * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<OpticBlood>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }
    }
    public class OpticBlood : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.hide = true;
            projectile.timeLeft = 100;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }


        int tileCollideCounter = 25;
        public override void AI()
        {
            if (tileCollideCounter <= 0)
            {
                projectile.tileCollide = true;
            }
            else
            {
                tileCollideCounter--;
            }

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 115, 0f, 0f, 100, Color.Red, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
        }
    }

    class OpticDemonBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Optic Demon");
            Description.SetDefault("An optic demon is fighting for you!");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<OpticDemon>())
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}
