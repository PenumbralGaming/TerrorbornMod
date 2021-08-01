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
    public class TerrorMaster : ModNPC
    {
        public override string Texture
        {
            get
            {
                return "TerrorbornMod/NPCs/TownNPCs/TerrorMaster";
            }
        }
        public override bool Autoload(ref string name)
        {
            name = "TerrorMaster";
            return mod.Properties.Autoload;
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 10;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 250;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 5;
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
            npc.defense = 45;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit54;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.knockBackResist = 0f;
            animationType = NPCID.Guide;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                npc.life = 1;
            }
        }

        public override bool UsesPartyHat()
        {
            return true;
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money)
        {
            return TerrorbornWorld.obtainedShriekOfHorror;
        }
        public override bool CheckConditions(int left, int right, int top, int bottom)
        {
            return true;
        }
        public override string TownNPCName()
        {
            return "???";
        }


        List<string> dialogue = new List<string>();

        bool doingDialogue = false;
        int currentOption1 = 0;
        const int optionCount = 3;
        int loreText = 0;
        int loreTextCount = 5;
        bool showingLore = false;

        public override void SetChatButtons(ref string button, ref string button2)
        {
            if (doingDialogue)
            {
                button = Language.GetTextValue("Next");
            }
            else
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
                        button = Language.GetTextValue("What did you do before you got here?");
                    }
                    if (loreText == 2)
                    {
                        button = Language.GetTextValue("Are there other civilizations out there?");
                    }
                    if (loreText == 3)
                    {
                        button = Language.GetTextValue("How many other places have you been to?");
                    }
                    if (loreText == 4)
                    {
                        button = Language.GetTextValue("Who built the shrines?");
                    }
                    return;
                }
                if (currentOption1 == 0)
                {
                    button = Language.GetTextValue("Shop");
                }
                else if (currentOption1 == 1)
                {
                    button = Language.GetTextValue("Where can I find the next shrine?");
                }
                else if (currentOption1 == 2)
                {
                    button = Language.GetTextValue("Talk");
                }
                button2 = Language.GetTextValue("Cycle Options");
            }
        }
        public override void OnChatButtonClicked(bool firstButton, ref bool openShop)
        {
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);
            if (doingDialogue)
            {
                Main.npcChatText = dialogue[0];
                dialogue.RemoveAt(0);
                if (dialogue.Count <= 0)
                {
                    doingDialogue = false;
                    TerrorbornWorld.TerrorMasterDialogue++;
                }
                if (TerrorbornWorld.TerrorMasterDialogue == 5)
                {
                    player.QuickSpawnItem(ModContent.ItemType<Items.Lore.fourthWallBreakInReal>());
                }
            }
            else
            {
                if (firstButton)
                {
                    if (showingLore)
                    {
                        if (loreText == 0) //Nevermind
                        {
                            Main.npcChatText = "Ok then idiot. How may I be of service to you?";
                            showingLore = false;
                        }
                        if (loreText == 1) //"What did you do before you got here?
                        {
                            Main.npcChatText = "I have a... long past. Truthfully, I kind of just wandered for a while before I found you here. The world is quite dead right now, at least compared to what it used to be, and there isn't a whole lot to help out with. I'm kind of glad I found a place to rest and help a fellow incarnate out. It's nice.";
                        }
                        if (loreText == 2) //Are there other civilizations out there right now?
                        {
                            Main.npcChatText = "Depends on your requirements. There used to be a ton, but most of them have either become extinct or have been taken over by another... dangerous force. Not that evil infection out that way- something much worse actually. Bottom line is that there *was* stuff out there, but the stuff all got annihilated. Still great historic sites, though.";
                        }
                        if (loreText == 3) //How many other places have you been to?
                        {
                            Main.npcChatText = "A lot. I used to work for a kingdom, but then that kingdom went down the tubes. So I went to another kingdom and now that kingdom is gone too. Everywhere I go there seems to be some foolish ruler who messes everything up for themself, or for everybody else. And that's why I've decided to stay here- to help make sure you don't do the same.";
                        }
                        if (loreText == 4) //Who built the shrines?
                        {
                            Main.npcChatText = "Uh... I'm not sure. I was alive before they were built- in a time where, as an incarnate, it took so much more training to master your potential. Now that those are there you can walk up to a glowing white thing and poof, you know how to do stuff now. Whoever made them must have been an expert, I must say.";
                        }
                        return;
                    }
                    if (currentOption1 == 0) //Shop
                    {
                        openShop = true;
                    }
                    else if (currentOption1 == 1) //Where is the next shrine
                    {
                        if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(new Abilities.NecromanticCurseInfo())))
                        {
                            Main.npcChatText = "The first ability you will be looking for is not actually located at a terror shrine, but rather the entrance of another structure. If I recall correctly, it should be to the same side of the island as the tundra, and on the surface, which is why this should be the first one you get- it's the easiest to find.";
                        }
                        else if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(new Abilities.HorrificAdaptationInfo())))
                        {
                            Main.npcChatText = "The next one is going to be significantly harder to find than the first one, since it's buried somewhere underground around the same area as the jungle. I don't remember if it's exactly in the jungle, but I do know it's somewhere in that area. Some dark pearls would be highly helpful when searching for it.";
                        }
                        else if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(new Abilities.VoidBlinkInfo())))
                        {
                            Main.npcChatText = "Still looking for more, eh? Well, I'm not surprised. Anyways, this one is about as deep as it gets; it's under the island, buried in the ashes of the underworld. I don't have an exact horizontal position for it, so you'll just have to search for it down there manually, or with the help of some trusty dark pearls.";
                        }
                        else if (!modPlayer.unlockedAbilities.Contains(TerrorbornUtils.abilityToInt(new Abilities.TerrorWarpInfo())))
                        {
                            Main.npcChatText = "This one is less dangerous to find, but highly protected and very well hidden. I have no bearing for its location, all I know is that it's somewhere underground. Unless you're highly lucky, you'll practically be needing dark pearls to find it. The person who created the shrines thought the chaotic nature of this ability was highly dangerous, and thus protected the entrance to it with titanium ore. As such, you'll need a good pickaxe to get in.";
                        }
                        else
                        {
                            Main.npcChatText = "You've discovered pretty much all of the shrines I know about. Congratulations! Just keep... doing your thing, I supppose, and I'm sure you'll find more soon enough.";
                        }
                    }
                    else if (currentOption1 == 2) //Talk
                    {
                        showingLore = true;
                        Main.npcChatText = "What is it then?";
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
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = ModContent.GetTexture("TerrorbornMod/ExclamationPoint");
            Vector2 position = npc.Center - new Vector2(0, 65);
            if (doingDialogue)
            {
                spriteBatch.Draw(texture, position: position - Main.screenPosition, new Rectangle(0, 0, texture.Width, texture.Height), Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }

        public override void PostAI()
        {
            base.PostAI();
            int dialogueState = TerrorbornWorld.TerrorMasterDialogue; //For ease of access
            Player player = Main.player[Main.myPlayer];
            TerrorbornPlayer modPlayer = TerrorbornPlayer.modPlayer(player);

            if (player.Distance(npc.Center) > 300)
            {
                showingLore = false;
                loreText = 0;
                currentOption1 = 0;
                if (dialogueState == 0)
                {
                    doingDialogue = true;
                }
                if (dialogueState == 1 && modPlayer.unlockedAbilities.Count >= 2)
                {
                    doingDialogue = true;
                }
                if (dialogueState == 2 && Main.hardMode)
                {
                    doingDialogue = true;
                }
                if (dialogueState == 3 && NPC.downedPlantBoss)
                {
                    doingDialogue = true;
                }
                if (dialogueState == 4 && NPC.downedMoonlord)
                {
                    doingDialogue = true;
                }
            }

        }

        public override string GetChat()
        {
            int dialogueState = TerrorbornWorld.TerrorMasterDialogue; //For ease of access
            string shownDialogue = "I'm bugged :D";
            if (doingDialogue)
            {
                if (dialogueState == 0)
                {
                    dialogue.Clear();
                    shownDialogue = "Hello there, you must be a fellow seeker of this... anomaly I've detected in this area. My spells have picked up very large amounts of terror emanating nearby.";
                    dialogue.Add("Wait... do you not know what terror is? That's... pathetic, really. I think I'll start nicknaming you idiot, hah!");
                    dialogue.Add("I'm only joking... considering you're the one who seems to live here I wouldn't want to insult you like that. But anyways, when I refer to terror, I mean a... substance.");
                    dialogue.Add("And a very special one. You see, terror is what grants us our instincts and consiousness. Our minds and souls are composed entirely of its essence. And certain beings, such as myself, are capable of taking advatage of it.");
                    dialogue.Add("These beings are called 'incarnates'. Although... there's something very peculiar about you in particular- I believe you yourself are an incarnate as well, and you seem to have stumbled upon a rather convenient spot to call home.");
                    dialogue.Add("You see, all over this island is a group of shrines. Terror shrines more specifically, that can be used to enhance your ability to use terror. And luckily for you, I happen to know where they all are.");
                    dialogue.Add("So, what do you think about this... in return for a home, I'll help guide you to these shrines and help train your abilities. Does that sound good? Yes? Very good, idiot. Now, feel free to ask when you're ready to go exploring for them.");
                }
                if (dialogueState == 1)
                {
                    dialogue.Clear();
                    shownDialogue = "Hello again! It seems you've been interested in enhancing your abilities; two terror spells have already been learned by you.";
                    dialogue.Add("Something to note is that there isn't a shrine for every single spell in existence. Some of them have to be extracted by slaying large foes, as I'm sure you might have guessed by now.");
                    dialogue.Add("Eventually, there will be certain abilities I can't help you get, however, I'll always be here to help you enhance them further.");
                }
                if (dialogueState == 2)
                {
                    dialogue.Clear();
                    shownDialogue = "Good, you're here. Just recently, I felt a great surge of terror, one that could only mean one thing- the seal has been broken.";
                    dialogue.Add("This seal was originally created as a sort of vault, by the ruler of a kingdom. This ruler was Navaylos, and this 'vault' was meant to store souls for future use. In destroying it now, all of its energy has been released into our world.");
                    dialogue.Add("This means plenty of things, and given that I know you're the one who must have destroyed it, that includes having both wanted and unwanted attention focused on yourself. As for what it means regarding Navaylos... I'm not sure.");
                    dialogue.Add("What I do know is that you aught to be careful now. I'm sure plenty of monsters have already taken advantage of this event and taken some of the energy for themselves.");
                    dialogue.Add("With all that out of the way, I just want to warn you- while your rapid empowerment may seem harmless right now, I've seen people do crazy things because of similar circumstances. If you were to... say... go mad, someone would have to stop you...");
                    dialogue.Add("...and I'm sure you don't want that. And neither do I, as there is potential here. Just be aware of your own motivations, okay?");
                }
                if (dialogueState == 3)
                {
                    dialogue.Clear();
                    shownDialogue = "You've made quite a lot of progress towards... whatever you're trying to achieve, recently, and I just wanted to congradulate you.";
                    dialogue.Add("Surviving and getting this powerful from practically nothing is a very rare and very lucky accomplishment. And I'm surprised no body has noticed you yet.");
                    dialogue.Add("That being said, you might not be as safe as you think, and I'd just like to warn you that there has been some strange presence recently- one highly equipped with terror.");
                    dialogue.Add("Somebody is watching us, but not directly. I'm not sure what they're waiting for, but I imagine they'll be attacking in the future. That's all I have to say... for now, at least.");
                }
                if (dialogueState == 4)
                {
                    dialogue.Clear();
                    shownDialogue = "Hello! That performance of yours... it's bound to get some attention. And honestly, [c/FF1919:there are people who most certainly should have tampered with us by now.]";
                    dialogue.Add("I want to help you, but now is not the time. All we can do is wait, I suppose, and see what happens next.");
                    dialogue.Add("By the way, I have a mysterious message for you. I don't know where it came from, but here....");
                }
            }
            else
            {
                WeightedRandom<string> chat = new WeightedRandom<string>();
                chat.Add("Why hello there, idiot of the island! How can I help you?");
                chat.Add("Ever heard of a terroranosaurus-rex? No? Good, because they don't exist.");
                chat.Add("I'm surprised you can go so long without dying. Having started out as weak as you did, you'd think you'd have died pretty quickly. This isn't some sort of game after all- this is real life!");
                chat.Add("How much wood could a wood chuck chuck if a wood chuck could chuck wood? Yeah, probably around 4,269 pieces before it got tired. Those things are really energetic.");
                chat.Add("Wanna hear a riddle? Alright, here goes: why did the chicken cross the platform? No, it's because it felt like it. And then it got hit by a meteor and died. The end.");
                chat.Add("Knock knock. An idiot. You.");
                chat.Add("I have an indescribable need to eat a taco right now. Got any bells for me? I don't know why, but I feel like a bell would be fun to play with while eating my taco. Don't judge me.");
                shownDialogue = chat;
            }
            return shownDialogue;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 5;
            knockback = 7f;
            if (Main.hardMode)
            {
                damage = 20;
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
            projType = ProjectileID.DiamondBolt;
            attackDelay = 1;
        }
        public override void TownNPCAttackShoot(ref bool inBetweenShots)
        {
            inBetweenShots = false;
        }
        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 14f;
            gravityCorrection = 0;
        }

        public override void SetupShop(Chest shop, ref int nextSlot)
        {
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Potions.DarkbloodPotion>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 3, 50, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.DarkQuill>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 10, 0, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.Equipable.Accessories.IntimidationAura>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 20, 0, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.PearlOfDarkness>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 0, 35, 0);
            nextSlot++;
            shop.item[nextSlot].SetDefaults(ModContent.ItemType<Items.MiscConsumables.TerrorTaco>());
            shop.item[nextSlot].shopCustomPrice = Item.buyPrice(0, 5, 0, 0);
            nextSlot++;
        }
    }
}

