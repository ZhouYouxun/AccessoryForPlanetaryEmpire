using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using AccessoryForPlanetaryEmpire.Global;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class SweetheartCloakPlayer : ModPlayer
    {
        public const int DashCooldown = 48; // 冲刺的冷却（0.8秒）
        public const int DashDuration = 30; // 冲刺持续时间
        public const float DashVelocity = 11f; // 普通冲刺速度
        public const float StrongDashVelocity = 14.3f; // 强冲刺速度

        // 四个开关
        public bool hasSweetheartCloak = false;       // 普通冲刺 - 特殊按键触发
        public bool hasSweetheartCloakAD = false;     // 普通冲刺 - 双击方向键触发
        public bool hasEarthshaking = false;          // 强冲刺 - 特殊按键触发
        public bool hasEarthshakingAD = false;        // 强冲刺 - 双击方向键触发

        public int DashDir = -1;

        public int DashDelay = 0; // 冷却时间
        public int DashTimer = 0; // 冲刺持续时间
        private int dashParticleTimer = 0;
        private int invincibleTimer = 0;

        public override void ResetEffects()
        {
            // 重置所有冲刺效果
            hasSweetheartCloak = false;
            hasSweetheartCloakAD = false;
            hasEarthshaking = false;
            hasEarthshakingAD = false;

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

        public override void PostUpdate()
        {
            if (DashDelay > 0)
            {
                DashDelay--;
            }

            // 处理双击方向键触发的冲刺（强弱两种）
            if ((hasSweetheartCloakAD || hasEarthshakingAD) && DashDir != -1 && DashDelay == 0)
            {
                Dash(hasEarthshakingAD);
            }
            // 处理特殊按键触发的冲刺（强弱两种）
            else if ((hasSweetheartCloak || hasEarthshaking) && KeybindSystem.DASHKeybind.JustPressed && DashDelay == 0)
            {
                Dash(hasEarthshaking);
            }

            // 粒子效果和无敌时间处理
            if (dashParticleTimer > 0)
            {
                dashParticleTimer--;
                CreateYellowDust();
            }

            if (invincibleTimer > 0)
            {
                invincibleTimer--;
            }

            if (DashTimer > 0)
            {
                DashTimer--;
            }
        }

        private void Dash(bool isStrongDash)
        {
            Vector2 dashDirection = Player.velocity;

            // 如果玩家当前速度为零，不执行冲刺
            if (dashDirection != Vector2.Zero)
            {
                dashDirection.Normalize(); // 保持当前的移动方向
                float dashSpeed = isStrongDash ? StrongDashVelocity : DashVelocity; // 根据冲刺强度调整速度
                Player.velocity = dashDirection * dashSpeed; // 只加速，不改变方向
                DashTimer = DashDuration;
                DashDelay = DashCooldown; // 冷却时间为0.8秒
                dashParticleTimer = DashDuration; // 粒子生成时间为冲刺持续时间
                if (Main.rand.NextFloat() < (isStrongDash ? 0.45f : 0.25f)) // 强冲刺无敌概率为45%
                {
                    invincibleTimer = DashDuration;
                }
            }
        }

        private void CreateYellowDust()
        {
            float sizeMultiplier = 3.0f; // 粒子的大小倍数
            float brightnessMultiplier = 6f; // 粒子的亮度倍数
            float radius = 15f; // 粒子生成半径

            // 在玩家周围随机生成粒子特效
            int particleCount = 10; // 每次生成的粒子数量
            for (int i = 0; i < particleCount; i++)
            {
                Vector2 randomOffset = new Vector2(Main.rand.NextFloat(-radius, radius), Main.rand.NextFloat(-radius, radius));
                Vector2 position = Player.Center + randomOffset;
                CreateParticle(position, sizeMultiplier, brightnessMultiplier);
            }

            // 生成类似 DeepDiverDash 的粒子特效
            for (int m = 0; m < 5; m++)
            {
                Vector2 dustVel3 = -Player.velocity.RotatedBy(Main.rand.NextFloat(-0.3f, 0.3f)) * Main.rand.NextFloat(0.03f, 0.2f);
                Dust dust = Dust.NewDustPerfect(Player.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), Main.rand.NextFloat(-15f, 15f)) - (Player.velocity * 1.7f), DustID.YellowTorch, dustVel3);
                dust.scale = Main.rand.NextFloat(0.6f, 1.8f);
                dust.alpha = 125;
                dust.noGravity = true;
            }
        }

        private void CreateParticle(Vector2 position, float sizeMultiplier, float brightnessMultiplier)
        {
            Dust dust = Dust.NewDustPerfect(position, DustID.YellowTorch, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
            dust.noGravity = true;
            dust.fadeIn = 1f;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            return invincibleTimer <= 0;
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            return invincibleTimer <= 0;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (DashTimer > 0 && invincibleTimer == 0)
            {
                DashDelay = DashCooldown; // 设置冷却时间
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (DashTimer > 0 && invincibleTimer == 0)
            {
                DashDelay = DashCooldown; // 设置冷却时间
            }
        }
    }
}
