using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.Audio;
using AccessoryForPlanetaryEmpire.Projectiles.Weapon;
using AccessoryForPlanetaryEmpire.Projectiles;

namespace AccessoryForPlanetaryEmpire.Weapons.Legendary
{
    public class Kong : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 25; // 初始伤害
            Item.DamageType = DamageClass.Ranged; // 伤害类型
            Item.width = 40; // 物品宽度
            Item.height = 20; // 物品高度
            Item.useTime = 20; // 使用时间
            Item.useAnimation = 20; // 使用动画时间
            Item.useStyle = ItemUseStyleID.Shoot; // 使用风格
            Item.noMelee = true; // 不是近战武器
            Item.knockBack = 4; // 击退
            Item.value = Item.buyPrice(0, 10, 0, 0); // 价值
            Item.rare = ItemRarityID.Green; // 稀有度
            Item.UseSound = SoundID.Item11; // 使用音效
            Item.autoReuse = true; // 自动连发
            Item.noUseGraphic = false; // 显示物品使用动画
            Item.channel = true; // 允许按住
            Item.scale = 1.3f; // 物品缩放
            Item.shoot = ModContent.ProjectileType<KongXiao>(); // 发射的手持弹幕
            Item.shootSpeed = 55f; // 弹幕速度
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2) // 右键
            {
                Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<KongDa>(), damage, knockback, player.whoAmI);
                //Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<TestPROJ>(), damage, knockback, player.whoAmI);
            }
            else // 左键
            {
                Projectile.NewProjectile(source, player.Center, velocity, ModContent.ProjectileType<KongXiao>(), damage, knockback, player.whoAmI);
            }
            return false;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        // 调整手持位置
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-10, 0); // 向后移动8像素
        }

        public override void AddRecipes() // 添加合成配方
        {
            Recipe recipe = CreateRecipe(); // 创建一个合成配方
            recipe.AddIngredient(ItemID.Wire, 15);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(TileID.Anvils); // 设置合成站类型
            recipe.Register(); // 注册合成配方
        }

        //// 增加击败Boss后伤害提升的逻辑
        //public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        //{
        //    bool kingSlime = NPC.downedSlimeKing;
        //    bool eyeOfCthulhu = NPC.downedBoss1;
        //    bool eaterOfWorlds = NPC.downedBoss2;
        //    bool brainOfCthulhu = NPC.downedBoss2;
        //    bool queenBee = NPC.downedQueenBee;
        //    bool skeletron = NPC.downedBoss3;
        //    bool wallOfFlesh = NPC.downedBoss3;
        //    bool theDestroyer = NPC.downedMechBoss1;
        //    bool skeletronPrime = NPC.downedMechBoss2;
        //    bool theTwins = NPC.downedMechBoss3;
        //    bool plantera = NPC.downedPlantBoss;
        //    bool golem = NPC.downedGolemBoss;
        //    bool dukeFishron = NPC.downedFishron;
        //    bool lunaticCultist = NPC.downedAncientCultist;
        //    bool moonLord = NPC.downedMoonlord;
        //    bool queenSlime = NPC.downedQueenSlime;
        //    bool empressOfLight = NPC.downedEmpressOfLight;
        //    bool deerclops = NPC.downedDeerclops;
        //    bool betsy = NPC.downedHalloweenTree;
        //    bool pumpking = NPC.downedHalloweenKing;
        //    bool iceQueen = NPC.downedChristmasIceQueen;

        //    float damageMult = 1f +
        //        (kingSlime ? 0.2f : 0f) +
        //        (eyeOfCthulhu ? 0.2f : 0f) +
        //        (eaterOfWorlds ? 0.2f : 0f) +
        //        (brainOfCthulhu ? 0.2f : 0f) +
        //        (queenBee ? 0.2f : 0f) +
        //        (skeletron ? 0.2f : 0f) +
        //        (wallOfFlesh ? 0.2f : 0f) +
        //        (theDestroyer ? 0.2f : 0f) +
        //        (skeletronPrime ? 0.2f : 0f) +
        //        (theTwins ? 0.2f : 0f) +
        //        (plantera ? 0.2f : 0f) +
        //        (golem ? 0.2f : 0f) +
        //        (dukeFishron ? 0.2f : 0f) +
        //        (lunaticCultist ? 0.2f : 0f) +
        //        (moonLord ? 0.2f : 0f) +
        //        (queenSlime ? 0.2f : 0f) +
        //        (empressOfLight ? 0.2f : 0f) +
        //        (deerclops ? 0.2f : 0f) +
        //        (betsy ? 0.2f : 0f) +
        //        (pumpking ? 0.2f : 0f) +
        //        (iceQueen ? 0.2f : 0f);

        //    damage *= damageMult;
        //}
    }
}




//public class KongProj : ModProjectile
//{
//    public override string Texture => "Terraria/Images/Projectile_0"; // 透明贴图

//    Player player => Main.player[Projectile.owner];

//    // 选择的弹幕ID数组，这些都是友方的各种常用的弹幕
//    private readonly int[] selectableProjectiles = new int[]
//    {
//            4, 6, 9, 12, 14, 16, 19, 30, 33, 36, 45, 79, 80, 85, 92, 93, 95, 106, 113, 116, 114, 132,
//            133, 134, 135, 139, 140, 141, 156, 157, 158, 159, 160, 162, 161, 166, 167, 168, 169, 170, 178, 182, 183, 190, 207,
//            228, 229, 274, 280, 283, 284, 285, 286, 287, 294, 295, 296, 297, 311, 312, 321, 323, 337, 357, 383, 409, 442, 451, 459, 477, 478,
//            479, 480, 502, 503, 510, 520, 521, 522, 532, 731, 732, 756, 611, 616, 636, 638, 639, 640, 642, 644, 728,
//            761, 762, 866, 933, 972, 973, 974, 975, 976, 977, 982, 983, 984, 985, 1000,
//            706, 709, 710, 711, 712, 715, 717, 917, 937, 483, 440, 408, 343, 304, 306, 301
//    };

