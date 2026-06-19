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
using System;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;


namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class MercuryProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Mercury");
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
            if (!player.active || player.dead || !player.GetModPlayer<MercuryPlayer>().hasMercuryAccessory)
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
            Vector2 offset = Vector2.UnitY * -15 * 16; // 公转半径，水星位于正上方
            offset = offset.RotatedBy(angle);

            // 手动调整公转中心的坐标
            Projectile.Center = player.Center + offset;

            // 添加浅灰色光效（水星主题色）
            Lighting.AddLight(Projectile.Center, 0.8f, 0.8f, 0.8f); // 添加浅灰色光效

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BoneTorch);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = texture.Size() * 0.5f;
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // 🌀 保留原本体贴图绘制
            spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);

            // 💫 魔法圈风格淡灰 twirl 绘制
            string[] twirlTexPaths = new[]
            {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_01",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_02",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/twirl_03"
    };

            float baseScale = 0.35f;

            for (int i = 0; i < twirlTexPaths.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(twirlTexPaths[i]).Value;

                float rotation = Main.GlobalTimeWrappedHourly * (i % 2 == 0 ? 0.6f : -0.6f) * (1f + i * 0.1f); // 交替旋转
                float scale = baseScale * (1.0f + 0.15f * i); // 每层略放大
                float pulse = 1f + 0.05f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3f + i);

                Color twirlColor = new Color(180, 180, 180, 0) * 0.4f; // 暗灰色，半透明

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    twirlColor,
                    rotation,
                    tex.Size() * 0.5f,
                    scale * pulse,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }




    }
}
