//using Microsoft.Xna.Framework;
//using System.IO;
//using Terraria;
//using Terraria.GameContent.Bestiary;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria.Localization;
//using System;

//namespace AccessoryForPlanetaryEmpire.NPCs.BossAIs.ImperialArmoredTrain
//{
//    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
//    internal class ImperialArmoredTrainHead : WormHead
//    {
//        public override int BodyType => ModContent.NPCType<ImperialArmoredTrainBody>();

//        public override int TailType => ModContent.NPCType<ImperialArmoredTrainTail>();

//        public override void SetStaticDefaults()
//        {
//            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
//            { // Influences how the NPC looks in the Bestiary
//                CustomTexturePath = "PlanetaryEmpireBosses/Bosses/ImperialArmoredTrain_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
//                Position = new Vector2(40f, 24f),
//                PortraitPositionXOverride = 0f,
//                PortraitPositionYOverride = 12f
//            };
//            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
//        }

//        public override void SetDefaults()
//        {
//            NPC.aiStyle = -1; // 自定义AI

//            NPC.width = 34 * 2; // 头部宽度
//            NPC.height = 45 * 2; // 头部高度

//            NPC.damage = 80; // 伤害值
//            NPC.defense = 20; // 防御值

//            NPC.lifeMax = 80000; // 最大生命值

//            NPC.knockBackResist = 0f; // 击退抵抗

//            NPC.value = 0; // 击败后的价值

//            NPC.npcSlots = 1f; // 占用NPC槽位

//            NPC.noTileCollide = true; // 无碰撞瓦块
//            NPC.noGravity = true; // 无重力
//            NPC.lavaImmune = true; // 免疫岩浆

//            NPC.boss = true; // 是boss
//            NPC.dontTakeDamage = false; // 可以受到伤害
//            NPC.scale = 1f; // 大小缩放

//            NPC.buffImmune[BuffID.Confused] = true; // 免疫混乱Debuff
//            NPC.buffImmune[BuffID.OnFire] = true; // 免疫着火Debuff

//            NPC.netAlways = true; // 在网络中始终同步

//            // 如果需要设置特定的音效，取消注释下面两行并选择合适的声音ID
//            // NPC.soundHit = SoundID.NPCHit4;
//            // NPC.soundKilled = SoundID.NPCDeath14;
//        }

//        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
//        {
//            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
//            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
//				// Sets the spawning conditions of this NPC that is listed in the bestiary.
//				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
//                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,

//				// Sets the description of this NPC that is listed in the bestiary.
//				new FlavorTextBestiaryInfoElement("Looks like a Digger fell into some aqua-colored paint. Oh well.")
//            });
//        }

//        public override void Init()
//        {
//            // 设置分段方差
//            // 如果希望段长恒定，将这两个属性设置为相同的值
//            MinSegmentLength = 24;
//            MaxSegmentLength = 30;
//            CommonWormInit(this);
//        }

//        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
//        internal static void CommonWormInit(Worm worm)
//        {
//            // These two properties handle the movement of the worm
//            worm.MoveSpeed = 5.5f;
//            worm.Acceleration = 0.045f;
//        }

//        private int attackCounter;
//        public override void SendExtraAI(BinaryWriter writer)
//        {
//            writer.Write(attackCounter);
//        }

//        public override void ReceiveExtraAI(BinaryReader reader)
//        {
//            attackCounter = reader.ReadInt32();
//        }




//        public override void AI()
//        {
//            Player target = Main.player[NPC.target];

//            // Calculate the center position of the circle
//            Vector2 circleCenter = target.Center;

//            // Calculate the radius of the circle
//            float circleRadius = 50f * NPC.scale; // Adjust the radius based on the boss's scale

//            // Calculate the angle between each segment in the circle
//            float angleBetweenSegments = MathHelper.TwoPi / NPC.realLife;

//            // Loop through each segment of the boss
//            for (int i = 0; i < NPC.realLife; i++)
//            {
//                NPC segment = Main.npc[NPC.realLife - i - 1];

//                // Calculate the angle for the current segment
//                float angle = i * angleBetweenSegments;

//                // Calculate the position of the segment on the circle
//                Vector2 segmentPosition = circleCenter + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * circleRadius;

