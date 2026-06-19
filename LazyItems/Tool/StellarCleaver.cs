using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.DataStructures;
using System;
using System.Collections.Generic;

namespace AccessoryForPlanetaryEmpire.LazyItems.Tool
{
    public class StellarCleaver : ModItem
    {

        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
            ItemID.Sets.UsesBetterMeleeItemLocation[Item.type] = true; // 提供更好的近战挥舞位置偏移
            ItemID.Sets.ForcesBreaksSleeping[Item.type] = false; // 使用此物品时，是否会唤醒睡觉的玩家
            //ItemID.Sets.CanGetPrefixes[Item.type] = false; // 此物品无法拥有任何前缀
            //ItemID.Sets.BonusAttackSpeedMultiplier[Item.type] = 0.5f; // 攻速加成效果减半
        }

        public override void SetDefaults()
        {
            Item.damage = CalculateDamage();
            Item.DamageType = DamageClass.Melee;
            Item.width = 49;
            Item.height = 49;
            Item.useTime = 3;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 6;
            Item.value = Item.buyPrice(gold: 40);
            Item.rare = ItemRarityID.Expert;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.scale = 1.0f;
            Item.pick = 59;
            Item.tileBoost = GetCurrentMaxTileBoost();
            Item.axe = 100;
            Item.hammer = 100;
            Item.useTurn = true;
        }
        public override Vector2? HoldoutOffset() => new Vector2(-25, -25);

