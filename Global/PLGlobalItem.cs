using AccessoryForPlanetaryEmpire.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;
using static tModPorter.ProgressUpdate;
using System;
using MonoMod.RuntimeDetour;
using static Humanizer.In;
using Microsoft.Xna.Framework.Input;
using System.Drawing;

namespace AccessoryForPlanetaryEmpire.Global
{
    public class PLGlobalItem : GlobalItem
    {
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<PlanetaryPlayer>().HeroDamageDevice && item.DamageType.CountsAsClass(DamageClass.Melee))
            {
                return false;
            }

            return true;
        }


        public override void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (item.DamageType.CountsAsClass(DamageClass.Melee) && player.GetModPlayer<PLPlayer>().HasHeroDamageDevice)
            {
                damage = (int)(damage * 0.05f);
            }



            //if (item.DamageType.CountsAsClass(DamageClass.Generic) && player.GetModPlayer<PLPlayer>().HasGiftFromTesla)
            //{
            //    int magnetSphereBallDamage = (int)(item.damage * 0.3f);

            //    // 获取玩家当前的攻击速度
            //    float currentAttackSpeed = player.GetModPlayer<PLPlayer>().CurrentAttackSpeed;

            //    // 计算发射间隔（根据攻击速度）
            //    int shootDelay = (int)(60f / currentAttackSpeed);

            //    // 检查是否满足发射条件
            //    if (player.itemAnimation > 0 && player.itemAnimation % shootDelay == 0)
            //    {
            //        // 创建火球
            //        Projectile.NewProjectile(player.Center, Vector2.Zero, ProjectileID.MagnetSphereBall, magnetSphereBallDamage, 0f, player.whoAmI);
            //    }
            //}



            // 这些代码已经被我废弃，他们原来属于重力增强和重力减弱装置，但由于它们会导致很多无法修复的bug，甚至导致崩溃，于是我便将他们永久移除了

            //if (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType.CountsAsClass(DamageClass.Magic) && player.GetModPlayer<PLPlayer>().HasGravityBooster) // 当拥有重力增强装置的时候
            //{
            //    for (int i = 0; i < Main.projectile.Length; i++)
            //    {
            //        Projectile projectile = Main.projectile[i];
            //        // 检查弹幕是否为玩家发射的
            //        if (projectile.owner == player.whoAmI && projectile.friendly)
            //        {
            //            if (projectile.scale < 7f) // 弹幕大小小于700%时进行增大
            //            {
            //                projectile.scale *= 1.3f; // 弹幕大小加30%
            //            }
            //            projectile.velocity *= 0.8f; // 弹幕速度减20%
            //        }
            //    }
            //}


            //if (item.DamageType.CountsAsClass(DamageClass.Melee) || item.DamageType.CountsAsClass(DamageClass.Magic) && player.GetModPlayer<PLPlayer>().HasGravityReducer) // 当拥有重力减弱装置的时候
            //{
            //    for (int i = 0; i < Main.projectile.Length; i++)
            //    {
            //        Projectile projectile = Main.projectile[i];

            //        // 检查弹幕是否为玩家发射的
            //        if (projectile.owner == player.whoAmI && projectile.friendly)
            //        {
            //            projectile.scale *= 0.8f; // 弹幕大小减20%
            //            projectile.timeLeft = (int)(projectile.timeLeft * 0.7f); // 速度变得快30%
            //        }
            //    }
            //
            //}
        }



        public class modRecipes : ModSystem
        {
            public override void AddRecipes()
            {

                // 赫尔墨斯靴
                Recipe recipe1 = Recipe.Create(ItemID.HermesBoots);
                recipe1.AddIngredient(ItemID.Silk, 15);
                recipe1.AddIngredient(ItemID.SwiftnessPotion, 5);
                recipe1.AddTile(TileID.WorkBenches);
                recipe1.Register();

                // 雷达
                Recipe recipe2 = Recipe.Create(ItemID.Radar);
                recipe2.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
                recipe2.AddTile(TileID.WorkBenches);
                recipe2.Register();

                // 金属探测器
                Recipe recipe3 = Recipe.Create(ItemID.MetalDetector);
                recipe3.AddIngredient(ItemID.CopperBar, 15);
                recipe3.AddIngredient(ItemID.Wire, 10);
                recipe3.AddIngredient(ItemID.Glowstick, 30);
                recipe3.AddTile(TileID.WorkBenches);
                recipe3.Register();

                Recipe recipeJSTC3 = Recipe.Create(ItemID.MetalDetector);
                recipeJSTC3.AddIngredient(ItemID.TinBar, 15);
                recipeJSTC3.AddIngredient(ItemID.Wire, 10);
                recipeJSTC3.AddIngredient(ItemID.Glowstick, 30);
                recipeJSTC3.AddTile(TileID.WorkBenches);
                recipeJSTC3.Register();

                // 魔法箭袋
                Recipe recipe4 = Recipe.Create(ItemID.MagicQuiver);
                recipe4.AddIngredient(ItemID.SoulBottleLight, 8);
                recipe4.AddIngredient(ItemID.PixieDust, 10);
                recipe4.AddIngredient(ItemID.EndlessQuiver, 1); 
                recipe4.AddIngredient(ItemID.Lens, 5);
                recipe4.AddTile(TileID.WorkBenches);
                recipe4.Register();

                // 溜冰鞋
                Recipe recipe5 = Recipe.Create(ItemID.IceSkates);
                recipe5.AddIngredient(ItemID.IceBlock, 15);
                recipe5.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe5.AddTile(TileID.WorkBenches);
                recipe5.Register();

                // 敏捷扣
                Recipe recipe6 = Recipe.Create(ItemID.Aglet);
                recipe6.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 5); 
                recipe6.AddIngredient(ItemID.SwiftnessPotion, 5);
                recipe6.AddTile(TileID.WorkBenches);
                recipe6.Register();

                // 疾风脚环
                Recipe recipe7 = Recipe.Create(ItemID.AnkletoftheWind);
                recipe7.AddIngredient(ItemID.Cloud, 15);
                recipe7.AddIngredient(ItemID.Ruby, 1);
                recipe7.AddTile(TileID.WorkBenches);
                recipe7.Register();

                // 云朵瓶
                Recipe recipe8 = Recipe.Create(ItemID.CloudinaBottle);
                recipe8.AddIngredient(ItemID.Bottle, 15);
                recipe8.AddIngredient(ItemID.FallenStar, 1);
                recipe8.AddIngredient(ItemID.Cloud, 100);
                recipe8.AddTile(TileID.WorkBenches);
                recipe8.Register();

                // 沙暴瓶
                Recipe recipe9 = Recipe.Create(ItemID.SandstorminaBottle);
                recipe9.AddIngredient(ItemID.Bottle, 15);
                recipe9.AddIngredient(ItemID.SandBlock, 100);
                recipe9.AddIngredient(ItemID.Sandstone, 100);
                recipe9.AddTile(TileID.WorkBenches);
                recipe9.Register();

                // 暴雪瓶
                Recipe recipe10 = Recipe.Create(ItemID.BlizzardinaBottle);
                recipe10.AddIngredient(ItemID.Bottle, 15);
                recipe10.AddIngredient(ItemID.SnowBlock, 100);
                recipe10.AddIngredient(ItemID.IceBlock, 100);
                recipe10.AddTile(TileID.WorkBenches);
                recipe10.Register();

                // 猛爪手套
                Recipe recipe11 = Recipe.Create(ItemID.FeralClaws);
                recipe11.AddIngredient(ItemID.Silk, 15);
                recipe11.AddIngredient(ItemID.JungleSpores, 5);
                recipe11.AddTile(TileID.WorkBenches);
                recipe11.Register();

                // 幸运马蹄
                Recipe recipe12 = Recipe.Create(ItemID.LuckyHorseshoe);
                recipe12.AddIngredient(ItemID.SunplateBlock, 10);
                recipe12.AddIngredient(ItemID.Cloud, 10);
                recipe12.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
                recipe12.AddTile(TileID.WorkBenches);
                recipe12.Register();

                // 水上靴子
                Recipe recipe13 = Recipe.Create(ItemID.WaterWalkingBoots);
                recipe13.AddIngredient(ItemID.Silk, 15);
                recipe13.AddIngredient(ItemID.WaterWalkingPotion, 5);
                recipe13.AddTile(TileID.WorkBenches);
                recipe13.Register();


                // 红气球
                Recipe recipe14 = Recipe.Create(ItemID.ShinyRedBalloon);
                recipe14.AddIngredient(ItemID.Cloud, 10);
                recipe14.AddIngredient(ItemID.Gel, 50);
                recipe14.AddIngredient(ItemID.Silk, 2);
                recipe14.AddTile(TileID.WorkBenches);
                recipe14.Register();


                // 钴盾牌
                Recipe recipe15 = Recipe.Create(ItemID.CobaltShield);
                recipe15.AddIngredient(ItemID.Bone, 15);
                recipe15.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 25);
                recipe15.AddTile(TileID.WorkBenches);
                recipe15.Register();

                // 天界磁石
                Recipe recipe16 = Recipe.Create(ItemID.CelestialMagnet);
                recipe16.AddIngredient(ItemID.FallenStar, 15);
                recipe16.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe16.AddIngredient(ItemID.WaterBucket, 3);
                recipe16.AddIngredient(ItemID.LuckyHorseshoe, 1);
                recipe16.AddTile(TileID.WorkBenches);
                recipe16.Register();

                // 暖手宝
                Recipe recipe17 = Recipe.Create(ItemID.HandWarmer);
                recipe17.AddIngredient(ItemID.Silk, 15);
                recipe17.AddIngredient(ItemID.SnowBlock, 114);
                recipe17.AddIngredient(ItemID.Shiverthorn, 5);
                recipe17.AddTile(TileID.WorkBenches);


                // 鞋钉
                Recipe recipe18 = Recipe.Create(ItemID.ShoeSpikes);
                recipe18.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 5);
                recipe18.AddIngredient(ItemID.Wood, 45);
                recipe18.AddTile(TileID.WorkBenches);
                recipe18.Register();


                // 攀爬爪
                Recipe recipe19 = Recipe.Create(ItemID.ClimbingClaws);
                recipe19.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 5);
                recipe19.AddIngredient(ItemID.Wood, 45);
                recipe19.AddTile(TileID.WorkBenches);
                recipe19.Register();

                // 鞋钉
                Recipe recipeXD18 = Recipe.Create(ItemID.ShoeSpikes);
                recipeXD18.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 5);
                recipeXD18.AddIngredient(ItemID.Wood, 45);
                recipeXD18.AddTile(TileID.WorkBenches);
                recipeXD18.Register();



                // 消耗品------------------------------------------------------------------------------
                // 生命水晶
                Recipe recipe20 = Recipe.Create(ItemID.LifeCrystal);
                recipe20.AddIngredient(ItemID.LesserHealingPotion, 20);
                recipe20.AddIngredient(ItemID.Ruby, 1);
                recipe20.AddIngredient(ItemID.Glass, 20);
                recipe20.AddTile(TileID.WorkBenches);
                recipe20.Register();


                // 生命果
                Recipe recipe21 = Recipe.Create(ItemID.LifeFruit);
                recipe21.AddIngredient(ItemID.LifeCrystal, 1);
                recipe21.AddIngredient(ItemID.SoulofLight, 1);
                recipe21.AddIngredient(ItemID.SoulofNight, 1);
                recipe21.AddTile(TileID.MythrilAnvil);
                recipe21.Register();


                // 困难模式前的武器------------------------------------------------------------------------------
                // 冰雪刃
                Recipe recipe22 = Recipe.Create(ItemID.IceBlade);
                recipe22.AddIngredient(ItemID.IceBlock, 120);
                recipe22.AddIngredient(ItemID.SnowBlock, 120);
                recipe22.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe22.AddTile(TileID.WorkBenches);
                recipe22.Register();

                // 妖刀村镇
                Recipe recipe23 = Recipe.Create(ItemID.Muramasa);
                recipe23.AddIngredient(ItemID.Bone, 30);
                recipe23.AddIngredient(ItemID.LavaBucket, 3);
                recipe23.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe23.AddTile(TileID.WorkBenches);
                recipe23.Register();


                // 心怒
                Recipe recipe24 = Recipe.Create(ItemID.Starfury);
                recipe24.AddIngredient(ItemID.FallenStar, 15);
                recipe24.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
                recipe24.AddIngredient(ItemID.Cloud, 15);
                recipe24.AddTile(TileID.WorkBenches);
                recipe24.Register();

                // 附魔剑
                Recipe recipe25 = Recipe.Create(ItemID.EnchantedSword);
                recipe25.AddIngredient(ItemID.FallenStar, 15);
                recipe25.AddIngredient(ItemID.StoneBlock, 100);
                recipe25.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 15);
                recipe25.AddIngredient(ItemID.Ruby, 1);
                recipe25.AddTile(TileID.WorkBenches);
                recipe25.Register();


                // 泰拉魔刃
                Recipe recipe26 = Recipe.Create(ItemID.Terragrim);
                recipe26.AddIngredient(ItemID.EnchantedSword, 5);
                recipe26.AddTile(TileID.WorkBenches);
                recipe26.Register();

                // 血雨弓
                Recipe recipe27 = Recipe.Create(ItemID.BloodRainBow);
                recipe27.AddIngredient(ItemID.Wood, 100);
                recipe27.AddIngredient(ItemID.MoneyTrough, 1);
                recipe27.AddIngredient(ItemID.WoodenArrow, 300);
                recipe27.AddTile(TileID.WorkBenches);
                recipe27.Register();

                // 三发猎枪
                Recipe recipe28 = Recipe.Create(ItemID.Boomstick);
                recipe28.AddIngredient(ItemID.JungleSpores, 5);
                recipe28.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe28.AddIngredient(ItemID.IllegalGunParts, 1);
                recipe28.AddTile(TileID.WorkBenches);
                recipe28.Register();


                // 手枪
                Recipe recipe29 = Recipe.Create(ItemID.Handgun);
                recipe29.AddIngredient(ItemID.Bone, 30);
                recipe29.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 15);
                recipe29.AddIngredient(ItemID.IllegalGunParts, 1);
                recipe29.AddTile(TileID.WorkBenches);
                recipe29.Register();


                // 火枪和夺命枪互换
                Recipe recipe30 = Recipe.Create(ItemID.TheUndertaker);
                recipe30.AddIngredient(ItemID.Musket, 1);
                recipe30.AddTile(TileID.WorkBenches);
                recipe30.Register();                
                Recipe recipe31 = Recipe.Create(ItemID.Musket);
                recipe31.AddIngredient(ItemID.TheUndertaker, 1);
                recipe31.AddTile(TileID.WorkBenches);
                recipe31.Register();


                // 油漆枪
                Recipe recipe32 = Recipe.Create(ItemID.PainterPaintballGun);
                recipe32.AddIngredient(ItemID.Paintbrush, 1);
                recipe32.AddIngredient(ItemID.PaintRoller, 1);
                recipe32.AddIngredient(ItemID.PaintScraper, 1);
                recipe32.AddTile(TileID.WorkBenches);
                recipe32.Register();

                // 钻石法杖
                Recipe recipe33 = Recipe.Create(ItemID.DiamondStaff);
                recipe33.AddIngredient(ItemID.Wood, 10);
                recipe33.AddIngredient(ItemID.StoneBlock, 10);
                recipe33.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
                recipe33.AddTile(TileID.WorkBenches);
                recipe33.Register();

                // 猩红魔杖和魔刺互换
                Recipe recipe34 = Recipe.Create(ItemID.Vilethorn);
                recipe34.AddIngredient(ItemID.CrimsonRod, 1);
                recipe34.AddTile(TileID.WorkBenches);
                recipe34.Register(); 
                Recipe recipe35 = Recipe.Create(ItemID.CrimsonRod);
                recipe35.AddIngredient(ItemID.Vilethorn, 1);
                recipe35.AddTile(TileID.WorkBenches);
                recipe35.Register();

                // 史莱姆法杖
                Recipe recipe36 = Recipe.Create(ItemID.SlimeStaff);
                recipe36.AddIngredient(ItemID.Wood, 10);
                recipe36.AddIngredient(ItemID.Gel, 10);
                recipe36.AddTile(TileID.WorkBenches);
                recipe36.Register();

                // 小鸟法杖
                Recipe recipe37 = Recipe.Create(ItemID.BabyBirdStaff);
                recipe37.AddIngredient(ItemID.Wood, 10);
                recipe37.AddIngredient(ItemID.Acorn, 1);
                recipe37.AddTile(TileID.WorkBenches);
                recipe37.Register();


                // 气球束
                Recipe recipe39 = Recipe.Create(ItemID.BundleofBalloons);
                recipe39.AddIngredient(ItemID.SnowBlock, 100);
                recipe39.AddIngredient(ItemID.SandBlock, 100);
                recipe39.AddIngredient(ItemID.Cloud, 100);
                recipe39.AddIngredient(ItemID.Gel, 20);
                recipe39.AddIngredient(ItemID.Bottle, 3);
                recipe39.AddTile(TileID.WorkBenches);
                recipe39.Register();

                // 手机
                Recipe recipe40 = Recipe.Create(ItemID.PDA);
                recipe40.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyIronBar", 10);
                recipe40.AddIngredient(ItemID.FallenStar, 10);
                recipe40.AddIngredient(ItemID.IceBlock, 200);
                recipe40.AddIngredient(ItemID.Chain, 10);
                recipe40.AddTile(TileID.WorkBenches);
                recipe40.Register();

                // 各种宝石
                Recipe recipe42 = Recipe.Create(ItemID.Diamond);
                recipe42.AddIngredient(ItemID.StoneBlock, 100);
                recipe42.AddTile(TileID.WorkBenches);
                recipe42.Register();
                Recipe recipe43 = Recipe.Create(ItemID.Ruby);
                recipe43.AddIngredient(ItemID.StoneBlock, 100);
                recipe43.AddTile(TileID.WorkBenches);
                recipe43.Register();
                Recipe recipe44 = Recipe.Create(ItemID.Emerald);
                recipe44.AddIngredient(ItemID.StoneBlock, 100);
                recipe44.AddTile(TileID.WorkBenches);
                recipe44.Register();
                Recipe recipe45 = Recipe.Create(ItemID.Sapphire);
                recipe45.AddIngredient(ItemID.StoneBlock, 100);
                recipe45.AddTile(TileID.WorkBenches);
                recipe45.Register();
                Recipe recipe46 = Recipe.Create(ItemID.Topaz);
                recipe46.AddIngredient(ItemID.StoneBlock, 100);
                recipe46.AddTile(TileID.WorkBenches);
                recipe46.Register();
                Recipe recipe47 = Recipe.Create(ItemID.Amethyst);
                recipe47.AddIngredient(ItemID.StoneBlock, 100);
                recipe47.AddTile(TileID.WorkBenches);
                recipe47.Register();
                Recipe recipe48 = Recipe.Create(ItemID.Amber);
                recipe48.AddIngredient(ItemID.StoneBlock, 100);
                recipe48.AddTile(TileID.WorkBenches);
                recipe48.Register();



                //-------------------------------------------------
                // 十字护符
                Recipe recipe38 = Recipe.Create(ItemID.AnkhCharm);
                recipe38.AddIngredient(ItemID.SoulofLight, 10);
                recipe38.AddIngredient(ItemID.SoulofNight, 10);
                recipe38.AddRecipeGroup("AccessoryForPlanetaryEmpire:AnyGoldBar", 10);
                recipe38.AddTile(TileID.WorkBenches);
                recipe38.Register();


                // 光束剑
                Recipe recipe49 = Recipe.Create(ItemID.BeamSword);
                recipe49.AddIngredient(ItemID.Bone, 20);
                recipe49.AddIngredient(ItemID.SoulofLight, 10);
                recipe49.AddIngredient(ItemID.SoulofNight, 10);
                recipe49.AddTile(TileID.WorkBenches);
                recipe49.Register();



               


                // 天顶
                Recipe recipe41 = Recipe.Create(ItemID.Zenith);
                recipe41.AddIngredient(ItemID.CopperShortsword, 1);
                recipe41.AddIngredient(ItemID.LunarBar, 10);
                recipe41.AddTile(TileID.LunarCraftingStation);
                recipe41.Register();

                // 天顶02
                Recipe recipeZE = Recipe.Create(ItemID.Zenith);
                recipeZE.AddIngredient(ItemID.Katana, 1);
                recipeZE.AddIngredient(ItemID.FalconBlade, 1);
                recipeZE.AddIngredient(ItemID.DyeTradersScimitar, 1);
                recipeZE.AddIngredient(ItemID.PearlwoodSword, 1);
                recipeZE.AddIngredient(ItemID.BreakerBlade, 1);
                recipeZE.AddIngredient(ItemID.ChlorophyteClaymore, 1);
                recipeZE.AddIngredient(ItemID.ChristmasTreeSword, 1);
                recipeZE.AddIngredient(ItemID.DD2SquireBetsySword, 1);
                recipeZE.AddIngredient(ItemID.PiercingStarlight, 1);
                recipeZE.AddIngredient(ItemID.LunarBar, 10);
                recipeZE.AddTile(TileID.LunarCraftingStation);
                recipeZE.Register();



                // 海盗入侵
                // 钱币枪
                Recipe recipeHD01 = Recipe.Create(ItemID.CoinGun);
                recipeHD01.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 5);
                recipeHD01.AddTile(TileID.Anvils);
                recipeHD01.Register();

                // 弯刀
                Recipe recipeHD02 = Recipe.Create(ItemID.Cutlass);
                recipeHD02.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD02.AddTile(TileID.Anvils);
                recipeHD02.Register();

                // 优惠卡
                Recipe recipeHD03 = Recipe.Create(ItemID.DiscountCard);
                recipeHD03.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD03.AddTile(TileID.Anvils);
                recipeHD03.Register();

                // 金戒指
                Recipe recipeHD04 = Recipe.Create(ItemID.GoldRing);
                recipeHD04.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD04.AddTile(TileID.Anvils);
                recipeHD04.Register();

                // 幸运硬币
                Recipe recipeHD05 = Recipe.Create(ItemID.LuckyCoin);
                recipeHD05.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD05.AddTile(TileID.Anvils);
                recipeHD05.Register();

                // 小海盗法杖
                Recipe recipeHD06 = Recipe.Create(ItemID.PirateStaff);
                recipeHD06.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD06.AddTile(TileID.Anvils);
                recipeHD06.Register();

                // 荷兰人坐骑
                Recipe recipeHD07 = Recipe.Create(ItemID.PirateShipMountItem);
                recipeHD07.AddIngredient(ItemID.FlyingDutchmanMasterTrophy, 1);
                recipeHD07.AddTile(TileID.Anvils);
                recipeHD07.Register();




                // 日食
                // Mothron Wings
                Recipe recipeRS01 = Recipe.Create(ItemID.MothronWings);
                recipeRS01.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS01.AddTile(TileID.MythrilAnvil);
                recipeRS01.Register();

                // The Eye of Cthulhu (Yoyo)
                Recipe recipeRS02 = Recipe.Create(ItemID.TheEyeOfCthulhu);
                recipeRS02.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS02.AddTile(TileID.MythrilAnvil);
                recipeRS02.Register();

                // Death Sickle
                Recipe recipeRS03 = Recipe.Create(ItemID.DeathSickle);
                recipeRS03.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS03.AddTile(TileID.MythrilAnvil);
                recipeRS03.Register();

                // Butcher's Chainsaw
                Recipe recipeRS04 = Recipe.Create(ItemID.ButchersChainsaw);
                recipeRS04.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS04.AddTile(TileID.MythrilAnvil);
                recipeRS04.Register();

                // Deadly Sphere Staff
                Recipe recipeRS05 = Recipe.Create(ItemID.DeadlySphereStaff);
                recipeRS05.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS05.AddTile(TileID.MythrilAnvil);
                recipeRS05.Register();

                // Toxic Flask
                Recipe recipeRS06 = Recipe.Create(ItemID.ToxicFlask);
                recipeRS06.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS06.AddTile(TileID.MythrilAnvil);
                recipeRS06.Register();

                // Psycho Knife
                Recipe recipeRS07 = Recipe.Create(ItemID.PsychoKnife);
                recipeRS07.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS07.AddTile(TileID.MythrilAnvil);
                recipeRS07.Register();

                // Nail Gun
                Recipe recipeRS08 = Recipe.Create(ItemID.NailGun);
                recipeRS08.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS08.AddTile(TileID.MythrilAnvil);
                recipeRS08.Register();

                // Moon Stone
                Recipe recipeRS09 = Recipe.Create(ItemID.MoonStone);
                recipeRS09.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS09.AddTile(TileID.MythrilAnvil);
                recipeRS09.Register();

                // Neptune's Shell
                Recipe recipeRS10 = Recipe.Create(ItemID.NeptunesShell);
                recipeRS10.AddIngredient(ItemID.BrokenHeroSword, 1);
                recipeRS10.AddTile(TileID.MythrilAnvil);
                recipeRS10.Register();




                // 地牢
                // 圣骑士锤
                Recipe recipeDL01 = Recipe.Create(ItemID.PaladinsHammer);
                recipeDL01.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL01.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL01.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL01.AddTile(TileID.MythrilAnvil);
                recipeDL01.Register();


                // 狙击步枪
                Recipe recipeDL02 = Recipe.Create(ItemID.SniperRifle);
                recipeDL02.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL02.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL02.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL02.AddTile(TileID.MythrilAnvil);
                recipeDL02.Register();


                // 幽魂法杖
                Recipe recipeDL03 = Recipe.Create(ItemID.SpectreStaff);
                recipeDL03.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL03.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL03.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL03.AddTile(TileID.MythrilAnvil);
                recipeDL03.Register();

                // 暗影射线
                Recipe recipeDL04 = Recipe.Create(ItemID.ShadowbeamStaff);
                recipeDL04.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL04.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL04.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL04.AddTile(TileID.MythrilAnvil);
                recipeDL04.Register();

                // 地狱火
                Recipe recipeDL05 = Recipe.Create(ItemID.InfernoFork);
                recipeDL05.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL05.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL05.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL05.AddTile(TileID.MythrilAnvil);
                recipeDL05.Register();

                // 磁球
                Recipe recipeDL06 = Recipe.Create(ItemID.MagnetSphere);
                recipeDL06.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL06.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL06.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL06.AddTile(TileID.MythrilAnvil);
                recipeDL06.Register();

                // 晨星
                Recipe recipeDL07 = Recipe.Create(ItemID.MaceWhip);
                recipeDL07.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL07.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL07.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL07.AddTile(TileID.MythrilAnvil);
                recipeDL07.Register();

                // 霰弹枪
                Recipe recipeDL08 = Recipe.Create(ItemID.TacticalShotgun);
                recipeDL08.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL08.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL08.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL08.AddTile(TileID.MythrilAnvil);
                recipeDL08.Register();

                // 钥匙
                Recipe recipeDL09 = Recipe.Create(ItemID.Keybrand);
                recipeDL09.AddIngredient(ItemID.Ectoplasm, 10);
                recipeDL09.AddIngredient(ItemID.SoulofLight, 10);
                recipeDL09.AddIngredient(ItemID.SoulofNight, 10);
                recipeDL09.AddTile(TileID.MythrilAnvil);
                recipeDL09.Register();

                // --------------------------地牢五神器
                // 吸血刀
                Recipe recipeSSQ01 = Recipe.Create(ItemID.VampireKnives);
                recipeSSQ01.AddIngredient(ItemID.CrimsonKey, 1);
                recipeSSQ01.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ01.AddTile(TileID.MythrilAnvil);
                recipeSSQ01.Register();

                // 腐化者长机
                Recipe recipeSSQ02 = Recipe.Create(ItemID.ScourgeoftheCorruptor);
                recipeSSQ02.AddIngredient(ItemID.CorruptionKey, 1);
                recipeSSQ02.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ02.AddTile(TileID.MythrilAnvil);
                recipeSSQ02.Register();

                // 九头龙法杖
                Recipe recipeSSQ04 = Recipe.Create(ItemID.StaffoftheFrostHydra);
                recipeSSQ04.AddIngredient(ItemID.FrozenKey, 1);
                recipeSSQ04.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ04.AddTile(TileID.MythrilAnvil);
                recipeSSQ04.Register();

                // 飞枪鱼
                Recipe recipeSSQ05 = Recipe.Create(ItemID.PiranhaGun);
                recipeSSQ05.AddIngredient(ItemID.JungleKey, 1);
                recipeSSQ05.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ05.AddTile(TileID.MythrilAnvil);
                recipeSSQ05.Register();

                // 沙漠虎
                Recipe recipeSSQ06 = Recipe.Create(ItemID.StormTigerStaff);
                recipeSSQ06.AddIngredient(ItemID.DungeonDesertKey, 1);
                recipeSSQ06.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ06.AddTile(TileID.MythrilAnvil);
                recipeSSQ06.Register();

                // 彩虹枪
                Recipe recipeSSQ07 = Recipe.Create(ItemID.RainbowGun);
                recipeSSQ07.AddIngredient(ItemID.HallowedKey, 1);
                recipeSSQ07.AddIngredient(ItemID.Ectoplasm, 5);
                recipeSSQ07.AddTile(TileID.MythrilAnvil);
                recipeSSQ07.Register();



                // 南瓜月
                // 哀木
                // 尖桩发射器
                Recipe recipeNG01 = Recipe.Create(ItemID.StakeLauncher);
                recipeNG01.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG01.AddTile(TileID.MythrilAnvil);
                recipeNG01.Register();

                // 死灵卷轴
                Recipe recipeNG02 = Recipe.Create(ItemID.NecromanticScroll);
                recipeNG02.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG02.AddTile(TileID.MythrilAnvil);
                recipeNG02.Register();

                // 阴森钩爪
                Recipe recipeNG03 = Recipe.Create(ItemID.SpookyHook);
                recipeNG03.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG03.AddTile(TileID.MythrilAnvil);
                recipeNG03.Register();

                // 阴森树脂
                Recipe recipeNG04 = Recipe.Create(ItemID.SpookyTwig);
                recipeNG04.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG04.AddTile(TileID.MythrilAnvil);
                recipeNG04.Register();

                // 扫帚坐骑
                Recipe recipeNG05 = Recipe.Create(ItemID.WitchBroom);
                recipeNG05.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG05.AddTile(TileID.MythrilAnvil);
                recipeNG05.Register();

                // 哀木坐骑
                Recipe recipeNG06 = Recipe.Create(ItemID.SpookyWoodMountItem);
                recipeNG06.AddIngredient(ItemID.MourningWoodMasterTrophy, 1);
                recipeNG06.AddTile(TileID.MythrilAnvil);
                recipeNG06.Register();

                // 南瓜王
                // The Horseman's Blade
                Recipe recipeNG07 = Recipe.Create(ItemID.TheHorsemansBlade);
                recipeNG07.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG07.AddTile(TileID.MythrilAnvil);
                recipeNG07.Register();

                // Raven Staff
                Recipe recipeNG08 = Recipe.Create(ItemID.RavenStaff);
                recipeNG08.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG08.AddTile(TileID.MythrilAnvil);
                recipeNG08.Register();

                // Bat Scepter
                Recipe recipeNG09 = Recipe.Create(ItemID.BatScepter);
                recipeNG09.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG09.AddTile(TileID.MythrilAnvil);
                recipeNG09.Register();

                // Candy Corn Rifle
                Recipe recipeNG10 = Recipe.Create(ItemID.CandyCornRifle);
                recipeNG10.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG10.AddTile(TileID.MythrilAnvil);
                recipeNG10.Register();

                // Jack 'O Lantern Launcher
                Recipe recipeNG11 = Recipe.Create(ItemID.JackOLanternLauncher);
                recipeNG11.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG11.AddTile(TileID.MythrilAnvil);
                recipeNG11.Register();

                // Black Fairy Dust
                Recipe recipeNG12 = Recipe.Create(ItemID.BlackFairyDust);
                recipeNG12.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG12.AddTile(TileID.MythrilAnvil);
                recipeNG12.Register();

                // Dark Harvest
                Recipe recipeNG13 = Recipe.Create(ItemID.ScytheWhip);
                recipeNG13.AddIngredient(ItemID.PumpkingMasterTrophy, 1);
                recipeNG13.AddTile(TileID.MythrilAnvil);
                recipeNG13.Register();


                // 寒霜月
                // 尖叫圣诞树的掉落物
                Recipe recipeSY01 = Recipe.Create(ItemID.ChristmasTreeSword);
                recipeSY01.AddIngredient(ItemID.EverscreamMasterTrophy, 1);
                recipeSY01.AddTile(TileID.MythrilAnvil);
                recipeSY01.Register();

                Recipe recipeSY02 = Recipe.Create(ItemID.Razorpine);
                recipeSY02.AddIngredient(ItemID.EverscreamMasterTrophy, 1);
                recipeSY02.AddTile(TileID.MythrilAnvil);
                recipeSY02.Register();

                Recipe recipeSY03 = Recipe.Create(ItemID.FestiveWings);
                recipeSY03.AddIngredient(ItemID.EverscreamMasterTrophy, 1);
                recipeSY03.AddTile(TileID.MythrilAnvil);
                recipeSY03.Register();

                Recipe recipeSY04 = Recipe.Create(ItemID.ChristmasHook);
                recipeSY04.AddIngredient(ItemID.EverscreamMasterTrophy, 1);
                recipeSY04.AddTile(TileID.MythrilAnvil);
                recipeSY04.Register();

                // 圣诞老人坦克的掉落物
                Recipe recipeSY05 = Recipe.Create(ItemID.ElfMelter);
                recipeSY05.AddIngredient(ItemID.SantankMasterTrophy, 1);
                recipeSY05.AddTile(TileID.MythrilAnvil);
                recipeSY05.Register();

                Recipe recipeSY06 = Recipe.Create(ItemID.ChainGun);
                recipeSY06.AddIngredient(ItemID.SantankMasterTrophy, 1);
                recipeSY06.AddTile(TileID.MythrilAnvil);
                recipeSY06.Register();

                Recipe recipeSY07 = Recipe.Create(ItemID.SantankMountItem);
                recipeSY07.AddIngredient(ItemID.SantankMasterTrophy, 1);
                recipeSY07.AddTile(TileID.MythrilAnvil);
                recipeSY07.Register();

                // 冰雪女王的掉落物
                Recipe recipeSY08 = Recipe.Create(ItemID.BlizzardStaff);
                recipeSY08.AddIngredient(ItemID.IceQueenMasterTrophy, 1);
                recipeSY08.AddTile(TileID.MythrilAnvil);
                recipeSY08.Register();

                Recipe recipeSY09 = Recipe.Create(ItemID.NorthPole);
                recipeSY09.AddIngredient(ItemID.IceQueenMasterTrophy, 1);
                recipeSY09.AddTile(TileID.MythrilAnvil);
                recipeSY09.Register();

                Recipe recipeSY10 = Recipe.Create(ItemID.SnowmanCannon);
                recipeSY10.AddIngredient(ItemID.IceQueenMasterTrophy, 1);
                recipeSY10.AddTile(TileID.MythrilAnvil);
                recipeSY10.Register();


                // 撒旦军团T3




                // 火星事件
                // 激光钻
                Recipe recipeHXR01 = Recipe.Create(ItemID.LaserDrill);
                recipeHXR01.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR01.AddTile(TileID.MythrilAnvil);
                recipeHXR01.Register();

                // 充能爆破加农炮
                Recipe recipeHXR02 = Recipe.Create(ItemID.ChargedBlasterCannon);
                recipeHXR02.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR02.AddTile(TileID.MythrilAnvil);
                recipeHXR02.Register();

                // 反重力钩爪
                Recipe recipeHXR03 = Recipe.Create(ItemID.AntiGravityHook);
                recipeHXR03.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR03.AddTile(TileID.MythrilAnvil);
                recipeHXR03.Register();

                // 脑波扰乱器
                Recipe recipeHXR04 = Recipe.Create(ItemID.BrainScrambler);
                recipeHXR04.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR04.AddTile(TileID.MythrilAnvil);
                recipeHXR04.Register();

                // 激光机枪
                Recipe recipeHXR05 = Recipe.Create(ItemID.LaserMachinegun);
                recipeHXR05.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR05.AddTile(TileID.MythrilAnvil);
                recipeHXR05.Register();

                // 异星泡波枪
                Recipe recipeHXR06 = Recipe.Create(ItemID.Xenopopper);
                recipeHXR06.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR06.AddTile(TileID.MythrilAnvil);
                recipeHXR06.Register();

                // 异星法杖
                Recipe recipeHXR07 = Recipe.Create(ItemID.XenoStaff);
                recipeHXR07.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR07.AddTile(TileID.MythrilAnvil);
                recipeHXR07.Register();

                // 宇宙飞车钥匙
                Recipe recipeHXR08 = Recipe.Create(ItemID.CosmicCarKey);
                recipeHXR08.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR08.AddTile(TileID.MythrilAnvil);
                recipeHXR08.Register();

                // 电球发射器
                Recipe recipeHXR09 = Recipe.Create(ItemID.ElectrosphereLauncher);
                recipeHXR09.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR09.AddTile(TileID.MythrilAnvil);
                recipeHXR09.Register();

                // 入侵波纹剑
                Recipe recipeHXR10 = Recipe.Create(ItemID.InfluxWaver);
                recipeHXR10.AddIngredient(ItemID.UFOMasterTrophy, 1);
                recipeHXR10.AddTile(TileID.MythrilAnvil);
                recipeHXR10.Register();



                // 月亮领主
                // 喵喵剑
                Recipe recipeML01 = Recipe.Create(ItemID.Meowmere);
                recipeML01.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML01.AddTile(TileID.MythrilAnvil);
                recipeML01.Register();

                // 泰拉利亚
                Recipe recipeML02 = Recipe.Create(ItemID.Terrarian);
                recipeML02.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML02.AddTile(TileID.MythrilAnvil);
                recipeML02.Register();

                // 星怒
                Recipe recipeML03 = Recipe.Create(ItemID.StarWrath);
                recipeML03.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML03.AddTile(TileID.MythrilAnvil);
                recipeML03.Register();

                // S.D.M.G.
                Recipe recipeML04 = Recipe.Create(ItemID.SDMG);
                recipeML04.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML04.AddTile(TileID.MythrilAnvil);
                recipeML04.Register();

                // 最后棱镜
                Recipe recipeML05 = Recipe.Create(ItemID.LastPrism);
                recipeML05.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML05.AddTile(TileID.MythrilAnvil);
                recipeML05.Register();

                // 月耀
                Recipe recipeML06 = Recipe.Create(ItemID.LunarFlareBook);
                recipeML06.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML06.AddTile(TileID.MythrilAnvil);
                recipeML06.Register();

                // 彩虹水晶
                Recipe recipeML07 = Recipe.Create(ItemID.RainbowCrystalStaff);
                recipeML07.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML07.AddTile(TileID.MythrilAnvil);
                recipeML07.Register();

                // 月亮传送门
                Recipe recipeML08 = Recipe.Create(ItemID.MoonlordTurretStaff);
                recipeML08.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML08.AddTile(TileID.MythrilAnvil);
                recipeML08.Register();

                // 火箭发射器
                Recipe recipeML09 = Recipe.Create(ItemID.Celeb2);
                recipeML09.AddIngredient(ItemID.MoonLordMasterTrophy, 1);
                recipeML09.AddTile(TileID.MythrilAnvil);
                recipeML09.Register();






                // 猪龙鱼公爵
                // 泡泡枪
                Recipe recipeZLYGJ01 = Recipe.Create(ItemID.BubbleGun);
                recipeZLYGJ01.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ01.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ01.Register();

                // 朱砂练球
                Recipe recipeZLYGJ02 = Recipe.Create(ItemID.Flairon);
                recipeZLYGJ02.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ02.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ02.Register();

                // 利刃台风
                Recipe recipeZLYGJ03 = Recipe.Create(ItemID.RazorbladeTyphoon);
                recipeZLYGJ03.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ03.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ03.Register();

                // 暴风雨法杖
                Recipe recipeZLYGJ04 = Recipe.Create(ItemID.TempestStaff);
                recipeZLYGJ04.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ04.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ04.Register();

                // 海啸弓
                Recipe recipeZLYGJ05 = Recipe.Create(ItemID.Tsunami);
                recipeZLYGJ05.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ05.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ05.Register();

                // 猪龙鱼之翼
                Recipe recipeZLYGJ06 = Recipe.Create(ItemID.FishronWings);
                recipeZLYGJ06.AddIngredient(ItemID.DukeFishronMasterTrophy, 1);
                recipeZLYGJ06.AddTile(TileID.MythrilAnvil);
                recipeZLYGJ06.Register();









                // 光之女皇
                // 夜光
                Recipe recipeGZNH01 = Recipe.Create(ItemID.FairyQueenMagicItem);
                recipeGZNH01.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH01.AddTile(TileID.MythrilAnvil);
                recipeGZNH01.Register();

                // 星光
                Recipe recipeGZNH02 = Recipe.Create(ItemID.PiercingStarlight);
                recipeGZNH02.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH02.AddTile(TileID.MythrilAnvil);
                recipeGZNH02.Register();

                // 万花筒
                Recipe recipeGZNH03 = Recipe.Create(ItemID.RainbowWhip);
                recipeGZNH03.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH03.AddTile(TileID.MythrilAnvil);
                recipeGZNH03.Register();

                // 日暮弓
                Recipe recipeGZNH04 = Recipe.Create(ItemID.FairyQueenRangedItem);
                recipeGZNH04.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH04.AddTile(TileID.MythrilAnvil);
                recipeGZNH04.Register();

                // 皇家飞翼
                Recipe recipeGZNH05 = Recipe.Create(ItemID.RainbowWings);
                recipeGZNH05.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH05.AddTile(TileID.MythrilAnvil);
                recipeGZNH05.Register();

                // 棱彩染料
                Recipe recipeGZNH06 = Recipe.Create(ItemID.HallowBossDye);
                recipeGZNH06.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH06.AddTile(TileID.MythrilAnvil);
                recipeGZNH06.Register();

                // 星光吉他
                Recipe recipeGZNH07 = Recipe.Create(ItemID.SparkleGuitar);
                recipeGZNH07.AddIngredient(ItemID.FairyQueenMasterTrophy, 10);
                recipeGZNH07.AddTile(TileID.MythrilAnvil);
                recipeGZNH07.Register();

                // 彩虹光标
                Recipe recipeGZNH08 = Recipe.Create(ItemID.RainbowCursor);
                recipeGZNH08.AddIngredient(ItemID.FairyQueenMasterTrophy, 1);
                recipeGZNH08.AddTile(TileID.MythrilAnvil);
                recipeGZNH08.Register();






                // 石巨人
                // 镐锯
                Recipe recipeSJR01 = Recipe.Create(ItemID.Picksaw);
                recipeSJR01.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR01.AddTile(TileID.MythrilAnvil);
                recipeSJR01.Register();

                // 尖刺发射器
                Recipe recipeSJR02 = Recipe.Create(ItemID.Stynger);
                recipeSJR02.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR02.AddTile(TileID.MythrilAnvil);
                recipeSJR02.Register();

                // 疯狂飞斧
                Recipe recipeSJR03 = Recipe.Create(ItemID.PossessedHatchet);
                recipeSJR03.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR03.AddTile(TileID.MythrilAnvil);
                recipeSJR03.Register();

                // 太阳石
                Recipe recipeSJR04 = Recipe.Create(ItemID.SunStone);
                recipeSJR04.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR04.AddTile(TileID.MythrilAnvil);
                recipeSJR04.Register();

                // 石巨人之眼
                Recipe recipeSJR05 = Recipe.Create(ItemID.EyeoftheGolem);
                recipeSJR05.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR05.AddTile(TileID.MythrilAnvil);
                recipeSJR05.Register();

                // 热射线
                Recipe recipeSJR06 = Recipe.Create(ItemID.HeatRay);
                recipeSJR06.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR06.AddTile(TileID.MythrilAnvil);
                recipeSJR06.Register();

                // 地之法杖
                Recipe recipeSJR07 = Recipe.Create(ItemID.StaffofEarth);
                recipeSJR07.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR07.AddTile(TileID.MythrilAnvil);
                recipeSJR07.Register();

                // 石巨人之拳
                Recipe recipeSJR08 = Recipe.Create(ItemID.GolemFist);
                recipeSJR08.AddIngredient(ItemID.GolemMasterTrophy, 1);
                recipeSJR08.AddTile(TileID.MythrilAnvil);
                recipeSJR08.Register();







                // 世纪之花
                // 丛林钥匙
                Recipe recipeSJZH01 = Recipe.Create(ItemID.TempleKey);
                recipeSJZH01.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH01.AddTile(TileID.MythrilAnvil);
                recipeSJZH01.Register();

                // 手弹发射器
                Recipe recipeSJZH02 = Recipe.Create(ItemID.GrenadeLauncher);
                recipeSJZH02.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH02.AddTile(TileID.MythrilAnvil);
                recipeSJZH02.Register();

                // 维纳斯枪
                Recipe recipeSJZH03 = Recipe.Create(ItemID.VenusMagnum);
                recipeSJZH03.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH03.AddTile(TileID.MythrilAnvil);
                recipeSJZH03.Register();

                // 荆棘藤
                Recipe recipeSJZH04 = Recipe.Create(ItemID.NettleBurst);
                recipeSJZH04.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH04.AddTile(TileID.MythrilAnvil);
                recipeSJZH04.Register();

                // 吹叶机
                Recipe recipeSJZH05 = Recipe.Create(ItemID.LeafBlower);
                recipeSJZH05.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH05.AddTile(TileID.MythrilAnvil);
                recipeSJZH05.Register();

                // 花之力
                Recipe recipeSJZH06 = Recipe.Create(ItemID.FlowerPow);
                recipeSJZH06.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH06.AddTile(TileID.MythrilAnvil);
                recipeSJZH06.Register();

                // 黄蜂机枪
                Recipe recipeSJZH07 = Recipe.Create(ItemID.WaspGun);
                recipeSJZH07.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH07.AddTile(TileID.MythrilAnvil);
                recipeSJZH07.Register();

                // 种子弯刀
                Recipe recipeSJZH08 = Recipe.Create(ItemID.Seedler);
                recipeSJZH08.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH08.AddTile(TileID.MythrilAnvil);
                recipeSJZH08.Register();

                // 矮人法杖
                Recipe recipeSJZH09 = Recipe.Create(ItemID.PygmyStaff);
                recipeSJZH09.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH09.AddTile(TileID.MythrilAnvil);
                recipeSJZH09.Register();

                // 常春藤钩爪
                Recipe recipeSJZH10 = Recipe.Create(ItemID.ThornHook);
                recipeSJZH10.AddIngredient(ItemID.PlanteraMasterTrophy, 1);
                recipeSJZH10.AddTile(TileID.MythrilAnvil);
                recipeSJZH10.Register();

                // 吉他
                Recipe recipeSJZH11 = Recipe.Create(ItemID.TheAxe);
                recipeSJZH11.AddIngredient(ItemID.PlanteraMasterTrophy, 10);
                recipeSJZH11.AddTile(TileID.MythrilAnvil);
                recipeSJZH11.Register();








                // 史莱姆皇后
                // 水晶刺客头盔
                Recipe recipeSLMHH01 = Recipe.Create(ItemID.CrystalNinjaHelmet);
                recipeSLMHH01.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH01.AddTile(TileID.Anvils);
                recipeSLMHH01.Register();
                // "水晶刺客头盔"

                // 水晶刺客衬衫
                Recipe recipeSLMHH02 = Recipe.Create(ItemID.CrystalNinjaChestplate);
                recipeSLMHH02.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH02.AddTile(TileID.Anvils);
                recipeSLMHH02.Register();
                // "水晶刺客衬衫"

                // 水晶刺客裤子
                Recipe recipeSLMHH03 = Recipe.Create(ItemID.CrystalNinjaLeggings);
                recipeSLMHH03.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH03.AddTile(TileID.Anvils);
                recipeSLMHH03.Register();
                // "水晶刺客裤子"

                // 刃之法杖
                Recipe recipeSLMHH04 = Recipe.Create(ItemID.Smolstar);
                recipeSLMHH04.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH04.AddTile(TileID.Anvils);
                recipeSLMHH04.Register();
                // "刃之法杖"

                // 胶质座骑
                Recipe recipeSLMHH05 = Recipe.Create(ItemID.QueenSlimeMountSaddle);
                recipeSLMHH05.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH05.AddTile(TileID.Anvils);
                recipeSLMHH05.Register();
                // "胶质座骑"

                // 不和谐钩爪
                Recipe recipeSLMHH06 = Recipe.Create(ItemID.QueenSlimeHook);
                recipeSLMHH06.AddIngredient(ItemID.QueenSlimeMasterTrophy, 1);
                recipeSLMHH06.AddTile(TileID.Anvils);
                recipeSLMHH06.Register();
                // "不和谐钩爪"











                // 肉山
                // 毁灭刃
                Recipe recipeXRQ01 = Recipe.Create(ItemID.BreakerBlade);
                recipeXRQ01.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ01.AddTile(TileID.Anvils);
                recipeXRQ01.Register();

                // 激光步枪
                Recipe recipeXRQ02 = Recipe.Create(ItemID.LaserRifle);
                recipeXRQ02.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ02.AddTile(TileID.Anvils);
                recipeXRQ02.Register();

                // 发条枪
                Recipe recipeXRQ022 = Recipe.Create(ItemID.ClockworkAssaultRifle);
                recipeXRQ022.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ022.AddTile(TileID.Anvils);
                recipeXRQ022.Register();

                // 鞭炮
                Recipe recipeXRQ03 = Recipe.Create(ItemID.FireWhip);
                recipeXRQ03.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ03.AddTile(TileID.Anvils);
                recipeXRQ03.Register();

                // 神圣锤
                Recipe recipeXRQ04 = Recipe.Create(ItemID.Pwnhammer);
                recipeXRQ04.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ04.AddTile(TileID.Anvils);
                recipeXRQ04.Register();

                // 战士徽章
                Recipe recipeXRQ05 = Recipe.Create(ItemID.WarriorEmblem);
                recipeXRQ05.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ05.AddTile(TileID.Anvils);
                recipeXRQ05.Register();

                // 射手徽章
                Recipe recipeXRQ06 = Recipe.Create(ItemID.RangerEmblem);
                recipeXRQ06.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ06.AddTile(TileID.Anvils);
                recipeXRQ06.Register();

                // 法师徽章
                Recipe recipeXRQ07 = Recipe.Create(ItemID.SorcererEmblem);
                recipeXRQ07.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ07.AddTile(TileID.Anvils);
                recipeXRQ07.Register();

                // 召唤师徽章
                Recipe recipeXRQ08 = Recipe.Create(ItemID.SummonerEmblem);
                recipeXRQ08.AddIngredient(ItemID.WallofFleshMasterTrophy, 1);
                recipeXRQ08.AddTile(TileID.Anvils);
                recipeXRQ08.Register();




                // 食人魔
                // 无限智慧宝典
                Recipe recipeSRM01 = Recipe.Create(ItemID.BookStaff);
                recipeSRM01.AddIngredient(ItemID.OgreMasterTrophy, 1);
                recipeSRM01.AddTile(TileID.MythrilAnvil);
                recipeSRM01.Register();

                // 幽灵凤凰
                Recipe recipeSRM02 = Recipe.Create(ItemID.DD2PhoenixBow);
                recipeSRM02.AddIngredient(ItemID.OgreMasterTrophy, 1);
                recipeSRM02.AddTile(TileID.MythrilAnvil);
                recipeSRM02.Register();

                // 地狱烙印
                Recipe recipeSRM03 = Recipe.Create(ItemID.DD2SquireDemonSword);
                recipeSRM03.AddIngredient(ItemID.OgreMasterTrophy, 1);
                recipeSRM03.AddTile(TileID.MythrilAnvil);
                recipeSRM03.Register();

                // 瞌睡章鱼
                Recipe recipeSRM04 = Recipe.Create(ItemID.MonkStaffT1);
                recipeSRM04.AddIngredient(ItemID.OgreMasterTrophy, 1);
                recipeSRM04.AddTile(TileID.MythrilAnvil);
                recipeSRM04.Register();

                // 恐怖关刀
                Recipe recipeSRM05 = Recipe.Create(ItemID.MonkStaffT2);
                recipeSRM05.AddIngredient(ItemID.OgreMasterTrophy, 1);
                recipeSRM05.AddTile(TileID.MythrilAnvil);
                recipeSRM05.Register();




                // 双足翼龙
                // 空中灾难
                Recipe recipeBTX01 = Recipe.Create(ItemID.DD2BetsyBow);
                recipeBTX01.AddIngredient(ItemID.BetsyMasterTrophy, 1);
                recipeBTX01.AddTile(TileID.MythrilAnvil);
                recipeBTX01.Register();

                // 双足翼龙怒气
                Recipe recipeBTX02 = Recipe.Create(ItemID.ApprenticeStaffT3);
                recipeBTX02.AddIngredient(ItemID.BetsyMasterTrophy, 1);
                recipeBTX02.AddTile(TileID.MythrilAnvil);
                recipeBTX02.Register();

                // 天龙之怒
                Recipe recipeBTX03 = Recipe.Create(ItemID.MonkStaffT3);
                recipeBTX03.AddIngredient(ItemID.BetsyMasterTrophy, 1);
                recipeBTX03.AddTile(TileID.MythrilAnvil);
                recipeBTX03.Register();

                // 飞龙
                Recipe recipeBTX04 = Recipe.Create(ItemID.DD2SquireBetsySword);
                recipeBTX04.AddIngredient(ItemID.BetsyMasterTrophy, 1);
                recipeBTX04.AddTile(TileID.MythrilAnvil);
                recipeBTX04.Register();



                // 克苏鲁之眼
                // 双筒望远镜
                Recipe recipeEOC01 = Recipe.Create(ItemID.Binoculars);
                recipeEOC01.AddIngredient(ItemID.EyeofCthulhuMasterTrophy, 1);
                recipeEOC01.AddTile(TileID.Anvils);
                recipeEOC01.Register();

                // 克苏鲁护盾
                Recipe recipeEOC02 = Recipe.Create(ItemID.EoCShield);
                recipeEOC02.AddIngredient(ItemID.EyeofCthulhuMasterTrophy, 1);
                recipeEOC02.AddTile(TileID.Anvils);
                recipeEOC02.Register();


                // 史莱姆王
                // 史莱姆坐骑
                Recipe recipeSLK01 = Recipe.Create(ItemID.SlimySaddle);
                recipeSLK01.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK01.AddTile(TileID.Anvils);
                recipeSLK01.Register();

                // 史莱姆钩爪
                Recipe recipeSLK02 = Recipe.Create(ItemID.SlimeHook);
                recipeSLK02.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK02.AddTile(TileID.Anvils);
                recipeSLK02.Register();

                // 史莱姆枪
                Recipe recipeSLK03 = Recipe.Create(ItemID.SlimeGun);
                recipeSLK03.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK03.AddTile(TileID.Anvils);
                recipeSLK03.Register();

                // 忍者头盔
                Recipe recipeSLK04 = Recipe.Create(ItemID.NinjaHood);
                recipeSLK04.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK04.AddTile(TileID.Anvils);
                recipeSLK04.Register();

                // 忍者衬衫
                Recipe recipeSLK05 = Recipe.Create(ItemID.NinjaShirt);
                recipeSLK05.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK05.AddTile(TileID.Anvils);
                recipeSLK05.Register();

                // 忍者裤子
                Recipe recipeSLK06 = Recipe.Create(ItemID.NinjaPants);
                recipeSLK06.AddIngredient(ItemID.KingSlimeMasterTrophy, 1);
                recipeSLK06.AddTile(TileID.Anvils);
                recipeSLK06.Register();








                // 蜂王
                // 蜜蜂帽
                Recipe recipeQB01 = Recipe.Create(ItemID.BeeHat);
                recipeQB01.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB01.AddTile(TileID.Anvils);
                recipeQB01.Register();

                // 蜜蜂衣
                Recipe recipeQB02 = Recipe.Create(ItemID.BeeShirt);
                recipeQB02.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB02.AddTile(TileID.Anvils);
                recipeQB02.Register();

                // 蜜蜂裤
                Recipe recipeQB03 = Recipe.Create(ItemID.BeePants);
                recipeQB03.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB03.AddTile(TileID.Anvils);
                recipeQB03.Register();

                // 蜂窝
                Recipe recipeQB04 = Recipe.Create(ItemID.HoneyComb);
                recipeQB04.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB04.AddTile(TileID.Anvils);
                recipeQB04.Register();

                // 蜂膝弓
                Recipe recipeQB05 = Recipe.Create(ItemID.BeesKnees);
                recipeQB05.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB05.AddTile(TileID.Anvils);
                recipeQB05.Register();

                // 养蜂人
                Recipe recipeQB06 = Recipe.Create(ItemID.BeeKeeper);
                recipeQB06.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB06.AddTile(TileID.Anvils);
                recipeQB06.Register();

                // 蜜蜂枪
                Recipe recipeQB07 = Recipe.Create(ItemID.BeeGun);
                recipeQB07.AddIngredient(ItemID.QueenBeeMasterTrophy, 1);
                recipeQB07.AddTile(TileID.Anvils);
                recipeQB07.Register();



                // 混沌法杖
                Recipe recipeRodofDiscord = Recipe.Create(ItemID.RodofDiscord);
                recipeRodofDiscord.AddIngredient(ItemID.SoulofLight, 20);
                recipeRodofDiscord.AddIngredient(ItemID.SoulofNight, 20);
                recipeRodofDiscord.AddTile(TileID.Anvils);
                recipeRodofDiscord.Register();




                // 普通宝箱怪
                // 双钩合成魔法匕首
                Recipe recipeMMKPT01 = Recipe.Create(ItemID.MagicDagger);
                recipeMMKPT01.AddIngredient(ItemID.DualHook, 1);
                recipeMMKPT01.AddTile(TileID.Anvils);
                recipeMMKPT01.Register();

                // 魔法匕首合成贤者之石
                Recipe recipeMMKPT02 = Recipe.Create(ItemID.PhilosophersStone);
                recipeMMKPT02.AddIngredient(ItemID.MagicDagger, 1);
                recipeMMKPT02.AddTile(TileID.Anvils);
                recipeMMKPT02.Register();

                // 贤者之石合成泰坦手套
                Recipe recipeMMKPT03 = Recipe.Create(ItemID.TitanGlove);
                recipeMMKPT03.AddIngredient(ItemID.PhilosophersStone, 1);
                recipeMMKPT03.AddTile(TileID.Anvils);
                recipeMMKPT03.Register();

                // 泰坦手套合成十字项链
                Recipe recipeMMKPT04 = Recipe.Create(ItemID.CrossNecklace);
                recipeMMKPT04.AddIngredient(ItemID.TitanGlove, 1);
                recipeMMKPT04.AddTile(TileID.Anvils);
                recipeMMKPT04.Register();

                // 十字项链合成星辰披风
                Recipe recipeMMKPT05 = Recipe.Create(ItemID.StarCloak);
                recipeMMKPT05.AddIngredient(ItemID.CrossNecklace, 1);
                recipeMMKPT05.AddTile(TileID.Anvils);
                recipeMMKPT05.Register();

                // 星辰披风合成双钩
                Recipe recipeMMKPT06 = Recipe.Create(ItemID.DualHook);
                recipeMMKPT06.AddIngredient(ItemID.StarCloak, 1);
                recipeMMKPT06.AddTile(TileID.Anvils);
                recipeMMKPT06.Register();



                // 冰雪宝箱怪
                // 玩具雪橇合成霜炎之剑
                Recipe recipeMMKBX01 = Recipe.Create(ItemID.Frostbrand);
                recipeMMKBX01.AddIngredient(ItemID.ToySled, 1);
                recipeMMKBX01.AddTile(TileID.Anvils);
                recipeMMKBX01.Register();

                // 霜炎之剑合成霜花
                Recipe recipeMMKBX02 = Recipe.Create(ItemID.FlowerofFrost);
                recipeMMKBX02.AddIngredient(ItemID.Frostbrand, 1);
                recipeMMKBX02.AddTile(TileID.Anvils);
                recipeMMKBX02.Register();

                // 霜花合成冰弓
                Recipe recipeMMKBX03 = Recipe.Create(ItemID.IceBow);
                recipeMMKBX03.AddIngredient(ItemID.FlowerofFrost, 1);
                recipeMMKBX03.AddTile(TileID.Anvils);
                recipeMMKBX03.Register();

                // 冰弓合成玩具雪橇
                Recipe recipeMMKBX04 = Recipe.Create(ItemID.ToySled);
                recipeMMKBX04.AddIngredient(ItemID.IceBow, 1);
                recipeMMKBX04.AddTile(TileID.Anvils);
                recipeMMKBX04.Register();





                // 腐化宝箱怪
                // 镖枪合成纠结之杖
                Recipe recipeDMMKFH01 = Recipe.Create(ItemID.ClingerStaff);
                recipeDMMKFH01.AddIngredient(ItemID.DartRifle, 1);
                recipeDMMKFH01.AddTile(TileID.Anvils);
                recipeDMMKFH01.Register();

                // 纠结之杖合成链锯刀
                Recipe recipeDMMKFH02 = Recipe.Create(ItemID.ChainGuillotines);
                recipeDMMKFH02.AddIngredient(ItemID.ClingerStaff, 1);
                recipeDMMKFH02.AddTile(TileID.Anvils);
                recipeDMMKFH02.Register();

                // 链锯刀合成腐臭香气
                Recipe recipeDMMKFH03 = Recipe.Create(ItemID.PutridScent);
                recipeDMMKFH03.AddIngredient(ItemID.ChainGuillotines, 1);
                recipeDMMKFH03.AddTile(TileID.Anvils);
                recipeDMMKFH03.Register();

                // 腐臭香气合成蠕虫钩
                Recipe recipeDMMKFH04 = Recipe.Create(ItemID.WormHook);
                recipeDMMKFH04.AddIngredient(ItemID.PutridScent, 1);
                recipeDMMKFH04.AddTile(TileID.Anvils);
                recipeDMMKFH04.Register();

                // 蠕虫钩合成镖枪
                Recipe recipeDMMKFH05 = Recipe.Create(ItemID.DartRifle);
                recipeDMMKFH05.AddIngredient(ItemID.WormHook, 1);
                recipeDMMKFH05.AddTile(TileID.Anvils);
                recipeDMMKFH05.Register();




                // 猩红宝箱怪
                // 镖枪手枪合成灵魂虹吸
                Recipe recipeDMMKXX01 = Recipe.Create(ItemID.SoulDrain);
                recipeDMMKXX01.AddIngredient(ItemID.DartPistol, 1);
                recipeDMMKXX01.AddTile(TileID.Anvils);
                recipeDMMKXX01.Register();

                // 灵魂虹吸合成恶臭双刺
                Recipe recipeDMMKXX02 = Recipe.Create(ItemID.FetidBaghnakhs);
                recipeDMMKXX02.AddIngredient(ItemID.SoulDrain, 1);
                recipeDMMKXX02.AddTile(TileID.Anvils);
                recipeDMMKXX02.Register();

                // 恶臭双刺合成血肉指节
                Recipe recipeDMMKXX03 = Recipe.Create(ItemID.FleshKnuckles);
                recipeDMMKXX03.AddIngredient(ItemID.FetidBaghnakhs, 1);
                recipeDMMKXX03.AddTile(TileID.Anvils);
                recipeDMMKXX03.Register();

                // 血肉指节合成腱钩
                Recipe recipeDMMKXX04 = Recipe.Create(ItemID.TendonHook);
                recipeDMMKXX04.AddIngredient(ItemID.FleshKnuckles, 1);
                recipeDMMKXX04.AddTile(TileID.Anvils);
                recipeDMMKXX04.Register();

                // 腱钩合成镖枪手枪
                Recipe recipeDMMKXX05 = Recipe.Create(ItemID.DartPistol);
                recipeDMMKXX05.AddIngredient(ItemID.TendonHook, 1);
                recipeDMMKXX05.AddTile(TileID.Anvils);
                recipeDMMKXX05.Register();




                // 神圣宝箱关
                // 代达罗斯风暴弓合成水晶邪恶碎片
                Recipe recipeDMMKSS01 = Recipe.Create(ItemID.CrystalVileShard);
                recipeDMMKSS01.AddIngredient(ItemID.DaedalusStormbow, 1);
                recipeDMMKSS01.AddTile(TileID.Anvils);
                recipeDMMKSS01.Register();

                // 水晶邪恶碎片合成飞刀
                Recipe recipeDMMKSS02 = Recipe.Create(ItemID.FlyingKnife);
                recipeDMMKSS02.AddIngredient(ItemID.CrystalVileShard, 1);
                recipeDMMKSS02.AddTile(TileID.Anvils);
                recipeDMMKSS02.Register();

                // 飞刀合成发光钩
                Recipe recipeDMMKSS03 = Recipe.Create(ItemID.IlluminantHook);
                recipeDMMKSS03.AddIngredient(ItemID.FlyingKnife, 1);
                recipeDMMKSS03.AddTile(TileID.Anvils);
                recipeDMMKSS03.Register();

                // 发光钩合成代达罗斯风暴弓
                Recipe recipeDMMKSS04 = Recipe.Create(ItemID.DaedalusStormbow);
                recipeDMMKSS04.AddIngredient(ItemID.IlluminantHook, 1);
                recipeDMMKSS04.AddTile(TileID.Anvils);
                recipeDMMKSS04.Register();


                // 两把小吉他
                // 常春藤
                Recipe recipeJiTa01 = Recipe.Create(ItemID.IvyGuitar);
                recipeJiTa01.AddIngredient(ItemID.SteampunkHat, 1);
                recipeJiTa01.AddIngredient(ItemID.SteampunkShirt, 1);
                recipeJiTa01.AddIngredient(ItemID.SteampunkPants, 1);
                recipeJiTa01.AddTile(TileID.Anvils);
                recipeJiTa01.Register();

                // 雨歌
                Recipe recipeJiTa02 = Recipe.Create(ItemID.CarbonGuitar);
                recipeJiTa02.AddIngredient(ItemID.Goldfish, 5);
                recipeJiTa02.AddIngredient(ItemID.Glowstick, 500);
                recipeJiTa02.AddTile(TileID.Anvils);
                recipeJiTa02.Register();






                // 灰色冲击波
                Recipe recipeCJB01 = Recipe.Create(ItemID.ZapinatorGray);
                recipeCJB01.AddIngredient(ItemID.SpaceGun, 1);
                recipeCJB01.AddIngredient(ItemID.Wire, 500);
                recipeCJB01.AddTile(TileID.Anvils);
                recipeCJB01.Register();


                // 红色冲击波
                Recipe recipeCJB02 = Recipe.Create(ItemID.ZapinatorOrange);
                recipeCJB02.AddIngredient(ItemID.ZapinatorGray, 1);
                recipeCJB02.AddIngredient(ItemID.Wire, 3000);
                recipeCJB02.AddTile(TileID.MythrilAnvil);
                recipeCJB02.Register();






                // 美队的盾牌
                Recipe recipeMDDD = Recipe.Create(ItemID.BouncingShield);
                recipeMDDD.AddIngredient(ItemID.CobaltShield, 1);
                recipeMDDD.AddIngredient(ItemID.SoulofLight, 3);
                recipeMDDD.AddIngredient(ItemID.SoulofNight, 3);
                recipeMDDD.AddTile(TileID.MythrilAnvil);
                recipeMDDD.Register();

                // 爱心雕像
                Recipe recipeAXDX = Recipe.Create(ItemID.HeartStatue);
                recipeAXDX.AddIngredient(ItemID.LifeCrystal, 1);
                recipeAXDX.AddIngredient(ItemID.StoneBlock, 50);
                recipeAXDX.AddTile(TileID.Anvils);
                recipeAXDX.Register();


                // 三叉戟
                Recipe recipeSCJ = Recipe.Create(ItemID.Trident);
                recipeSCJ.AddIngredient(ItemID.IronBar, 5);
                recipeSCJ.AddIngredient(ItemID.WaterBucket, 3);
                recipeSCJ.AddTile(TileID.Anvils);
                recipeSCJ.Register();

                // 邪恶三叉戟
                Recipe recipeXESCJ = Recipe.Create(ItemID.UnholyTrident);
                recipeXESCJ.AddIngredient(ItemID.Trident, 1);
                recipeXESCJ.AddIngredient(ItemID.InfernoPotion, 5);
                recipeXESCJ.AddIngredient(ItemID.HallowedBar, 5);
                recipeXESCJ.AddTile(TileID.MythrilAnvil);
                recipeXESCJ.Register();

                // 双筒望远镜
                Recipe recipeSTWYJ = Recipe.Create(ItemID.Binoculars);
                recipeSTWYJ.AddIngredient(ItemID.EoCShield, 1);
                recipeSTWYJ.AddIngredient(ItemID.Goggles, 1);
                recipeSTWYJ.AddIngredient(ItemID.Lens, 5);
                recipeSTWYJ.AddTile(TileID.Anvils);
                recipeSTWYJ.Register();


                // 左轮手枪
                Recipe recipeRevolver = Recipe.Create(ItemID.Revolver);
                recipeRevolver.AddIngredient(ItemID.FlintlockPistol, 1);
                recipeRevolver.AddIngredient(ItemID.MusketBall, 6);
                recipeRevolver.AddTile(TileID.Anvils);
                recipeRevolver.Register();






                // 茉莉花和玫瑰互转
                Recipe recipeJungleRose = Recipe.Create(ItemID.JungleRose);
                recipeJungleRose.AddIngredient(ItemID.NaturesGift, 1);
                recipeJungleRose.AddTile(TileID.Anvils);
                recipeJungleRose.Register();


                Recipe recipeNaturesGift = Recipe.Create(ItemID.NaturesGift);
                recipeNaturesGift.AddIngredient(ItemID.JungleRose, 1);
                recipeNaturesGift.AddTile(TileID.Anvils);
                recipeNaturesGift.Register();



            }


        }

    }
}
