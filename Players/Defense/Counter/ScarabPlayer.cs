using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class ScarabPlayer : ModPlayer
    {
        public bool hasScarab = false;
        private int hitCount = 0; // 记录受击次数

        public override void ResetEffects()
        {
            hasScarab = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (hasScarab)
            {
                hitCount++;

                // 根据模式调整触发次数
                int requiredHits = Main.getGoodWorld ? 2 : 5;

                if (hitCount >= requiredHits) // 每受到指定次数的伤害触发效果
                {
                    // 生成粒子特效
                    CreateScarabActivationEffect(Player.Center, DustID.PurpleCrystalShard, 50, 1f);

                    // 反射玩家周围50格内的敌对弹幕
                    ReflectNearbyProjectiles();

                    // 重置受击计数
                    hitCount = 0;
                }
            }
        }

        private void ReflectNearbyProjectiles()
        {
            foreach (Projectile proj in Main.projectile)
            {
                // 只影响敌对弹幕，并且弹幕距离玩家不超过50格（800像素）
                if (proj.active && proj.hostile && Vector2.Distance(proj.Center, Player.Center) <= 50 * 16)
                {
                    proj.hostile = false; // 转化为友方弹幕
                    proj.friendly = true;
                    proj.damage *= 6; // 提升弹幕伤害至原来的6倍
                    proj.velocity = -proj.velocity; // 反转弹幕方向
                }
            }
        }

        // 创建“圣甲虫开启光环”粒子特效的方法
        private void CreateScarabActivationEffect(Vector2 position, int dustID, int particleCount, float duration)
        {
            // 绘制圣甲虫的头部
            CreateEllipse(position, dustID, 12, 8, 10f, 5f, duration);

            // 绘制圣甲虫的身体
            CreateEllipse(position + new Vector2(0, 10), dustID, 20, 15, 15f, 8f, duration);

            // 绘制圣甲虫的翅膀
            CreateEllipse(position + new Vector2(-10, 10), dustID, 15, 10, 8f, 12f, duration);
            CreateEllipse(position + new Vector2(10, 10), dustID, 15, 10, 8f, 12f, duration);

            // 添加闪光效果
            CreateFlashEffect(position, dustID, 30, duration);
        }

        // 绘制椭圆形粒子特效的方法
        private void CreateEllipse(Vector2 position, int dustID, int horizontalParticles, int verticalParticles, float radiusX, float radiusY, float duration)
        {
            float angleStep = MathHelper.TwoPi / horizontalParticles;
            for (int i = 0; i < horizontalParticles; i++)
            {
                float angle = i * angleStep;
                float x = radiusX * (float)Math.Cos(angle);
                float y = radiusY * (float)Math.Sin(angle);

                // 创建粒子特效
                Dust dust = Dust.NewDustPerfect(position + new Vector2(x, y), dustID, Vector2.Zero, 100, Color.Purple, 4f);
                dust.noGravity = true;
                dust.fadeIn = duration;
            }

            angleStep = MathHelper.TwoPi / verticalParticles;
            for (int i = 0; verticalParticles > i; i++)
            {
                float angle = i * angleStep;
                float x = radiusX * (float)Math.Sin(angle);
                float y = radiusY * (float)Math.Cos(angle);

                // 创建粒子特效
                Dust dust = Dust.NewDustPerfect(position + new Vector2(x, y), dustID, Vector2.Zero, 100, Color.Purple, 4f);
                dust.noGravity = true;
                dust.fadeIn = duration;
            }
        }

        // 创建闪光效果的方法
        private void CreateFlashEffect(Vector2 position, int dustID, int particleCount, float duration)
        {
            for (int i = 0; i < particleCount; i++)
            {
                Vector2 velocity = Main.rand.NextVector2Circular(6f, 6f);
                Dust flashDust = Dust.NewDustPerfect(position, dustID, velocity, 100, Color.White, 6f);
                flashDust.noGravity = true;
                flashDust.fadeIn = duration;
            }
        }
    }
}