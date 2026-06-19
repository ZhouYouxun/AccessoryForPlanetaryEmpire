using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MonoMod.RuntimeDetour;
using Terraria.ModLoader.Core;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using System.Reflection; // 引入反射命名空间


namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class AITrackingDevicePROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private int trackingCooldown = 0;
        private int timeWithoutHit = 0; // 无敌人命中的时间计时器

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;
            var modPlayer = player.GetModPlayer<AITrackingDevicePlayer>();

            if (trackingCooldown > 0)
            {
                trackingCooldown--;
                return;
            }

            if (modPlayer.hasAITrackingDevice && projectile.friendly && !projectile.hostile && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                {
                    float nearestDist = float.MaxValue;
                    NPC target = null;

                    foreach (NPC npc in Main.npc)
                    {
                        if (npc.active && !npc.friendly && !npc.dontTakeDamage && !npc.immortal && npc.lifeMax > 5)
                        {
                            float distance = Vector2.Distance(projectile.Center, npc.Center);
                            if (distance < nearestDist)
                            {
                                nearestDist = distance;
                                target = npc;
                            }
                        }
                    }

                    if (target != null && nearestDist < 350f)
                    {
                        Vector2 currentVelocity = projectile.velocity;
                        Vector2 desiredDirection = (target.Center - projectile.Center).SafeNormalize(Vector2.UnitY);

                        // 1️⃣ 获取当前和目标角度
                        float currentAngle = currentVelocity.ToRotation();
                        float targetAngle = desiredDirection.ToRotation();
                        float maxTurnAngle = MathHelper.ToRadians(9f); // 限制每帧转动最多 ?°

                        // 2️⃣ 插值角度：让其逐步转向目标
                        float newAngle = currentAngle.AngleTowards(targetAngle, maxTurnAngle);
                        projectile.velocity = currentVelocity.Length() * newAngle.ToRotationVector2();

                        timeWithoutHit = 0;
                    }
                    else
                    {
                        timeWithoutHit++; // 增加未命中敌人的时间计时器
                    }
                }
            }

            if (timeWithoutHit >= 90) // 如果计时器达到90帧（1.5秒），销毁弹幕
            {
                projectile.Kill();
            }

            trackingCooldown = 3; // 每3帧追踪一次
        }


        //private bool IsEligibleProjectile(Projectile projectile)
        //{
        //    // 默认的职业类型检测
        //    bool isEligible = projectile.DamageType == DamageClass.Melee ||
        //                      projectile.DamageType == DamageClass.Ranged ||
        //                      projectile.DamageType == DamageClass.Magic ||
        //                      projectile.DamageType == DamageClass.Throwing;

        //    // 尝试加载 CalamityMod 模组并检查 RogueDamageClass
        //    Mod calamityMod;
        //    if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
        //    {
        //        // 使用反射获取 RogueDamageClass 的实例
        //        var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");

        //        if (rogueType != null)
        //        {
        //            FieldInfo rogueInstanceField = rogueType.GetField("Instance", BindingFlags.NonPublic | BindingFlags.Static);
        //            var rogueInstance = rogueInstanceField?.GetValue(null) as DamageClass;
        //            if (rogueInstance != null && projectile.DamageType == rogueInstance)
        //            {
        //                isEligible = true; // 允许 Rogue 类型弹幕也能享受效果
        //            }
        //        }
        //    }

        //    return isEligible;
        //}



    }
}






