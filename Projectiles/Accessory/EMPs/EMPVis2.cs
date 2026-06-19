using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players.Defense;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
{
    internal class EMPVis2 : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV";

        private int lifeFrameCounter = 0;

        private int cooldown = 0;
        private int maxCooldown = 1;
        private float radius = 16f;
        // 🎯 可统一调整的视觉缩放范围（不随装备变化）
        private const float minVisualScale = 0.1f;
        private const float maxVisualScale = 2.4f;

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10 * 60;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 100;
        }

        public override void AI()
        {
            lifeFrameCounter++;

            // 🧠 绑定玩家中心
            if (Projectile.owner < 0 || Projectile.owner >= Main.maxPlayers)
            {
                Projectile.Kill();
                return;
            }

            Player player = Main.player[Projectile.owner];
            if (!player.active)
            {
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Center;

            // 📦 读取玩家拦截器冷却数据
            EMPClearBulletPlayer modPlayer = player.GetModPlayer<EMPClearBulletPlayer>();
            EMPSupportBulletPlayer supportPlayer = player.GetModPlayer<EMPSupportBulletPlayer>();
            EMPUniversalPlayer universalPlayer = player.GetModPlayer<EMPUniversalPlayer>();

            // 💡 测试触发
            bool cooldownFinished = cooldown <= 0;
            bool timePassed = lifeFrameCounter > 60;
            bool keyPressed = KeybindSystem.ClearBulletKeybind.JustPressed;

            if (cooldownFinished && timePassed && keyPressed && Main.myPlayer == Projectile.owner)
            {
                //Main.NewText("冷却完成 ✔️");
                //Main.NewText("时间足够 ✔️");
                //Main.NewText("按键触发 ✔️");
                hasTriggeredFadeOut = true;
                fadeOutTimer = fadeOutDuration;
                Projectile.timeLeft = fadeOutDuration; // ⏳ 确保 60帧后自动消失

                float percent = MathHelper.Clamp(1f - cooldown / (float)maxCooldown, 0f, 1f);
                fadeOutStartScale = MathHelper.Lerp(minVisualScale, maxVisualScale, percent);

            }
            else
            {
                cooldown = 0;
                maxCooldown = 1;
                radius = 16f;

                if (modPlayer.has15Interceptor)
                {
                    cooldown = modPlayer.cooldown15Interceptor;
                    maxCooldown = 60;
                    radius = 15 * 16f;
                    baseColor = Color.Purple; // 紫皮凤凰
                }
                else if (modPlayer.has25IIInterceptor)
                {
                    cooldown = modPlayer.cooldown25IIInterceptor;
                    maxCooldown = 45 * 60;
                    radius = 55 * 16f;
                    baseColor = Color.Lime; // 04236
                }
                else if (modPlayer.has35IVInterceptor)
                {
                    cooldown = modPlayer.cooldown35IVInterceptor;
                    maxCooldown = 45 * 60;
                    radius = 75 * 16f;
                    baseColor = Color.Bisque; // 维杰特
                }
                else if (modPlayer.has45Interceptor)
                {
                    cooldown = modPlayer.cooldown45Interceptor;
                    maxCooldown = 45 * 60;
                    radius = 45 * 16f;
                    baseColor = Color.DarkGreen; // 松纳
                }
                else if (modPlayer.has15Absorber)
                {
                    cooldown = modPlayer.cooldown15Absorber;
                    maxCooldown = 45 * 60;
                    radius = 25 * 16f;
                    baseColor = Color.MediumVioletRed; // 吴可欣
                }
                else if (modPlayer.has55IInterceptor)
                {
                    cooldown = modPlayer.cooldown55IInterceptor;
                    maxCooldown = 45 * 60;
                    radius = 75 * 16f;
                    baseColor = Color.WhiteSmoke; // 连锁引爆-海盗
                }
                else if (modPlayer.has55VIInterceptor)
                {
                    cooldown = modPlayer.cooldown55VIInterceptor;
                    maxCooldown = 45 * 60;
                    radius = 75 * 16f;
                    baseColor = Color.Cyan; // 震荡弹射-无星
                }
                else if (modPlayer.has55IXInterceptor)
                {
                    cooldown = modPlayer.cooldown55IXInterceptor;
                    maxCooldown = 45 * 60;
                    radius = 55 * 16f;
                    baseColor = Color.IndianRed; // 转化弹幕-黛莎莉丝
                }
                else if (supportPlayer.has25dBarrier)
                {
                    cooldown = supportPlayer.cooldown25dBarrier;
                    maxCooldown = 30 * 60;
                    radius = 35 * 16f;
                    baseColor = Color.DarkGray; // 剃刀VI
                }
                else if (supportPlayer.has25eSubSpace)
                {
                    cooldown = supportPlayer.cooldown25fSubSpace;
                    maxCooldown = 30 * 60;
                    radius = 35 * 16f;
                    baseColor = Color.MidnightBlue; // 星战
                }
                else if (universalPlayer.hasUniversalProtectionDevice)
                {
                    cooldown = universalPlayer.cooldownUniversalProtectionDevice;
                    maxCooldown = 60 * 60;
                    radius = 250 * 16f;
                    // 🌈 彩虹色控制：用 hueShift 控制颜色
                    hueShift += 0.01f;
                    if (hueShift > 1f)
                        hueShift -= 1f;

                    baseColor = Main.hslToRgb(hueShift, 1f, 0.5f); // 总睿号-柔和彩虹色
                }
                else
                {
                    // ❌ 没有配备任何拦截器，直接销毁自己
                    Projectile.Kill();
                    return;
                }
            }


            if (hasTriggeredFadeOut)
            {
                fadeOutTimer--;
                if (fadeOutTimer <= 0)
                {
                    Projectile.Kill();
                    return;
                }
            }




        }
        private bool hasTriggeredFadeOut = false;
        private int fadeOutTimer = 0;
        private const int fadeOutDuration = 60;
        private float fadeOutStartScale = 1f;

        // 当前主色调，默认是白色（会在 AI 里设置）
        private Color baseColor = Color.White;

        // 彩虹色过渡控制（用于超级装置）
        private float hueShift = 0f;

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            float scale;
            float alpha;
            float brightness;
            Color ringColor;
            Color starColor;

            if (hasTriggeredFadeOut)
            {
                // 🌀 离场动画进度 t：0 → 1
                float t = 1f - fadeOutTimer / (float)fadeOutDuration;

                // 🔁 大小从触发瞬间放大至 5 倍
                scale = MathHelper.Lerp(fadeOutStartScale, fadeOutStartScale * 5f, t);

                // 💡 透明度（越高越亮）从 1 → 0（可调）
                alpha = MathHelper.Lerp(1f, 0f, t);

                // 🔅亮度从 100% 降低至接近 0%（视觉熄灭）
                brightness = MathHelper.Lerp(0.5f, 0.003f, t);
            }
            else
            {
                // 🌡️根据当前冷却进度计算比例（0 ~ 1）
                float percent = MathHelper.Clamp(1f - cooldown / (float)maxCooldown, 0f, 1f);

                // 🎯统一从 minVisualScale → maxVisualScale 增长（不随装备变）
                scale = MathHelper.Lerp(minVisualScale, maxVisualScale, percent);

                // ✨默认透明度为常数
                alpha = 1f;

                // 🔅默认亮度为固定值
                brightness = 0.3f;
            }

            // 🎨最终颜色 = 色彩 × 亮度 × 透明度（亮但暗淡）
            ringColor = baseColor * brightness * alpha;
            starColor = baseColor * brightness * alpha;

            // 🧽强制去除 alpha 通道，防止颜色残影
            ringColor.A = 0;
            starColor.A = 0;

            // 🌀 加回 rotationTime，供圈和星星旋转用
            float rotationTime = Main.GlobalTimeWrappedHourly;


            string[] outerRings = {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_01",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_02",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_03"
    };

            for (int i = 0; i < outerRings.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(outerRings[i]).Value;
                float rotation = rotationTime * (0.4f + 0.1f * i);

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    ringColor,
                    rotation,
                    tex.Size() * 0.5f,
                    scale,
                    SpriteEffects.None,
                    0
                );
            }

            string[] stars = {
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_04",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07",
        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08"
    };

            for (int i = 0; i < stars.Length; i++)
            {
                Texture2D tex = ModContent.Request<Texture2D>(stars[i]).Value;
                float rotation = rotationTime * (i % 2 == 0 ? 1f : -1f);

                Main.EntitySpriteDraw(
                    tex,
                    drawPos,
                    null,
                    starColor,
                    rotation,
                    tex.Size() * 0.5f,
                    scale * 0.5f,
                    SpriteEffects.None,
                    0
                );
            }

            return false;
        }




    }
}