using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class ShinySlimeShieldProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Slime Shield");
        }
        public override string Texture => "Terraria/Images/Extra_89"; // 使用原版光点

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;
            Projectile.timeLeft = 600;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.98f;

            {
                float time = Main.GlobalTimeWrappedHourly;

                // 🌊 波浪数量（越多越复杂）
                int waveCount = 3;

                for (int wave = 0; wave < waveCount; wave++)
                {
                    for (int i = 0; i < 8; i++) // 每波8个点
                    {
                        // 🎯 动态角度：基础角 + 旋转偏移 + 波动
                        float baseAngle = MathHelper.TwoPi * i / 8f;
                        float rotationOffset = time * 0.7f; // 整体旋转速度
                        float waveOffset = (float)Math.Sin(time * 4f + wave * 1.2f + i) * 0.4f;

                        float angle = baseAngle + rotationOffset + waveOffset;
                        Vector2 dir = angle.ToRotationVector2();

                        // 🌀 半径：绕着50像素为中心上下波动
                        float radius = 50f + (float)Math.Sin(time * 2.5f + i + wave * 0.5f) * 6f;
                        Vector2 spawnPos = Projectile.Center + dir * radius;

                        // 💨 速度方向略微带切向扰动
                        Vector2 velocity = -dir * Main.rand.NextFloat(0.8f, 1.4f) +
                                           dir.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-0.4f, 0.4f);

                        int type = Utils.SelectRandom(Main.rand, DustID.PinkTorch, DustID.PinkTorch, DustID.Confetti_Pink);
                        Dust d = Dust.NewDustPerfect(spawnPos, type, velocity, 100, Color.HotPink * 0.8f, Main.rand.NextFloat(1f, 1.5f));
                        d.noGravity = true;
                        d.fadeIn = 1f;
                    }
                }
            }


            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile other = Main.projectile[i];
                if (other.active && other.hostile && other.Hitbox.Intersects(Projectile.Hitbox))
                {
                    other.Kill();
                    Projectile.penetrate--;
                    if (Projectile.penetrate <= 0)
                    {
                        Projectile.Kill();
                    }
                }
            }

            if (Projectile.timeLeft % 2 == 0)
            {
                for (int i = Projectile.oldPos.Length - 1; i > 0; i--)
                {
                    Projectile.oldPos[i] = Projectile.oldPos[i - 1];
                }
                Projectile.oldPos[0] = Projectile.position;
            }
        }

        //public override bool PreDraw(ref Color lightColor)
        //{
        //    // 获取投射物的纹理
        //    Texture2D texture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Accessory/ShinySlimeShieldProjectile").Value;

        //    // 遍历投射物的旧位置数组，绘制光学拖尾效果
        //    for (int i = 0; i < Projectile.oldPos.Length; i++)
        //    {
        //        // 跳过未初始化的位置
        //        if (Projectile.oldPos[i] == Vector2.Zero) continue;

        //        // 计算颜色插值值，使颜色在旧位置之间平滑过渡
        //        float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;

        //        // 插值计算出光学拖尾颜色，逐渐变为半透明
        //        Color color = Color.Lerp(Color.Pink, Color.Silver, colorInterpolation) * 0.4f; // 修改为粉红色
        //        color.A = 0; // 设置透明度

        //        // 计算绘制位置，将位置调整到碰撞箱的中心
        //        Vector2 drawPosition = Projectile.oldPos[i] + texture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-28f, -28f);

        //        // 计算外部和内部的颜色
        //        Color outerColor = color;
        //        Color innerColor = color * 0.5f;

        //        // 计算强度，使拖尾逐渐变弱
        //        float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
        //        intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
        //        if (Projectile.timeLeft <= 60)
        //        {
        //            intensity *= Projectile.timeLeft / 60f; // 如果弹幕即将消失，则拖尾也逐渐消失
        //        }

        //        // 计算外部和内部的缩放比例，使拖尾具有渐变效果
        //        Vector2 outerScale = new Vector2(2f) * intensity;
        //        Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
        //        outerColor *= intensity;
        //        innerColor *= intensity;

        //        // 绘制外部的拖尾效果
        //        Main.EntitySpriteDraw(texture, drawPosition, null, outerColor, 0f, texture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);

        //        // 绘制内部的拖尾效果
        //        Main.EntitySpriteDraw(texture, drawPosition, null, innerColor, 0f, texture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
        //    }
        //    return false; // 告诉游戏我们已经手动绘制了投射物，不需要默认绘制
        //}


        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float sizeMultiplier = 0.25f;

            Texture2D lightTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/light_02").Value;
            Texture2D magicTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_05").Value;
            Texture2D starTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_09").Value;

            float time = Main.GlobalTimeWrappedHourly;
            float pulse = 1f + (float)Math.Sin(time * 2f) * 0.08f;

            // 🎀 外圈粉紫脉冲发光
            spriteBatch.Draw(lightTex, drawPos, null, Color.Pink * 0.5f, -time * 0.4f, lightTex.Size() / 2f, sizeMultiplier * 1.3f, SpriteEffects.None, 0f);

            // 🌀 中层魔法旋纹
            spriteBatch.Draw(magicTex, drawPos, null, Color.MediumVioletRed * 0.7f, time * 0.5f, magicTex.Size() / 2f, sizeMultiplier * 1.1f * pulse, SpriteEffects.None, 0f);

            // 💖 中心心核双层旋转、脉动
            float centerPulse = 1f + (float)Math.Sin(time * 1.8f) * 0.07f;
            float rot1 = time * 0.65f;
            float rot2 = -time * 0.65f;

            spriteBatch.Draw(starTex, drawPos, null, Color.White, rot1, starTex.Size() / 2f, sizeMultiplier * 0.85f * centerPulse, SpriteEffects.None, 0f);
            spriteBatch.Draw(starTex, drawPos, null, Color.White, rot2, starTex.Size() / 2f, sizeMultiplier * 0.85f * centerPulse, SpriteEffects.None, 0f);

            return false;
        }


    }
}
