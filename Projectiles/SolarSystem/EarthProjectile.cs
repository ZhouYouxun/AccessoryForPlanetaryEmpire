using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using AccessoryForPlanetaryEmpire.Players;
using System;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class EarthProjectile : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Earth");
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.light = 0.5f; // 添加发光效果
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<EarthPlayer>().hasEarthAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            double baseRotatingSpeed = MathHelper.TwoPi / 900; // 默认公转速度
            double rotatingSpeed = baseRotatingSpeed;

            if (Projectile.localAI[0] == 0) // 在首次生成时初始化
            {
                Projectile.localAI[1] = 10; // 设置初始速度倍率为10倍
            }

            // 调整衰减速度，使旋转角度覆盖720度
            Projectile.localAI[1] = MathHelper.Lerp(Projectile.localAI[1], 1f, 0.02f); // 减少减速速度
            rotatingSpeed *= Math.Max(Projectile.localAI[1], 1f); // 保证速度不会低于原始速度

            Projectile.localAI[0]++; // 递增计时器

            // 调整角度增量以覆盖720度
            Projectile.ai[0] += (float)(rotatingSpeed / baseRotatingSpeed * 2f); // 通过增加倍数来控制角度增量

            double angle = Projectile.ai[0] * baseRotatingSpeed;
            Vector2 offset = Vector2.UnitX * 15 * 16; // 公转半径
            offset = offset.RotatedBy(angle);
            Projectile.Center = player.Center + offset;

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;

            Lighting.AddLight(Projectile.Center, 0.1f, 0.5f, 0.1f); // 添加光效
        }


        public override bool PreDraw(ref Color lightColor) // 确保贴图的中心点为绘制的中心点
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale * 0.78f, SpriteEffects.None, 0f);
            return false;
        }

    }
}