//                // Move the segment to the calculated position
//                segment.position = segmentPosition;
//                segment.velocity = Vector2.Zero;
//                segment.direction = 1; // Set the segment's direction to move clockwise
//                segment.spriteDirection = 1; // Set the segment's sprite direction to face right
//                segment.rotation = angle + MathHelper.PiOver2; // Adjust the segment's rotation to face outward from the circle

//                // Update the segment's AI, if necessary
//                // Add your custom AI logic for each segment here

//                // Optional: Set other properties for the segment if needed
//                segment.lifeMax = 5000; // Adjust the segment's maximum life
//                segment.life = segment.lifeMax; // Set the segment's current life

//                // Optional: Add any special effects or custom behavior for each segment

//                // Optional: If you want the segments to despawn after their health reaches 0, uncomment the following line
//                // segment.dontTakeDamage = true;

//                segment.netUpdate = true;
//            }

//            // Keep the boss from moving or rotating
//            NPC.position = NPC.Center;
//            NPC.velocity = Vector2.Zero;
//            NPC.rotation = 0f;
//            NPC.direction = NPC.spriteDirection = NPC.Center.X < target.Center.X ? 1 : -1;

//            // Optional: Adjust the boss's health or damage values based on your needs
//            NPC.lifeMax = 50000;
//            NPC.life = NPC.lifeMax;

//            // Optional: Add any special effects or custom behavior for the boss during the circle attack

//            // Set the boss's AI fields
//            NPC.ai[0] = 1f;
//            NPC.ai[1] = 0f;
//            NPC.ai[2] = 0f;

//            // Optional: If you want the boss to despawn after its health reaches 0, uncomment the following line
//            // NPC.dontTakeDamage = true;

//            // Optional: If you want the boss to drop specific items, handle the drops here

//            // Optional: If you want the boss to have a death animation or special effects, handle them here
//        }










































//        //public override void AI()
//        //{
//        //    if (Main.netMode != NetmodeID.MultiplayerClient)
//        //    {
//        //        if (attackCounter > 0)
//        //        {
//        //            attackCounter--; // tick down the attack counter.
//        //        }

//        //        Player target = Main.player[NPC.target];
//        //        // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
//        //        if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
//        //        {
//        //            Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
//        //            direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

//        //            int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 1, ProjectileID.ShadowBeamHostile, 5, 0, Main.myPlayer);
//        //            Main.projectile[projectile].timeLeft = 300;
//        //            attackCounter = 500;
//        //            NPC.netUpdate = true;
//        //        }
//        //    }
//        //}




//        //public override void AI()
//        //{
//        //    if (Main.netMode != NetmodeID.MultiplayerClient)
//        //    {
//        //        Player target = Main.player[NPC.target];

//        //        // 阶段判断
//        //        bool isPhase1 = NPC.life > NPC.lifeMax / 2; // 是否处于第1阶段

//        //        if (isPhase1)
//        //        {
//        //            // 第1阶段的逻辑
//        //            if (NPC.ai[0] == 0f)
//        //            {
//        //                // 寻空模式
//        //                Vector2 targetPosition = target.Center + new Vector2(0f, -400f); // 玩家正上方50格高度
//        //                float moveSpeed = 12f; // 移动速度

//        //                Vector2 moveDirection = targetPosition - NPC.Center;
//        //                float distanceToTarget = moveDirection.Length();

//        //                if (distanceToTarget > 20f)
//        //                {
//        //                    moveDirection.Normalize();
//        //                    moveDirection *= moveSpeed;

//        //                    NPC.velocity = (NPC.velocity * 40f + moveDirection) / 41f;
//        //                }
//        //                else
//        //                {
//        //                    NPC.velocity *= 0.9f;
//        //                }

//        //                // 头部射击 DeathLaser
//        //                if (Main.rand.Next(180) == 0)
//        //                {
//        //                    Vector2 direction = target.Center - NPC.Center;
//        //                    direction.Normalize();

//        //                    int laserProjectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 10f, ProjectileID.DeathLaser, 80, 0, Main.myPlayer);
//        //                    Main.projectile[laserProjectile].timeLeft = 300;
//        //                }

