using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerrorbornMod.Items.Weapons.Restless
{
    class SoulCyclone : RestlessWeapon
    {
        public override string Texture => "TerrorbornMod/Items/Weapons/Restless/SoulCyclone";
        int UntilBlast;
        public override void restlessSetStaticDefaults()
        {
            DisplayName.SetDefault("Soul Cyclone");
            Item.staff[item.type] = true;
        }

        public override string defaultTooltip()
        {
            return "Fires a random type of soul" +
                "\nEach soul type has a different behavior";
        }

        public override string altTooltip()
        {
            return "Fires all soul types at once";
        }

        public override void restlessSetDefaults(TerrorbornItem modItem)
        {
            item.damage = 66;
            item.noMelee = true;
            item.magic = true;
            item.width = 74;
            item.mana = 15;
            item.height = 78;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = ItemUseStyleID.HoldingOut;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item8;
            item.shoot = ModContent.ProjectileType<SoulProjectile>();
            item.shootSpeed = 20;
            item.crit = 7;
            item.autoReuse = true;
            modItem.restlessTerrorDrain = 8f;
            modItem.restlessChargeUpUses = 4;
        }

        public override bool RestlessCanUseItem(Player player)
        {
            return base.RestlessCanUseItem(player);
        }

        public override bool RestlessShoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            TerrorbornItem modItem = TerrorbornItem.modItem(item);
            if (modItem.RestlessChargedUp())
            {
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item117, player.Center);
                for (int i = 0; i < 7; i++)
                {
                    int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                    Main.projectile[proj].ai[0] = i;
                }
            }
            else
            {
                int proj = Projectile.NewProjectile(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI);
                Main.projectile[proj].ai[0] = Main.rand.Next(7);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Materials.ThunderShard>(), 20);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ModContent.ItemType<Materials.TerrorSample>(), 5);
            recipe.AddIngredient(ModContent.ItemType<Materials.SoulOfPlight>(), 5);
            recipe.AddIngredient(ItemID.SoulofFright, 5);
            recipe.AddIngredient(ItemID.SoulofSight, 5);
            recipe.AddIngredient(ItemID.SoulofMight, 5);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(ItemID.SoulofFlight, 5);
            recipe.AddTile(ModContent.TileType<Tiles.MeldingStation>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    class SoulProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 8;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
        }
        
        //SOUL NUMBERS:
        //0 = Soul of Flight
        //1 = Soul of Light
        //2 = Soul of Night
        //3 = Soul of Might
        //4 = Soul of Sight
        //5 = Soul of Fright
        //6 = Soul of Plight

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            //Thanks to Seraph for afterimage code.
            Color soulColor = Color.White;
            switch (projectile.ai[0])
            {
                case 0:
                    soulColor = Color.Cyan;
                    break;
                case 1:
                    soulColor = Color.HotPink;
                    break;
                case 2:
                    soulColor = Color.Purple;
                    break;
                case 3:
                    soulColor = Color.CornflowerBlue;
                    break;
                case 4:
                    soulColor = Color.FromNonPremultiplied(144, 255, 144, 255);
                    break;
                case 5:
                    soulColor = Color.OrangeRed;
                    break;
                case 6:
                    soulColor = Color.Green;
                    break;
            }

            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                Vector2 drawPos = projectile.oldPos[i] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(soulColor) * ((float)(projectile.oldPos.Length - i) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle?(), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 30;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = 3;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.timeLeft = 300;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 60;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 14;
            height = 14;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);

            switch (projectile.ai[0])
            {
                case 0:
                    FlightAI();
                    break;
                case 1:
                    LightAI();
                    break;
                case 2:
                    NightAI();
                    break;
                case 3:
                    MightAI();
                    break;
                case 4:
                    SightAI();
                    break;
                case 5:
                    FrightAI();
                    break;
                case 6:
                    PlightAI();
                    break;
            }
        }

        public void FlightAI()
        {
            projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(Main.MouseWorld).ToRotation(), MathHelper.ToRadians(5f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public void LightAI()
        {
            int RotatationSpeed = 5;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * Direction));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }

        public void NightAI()
        {
            int RotatationSpeed = 5;
            projectile.velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(RotatationSpeed * -Direction));
            DirectionCounter--;
            if (DirectionCounter <= 0)
            {
                DirectionCounter = 10;
                Direction *= -1;
            }
        }

        public void MightAI()
        {
            float speed = 3f;
            float currentVelocityLength = projectile.velocity.Length();
            projectile.velocity.Normalize();
            projectile.velocity *= currentVelocityLength + speed;
        }

        public void SightAI()
        {
            NPC targetNPC = Main.npc[0];
            float Distance = 700; //max distance away
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
                projectile.velocity = projectile.velocity.ToRotation().AngleTowards(projectile.DirectionTo(targetNPC.Center).ToRotation(), MathHelper.ToRadians(2f * (projectile.velocity.Length() / 20))).ToRotationVector2() * projectile.velocity.Length();
            }
        }

        public void FrightAI()
        {
            projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(15));
        }

        public void PlightAI()
        {
            projectile.extraUpdates = 2;
        }
    }
}
