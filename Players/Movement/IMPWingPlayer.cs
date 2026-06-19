using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class IMPWingPlayer : ModPlayer
    {
        public bool hasIMPWing = false;   // 单击按键冲刺
        public bool hasIMPWingAD = false; // 双击方向键冲刺
        private bool isDashing = false;
        private int dashCooldown = 0;
        private int dashDuration = 15; // 冲刺持续时间
        private int dashSpeed = 18 * 3; // 冲刺速度大幅提升为原来的三倍
        private int invincibilityFrames = 60; // 冲刺期间无敌60帧

        public int DashDir = -1; // 用于双击方向键的方向检测

        public override void ResetEffects()
        {
            hasIMPWing = false;
            hasIMPWingAD = false;

            // 检测双击方向键
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15)
            {
                DashDir = 2; // 右
            }
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15)
            {
                DashDir = 3; // 左
            }
            else
            {
                DashDir = -1; // 重置方向
            }
        }

        public override void PreUpdateMovement()
        {
            // 双击方向键冲刺
            if (hasIMPWingAD && DashDir != -1 && dashCooldown == 0)
            {
                StartDash();
            }
            // 单击按键冲刺
            else if (hasIMPWing && KeybindSystem.DASHKeybind.JustPressed && dashCooldown == 0)
            {
                StartDash();
            }
            else if (isDashing)
            {
                PerformDash();
            }

            // 处理冷却时间
            if (dashCooldown > 0)
            {
                dashCooldown--;
            }
        }

        private void StartDash()
        {
            isDashing = true;
            dashCooldown = 60; // 冲刺冷却时间
            Player.immuneTime = invincibilityFrames; // 设置无敌时间

            // 如果当前有速度方向，保持当前方向并加速，否则默认向右冲刺
            if (Player.velocity.Length() > 0)
            {
                Player.velocity = Vector2.Normalize(Player.velocity) * dashSpeed; // 保持当前方向并加速
            }
            else
            {
                Player.velocity = Vector2.UnitX * dashSpeed; // 默认向右冲刺
            }
        }

        private void PerformDash()
        {
            if (dashDuration > 0)
            {
                Player.velocity *= 0.975f; // 冲刺期间速度逐渐减少
                dashDuration--;

                CreateDashEffect(); // 生成粒子特效
                CheckDashCollision(); // 检测冲刺碰撞
            }
            else
            {
                isDashing = false;
                dashDuration = 15; // 重置冲刺持续时间
            }
        }

        private void CreateDashEffect()
        {
            Vector2 position = Player.Center;

            // 动态缩小的粒子特效区域，根据速度变化
            float progress = 1f - (dashDuration / (float)15); // 随时间推移，缩小特效大小
            float radius = 20f * (1 - progress); // 根据冲刺速度变化的粒子半径
            int particleCount = 20; // 粒子数量

            // 生成一个合理的图形（圆形）粒子特效
            for (int i = 0; i < particleCount; i++)
            {
                float angle = MathHelper.TwoPi * i / particleCount; // 圆周角度
                Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

                Vector2 velocity = Player.velocity * 0.1f + offset; // 粒子速度略随机化
                Dust dust = Dust.NewDustPerfect(Player.Center + offset, DustID.Smoke, velocity);
                dust.noGravity = true;
                dust.scale = 1.5f * Main.rand.NextFloat(0.7f, 1.3f);
                dust.fadeIn = 0.8f;
            }
        }

        private void CheckDashCollision()
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.lifeMax > 5 && Player.getRect().Intersects(npc.getRect()))
                {
                    npc.StrikeNPC(new NPC.HitInfo
                    {
                        Damage = 350, // 冲刺伤害提升到350
                        Knockback = 10f,
                        HitDirection = Player.direction
                    });

                    for (int i = 0; i < 10; i++)
                    {
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.GoldFlame, npc.velocity.X * 0.5f, npc.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                    }
                }
            }
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return !isDashing; // 冲刺期间免疫敌人伤害
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (isDashing && Main.rand.NextFloat() < 0.7f) // 概率从50%提升到70%
            {
                proj.Kill(); // 70% 概率清除弹幕
                return false;
            }
            return !isDashing;
        }
    }
}
