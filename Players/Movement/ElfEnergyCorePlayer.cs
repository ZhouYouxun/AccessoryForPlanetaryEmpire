using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Accessory.Dash;

namespace AccessoryForPlanetaryEmpire.Players.Movement
{
    public class ElfEnergyCorePlayer : ModPlayer
    {
        public const int DashCooldown = 60; // 冲刺的冷却
        public const int DashDuration = 60; // 冲刺持续时间
        public const float DashVelocity = 15f;

        public bool hasElfEnergyCore = false;   // 使用特殊按键冲刺
        public bool hasElfEnergyCoreAD = false; // 使用双击方向键冲刺
        public bool hasQueensCrown = false;     // 新增，使用特殊按键冲刺
        public bool hasQueensCrownAD = false;   // 新增，使用双击方向键冲刺

        public int DashDir = -1;

        public int DashDelay = 0; // 冲刺的冷却时间
        public int DashTimer = 0; // 冲刺的持续时间

        private int dashParticleTimer = 0;
        private int invincibleTimer = 0;

        public override void ResetEffects()
        {
            hasElfEnergyCore = false;
            hasElfEnergyCoreAD = false;
            hasQueensCrown = false;   // 重置QueensCrown开关
            hasQueensCrownAD = false; // 重置QueensCrown双击开关

            // 双击左右键检测
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

            // 使用双击方向键冲刺
            if ((hasElfEnergyCoreAD || hasQueensCrownAD) && DashDir != -1 && DashDelay == 0)
            {
                Dash();
            }
            // 使用特殊按键冲刺
            else if ((hasElfEnergyCore || hasQueensCrown) && KeybindSystem.DASHKeybind.JustPressed && DashDelay == 0)
            {
                Dash();
            }

            // 粒子效果和无敌时间处理
            if (dashParticleTimer > 0)
            {
                dashParticleTimer--;
                if (hasElfEnergyCore || hasElfEnergyCoreAD)
                {
                    CreateElfDust(); // 绘制黄色粒子
                }
                else if (hasQueensCrown || hasQueensCrownAD)
                {
                    CreatePinkDust(); // 绘制粉红色粒子
                }
            }

            if (invincibleTimer > 0)
            {
                invincibleTimer--;
            }

            if (DashTimer > 0)
            {
                DashTimer--;
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;
            }
        }

        private void Dash()
        {
            Vector2 dashDirection = Player.velocity;

            // 保持当前的移动方向，不改变方向
            if (dashDirection != Vector2.Zero)
            {
                dashDirection.Normalize();
                float dashSpeed = hasQueensCrown || hasQueensCrownAD ? DashVelocity * 0.7f : DashVelocity; // 如果是QueensCrown，速度比ElfEnergyCore慢30%
                Player.velocity = dashDirection * dashSpeed; // 只加速，不改变方向
                DashTimer = DashDuration;
                DashDelay = DashCooldown; // 冷却时间为1秒
                dashParticleTimer = DashDuration; // 粒子生成时间为1秒
                invincibleTimer = 30; // 无敌时间为30帧
            }
        }

        // 创建黄色粒子特效
        private void CreateElfDust()
        {
            float progress = (DashDuration - dashParticleTimer) / (float)DashDuration;
            float radius = 10f * (1 - progress);
            float sizeMultiplier = 2.5f; // 粒子的大小倍数
            float brightnessMultiplier = 6f; // 粒子的亮度倍数

            // 生成正方形形状的粒子特效
            int particlesPerSide = 5;
            for (int i = 0; i < particlesPerSide; i++)
            {
                float offset = i - (particlesPerSide - 1) / 2f;

                // 上边
                CreateParticle(new Vector2(offset, -3.5f) * radius, sizeMultiplier, brightnessMultiplier, DustID.YellowTorch);
                // 下边
                CreateParticle(new Vector2(offset, 3.5f) * radius, sizeMultiplier, brightnessMultiplier, DustID.YellowTorch);
                // 左边
                CreateParticle(new Vector2(-3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier, DustID.YellowTorch);
                // 右边
                CreateParticle(new Vector2(3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier, DustID.YellowTorch);
            }
        }

        // 创建粉红色粒子特效
        private void CreatePinkDust()
        {
            float progress = (DashDuration - dashParticleTimer) / (float)DashDuration;
            float radius = 10f * (1 - progress);
            float sizeMultiplier = 2.5f; // 粒子的大小倍数
            float brightnessMultiplier = 6f; // 粒子的亮度倍数

            // 生成正方形形状的粒子特效
            int particlesPerSide = 5;
            for (int i = 0; i < particlesPerSide; i++)
            {
                float offset = i - (particlesPerSide - 1) / 2f;

                // 上边
                CreateParticle(new Vector2(offset, -3.5f) * radius, sizeMultiplier, brightnessMultiplier, DustID.PinkTorch);
                // 下边
                CreateParticle(new Vector2(offset, 3.5f) * radius, sizeMultiplier, brightnessMultiplier, DustID.PinkTorch);
                // 左边
                CreateParticle(new Vector2(-3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier, DustID.PinkTorch);
                // 右边
                CreateParticle(new Vector2(3.5f, offset) * radius, sizeMultiplier, brightnessMultiplier, DustID.PinkTorch);
            }
        }

        private void CreateParticle(Vector2 positionOffset, float sizeMultiplier, float brightnessMultiplier, int dustType)
        {
            Vector2 dustPosition = Player.Center + positionOffset;
            Dust dust = Dust.NewDustPerfect(dustPosition, dustType, null, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
            dust.noGravity = true;
            dust.fadeIn = 1f;
            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
            dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
            dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
        }

        public override bool CanBeHitByProjectile(Projectile proj)
        {
            if (invincibleTimer > 0)
            {
                if (Main.rand.NextFloat() < 0.2f)
                {
                    proj.Kill();
                }
                return false;
            }
            return true;
        }

        public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
        {
            if (invincibleTimer > 0)
            {
                return false;
            }
            return true;
        }
    }
}
