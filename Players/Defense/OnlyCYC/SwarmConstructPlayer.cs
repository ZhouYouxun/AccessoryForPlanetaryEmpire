using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class SwarmConstructPlayer : ModPlayer
    {
        public bool hasSwarmConstruct;
        private int damageTaken = 0;
        private int healTimer = 0;

        public override void ResetEffects()
        {
            hasSwarmConstruct = false;
        }


        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            OnPlayerHit(hurtInfo.Damage);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            OnPlayerHit(hurtInfo.Damage);
        }

        private void OnPlayerHit(int damage)
        {
            if (hasSwarmConstruct)
            {
                // 这边可以正确的走道里面

                damageTaken = damage;

                healTimer = 300; // 5秒计时器 (60 ticks = 1秒)
                ReleaseHoneyParticles();
            }
        }

        public override void PostUpdate()
        {
            if (hasSwarmConstruct && healTimer > 0)
            {
                healTimer--;


                if (healTimer == 0 && damageTaken > 0)
                {

                    // 计算随机的恢复量
                    float healPercentage = Main.rand.Next(10, 71) / 100f;
                    int healAmount = (int)(damageTaken * healPercentage);

                    // 恢复生命值并显示绿色数字
                    Player.statLife += healAmount;
                    Player.HealEffect(healAmount, true);

                    // 释放绿色粒子特效
                    ShootGreenParticles();

                    // 重置damageTaken
                    damageTaken = 0;
                }
            }
        }

        private void ReleaseHoneyParticles()
        {
            int particleCount = Main.rand.Next(5, 15); // 随机数量的粒子
            for (int i = 0; i < particleCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(1f, 5f);
                Dust dust = Dust.NewDustPerfect(Player.Center, 152, velocity); // 152 是蜂蜜粒子
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(1f, 2f); // 随机大小
            }
        }
        private void ShootGreenParticles()
        {
            // ⭐ 使用蜂蜜粒子 DustID = 152
            int dustType = 152;

            Vector2 center = Player.Center;

            // 三层六边形的半径
            float[] radii = new float[] { 24f, 48f, 72f };

            foreach (float radius in radii)
            {
                // 六边形的 6 个顶点
                for (int i = 0; i < 6; i++)
                {
                    float angle = MathHelper.TwoPi / 6f * i;

                    // 基础位置（六边形角点）
                    Vector2 hexPos = center + angle.ToRotationVector2() * radius;

                    // 粒子向外爆裂
                    Vector2 velocity = (hexPos - center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3f, 7f);

                    // 释放粒子
                    Dust d = Dust.NewDustPerfect(hexPos, dustType, velocity);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.2f, 2.4f);
                    d.fadeIn = 1f;
                }

                // 六边形边中点（更细的结构）
                for (int i = 0; i < 6; i++)
                {
                    float angle1 = MathHelper.TwoPi / 6f * i;
                    float angle2 = MathHelper.TwoPi / 6f * (i + 1);

                    // 中点角度
                    float midAngle = (angle1 + angle2) * 0.5f;

                    Vector2 hexMid = center + midAngle.ToRotationVector2() * radius;

                    Vector2 velocity = (hexMid - center).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 6f);

                    Dust d = Dust.NewDustPerfect(hexMid, dustType, velocity);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(0.9f, 1.6f);
                    d.fadeIn = 1f;
                }
            }

            // 最终核心爆裂（让中心也有一点动感）
            for (int i = 0; i < 8; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(3f, 7f);
                Dust d = Dust.NewDustPerfect(center, dustType, velocity);
                d.noGravity = true;
                d.scale = Main.rand.NextFloat(1.5f, 2.8f);
                d.fadeIn = 1f;
            }
        }

    }

}
