using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures; // IEntitySource的
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using System.Reflection;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class TrackingPlayerPROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private int Cooldown = 0;
        private float timeInOuterRing = 0f; // 在最外圈待的时间


        // 字典存储每个弹幕的旋转参数
        private Dictionary<int, (float radius, float speed, float initialAngle)> projectileData = new Dictionary<int, (float radius, float speed, float initialAngle)>();

        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (!projectileData.ContainsKey(projectile.whoAmI))
            {
                float initialRadius = Main.rand.NextFloat(50f, 50f); // 初始随机半径
                float initialSpeed = Main.rand.NextFloat(0.01f, 0.01f); // 初始随机速度
                float initialAngle = Main.rand.NextFloat(0f, MathHelper.TwoPi); // 初始角度
                projectileData[projectile.whoAmI] = (initialRadius, initialSpeed, initialAngle);
            }

            // 如果满足 hasUniversalgravitation 和 IsEligibleProjectile 条件，则设置持续时间
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                projectile.timeLeft = (int)(7.5f * 60f); // 设置弹幕的持续时间为 7.5 秒
            }
        }

        public override void AI(Projectile projectile)
        {
            Player player = Main.LocalPlayer;

            // 冷却计时器
            if (Cooldown > 0)
            {
                Cooldown--;
                return;
            }

            //// 检测弹幕是否距离玩家过远，并且符合职业和装备条件
            //if (player.getmodplayer<universalgravitationplayer>().hasuniversalgravitation && DamageClassUtils.IsEligibleProjectile(projectile))
            //{
            //    float distancefromplayer = vector2.distance(projectile.center, player.center);

            //    // 保留超出600像素销毁逻辑
            //    if (distancefromplayer >= 600f)
            //    {
            //        timeinouterring += 1f / 60f;

            //        if (timeinouterring >= 1f)
            //        {
            //            projectile.kill();
            //        }
            //    }
            //    else
            //    {
            //        timeinouterring = 0f;
            //    }
            //}

            // 保持绕玩家的公转逻辑
            if (player.GetModPlayer<UniversalgravitationPlayer>().hasUniversalgravitation && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                var data = projectileData[projectile.whoAmI];
                float radius = data.radius;
                float speed = data.speed;
                float initialAngle = data.initialAngle;

                // 找到距离最近的敌人并调整轨道
                NPC targetEnemy = FindClosestEnemy(player.Center, 1000f);  // 查找1000像素范围内的敌人

                if (targetEnemy != null)
                {
                    float targetDistance = Vector2.Distance(player.Center, targetEnemy.Center);
                    radius = MathHelper.Lerp(radius, targetDistance, 0.05f);  // 半径逐渐调整为玩家与敌人之间的距离
                }
                else
                {
                    // 如果没有找到敌人，维持原有的半径增大逻辑
                    radius = MathHelper.Clamp(radius + 5.0f, 50f, Main.rand.NextFloat(700f, 700f));
                }

                speed = MathHelper.Clamp(speed + 0.001f, 0.01f, Main.rand.NextFloat(0.1f, 0.1f));

                float angle = initialAngle + Main.GameUpdateCount * speed;
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

                projectile.Center = player.Center + offset;
                projectileData[projectile.whoAmI] = (radius, speed, initialAngle);
            }





            // 冷却计时器恢复
            Cooldown = 5;
        }

        private NPC FindClosestEnemy(Vector2 position, float maxDistance)
        {
            NPC closestEnemy = null;
            float closestDistance = maxDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage && !npc.immortal && npc.lifeMax > 5)
                {
                    float distance = Vector2.Distance(position, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestEnemy = npc;
                    }
                }
            }

            return closestEnemy;
        }





    }
}