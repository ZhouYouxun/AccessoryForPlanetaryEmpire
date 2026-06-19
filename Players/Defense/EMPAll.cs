using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    internal class EMPAll
    {

        // 往范围内的每一个敌人发射一个超高速激光弹幕
        public static void FireEMPUse(Vector2 startPos, Vector2 targetPos)
        {
            Vector2 direction = (targetPos - startPos).SafeNormalize(Vector2.UnitY);
            Vector2 velocity = direction * 48f;
            int type = ModContent.ProjectileType<EMPUse>();

            for (int i = 0; i < 16; i++)
            {
                float angleOffset = MathHelper.Lerp(-0.4f, 0.4f, i / 15f);
                Vector2 offsetDir = direction.RotatedBy(angleOffset);
                Vector2 spawnPos = startPos + offsetDir * Main.rand.NextFloat(8f, 24f);

                Dust dust = Dust.NewDustPerfect(spawnPos, DustID.Electric);
                dust.velocity = offsetDir * Main.rand.NextFloat(1.5f, 4f);
                dust.scale = Main.rand.NextFloat(1.2f, 2.0f);
                dust.fadeIn = 1f;
                dust.noGravity = true;
                dust.color = Color.Lerp(Color.Cyan, Color.White, Main.rand.NextFloat());
            }

            Projectile.NewProjectile(
                Main.LocalPlayer.GetSource_FromThis(),
                startPos,
                velocity,
                type,
                0,
                0f,
                Main.LocalPlayer.whoAmI,
                targetPos.X,
                targetPos.Y
            );
        }



        // 统一过后的屏幕震动
        public static float GeneralShakePower = 0f;

        public static void TriggerScreenShake(float intensity)
        {
            GeneralShakePower = intensity;
            Main.OnPreDraw += UpdateScreenShake;
        }

        private static void UpdateScreenShake(GameTime gameTime)
        {
            if (GeneralShakePower > 0f)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(GeneralShakePower, GeneralShakePower);
                GeneralShakePower = MathHelper.Clamp(GeneralShakePower - 0.185f, 0f, 20f);
            }
            else
            {
                Main.OnPreDraw -= UpdateScreenShake;
            }
        }



        // 创建冷却结束的圆环包括光点
        // 自动内部声明角度和速度，不从外部传入
        private static float internalRotationAngle = 0f;
        private const float internalRotationSpeed = 0.07f;

        public static void CreateCooldownCircle(int radius)
        {
            Player player = Main.LocalPlayer; // 🔁 直接用本地玩家

            // ➰ 更新旋转角度（用内部变量）
            internalRotationAngle += internalRotationSpeed;
            if (internalRotationAngle > MathHelper.TwoPi)
                internalRotationAngle -= MathHelper.TwoPi;

            // 📍粒子分布角度（8个方向）
            float[] angles = {
        0f, MathHelper.PiOver4, MathHelper.PiOver2, 3 * MathHelper.PiOver4,
        MathHelper.Pi, 5 * MathHelper.PiOver4, 3 * MathHelper.PiOver2, 7 * MathHelper.PiOver4
    };

            foreach (float baseAngle in angles)
            {
                float angle = baseAngle + internalRotationAngle;
                Vector2 pos = player.Center + angle.ToRotationVector2() * radius * 16f;

                // 🔵 蓝色 Dust
                for (int j = 0; j < 6; j++)
                {
                    Dust dust = Dust.NewDustPerfect(pos, DustID.BlueTorch);
                    dust.noGravity = true;
                    dust.scale = 3f;
                    dust.fadeIn = 1.2f;
                    dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    dust.velocity = Vector2.Zero;
                    dust.customData = Main.rand.NextFloat(0.05f, 0.15f);
                    dust.color = Color.White * 6f;
                }

                // ✨扰动粒子
                for (int k = 0; k < 2; k++)
                {
                    Vector2 jitter = Main.rand.NextVector2Circular(6f, 6f);
                    PLALightingBoltsSystem.PLA_SpawnTrigSparkle(pos + jitter, Vector2.Zero, 1.3f);
                }
            }

            // 🌪️ 动态光点环绕
            PLALightingBoltsSystem.EMP_Spawn_RotatingBlueSparkles(player.Center, radius * 16f, 14);
        }





        // 冷却完成时的一次性特效
        public static void ShootInterceptEffect(Vector2 center, Color baseColor)
        {
            // 🌈 控制整体数量
            int spiralLayers = 6;
            int particlesPerLayer = 36;
            float maxRadius = 96f;

            // 🌪️ 彩虹旋转层（多个旋转圈）
            for (int layer = 0; layer < spiralLayers; layer++)
            {
                float angleOffset = Main.rand.NextFloat(MathHelper.TwoPi);
                float radius = maxRadius * (layer + 1) / spiralLayers;

                for (int i = 0; i < particlesPerLayer; i++)
                {
                    float angle = MathHelper.TwoPi * i / particlesPerLayer + angleOffset;
                    Vector2 offset = angle.ToRotationVector2() * radius;
                    Vector2 pos = center + offset;
                    Vector2 velocity = offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.5f, 2f);

                    Dust dust = Dust.NewDustPerfect(pos, DustID.RainbowTorch, velocity);
                    dust.noGravity = true;
                    dust.scale = Main.rand.NextFloat(1.2f, 2.4f);
                    dust.fadeIn = 1f;
                    dust.color = baseColor;
                }
            }

            // 💥 中心爆炸层（高密度）
            for (int i = 0; i < 80; i++)
            {
                Vector2 velocity = Main.rand.NextVector2CircularEdge(1f, 1f) * Main.rand.NextFloat(6f, 14f);
                Dust dust = Dust.NewDustPerfect(center, DustID.GemDiamond, velocity);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(2f, 3.5f);
                dust.fadeIn = 1.1f;
                dust.color = baseColor * Main.rand.NextFloat(0.5f, 1.2f);
            }

            // ✨ 发射大量光点（调用你定义好的光点函数）
            for (int i = 0; i < 36; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(4f, 4f);
                float scale = Main.rand.NextFloat(0.9f, 1.6f);
                PLALightingBoltsSystem.PLA_SpawnTrigSparkle(center, baseColor);
            }

            // 🎇 环形爆裂（白色主色）
            for (int i = 0; i < 24; i++)
            {
                float angle = MathHelper.TwoPi * i / 24f;
                Vector2 dir = angle.ToRotationVector2() * 12f;
                Dust d = Dust.NewDustPerfect(center, DustID.GemDiamond, dir);
                d.scale = 2.5f;
                d.noGravity = true;
                d.fadeIn = 1.5f;
                d.color = baseColor * 1.2f;
            }
        }







    }
}
