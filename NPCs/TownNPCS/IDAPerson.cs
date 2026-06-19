////using ExampleMod.Content.Biomes;
////using ExampleMod.Content.Dusts;
////using ExampleMod.Content.Items;
////using ExampleMod.Content.Items.Accessories;
////using ExampleMod.Content.Items.Armor;
////using ExampleMod.Content.Tiles;
////using ExampleMod.Content.Tiles.Furniture;
////using ExampleMod.Content.Walls;
////using ExampleMod.Common.Configs;
////using ExampleMod.Common;
////using ExampleMod.Content.Projectiles;

//using Microsoft.Xna.Framework;
//using System;
//using System.Linq;
//using Terraria;
//using Terraria.Audio;
//using Terraria.ID;
//using Terraria.Localization;
//using Terraria.ModLoader;
//using Terraria.Utilities;
//using Terraria.GameContent.Bestiary;
//using Terraria.GameContent.ItemDropRules;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria.GameContent;
//using Terraria.GameContent.Personalities;
//using System.Collections.Generic;
//using Terraria.ModLoader.IO;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.GameContent.Bestiary;
//using Terraria.GameContent.Personalities;
//using Terraria.GameContent;
//using Terraria.ID;
//using Terraria.ModLoader.IO;
//using Terraria.ModLoader;
//using Terraria.Utilities;

//using AccessoryForPlanetaryEmpire.Accessory.Attack;
//using AccessoryForPlanetaryEmpire.Accessory.Defense;
//using AccessoryForPlanetaryEmpire.Accessory.Movement;
//using AccessoryForPlanetaryEmpire.Accessory.JobAcc;
//using AccessoryForPlanetaryEmpire.Accessory.Others;
//using AccessoryForPlanetaryEmpire.Accessory.Y_TestAcc;
//using ReLogic.Content;


//// 帝国防卫自动机1号
//namespace AccessoryForPlanetaryEmpire.NPCs.TownNPCS
//{
//    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
//    [AutoloadHead]
//    public class IDAPerson : ModNPC
//    {

//        public List<Item> shopItems = new List<Item>();
//        public void CreateNewShop()
//        {
//            // create a list of item ids
//            var itemIds = new List<int>();


//            //switch (Main.rand.Next(2))
//            //{
//            //    case 0:
//            //        itemIds.Add(ModContent.ItemType<GravityBooster>());
//            //        break;


//            //}
//            // For each slot we add a switch case to determine what should go in that slot


//            //// 游戏初期：
//            //itemIds.Add(ModContent.ItemType<GravityBooster>());
//            //itemIds.Add(ModContent.ItemType<GravityReducer>());
//            //itemIds.Add(ModContent.ItemType<HighVoltageGrid>());
//            //itemIds.Add(ModContent.ItemType<MagicSeeker>());
//            //itemIds.Add(ModContent.ItemType<PeriodicBarrierBooster>());
//            //itemIds.Add(ModContent.ItemType<PeriodicBarrierBoosterHAV>());
//            //itemIds.Add(ModContent.ItemType<WoDeShengGao>());

//            //// 世吞克脑后
//            //if (NPC.downedBoss2)
//            //{
//            //    itemIds.Add(ModContent.ItemType<SpeedModActive>());
//            //    itemIds.Add(ModContent.ItemType<ReactShieldGenerator>());
//            //    itemIds.Add(ModContent.ItemType<ReflectArcPulse>());
//            //    itemIds.Add(ModContent.ItemType<ReflectArcPulseHAV>());
//            //}

//            //// 骷髅王后
//            //if (NPC.downedBoss3)
//            //{
//            //    //itemIds.Add(ModContent.ItemType<Tank1>());
//            //    //itemIds.Add(ModContent.ItemType<Ranger1>());
//            //    //itemIds.Add(ModContent.ItemType<Magic1>());
//            //    //itemIds.Add(ModContent.ItemType<Summon1>());
//            //    itemIds.Add(ModContent.ItemType<AIAssistDevice>());
//            //    itemIds.Add(ModContent.ItemType<FixedPointAttackDevice>());
//            //    itemIds.Add(ModContent.ItemType<HeroDamageDevice>());
//            //    itemIds.Add(ModContent.ItemType<FixedPointDefenseDevice>());
//            //    itemIds.Add(ModContent.ItemType<HighVoltageGrid2>());
//            //}

