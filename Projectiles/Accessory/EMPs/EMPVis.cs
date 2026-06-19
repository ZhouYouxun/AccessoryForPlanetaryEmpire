//using AccessoryForPlanetaryEmpire.Buffs;
//using Microsoft.Xna.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.ID;
//using Terraria.ModLoader;
//using Terraria;
//using Microsoft.Xna.Framework.Graphics;
//using AccessoryForPlanetaryEmpire.Players.Defense;
//using AccessoryForPlanetaryEmpire.Global;

//namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
//{
//    internal class EMPVis : ModProjectile
//    {
//        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图
//        private float percent = 0f; // 当前冷却百分比（0 ~ 1）
//        public float ChargeRatio => Projectile.ai[0];
//        public float MaxRadius => Projectile.ai[1];

//        private Player owner;

//        private bool hasTriggeredFadeOut = false; // 替代 localAI[0]
//        private int lifeFrameCounter = 0;         // 替代 localAI[1]

//        public override bool PreDraw(ref Color lightColor)
//        {
//            //if (Projectile.localAI[1] < 80f)
//            //    return false;

//            Vector2 drawPos = Projectile.Center - Main.screenPosition;

//            // === 🧠 核心参数统一提取（方便修改） ===
//            float lifeProgress = hasTriggeredFadeOut && fadeOutStartTimeLeft > 0
//                ? 1f - Projectile.timeLeft / (float)fadeOutStartTimeLeft
//                : 0f;
//            float baseScale = 1f;
//            float maxScaleMultiplier = 3f;
//            float scale = MathHelper.Lerp(baseScale, baseScale * maxScaleMultiplier, lifeProgress);

//            float ringPulseSpeed = 3f;
//            float ringPulseStrength = 0.08f;
//            float starPulseSpeed = 5f;
//            float starPulseBase = 0.85f;
//            float starPulseAmp = 0.1f;
//            float ringBaseScale = 1.2f;
//            float starBaseScale = 0.5f;

//            float maxAlpha = 0.7f;

//            // 🎨 根据出生时间决定淡入（前80帧）
//            float appearAlpha = lifeFrameCounter < 80
//                ? MathHelper.Lerp(0f, maxAlpha, lifeFrameCounter / 80f)
//                : maxAlpha;

//            // 🌀 进入淡出阶段后开始从 maxAlpha → 0
//            float fade = MathHelper.Lerp(appearAlpha, 0f, lifeProgress); // 从当前透明度淡出

//            Color ringColor = Color.Lerp(Color.LightBlue, Color.Cyan, 0.4f) * 0.5f * appearAlpha * fade;
//            Color starColor = Color.Lerp(Color.SkyBlue, Color.White, 0.3f) * 0.7f * appearAlpha * fade;
//            ringColor.A = 0;
//            starColor.A = 0;











//            // === 🌌 外围魔法圈 ===
//            string[] outerRings = {
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_01",
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_02",
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_03"
//    };

//            for (int i = 0; i < outerRings.Length; i++)
//            {
//                Texture2D tex = ModContent.Request<Texture2D>(outerRings[i]).Value;
//                float rotation = Main.GlobalTimeWrappedHourly * (0.4f + 0.1f * i);
//                float pulse = 1f + ringPulseStrength * (float)Math.Sin(Main.GlobalTimeWrappedHourly * ringPulseSpeed + i);
//                float ringScale = ringBaseScale * pulse * scale;

//                Main.EntitySpriteDraw(
//                    tex,
//                    drawPos,
//                    null,
//                    ringColor,
//                    rotation,
//                    tex.Size() * 0.5f,
//                    ringScale,
//                    SpriteEffects.None,
//                    0
//                );
//            }

//            // === ⭐星星图层 ===
//            string[] stars = {
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_04",
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_07",
//        "AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08"
//    };

//            for (int i = 0; i < stars.Length; i++)
//            {
//                Texture2D tex = ModContent.Request<Texture2D>(stars[i]).Value;
//                float pulse = starPulseBase + starPulseAmp * (float)Math.Sin(Main.GlobalTimeWrappedHourly * starPulseSpeed + i);
//                float rotation = Main.GlobalTimeWrappedHourly * (i % 2 == 0 ? 1f : -1f);
//                float starScale = pulse * starBaseScale * scale;

//                Main.EntitySpriteDraw(
//                    tex,
//                    drawPos,
//                    null,
//                    starColor,
//                    rotation,
//                    tex.Size() * 0.5f,
//                    starScale,
//                    SpriteEffects.None,
//                    0
//                );
//            }

//            return false;
//        }


//        public int boundPlayerWhoAmI = -1;
//        public int maxCooldown = 1; // 防止除以 0
//        private int fadeOutStartTimeLeft = -1;
//        private bool isFadingOut = false;

