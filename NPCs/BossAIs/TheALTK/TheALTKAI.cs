using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;

namespace AccessoryForPlanetaryEmpire.NPCs.BossAIs.TheALTK
{
    public static class TheALTKAI
    {
        public static void AI(NPC npc)
        {
            Player player = Main.player[npc.target]; // 获取玩家目标
            float lifeRatio = npc.life / (float)npc.lifeMax; // 计算生命比例

            bool phase2 = lifeRatio < 0.75f; // 进入第二阶段的条件
            bool phase3 = lifeRatio < 0.35f; // 进入第三阶段的条件

            if (npc.ai[0] == 0f) // 初始化
            {
                npc.TargetClosest(true); // 锁定最近的玩家
                npc.ai[0] = 1f; // 标志 AI 已经初始化
                npc.ai[1] = 0f; // 用于不同攻击模式的阶段标志
            }

            // 弹幕炼狱阶段设置
            if (npc.ai[2] > 0f)
            {
                BulletHellPhase(npc, player); // 调用弹幕炼狱
                return;
            }

            if (npc.ai[1] == 0f) // 阶段1
            {
                HoverAndShoot(npc, player, ProjectileID.EyeBeam); // 悬停并发射 EyeBeam
                npc.ai[1] = 1f; // 切换至冲刺
            }
            else if (npc.ai[1] == 1f) // 冲刺阶段
            {
                DashAtPlayer(npc, player, phase2 ? 7 : 5); // 根据阶段确定冲刺次数
                npc.ai[1] = 0f; // 切换至悬停
            }

            // 如果血量进入第二阶段，触发弹幕炼狱
            if (phase2 && npc.ai[2] == 0f)
            {
                npc.ai[2] = 1f;
                npc.dontTakeDamage = true; // 进入弹幕炼狱，玩家无法对 boss 造成伤害
            }

            if (phase3 && npc.ai[2] == 1f)
            {
                npc.ai[2] = 2f;
                npc.dontTakeDamage = true; // 第二次弹幕炼狱
            }
        }

        public static void HoverAndShoot(NPC npc, Player player, int projectileType)
        {
            float hoverDistance = 480f; // 30 格的距离
            MoveToPosition(npc, player); // 先调用移动逻辑确保保持距离

            // 确保 boss 与玩家之间的距离不小于 hoverDistance
            if (Vector2.Distance(npc.Center, player.Center) >= hoverDistance)
            {
                // 引入一个计时器来控制 EyeBeam 的发射间隔
                if (npc.ai[3] > 0)
                {
                    npc.ai[3] -= 1f; // 每帧减少计时器
                }

                // 只有计时器为0时才能发射投射物，并重置计时器
                if (npc.ai[3] <= 0f)
                {
                    Vector2 direction = player.Center - npc.Center;
                    direction.Normalize();
                    Projectile.NewProjectile(npc.GetSource_FromAI(), npc.Center, direction * 10f, projectileType, npc.damage, 0f, Main.myPlayer);
                    //Main.PlaySound(SoundID.Item15, npc.Center);
                    npc.ai[3] = 42f; // 0.7秒的延迟，60帧每秒则是42帧
                }
            }
        }
        public static void DashAtPlayer(NPC npc, Player player, int dashes)
        {
            Vector2 direction = player.Center - npc.Center;
            direction.Normalize();
            npc.velocity = direction * 25f;

            for (int i = 0; i < dashes; i++)
            {
                if (npc.velocity.Length() > 0.1f)
                {
                    npc.velocity *= 0.9f;
                    Vector2 dashDirection = (player.Center - npc.Center).SafeNormalize(Vector2.Zero);
                    npc.velocity = dashDirection * 30f;
                }
            }
        }

        public static void BulletHellPhase(NPC npc, Player player)
        {
            // 弹幕炼狱逻辑：从屏幕上下左右发射火箭
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[2] == 1f) // 弹幕炼狱阶段1
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), player.position.X + Main.rand.Next(-1000, 1001), player.position.Y - 1000f, 0f, 5f, ProjectileID.RocketSkeleton, npc.damage, 0f, Main.myPlayer);
                }
                else if (npc.ai[2] == 2f) // 弹幕炼狱阶段2
                {
                    Projectile.NewProjectile(npc.GetSource_FromAI(), player.position.X + 1000f, player.position.Y + Main.rand.Next(-1000, 1001), -5f, 0f, ProjectileID.RocketSkeleton, npc.damage, 0f, Main.myPlayer);
                }
            }

            // 控制炼狱持续时间
            npc.ai[3] += 1f;
            if (npc.ai[3] >= 600f)
            {
                npc.ai[3] = 0f;
                npc.ai[2] = 0f; // 弹幕炼狱结束
                npc.dontTakeDamage = false; // 恢复受到伤害
            }
        }

        public static void MoveToPosition(NPC npc, Player player)
        {
            float hoverDistance = 480f; // 30 格的距离（480 像素）
            float speed = 6f; // 调整速度，使其不至于太快
            float acceleration = 0.15f; // 使用加速度来平滑移动

            // 计算悬停位置：玩家上方 hoverDistance 像素处
            Vector2 hoverPosition = player.Center + new Vector2(0, -hoverDistance) + new Vector2(Main.rand.Next(-100, 101), 0);

            Vector2 direction = hoverPosition - npc.Center;
            float distance = direction.Length(); // 计算目标位置与当前的位置之间的距离

            // 在离目标位置较远时加速，接近时减速
            if (distance > 50f) // 如果距离大于50像素，继续移动
            {
                direction.Normalize();
                npc.velocity = (npc.velocity * (1f - acceleration)) + (direction * speed * acceleration); // 使用加速度平滑移动
            }
            else
            {
                npc.velocity *= 0.95f; // 当接近目标位置时减速，逐渐停止
                if (npc.velocity.Length() < 0.1f)
                {
                    npc.velocity = Vector2.Zero; // 停止移动
                }
            }
        }
    }
}