//            //// 困难模式早期
//            //if (Main.hardMode)
//            //{
//            //    //itemIds.Add(ModContent.ItemType<Tank2>());
//            //    //itemIds.Add(ModContent.ItemType<Ranger2>());
//            //    //itemIds.Add(ModContent.ItemType<Magic2>());
//            //    //itemIds.Add(ModContent.ItemType<Summon2>());
//            //    itemIds.Add(ModContent.ItemType<Teleportation>());
//            //    itemIds.Add(ModContent.ItemType<ImperialDashShield>());
//            //    itemIds.Add(ModContent.ItemType<UltimateFunctionOverloadDevice>());
//            //    itemIds.Add(ModContent.ItemType<Summon2>());
//            //    itemIds.Add(ModContent.ItemType<RangeBarrier>());
//            //}

//            //// 世纪之花后
//            //if (NPC.downedPlantBoss)
//            //{
//            //    //itemIds.Add(ModContent.ItemType<Tank3>());
//            //    //itemIds.Add(ModContent.ItemType<Ranger3>());
//            //    //itemIds.Add(ModContent.ItemType<Magic3>());
//            //    //itemIds.Add(ModContent.ItemType<Summon3>());
//            //    itemIds.Add(ModContent.ItemType<HighVoltageGrid3>());
//            //}

//            //// 石巨人后
//            //if (NPC.downedGolemBoss)
//            //{
//            //    itemIds.Add(ModContent.ItemType<UltraHighCrit>());
//            //    itemIds.Add(ModContent.ItemType<LiquidAbsorbentShield>());
//            //    itemIds.Add(ModContent.ItemType<ImperialDashShieldSJR>());
//            //    itemIds.Add(ModContent.ItemType<HighVoltageGrid4>());
//            //}

//            //// 月亮领主后
//            //if (NPC.downedMoonlord)
//            //{
//            //    itemIds.Add(ModContent.ItemType<Tank4>());
//            //    itemIds.Add(ModContent.ItemType<Ranger4>());
//            //    itemIds.Add(ModContent.ItemType<Magic4>());
//            //    itemIds.Add(ModContent.ItemType<Summon4>());
//            //}



//            // convert to a list of items
//            shopItems = new List<Item>();
//            foreach (int itemId in itemIds)
//            {
//                Item item = new Item();
//                item.SetDefaults(itemId);
//                shopItems.Add(item);
//            }
//        }


//        //Not completely finished, but below is what the NPC will sell


//        // 新版本禁用，以后再说
//        //public override void SetupShop(Chest shop, ref int nextSlot)
//        //{
//        //    // shop.item[nextSlot].SetDefaults(ItemType<EquipMaterial>());
//        //    // nextSlot++;
//        //    // shop.item[nextSlot].SetDefaults(ItemType<BossItem>());
//        //    // nextSlot++;

//        //    // 游戏初期：
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<GravityBooster>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<GravityReducer>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<HighVoltageGrid>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<MagicSeeker>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<PeriodicBarrierBooster>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<PeriodicBarrierBoosterHAV>());
//        //    shop.item[nextSlot++].SetDefaults(ModContent.ItemType<WoDeShengGao>());

//        //    // 世吞克脑后
//        //    if (NPC.downedBoss2)
//        //    {
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<SpeedModActive>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ReactShieldGenerator>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ReflectArcPulse>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ReflectArcPulseHAV>());
//        //    }

//        //    // 骷髅王后
//        //    if (NPC.downedBoss3)
//        //    {
//        //        //itemIds.Add(ModContent.ItemType<Tank1>());
//        //        //itemIds.Add(ModContent.ItemType<Ranger1>());
//        //        //itemIds.Add(ModContent.ItemType<Magic1>());
//        //        //itemIds.Add(ModContent.ItemType<Summon1>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<AIAssistDevice>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<FixedPointAttackDevice>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<HeroDamageDevice>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<FixedPointDefenseDevice>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<HighVoltageGrid2>());
//        //    }