//        public override void SetDefaults()
//        {
//            Projectile.width = Projectile.height = 900; 
//            Projectile.friendly = true; // 弹幕对玩家友好
//            Projectile.penetrate = -1; // 弹幕可以无限穿透
//            Projectile.timeLeft = 10 * 60;
//            Projectile.tileCollide = false; // 弹幕不与瓷砖碰撞
//            Projectile.ignoreWater = true; // 弹幕忽略水的影响
//            Projectile.alpha = 100; // 初始时弹幕是半透明的
//        }
//        public override void AI()
//        {
//            // 获取玩家
//            if (Projectile.owner < 0 || Projectile.owner >= Main.maxPlayers)
//            {
//                Projectile.Kill();
//                return;
//            }

//            Player player = Main.player[Projectile.owner];
//            if (!player.active)
//            {
//                Projectile.Kill();
//                return;
//            }

//            Projectile.Center = player.Center; // 与玩家绑定

//            // 从玩家身上找出正在使用的拦截器，并获取其冷却数据
//            EMPClearBulletPlayer modPlayer = player.GetModPlayer<EMPClearBulletPlayer>();

//            int cooldown = 0;
//            int maxCooldown = 1;
//            float radius = 16f;

//            if (modPlayer.has15Interceptor)
//            {
//                cooldown = modPlayer.cooldown15Interceptor;
//                maxCooldown = 60;
//                radius = 15 * 16f;
//            }
//            else if (modPlayer.has25IIInterceptor)
//            {
//                cooldown = modPlayer.cooldown25IIInterceptor;
//                maxCooldown = 45 * 60;
//                radius = 55 * 16f;
//            }
//            else if (modPlayer.has35IVInterceptor)
//            {
//                cooldown = modPlayer.cooldown35IVInterceptor;
//                maxCooldown = 45 * 60;
//                radius = 75 * 16f;
//            }
//            else if (modPlayer.has45Interceptor)
//            {
//                cooldown = modPlayer.cooldown45Interceptor;
//                maxCooldown = 45 * 60;
//                radius = 45 * 16f;
//            }
//            else if (modPlayer.has15Absorber)
//            {
//                cooldown = modPlayer.cooldown15Absorber;
//                maxCooldown = 45 * 60;
//                radius = 25 * 16f;
//            }
//            else if (modPlayer.has55IInterceptor)
//            {
//                cooldown = modPlayer.cooldown55IInterceptor;
//                maxCooldown = 45 * 60;
//                radius = 75 * 16f;
//            }
//            else if (modPlayer.has55VIInterceptor)
//            {
//                cooldown = modPlayer.cooldown55VIInterceptor;
//                maxCooldown = 45 * 60;
//                radius = 75 * 16f;
//            }
//            else if (modPlayer.has55IXInterceptor)
//            {
//                cooldown = modPlayer.cooldown55IXInterceptor;
//                maxCooldown = 45 * 60;
//                radius = 55 * 16f;
//            }
//            else
//            {
//                // 没装备任何拦截器，直接杀死弹幕
//                Projectile.Kill();
//                return;
//            }


//            // 🎯 一开始计数帧数
//            lifeFrameCounter++;

//            // 🧠 如果尚未触发淡出阶段
//            if (!hasTriggeredFadeOut)
//            {
//                // 🧪 满足条件时，进入淡出阶段
//                bool cooldownFinished = cooldown == 0;
//                bool timePassed = lifeFrameCounter > 60;
//                bool keyPressed = KeybindSystem.ClearBulletKeybind.JustPressed;

//                // 🧪 临时：每帧显示 cooldown 数值调试
//                if (Main.myPlayer == Projectile.owner)
//                {
//                    CombatText.NewText(
//                        Main.player[Projectile.owner].getRect(),
//                        Color.Lerp(Color.Red, Color.Lime, 1f - MathHelper.Clamp(cooldown / 1800f, 0f, 1f)), // 颜色代表剩余
//                        $"{cooldown}",
//                        true
//                    );
//                }

//                if (!isFadingOut) 
//                {
//                    if (cooldownFinished && timePassed && keyPressed)
//                    {
//                        // ✅ 一生只触发一次
//                        hasTriggeredFadeOut = true;
//                        //Projectile.timeLeft = 60;
//                        isFadingOut = true;
//                        fadeOutStartTimeLeft = Projectile.timeLeft;

                     
//                    }
//                    else
//                    {
//                        Projectile.timeLeft = 60;  // 🔁 持续刷新
//                    }
//                }
                   
//            }


//            Projectile.ai[0] = MathHelper.Clamp(1f - cooldown / (float)maxCooldown, 0f, 1f);
//            Projectile.ai[1] = radius;


//        }

//    }
//}
