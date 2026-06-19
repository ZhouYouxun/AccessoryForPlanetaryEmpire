using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class UranusProjectile : ModProjectile
    {
        private float rotationAngle = 0f;
        private float rotationSpeed = MathHelper.Pi / 60f; // 控制旋转速度

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Uranus");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            //Projectile.timeLeft = 300; 弹幕将永远存在，不需要存在时间限制
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<UranusPlayer>().hasUranusAccessory)
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
            Vector2 offset = new Vector2(1, -1) * 25 * 16; // 公转半径，天王星位于右上角
            offset = offset.RotatedBy(angle);
            Projectile.Center = player.Center + offset;

            // 创建冷却完成的圆环
            if (player.GetModPlayer<UranusPlayer>().GuangHuan)
            {
                CreateCooldownCircle(40); // 调整半径
            }

            // 添加天蓝色光效（天王星主题色）
            Lighting.AddLight(Projectile.Center, 0.4f, 0.8f, 1.0f); // 添加天蓝色光效
        }

        private void CreateCooldownCircle(int radius)
        {
            //// 更新旋转角度
            //rotationAngle += rotationSpeed;
            //if (rotationAngle > MathHelper.TwoPi)
            //{
            //    rotationAngle -= MathHelper.TwoPi;
            //}

            //int particleCount = 16; // 增加粒子数量
            //float[] angles = { 0f, MathHelper.PiOver4, MathHelper.PiOver2, 3 * MathHelper.PiOver4, MathHelper.Pi, 5 * MathHelper.PiOver4, 3 * MathHelper.PiOver2, 7 * MathHelper.PiOver4 }; // 八个点

            //foreach (float initialAngle in angles)
            //{
            //    float angle = initialAngle + rotationAngle;
            //    Vector2 position = Projectile.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius * 16f;

            //    // 创建粒子特效
            //    for (int j = 0; j < 15; j++) // 增加粒子密度
            //    {
            //        Dust dust = Dust.NewDustPerfect(position, DustID.BlueTorch);
            //        dust.noGravity = true;
            //        dust.scale = 4f; // 调整粒子大小
            //        dust.fadeIn = 1.2f; // 调整粒子淡入时间
            //        dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
            //        dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
            //        dust.customData = Main.rand.NextFloat(0.1f, 0.2f); // 增加旋转速度

            //        // 增加亮度
            //        dust.color = Color.White * 8f; // 提高亮度倍数
            //    }
            //}

            PLALightingBoltsSystem.DrawCooldownVisual(Projectile.Center, radius);

        }

        public override bool PreDraw(ref Color lightColor) // 确保贴图的中心点为绘制的中心点
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }


    }
}
