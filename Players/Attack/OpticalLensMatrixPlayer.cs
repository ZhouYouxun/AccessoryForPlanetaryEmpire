using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Accessory.Attack;
using System.Reflection;
using AccessoryForPlanetaryEmpire.Projectiles.GlobalChange;
using Terraria.DataStructures;
using Terraria.ModLoader.Utilities;


namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    public class OpticalLensMatrixPlayer : ModPlayer
    {
        public bool hasOpticalLensMatrix;

        public override void ResetEffects()
        {
            hasOpticalLensMatrix = false;
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (hasOpticalLensMatrix && DamageClassUtils.IsEligiblePlayerClass(Player))
            {
                // 如果弹幕是召唤师职业，退出方法
                if (item.DamageType == DamageClass.Summon)
                {
                    return;
                }


                {
                    Player player = Player;
                    Vector2 baseDirection = velocity.SafeNormalize(Vector2.UnitX);

                    int spiralCount = Main.rand.Next(3, 6); // 发射数量
                    float spiralAmplitude = 10f;
                    float speedVariance = 0.15f;

                    // 🧮 第一步：计算正弦权重列表
                    float[] weights = new float[spiralCount];
                    float totalWeight = 0f;
                    for (int i = 0; i < spiralCount; i++)
                    {
                        // 使用三角函数分布：中间高、两边低
                        float angleRatio = (i + 1f) / (spiralCount + 1f);
                        weights[i] = (float)Math.Sin(MathHelper.Pi * angleRatio);
                        totalWeight += weights[i];
                    }

                    // 🌀 第二步：发射每一个副弹幕
                    for (int i = 0; i < spiralCount; i++)
                    {
                        float sineSeed = Main.GameUpdateCount + i * 10;
                        float angleOffset = (float)Math.Sin(sineSeed * 0.1f) * MathHelper.ToRadians(spiralAmplitude);
                        float speedFactor = Main.rand.NextFloat(1f - speedVariance, 1f + speedVariance);

                        Vector2 spiralVelocity = baseDirection.RotatedBy(angleOffset) * velocity.Length() * speedFactor;

                        // 📉 计算每发的柔和伤害值（总共为 45% 主弹幕伤害）
                        int splitDamage = (int)(damage * 0.45f * (weights[i] / totalWeight));

                        Projectile.NewProjectile(
                            player.GetSource_ItemUse(item),
                            position,
                            spiralVelocity,
                            type,
                            splitDamage,
                            knockback,
                            player.whoAmI
                        );
                    }
                }




            }
        }






    }
}



//namespace AccessoryForPlanetaryEmpire.Players.Attack
//{

//    public class OpticalLensMatrixGP : GlobalProjectile
//    {
//        public static bool[] HasSplit = new bool[Main.maxProjectiles]; // ✅ 自定义标记数组，避免使用 localAI

//        public override void AI(Projectile projectile)
//        {
//            Player owner = Main.player[projectile.owner];
//            if (!owner.active || owner.dead || !owner.GetModPlayer<OpticalLensMatrixPlayer>().hasOpticalLensMatrix)
//                return;

//            // ✅ 排除召唤物和敌对弹幕
//            if (!projectile.friendly || projectile.hostile || projectile.minion || projectile.sentry)
//                return;

//            // ✅ 名称中包含 hold 的弹幕直接排除（不区分大小写）
//            string name = projectile.ModProjectile?.Name ?? "";
//            if (name.ToLower().Contains("hold"))
//                return;
//        }




//        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
//        {
//            // ❌ 不满足条件则不触发（召唤弹幕等不参与）
//            //Player owner = Main.player[projectile.owner];
//            Player owner = Main.LocalPlayer;

//            if (!owner.active || owner.dead || !owner.GetModPlayer<OpticalLensMatrixPlayer>().hasOpticalLensMatrix)
//                return;
//            if (!projectile.friendly || projectile.hostile || projectile.minion || projectile.sentry)
//                return;