//        //    // 困难模式早期
//        //    if (Main.hardMode)
//        //    {
//        //        //itemIds.Add(ModContent.ItemType<Tank2>());
//        //        //itemIds.Add(ModContent.ItemType<Ranger2>());
//        //        //itemIds.Add(ModContent.ItemType<Magic2>());
//        //        //itemIds.Add(ModContent.ItemType<Summon2>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Teleportation>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ImperialDashShield>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<UltimateFunctionOverloadDevice>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<RangeBarrier>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<StunEMP>());
//        //    }

//        //    // 世纪之花后
//        //    if (NPC.downedPlantBoss)
//        //    {
//        //        //itemIds.Add(ModContent.ItemType<Tank3>());
//        //        //itemIds.Add(ModContent.ItemType<Ranger3>());
//        //        //itemIds.Add(ModContent.ItemType<Magic3>());
//        //        //itemIds.Add(ModContent.ItemType<Summon3>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<HighVoltageGrid3>());
//        //    }

//        //    // 石巨人后
//        //    if (NPC.downedGolemBoss)
//        //    {
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<UltraHighCrit>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<LiquidAbsorbentShield>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<ImperialDashShieldSJR>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<HighVoltageGrid4>());
//        //    }

//        //    // 月亮领主后
//        //    if (NPC.downedMoonlord)
//        //    {
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Tank4>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Ranger4>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Magic4>());
//        //        shop.item[nextSlot++].SetDefaults(ModContent.ItemType<Summon4>());
//        //    }


//        //    // 当有特定buff的时候售卖什么
//        //    //if (Main.LocalPlayer.HasBuff(BuffID.Lifeforce))
//        //    //{
//        //    //    shop.item[nextSlot++].SetDefaults(ItemType<ExampleHealingPotion>());
//        //    //}

//        //    // if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>().ZoneExample && !GetInstance<ExampleConfigServer>().DisableExampleWings) {
//        //    // 	shop.item[nextSlot].SetDefaults(ItemType<ExampleWings>());
//        //    // 	nextSlot++;
//        //    // }

//        //    // 当有特定月亮的时候售卖什么
//        //    //if (Main.moonPhase < 2)
//        //    //{
//        //    //    shop.item[nextSlot++].SetDefaults(ItemType<ExampleSword>());
//        //    //}
//        //    //else if (Main.moonPhase < 4)
//        //    //{
//        //    //    // shop.item[nextSlot++].SetDefaults(ItemType<ExampleGun>());
//        //    //    shop.item[nextSlot].SetDefaults(ItemType<ExampleBullet>());
//        //    //}
//        //    //else if (Main.moonPhase < 6)
//        //    //{
//        //    //    // shop.item[nextSlot++].SetDefaults(ItemType<ExampleStaff>());
//        //    //}

//        //    // todo: Here is an example of how your npc can sell items from other mods.
//        //    // var modSummonersAssociation = ModLoader.TryGetMod("SummonersAssociation");
//        //    // if (ModLoader.TryGetMod("SummonersAssociation", out Mod modSummonersAssociation)) {
//        //    // 	shop.item[nextSlot].SetDefaults(modSummonersAssociation.ItemType("BloodTalisman"));
//        //    // 	nextSlot++;
//        //    // }

//        //    // if (!Main.LocalPlayer.GetModPlayer<ExamplePlayer>().examplePersonGiftReceived && GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList != null) {
//        //    // 	foreach (var item in GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList) {
//        //    // 		if (Item.IsUnloaded) continue;
//        //    // 		shop.item[nextSlot].SetDefaults(Item.Type);
//        //    // 		shop.item[nextSlot].shopCustomPrice = 0;
//        //    // 		shop.item[nextSlot].GetGlobalItem<ExampleInstancedGlobalItem>().examplePersonFreeGift = true;
//        //    // 		nextSlot++;
//        //    // 		//TODO: Have tModLoader handle index issues.
//        //    // 	}
//        //    // }
//        //}





//        // 新版本禁用，以后再说
//        //public override bool CanTownNPCSpawn(int numTownNPCs, int money)
//        //{ // Requirements for the town NPC to spawn.
//        //    for (int k = 0; k < 255; k++)
//        //    {
//        //        Player player = Main.player[k];
//        //        if (!player.active)
//        //        {
//        //            continue;
//        //        }

