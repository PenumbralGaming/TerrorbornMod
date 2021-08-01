using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TerrorbornMod.NPCs.TownNPCs
{
    [AutoloadHead]
    public class SkeletonSheriff : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "TerrorbornMod/NPCs/TownNPCs/SkeletonSheriff";
            }
        }

        public override bool Autoload(ref string name)
        {
            name = "SkeletonSheriff";
            return mod.Properties.Autoload;
        }

        public override void SetStaticDefaults()
        {
			Main.npcFrameCount[npc.type] = 25;
			NPCID.Sets.ExtraFramesCount[npc.type] = 10;
			NPCID.Sets.AttackFrameCount[npc.type] = 4;
			NPCID.Sets.DangerDetectRange[npc.type] = 700;
			NPCID.Sets.AttackType[npc.type] = 0;
			NPCID.Sets.AttackTime[npc.type] = 90;
			NPCID.Sets.AttackAverageChance[npc.type] = 30;
			NPCID.Sets.HatOffsetY[npc.type] = 4;
        }

        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 23;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.knockBackResist = 0f;
            animationType = NPCID.Guide;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.CursedInferno] = true;
        }

        public override void NPCLoot()
        {
            Main.NewText("<" + npc.GivenName + " the Skeleton Sheriff> Don't worry, I'm an undead. I'll be back....", Color.Yellow);
            Item.NewItem(npc.getRect(), ModContent.ItemType<Items.Equipable.Vanity.SheriffsHat>());
        }

        public override bool UsesPartyHat()
        {
            return false;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(mod.ItemType("BountyHunterCap"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 30;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("BountyHunterLeatherwear"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 30;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("BountyHunterPants"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 30;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("BoneBuster"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 20;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("CartilageRound"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 1;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Melee.BoneBreaker>());
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 30;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.PearlyEyedStaff>());
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 30;
            nextSlot++;
            if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Ammo.PincushionArrow>());
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 1;
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(mod.ItemType("ThornyMaraca"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 45;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Weapons.Magic.Bombinomicon>());
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 45;
            nextSlot++;
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("TheDoubleBarrel"));
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 125;
                nextSlot++;
            }
            if (NPC.downedPlantBoss)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("Bonezooka"));
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 195;
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(ItemID.Worm);
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 3;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.LifeCrystal);
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 8;
            nextSlot++;
            if (NPC.downedMechBoss1 || NPC.downedMechBoss2 || NPC.downedMechBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.LifeFruit);
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 20;
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(ItemID.BattlePotion);
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ItemID.CalmingPotion);
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 15;
            nextSlot++;
            if (NPC.downedBoss3)
            {
                shop.item[nextSlot].SetDefaults(ItemID.BlueBrick);
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 1;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.GreenBrick);
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 1;
                nextSlot++;
                shop.item[nextSlot].SetDefaults(ItemID.PinkBrick);
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 1;
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(mod.ItemType("DeputyBag_prehm"));
            shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
            shop.item[nextSlot].shopCustomPrice = 25;
            nextSlot++;
            if (Main.hardMode)
            {
                shop.item[nextSlot].SetDefaults(mod.ItemType("DeputyBag_hm"));
                shop.item[nextSlot].shopSpecialCurrency = TerrorbornMod.CombatTokenCustomCurrencyId;
                shop.item[nextSlot].shopCustomPrice = 65;
                nextSlot++;
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            if (NPC.downedBoss2)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }

        public override string TownNPCName()
        {
            return TerrorbornWorld.SkeletonSheriffName;
        }

        int currentOption1 = 0;
        const int optionCount = 3;
        int loreText = 0;
        int loreTextCount = 5;
        bool showingLore = false;

        public override void PostAI()
        {
            base.PostAI();
            if (Main.player[Player.FindClosest(npc.Center, 0, 0)].Distance(npc.Center) > 300)
            {
                showingLore = false;
                loreText = 0;
                currentOption1 = 0;
            }
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (showingLore)
            {
                button2 = Language.GetTextValue("Cycle Lore");
                if (loreText == 0)
                {
                    button = Language.GetTextValue("Nevermind");
                }
                if (loreText == 1)
                {
                    button = Language.GetTextValue("How/when did you become undead?");
                }
                if (loreText == 2)
                {
                    button = Language.GetTextValue("Tell me about Anekronyx");
                }
                if (loreText == 3)
                {
                    button = Language.GetTextValue("What were the Anekronian tournaments like?");
                }
                if (loreText == 4)
                {
                    button = Language.GetTextValue("Who was Rath?");
                }
                return;
            }
            if (currentOption1 == 0)
            {
                button = Language.GetTextValue("Shop");
            }
            else if (currentOption1 == 1)
            {
                button = Language.GetTextValue("Combat Bounty");
            }
            else if (currentOption1 == 2)
            {
                button = Language.GetTextValue("Talk");
            }
            button2 = Language.GetTextValue("Cycle Options");
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool openShop) 
        {
            if (firstButton)
            {
                if (showingLore)
                {
                    if (loreText == 0) //Nevermind
                    {
                        Main.npcChatText = "I see. Well, how can I help you?";
                        showingLore = false;
                    }
                    if (loreText == 1) //How/when did you become undead?
                    {
                        Main.npcChatText = "Well, I've been undead for as long as I can remember. Literally. When a spirit is revived as an undead, they lose all of their memories, in most cases also losing their character, becoming viscious beasts. If you were killed in a very specific way, apparently you maintain your character. It's very interesting, actually.";
                    }
                    if (loreText == 2) //Tell me about Anekronyx
                    {
                        Main.npcChatText = "Ahhhh, the grand kingdom of Anekronyx. You see, the population was primarily composed of 'shadows', a race that is created through magic similar to necromancy by the Anekronian king, Tenebris. After a foolhearty decision on his behalf, he lost his kingdom and the ability to create these beings. From my knowledge, he never died, though.";
                    }
                    if (loreText == 3) //What were the Anekronian tournaments like?
                    {
                        Main.npcChatText = "I'm glad you asked. You see, Tenebris would invite all warriors around the globe to fight in a grand colloseum. The rewards got better and better the higher up you got in the ranks of this tournament, but you'd also fight increasingly strong foes, naturally. There were 1v1 tournaments and 2v2 tournaments. I used to be the champion, until I was defeated by a terrarian like yourself.";
                    }
                    if (loreText == 4) //Who was rath?
                    {
                        Main.npcChatText = "Rath, well... he was a good friend. In the kingdom of Anekronyx, he was a researcher, primarily for military purposes. That said, he had a great fascination with terror and incarnates. After a certain decision from the king, he and a few others left the kingdom to establish their own. That kingdom ended up failing for... other reasons. You'd probably get along with him.";
                    }
                    return;
                }
                if (currentOption1 == 0)
                {
                    openShop = true;
                }
                else if (currentOption1 == 1)
                {
                    Player player = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
                    TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
                    string BiomeText = "Biome Text Broke :P, report this on the Terrorborn discord.";
                    string CombatTokenText;
                    if (modPlayer.CombatPoints / 250 == 0)
                    {
                        CombatTokenText = "You haven't done enough peacekeeping for any combat tokens, yet. ";
                    }
                    else
                    {
                        player.QuickSpawnItem(mod.ItemType("CombatToken"), modPlayer.CombatPoints / 250);
                        CombatTokenText = "Seems you've been doing work. Here's " + modPlayer.CombatPoints / 250 + " Combat Tokens as credit! ";
                        modPlayer.CombatPoints = 0;
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 0)
                    {
                        BiomeText = "If you want tokens, hang out in the sandy caves. It seems there is an unusual concentration of antlion breeding going on.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 1)
                    {
                        BiomeText = "In the icy caves, the monsters seem to have been getting quite powerful. If you want credit, that's the place to go.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 2)
                    {
                        BiomeText = "Do you want combat tokens? Got to hell! In all seriousness, the beasts of the underworld have been getting out of hand. Try to clean things up down there.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 3)
                    {
                        BiomeText = "The tropical forests of the jungle have had some issues at the surface as of late. If you want some tokens, I'll give you the credit it you spend some time peacekeeping there.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 4)
                    {
                        BiomeText = "Under the tropical rainforests, in those lush caves, I hear there's been some trouble. If you want credit, I'll give it to you- as long as you've killed enough enemies in the underground jungle.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 5)
                    {
                        if (WorldGen.crimson)
                        {
                            BiomeText = "The deadliest of bloody creatures have been forming at a faster rate than normal- in the crimson, that is. If you go there and kill some stuff, I'll be waiting here to give you tokens.";
                        }
                        else
                        {
                            BiomeText = "The infectious purple corruption has had some activity as of late. If you're in need of tokens, that's the place to go for it.";
                        }
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 6)
                    {
                        BiomeText = "If you want some tokens, the surface of that large mass of snow is where you can kill things for credit. For whatever reason, it seems to be oddly active today.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 7)
                    {
                        BiomeText = "Remember those fairy tales, with the nice unicorns? Well guess what, they aren't that nice, and they're stampeeding all over the place in the hallowed biome! If you kill some monsters over there, I'll give you tokens.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 8)
                    {
                        BiomeText = "Looking for tokens? The chaotic depths of the hallowed biome are being extra chaotic today, so if you were to take charge of some of the creatures living there, it would be much appreciated.";
                    }
                    if (TerrorbornWorld.CurrentBountyBiome == 9)
                    {
                        if (WorldGen.crimson)
                        {
                            BiomeText = "They say the monsters in the deep crimson are powered by the blood of gods. The gods must be bleeding a lot, because it's getting crazy over there! I'll give you tokens if you can check out what's going on.";
                        }
                        else
                        {
                            BiomeText = "The most cursed of flamse are being extra cursed today. Why? I just don't know. What I do know is that if you douse the flames in the corrupt depths, I'll give you tokens!";
                        }
                    }
                    Main.npcChatText = CombatTokenText + BiomeText;
                }
                else if (currentOption1 == 2)
                {
                    showingLore = true;
                    Main.npcChatText = "What would you like to talk about?";
                }
            }
            else
            {
                if (showingLore)
                {
                    loreText++;
                    if (loreText >= loreTextCount)
                    {
                        loreText = 0;
                    }
                    return;
                }
                currentOption1++;
                if (currentOption1 >= optionCount)
                {
                    currentOption1 = 0;
                }
            }
        }
        
        //public override void SetupShop(Chest shop, ref int nextSlot)
        //{

        //}
        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            
            chat.Add("I just want to thank you for doing my work. As an undead, people don't trust me, and that makes peacekeeping quite hard.");
            chat.Add("Ever heard the story of Navaylos? It ain't a happy one. While I don't know a lot about it, all you need to know is that he's bad, and that rumors tell he's still around.");
            chat.Add("Hey, do you want this vial of... oh yeah... poison is deadly to the living. Nevermind, but it's the thought that counts, right?");
            if (NPC.downedBoss3)
            {
                if (NPC.AnyNPCs(NPCID.Clothier))
                {
                    string clothierName = Main.npc[NPC.FindFirstNPC(NPCID.Clothier)].GivenName;
                    chat.Add("I'm proud of you for freeing" + clothierName + " of his curse. He says that he was forced to guard a dungeon full of angry undead. Glad I'm not one of those undead.");
                }
                chat.Add("I'm proud of you for freeing that old guy at that strange structure of his curse. Makes me wonder what he was being forced to guard.");
            }
            else
            {
                chat.Add("Have you seen that creepy old structure? Yeah, it's pretty creepy. There's this old guy guarding it... wonder what would happen if you tried to sneak past him.");
            }
            chat.Add("Way back when, I had a friend who went by the name of Rath. He lived in the grand kingdom of Anekronyx, and was a member of a race that's supposedly extinct. Legend has it though that the Anekronians still have one member alive, I wonder if it could be him.");
            chat.Add("I remember the good old days, back when king Tenebris held his fun old gladiator's tournaments. They're gone now, but I remember participating in one clearly! Hah, I was the champion until a terrarian came around and beat me. You kinda remind me of them, actually...");
            chat.Add("Remember, I'll only give you things if you help me do my job. It's certainly not an easy one, so be prepared for some... 'fun'.");
            chat.Add("I've always wondered... what's it like being an incarnate? I suppose it's something regular beings such as myself will never understand. After all, it's impossible to make yourself an incarnate. At least, I think....");
            chat.Add("You know how the others look at me suspiciously? Yeah, that's why I need you to do my job for me. Nobody trusts a corpse to do the law enforcement, as I learned the hard way.");
            return chat;
        }


        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 22;
            knockback = 7f;
            if (Main.hardMode)
            {
                damage = 70;
                knockback = 10f;
            }
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 0;
            randExtraCooldown = 0;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = ProjectileID.BoneGloveProj;
            attackDelay = 1;
        }

        public override void TownNPCAttackShoot(ref bool inBetweenShots)
        {
            inBetweenShots = false;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 7f;
            gravityCorrection = 0;
        }
    }
}
