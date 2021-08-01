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
    class AnglerStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Summons a flying angler to electrocute foes");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 42;
            item.mana = 10;
            item.summon = true;
            item.damage = 34;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.noMelee = true;
            item.knockBack = 0;
            item.rare = 5;
            item.UseSound = SoundID.Item44;
            item.shoot = mod.ProjectileType("FlyingAngler");
            item.shootSpeed = 10f;
            item.value = Item.sellPrice(0, 5, 0, 0);
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
                    player.AddBuff(ModContent.BuffType<FlyingAnglerBuff>(), 60);
                }
            }
            return false;
        }
    }
    class FlyingAngler : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = 4;
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
            projectile.width = 52;
            projectile.height = 36;
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
            DustExplosion(projectile.Center, 0, 12, 7, 62, 2f, true);
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

                Dust dust = Dust.NewDustPerfect(position + (new Vector2(Main.rand.Next(RectWidth), Main.rand.Next(RectWidth))), DustType, direction, 0, Color.White, DustScale);
                if (NoGravity)
                {
                    dust.noGravity = true;
                }
            }
        }

        int mode = 0;
        int projectileCounter = 10;
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

            if (!player.HasBuff(ModContent.BuffType<FlyingAnglerBuff>()))
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
                if (projectile.Distance(target.Center) > 450)
                {
                    float speed = 0.4f;
                    projectile.velocity += projectile.DirectionTo(target.Center) * speed;
                    projectile.velocity *= 0.98f;
                }

                projectileCounter--;
                if (projectileCounter <= 0)
                {
                    projectileCounter = 20;
                    SpawnLightning(target.whoAmI);
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            base.PostDraw(spriteBatch, lightColor);
            SpriteEffects effects = SpriteEffects.None;
            if (projectile.spriteDirection == -1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = ModContent.GetTexture(Texture + "_Glow");
            Vector2 position = projectile.position - Main.screenPosition;
            position += new Vector2(projectile.width / 2, projectile.height / 2);
            //position.Y += 4;
            Main.spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, projectile.width, projectile.height), new Rectangle(0, projectile.frame * projectile.height, projectile.width, projectile.height), projectile.GetAlpha(Color.White), projectile.rotation, new Vector2(projectile.width / 2, projectile.height / 2), effects, 0);
        }

        public void SpawnLightning(int target)
        {
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.NextFloat(10f, 25f);

            int proj = Projectile.NewProjectile(projectile.Center, direction * speed, ModContent.ProjectileType<Projectiles.SoulLightning>(), projectile.damage, 0.5f, projectile.owner);
            Main.projectile[proj].ai[0] = target;
        }
    }

    class FlyingAnglerBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Flying Angler");
            Description.SetDefault("An airborne swimmer fights for you!");
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
                if (Main.projectile[i].type == ModContent.ProjectileType<FlyingAngler>())
                {
                    player.buffTime[buffIndex] = 60;
                }
            }
        }
    }
}