//        //        // Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
//        //        if (true)
//        //        {
//        //            return true;
//        //        }
//        //    }

//        //    return false;
//        //}

//        public override void SaveData(TagCompound tag)
//        {
//            tag["itemIds"] = shopItems;
//        }

//        public override void LoadData(TagCompound tag)
//        {
//            shopItems = tag.Get<List<Item>>("shopItems");
//        }


//        //public override void SetupShop(Chest shop, ref int nextSlot)
//        //{
//        //    foreach (Item item in shopItems)
//        //    {
//        //        // We don't want "empty" items and unloaded items to appear
//        //        if (item == null || item.type == ItemID.None)
//        //            continue;

//        //        shop.item[nextSlot].SetDefaults(item.type);
//        //        nextSlot++;
//        //    }
//        //}

//        public int NumberOfTimesTalkedTo = 0;

//        public override void SetStaticDefaults()
//        {
//            // DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
//            // DisplayName.SetDefault("Example Person");
//            Main.npcFrameCount[Type] = 25; // The amount of frames the NPC has

//            NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
//            NPCID.Sets.AttackFrameCount[Type] = 4;
//            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
//            NPCID.Sets.AttackType[Type] = 0;
//            NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
//            NPCID.Sets.AttackAverageChance[Type] = 30;
//            NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

//            // Influences how the NPC looks in the Bestiary
//            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
//            {
//                Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
//                Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
//                              // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
//                              // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
//            };

//            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

//            // Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
//            // NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
//            NPC.Happiness
//                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like) // Example Person prefers the forest.
//                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow.
//                                                                      //.SetBiomeAffection<ExampleSurfaceBiome>(AffectionLevel.Love) // Example Person likes the Example Surface Biome
//                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love) // Loves living near the dryad.
//                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like) // Likes living near the guide.
//                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike) // Dislikes living near the merchant.
//                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
//            ; // < Mind the semicolon!
//        }






//        //public override void SetupShop(Chest shop, ref int nextSlot)
//        //{
//        //    foreach (Item item in shopItems)
//        //    {
//        //        // We dont want "empty" items and unloaded items to appear
//        //        if (item == null || item.type == ItemID.None)
//        //            continue;

//        //        shop.item[nextSlot].SetDefaults(item.type);
//        //        nextSlot++;
//        //    }
//        //}



//        public override void SetDefaults()
//        {
//            NPC.townNPC = true; // Sets NPC to be a Town NPC
//            NPC.friendly = true; // NPC Will not attack player
//            NPC.width = 18;
//            NPC.height = 40;
//            NPC.aiStyle = 7;
//            NPC.damage = 10;
//            NPC.defense = 15;
//            NPC.lifeMax = 250;
//            NPC.HitSound = SoundID.NPCHit1;
//            NPC.DeathSound = SoundID.NPCDeath1;
//            NPC.knockBackResist = 0.5f;
//            TownNPCStayingHomeless = true;
//            AnimationType = NPCID.Guide;
//            CreateNewShop();

//        }


//        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
//        {
//            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
//            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
//				// Sets the preferred biomes of this town NPC listed in the bestiary.
//				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
//				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

//				// Sets your NPC's flavor text in the bestiary.
//				new FlavorTextBestiaryInfoElement("Hailing from a mysterious greyscale cube world, the Example Person is here to help you understand everything about tModLoader."),

//				// You can add multiple elements if you really wanted to
//				// You can also use localization keys (see Localization/en-US.lang)
//				new FlavorTextBestiaryInfoElement("Mods.ExampleMod.Bestiary.ExamplePerson")
//                });
//        }

//        // The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
//        // Returning false will allow you to manually draw your NPC
//        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
//        {
//            // This code slowly rotates the NPC in the bestiary
//            // (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
//            if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
//            {
//                drawModifiers.Rotation += 0.001f;

//                // Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
//                NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
//                NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
//            }

//            return true;
//        }


//        // 新版本禁用，以后再说
//        // public override void HitEffect(int hitDirection, double damage)
//        //{
//        //    int num = NPC.life > 0 ? 1 : 5;

//        //    for (int k = 0; k < num; k++)
//        //    {
//        //        //Dust.NewDust(NPC.position, NPC.width, NPC.height, ModContent.DustType<Sparkle>());
//        //    }
//        //}


