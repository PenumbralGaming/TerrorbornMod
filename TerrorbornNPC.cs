using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using System;

namespace TerrorbornMod
{
    public class TerrorbornNPC : GlobalNPC
    {
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public float ogKnockbackResist;
        public int soulSplitTime = 0;
        public int soulOrbCooldown = 0;
        public int mindSpearheadTime = 0;
        public bool extraWormSegment = false;
        public int CumulusEmpowermentTime = 0;
        public int SoulEaterTotalDamageTaken;
        public int SoulReaperTotalDamageTaken;

        public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
        {
            TerrorbornPlayer player = TerrorbornPlayer.modPlayer(target);
            if (player.iFrames > 0 || player.VoidBlinkTime > 0)
            {
                return false;
            }
            return base.CanHitPlayer(npc, target, ref cooldownSlot);
        }

        public override void SetDefaults(NPC npc)
        {
            ogKnockbackResist = npc.knockBackResist;
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.TravellingMerchant)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.HermesFeather>());
                nextSlot++;
            }
        }

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && npc.life >= npc.lifeMax)
            {
                crit = true;
            }

            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (modPlayer.LiesOfNourishment && npc.life >= npc.lifeMax)
            {
                crit = true;
            }

            if (CumulusEmpowermentTime > 0)
            {
                damage = (int)(damage * 0.5f);
                knockback = 0;
            }
        }

        public override void PostAI(NPC npc)
        {
            if (soulSplitTime > 0)
            {
                soulSplitTime--;
                if (soulSplitTime <= 0)
                {
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), Color.LightCyan, "Soul Regained");
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath39, npc.Center);
                }
            }
            if (soulOrbCooldown > 0)
            {
                soulOrbCooldown--;
            }

            if (mindSpearheadTime > 0)
            {
                mindSpearheadTime--;
                if (mindSpearheadTime <= 0)
                {
                    CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(80, 112, 109), "Spearhead Dropped!", false, true);
                }
            }

            if (CumulusEmpowermentTime > 0)
            {
                CumulusEmpowermentTime--;
                int d = Dust.NewDust(npc.position, npc.width, npc.height, 21);
                Main.dust[d].noGravity = true;
                Main.dust[d].velocity = npc.velocity;
            }
        }

        public static TerrorbornNPC modNPC(NPC npc)
        {
            return npc.GetGlobalNPC<TerrorbornNPC>();
        }

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (player.ZoneRain && TerrorbornWorld.terrorRain && maxSpawns != 0) //Checks current maxSpawns specifically so it works with HERO's mod's spawn thingy
            {
                if (Main.dayTime)
                {
                    spawnRate = (int)(spawnRate * 0.35f);
                }
                else
                {
                    spawnRate = (int)(spawnRate * 0.55f);
                }
                //spawnRate = 180;
                maxSpawns = (int)(maxSpawns * 2f);
            }
            //maxSpawns += TerrorbornWorld.wormExtraSegmentCount;
        }

        List<int> terrorRainEnemies = new List<int>()
        {
            { ModContent.NPCType<NPCs.TerrorRain.VentedCumulus>() },
            { ModContent.NPCType<NPCs.TerrorRain.VenomHopper>() },
            { ModContent.NPCType<NPCs.TerrorRain.BrainStinger>() },
            { ModContent.NPCType<NPCs.TerrorRain.DownpourGecko>() },
        };

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.ZoneRain && TerrorbornWorld.terrorRain)
            {
                pool.Clear();

                foreach (int i in terrorRainEnemies)
                {
                    float weight = 1f;
                    if (i == ModContent.NPCType<NPCs.TerrorRain.VentedCumulus>())
                    {
                        weight = 0.3f;
                    }
                    if (i == ModContent.NPCType<NPCs.TerrorRain.BrainStinger>())
                    {
                        weight = 0.8f;
                    }
                    //if (i == ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>())
                    //{
                    //    if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>()))
                    //    {
                    //        weight = 0f;
                    //    }
                    //    else
                    //    {
                    //        weight = 0.07f;
                    //    }
                    //}
                    if (i == ModContent.NPCType<NPCs.TerrorRain.DownpourGecko>())
                    {
                        weight = 0.6f;
                    }
                    pool.Add(i, weight);
                }
            }
        }

        public bool CheckBountyBiome(Player player)
        {
            if (TerrorbornWorld.CurrentBountyBiome == 0 && player.ZoneUndergroundDesert)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 1 && player.ZoneSnow && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 2 && player.ZoneUnderworldHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 3 && player.ZoneJungle && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 4 && player.ZoneJungle && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 5 && (player.ZoneCorrupt || player.ZoneCrimson) && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 6 && player.ZoneSnow)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 7 && player.ZoneHoly && !player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 8 && player.ZoneHoly && player.ZoneRockLayerHeight)
            {
                return true;
            }
            if (TerrorbornWorld.CurrentBountyBiome == 9 && (player.ZoneCorrupt || player.ZoneCrimson) && player.ZoneRockLayerHeight)
            {
                return true;
            }
            return false;
        }

        void SpawnGeyser(int originalDamage, NPC npc, Player player)
        {
            if (Main.rand.NextFloat() > 0.15f && npc.life > 0)
            {
                return;
            }
            for (int i = 0; i < Main.rand.Next(2, 5); i++)
            {
                Vector2 position = npc.Center;
                if (i != 1)
                {
                    position.X += Main.rand.Next(-200, 200);
                }
                while (!WorldUtils.Find(position.ToTileCoordinates(), Searches.Chain(new Searches.Down(1), new GenCondition[]
                    {
        new Conditions.IsSolid()
                    }), out _))
                {
                    position.Y++;
                }
                Projectile.NewProjectile(position, new Vector2(0, -20), ModContent.ProjectileType<Items.Equipable.Armor.TideFireFriendly>(), originalDamage / 2, 0f, player.whoAmI);
            }
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item88, npc.Center);
        }

        void SpawnAzuriteShard(int originalDamage, NPC npc, Player player)
        {
            if (Main.rand.NextFloat() > 0.25f)
            {
                return;
            }
            Vector2 direction = MathHelper.ToRadians(Main.rand.Next(360)).ToRotationVector2();
            float speed = Main.rand.Next(15, 25);
            Projectile.NewProjectile(npc.Center, direction * speed, ModContent.ProjectileType<Projectiles.AzuriteShard>(), originalDamage / 2, 0f, player.whoAmI);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item118, npc.Center);
        }

        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
            Player player = Main.LocalPlayer;
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (npc.life <= 0 && !npc.SpawnedFromStatue)
            {
                if (modPlayer.SoulEater)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                }
            }

            if (modPlayer.SoulEater && npc.boss)
            {
                SoulEaterTotalDamageTaken += (int)damage;
                while (SoulEaterTotalDamageTaken > npc.lifeMax * 0.075f)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<TerrorOrb>(), 0, 0, player.whoAmI);
                    SoulEaterTotalDamageTaken -= (int)(npc.lifeMax * 0.075f);
                }
            }

            if (modPlayer.SoulReaperArmorBonus && npc.boss)
            {
                SoulReaperTotalDamageTaken += (int)damage;
                while (SoulReaperTotalDamageTaken > npc.lifeMax * 0.075f)
                {
                    Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
                    SoulReaperTotalDamageTaken -= (int)(npc.lifeMax * 0.075f);
                }
            }
        }

        public override void OnHitByItem(NPC npc, Player player, Item item, int damage, float knockback, bool crit)
        {
            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit36, npc.Center);
                    soulOrbCooldown = 3;
                }
            }

            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (item.melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, npc, player);
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, npc, player);
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (npc.life <= 0)
                {
                    if (CheckBountyBiome(player))
                    {
                        modPlayer.CombatPoints += npc.lifeMax;
                    }
                }
            }
            if (darkblood)
            {
                modPlayer.terrorDrainCounter = 30;
            }
            if (item.melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.meleeCrit / 2)
                {
                    DustExplosion(npc.Center, 0, 45, 30, DustID.Fire, DustScale: 1f, NoGravity: true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && npc.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }
                            else
                            {
                                target.StrikeNPC(damage, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }

                            int choice = Main.rand.Next(4);
                            if (choice == 0)
                            {
                                target.AddBuff(BuffID.OnFire, 60 * 5);
                            }
                            if (choice == 1)
                            {
                                target.AddBuff(BuffID.Frostburn, 60 * 5);
                            }
                            if (choice == 2)
                            {
                                target.AddBuff(BuffID.CursedInferno, 60 * 5);
                            }
                            if (choice == 3)
                            {
                                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
                            }
                        }
                    }
                }
            }
        }

        bool start = true;
        public override void AI(NPC npc)
        {
            if (start)
            {
                start = false;
                ogKnockbackResist = npc.knockBackResist;
            }
        }

        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(Main.player[projectile.owner]);
            Player player = Main.player[projectile.owner];
            bool darkblood = player.HasBuff(ModContent.BuffType<Buffs.Darkblood>());

            if (projectile.melee && modPlayer.TidalShellArmorBonus)
            {
                SpawnGeyser(damage, npc, player);
            }

            if (modPlayer.AzuriteBrooch)
            {
                SpawnAzuriteShard(damage, npc, player);
            }

            if (soulSplitTime > 0)
            {
                if (soulOrbCooldown <= 0)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<SoulOrb>(), 0, 0, player.whoAmI);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCHit36, npc.Center);
                    soulOrbCooldown = 3;
                }
            }

            if (darkblood)
            {
                modPlayer.terrorDrainCounter = 30;
            }

            if (NPC.AnyNPCs(ModContent.NPCType<NPCs.TownNPCs.SkeletonSheriff>()))
            {
                if (npc.life <= 0)
                {
                    if (CheckBountyBiome(Main.player[projectile.owner]))
                    {
                        modPlayer.CombatPoints += npc.lifeMax;
                    }
                }
            }
            if (projectile.melee && modPlayer.IncendiaryShield)
            {
                if (Main.rand.Next(101) <= 8 + player.meleeCrit / 2)
                {
                    DustExplosion(npc.Center, 0, 45, 30, DustID.Fire, DustScale: 1f, NoGravity: true);
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, npc.Center);
                    for (int i = 0; i < 200; i++)
                    {
                        NPC target = Main.npc[i];
                        if (!target.friendly && npc.Distance(target.Center) <= 200 + (target.width + target.height) / 2 && !target.dontTakeDamage)
                        {
                            if (target.type == NPCID.TheDestroyerBody)
                            {
                                target.StrikeNPC(damage / 10, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }
                            else
                            {
                                target.StrikeNPC(damage, 0, 0, Main.rand.Next(101) <= player.meleeCrit);
                            }

                            int choice = Main.rand.Next(4);
                            if (choice == 0)
                            {
                                target.AddBuff(BuffID.OnFire, 60 * 5);
                            }
                            if (choice == 1)
                            {
                                target.AddBuff(BuffID.Frostburn, 60 * 5);
                            }
                            if (choice == 2)
                            {
                                target.AddBuff(BuffID.CursedInferno, 60 * 5);
                            }
                            if (choice == 3)
                            {
                                target.AddBuff(BuffID.ShadowFlame, 60 * 5);
                            }
                        }
                    }
                }
            }
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
        public override void NPCLoot(NPC npc)
        {
            Player player = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            List<int> extraDarkEnergyIDs = new List<int>();
            extraDarkEnergyIDs.Add(NPCID.SkeletronHand);
            extraDarkEnergyIDs.Add(NPCID.GolemFistLeft);
            extraDarkEnergyIDs.Add(NPCID.GolemFistRight);
            extraDarkEnergyIDs.Add(NPCID.Paladin);
            extraDarkEnergyIDs.Add(NPCID.PirateCaptain);
            extraDarkEnergyIDs.Add(NPCID.MourningWood);
            extraDarkEnergyIDs.Add(NPCID.Pumpking);
            extraDarkEnergyIDs.Add(NPCID.Everscream);
            extraDarkEnergyIDs.Add(NPCID.SantaNK1);
            extraDarkEnergyIDs.Add(NPCID.IceQueen);
            extraDarkEnergyIDs.Add(NPCID.DD2DarkMageT1);
            extraDarkEnergyIDs.Add(NPCID.DD2DarkMageT3);
            extraDarkEnergyIDs.Add(NPCID.DD2OgreT2);
            extraDarkEnergyIDs.Add(NPCID.DD2OgreT3);
            extraDarkEnergyIDs.Add(NPCID.DD2Betsy);
            extraDarkEnergyIDs.Add(NPCID.BigMimicCorruption);
            extraDarkEnergyIDs.Add(NPCID.BigMimicCrimson);
            extraDarkEnergyIDs.Add(NPCID.BigMimicHallow);
            extraDarkEnergyIDs.Add(NPCID.GoblinSummoner);
            extraDarkEnergyIDs.Add(NPCID.IceGolem);
            extraDarkEnergyIDs.Add(ModContent.NPCType<NPCs.Bosses.Sangrune>());
            extraDarkEnergyIDs.Add(ModContent.NPCType<NPCs.UndyingSpirit>());
            extraDarkEnergyIDs.Add(ModContent.NPCType<NPCs.TerrorRain.FrightcrawlerHead>());

            if (modPlayer.SoulReaperArmorBonus)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Armor.ThunderSoul>());
            }

            if (TerrorbornWorld.terrorRain && player.ZoneRain && Main.rand.NextFloat() <= 0.5f)
            {
                int type = ItemID.DirtBlock;
                switch (Main.rand.Next(3))
                {
                    case 0: type = ItemID.SoulofNight;
                        break;
                    case 1: type = ItemID.SoulofLight;
                        break;
                    case 2: type = ItemID.SoulofFlight;
                        break;
                }
                Item.NewItem(npc.getRect(), type);
            }

            if (NPCID.Sets.ProjectileNPC[npc.type])
            {
                return;
            }

            float ExpertBoost = 1;
            if (Main.expertMode)
            {
                ExpertBoost = 2;
            }

            if (npc.type == NPCID.GoblinSorcerer)
            {
                if (Main.rand.NextFloat() <= 0.15f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Magic.BookOfChaos>());
                }
            }

            if (npc.type == NPCID.GoblinArcher)
            {
                if (Main.rand.NextFloat() <= 0.08f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Ranged.WarBow>());
                }
            }

            if (npc.type == NPCID.EyeofCthulhu)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.EyeOfTheMenace>());
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Weapons.Summons.Minions.OpticCane>());
            }

            if (npc.type == NPCID.MoonLordCore)
            {
                modPlayer.TerrorPercent = 0f;
            }

            if (extraDarkEnergyIDs.Contains(npc.type) || npc.boss)
            {
                for (int i = 0; i < Main.rand.Next(3, 5); i++)
                {
                    Item.NewItem(npc.Center, ModContent.ItemType<Items.DarkEnergy>());
                }
            }

            if (npc.type == NPCID.WallofFlesh)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.DemonicLense>());
            }

            if (npc.type == NPCID.Mothron && Main.rand.NextFloat() <= 0.33f)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Weapons.Summons.Other.Armagrenade>());
            }

            if (npc.type == NPCID.MartianSaucerCore)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.Equipable.Accessories.Wings.MartianBoosters>());
            }

            if (npc.type == NPCID.SkeletronHead)
            {
                Item.NewItem(npc.Center, ModContent.ItemType<Items.PermanentUpgrades.CoreOfFear>());
            }

            if (npc.type == NPCID.GraniteFlyer || npc.type == NPCID.GraniteGolem)
            {
                if (Main.rand.NextFloat(101) <= 0.45 * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.graniteVirusSpark>());
                }
            }

            if (npc.type == NPCID.Antlion || npc.type == NPCID.FlyingAntlion || npc.type == NPCID.WalkingAntlion)
            {
                if (Main.rand.NextFloat() <= 0.01f * ExpertBoost)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Equipable.Accessories.AntsMandible>());
                }
            }

            if (player.ZoneBeach && Main.rand.NextFloat() <= 0.02f)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.LunarRitual>());
            }

            if (npc.type == NPCID.KingSlime)
            {
                bool spawnGA = !TerrorbornPlayer.modPlayer(Main.player[Main.myPlayer]).unlockedAbilities.Contains(6);
                for (int i = 0; i < 1000; i++)
                {
                    Projectile projectile = Main.projectile[i];
                    if (projectile.active && projectile.type == ModContent.ProjectileType<Abilities.GelatinArmor>())
                    {
                        spawnGA = false;
                    }
                }

                if (spawnGA)
                {
                    Projectile.NewProjectile(npc.Center, Vector2.Zero, ModContent.ProjectileType<Abilities.GelatinArmor>(), 0, 0, Main.myPlayer);
                }
            }

            if (modPlayer.LiesOfNourishment && Main.rand.Next(5) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart);
            }

            if (Main.rand.Next(12) == 0 && modPlayer.TerrorPercent < 100 && TerrorbornWorld.obtainedShriekOfHorror)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.DarkEnergy>());
            }
        }
    }


    class SoulOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 132;
            if (modPlayer.SanguineSetBonus) type = 130;
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, type);
            Main.dust[dust].velocity = projectile.velocity;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 8;
            if (modPlayer.SanguineSetBonus) speed = 25;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                int healAmount = 1;
                player.HealEffect(healAmount);
                player.statLife += healAmount;
                projectile.active = false;
            }
        }
    }

    class TerrorOrb : ModProjectile
    {
        public override string Texture { get { return "Terraria/Projectile_" + ProjectileID.EmeraldBolt; } }
        //private bool HasGravity = true;
        //private bool Spawn = true;
        //private bool GravDown = true;
        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.aiStyle = 0;
            projectile.tileCollide = true;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.hide = true;
            projectile.timeLeft = 300;
            projectile.damage = 0;
        }

        int Direction = 1;
        int DirectionCounter = 5;
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            int type = 54;
            int dust = Dust.NewDust(projectile.Center, 0, 0, type);
            Main.dust[dust].velocity = Vector2.Zero;
            Main.dust[dust].scale = 1f;
            Main.dust[dust].alpha = 255 / 2;
            Main.dust[dust].noGravity = true;
            Main.dust[dust].color = Color.White;


            int speed = 25;
            projectile.velocity = projectile.DirectionTo(player.Center) * speed;
            if (projectile.Distance(player.Center) <= speed)
            {
                modPlayer.TerrorPercent += 2f;
                if (modPlayer.TerrorPercent > 100)
                {
                    modPlayer.TerrorPercent = 100;
                }
                CombatText.NewText(player.getRect(), Color.FromNonPremultiplied(108, 150, 143, 255), "2%");
                projectile.active = false;
            }
        }
    }
}
