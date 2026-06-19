using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.GameContent;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    internal class MedicalDrones : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 50;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override string Texture => "Terraria/Images/Extra_89"; // 使用原版光点

        public override bool PreDraw(ref Color lightColor)
        {
            // 📍贴图准备
            Texture2D ringTex = TextureAssets.Extra[89].Value;
            Texture2D gearTex1 = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/diamondgear_007").Value;
            Texture2D gearTex2 = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Texture/STP/diamondgear_016").Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition + Vector2.UnitY * Projectile.gfxOffY;
            Vector2 origin = ringTex.Size() * 0.5f;

            float pulse = 1f + 0.08f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 5f);

            // ✴️Extra_89 多层脉冲
            for (int i = 0; i < 3; i++)
            {
                float scale = (0.25f + 0.05f * i) * pulse * 2.5f;
                Color ringColor = new Color(80, 255, 100, 0) * 1.6f;
                float angle = Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * (0.4f + i * 0.1f);

                Main.EntitySpriteDraw(ringTex, drawPos, null, ringColor, angle, origin, scale, SpriteEffects.None, 0);
            }

            // ⚙️齿轮 1 顺时针
            float angle1 = Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 0.3f;
            Main.EntitySpriteDraw(gearTex1, drawPos, null, Color.White * 0.7f, angle1, gearTex1.Size() * 0.5f, 0.1f, SpriteEffects.None, 0);

            // ⚙️齿轮 2 逆时针
            float angle2 = -Main.GlobalTimeWrappedHourly * MathHelper.TwoPi * 0.4f;
            Main.EntitySpriteDraw(gearTex2, drawPos, null, Color.White * 0.7f, angle2, gearTex2.Size() * 0.5f, 0.1f, SpriteEffects.None, 0);

            return false;
        }




        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Default;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1200; // 存活
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (Projectile.timeLeft % 2 == 0)
            {
                PLALightingBoltsSystem.PLA_Spawn_MedicalPulse(Projectile.Center);
            }

            //for (int i = 0; i < 5; i++)
            //{
            //    float radius = 10f + 4f * (float)Math.Sin(i + Main.GlobalTimeWrappedHourly * 6f);
            //    float angle = MathHelper.TwoPi * i / 20f + Main.GlobalTimeWrappedHourly * 1.5f;

            //    Vector2 offset = angle.ToRotationVector2() * radius;
            //    Vector2 velocity = offset.SafeNormalize(Vector2.One) * Main.rand.NextFloat(2.5f, 4f);

            //    int dustType = Main.rand.NextBool() ? DustID.WhiteTorch : DustID.GemDiamond;
            //    Dust d = Dust.NewDustPerfect(Projectile.Center + offset, dustType, velocity);
            //    d.noGravity = true;
            //    d.scale = Main.rand.NextFloat(1.0f, 1.6f);
            //}




            if (Projectile.timeLeft > 1160)
            {
                Projectile.localAI[0] += 1f;

                if (Projectile.localAI[0] % 90f == 0f)
                {
                    // 每隔一段时间，改变“目标游荡方向”
                    Projectile.ai[0] = Main.rand.NextFloat(MathHelper.TwoPi);
                }

                // 使用 ai[0] 表示当前“偏好的漂移方向”
                Vector2 wanderDir = new Vector2((float)Math.Cos(Projectile.ai[0]), (float)Math.Sin(Projectile.ai[0]));

                // 缓慢趋向目标方向，加上一点扰动
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, wanderDir * 2f, 0.02f);
                Projectile.velocity += Main.rand.NextVector2Circular(0.1f, 0.1f);
                Projectile.velocity *= 0.98f;
            }
            else
            {
                Projectile.ai[1] += 0.2f;
                float maxSpeed = MathHelper.Clamp(Projectile.ai[1], 4f, 98f);

                Vector2 toPlayer = player.Center - Projectile.Center;

                // 🔁 添加旋转扰动：每帧一个小角度旋转，用来模拟螺旋轨迹
                float spinStrength = 0.2f; // 越大越剧烈
                float spinTime = Projectile.timeLeft * 0.1f; // 控制角度变化
                Vector2 spiralOffset = toPlayer.RotatedBy(Math.Sin(spinTime) * spinStrength);

                // 🚀 加速靠近（目标是 spiralOffset，而非直线）
                Vector2 desiredVelocity = spiralOffset.SafeNormalize(Vector2.Zero) * maxSpeed;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desiredVelocity, 0.12f);

                // ❤️ 碰撞回血
                if (Projectile.Hitbox.Intersects(player.Hitbox))
                {
                    int heal = (int)Projectile.ai[0]; // 读取主弹幕提供的回血值
                    player.statLife += heal;
                    player.HealEffect(heal, true);
                    Projectile.Kill();
                }
            }





        }







    }
}