//        // Example Person needs a house built out of ExampleMod tiles. You can delete this whole method in your townNPC for the regular house conditions.
//        public override bool CheckConditions(int left, int right, int top, int bottom)
//        {
//            int score = 0;
//            for (int x = left; x <= right; x++)
//            {
//                for (int y = top; y <= bottom; y++)
//                {
//                    int type = Main.tile[x, y].TileType;
//                    score++;
//                    //if (type == ModContent.TileType<ExampleBlock>() || type == ModContent.TileType<ExampleChair>() || type == ModContent.TileType<ExampleWorkbench>() || type == ModContent.TileType<ExampleBed>() || type == ModContent.TileType<ExampleDoorOpen>() || type == ModContent.TileType<ExampleDoorClosed>())
//                    //{
//                    //    score++;
//                    //}

//                    //if (Main.tile[x, y].WallType == ModContent.WallType<ExampleWall>())
//                    //{
//                    //    score++;
//                    //}
//                }
//            }

//            return score >= (right - left) * (bottom - top) / 2;
//        }

//        //public override ITownNPCProfile TownNPCProfile()
//        //{
//        //    return new ExamplePersonProfile();
//        //}

//        public override List<string> SetNPCNameList()
//        {
//            return new List<string>()
//                {
//                "ADT",
//                "AEXTD"
//                };
//        }

//        public override void FindFrame(int frameHeight)
//        {
//            /*npc.frame.Width = 40;
//            if (((int)Main.time / 10) % 2 == 0)
//            {
//                npc.frame.X = 40;
//            }
//            else
//            {
//                npc.frame.X = 0;
//            }*/
//        }

//        public override string GetChat()
//        {
//            WeightedRandom<string> chat = new WeightedRandom<string>();

//            //int partyGirl = NPC.FindFirstNPC(NPCID.PartyGirl);
//            //if (partyGirl >= 0 && Main.rand.NextBool(4))
//            //{
//            //    chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.PartyGirlDialogue", Main.npc[partyGirl].GivenName));
//            //}
//            // These are things that the NPC has a chance of telling you when you talk to it.
//            //chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.StandardDialogue1"));
//            //chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.StandardDialogue2"));
//            //chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.StandardDialogue3"));
//            //chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.CommonDialogue"), 5.0);
//            //chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.RareDialogue"), 0.1);

//            chat.Add(Language.GetTextValue("旧行星帝国的毁灭源自于一场声势浩大的战斗，我不知道这场战斗的细节，我只知道只有我和其他几个战争人形从战争中逃脱"));
//            chat.Add(Language.GetTextValue("请不要摸我的头，那里是我的天线感应部位，谢谢"));
//            chat.Add(Language.GetTextValue("希望这些装备有帮到你的战斗"));
//            chat.Add(Language.GetTextValue("银铃胸甲五金一件啊，抱歉，走错片场"));
//            chat.Add(Language.GetTextValue("好想回家..."));
//            chat.Add(Language.GetTextValue("不好意思，我不习惯卸下我的武器系统，希望没有伤到您"));
//            chat.Add(Language.GetTextValue("不，我不是人，我是战争人形，是旧帝国制造出来的相当于他们100倍大小的人形兵器。什么？你问我为什么我跟你一样大小，我怎么知道？"));
//            chat.Add(Language.GetTextValue("我记得当初我的飞船坠毁的时候，我还有一个存活的同伴，不知道你有没有找到她，我希望她平安无事"));
//            chat.Add(Language.GetTextValue("那三台巨大的机械？它们似乎有点眼熟，只不过我说不出来它们究竟来自哪里。但他们的外部构造似乎跟当时毁灭旧行星帝国的军队有异曲同工之妙"));
//            chat.Add(Language.GetTextValue("请不要随意摆弄我背部的飞行翼，它们很脆弱"));


//            //chat.Add(Language.GetTextValue("你问我神射手是什么职业？那是旧行星帝国曾经拥有的一项极高的荣誉，专门颁发给那些拥有出色远程打击能力的射手们"));
//            //chat.Add(Language.GetTextValue("你问我凤凰是什么职业？那是旧行星帝国最为神秘的特种作战部门，他们拥有着难以想象的单体作战能力，但似乎在实战中充满了弱点，只要被敌人碰到一下就会死"));


