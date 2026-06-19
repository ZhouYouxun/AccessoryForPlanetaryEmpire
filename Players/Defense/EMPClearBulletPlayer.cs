using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.Audio;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs;
using AccessoryForPlanetaryEmpire.Buffs;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class EMPClearBulletPlayer : ModPlayer
    {
        // 瞬间消除弹幕：
        // 15型弹幕拦截器【骷髅王后】：按下快捷键时清除半径35格范围内的敌方弹幕
        // 25II型弹幕拦截器【肉山后】：按下快捷键时清除半径55格范围内的敌方弹幕，给所有敌人施加困惑debuff
        // 35IV型弹幕拦截器【毁灭者后】：按下快捷键时清除半径75格范围内的敌方弹幕，并在之后向周围散射大量激光
        // 45型转化弹幕拦截器【花后】：按下快捷键时清除半径105格范围内的敌方弹幕，同时叛变这些弹幕中的一半
        // 15型能量吸收装置【花后】：按下快捷键时清除半径25格范围内的敌方弹幕，但清除的每一发弹幕都能转化为3.5%的临时伤害提升，伤害提升持续5秒
        // 55I型连锁引爆炮【石巨人后】：按下快捷键时清除半径75格范围内的敌方弹幕，弹幕被清除时会引爆，并对周围的敌人造成5倍原弹幕伤害
        // 55VI型震荡弹射器【石巨人后】：按下快捷键时清除半径75格范围内的敌方弹幕，与此同时，眩晕范围内的所有敌人0.7秒
        // 55IX型转化弹幕拦截器【教徒后】：按下快捷键时清除半径65格范围内的敌方弹幕，同时叛变这些弹幕

        // 定义所有拦截器的标志变量和冷却时间变量
        public bool has15Interceptor = false;
        public bool has25IIInterceptor = false;
        public bool has35IVInterceptor = false;
        public bool has45Interceptor = false;
        public bool has15Absorber = false;
        public bool has55IInterceptor = false;
        public bool has55VIInterceptor = false;
        public bool has55IXInterceptor = false;

        public int cooldown15Interceptor = 10 * 60;
        public int cooldown25IIInterceptor = 10 * 60;
        public int cooldown35IVInterceptor = 10 * 60;
        public int cooldown45Interceptor = 10 * 60;
        public int cooldown15Absorber = 10 * 60;
        public int cooldown55IInterceptor = 10 * 60;
        public int cooldown55VIInterceptor = 10 * 60;
        public int cooldown55IXInterceptor = 10 * 60;

        public int particleTimer = 60; // 粒子特效计时器


        // 是否应该绘制圆环
        public bool shouldDrawCircle15 = false;
        public bool shouldDrawCircle25 = false;
        public bool shouldDrawCircle35 = false;
        public bool shouldDrawCircle45 = false;
        public bool shouldDrawCircle55 = false;
        public bool shouldDrawCircle = false;

        // int projectileType = ModContent.ProjectileType<EMPProj>(); // 弹幕类型


        public int cooldownCircleTimer = 0; // 添加计时器

        public float empDamageIncrease = 0f; // 存储计算好的伤害增幅值






        private int shakeDuration;
        private float shakeIntensity;
        private Vector2 originalScreenPosition;



        // 先建立一个构造
        private void HandleEMPUnit(
            bool hasUnit,
            ref int cooldown,
            int radius,
            Action onCast,
            ref bool shouldDrawCircle,
            string readyText,
            string castText,
            float shakePower)
        {
            if (!hasUnit)
                return;

            // 冷却计时逻辑
            if (cooldown > 0)
            {
                cooldown--;
                if (cooldown == 0)
                {
                    //CombatText.NewText(Player.getRect(), Color.Cyan, readyText);
                    SoundEngine.PlaySound(SoundID.Item4, Player.position);
                    shouldDrawCircle = true;
                }
            }

            // 释放技能逻辑
            if (KeybindSystem.ClearBulletKeybind.JustPressed && cooldown == 0)
            {
                onCast?.Invoke(); // 调用外部传入的具体释放逻辑
                cooldown = 45 * 60;
                EMPAll.CreateCooldownCircle(radius);
                shouldDrawCircle = false;
                //CombatText.NewText(Player.getRect(), Color.Cyan, castText);
                EMPAll.TriggerScreenShake(shakePower);
                {
                    Color baseColor = Color.White; // 默认值

                    if (has15Interceptor)
                        baseColor = Color.Purple;
                    else if (has25IIInterceptor)
                        baseColor = Color.Lime;
                    else if (has35IVInterceptor)
                        baseColor = Color.Bisque;
                    else if (has45Interceptor)
                        baseColor = Color.DarkGreen;
                    else if (has15Absorber)
                        baseColor = Color.MediumVioletRed;
                    else if (has55IInterceptor)
                        baseColor = Color.WhiteSmoke;
                    else if (has55VIInterceptor)
                        baseColor = Color.Cyan;
                    else if (has55IXInterceptor)
                        baseColor = Color.IndianRed;

                    EMPAll.ShootInterceptEffect(Player.Center, baseColor);

                }
             
            }

            // 圆环绘制逻辑
            if (shouldDrawCircle)
            {
                EMPAll.CreateCooldownCircle(radius);
            }
        }

        public override void ResetEffects()
        {
            has15Interceptor = false;
            has25IIInterceptor = false;
            has35IVInterceptor = false;
            has45Interceptor = false;
            has15Absorber = false;
            has55IInterceptor = false;
            has55VIInterceptor = false;
            has55IXInterceptor = false;
        }


        public override void PostUpdate()
        {
            // 通用处理函数
            void HandleEMPUnit(ref bool hasUnit, ref int cooldown, int radius, ref bool shouldDraw, string message, Action effect)
            {
                if (hasUnit && cooldown > 0)
                {
                    cooldown--;
                    if (cooldown == 0)
                    {
                        //CombatText.NewText(Player.getRect(), Color.Cyan, $"{message}冷却成功！");
                        SoundEngine.PlaySound(SoundID.Item4, Player.position);
                        shouldDraw = true;
                    }
                }

                if (hasUnit && KeybindSystem.ClearBulletKeybind.JustPressed && cooldown == 0)
                {
                    effect?.Invoke();
                    cooldown = 45 * 60;
                    EMPAll.CreateCooldownCircle(radius);
                    shouldDraw = false;
                    //CombatText.NewText(Player.getRect(), Color.Cyan, $"{message}释放成功！");


                    {
                        Color baseColor = Color.White; // 默认值

                        if (has15Interceptor)
                            baseColor = Color.Purple;
                        else if (has25IIInterceptor)
                            baseColor = Color.Lime;
                        else if (has35IVInterceptor)
                            baseColor = Color.Bisque;
                        else if (has45Interceptor)
                            baseColor = Color.DarkGreen;
                        else if (has15Absorber)
                            baseColor = Color.MediumVioletRed;
                        else if (has55IInterceptor)
                            baseColor = Color.WhiteSmoke;
                        else if (has55VIInterceptor)
                            baseColor = Color.Cyan;
                        else if (has55IXInterceptor)
                            baseColor = Color.IndianRed;

                        EMPAll.ShootInterceptEffect(Player.Center, baseColor);

                    }
                }
                if (hasUnit && shouldDraw)
                {
                    EMPAll.CreateCooldownCircle(radius);
                }
            }

            {
                // ⛔ 不要 return！改为前置检测，然后继续往下跑
                bool hasAnyEMPUnit =
                    has15Interceptor || has25IIInterceptor || has35IVInterceptor ||
                    has45Interceptor || has15Absorber || has55IInterceptor ||
                    has55VIInterceptor || has55IXInterceptor;

                bool hasEMPVis = false;
                if (hasAnyEMPUnit)
                {
                    foreach (Projectile proj in Main.projectile)
                    {
                        if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<EMPVis2>())
                        {
                            hasEMPVis = true;
                            break;
                        }
                    }

                    if (!hasEMPVis)
                    {
                        float maxRadius = 880f;

                        if (has15Interceptor) maxRadius = 560f;
                        else if (has25IIInterceptor) maxRadius = 880f;
                        else if (has35IVInterceptor) maxRadius = 1200f;
                        else if (has45Interceptor) maxRadius = 1680f;
                        else if (has15Absorber) maxRadius = 400f;
                        else if (has55IInterceptor) maxRadius = 1200f;
                        else if (has55VIInterceptor) maxRadius = 1200f;
                        else if (has55IXInterceptor) maxRadius = 1040f;

                        Projectile.NewProjectile(
                             Player.GetSource_Misc("EMPVis"),
                            Player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<EMPVis2>(),
                            0,
                            0,
                            Player.whoAmI,
                            0f,
                            maxRadius
                        );
                    }
                }
            }

            HandleEMPUnit(ref has15Interceptor, ref cooldown15Interceptor, 15, ref shouldDrawCircle15, "15型弹幕拦截器", () => {
                ClearBulletsAdvanced(15);
                EMPAll.TriggerScreenShake(15f);
            });

            HandleEMPUnit(ref has25IIInterceptor, ref cooldown25IIInterceptor, 25, ref shouldDrawCircle25, "25II型弹幕拦截器", () => {
                ClearBulletsAdvanced(25);
                ApplyDebuffToEnemies(BuffID.Confused, 60 * 5);
                EMPAll.TriggerScreenShake(25f);
            });

            HandleEMPUnit(ref has35IVInterceptor, ref cooldown35IVInterceptor, 35, ref shouldDrawCircle35, "35IV型弹幕拦截器", () => {
                ClearBulletsAdvanced(35);
                ShootRandomLasers(35);
                EMPAll.TriggerScreenShake(35f);
            });

            HandleEMPUnit(ref has45Interceptor, ref cooldown45Interceptor, 45, ref shouldDrawCircle45, "45型转化弹幕拦截器", () => {
                ClearBulletsAdvanced(45, EMPClearMode.Reflect, 0.5f);
                EMPAll.TriggerScreenShake(45f);
            });

            HandleEMPUnit(ref has15Absorber, ref cooldown15Absorber, 15, ref shouldDrawCircle15, "15型能量吸收装置", () => {
                ClearBulletsAdvanced(15, EMPClearMode.IncreaseDamage, 3.5f, 300);
                EMPAll.TriggerScreenShake(55f);
            });

            HandleEMPUnit(ref has55IInterceptor, ref cooldown55IInterceptor, 55, ref shouldDrawCircle55, "55I型连锁引爆炮", () => {
                ClearBulletsAdvanced(55, EMPClearMode.Explode, 5f);
                EMPAll.TriggerScreenShake(55f);
            });

            HandleEMPUnit(ref has55VIInterceptor, ref cooldown55VIInterceptor, 55, ref shouldDrawCircle55, "55VI型震荡弹射器", () => {
                ClearBulletsAdvanced(55, EMPClearMode.Stun, ModContent.BuffType<Uranium235Debuff>(), 3f);
                EMPAll.TriggerScreenShake(55f);
            });

            HandleEMPUnit(ref has55IXInterceptor, ref cooldown55IXInterceptor, 55, ref shouldDrawCircle55, "55IX型转化弹幕拦截器", () => {
                ClearBulletsAdvanced(55, EMPClearMode.Reflect, 1f);
                EMPAll.TriggerScreenShake(55f);
            });


            foreach (Dust dust in Main.dust)
            {
                if (dust.active && dust.customData is float rotationSpeed)
                {
                    dust.rotation += rotationSpeed;
                    dust.scale *= 0.95f;
                    dust.alpha += 10;
                }
            }
        }



        // 清除敌方弹幕[包含多种不同模式]
        private enum EMPClearMode
        {
            Default = 0,       // 只清除
            Reflect = 1,       // 反转敌弹
            IncreaseDamage = 2, // 吸收增强玩家伤害
            Explode = 3,       // 清除后对附近敌人造成伤害
            Stun = 4           // 清除后对敌人施加debuff
        }

        // 这是新的更统一的函数，高度集成
        private void ClearBulletsAdvanced(
            int radius,
            EMPClearMode mode = EMPClearMode.Default,
            float extraValue1 = 0f,
            float extraValue2 = 0f)
        {
            int clearedCount = 0;

            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.hostile && !proj.friendly && Vector2.Distance(proj.Center, Player.Center) <= radius * 16f)
                {
                    switch (mode)
                    {
                        case EMPClearMode.Reflect:
                            if (Main.rand.NextFloat() <= extraValue1) // 概率反射
                            {
                                proj.hostile = false;
                                proj.friendly = true;
                                proj.damage *= 3;
                                proj.velocity = -proj.velocity;
                                EMPAll.FireEMPUse(Player.Center, proj.Center);
                            }
                            break;

                        case EMPClearMode.Explode:
                            foreach (NPC npc in Main.npc)
                            {
                                if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, proj.Center) <= 5 * 16f)
                                {
                                    int damage = (int)(proj.damage * extraValue1); // 倍率伤害
                                    npc.SimpleStrikeNPC(damage, 0, true, 0f, DamageClass.Generic, false);
                                    EMPAll.FireEMPUse(Player.Center, npc.Center);
                                }
                            }
                            EMPAll.FireEMPUse(Player.Center, proj.Center);
                            proj.Kill();
                            break;

                        default:
                            EMPAll.FireEMPUse(Player.Center, proj.Center);
                            proj.Kill();
                            break;
                    }

                    if (mode != EMPClearMode.Reflect)
                        clearedCount++;
                }
            }

            // 🧪 吸收伤害增益
            if (mode == EMPClearMode.IncreaseDamage && clearedCount > 0)
            {
                empDamageIncrease = extraValue1 * clearedCount / 100f;
                Player.AddBuff(ModContent.BuffType<EMPIncreasedDamageBuff>(), (int)extraValue2);
            }

            // 🌀 定身敌人
            if (mode == EMPClearMode.Stun)
            {
                int buffID = (int)extraValue1;
                int duration = (int)(extraValue2 * 60);

                foreach (NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && Vector2.Distance(npc.Center, Player.Center) <= radius * 16f)
                    {
                        npc.AddBuff(buffID, duration);
                    }
                }
            }

            SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/EMP"), Player.position);
        }



        // 给所有敌人施加debuff
        private void ApplyDebuffToEnemies(int buffID, int buffDuration)
        {
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly)
                {
                    npc.AddBuff(buffID, buffDuration);
                }
            }
        }

        // 射出随机激光
        private void ShootRandomLasers(int radius)
        {
            int laserCount = 20;
            for (int i = 0; i < laserCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(10f, 20f);
                Projectile.NewProjectile(Player.GetSource_Misc("Laser"), Player.Center, velocity, ProjectileID.PurpleLaser, Main.rand.Next(30, 51), 0, -1);
            }
        }






        //  Ui调的绘制

        public void DrawCooldownBar(SpriteBatch spriteBatch, float current, float max) // 另外几个文件也能享受
        {
            Texture2D barBG = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/UI/GenericBarBack").Value;
            Texture2D barFG = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/UI/GenericBarFront").Value;


            Vector2 screenRatioPos = new Vector2(0.5f, 0.5f); // 暂时居中绘制
            Vector2 drawPos = new Vector2(Main.screenWidth * screenRatioPos.X, Main.screenHeight * screenRatioPos.Y);
            drawPos -= barBG.Size() / 2f;
            drawPos += new Vector2(0f, 2 * 16f); // 向下偏移 X格
            //这里我讲解一下：
            //第1句话： Vector2 screenRatioPos = new Vector2(0.5f, 0.5f); // 暂时居中绘制
            //Vector2：二维向量，表示一个(X, Y) 坐标
            //screenRatioPos：意思是“屏幕比例位置”
            //new Vector2(0.5f, 0.5f)：表示我们想让东西绘制在屏幕的 50 % 宽度、50 % 高度，也就是屏幕正中央
            //注意这是一个比例，不是一个值！

            //第2句话：Vector2 drawPos = new Vector2(Main.screenWidth * screenRatioPos.X, Main.screenHeight * screenRatioPos.Y);
            //我们将屏幕的宽和高各自乘一半：
            //→ screenRatioPos.X = 0.5，就会乘出屏幕宽度的一半
            //→ screenRatioPos.Y = 0.5，也就是屏幕高度的一半
            //最终定位在 屏幕正中央的绝对像素坐标

            //然后最后一步，我们微调：drawPos -= barBG.Size() / 2f;
            //这句话可以让冷却条集中绘制，而不是从左开始

            // 之前弹幕里的逻辑是这条：Vector2 drawPos = Owner.Center - Main.screenPosition + new Vector2(0, -36) - barBG.Size() / 2f;
            //Owner.Center 是弹幕 Projectile.owner 所指向的 Player 的世界坐标
            //Main.screenPosition 是摄像机左上角的世界坐标
            //所以 Owner.Center - Main.screenPosition 就是玩家在屏幕上的绘制位置（会跟随摄像头移动）
            //+new Vector2(0, -36) 把它往头顶提一截
            //- barBG.Size() / 2 保证居中绘制
            //这套逻辑没问题，因为弹幕本身已经运行在世界坐标系统内，所以画特效 / 贴图也就是“跟着玩家的世界位置移动”。但你原来写在 ModPlayer 的 ModifyInterfaceLayers 里，那里是 HUD 层，不适合用世界坐标。

            float fillRatio = MathHelper.Clamp(current / max, 0f, 1f);
            Rectangle frame = new Rectangle(0, 0, (int)(barFG.Width * fillRatio), barFG.Height);

            float t = Main.GlobalTimeWrappedHourly;
            float lum = 0.85f + (float)Math.Sin(t * 3f) * 0.1f;
            Color color = Main.hslToRgb((t * 0.6f) % 1f, 1f, lum);

            spriteBatch.Draw(barBG, drawPos, color);
            spriteBatch.Draw(barFG, drawPos, frame, color * 0.8f);
        }
    }
}