//        //                // 身体射击 BombSkeletronPrime
//        //                if (Main.rand.Next(120) == 0)
//        //                {
//        //                    Vector2 direction = target.Center - NPC.Center;
//        //                    direction.Normalize();

//        //                    int bombProjectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, direction * 10f, ProjectileID.BombSkeletronPrime, 50, 0, Main.myPlayer);
//        //                    Main.projectile[bombProjectile].timeLeft = 300;
//        //                }

//        //                // 尾部释放蜜蜂 Bee
//        //                if (Main.rand.Next(60) == 0)
//        //                {
//        //                    for (int i = 0; i < 14; i++)
//        //                    {
//        //                        int beeProjectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)), ProjectileID.Bee, 20, 0, Main.myPlayer);
//        //                        Main.projectile[beeProjectile].timeLeft = 300;
//        //                    }
//        //                }
//        //            }
//        //        }
//        //        else
//        //        // 第2阶段--------------------------------------------------------------------------------------------------------------------------------
//        //        {
//        //            // 第2阶段的逻辑
//        //            if (NPC.ai[0] == 0f)
//        //            {
//        //                // 冲刺模式
//        //                NPC.ai[0] = 1f; // 标记为冲刺模式
//        //                NPC.ai[1] = 0f; // 重置冲刺计数

//        //                NPC.velocity = Vector2.Zero; // 停止移动
//        //            }

//        //            // 冲刺模式
//        //            if (NPC.ai[0] == 1f)
//        //            {
//        //                // 冲刺计数
//        //                NPC.ai[1]++;
//        //                if (NPC.ai[1] <= 10)
//        //                {
//        //                    // 冲刺前的减速
//        //                    NPC.velocity *= 0.95f;
//        //                }
//        //                else if (NPC.ai[1] > 10 && NPC.ai[1] <= 20)
//        //                {
//        //                    // 冲刺
//        //                    Vector2 direction = target.Center - NPC.Center;
//        //                    direction.Normalize();
//        //                    NPC.velocity = direction * 70f;
//        //                }
//        //                else if (NPC.ai[1] > 20)
//        //                {
//        //                    // 围绕玩家形成圈
//        //                    NPC.ai[0] = 2f; // 标记为围绕模式
//        //                    NPC.ai[1] = 0f; // 重置围绕计数

//        //                    // 计算圆周上的点
//        //                    int segmentCount = (int)(2 * Math.PI * 50 / NPC.width);
//        //                    float angleIncrement = MathHelper.TwoPi / segmentCount;

//        //                    for (int i = 0; i < NPC.oldPos.Length; i++)
//        //                    {
//        //                        NPC.oldPos[i] = NPC.position;
//        //                    }

//        //                    for (int i = 0; i < NPC.oldRot.Length; i++)
//        //                    {
//        //                        NPC.oldRot[i] = NPC.rotation;
//        //                    }
//        //                }
//        //            }
//        //            else if (NPC.ai[0] == 2f)
//        //            {
//        //                // 围绕模式
//        //                NPC.ai[1]++;
//        //                if (NPC.ai[1] <= 10)
//        //                {
//        //                    // 围绕前的减速
//        //                    NPC.velocity *= 0.95f;
//        //                }
//        //                else if (NPC.ai[1] > 10 && NPC.ai[1] <= 20)
//        //                {
//        //                    // 围绕玩家
//        //                    Vector2 direction = target.Center - NPC.Center;
//        //                    direction.Normalize();
//        //                    NPC.velocity = direction * 20f;

//        //                    // 计算圆周上的点
//        //                    int segmentCount = (int)(2 * Math.PI * 50 / NPC.width);
//        //                    float angleIncrement = MathHelper.TwoPi / segmentCount;

//        //                    for (int i = 0; i < NPC.oldPos.Length; i++)
//        //                    {
//        //                        NPC.oldPos[i] = NPC.position;
//        //                    }

//        //                    for (int i = 0; i < NPC.oldRot.Length; i++)
//        //                    {
//        //                        NPC.oldRot[i] = NPC.rotation;
//        //                    }
//        //                }
//        //                else if (NPC.ai[1] > 20)
//        //                {
//        //                    // 发射 DeathLaser
//        //                    for (int i = 2; i < NPC.oldPos.Length; i++)
//        //                    {
//        //                        if (i % 3 == 0) // 每一秒只有三个身体射出 DeathLaser
//        //                        {
//        //                            Vector2 direction = target.Center - NPC.oldPos[i];
//        //                            direction.Normalize();