//            NumberOfTimesTalkedTo++;
//            if (NumberOfTimesTalkedTo >= 10)
//            {
//                //This counter is linked to a single instance of the NPC, so if ExamplePerson is killed, the counter will reset.
//                chat.Add(Language.GetTextValue("Mods.ExampleMod.Dialogue.ExamplePerson.TalkALot"));
//            }

//            return chat; // chat is implicitly cast to a string.
//        }

//        public override void SetChatButtons(ref string button, ref string button2)
//        {
//            // What the chat buttons are when you open up the chat UI
//            button = Language.GetTextValue("LegacyInterface.28");
//            button2 = "Awesomeify";
//            if (Main.LocalPlayer.HasItem(ItemID.HiveBackpack))
//            {
//                button = "Upgrade " + Lang.GetItemNameValue(ItemID.HiveBackpack);
//            }
//        }

//        // 新版本禁用，以后再说
//        //public override void OnChatButtonClicked(bool firstButton, ref bool shop)
//        //{
//        //    if (firstButton)
//        //    {
//        //        // We want 3 different functionalities for chat buttons, so we use HasItem to change button 1 between a shop and upgrade action.

//        //        if (Main.LocalPlayer.HasItem(ItemID.HiveBackpack))
//        //        {
//        //            SoundEngine.PlaySound(SoundID.Item37); // Reforge/Anvil sound

//        //            //Main.npcChatText = $"I upgraded your {Lang.GetItemNameValue(ItemID.HiveBackpack)} to a {Lang.GetItemNameValue(ModContent.ItemType<WaspNest>())}";

//        //            int hiveBackpackItemIndex = Main.LocalPlayer.FindItem(ItemID.HiveBackpack);
//        //            var entitySource = NPC.GetSource_GiftOrReward();

//        //            Main.LocalPlayer.inventory[hiveBackpackItemIndex].TurnToAir();
//        //            //Main.LocalPlayer.QuickSpawnItem(entitySource, ModContent.ItemType<WaspNest>());

//        //            return;
//        //        }

//        //        shop = true;
//        //    }
//        //}

//        // Not completely finished, but below is what the NPC will sell

//        // public override void SetupShop(Chest shop, ref int nextSlot) {
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<ExampleItem>());
//        // 	// shop.item[nextSlot].SetDefaults(ItemType<EquipMaterial>());
//        // 	// nextSlot++;
//        // 	// shop.item[nextSlot].SetDefaults(ItemType<BossItem>());
//        // 	// nextSlot++;
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleWorkbench>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleChair>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleDoor>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleBed>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<Items.Placeable.Furniture.ExampleChest>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<ExamplePickaxe>());
//        // 	shop.item[nextSlot++].SetDefaults(ItemType<ExampleHamaxe>());
//        //
//        // 	if (Main.LocalPlayer.HasBuff(BuffID.Lifeforce)) {
//        // 		shop.item[nextSlot++].SetDefaults(ItemType<ExampleHealingPotion>());
//        // 	}
//        //
//        // 	// if (Main.LocalPlayer.GetModPlayer<ExamplePlayer>().ZoneExample && !GetInstance<ExampleConfigServer>().DisableExampleWings) {
//        // 	// 	shop.item[nextSlot].SetDefaults(ItemType<ExampleWings>());
//        // 	// 	nextSlot++;
//        // 	// }
//        //
//        // 	if (Main.moonPhase < 2) {
//        // 		shop.item[nextSlot++].SetDefaults(ItemType<ExampleSword>());
//        // 	}
//        // 	else if (Main.moonPhase < 4) {
//        // 		// shop.item[nextSlot++].SetDefaults(ItemType<ExampleGun>());
//        // 		shop.item[nextSlot].SetDefaults(ItemType<ExampleBullet>());
//        // 	}
//        // 	else if (Main.moonPhase < 6) {
//        // 		// shop.item[nextSlot++].SetDefaults(ItemType<ExampleStaff>());
//        // 	}
//        //
//        // 	// todo: Here is an example of how your npc can sell items from other mods.
//        // 	// var modSummonersAssociation = ModLoader.TryGetMod("SummonersAssociation");
//        // 	// if (ModLoader.TryGetMod("SummonersAssociation", out Mod modSummonersAssociation)) {
//        // 	// 	shop.item[nextSlot].SetDefaults(modSummonersAssociation.ItemType("BloodTalisman"));
//        // 	// 	nextSlot++;
//        // 	// }
//        //
//        // 	// if (!Main.LocalPlayer.GetModPlayer<ExamplePlayer>().examplePersonGiftReceived && GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList != null) {
//        // 	// 	foreach (var item in GetInstance<ExampleConfigServer>().ExamplePersonFreeGiftList) {
//        // 	// 		if (Item.IsUnloaded) continue;
//        // 	// 		shop.item[nextSlot].SetDefaults(Item.Type);
//        // 	// 		shop.item[nextSlot].shopCustomPrice = 0;
//        // 	// 		shop.item[nextSlot].GetGlobalItem<ExampleInstancedGlobalItem>().examplePersonFreeGift = true;
//        // 	// 		nextSlot++;
//        // 	// 		//TODO: Have tModLoader handle index issues.
//        // 	// 	}
//        // 	// }
//        // }