//    public override void SetDefaults()
//    {
//        Projectile.width = Projectile.height = 32; // 碰撞箱大小
//        Projectile.friendly = true; // 友方弹幕
//        Projectile.tileCollide = false; // 穿墙
//        Projectile.timeLeft = 2; // 存在时间，每帧刷新
//        Projectile.aiStyle = -1; // 不使用原版AI
//        Projectile.DamageType = DamageClass.Ranged; // 远程
//        Projectile.penetrate = -1; // 无限穿透
//        Projectile.ignoreWater = true; // 无视液体
//    }

//    public override bool? CanDamage() => false; // 修正返回类型为 bool?

//    public override void AI()
//    {
//        if (player.channel)
//        {
//            player.direction = (Main.MouseWorld - player.Center).X > 0 ? 1 : -1;

//            if (player.direction == 1)
//            {
//                player.itemRotation = (Main.MouseWorld - player.Center).ToRotation();
//            }
//            else
//            {
//                player.itemRotation = (Main.MouseWorld - player.Center).ToRotation() + MathHelper.Pi;
//            }

//            Projectile.timeLeft = 2;
//            player.itemTime = player.itemAnimation = 5; // 攻击速度
//            Projectile.Center = player.Center;
//            Projectile.ai[0]++;

//            if (Projectile.ai[0] % 5 == 0) // 每5帧发射一次
//            {
//                int selectedProj = selectableProjectiles[Main.rand.Next(selectableProjectiles.Length)];
//                Vector2 velocity = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 16f;

//                float damageMult = CalculateDamageMultiplier(player);
//                int actualDamage = (int)(player.HeldItem.damage * damageMult);

//                // 创建弹幕
//                Projectile proj = Main.projectile[Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, velocity, selectedProj, actualDamage, player.HeldItem.knockBack, player.whoAmI)];

//                // 确保发射的弹幕是友方弹幕
//                if (!proj.friendly || proj.hostile)
//                {
//                    // 如果弹幕不是友方弹幕，重新选择一个弹幕
//                    selectedProj = selectableProjectiles[Main.rand.Next(selectableProjectiles.Length)];
//                    Projectile.NewProjectile(Projectile.GetSource_FromAI(), player.Center, velocity, selectedProj, actualDamage, player.HeldItem.knockBack, player.whoAmI);
//                }
//            }
//        }
//    }

//    private float CalculateDamageMultiplier(Player player)
//    {
//        bool kingSlime = NPC.downedSlimeKing;
//        bool eyeOfCthulhu = NPC.downedBoss1;
//        bool eaterOfWorlds = NPC.downedBoss2;
//        bool brainOfCthulhu = NPC.downedBoss2;
//        bool queenBee = NPC.downedQueenBee;
//        bool skeletron = NPC.downedBoss3;
//        bool wallOfFlesh = NPC.downedBoss3;
//        bool theDestroyer = NPC.downedMechBoss1;
//        bool skeletronPrime = NPC.downedMechBoss2;
//        bool theTwins = NPC.downedMechBoss3;
//        bool plantera = NPC.downedPlantBoss;
//        bool golem = NPC.downedGolemBoss;
//        bool dukeFishron = NPC.downedFishron;
//        bool lunaticCultist = NPC.downedAncientCultist;
//        bool moonLord = NPC.downedMoonlord;
//        bool queenSlime = NPC.downedQueenSlime;
//        bool empressOfLight = NPC.downedEmpressOfLight;
//        bool deerclops = NPC.downedDeerclops;
//        bool betsy = NPC.downedHalloweenTree;
//        bool pumpking = NPC.downedHalloweenKing;
//        bool iceQueen = NPC.downedChristmasIceQueen;

//        float damageMult = 1f +
//            (kingSlime ? 0.2f : 0f) +
//            (eyeOfCthulhu ? 0.2f : 0f) +
//            (eaterOfWorlds ? 0.2f : 0f) +
//            (brainOfCthulhu ? 0.2f : 0f) +
//            (queenBee ? 0.2f : 0f) +
//            (skeletron ? 0.2f : 0f) +
//            (wallOfFlesh ? 0.2f : 0f) +
//            (theDestroyer ? 0.2f : 0f) +
//            (skeletronPrime ? 0.2f : 0f) +
//            (theTwins ? 0.2f : 0f) +
//            (plantera ? 0.2f : 0f) +
//            (golem ? 0.2f : 0f) +
//            (dukeFishron ? 0.2f : 0f) +
//            (lunaticCultist ? 0.2f : 0f) +
//            (moonLord ? 0.2f : 0f) +
//            (queenSlime ? 0.2f : 0f) +
//            (empressOfLight ? 0.2f : 0f) +
//            (deerclops ? 0.2f : 0f) +
//            (betsy ? 0.2f : 0f) +
//            (pumpking ? 0.2f : 0f) +
//            (iceQueen ? 0.2f : 0f);

//        return damageMult;
//    }
//}










