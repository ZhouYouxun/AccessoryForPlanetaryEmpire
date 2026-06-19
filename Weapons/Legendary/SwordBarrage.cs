using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Drawing;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AccessoryForPlanetaryEmpire.Weapons.Legendary
{
    internal class SwordBarrage : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4; // 刀光动画有4帧
        }

        public override void SetDefaults()
        {
            // 设置投射物的基本属性
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.ownerHitCheck = true;
            Projectile.ownerHitCheckDistance = 300f;
            Projectile.usesOwnerMeleeHitCD = true;
            Projectile.aiStyle = -1;
            Projectile.noEnchantmentVisuals = true; // 不显示附魔特效
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;

        }

        public override void AI()
        {
            Projectile.localAI[0]++; // 更新存活时间
            Player player = Main.player[Projectile.owner];

            // 计算挥砍的旋转角度
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1];
            float direction = Projectile.ai[0];
            float velocityRotation = Projectile.velocity.ToRotation();
            float adjustedRotation = MathHelper.Pi * direction * percentageOfLife + velocityRotation + direction * MathHelper.Pi + player.fullRotation;
            Projectile.rotation = adjustedRotation; // 设置旋转角度

            // 随玩家的挥动而移动
            Projectile.Center = player.RotatedRelativePoint(player.MountedCenter) - Projectile.velocity;

            // 设置缩放
            float baseScale = Projectile.ai[2]; // 从 ai[2] 获取初始缩放（由发射者传入）
            float scaleMulti = 0.3f;
            Projectile.scale = baseScale + percentageOfLife * scaleMulti;


            // 在挥砍弧线中生成粒子
            float dustRotation = Projectile.rotation + Main.rand.NextFloatDirection() * MathHelper.PiOver2 * 0.7f;
            Vector2 dustPosition = Projectile.Center + dustRotation.ToRotationVector2() * 84f * Projectile.scale;
            Vector2 dustVelocity = (dustRotation + Projectile.ai[0] * MathHelper.PiOver2).ToRotationVector2();

            if (Main.rand.NextFloat() * 2f < Projectile.Opacity)
            {
                Color dustColor = Color.Lerp(Color.SkyBlue, Color.White, Main.rand.NextFloat() * 0.3f);
                Dust coloredDust = Dust.NewDustPerfect(Projectile.Center + dustRotation.ToRotationVector2() * (Main.rand.NextFloat() * 80f * Projectile.scale + 20f * Projectile.scale), DustID.FireworksRGB, dustVelocity * 1f, 100, dustColor, 0.4f);
                coloredDust.fadeIn = 0.4f + Main.rand.NextFloat() * 0.15f;
                coloredDust.noGravity = true;
            }


            // 在挥砍弧线中生成粒子（已存在代码段）下方添加：
            Vector2 forward = Projectile.rotation.ToRotationVector2(); // 正前方方向
            Vector2 spawnPos = Projectile.Center + forward * 48f * Projectile.scale; // 向正前方偏移

            if (Main.rand.NextBool(1))
            {
                int dustID = Dust.NewDust(spawnPos, 0, 0, DustID.SilverFlame, 0f, 0f, 100, default, Main.rand.NextFloat(1.1f, 1.7f));
                Dust d = Main.dust[dustID];
                d.velocity = forward.RotatedByRandom(MathHelper.ToRadians(10f)) * Main.rand.NextFloat(0.3f, 1.2f);
                d.noGravity = true;
                d.color = Color.Lerp(Color.Silver, Color.Purple, Main.rand.NextFloat(0.3f, 0.7f));
                d.fadeIn = 0.3f;
            }


            if (Projectile.localAI[0] >= Projectile.ai[1])
            {
                Projectile.Kill(); // 超过最大存活时间时销毁投射物
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // 计算刀光碰撞的长度，基于投射物的缩放比例
            float coneLength = 94f * Projectile.scale;

            // 计算碰撞检测的旋转角度，基于挥砍方向（Projectile.ai[0]）
            float collisionRotation = MathHelper.Pi * 2f / 25f * Projectile.ai[0];

            // 设置最大角度，即碰撞区域的弧度范围，这里为 45 度（Pi/4）
            float maximumAngle = MathHelper.PiOver4;

            // 计算碰撞区域的旋转角度，加上挥砍的旋转
            float coneRotation = Projectile.rotation + collisionRotation;

            // 检查目标是否在挥砍弧线的碰撞区域内
            if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation, maximumAngle))
            {
                // 如果目标在主挥砍弧线的碰撞区域内，返回 true，表示发生碰撞
                return true;
            }

            // 计算挥砍弧线的后半部分，确保挥砍的整个弧线都能检测到碰撞
            float backOfTheSwing = Utils.Remap(Projectile.localAI[0], Projectile.ai[1] * 0.3f, Projectile.ai[1] * 0.5f, 1f, 0f);

            // 如果后半部分的挥砍还在进行，继续检测
            if (backOfTheSwing > 0f)
            {
                // 计算后半挥砍弧线的碰撞区域
                float coneRotation2 = coneRotation - MathHelper.PiOver4 * Projectile.ai[0] * backOfTheSwing;

                // 检查目标是否在后半挥砍弧线的碰撞区域内
                if (targetHitbox.IntersectsConeSlowMoreAccurate(Projectile.Center, coneLength, coneRotation2, maximumAngle))
                {
                    // 如果目标在后半挥砍弧线的碰撞区域内，返回 true，表示发生碰撞
                    return true;
                }
            }

            // 如果没有检测到碰撞，返回 false
            return false;
        }

        // 用于破坏场景中的物件，如草丛、花朵、瓷砖等。
        public override void CutTiles()
        {
            Vector2 starting = (Projectile.rotation - MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            Vector2 ending = (Projectile.rotation + MathHelper.PiOver4).ToRotationVector2() * 60f * Projectile.scale;
            float width = 60f * Projectile.scale;
            Utils.PlotTileLine(Projectile.Center + starting, Projectile.Center + ending, width, DelegateMethods.CutTiles);
        }

        // 击中 NPC（敌人）时触发
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 所有可选的粒子特效
            ParticleOrchestraType[] choices = new[]
            {
                ParticleOrchestraType.Keybrand,
                ParticleOrchestraType.NightsEdge,
                ParticleOrchestraType.TrueNightsEdge,
                ParticleOrchestraType.Excalibur,
                ParticleOrchestraType.TrueExcalibur,
                ParticleOrchestraType.TerraBlade,
                ParticleOrchestraType.RainbowRodHit,
                ParticleOrchestraType.SilverBulletSparkle,
                ParticleOrchestraType.ShimmerArrow
            };

            // 随机选 ？ 个不同的特效
            var selected = choices.OrderBy(_ => Main.rand.Next()).Take(5);

            foreach (var effect in selected)
            {
                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, effect,
                    new ParticleOrchestraSettings
                    {
                        PositionInWorld = Main.rand.NextVector2FromRectangle(target.Hitbox)
                    }, Projectile.owner);
            }

            hit.HitDirection = (Main.player[Projectile.owner].Center.X < target.Center.X) ? 1 : (-1);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 获取投射物中心的屏幕位置
            Vector2 position = Projectile.Center - Main.screenPosition;

            // 加载投射物的纹理，这里使用的是 TheSixthArkProjectile 的纹理
            //Texture2D texture = ModContent.Request<Texture2D>("CalamityYoYo/Weapons/OtherWeapons/TheSixthArkProjectile").Value;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            // 选择投射物的动画帧，刀光的动画有 4 帧
            Rectangle sourceRectangle = texture.Frame(1, 4);

            // 设置原点为图片中心，用于旋转时的参考点
            Vector2 origin = sourceRectangle.Size() / 2f;

            // 设置投射物的缩放比例，基础比例乘以 1.1 倍
            float scale = Projectile.scale * 1.1f;

            // 根据投射物的 AI 来决定是否翻转贴图，Projectile.ai[0] 大于等于 0 时不翻转
            SpriteEffects spriteEffects = ((!(Projectile.ai[0] >= 0f)) ? SpriteEffects.FlipVertically : SpriteEffects.None);

            // 计算投射物当前生命周期的百分比，用于控制动画和粒子的变化
            float percentageOfLife = Projectile.localAI[0] / Projectile.ai[1];

            // 用于平滑动画的时间插值，通过 Remap 函数将生命周期映射到 0 到 1 的范围
            float lerpTime = Utils.Remap(percentageOfLife, 0f, 0.6f, 0f, 1f) * Utils.Remap(percentageOfLife, 0.6f, 1f, 1f, 0f);

            // 获取当前投射物位置的光照颜色，Lighting.GetColor 用于计算光照强度
            float lightingColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates()).ToVector3().Length() / (float)Math.Sqrt(3.0);

            // 进一步调整光照颜色，使其映射到 0 到 1 的范围
            lightingColor = Utils.Remap(lightingColor, 0.2f, 1f, 0f, 1f);

            // 设置刀光的颜色（根据传入的值决定）
            float r = Projectile.localAI[0];
            float g = Projectile.localAI[1];
            float b = Projectile.localAI[2];

            Color backDarkColor;
            Color middleMediumColor;
            Color frontLightColor;

            if (r == 0f && g == 0f && b == 0f)
            {
                // 默认：银白紫三色
                backDarkColor = new Color(160, 160, 180);
                middleMediumColor = new Color(200, 200, 220);
                frontLightColor = new Color(255, 255, 255);
            }
            else
            {
                Color overrideColor = new Color(r, g, b);
                backDarkColor = overrideColor;
                middleMediumColor = overrideColor;
                frontLightColor = overrideColor;
            }


            // 用白色结合 lerpTime 和透明度生成一个渐变的颜色，光效的基本色
            Color whiteTimesLerpTime = Color.White * lerpTime * 0.5f;

            // 根据光照颜色调整透明度，将白色的 Alpha 渐变调整到 1 到 0 范围
            whiteTimesLerpTime.A = (byte)(whiteTimesLerpTime.A * (1f - lightingColor));

            // 生成微弱的光效颜色，通过光照和透明度计算出的颜色
            Color faintLightingColor = whiteTimesLerpTime * lightingColor * 0.5f;
            faintLightingColor.G = (byte)(faintLightingColor.G * lightingColor); // 调整绿色分量
            faintLightingColor.B = (byte)(faintLightingColor.R * (0.25f + lightingColor * 0.75f)); // 调整蓝色分量

            // 绘制背部的光效，红色背景光效，随挥砍过程变化
            Main.EntitySpriteDraw(texture, position, sourceRectangle, backDarkColor * lightingColor * lerpTime,
                Projectile.rotation + Projectile.ai[0] * MathHelper.PiOver4 * -1f * (1f - percentageOfLife), origin, scale, spriteEffects, 0f);

            // 绘制微弱的光效，作为背景光效的补充，使效果更加柔和
            Main.EntitySpriteDraw(texture, position, sourceRectangle, faintLightingColor * 0.15f,
                Projectile.rotation + Projectile.ai[0] * 0.01f, origin, scale, spriteEffects, 0f);

            // 绘制中间的光效，较亮的红色光效
            Main.EntitySpriteDraw(texture, position, sourceRectangle, middleMediumColor * lightingColor * lerpTime * 0.3f,
                Projectile.rotation, origin, scale, spriteEffects, 0f);

            // 绘制前端光效，浅红色，前端的光效较亮
            Main.EntitySpriteDraw(texture, position, sourceRectangle, frontLightColor * lightingColor * lerpTime * 0.5f,
                Projectile.rotation, origin, scale * 0.975f, spriteEffects, 0f);

            // 生成其他粒子效果，刀光挥砍路径上的星光效果
            for (float i = 0f; i < 8f; i += 1f)
            {
                // 计算每个粒子在挥砍路径中的旋转角度
                float edgeRotation = Projectile.rotation + Projectile.ai[0] * i * (MathHelper.Pi * -2f) * 0.025f +
                    Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) * Projectile.ai[0];

                // 计算粒子的位置，通过旋转和位置偏移
                Vector2 drawPos = position + edgeRotation.ToRotationVector2() * ((float)texture.Width * 0.5f - 6f) * scale;

                // 调用 DrawPrettyStarSparkle 绘制星光效果
                DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos,
                    new Color(255, 255, 255, 0) * lerpTime * (i / 9f), middleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f,
                    edgeRotation, new Vector2(0f, Utils.Remap(percentageOfLife, 0f, 1f, 3f, 0f)) * scale, Vector2.One * scale);
            }

            // 生成额外的一颗星光效果
            Vector2 drawPos2 = position + (Projectile.rotation + Utils.Remap(percentageOfLife, 0f, 1f, 0f, MathHelper.PiOver4) *
                Projectile.ai[0]).ToRotationVector2() * ((float)texture.Width * 0.5f - 4f) * scale;

            // 绘制额外的星光粒子效果
            DrawPrettyStarSparkle(Projectile.Opacity, SpriteEffects.None, drawPos2,
                new Color(255, 255, 255, 0) * lerpTime * 0.5f, middleMediumColor, percentageOfLife, 0f, 0.5f, 0.5f, 1f, 0f,
                new Vector2(2f, Utils.Remap(percentageOfLife, 0f, 1f, 4f, 1f)) * scale, Vector2.One * scale);

            // 返回 false 表示不执行默认的投射物绘制行为，完全使用自定义绘制
            return false;
        }


        private static void DrawPrettyStarSparkle(float opacity, SpriteEffects dir, Vector2 drawPos, Color drawColor, Color shineColor, float flareCounter, float fadeInStart, float fadeInEnd, float fadeOutStart, float fadeOutEnd, float rotation, Vector2 scale, Vector2 fatness)
        {
            // 获取星光粒子的纹理，ID 为 98 的额外材质
            Texture2D sparkleTexture = TextureAssets.Extra[98].Value;

            // 计算大光斑的颜色，颜色为 shineColor，透明度乘以 opacity 的一半
            Color bigColor = shineColor * opacity * 0.5f;
            // 将大光斑的 Alpha 设置为 0，使其逐渐消失
            bigColor.A = 0;

            // 计算纹理的原点为图片中心，方便后面的旋转
            Vector2 origin = sparkleTexture.Size() / 2f;

            // 计算小光斑的颜色，颜色为 drawColor，透明度乘以 0.5
            Color smallColor = drawColor * 0.5f;

            // 计算 lerp 值，用于平滑光斑的出现和消失
            float lerpValue = Utils.GetLerpValue(fadeInStart, fadeInEnd, flareCounter, clamped: true) * Utils.GetLerpValue(fadeOutEnd, fadeOutStart, flareCounter, clamped: true);

            // 计算左右方向的缩放比例，用 fatness 来控制光斑的形状
            Vector2 scaleLeftRight = new Vector2(fatness.X * 0.5f, scale.X) * lerpValue;

            // 计算上下方向的缩放比例
            Vector2 scaleUpDown = new Vector2(fatness.Y * 0.5f, scale.Y) * lerpValue;

            // 根据 lerpValue 调整大光斑的透明度
            bigColor *= lerpValue;

            // 根据 lerpValue 调整小光斑的透明度
            smallColor *= lerpValue;

            // 绘制第一个大光斑，旋转 90 度，左右方向的缩放
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight, dir);

            // 绘制第二个大光斑，旋转 0 度，上下方向的缩放
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, bigColor, 0f + rotation, origin, scaleUpDown, dir);

            // 绘制第一个小光斑，缩放值为大光斑的 0.6 倍，旋转 90 度，左右方向的缩放
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, MathHelper.PiOver2 + rotation, origin, scaleLeftRight * 0.6f, dir);

            // 绘制第二个小光斑，缩放值为大光斑的 0.6 倍，旋转 0 度，上下方向的缩放
            Main.EntitySpriteDraw(sparkleTexture, drawPos, null, smallColor, 0f + rotation, origin, scaleUpDown * 0.6f, dir);
        }





    }

}
