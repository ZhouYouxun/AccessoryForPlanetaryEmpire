using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using AccessoryForPlanetaryEmpire.Global;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.GameInput;

namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    internal class SonicRingPlayer : ModPlayer
    {
        private int innerRingRadius = 48; // 3格方块（48像素）
        private int outerRingRadius = 160; // 10格方块（160像素）
        private int attackBonusStack = 0;
        private const int maxAttackBonus = 50;
        private int missCooldown = 60; // Miss后的一秒钟冷却（60帧）
        private int particleCooldown = 2; // 粒子生成的冷却时间为2帧
        private int particleCooldownTimer = 0; // 计时器，用于控制粒子生成
        public bool hasSonicRing; // 布尔开关

        public override void ResetEffects()
        {
            hasSonicRing = false; // 每次重置为 false，确保饰品不装备时无效
        }

        public override void PostUpdate()
        {
            if (!hasSonicRing) return; // 只有当 hasSonicRing 为 true 时才触发效果

            // Miss判定后的冷却计时器
            if (missCooldown > 0)
            {
                missCooldown--;
            }

            // 递减粒子生成冷却计时器
            if (particleCooldownTimer > 0)
            {
                particleCooldownTimer--;
            }

            // 只有当粒子冷却时间结束时才生成粒子
            if (particleCooldownTimer <= 0)
            {
                // 生成粒子效果的两个圆环，随机生成粒子
                GenerateRandomRing(Player.Center, innerRingRadius, Color.Yellow, true);
                GenerateRandomRing(Player.Center, outerRingRadius, Color.DarkBlue, true);

                // 重置粒子冷却计时器为2帧
                particleCooldownTimer = particleCooldown;
            }
        }

        private float rotationAngle = 0f;
        private const float rotationSpeed = MathHelper.TwoPi / 30f; // 旋转速度，每秒旋转两圈

        // 随机生成粒子特效
        private void GenerateRandomRing(Vector2 center, float radius, Color color, bool fastFade)
        {
            int particleAmount = 60; // 控制粒子数量
            rotationAngle += rotationSpeed; // 更新旋转角度
            if (rotationAngle > MathHelper.TwoPi)
            {
                rotationAngle -= MathHelper.TwoPi; // 保持角度在 0 到 2π 之间
            }

            // 随机位置生成大量粒子
            for (int i = 0; i < particleAmount; i++)
            {
                // 生成随机角度和随机半径
                float randomAngle = Main.rand.NextFloat(MathHelper.TwoPi);
                float randomRadius = radius * Main.rand.NextFloat(0.9f, 1.1f); // 偏离圆圈半径的随机范围

                // 计算粒子位置
                Vector2 particlePos = center + randomRadius * new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

                Dust dust = Dust.NewDustPerfect(particlePos, DustID.BlueTorch); // 使用蓝色火炬粒子
                dust.noGravity = true;
                dust.color = color;

                dust.velocity = Vector2.Zero; // 粒子保持静止
                dust.scale = 1.5f; // 调整粒子大小
                dust.fadeIn = fastFade ? 0.1f : 0.3f; // 更快的淡入淡出
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.customData = Main.rand.NextFloat(0.1f, 0.2f); // 增加旋转速度，使其在生成时拥有轻微旋转

                // 调整亮度
                dust.color = color * 8f; // 提高亮度
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!hasSonicRing || missCooldown > 0) return; // 在Miss的冷却期间，无法触发判定

            // 使用你提前定义的快捷键，移除冷却逻辑
            if (KeybindSystem.SRKeybind.JustPressed)
            {
                DetectProjectile(); // 每次按下快捷键都进行一次判定
            }
        }

        private bool DetectProjectile()
        {
            if (!hasSonicRing) return false; // 只有当 hasSonicRing 为 true 时才触发效果

            // 遍历所有弹幕
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.hostile)
                {
                    float distanceToProj = Vector2.Distance(Player.Center, proj.Center);

                    if (distanceToProj <= innerRingRadius) // 判定为 perfect
                    {
                        // 清除弹幕
                        proj.Kill();
                        attackBonusStack = Math.Min(attackBonusStack + 2, maxAttackBonus); // 叠加攻击力
                        CombatText.NewText(Player.getRect(), Color.Green, "Perfect!");
                        Player.GetDamage(DamageClass.Generic) += 0.02f; // 2%攻击力提升
                        SoundEngine.PlaySound(SoundID.Item4); // 播放清除弹幕的音效
                        return true;
                    }
                    else if (distanceToProj > innerRingRadius && distanceToProj <= outerRingRadius) // 判定为 great
                    {
                        // 清除弹幕
                        proj.Kill();
                        CombatText.NewText(Player.getRect(), Color.Yellow, "Great!");
                        SoundEngine.PlaySound(SoundID.Item4); // 播放清除弹幕的音效
                        return true;
                    }
                    else if (distanceToProj > outerRingRadius) // 判定为 miss
                    {
                        CombatText.NewText(Player.getRect(), Color.Red, "Miss!");
                        missCooldown = 60; // 进入1秒钟的冷却
                        return true;
                    }
                }
            }
            return false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (!hasSonicRing) return; // 只有当 hasSonicRing 为 true 时才触发效果

            // 受到伤害时，重置攻击力加成
            attackBonusStack = 0;
        }
    }
}
