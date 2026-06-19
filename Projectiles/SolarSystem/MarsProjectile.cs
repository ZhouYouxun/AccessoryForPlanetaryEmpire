using AccessoryForPlanetaryEmpire.Players.SolarSystem;
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
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class MarsProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Mars");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.scale = 1.5f;

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<MarsPlayer>().hasMarsAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            // 设置公转初始速度
            float baseRotatingSpeed = MathHelper.TwoPi / 900f; // 类地行星的默认公转速度（每15秒一圈）
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
            Vector2 offset = -Vector2.UnitX * 15 * 16; // 公转半径，火星位于正左
            offset = offset.RotatedBy(angle);
            Projectile.Center = player.Center + offset;

            // 添加红色光效（火星主题色）
            Lighting.AddLight(Projectile.Center, 1.0f, 0.2f, 0.2f); // 添加红色光效

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;
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