//        //                            // int projectile = Projectile.NewProjectile(NPC.GetProjectileSpawnSource(), NPC.Center, direction * 10f, ProjectileID.DeathLaser, 80, 0, Main.myPlayer);
//        //                            // Main.projectile[projectile].timeLeft = 300;
//        //                            NPC.netUpdate = true;
//        //                        }
//        //                    }

//        //                    // 重置计数和模式
//        //                    NPC.ai[0] = 1f; // 标记为冲刺模式
//        //                    NPC.ai[1] = 0f; // 重置冲刺计数
//        //                }
//        //            }
//        //        }

//        //    }
//        //}

//    }



//    internal class ImperialArmoredTrainBody : WormBody
//    {
//        public override void SetStaticDefaults()
//        {
//            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
//            {
//                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
//            };
//            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
//        }

//        public override void SetDefaults()
//        {
//            NPC.aiStyle = -1; // 自定义AI

//            NPC.width = 38 * 2; // 身体宽度
//            NPC.height = 34 * 2; // 身体高度

//            NPC.damage = 80; // 伤害值
//            NPC.defense = 170; // 防御值

//            NPC.lifeMax = 50000; // 最大生命值

//            NPC.knockBackResist = 0f; // 击退抵抗

//            NPC.value = 0; // 击败后的价值

//            NPC.npcSlots = 1f; // 占用NPC槽位

//            NPC.noTileCollide = true; // 无碰撞瓦块
//            NPC.noGravity = true; // 无重力
//            NPC.lavaImmune = true; // 免疫岩浆

//            NPC.boss = true; // 是boss
//            NPC.dontTakeDamage = false; // 可以受到伤害
//            NPC.scale = 1f; // 大小缩放

//            NPC.buffImmune[BuffID.Confused] = true; // 免疫混乱Debuff
//            NPC.buffImmune[BuffID.OnFire] = true; // 免疫着火Debuff

//            NPC.netAlways = true; // 在网络中始终同步

//            // 如果需要设置特定的音效，取消注释下面两行并选择合适的声音ID
//            // NPC.soundHit = SoundID.NPCHit4;
//            // NPC.soundKilled = SoundID.NPCDeath14;
//        }


//        public override void Init()
//        {
//            ImperialArmoredTrainHead.CommonWormInit(this);
//        }
//    }

//    internal class ImperialArmoredTrainTail : WormTail
//    {
//        public override void SetStaticDefaults()
//        {
//            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
//            {
//                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
//            };
//            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
//        }

//        public override void SetDefaults()
//        {
//            NPC.aiStyle = -1; // 自定义AI

//            NPC.width = 30 * 2; // 尾部宽度
//            NPC.height = 29 * 2; // 尾部高度

//            NPC.damage = 300; // 伤害值
//            NPC.defense = 0; // 防御值

//            NPC.lifeMax = 300000; // 最大生命值

//            NPC.knockBackResist = 0f; // 击退抵抗

//            NPC.value = 0; // 击败后的价值

//            NPC.npcSlots = 1f; // 占用NPC槽位

//            NPC.noTileCollide = true; // 无碰撞瓦块
//            NPC.noGravity = true; // 无重力
//            NPC.lavaImmune = true; // 免疫岩浆

//            NPC.boss = true; // 是boss
//            NPC.dontTakeDamage = false; // 可以受到伤害
//            NPC.scale = 1f; // 大小缩放

//            NPC.buffImmune[BuffID.Confused] = true; // 免疫混乱Debuff
//            NPC.buffImmune[BuffID.OnFire] = true; // 免疫着火Debuff

//            NPC.netAlways = true; // 在网络中始终同步

//            // 如果需要设置特定的音效，取消注释下面两行并选择合适的声音ID
//            // NPC.soundHit = SoundID.NPCHit4;
//            // NPC.soundKilled = SoundID.NPCDeath14;
//        }


//        public override void Init()
//        {
//            ImperialArmoredTrainHead.CommonWormInit(this);
//        }
//    }
//}



