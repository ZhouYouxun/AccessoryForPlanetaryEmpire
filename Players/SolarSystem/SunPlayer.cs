using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class SunPlayer : ModPlayer
    {
        public bool hasSunAccessory = false;
        public int sunCooldown = 0; // 冷却计时器（帧）
        public bool GuangHuan = false;

        // ——新增：连射调度字段（不使用 localAI）——
        private bool sunVolleyActive;        // 是否处于一轮连射中
        private int sunVolleyRound;          // 已发出的轮次 0..4
        private int sunVolleyTimer;          // 两轮之间的帧间隔
        private Vector2 sunVolleyOrigin;     // 发射原点（太阳位置）
        private const int VolleyInterval = 4;       // 两轮间隔 X 帧
        private const float SunProjSpeed = 6f;     // SunProj2 初速

        public override void ResetEffects()
        {
            hasSunAccessory = false;
            GuangHuan = false;
        }

        public override void PostUpdate()
        {
            if (!hasSunAccessory)
                return;

            ManageSunProjectile();

            // ——冷却：10秒（600帧）触发一次——
            if (sunCooldown > 0)
                sunCooldown--;

            if (sunCooldown <= 0 && !sunVolleyActive)
            {
                // 找到太阳原点（若意外不存在则用头顶）
                sunVolleyOrigin = GetSunOrigin(Player) ?? (Player.Center + new Vector2(0f, -50f));

                // 1) 只用 Dust 的“清爽冲击波”特效
                EmitSunDustShockwave(sunVolleyOrigin);

                // 2) 启动连射
                sunVolleyActive = true;
                sunVolleyRound = 0;
                sunVolleyTimer = 0;

                // 首轮专属音效（沉一些，避免刺耳/刺眼）
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.6f, Pitch = -0.1f }, sunVolleyOrigin);

                // 重置冷却：10秒
                sunCooldown = 600;
            }

            // ——连射调度：5 轮 × 每轮 2 发（左右对称），间隔 5 帧——
            if (sunVolleyActive)
            {
                if (sunVolleyTimer > 0)
                    sunVolleyTimer--;

                if (sunVolleyTimer <= 0)
                {
                    // 发射当前轮
                    Vector2 currentOrigin = Player.Center + new Vector2(0f, -50f); // 每轮动态跟随玩家头顶
                    FireSunVolleyRound(Player, currentOrigin, sunVolleyRound);

                    sunVolleyRound++;
                    if (sunVolleyRound >= 20) // 总共射击多少轮
                    {
                        // 完成 20 轮，结束
                        sunVolleyActive = false;
                    }
                    else
                    {
                        // 非首轮：普通音效
                        currentOrigin = Player.Center + new Vector2(0f, -50f);
                        Terraria.Audio.SoundEngine.PlaySound(SoundID.Item9 with { Volume = 0.5f, Pitch = -0.05f }, currentOrigin); 
                        sunVolleyTimer = VolleyInterval;
                    }
                }
            }
        }




        // ——————————————————————
        // 太阳本体维持（保留你原有逻辑）
        // ——————————————————————
        private void ManageSunProjectile()
        {
            bool sunExists = false;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<SunProjectile>())
                {
                    sunExists = true;
                    break;
                }
            }

            if (!sunExists)
            {
                Vector2 sunPosition = Player.Center + new Vector2(0, -50); // 悬停在玩家头顶
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), sunPosition, Vector2.Zero, ModContent.ProjectileType<SunProjectile>(), 0, 0, Player.whoAmI);
            }
        }

        // === 工具：获取太阳原点（若找不到返回 null） ===
        private Vector2? GetSunOrigin(Player owner)
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == owner.whoAmI && proj.type == ModContent.ProjectileType<SunProjectile>())
                    return proj.Center;
            }
            return null;
        }

        // === 连射一轮：以“正上方”为基准左右对称（单位圆 -90°/270°） ===
        private void FireSunVolleyRound(Player owner, Vector2 origin, int roundIndex)
        {
            // 每轮相对偏移：5°, 10°, ..., 50°（共 10 轮）
            int offset = 5 * (roundIndex + 1);

            // 基准：正上方（注意：XNA 里 90° 是“向下”，所以这里用 -90°/270°）
            const float baseDeg = -90f;

            float angLeftDeg = baseDeg + offset; // -85, -80, ..., -40
            float angRightDeg = baseDeg - offset; // -95, -100, ..., -140

            SpawnSunProj2(owner, origin, DegToVel(angLeftDeg, SunProjSpeed));
            SpawnSunProj2(owner, origin, DegToVel(angRightDeg, SunProjSpeed));
        }


        // === 只用 Dust 的“清爽冲击波” ===
        private void EmitSunDustShockwave(Vector2 center)
        {
            // 轻量的双层环：外圈稀疏亮点 + 内圈细雾，全程仅 Dust
            // DustID 需保证存在（遵循你的规则）：SolarFlare / GoldFlame / Torch 等
            int outer = 32;
            int inner = 24;

            // 外圈（亮点）
            for (int i = 0; i < outer; i++)
            {
                float t = i / (float)outer;
                float ang = MathHelper.TwoPi * t;
                Vector2 dir = ang.ToRotationVector2();
                Vector2 pos = center + dir * 24f + Main.rand.NextVector2Circular(2f, 2f);

                Dust d = Dust.NewDustPerfect(
                    pos,
                    DustID.SolarFlare,
                    dir * Main.rand.NextFloat(1.6f, 2.6f),
                    160,
                    new Color(255, 220, 120),
                    1.0f
                );
                d.noGravity = true;
            }

            // 内圈（细雾）
            for (int i = 0; i < inner; i++)
            {
                float t = i / (float)inner;
                float ang = MathHelper.TwoPi * t + Main.rand.NextFloat(-0.08f, 0.08f);
                Vector2 dir = ang.ToRotationVector2();
                Vector2 pos = center + dir * Main.rand.NextFloat(6f, 14f);

                Dust d = Dust.NewDustPerfect(
                    pos,
                    DustID.GoldFlame,
                    dir * Main.rand.NextFloat(0.6f, 1.2f),
                    180,
                    new Color(255, 230, 140),
                    0.9f
                );
                d.noGravity = true;
            }
        }

        // === 生成一发 SunProj2（伤害沿用原基础伤害） ===
        private void SpawnSunProj2(Player owner, Vector2 origin, Vector2 velocity)
        {
            Projectile.NewProjectile(
                owner.GetSource_Accessory(owner.HeldItem),
                origin,
                velocity,
                ModContent.ProjectileType<SunProj2>(),
                3995,
                2f,
                owner.whoAmI
            );
        }

        private static Vector2 DegToVel(float deg, float speed)
        {
            float rad = MathHelper.ToRadians(deg);
            return rad.ToRotationVector2() * speed;
        }
    }
}
