using AccessoryForPlanetaryEmpire.Players.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class SunProjectile : ModProjectile
    {
        private float rotationAngle = 0f;
        private const float rotationSpeed = MathHelper.Pi / 60f; // 旋转速度

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("太阳光环");
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            Projectile.width = 55;
            Projectile.height = 55;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 36000; // 弹幕将永远存在，不需要时间限制
            Projectile.scale = 1.5f;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<SunPlayer>().hasSunAccessory)
            {
                Projectile.Kill();
                return;
            }
            Projectile.timeLeft = 10;

            Projectile.Center = player.Center + new Vector2(0, -50); // 悬停在玩家头顶
            Lighting.AddLight(Projectile.Center, 1f, 1f, 0.5f); // 添加光效

            // 添加拖尾效果
            //int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
            //Main.dust[dustId].noGravity = true;
            //Main.dust[dustId].scale = 1.5f;
            //Main.dust[dustId].velocity *= 0.5f;

            // 生成粒子特效组成的小圆圈
            if (player.GetModPlayer<SunPlayer>().GuangHuan)
            {
                //CreateParticleCircle(25); // 半径为10像素的圆圈
            }
            
        }

        public override bool? CanDamage()
        {
            return true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;

            // ——通用位姿——
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

            // ——时间参数（缓慢脉动 + 旋转相位）——
            float t = (float)Main.GlobalTimeWrappedHourly;
            float t2 = t * 0.6f;   // 更慢
            float t3 = t * 0.35f;  // 最慢

            // ——太阳配色（暖橙，不刺眼）——
            Color coreCol = new Color(255, 190, 80) * 0.95f; // 本体主色
            Color coreColDim = new Color(255, 200, 120) * 0.60f;
            Color ringCol = new Color(255, 180, 90) * 0.45f;
            Color starCol = new Color(255, 210, 120) * 0.35f;
            Color gearCol = new Color(255, 170, 70) * 0.35f;

            // =========================================================
            // 1) 本体贴图（不旋转，只脉动缩放 + 染色），叠加两层
            //    贴图：AccessoryForPlanetaryEmpire/Texture/STP/fbmnoise2_006
            //    大小“缩小至 0.33 倍”，并在此基础上做缓慢脉动
            // =========================================================
            Texture2D texBase = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/fbmnoise2_006").Value;
            Vector2 orgBase = texBase.Size() * 0.5f;

            // 缓慢脉动：两条不同相位与幅度的曲线叠加
            float pulseA = 1f + 0.05f * (float)Math.Sin(t * 2.0f);
            float pulseB = 1f + 0.03f * (float)Math.Sin(t2 * 2.2f + 1.3f);

            float scaleBaseA = 0.13f * pulseA;
            float scaleBaseB = 0.13f * 1.20f * pulseB; // 第二层略大，仍不旋转

            // 本体两层（不旋转）
            sb.Draw(texBase, drawPos, null, coreCol, 0f, orgBase, scaleBaseA, SpriteEffects.None, 0f);
            sb.Draw(texBase, drawPos, null, coreColDim, 0f, orgBase, scaleBaseB, SpriteEffects.None, 0f);

            // =========================================================
            // 2) 外环/星芒叠加（可旋转/缩放/反转），营造层次感
            //    贴图：
            //      - "Terraria/Images/Extra_89" （柔和能量环）
            //      - "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07/08/09"
            //      - "AccessoryForPlanetaryEmpire/Texture/STP/diamondgear_016"（双层 100% 等比、等速反向旋转）
            // =========================================================

            // ——Extra_89：两层反相位、缓慢旋转的柔和能量环
            Texture2D texRing = Terraria.GameContent.TextureAssets.Extra[89].Value;
            Vector2 orgRing = texRing.Size() * 0.5f;

            float ringScale1 = 0.85f * (1f + 0.04f * (float)Math.Sin(t * 1.7f));
            float ringScale2 = 1.15f * (1f + 0.03f * (float)Math.Sin(t2 * 1.2f + 0.8f));

            float ringRot1 = t * MathHelper.TwoPi * 0.20f; // 顺时针
            float ringRot2 = -t * MathHelper.TwoPi * 0.16f; // 逆时针

            sb.Draw(texRing, drawPos, null, ringCol, ringRot1, orgRing, ringScale1, SpriteEffects.None, 0f);
            sb.Draw(texRing, drawPos, null, ringCol * 0.8f, ringRot2, orgRing, ringScale2, SpriteEffects.None, 0f);

            // ——星芒三件：不同缩放与角速度
            Texture2D star07 = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07").Value;
            Texture2D star08 = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08").Value;
            Texture2D star09 = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_09").Value;
            Vector2 orgS7 = star07.Size() * 0.5f;
            Vector2 orgS8 = star08.Size() * 0.5f;
            Vector2 orgS9 = star09.Size() * 0.5f;

            float s7Scale = 0.55f * (1f + 0.02f * (float)Math.Sin(t * 2.3f + 0.2f));
            float s8Scale = 0.70f * (1f + 0.03f * (float)Math.Sin(t2 * 1.9f + 2.1f));
            float s9Scale = 0.95f * (1f + 0.02f * (float)Math.Sin(t3 * 2.5f + 1.2f));

            float s7Rot = t * MathHelper.TwoPi * 0.40f;
            float s8Rot = -t * MathHelper.TwoPi * 0.28f;
            float s9Rot = t * MathHelper.TwoPi * 0.18f;

            sb.Draw(star07, drawPos, null, starCol, s7Rot, orgS7, s7Scale, SpriteEffects.None, 0f);
            sb.Draw(star08, drawPos, null, starCol * 0.9f, s8Rot, orgS8, s8Scale, SpriteEffects.None, 0f);
            sb.Draw(star09, drawPos, null, starCol * 0.8f, s9Rot, orgS9, s9Scale, SpriteEffects.None, 0f);

            // ——diamondgear_016：两层 100% 大小、等速反向旋转
            Texture2D gear = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/diamondgear_016").Value;
            Vector2 orgG = gear.Size() * 0.5f;

            float gearScale = 0.50f;
            float gearRot = t * MathHelper.TwoPi * 0.22f;

            sb.Draw(gear, drawPos, null, gearCol, gearRot, orgG, gearScale, SpriteEffects.None, 0f);
            sb.Draw(gear, drawPos, null, gearCol, -gearRot, orgG, gearScale, SpriteEffects.None, 0f);

            return false; // 关闭默认绘制
        }


        private void CreateParticleCircle(int radius)
        {
            // 更新旋转角度
            rotationAngle += rotationSpeed;
            if (rotationAngle > MathHelper.TwoPi)
            {
                rotationAngle -= MathHelper.TwoPi;
            }

            int particleCount = 16; // 增加粒子数量
            float[] angles = { 0f, MathHelper.PiOver4, MathHelper.PiOver2, 3 * MathHelper.PiOver4, MathHelper.Pi, 5 * MathHelper.PiOver4, 3 * MathHelper.PiOver2, 7 * MathHelper.PiOver4 }; // 八个点

            foreach (float initialAngle in angles)
            {
                float angle = initialAngle + rotationAngle;
                Vector2 position = Projectile.Center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;

                // 创建粒子特效
                for (int j = 0; j < 15; j++) // 增加粒子密度
                {
                    Dust dust = Dust.NewDustPerfect(position, DustID.Lava);
                    dust.noGravity = true;
                    dust.scale = 1.75f; // 调整粒子大小
                    dust.fadeIn = 0.5f; // 调整粒子淡入时间，使其更快地淡出
                    dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                    dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                    dust.customData = Main.rand.NextFloat(0.1f, 0.2f); // 增加旋转速度

                    // 增加亮度
                    dust.color = Color.White * 2f; // 提高亮度倍数

                    // 使粒子更快地淡出
                    dust.velocity = -Vector2.UnitY.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(0.1f, 0.5f);
                }
            }
        }


    }
}