//        public override void ModifyNPCLoot(NPCLoot npcLoot)
//        {
//            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ExampleCostume>()));
//        }

//        // Make this Town NPC teleport to the King and/or Queen statue when triggered.
//        public override bool CanGoToStatue(bool toKingStatue) => true;

//        // Make something happen when the npc teleports to a statue. Since this method only runs server side, any visual effects like dusts or gores have to be synced across all clients manually.
//        public override void OnGoToStatue(bool toKingStatue)
//        {
//            if (Main.netMode == NetmodeID.Server)
//            {
//                ModPacket packet = Mod.GetPacket();
//                //packet.Write((byte)ExampleMod.MessageType.ExampleTeleportToStatue);
//                packet.Write((byte)NPC.whoAmI);
//                packet.Send();
//            }
//            else
//            {
//                StatueTeleport();
//            }
//        }

//        // Create a square of pixels around the NPC on teleport.
//        public void StatueTeleport()
//        {
//            for (int i = 0; i < 30; i++)
//            {
//                Vector2 position = Main.rand.NextVector2Square(-20, 21);
//                if (Math.Abs(position.X) > Math.Abs(position.Y))
//                {
//                    position.X = Math.Sign(position.X) * 20;
//                }
//                else
//                {
//                    position.Y = Math.Sign(position.Y) * 20;
//                }

//                //Dust.NewDustPerfect(NPC.Center + position, ModContent.DustType<Sparkle>(), Vector2.Zero).noGravity = true;
//            }
//        }

//        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
//        {
//            damage = 20;
//            knockback = 4f;
//        }

//        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
//        {
//            cooldown = 30;
//            randExtraCooldown = 30;
//        }

//        // todo: implement
//        // public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
//        // 	projType = ProjectileType<SparklingBall>();
//        // 	attackDelay = 1;
//        // }

//        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
//        {
//            multiplier = 12f;
//            randomOffset = 2f;
//        }

//        //public override void LoadData(TagCompound tag)
//        //{
//        //    NumberOfTimesTalkedTo = tag.GetInt("numberOfTimesTalkedTo");
//        //}

//        //public override void SaveData(TagCompound tag)
//        //{
//        //    tag["numberOfTimesTalkedTo"] = NumberOfTimesTalkedTo;
//        //}
//    }

//    //public class ExamplePersonProfile : ITownNPCProfile
//    //{
//    //    public int RollVariation() => 0;
//    //    public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

//    //    public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
//    //    {
//    //        if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
//    //            return ModContent.Request<Texture2D>("ExampleMod/Content/NPCs/ExamplePerson");

//    //        if (npc.altTexture == 1)
//    //            return ModContent.Request<Texture2D>("ExampleMod/Content/NPCs/ExamplePerson_Party");

//    //        return ModContent.Request<Texture2D>("ExampleMod/Content/NPCs/ExamplePerson");
//    //    }

//    //    public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("ExampleMod/Content/NPCs/ExamplePerson_Head");
//    //}
//}