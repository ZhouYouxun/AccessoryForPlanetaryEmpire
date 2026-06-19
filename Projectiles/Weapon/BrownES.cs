using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class BrownES : ModProjectile
    {
        private float amplitude = 10f; // 波动幅度
        private float frequency = 0.1f; // 波动频率

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.scale = 1.0f;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // 旋转弹幕方向

            // 使用sin函数使弹幕波动
            float sineWave = (float)Math.Sin(Projectile.timeLeft * frequency) * amplitude;
            Projectile.position.Y += sineWave;

            // 棕色粒子效果
            for (int i = 0; i < 4; i++)
            {
                Vector2 dustPos = Projectile.position - Projectile.velocity * ((float)i / 4f);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Dirt, null, 100, Color.Brown, 1.8f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.2f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Weapon/BrownES").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);

            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }
            return true;
        }
    }
}
