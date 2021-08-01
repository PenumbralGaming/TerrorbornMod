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
    class IncendiusStaff : ModItem
    {
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe.AddIngredient(ItemID.CobaltBar, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ModContent.ItemType<Items.Materials.IncendiusAlloy>(), 25);
            recipe2.AddIngredient(ItemID.PalladiumBar, 15);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons an incendiary demon that surrounds you and fires predictive fireballs at nearby enemies");
        }
        public override void SetDefaults()
        {
            item.width = 64;
            item.height = 60;
            item.mana = 10;
            item.summon = true;
            item.damage = 19;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 4;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("IncendiaryDemon");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 3, 0, 0);
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
                    player.AddBuff(ModContent.BuffType<IncendiaryDemonBuff>(), 60);
                }
            }
            return false;
        }
    }
    class IncendiaryDemon : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 6;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void SetDefaults()
        {
            projectile.penetrate = -1;
            projectile.width = 86;
            projectile.height = 66;
            projectile.tileCollide = false;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.ignoreWater = true;
            projectile.minionSlots = 1;
            projectile.timeLeft = 360;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 10;
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
            DustExplosion(projectile.Center, 0, 25, 7, DustID.Fire, DustScale: 1f, NoGravity: true);
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
        int fireCounter = 10;
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

            if (!player.HasBuff(ModContent.BuffType<IncendiaryDemonBuff>()))
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
                Targeted = true;
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
            }

            if (mode == 1)
            {
                if (projectile.Distance(player.Center) > 100)
                {
                    float speed = 0.6f;
                    projectile.velocity += projectile.DirectionTo(player.Center) * speed;
                    projectile.velocity *= 0.98f;
                }

                fireCounter--;
                if (fireCounter <= 0)
                {
                    fireCounter = 30;
                    float speed = 15;
                    Vector2 velocity = projectile.DirectionTo(target.Center + (target.Distance(projectile.Center) / speed) * target.velocity) * speed;
                    Projectile.NewProjectile(projectile.Center, velocity, ModContent.ProjectileType<HellFire2>(), projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }
    }

    class HellFire2 : ModProjectile
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
            projectile.timeLeft = 150;
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

            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Fire, 0f, 0f, 100, Color.Orange, 1.5f);
            Main.dust[dust].noGravity = true;
            Main.dust[dust].velocity = projectile.velocity;
        }
    }

    class IncendiaryDemonBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Incendiary Demon");
            Description.SetDefault("An incendiary demon is fighting for you!");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<IncendiaryDemon>())
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}