        public override void HoldItem(Player player)
        {
            StellarCleaverPlayer cleaverPlayer = player.GetModPlayer<StellarCleaverPlayer>();
            cleaverPlayer.HoldingStellarCleaver = true;
            cleaverPlayer.CurrentMaxTileBoost = GetCurrentMaxTileBoost();
            cleaverPlayer.ClampSettings();
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (!player.GetModPlayer<StellarCleaverPlayer>().LightingParticlesEnabled)
            {
                return;
            }

            // 💡 蓝白照明（偏冷光）
            Lighting.AddLight(player.Center, 0.6f, 0.8f, 1.5f);

            // 🎐 控制粒子生成几率（越小越稀疏）
            if (Main.rand.NextBool(1)) // 大约 25% 概率每帧出一个
            {
                int particleCount = Main.rand.Next(2, 4); // 每次生成 2~3 个粒子

                for (int i = 0; i < particleCount; i++)
                {
                    // 🔹 在武器附近随机生成位置
                    Vector2 spawnPos = hitbox.Center.ToVector2() + Main.rand.NextVector2Circular(20f, 20f);

                    // 🔹 粒子速度方向：从玩家中心 → 向外扩散
                    Vector2 direction = (spawnPos - player.Center).SafeNormalize(Vector2.UnitX);
                    Vector2 velocity = direction * Main.rand.NextFloat(3f, 6f); // 有一定速度

                    // 💠 创建蓝色系粒子（冰雪火把 Dust）
                    int dust = Dust.NewDust(spawnPos, 0, 0, DustID.IceTorch, velocity.X, velocity.Y, 150, default, Main.rand.NextFloat(1f, 1.5f));
                    Main.dust[dust].noGravity = true;       // 漂浮感
                    Main.dust[dust].velocity *= 0.8f;       // 稍微柔和
                    Main.dust[dust].fadeIn = 1.2f;          // 渐显
                    Main.dust[dust].scale *= Main.rand.NextFloat(0.9f, 1.3f);
                }
            }
        }



        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            //target.AddBuff(BuffID.OnFire, 300);
        }

        public override void OnHitPvp(Player player, Player target, Player.HurtInfo hurtInfo)
        {
            //target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Emerald, 3);
            recipe.AddIngredient(ItemID.Wood, 2);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }

        public override bool AltFunctionUse(Player player) => true; // 允许右键使用

        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                // 右键：只有锤子
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTime = CalculateUseTime();
                Item.useAnimation = 7;
                Item.damage = CalculateDamage();
                Item.axe = 0; // 去掉斧头
                Item.hammer = CalculateHammerPower();
                Item.pick = 0; // 去掉镐子
            }
            else
            {
                // 左键：镐子 + 斧子
                Item.useStyle = ItemUseStyleID.Swing;
                Item.useTime = CalculateUseTime();
                Item.useAnimation = 15;
                Item.damage = CalculateDamage();
                Item.axe = CalculateAxePower();   // ✅ 加上斧头
                Item.hammer = 0;                  // ❌ 不要锤子
                Item.pick = CalculatePickPower(player); // ✅ 保留镐子
            }
            return base.CanUseItem(player);
        }




        private int CalculatePickPower(Player player = null)
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int pickPower;
            int tileBoost;

            // 检查是否加载了 CalamityMod
            Mod calamityMod;
            bool calamityLoaded = ModLoader.TryGetMod("CalamityMod", out calamityMod);

            // 如果加载了 CalamityMod，改变镐力值
            if (calamityLoaded)
            {
                pickPower = 100;
                tileBoost = 10;
            }
            else
            {
                // 原有阶段检测
                pickPower = 59;
                tileBoost = 10;
            }

            if (eaterOfWorlds || brainOfCthulhu)
            {
                // 如果加载了 CalamityMod，改变镐力值
                if (calamityLoaded)
                {
                    pickPower = 100;
                    tileBoost = 15;
                }
                else
                {
                    // 原有阶段检测
                    pickPower = 70;
                    tileBoost = 15;
                }


            }


            if (skeletron)
            {
                pickPower = 100;
                tileBoost = 25;
            }
            if (wallOfFlesh)
            {
                pickPower = 190;
                tileBoost = 35;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                pickPower = 200;
                tileBoost = 60;
            }
            if (golem)
            {
                pickPower = 210;
                tileBoost = 85;
            }
            if (moonLord)
            {
                pickPower = 250;
                tileBoost = 130;
            }

            if (player != null)
            {
                tileBoost = player.GetModPlayer<StellarCleaverPlayer>().GetAppliedTileBoost(tileBoost);
            }

            Item.pick = pickPower;
            Item.tileBoost = tileBoost;
            return pickPower;
        }

        public static int GetCurrentMaxTileBoost()
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int tileBoost = 10;

            if (eaterOfWorlds || brainOfCthulhu)
            {
                tileBoost = 15;
            }
            if (skeletron)
            {
                tileBoost = 25;
            }
            if (wallOfFlesh)
            {
                tileBoost = 35;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                tileBoost = 60;
            }
            if (golem)
            {
                tileBoost = 85;
            }
            if (moonLord)
            {
                tileBoost = 130;
            }

            return tileBoost;
        }
        private int CalculateUseTime() // 计算挖掘速度（使用速度）
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int UseTime = 7;

            if (eaterOfWorlds || brainOfCthulhu)
            {
                UseTime = 7;
            }
            if (skeletron)
            {
                UseTime = 6;
            }
            if (wallOfFlesh)
            {
                UseTime = 5;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                UseTime = 4;
            }
            if (golem)
            {
                UseTime = 3;
            }
            if (moonLord)
            {
                UseTime = 3;
            }

            Item.useTime = UseTime;
            return UseTime;
        }

        private int CalculateDamage() // 计算伤害
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int damage = 15;

            if (eaterOfWorlds || brainOfCthulhu)
            {
                damage = 22;
            }
            if (skeletron)
            {
                damage = 34;
            }
            if (wallOfFlesh)
            {
                damage = 67;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                damage = 98;
            }
            if (golem)
            {
                damage = 120;
            }
            if (moonLord)
            {
                damage = 150;
            }

            return damage;
        }


        private int CalculateAxePower() // 计算斧子的能力
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int axePower = 100;

            if (eaterOfWorlds || brainOfCthulhu)
            {
                axePower = 125;
            }
            if (skeletron)
            {
                axePower = 150;
            }
            if (wallOfFlesh)
            {
                axePower = 175;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                axePower = 200;
            }
            if (golem)
            {
                axePower = 225;
            }
            if (moonLord)
            {
                axePower = 250;
            }

            return axePower;
        }

        private int CalculateHammerPower() // 计算锤子的能力
        {
            bool eaterOfWorlds = NPC.downedBoss2;
            bool brainOfCthulhu = NPC.downedBoss2;
            bool skeletron = NPC.downedBoss3;
            bool wallOfFlesh = Main.hardMode;
            bool theDestroyer = NPC.downedMechBoss1;
            bool skeletronPrime = NPC.downedMechBoss2;
            bool theTwins = NPC.downedMechBoss3;
            bool golem = NPC.downedGolemBoss;
            bool moonLord = NPC.downedMoonlord;

            int hammerPower = 70;

            if (eaterOfWorlds || brainOfCthulhu)
            {
                hammerPower = 70;
            }
            if (skeletron)
            {
                hammerPower = 75;
            }
            if (wallOfFlesh)
            {
                hammerPower = 80;
            }
            if (theDestroyer || skeletronPrime || theTwins)
            {
                hammerPower = 120;
            }
            if (golem)
            {
                hammerPower = 155;
            }
            if (moonLord)
            {
                hammerPower = 500;
            }

            return hammerPower;
        }
        public override void UpdateInventory(Player player)
        {
            CalculateUseTime();
            CalculateDamage();
            CalculatePickPower(player);
            CalculateAxePower();
            CalculateHammerPower();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            tooltips.Add(new TooltipLine(Mod, "StellarCleaverMatrixDashboard", "按住左右键 1 秒打开矩阵仪表盘，可调节挖掘距离与照明粒子"));
        }

    }
}
