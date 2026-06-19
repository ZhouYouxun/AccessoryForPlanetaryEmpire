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
    public class SlimeShieldProjectile : ModProjectile
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
            Projectile.velocity *= 0.97f;

            {
                float time = Main.GlobalTimeWrappedHourly;

                int waveCount = 2;
                for (int wave = 0; wave < waveCount; wave++)
                {
                    for (int i = 0; i < 6; i++) // 每波6个点
                    {
                        float baseAngle = MathHelper.TwoPi * i / 6f;
                        float rotationOffset = time * 0.5f;
                        float waveOffset = (float)Math.Sin(time * 3f + wave + i) * 0.3f;

                        float angle = baseAngle + rotationOffset + waveOffset;
                        Vector2 dir = angle.ToRotationVector2();

                        float radius = 50f + (float)Math.Cos(time * 1.5f + i + wave) * 4f;
                        Vector2 spawnPos = Projectile.Center + dir * radius;

                        Vector2 velocity = -dir * Main.rand.NextFloat(0.7f, 1.2f); // ✅ 只保留收缩速度

                        int type = Utils.SelectRandom(Main.rand, DustID.BlueTorch, DustID.Electric, DustID.IceRod);
                        Dust d = Dust.NewDustPerfect(spawnPos, type, velocity, 100, Color.LightBlue * 0.7f, Main.rand.NextFloat(1f, 1.4f));
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
        //    Texture2D texture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Accessory/SlimeShieldProjectile").Value;
        //    for (int i = 0; i < Projectile.oldPos.Length; i++)
        //    {
        //        if (Projectile.oldPos[i] == Vector2.Zero) continue;

        //        float colorInterpolation = (float)Math.Cos(Projectile.timeLeft / 32f + Main.GlobalTimeWrappedHourly / 20f + i / (float)Projectile.oldPos.Length * MathHelper.Pi) * 0.5f + 0.5f;
        //        Color color = Color.Lerp(Color.Blue, Color.Silver, colorInterpolation) * 0.4f;
        //        color.A = 0;

        //        Vector2 drawPosition = Projectile.oldPos[i] + texture.Size() * 0.5f - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) + new Vector2(-28f, -28f);
        //        Color outerColor = color;
        //        Color innerColor = color * 0.5f;

        //        float intensity = 0.9f + 0.15f * (float)Math.Cos(Main.GlobalTimeWrappedHourly % 60f * MathHelper.TwoPi);
        //        intensity *= MathHelper.Lerp(0.15f, 1f, 1f - i / (float)Projectile.oldPos.Length);
        //        if (Projectile.timeLeft <= 60)
        //        {
        //            intensity *= Projectile.timeLeft / 60f;
        //        }

        //        Vector2 outerScale = new Vector2(2f) * intensity;
        //        Vector2 innerScale = new Vector2(2f) * intensity * 0.7f;
        //        outerColor *= intensity;
        //        innerColor *= intensity;

        //        Main.EntitySpriteDraw(texture, drawPosition, null, outerColor, 0f, texture.Size() * 0.5f, outerScale * 0.6f, SpriteEffects.None, 0);
        //        Main.EntitySpriteDraw(texture, drawPosition, null, innerColor, 0f, texture.Size() * 0.5f, innerScale * 0.6f, SpriteEffects.None, 0);
        //    }
        //    return false;
        //}

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch spriteBatch = Main.spriteBatch;
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float sizeMultiplier = 0.25f;

            Texture2D lightTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/light_01").Value;
            Texture2D magicTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/magic_03").Value;
            Texture2D starTex = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/KsTexture/star_08").Value;

            float time = Main.GlobalTimeWrappedHourly;

            // 💠 外圈发光波纹旋转
            spriteBatch.Draw(lightTex, drawPos, null, Color.Cyan * 0.5f, time * 0.6f, lightTex.Size() / 2f, sizeMultiplier * 1.4f, SpriteEffects.None, 0f);

            // ✨ 魔法旋涡缓动
            float scale = 1f + (float)Math.Cos(time * 4f) * 0.1f;
            spriteBatch.Draw(magicTex, drawPos, null, Color.LightBlue * 0.7f, -time * 0.8f, magicTex.Size() / 2f, sizeMultiplier * 1.15f * scale, SpriteEffects.None, 0f);

            // 💎 中心核心双层叠加、旋转、呼吸动画
            float centerPulse = 1f + (float)Math.Sin(time * 2.2f) * 0.06f;
            float rot1 = time * 0.75f;
            float rot2 = -time * 0.75f;

            spriteBatch.Draw(starTex, drawPos, null, Color.White * 0.9f, rot1, starTex.Size() / 2f, sizeMultiplier * 0.8f * centerPulse, SpriteEffects.None, 0f);
            spriteBatch.Draw(starTex, drawPos, null, Color.White * 0.9f, rot2, starTex.Size() / 2f, sizeMultiplier * 0.8f * centerPulse, SpriteEffects.None, 0f);

            return false;
        }




    }
}
