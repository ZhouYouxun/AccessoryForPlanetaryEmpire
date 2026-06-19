using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.ID;


namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class VenusExplosionProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180; // 粒子效果的持续时间
        }



        public override void AI()
        {
            float t = (float)(180 - Projectile.timeLeft) / 60f; // 时间归一化（最大3秒）
            Vector2 center = Projectile.Center;

            // === 🌀 星形结构（正五角星外轮廓） ===
            for (int i = 0; i < 5; i++)
            {
                float angle = MathHelper.TwoPi * i / 5f + MathHelper.PiOver2;
                float nextAngle = MathHelper.TwoPi * ((i + 2) % 5) / 5f + MathHelper.PiOver2;

                for (int j = 0; j <= 20; j++)
                {
                    float lerp = j / 20f;
                    float interpAngle = MathHelper.Lerp(angle, nextAngle, lerp);
                    float radius = MathHelper.Lerp(0f, 80 * 16f, t);

                    Vector2 pos = center + interpAngle.ToRotationVector2() * radius;

                    var d = Dust.NewDustPerfect(pos, DustID.GoldCoin, Vector2.Zero, 100, Color.White, 1.4f);
                    d.noGravity = true;
                }
            }

            // === 🔄 螺旋波结构 ===
            int spiralPoints = 60;
            for (int i = 0; i < spiralPoints; i++)
            {
                float angle = i * MathHelper.TwoPi / spiralPoints + t * 3f;
                float radius = (i / (float)spiralPoints) * 90 * 16f * (float)Math.Sin(t * MathHelper.Pi);
                Vector2 pos = center + angle.ToRotationVector2() * radius;

                var d = Dust.NewDustPerfect(pos, DustID.GoldCoin, Vector2.Zero, 100, Color.Gold, 1.2f);
                d.noGravity = true;
            }

            // === 💞 心形轨迹 ===
            for (int i = 0; i < 20; i++)
            {
                float theta = MathHelper.TwoPi * i / 20f;
                float r = 20f * (1 - (float)Math.Sin(theta));
                Vector2 pos = center + new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * r * 16f * (0.5f + 0.5f * t);

                var d = Dust.NewDustPerfect(pos, DustID.GoldCoin, Vector2.Zero, 100, Color.Goldenrod, 1.1f);
                d.noGravity = true;
            }

            // === 🟠 完整圆环 ===
            for (int i = 0; i < 60; i++)
            {
                float angle = MathHelper.TwoPi * i / 60f;
                float radius = 100 * 16f * t;
                Vector2 pos = center + angle.ToRotationVector2() * radius;

                var d = Dust.NewDustPerfect(pos, DustID.GoldCoin, Vector2.Zero, 100, Color.LightGoldenrodYellow, 1.5f);
                d.noGravity = true;
            }

            Projectile.Kill(); // 只执行一次
        }

        private void CreateDustCircle(Vector2 center, float maxRadius, int dustType, int dustAmount, int steps)
        {
            for (int step = 0; step < steps; step++)
            {
                float radius = (maxRadius / steps) * (step + 1);
                for (int i = 0; i < dustAmount; i++)
                {
                    double angle = Main.rand.NextDouble() * 2 * Math.PI;
                    Vector2 dustPosition = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
                    int dustIndex = Dust.NewDust(dustPosition, 1, 1, dustType, 0f, 0f, 100, default, 3f);
                    Main.dust[dustIndex].velocity = Vector2.Zero;
                    Main.dust[dustIndex].scale = 1.5f - (step * 0.01f); // 逐步缩小粒子
                }

                // 添加一点延迟，使其看起来像是逐步扩散
                Projectile.timeLeft--;
            }
        }







    }
}
