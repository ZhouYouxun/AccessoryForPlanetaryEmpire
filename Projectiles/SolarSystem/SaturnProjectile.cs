using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class SaturnProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Saturn");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;
            Projectile.DamageType = DamageClass.Melee; // 设置为近战伤害类型

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<SaturnPlayer>().hasSaturnAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            // 设置公转初始速度
            float baseRotatingSpeed = MathHelper.TwoPi / 1200f; // 类木行星的默认公转速度（每20秒一圈）
            float rotatingSpeed = baseRotatingSpeed;

            // 初始化设置，第一次运行时触发
            if (Projectile.localAI[0] == 0)
            {
                Projectile.localAI[1] = 10f; // 初始速度倍率为10倍
            }

            // 每一帧减速，直到达到正常速度
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.02f); // 指数减速
            rotatingSpeed *= Math.Max(Projectile.localAI[1], 1f); // 确保旋转速度不低于正常速度

            Projectile.localAI[0]++; // 递增计时器

            // 根据减速后的旋转速度调整角度增量
            Projectile.ai[0] += (float)(rotatingSpeed / baseRotatingSpeed * 2f);
            double angle = Projectile.ai[0] * baseRotatingSpeed;
            Vector2 offset = new Vector2(-1, 1) * 25 * 16; // 公转半径，土星位于左下角
            offset = offset.RotatedBy(angle);
            Projectile.Center = player.Center + offset; // 公转中心调整

            // 添加黄色光效（土星主题色）
            Lighting.AddLight(Projectile.Center, 1.0f, 0.9f, 0.4f); // 添加黄色光效

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemAmber);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;



            {
                SaturnPlayer modPlayer = Main.player[Projectile.owner].GetModPlayer<SaturnPlayer>();
                Projectile.localAI[2] = modPlayer.GetCooldownRatio(); // 👈 这一句至关重要

                //// 🌀 绘制围绕土星的旋转粒子圈
                //float t = (float)Main.GameUpdateCount / 60f;
                //float cooldownRatio = Projectile.localAI[2]; // 越小越快
                //float angleSpeed = MathHelper.Lerp(0.1f, 1.5f, 1f - cooldownRatio); // 冷却越接近0越快

                //int particleCount = 6;
                //float radius = 30f;

                //for (int i = 0; i < particleCount; i++)
                //{
                //    float angle1 = MathHelper.TwoPi * i / particleCount + t * angleSpeed;
                //    Vector2 offset1 = angle1.ToRotationVector2() * radius;
                //    Vector2 pos = Projectile.Center + offset1;

                //    Dust d = Dust.NewDustPerfect(pos, DustID.GemTopaz, Vector2.Zero, 100, Color.Orange, 0.8f);
                //    d.noGravity = true;
                //    d.fadeIn = 0.6f;
                //}

            }



        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 使目标有短暂无敌帧
            target.immune[Projectile.owner] = 10;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // 🌕 获取 cooldownRatio（0 = 刚发射，1 = 即将发射）
            float cooldownRatio = Projectile.localAI[2];

            // 🍎 使用 cubic easeInOut 缓动，模拟“呼吸”亮度弹跳
            float progress;
            if (cooldownRatio < 0.5f)
                progress = 4f * cooldownRatio * cooldownRatio * cooldownRatio;
            else
            {
                float inv = 1f - cooldownRatio;
                progress = 1f - 4f * inv * inv * inv;
            }

            // 🔆 显著调整亮度范围：从 0.5 ~ ? 倍
            float brightness = MathHelper.Lerp(0.5f, 1.0f, progress);

            // 💡 根据亮度渐变颜色
            Color glowColor = Color.Lerp(Color.Orange, Color.Yellow, progress) * brightness;
            glowColor.A = 0;

            spriteBatch.Draw(texture, drawPosition, null, glowColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }


    }
}
