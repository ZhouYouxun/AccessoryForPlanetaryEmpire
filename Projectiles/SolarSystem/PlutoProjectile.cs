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
using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class PlutoProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Pluto");
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
            if (!player.active || player.dead || !player.GetModPlayer<PlutoPlayer>().hasPlutoAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            // 设置公转初始速度
            float baseRotatingSpeed = MathHelper.TwoPi / 900f; // 修改后的公转速度（每15秒一圈）
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
            Vector2 offset = new Vector2((float)(Math.Cos(angle) * 30 * 16), (float)(Math.Sin(angle) * 30 * 16)); // 长短轴公转

            Projectile.Center = player.Center + offset;

            // 添加紫色光效（冥王星主题色）
            Lighting.AddLight(Projectile.Center, 0.6f, 0.2f, 0.8f); // 添加紫色光效

            // 检测碰撞并消除或反弹敌方弹幕
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProj = Main.projectile[i];
                if (otherProj.active && otherProj.hostile && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                {
                    if (Main.rand.NextFloat() < 0.8f)
                    {
                        // 80% 概率消除敌方弹幕
                        CreateDispelEffect(otherProj.Center, otherProj.velocity);
                        otherProj.Kill();
                    }
                    else
                    {
                        // 20% 概率反弹敌方弹幕
                        ReflectProjectile(otherProj);
                    }
                }
            }

            // 添加拖尾效果
            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lead);
            Main.dust[dustId].noGravity = true;
            Main.dust[dustId].scale = 1.5f;
            Main.dust[dustId].velocity *= 0.5f;
        }

        private void ReflectProjectile(Projectile proj)
        {
            CreateReflectEffect(proj.Center, proj.velocity);
            proj.hostile = false;
            proj.friendly = true;
            proj.damage *= 3;

            // 反转弹幕方向
            proj.velocity = -proj.velocity;
        }

        private void CreateDispelEffect(Vector2 position, Vector2 projectileVelocity)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, DustID.Shadowflame);
                dust.noGravity = true;
                dust.scale = 2f;
                dust.velocity = Main.rand.NextVector2Circular(2f, 2f) + projectileVelocity * 0.5f; // 添加后坐力效果
                dust.fadeIn = 1.2f;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity += projectileVelocity * 0.5f; // 粒子在弹幕的反方向喷射
            }
        }

        private void CreateReflectEffect(Vector2 position, Vector2 projectileVelocity)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustPerfect(position, DustID.Shadowflame);
                dust.noGravity = true;
                dust.scale = 3f;
                dust.velocity = Main.rand.NextVector2Circular(3f, 3f) + projectileVelocity * 0.75f; // 添加更强的后坐力效果
                dust.fadeIn = 1.5f;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                dust.velocity += projectileVelocity * 0.75f; // 粒子在弹幕的反方向喷射
            }
        }

        public override bool PreDraw(ref Color lightColor)
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