//            string name = projectile.ModProjectile?.Name ?? "";
//            if (name.ToLower().Contains("hold"))
//                return;

//            // ✅ 仅触发一次
//            if (HasSplit[projectile.whoAmI])
//                return;

//            HasSplit[projectile.whoAmI] = true;

//            // 🌀 分裂体从后方生成，向弹幕当前位置飞来
//            Vector2 back = -projectile.velocity.SafeNormalize(Vector2.UnitY); // 反向单位向量
//            Vector2 center = projectile.Center;
//            float radius = 64f; // 距离：4格
//            float speed = projectile.velocity.Length(); // 模拟“来袭”的速度

//            for (int i = 0; i < 4; i++)
//            {
//                // 每个复制体稍微有点偏移（形成扇形），增强视觉效果
//                float angleOffset = MathHelper.ToRadians(-15f + 10f * i);
//                Vector2 spawnPos = center + back.RotatedBy(angleOffset) * radius;
//                Vector2 dirToProjectile = (center - spawnPos).SafeNormalize(Vector2.UnitY);

//                int proj = Projectile.NewProjectile(
//                    owner.GetSource_FromThis(),
//                    spawnPos,
//                    dirToProjectile * speed,
//                    projectile.type,
//                    (int)(projectile.damage * 0.4f),
//                    projectile.knockBack,
//                    owner.whoAmI
//                );

//                if (proj >= 0 && proj < Main.maxProjectiles)
//                {
//                    Main.projectile[proj].timeLeft = (int)(projectile.timeLeft * 0.5f);
//                    HasSplit[proj] = true;
//                }
//            }
//        }









//        // ✅ 分裂发射函数，朝 4 个方向生成弱化弹幕
//        private void SpawnSplitProjectiles(Projectile projectile)
//        {
//            // 🧭 定义方向：左、右、左前、右前（所有单位向量已归一化）
//            Vector2[] directions = new Vector2[]
//            {
//    -Vector2.UnitX, // 左
//    Vector2.UnitX, // 右
//    new Vector2(-1f, 1f).SafeNormalize(Vector2.UnitY), // 左前
//    new Vector2(1f, 1f).SafeNormalize(Vector2.UnitY)   // 右前
//            };

//            float speed = projectile.velocity.Length();
//            Vector2 center = projectile.Center;
//            int ownerID = projectile.owner;

//            foreach (var dir in directions)
//            {
//                int proj = Projectile.NewProjectile(
//                    Main.player[ownerID].GetSource_FromThis(),
//                    center,
//                    dir * speed,
//                    projectile.type,
//                    (int)(projectile.damage * 0.4f), // ✅ 分裂体伤害为原来的40%
//                    projectile.knockBack,
//                    ownerID
//                );

//                if (proj >= 0 && proj < Main.maxProjectiles)
//                {
//                    Projectile p = Main.projectile[proj];
//                    p.timeLeft = (int)(projectile.timeLeft * 0.5f); // ✅ 分裂体存在时间较短
//                    OpticalLensMatrixGP.HasSplit[proj] = true; // ✅ 标记为分裂
//                }
//            }
//        }







//        // ✅ 内联版本的职业合法性判断（用于 OpticalLensMatrix）
//        private static bool IsEligiblePlayerClass(DamageClass damageClass)
//        {
//            bool isEligible = damageClass == DamageClass.Melee ||
//                              damageClass == DamageClass.Ranged ||
//                              damageClass == DamageClass.Magic ||
//                              damageClass == DamageClass.Throwing;

//            if (ModLoader.TryGetMod("CalamityMod", out Mod calamityMod))
//            {
//                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");
//                if (rogueType != null)
//                {
//                    FieldInfo rogueField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
//                    var rogueInstance = rogueField?.GetValue(null) as DamageClass;
//                    if (rogueInstance != null && damageClass == rogueInstance)
//                        isEligible = true;
//                }
//            }

//            return isEligible;
//        }
//    }
//}