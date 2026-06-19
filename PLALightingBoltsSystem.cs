using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Renderers;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire
{
    public class PLALightingBoltsSystem
    {
        // 五类粒子池
        private static ParticlePool<PrettySparkleParticle> _poolPretty = new(200, () => new PrettySparkleParticle());
        private static ParticlePool<FadingParticle> _poolFading = new(100, () => new FadingParticle());
        private static ParticlePool<FlameParticle> _poolFlame = new(100, () => new FlameParticle());

        // 1️⃣ PrettySparkleParticle 示例
        public static void PLA_Spawn_PrettySparkle(Vector2 position)
        {
            PrettySparkleParticle p = _poolPretty.RequestParticle();
            p.ColorTint = Color.Aqua;                  // 设置颜色
            p.Opacity = 1f;                            // 初始透明度（用于内部变化）
            p.AdditiveAmount = 0.35f;                  // 发光强度（0~1）
            p.FadeInNormalizedTime = 0.05f;            // 开始淡入的时间（0~1）
            p.FadeOutNormalizedTime = 0.9f;            // 进入淡出的时间（0~1）
            p.TimeToLive = 60f;                        // 存活帧数
            p.FadeInEnd = 15f;                         // 淡入结束帧
            p.FadeOutStart = 45f;                      // 淡出开始帧
            p.FadeOutEnd = 60f;                        // 淡出结束帧
            p.DrawHorizontalAxis = true;               // 是否横向绘制
            p.DrawVerticalAxis = true;                 // 是否纵向绘制
            p.LocalPosition = position;                // 初始位置
            p.Velocity = Vector2.Zero;                 // 速度（可省略）
            p.Scale = new Vector2(1.5f, 0.5f);          // 缩放

            Main.ParticleSystem_World_OverPlayers.Add(p);
        }

        // 2️⃣ FadingParticle 示例
        public static void PLA_Spawn_Fading(Vector2 position)
        {
            FadingParticle f = _poolFading.RequestParticle();
            f.SetBasicInfo(TextureAssets.Extra[174], null, Vector2.Zero, position); // 贴图、帧控制、原点、位置
            f.SetTypeInfo(40); // 存活时间（帧）

            f.ColorTint = new Color(1f, 0.6f, 0.2f, 1f);  // 颜色
            f.Scale = Vector2.One * 0.6f;                // 初始缩放
            f.ScaleVelocity = Vector2.One * 0.1f;        // 扩张速度
            f.ScaleAcceleration = -f.ScaleVelocity / 40f; // 减速收尾
            f.FadeInNormalizedTime = 0.1f;
            f.FadeOutNormalizedTime = 0.9f;

            Main.ParticleSystem_World_OverPlayers.Add(f);
        }

        // 3️⃣ FlameParticle 示例（正确版）
        public static void PLA_Spawn_Flame(Vector2 position)
        {
            // 创建粒子
            FlameParticle flame = _poolFlame.RequestParticle();

            // 贴图使用原版火焰粒子贴图（Extra[92] 是常见的火焰图）
            flame.SetBasicInfo(TextureAssets.Extra[92], null,
                initialVelocity: new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), -1.5f),
                initialLocalPosition: position
            );

            // 设置生命周期和额外参数
            int lifetime = 50;
            int owner = Main.myPlayer;
            int shader = 0; // 若你希望使用染色效果，可以填入染色 shader 索引

            flame.SetTypeInfo(timeToLive: lifetime, indexOfPlayerWhoSpawnedIt: owner, packedShaderIndex: shader);

            // 控制淡出时机（火焰粒子只支持淡出）
            flame.FadeOutNormalizedTime = 0.85f;

            // 添加到渲染系统
            Main.ParticleSystem_World_OverPlayers.Add(flame);
        }


        public static void PLA_Spawn_IMPWing(Vector2 position)
        {
            PrettySparkleParticle spark = _poolPretty.RequestParticle();

            spark.LocalPosition = position;

            // 更银蓝的颜色基调
            spark.ColorTint = Main.rand.NextBool()
                ? new Color(180, 220, 255) // 偏蓝银
                : new Color(200, 220, 230); // 柔和亮灰

            // 自旋效果（以初始角度 + 转速实现）
            spark.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            spark.RotationVelocity = Main.rand.NextFloat(-0.04f, 0.04f);

            // 缩放略带变化
            spark.Scale = new Vector2(1.6f, 0.6f) * Main.rand.NextFloat(0.9f, 1.2f);

            // 速度固定向下
            spark.Velocity = new Vector2(0f, Main.rand.NextFloat(0.5f, 1.2f));

            // 生命周期和淡入淡出设定
            spark.TimeToLive = 40;
            spark.FadeInNormalizedTime = 0.05f;
            spark.FadeOutNormalizedTime = 0.98f; // 更接近末尾才淡出

            spark.FadeInEnd = 10f;
            spark.FadeOutStart = 28f; // 更靠后开始消失
            spark.FadeOutEnd = 40f;

            // 亮度程度稍弱，避免晃眼
            spark.AdditiveAmount = 0.2f;

            Main.ParticleSystem_World_OverPlayers.Add(spark);
        }




        public static void PLA_Spawn_SuperCrit(Vector2 position, int index, int totalCount)
        {
            PrettySparkleParticle spark = _poolPretty.RequestParticle();

            spark.LocalPosition = position;

            // 色彩（原样保留）
            spark.ColorTint = Utils.SelectRandom(Main.rand,
                new Color(255, 80, 80),
                new Color(255, 220, 100),
                new Color(160, 255, 220),
                new Color(255, 150, 255));

            // 旋转 + 旋转速度（不变）
            spark.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            spark.RotationVelocity = Main.rand.NextFloat(-0.1f, 0.1f);

            // 缩放（不变）
            spark.Scale = new Vector2(Main.rand.NextFloat(2f, 5f), 0.5f);

            // 🧠 方向使用规则偏转角度形成螺旋
            float baseAngle = MathHelper.TwoPi * index / totalCount;         // 均匀分布
            float twist = index * 0.1f;                                       // 每个增加一点偏移形成旋涡
            Vector2 dir = (baseAngle + twist).ToRotationVector2();           // 偏转后的方向

            float speed = Main.rand.NextFloat(1.4f, 3.5f);                    // 原速不变
            spark.Velocity = dir * speed;

            // 生命周期
            spark.TimeToLive = 50;
            spark.FadeInNormalizedTime = 0.02f;
            spark.FadeOutNormalizedTime = 0.85f;
            spark.FadeInEnd = 10f;
            spark.FadeOutStart = 35f;
            spark.FadeOutEnd = 50f;
            spark.AdditiveAmount = 0.4f;

            Main.ParticleSystem_World_OverPlayers.Add(spark);
        }

        public static void PLA_Spawn_EchoSpiral(Vector2 center, Vector2 reflectedDirection)
        {
            const int particleCount = 40;
            const float spiralStep = 0.35f;
            const float spiralRadiusFactor = 4.5f;
            float angleOffset = reflectedDirection.ToRotation(); // 偏移到法线方向

            for (int i = 0; i < particleCount; i++)
            {
                float radius = spiralRadiusFactor * MathF.Sqrt(i);
                float angle = i * spiralStep;

                // 偏转角度 + 随机轻微扰动
                float actualAngle = angleOffset + angle + Main.rand.NextFloat(-0.15f, 0.15f);
                Vector2 offset = radius * actualAngle.ToRotationVector2();

                Vector2 velocity = offset.SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(1.6f, 3.2f);

                PrettySparkleParticle spark = _poolPretty.RequestParticle();
                spark.LocalPosition = center + offset;
                spark.Velocity = velocity;
                spark.ColorTint = new Color(180, 220, 255);
                spark.Scale = new Vector2(1.5f, 0.5f) * Main.rand.NextFloat(0.9f, 1.1f);
                spark.Rotation = velocity.ToRotation();
                spark.TimeToLive = 40;
                spark.FadeInNormalizedTime = 0.05f;
                spark.FadeOutNormalizedTime = 0.9f;
                spark.FadeInEnd = 10f;
                spark.FadeOutStart = 30f;
                spark.FadeOutEnd = 40f;
                spark.AdditiveAmount = 0.35f;
                Main.ParticleSystem_World_OverPlayers.Add(spark);
            }
        }

        public static void SpawnRainbowBurst_Lively(Vector2 center)
        {
            int count = 12;
            float ringRadius = 20f; // 半径为 2×10

            for (int i = 0; i < count; i++)
            {
                PrettySparkleParticle p = _poolPretty.RequestParticle();

                // 🌈 HSL 彩色
                float hue = Main.rand.NextFloat();
                p.ColorTint = Main.hslToRgb(hue, 1f, 0.65f);
                p.Opacity = 1f;
                p.AdditiveAmount = 0.5f;

                // 🌪️ 每个粒子分布在圆周上
                float angle = MathHelper.TwoPi * i / count + Main.rand.NextFloat(-0.1f, 0.1f); // 加点抖动
                Vector2 spawnOffset = angle.ToRotationVector2() * ringRadius;

                // ✨从圆环边缘往外射
                Vector2 velocity = spawnOffset.SafeNormalize(Vector2.UnitY) * Main.rand.NextFloat(2f, 3.2f);

                p.LocalPosition = center + spawnOffset;
                p.Velocity = velocity;

                // 🎵 淡入淡出节奏
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.9f;
                p.TimeToLive = Main.rand.Next(40, 52);
                p.FadeInEnd = (int)(p.TimeToLive * 0.25f);
                p.FadeOutStart = (int)(p.TimeToLive * 0.6f);
                p.FadeOutEnd = p.TimeToLive;

                // 📍方向与缩放
                p.Rotation = angle;
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                p.Scale = new Vector2(Main.rand.NextFloat(1.4f, 2f), Main.rand.NextFloat(0.5f, 0.9f));

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }


        public static void PLA_Spawn_MedicalPulse(Vector2 center)
        {
            float t = (float)Main.GameUpdateCount / 60f;

            // 🎵 波动旋转速度（主旋律）
            float rotationSpeed = 0.4f + 0.1f * (float)Math.Sin(t * 2f); // 0.3 ~ 0.5
            float baseAngle = t * MathHelper.TwoPi * rotationSpeed;

            for (int i = 0; i < 2; i++)
            {
                PrettySparkleParticle p = _poolPretty.RequestParticle();

                // 🌀 每对粒子在原角度基础上加入扰动
                float jitter = (float)Math.Sin(t * 3f + i) * 0.2f;
                float angle = baseAngle + MathHelper.Pi * i + jitter;

                // 🌊 半径扰动：在 9~12 像素范围内波动
                float radius = 10f + (float)Math.Sin(t * 4f + i) * 2f;
                Vector2 offset = angle.ToRotationVector2() * radius;

                p.LocalPosition = center + offset;
                p.Velocity = Vector2.Zero;

                // 💚 基础绿色调
                p.ColorTint = new Color(80, 255, 150);

                // 🔮 脉动式透明度（稍微有呼吸感）
                p.Opacity = 0.9f + 0.1f * (float)Math.Sin(t * 6f + i);
                p.AdditiveAmount = 0.5f;

                // ✨ 缩放轻抖动
                float scaleBase = 1f + 0.1f * (float)Math.Sin(t * 5f + i);
                p.Scale = new Vector2(scaleBase);

                p.Rotation = 0f;
                p.TimeToLive = 30;
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 8;
                p.FadeOutStart = 20;
                p.FadeOutEnd = 30;
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }



        public static void PLA_Spawn_HeartExplosionLine(Vector2 basePos)
        {
            for (int i = -32; i <= 32; i += 2)
            {
                Vector2 spawn = basePos + new Vector2(i, -6f); // 横向偏移
                Vector2 velocity = new Vector2(i * 0.08f, -Main.rand.NextFloat(1.6f, 2.4f)).RotatedByRandom(0.2); // 向上喷发+扰动

                PrettySparkleParticle p = _poolPretty.RequestParticle();
                p.LocalPosition = spawn;
                p.Velocity = velocity;
                p.ColorTint = new Color(255, 150, 180); // 粉色
                p.Opacity = 1f;
                p.AdditiveAmount = 0.55f;
                p.Scale = new Vector2(Main.rand.NextFloat(1.2f, 1.8f), 0.6f);
                p.Rotation = velocity.ToRotation();
                p.TimeToLive = 40;
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 8;
                p.FadeOutStart = 28;
                p.FadeOutEnd = 40;
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }


        public static void SpawnHealSparkles(Vector2 center)
        {
            for (int i = 0; i < 4; i++)
            {
                PrettySparkleParticle p = _poolPretty.RequestParticle();
                p.LocalPosition = center + Main.rand.NextVector2Circular(6f, 6f);
                p.Velocity = Vector2.Zero;
                p.ColorTint = new Color(255, 150, 200);
                p.Scale = new Vector2(1.2f);
                p.Opacity = 0f;
                p.FadeInNormalizedTime = 0.1f;
                p.FadeOutNormalizedTime = 0.1f;
                p.TimeToLive = 30;
                p.DrawVerticalAxis = true;
                p.DrawHorizontalAxis = true;
                p.Rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                p.FadeInEnd = 6;
                p.FadeOutStart = 18;
                p.FadeOutEnd = 30;
                p.AdditiveAmount = 0.5f;
                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }





        public static void PLA_Spawn_BloodAbsorption(Vector2 center)
        {
            float t = (float)Main.GameUpdateCount / 60f;

            for (int i = 0; i < 3; i++) // 每帧 3 个
            {
                PrettySparkleParticle p = _poolPretty.RequestParticle();

                // 🧠 用波动函数控制偏移
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float radius = 120f + (float)Math.Sin(t * 2f + i) * 20f;
                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 spawnPos = center + offset;

                // 🧲 朝向中心吸收 + 螺旋偏移
                Vector2 velocity = (center - spawnPos).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1.6f, 3.2f);
                velocity += offset.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * 0.4f;

                p.LocalPosition = spawnPos;
                p.Velocity = velocity;

                // 🔴 暗红 + 明红混合
                p.ColorTint = Main.rand.NextBool()
                    ? new Color(255, 60, 80)
                    : new Color(200, 30, 40);

                p.Scale = new Vector2(Main.rand.NextFloat(1.4f, 2.2f), 0.5f);
                p.Opacity = 1f;
                p.AdditiveAmount = 0.5f;
                p.TimeToLive = 50;
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 8;
                p.FadeOutStart = 30;
                p.FadeOutEnd = 50;

                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                p.Rotation = velocity.ToRotation();

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }




        // 💫 用于围绕圆环生成动态白色光点（用于回血立场特效）
        public static void PLA_Spawn_BloodRingWhiteSparkles(Vector2 center, float radius, int count = 12)
        {
            float t = (float)Main.GameUpdateCount / 60f;
            float baseAngle = t * MathHelper.TwoPi * 0.25f; // 环绕旋转

            for (int i = 0; i < count; i++)
            {
                float wave = (float)Math.Sin(t * 3f + i * 1.4f); // 波动扰动
                float angle = baseAngle + MathHelper.TwoPi * i / count + wave * 0.15f;
                Vector2 offset = angle.ToRotationVector2() * (radius + wave * 4f);

                PrettySparkleParticle p = _poolPretty.RequestParticle();
                p.LocalPosition = center + offset;
                p.Velocity = offset.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * 0.5f; // 顺时针绕圈
                p.ColorTint = Color.Lime;
                p.Scale = new Vector2(Main.rand.NextFloat(1.3f, 1.7f), 0.4f);
                p.Opacity = 0.9f;
                p.AdditiveAmount = 0.5f;
                p.Rotation = p.Velocity.ToRotation();
                p.TimeToLive = 40;
                p.FadeInNormalizedTime = 0.1f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 6;
                p.FadeOutStart = 28;
                p.FadeOutEnd = 40;
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }




        public static void SpawnRedSparkExplosion(Vector2 center)
        {
            for (int i = 0; i < 20; i++)
            {
                float baseAngle = MathHelper.TwoPi * i / 20f;
                Vector2 velocity = Vector2.UnitY.RotatedBy(baseAngle) * Main.rand.NextFloat(4f, 8f);

                PrettySparkleParticle p = _poolPretty.RequestParticle();

                // 初始位置略微向外偏移
                p.LocalPosition = center + velocity.SafeNormalize(Vector2.Zero) * 4f;
                p.Velocity = velocity;

                p.ColorTint = Color.Lerp(Color.OrangeRed, Color.Red, Main.rand.NextFloat(0.2f, 0.8f));
                p.Opacity = 1f;
                p.AdditiveAmount = 0.4f;

                // 拉长箭头感，纵向伸长，横向收缩
                p.Scale = new Vector2(0.35f, 2.2f);
                p.Rotation = velocity.ToRotation() - MathHelper.PiOver2; // 向上

                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 8f;
                p.FadeOutStart = 22f;
                p.FadeOutEnd = 30f;
                p.TimeToLive = 30f;

                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }



        public static void PLA_Spawn_PrettySparkle_YellowBurst(Vector2 position)
        {
            PrettySparkleParticle p = _poolPretty.RequestParticle();
            p.ColorTint = Color.Yellow;
            p.Opacity = 1f;
            p.AdditiveAmount = 0.45f;
            p.FadeInNormalizedTime = 0.05f;
            p.FadeOutNormalizedTime = 0.8f;
            p.TimeToLive = 50f;
            p.FadeInEnd = 10f;
            p.FadeOutStart = 30f;
            p.FadeOutEnd = 50f;
            p.DrawHorizontalAxis = true;
            p.DrawVerticalAxis = true;
            p.LocalPosition = position;
            p.Velocity = Vector2.Zero;
            p.Scale = new Vector2(1.8f, 0.5f).RotatedByRandom(0.5f);

            Main.ParticleSystem_World_OverPlayers.Add(p);
        }



        public static void PLA_SpawnTrigSparkle(Vector2 position, Vector2 velocity, float scale = 1f)
        {
            var p = _poolPretty.RequestParticle();
            p.LocalPosition = position;
            p.Velocity = velocity;
            p.ColorTint = Color.Lerp(Color.Cyan, Color.LightBlue, Main.rand.NextFloat(0.6f, 1f));
            p.Opacity = 1f;
            p.AdditiveAmount = 0.45f;

            p.Scale = new Vector2(scale, 0.5f).RotatedByRandom(0.3f);
            p.FadeInNormalizedTime = 0.1f;
            p.FadeOutNormalizedTime = 0.85f;
            p.FadeInEnd = 10f;
            p.FadeOutStart = 24f;
            p.FadeOutEnd = 30f;
            p.TimeToLive = 30f;

            p.DrawHorizontalAxis = true;
            p.DrawVerticalAxis = true;

            Main.ParticleSystem_World_OverPlayers.Add(p);
        }



        public static void SpawnMoonShieldSparkle(Vector2 center)
        {
            float time = Main.GlobalTimeWrappedHourly;

            // 🌀 圆环角度偏移（使整个圆环缓慢旋转）
            float angleOffset = time * 1.2f;

            // 🧿 每次生成一个粒子，随机角度扰动，整体构成一个缓慢螺旋外放的光点圆
            float angle = Main.rand.NextFloat(MathHelper.TwoPi) + angleOffset;
            Vector2 direction = angle.ToRotationVector2();

            // 🔄 半径扰动：形成“呼吸”波动感
            float radius = 62f + (float)Math.Sin(time * 4f + angle) * 4f;
            Vector2 spawnPos = center + direction * radius;

            // 🌟 光点速度向外扩散，并加入轻微随机扰动
            Vector2 velocity = direction * Main.rand.NextFloat(0.5f, 1.2f) + direction.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-0.3f, 0.3f);

            PrettySparkleParticle p = _poolPretty.RequestParticle();
            p.LocalPosition = spawnPos;
            p.Velocity = velocity;
            p.ColorTint = Color.Lerp(Color.White, Color.LightGray, Main.rand.NextFloat(0.3f, 0.7f));
            p.Opacity = 1f;
            p.AdditiveAmount = 0.35f;

            p.FadeInNormalizedTime = 0.1f;
            p.FadeOutNormalizedTime = 0.85f;
            p.FadeInEnd = 10f;
            p.FadeOutStart = 25f;
            p.FadeOutEnd = 35f;
            p.TimeToLive = 35f;

            p.DrawHorizontalAxis = true;
            p.DrawVerticalAxis = true;
            p.Scale = new Vector2(0.8f, 0.6f).RotatedByRandom(0.3f);

            Main.ParticleSystem_World_OverPlayers.Add(p);
        }




        public static void EMP_Spawn_RotatingBlueSparkles(Vector2 center, float radius, int count = 12)
        {
            float t = (float)Main.GameUpdateCount / 60f;
            float baseAngle = t * MathHelper.TwoPi * 0.4f;

            for (int i = 0; i < count; i++)
            {
                float angle = baseAngle + MathHelper.TwoPi * i / count;
                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 spawnPos = center + offset;

                PrettySparkleParticle p = _poolPretty.RequestParticle();
                p.LocalPosition = spawnPos;

                // 🌪️ 环绕飞行速度（带抖动）
                Vector2 velocity = offset.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.4f, 1.1f);
                p.Velocity = velocity;

                // 💠 淡蓝紫色调
                p.ColorTint = Color.Lerp(Color.Cyan, Color.LightBlue, Main.rand.NextFloat(0.4f, 0.8f));
                p.Opacity = 0.9f;
                p.AdditiveAmount = 0.45f;

                // 🔄 拉长 + 稍微旋转一点方向
                p.Scale = new Vector2(Main.rand.NextFloat(1.2f, 1.6f), 0.4f).RotatedByRandom(0.2f);
                p.Rotation = velocity.ToRotation();

                // ⏱️ 生命周期控制
                p.TimeToLive = Main.rand.Next(32, 45);
                p.FadeInNormalizedTime = 0.1f;
                p.FadeOutNormalizedTime = 0.85f;
                p.FadeInEnd = 8f;
                p.FadeOutStart = 26f;
                p.FadeOutEnd = p.TimeToLive;

                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }






        //  Emp系列的
        public static void PLA_SpawnTrigSparkle(Vector2 center, Color color)
        {
            float t = (float)Main.GameUpdateCount / 60f;

            // 📈 粒子总数
            int spiralCount = 80;
            int burstCount = 60;
            int chaosCount = 40;

            // 1️⃣ 螺旋环绕粒子（有序 + 柔和）
            for (int i = 0; i < spiralCount; i++)
            {
                float radius = 24f + 8f * (i % 5); // 层次半径
                float angle = t * 2f + MathHelper.TwoPi * i / spiralCount;
                Vector2 offset = angle.ToRotationVector2() * radius;

                var p = _poolPretty.RequestParticle();
                p.LocalPosition = center + offset;
                p.Velocity = offset.RotatedBy(MathHelper.PiOver2).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 2f);
                p.ColorTint = color;
                p.Scale = new Vector2(Main.rand.NextFloat(1f, 1.4f), 0.5f).RotatedByRandom(0.3f);
                p.Opacity = 1f;
                p.AdditiveAmount = 0.4f;
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.9f;
                p.TimeToLive = Main.rand.Next(36, 50);
                p.FadeInEnd = 8;
                p.FadeOutStart = 28;
                p.FadeOutEnd = p.TimeToLive;
                p.Rotation = p.Velocity.ToRotation();
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                Main.ParticleSystem_World_OverPlayers.Add(p);
            }

            // 2️⃣ 爆裂光锥粒子（放射 + 动感）
            for (int i = 0; i < burstCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(2f, 6f);

                var p = _poolPretty.RequestParticle();
                p.LocalPosition = center;
                p.Velocity = vel;
                p.ColorTint = color;
                p.Scale = new Vector2(0.5f, Main.rand.NextFloat(1.8f, 2.4f)).RotatedByRandom(0.2f);
                p.Opacity = 1f;
                p.AdditiveAmount = 0.5f;
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.TimeToLive = 30;
                p.FadeInEnd = 6;
                p.FadeOutStart = 20;
                p.FadeOutEnd = 30;
                p.Rotation = vel.ToRotation();
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                Main.ParticleSystem_World_OverPlayers.Add(p);
            }

            // 3️⃣ 混乱扰动粒子（细节 + 无序）
            for (int i = 0; i < chaosCount; i++)
            {
                Vector2 pos = center + Main.rand.NextVector2Circular(24f, 24f);
                Vector2 vel = Main.rand.NextVector2Circular(1.5f, 1.5f);

                var p = _poolPretty.RequestParticle();
                p.LocalPosition = pos;
                p.Velocity = vel;
                p.ColorTint = color;
                p.Scale = new Vector2(Main.rand.NextFloat(0.8f, 1.2f), 0.4f).RotatedByRandom(0.4f);
                p.Opacity = 0.9f;
                p.AdditiveAmount = 0.4f;
                p.FadeInNormalizedTime = 0.1f;
                p.FadeOutNormalizedTime = 0.85f;
                p.TimeToLive = Main.rand.Next(32, 46);
                p.FadeInEnd = 6;
                p.FadeOutStart = 24;
                p.FadeOutEnd = p.TimeToLive;
                p.Rotation = vel.ToRotation();
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }


        
        public static void PLA_Spawn_EarthHealBurst(Vector2 center)
        {
            // 🌱 医疗主题绿色爆发
            for (int i = 0; i < 16; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(3f, 3f);
                var p = _poolPretty.RequestParticle();
                p.LocalPosition = center;
                p.Velocity = velocity;
                p.ColorTint = Color.Lerp(new Color(80, 255, 150), new Color(150, 255, 200), Main.rand.NextFloat());
                p.Opacity = 1f;
                p.AdditiveAmount = 0.45f;
                p.Scale = new Vector2(Main.rand.NextFloat(1f, 1.4f), 0.5f).RotatedByRandom(0.2f);
                p.FadeInNormalizedTime = 0.05f;
                p.FadeOutNormalizedTime = 0.85f;
                p.TimeToLive = 40;
                p.FadeInEnd = 6;
                p.FadeOutStart = 28;
                p.FadeOutEnd = 40;
                p.Rotation = velocity.ToRotation();
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;
                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }



        public static void DrawCooldownVisual(Vector2 center, int radius)
        {
            float t = (float)Main.GameUpdateCount / 60f;
            int dustCount = 48;
            int spiralCount = 3;

            for (int i = 0; i < dustCount; i++)
            {
                float angle = MathHelper.TwoPi * i / dustCount + t * 0.8f;
                float wave = (float)Math.Sin(i * 0.5f + t * 2f) * 4f;

                Vector2 offset = angle.ToRotationVector2() * (radius * 16f + wave);

                // 💨 Dust：构成柔和波环
                Dust d = Dust.NewDustPerfect(center + offset, DustID.BlueTorch, Vector2.Zero, 100, Color.Cyan * 0.7f, 1.2f);
                d.noGravity = true;
                d.fadeIn = 1f;
            }

            // 🌌 光点环：由外向内旋转的神秘流线
            int sparkleCount = 24;
            for (int i = 0; i < sparkleCount; i++)
            {
                float spiralAngle = t * 1.5f + i * MathHelper.TwoPi / sparkleCount;
                float r = radius * 16f * (0.6f + 0.3f * (float)Math.Sin(t * 2f + i));

                Vector2 pos = center + spiralAngle.ToRotationVector2() * r;
                PLALightingBoltsSystem.PLA_SpawnTrigSparkle(pos, Vector2.Zero, 1f);
            }
        }



        private static int sparkleFrameCounter = 0;

        public static void PLA_Spawn_CrossColorSparkleBurst(Vector2 center, Vector2 direction)
        {
            int count = 6; // 每帧生成几个
            float radius = 5f * 16f;
            float rotationOffset = (sparkleFrameCounter++ % 360) / 360f * MathHelper.TwoPi; // 每帧推进一个角度偏移

            for (int i = 0; i < count; i++)
            {
                float angle = MathHelper.TwoPi * i / count + rotationOffset;
                Vector2 posOnCircle = center + angle.ToRotationVector2() * radius;

                // 顺时针切线方向（即角度 + 90度）
                Vector2 tangentDir = (angle + MathHelper.PiOver2).ToRotationVector2();

                // 主电束（白蓝混色）
                PrettySparkleParticle main = _poolPretty.RequestParticle();
                Vector2 velMain = tangentDir * Main.rand.NextFloat(1.6f, 2.3f);
                main.LocalPosition = posOnCircle;
                main.Velocity = velMain;
                main.ColorTint = Color.Lerp(Color.White, new Color(180, 220, 255), Main.rand.NextFloat(0.3f, 0.6f));
                main.Opacity = 1f;
                main.AdditiveAmount = 0.5f;
                main.Scale = new Vector2(Main.rand.NextFloat(1.4f, 1.8f), 0.4f);
                main.Rotation = velMain.ToRotation();
                main.TimeToLive = 30;
                main.FadeInNormalizedTime = 0.05f;
                main.FadeOutNormalizedTime = 0.85f;
                main.FadeInEnd = 6;
                main.FadeOutStart = 20;
                main.FadeOutEnd = 30;
                main.DrawHorizontalAxis = true;
                main.DrawVerticalAxis = true;
                Main.ParticleSystem_World_OverPlayers.Add(main);

                // 两个交叉小光点（银色）
                for (int j = -1; j <= 1; j += 2)
                {
                    PrettySparkleParticle cross = _poolPretty.RequestParticle();
                    Vector2 vel = tangentDir.RotatedBy(MathHelper.PiOver4 * j) * Main.rand.NextFloat(0.8f, 1.2f);
                    cross.LocalPosition = posOnCircle;
                    cross.Velocity = vel;
                    cross.ColorTint = Color.Silver;
                    cross.Opacity = 1f;
                    cross.AdditiveAmount = 0.35f;
                    cross.Scale = new Vector2(Main.rand.NextFloat(1.0f, 1.3f), 0.3f);
                    cross.Rotation = vel.ToRotation();
                    cross.TimeToLive = 24;
                    cross.FadeInNormalizedTime = 0.1f;
                    cross.FadeOutNormalizedTime = 0.8f;
                    cross.FadeInEnd = 4;
                    cross.FadeOutStart = 16;
                    cross.FadeOutEnd = 24;
                    cross.DrawHorizontalAxis = true;
                    cross.DrawVerticalAxis = true;
                    Main.ParticleSystem_World_OverPlayers.Add(cross);
                }
            }
        }


        // 太阳的特效（橙红色扇形前喷）
        // forward：作为“前方”的方向向量
        public static void PLA_SpawnSUNBurst(Vector2 center, Vector2 forward)
        {
            // 归一化前方方向，避免零向量
            forward = forward.LengthSquared() > 1e-4f ? Vector2.Normalize(forward) : Vector2.UnitX;

            // 扇形参数（±28°），总数 24 发
            float halfFan = MathHelper.ToRadians(28f);
            int count = 24;

            for (int i = 0; i < count; i++)
            {
                // 均匀分布 + 轻微随机
                float t = (i + 0.5f) / count;
                float ang = MathHelper.Lerp(-halfFan, halfFan, t) + Main.rand.NextFloat(-0.05f, 0.05f);
                Vector2 dir = forward.RotatedBy(ang);

                // 速度与初始位置
                float spd = Main.rand.NextFloat(2.4f, 3.6f);
                Vector2 vel = dir * spd;
                Vector2 pos = center + dir * Main.rand.NextFloat(2f, 6f);

                // 粒子：PrettySparkle（橙红色，发光适中）
                var p = _poolPretty.RequestParticle();
                p.LocalPosition = pos;
                p.Velocity = vel;

                // 太阳橙红渐变（暖而不刺眼）
                Color c1 = new Color(255, 170, 70);
                Color c2 = new Color(255, 95, 60);
                p.ColorTint = Color.Lerp(c1, c2, Main.rand.NextFloat());

                p.Opacity = 1f;
                p.AdditiveAmount = 0.50f;

                // ✅ 缩短拉伸：接近等比，仅轻微各向异性
                float s = Main.rand.NextFloat(0.75f, 1.15f);
                p.Scale = new Vector2(s * 1.05f, s * 0.95f);

                // 节奏：短促淡入，提前淡出
                p.FadeInNormalizedTime = 0.08f;
                p.FadeOutNormalizedTime = 0.80f;
                p.TimeToLive = Main.rand.Next(26, 34);
                p.FadeInEnd = 5;
                p.FadeOutStart = p.TimeToLive - 10;
                p.FadeOutEnd = p.TimeToLive;

                p.Rotation = vel.ToRotation();
                p.RotationVelocity = Main.rand.NextFloat(-0.04f, 0.04f); // 轻微自旋
                p.DrawHorizontalAxis = true;
                p.DrawVerticalAxis = true;

                Main.ParticleSystem_World_OverPlayers.Add(p);
            }
        }





        public static void PLA_Spawn_DualBlueSpiral(Vector2 center, float angle)
        {
            // 计算两个对立方向
            Vector2 dir1 = angle.ToRotationVector2();
            Vector2 dir2 = (angle + MathHelper.Pi).ToRotationVector2();

            // 速度
            float speed = Main.rand.NextFloat(1.8f, 3.2f);

            // ========= 第 1 个粒子 =========
            PrettySparkleParticle p1 = _poolPretty.RequestParticle();
            p1.LocalPosition = center;
            p1.Velocity = dir1 * speed;

            p1.ColorTint = Color.Lerp(Color.Cyan, Color.LightBlue, Main.rand.NextFloat(0.4f, 0.8f));
            p1.Opacity = 1f;
            p1.AdditiveAmount = 0.45f;

            p1.Scale = new Vector2(Main.rand.NextFloat(1.0f, 1.1f), 0.3f);
            p1.Rotation = dir1.ToRotation();
            p1.TimeToLive = Main.rand.Next(22, 36);

            p1.FadeInNormalizedTime = 0.1f;
            p1.FadeOutNormalizedTime = 0.85f;
            p1.FadeInEnd = 8f;
            p1.FadeOutStart = p1.TimeToLive - 10;
            p1.FadeOutEnd = p1.TimeToLive;

            p1.DrawHorizontalAxis = true;
            p1.DrawVerticalAxis = true;
            Main.ParticleSystem_World_OverPlayers.Add(p1);

            // ========= 第 2 个粒子（180° 对立方向）=========
            PrettySparkleParticle p2 = _poolPretty.RequestParticle();
            p2.LocalPosition = center;
            p2.Velocity = dir2 * speed;

            p2.ColorTint = Color.Lerp(Color.Cyan, Color.LightBlue, Main.rand.NextFloat(0.4f, 0.8f));
            p2.Opacity = 1f;
            p2.AdditiveAmount = 0.45f;

            p2.Scale = new Vector2(Main.rand.NextFloat(1.3f, 1.6f), 0.5f);
            p2.Rotation = dir2.ToRotation();
            p2.TimeToLive = p1.TimeToLive;

            p2.FadeInNormalizedTime = 0.1f;
            p2.FadeOutNormalizedTime = 0.85f;
            p2.FadeInEnd = 8f;
            p2.FadeOutStart = p2.TimeToLive - 10;
            p2.FadeOutEnd = p2.TimeToLive;

            p2.DrawHorizontalAxis = true;
            p2.DrawVerticalAxis = true;
            Main.ParticleSystem_World_OverPlayers.Add(p2);
        }









    }
}
