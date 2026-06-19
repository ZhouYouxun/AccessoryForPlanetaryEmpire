using System.Linq;
using Terraria.ModLoader;
//using ExampleMod.Common.Configs;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Movement;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Accessory.Movement;
using AccessoryForPlanetaryEmpire.Accessory.Dash;
using AccessoryForPlanetaryEmpire.Accessory;
using Terraria.Graphics.Shaders;
using System;


namespace AccessoryForPlanetaryEmpire.Accessory.Wings
{
    [AutoloadEquip(EquipType.Wings)]
    public class IMPWing : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Accessory.Wings";


        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;

            // 这些翅膀使用了自定义的翅膀参数，包括启用悬停功能，平飞最大速度设为12，平飞加速度设为12
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(
                10800, // 飞行时间
                11.5f, // 飞行速度
                5.1f, // 加速度
                true, // 启用平飞功能
                12f, // 平飞最大速度
                5f // 平飞加速度
            );

            // 翅膀性能数据参考
            // 名称            平飞最大速度    平飞加速度
            // 悬浮板            10             10
            // 双足翼龙之翼      12             12
            // 星云斗篷          12             12
            // 星旋强化翼        12             12
            // 天界星盘          16             16
        }

        public override void SetDefaults()
        {
            Item.width = 64;
            Item.height = 64;
            Item.accessory = true;
            // 最终毕业装：Master，价值95金
            Item.rare = ItemRarityID.Master;
            Item.value = Item.buyPrice(0, 95, 0, 0);
            Item.value = Item.sellPrice(0, 95, 0, 0);

        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
            ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 3.85f; // Falling glide speed
            ascentWhenRising = 0.75f; // Rising speed
            maxCanAscendMultiplier = 1.4f;
            maxAscentMultiplier = 4.1f;
            constantAscend = 0.135f;
        }

        //public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
        //{
        //    speed = 7f; // 水平飞行速度
        //    acceleration = 2f; // 水平飞行加速度
        //}


        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // 原始飞行判断逻辑保持一致
            if (player.controlJump && player.wingTime > 0f && player.jump == 0 && player.velocity.Y != 0f && !hideVisual)
            {
                // ✅ 位置设定为玩家背后，即翅膀起始点附近
                Vector2 wingPos = player.MountedCenter + new Vector2(player.direction * -20f, -6f);

                float time = Main.GameUpdateCount * 0.15f;

                for (int i = 0; i < 2; i++)
                {
                    float offsetX = (float)Math.Sin(time + i * MathHelper.Pi) * 16f;
                    float offsetY = (float)Math.Cos((time + i * MathHelper.Pi) * 2f) * 2f;
                    Vector2 spawnPos = wingPos + new Vector2(offsetX, offsetY);

                    // 🌟 电+星辉混合 Dust
                    int dustType = Utils.SelectRandom(Main.rand, 226, 229, 230, 307);
                    Dust dust = Dust.NewDustPerfect(spawnPos, dustType, Vector2.Zero, 100, Color.White, 1.3f);
                    dust.noGravity = true;
                    dust.velocity *= 0f;

                    // ✅ 添加玩家染料着色
                    dust.shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);

                    // 🌟 点缀光点
                    if (Main.rand.NextBool(4))
                        PLALightingBoltsSystem.PLA_Spawn_IMPWing(spawnPos);
                }
            }


            // 一些基础效果
            player.GetDamage(DamageClass.Generic) += 0.15f;
            player.lifeRegen += 15;
            player.statDefense += 15;
            player.noFallDmg = true; // 不受到摔落伤害                                     
            player.jumpSpeedBoost += player.jumpSpeedBoost * 0.45f; // 增加45%的跳跃速度
            player.moveSpeed += 0.18f; // 先加一点点移动速度
            player.accRunSpeed = 9f; // 加速度的提升
            player.moveSpeed += player.moveSpeed * 0.25f; // 然后增加25%的移速

            // 一堆免疫的东西
            player.iceSkate = true;
            player.waterWalk = true;
            player.fireWalk = true;
            player.lavaImmune = true;
            player.noFallDmg = true;

            // 工匠令牌的效果
            if (Item.prefix == PrefixID.Warding) // Warding (护佑) 增加4点防御
            {
                player.statDefense += 4 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Menacing) // Menacing (险恶) 增加4%伤害
            {
                player.GetDamage(DamageClass.Generic) += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Quick) // Quick (迅捷) 增加4%移动速度
            {
                player.moveSpeed += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Arcane) // Arcane (奥术) 增加20点魔力
            {
                player.statManaMax2 += 20 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Hard) // Hard (坚韧) 增加1点防御力
            {
                player.statDefense += 1 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Guarding) // Guarding (守护) 增加2点防御力
            {
                player.statDefense += 2 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Armored) // Armored (装甲) 增加3点防御力
            {
                player.statDefense += 3 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Precise) // Precise (精准) 增加2%暴击率
            {
                player.GetCritChance(DamageClass.Generic) += 2 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Lucky) // Lucky (幸运) 增加4%暴击率
            {
                player.GetCritChance(DamageClass.Generic) += 4 * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Jagged) // Jagged (锯齿) 增加1%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Spiked) // Spiked (尖刺) 增加2%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Angry) // Angry (愤怒) 增加3%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Menacing) // Menacing (险恶) 增加4%攻击力
            {
                player.GetDamage(DamageClass.Generic) += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Brisk) // Brisk (活泼) 增加1%移动速度
            {
                player.moveSpeed += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Fleeting) // Fleeting (转瞬) 增加2%移动速度
            {
                player.moveSpeed += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Hasty) // Hasty (匆忙) 增加3%移动速度
            {
                player.moveSpeed += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Quick) // Quick (迅捷) 增加4%移动速度
            {
                player.moveSpeed += 0.04f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Wild) // Wild (狂野) 增加1%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.01f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Rash) // Rash (轻率) 增加2%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.02f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Intrepid) // Intrepid (无畏) 增加3%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.03f * 6; // 乘以6倍效果
            }
            else if (Item.prefix == PrefixID.Violent) // Violent (暴力) 增加4%近战攻速
            {
                player.GetAttackSpeed(DamageClass.Melee) += 0.04f * 6; // 乘以6倍效果
            }

            // ✅ 启用悬停（灾厄式悬停）
            if (player.controlJump && player.wingTime > 0f && player.jump == 0)
            {
                bool hovering = player.TryingToHoverDown && !player.merman;

                if (hovering)
                {
                    // 保持漂浮状态
                    player.velocity.Y *= 0.2f;
                    if (player.velocity.Y > -2f && player.velocity.Y < 2f)
                    {
                        player.velocity.Y = 0.105f;
                    }

                    // ⚠️可选：是否要补充飞行时间？你可以注释掉这行
                    // player.wingTime += 0.75f;
                }

                // ✅ 视觉粒子特效（可保留你自己的效果）
            }


            // 当玩家飞行、没有隐藏翅膀视觉效果、玩家在空中且按住跳跃键时
            if (player.wingTime > 0f && !hideVisual && player.velocity.Y != 0f && player.controlJump)
            {
                // 定义粒子生成位置的偏移量（以格子为单位，每格等于16像素）
                Vector2 electricOffset1 = new Vector2(1, 1) * 16f;
                Vector2 electricOffset2 = new Vector2(-1, 1) * 16f;

                // 根据玩家的朝向动态调整生成位置
                if (player.direction == -1)
                {
                    // 玩家朝左时，调整偏移量
                    electricOffset1.X = -electricOffset1.X;
                    electricOffset2.X = -electricOffset2.X;
                }

                // 生成电能粒子特效
                //CreateDust(player.Center + electricOffset1, DustID.Electric, Color.White);
                //CreateDust(player.Center + electricOffset2, DustID.Electric, Color.White);
            }


            player.GetModPlayer<TeleportationPlayer>().TeleportationEquipped = true;
            player.GetModPlayer<WindrunnerBootsPlayer>().hasSwiftWindWalkerBoots = true;
            player.GetModPlayer<SpeedModActivePlayer>().speedModActiveEquipped = true; 
            player.GetModPlayer<PowerJetPackPlayer>().hasPowerJetPack = true;



            if (hideVisual)
            {          
                player.GetModPlayer<IMPWingPlayer>().hasIMPWingAD = true;
                player.GetModPlayer<IMPWingPlayer>().hasIMPWing = false;
            }
            else
            {
                player.GetModPlayer<IMPWingPlayer>().hasIMPWingAD = false;
                player.GetModPlayer<IMPWingPlayer>().hasIMPWing = true;
            }
            //if (hideVisual)
            //{
            //    player.GetModPlayer<IMPWingADPlayer>().hasIMPWingAD = true;
            //    player.GetModPlayer<IMPWingPlayer>().hasIMPWing = false;
            //}
            //else
            //{
            //    player.GetModPlayer<IMPWingADPlayer>().hasIMPWingAD = false;
            //    player.GetModPlayer<IMPWingPlayer>().hasIMPWing = true;
            //}
        }



        // 创建粒子特效的函数
        private void CreateDust(Vector2 position, int dustType, Color color)
        {
            // 生成一个无重力的粒子特效，位置根据传入的坐标确定
            Dust dust = Dust.NewDustPerfect(position, dustType, null, 100, color, 1.5f);
            dust.noGravity = true; // 粒子不受重力影响
            dust.velocity *= 0.3f; // 将粒子的速度调整为原来的30%，使其移动较慢
            dust.shader = GameShaders.Armor.GetSecondaryShader(Main.LocalPlayer.cWings, Main.LocalPlayer); // 使用玩家翅膀的染色效果
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LongRainbowTrailWings, 1);
            recipe.AddIngredient(ItemID.EmpressFlightBooster, 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 100);
            recipe.AddIngredient(ModContent.ItemType<SweetheartCloak>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ElfEnergyCore>(), 1);
            recipe.AddIngredient(ModContent.ItemType<ShallowOxygenBottle>(), 1);
            recipe.AddIngredient(ModContent.ItemType<PowerJetPack>(), 1);

            //recipe.AddIngredient(ModContent.ItemType<Earthshaking>(), 1);
            //recipe.AddIngredient(ModContent.ItemType<QueensCrown>(), 1);
            //recipe.AddIngredient(ModContent.ItemType<CommandersSaw>(), 1);
            //recipe.AddIngredient(ModContent.ItemType<MultipleTorpedoTubes>(), 1);

            recipe.AddIngredient(ModContent.ItemType<ArtisansToken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SpeedModActive>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SwiftWindWalkerBoots>(), 1);
            recipe.AddIngredient(ModContent.ItemType<Teleportation>(), 1);

            // 判断是否启用了 CalamityMod
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
            {
                recipe.AddIngredient(calamityMod.Find<ModItem>("EffulgentFeather").Type, 5);
            }

            recipe.AddIngredient(ItemID.LunarBar, 5);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }
    }
}