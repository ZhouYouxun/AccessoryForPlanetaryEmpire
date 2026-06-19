using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class EarthHealProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Extra_89"; // 使用原版光点

        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            Vector2 origin = texture.Size() * 0.5f;

            // === Extra_89 多层脉动圆环绘制 ===
            Texture2D ringTex = Terraria.GameContent.TextureAssets.Extra[89].Value;
            float pulse = 1f + 0.08f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f);

            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f + Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 0.6f; // 旋转
                Color ringColor = new Color(180, 50, 50, 0) * 1.6f; // 深红色带透明
                Vector2 offset = Vector2.Zero;

                float scale = (0.25f + 0.05f * i) * pulse * 3f; // 大小翻三倍

                Main.EntitySpriteDraw(
                    ringTex,
                    drawPosition + offset,
                    null,
                    ringColor,
                    angle,
                    ringTex.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }

        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600; // 存在时间
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.velocity = (player.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 10f; // 追踪玩家

            CreateParticleEffect(Projectile.Center); // 生成粒子特效

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                // 如果 getGoodWorld 模式开启，则恢复 125 点血量，否则恢复 50 点
                int healAmount = Main.getGoodWorld ? 125 : 50;

                player.statLife += healAmount; // 恢复对应的血量
                player.HealEffect(healAmount, true); // 显示恢复效果
                Projectile.Kill();
            }






        }

        private void CreateParticleEffect(Vector2 center)
        {
            float t = (float)Main.GameUpdateCount / 30f;
            for (int i = 0; i < 4; i++)
            {
                float angle = MathHelper.TwoPi * i / 4f + t;
                Vector2 offset = angle.ToRotationVector2() * (6f + (float)Math.Sin(t + i) * 2f);
                Vector2 spawnPos = center + offset;
                Dust d = Dust.NewDustPerfect(spawnPos, DustID.MagicMirror, Vector2.Zero, 150, new Color(100, 255, 200), 0.9f);
                d.noGravity = true;
                d.fadeIn = 1f;
                d.velocity = Vector2.Zero;
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 💥 释放医疗光点爆发
            PLALightingBoltsSystem.PLA_Spawn_EarthHealBurst(Projectile.Center);

            // 💫 三层同心波动环形 Dust 爆发
            for (int ring = 0; ring < 3; ring++)
            {
                int count = 12 + ring * 4; // 每层 Dust 数量递增
                float radius = 6f + ring * 6f; // 每层半径递增
                float alpha = 1f - ring * 0.25f; // 每层透明度递减

                for (int i = 0; i < count; i++)
                {
                    // 角度扰动形成波纹感
                    float angle = MathHelper.TwoPi * i / count + Main.rand.NextFloat(-0.1f, 0.1f);
                    Vector2 dir = angle.ToRotationVector2();

                    // 每层 Dust 飞出位置 + 速度 = 圆环 + 波动
                    Vector2 spawnPos = Projectile.Center + dir * radius;
                    Vector2 velocity = dir * Main.rand.NextFloat(0.8f, 1.6f) + dir.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-0.2f, 0.2f);

                    // 颜色渐变从嫩绿→薄荷绿→青绿
                    Color color = ring switch
                    {
                        0 => new Color(150, 255, 190),
                        1 => new Color(100, 255, 210),
                        _ => new Color(80, 240, 180)
                    };

                    Dust d = Dust.NewDustPerfect(spawnPos, DustID.MagicMirror, velocity, 100, color * alpha, 1.2f - ring * 0.2f);
                    d.noGravity = true;
                    d.fadeIn = 0.8f + Main.rand.NextFloat(0.1f, 0.5f);
                }
            }

        }










    }
}
