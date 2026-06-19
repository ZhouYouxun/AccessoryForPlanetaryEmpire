using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class PurpleES : ModProjectile
    {
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
            Projectile.scale = 1.0f; // 通过调整缩放比例来变大子弹贴图
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f); // 旋转弹幕方向

            // 更明显且炫酷的 DNA 形状的 Torch dust
            float sineWave = (float)Math.Sin(Projectile.timeLeft / 10f) * 5f;
            for (int i = 0; i < 4; i++) // 增加粒子数量
            {
                Vector2 dustPos = Projectile.position - Projectile.velocity * ((float)i / 4f);
                dustPos += new Vector2(0, sineWave);
                Dust dust = Dust.NewDustPerfect(dustPos, DustID.Torch, null, 100, Color.Purple, 5f); // 改变颜色和大小
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += Projectile.velocity * 0.2f; // 增加速度
            }
        }

        public override void OnKill(int timeLeft)
        {
            // 从敌人左边和右边各自召唤5发高速子弹射向敌人
            for (int i = 0; i < 5; i++)
            {
                Vector2 leftVelocity = new Vector2(-1f, Main.rand.NextFloat(-0.5f, 0.5f)) * 10f;
                Vector2 rightVelocity = new Vector2(1f, Main.rand.NextFloat(-0.5f, 0.5f)) * 10f;

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, leftVelocity, ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack, Projectile.owner);
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, rightVelocity, ProjectileID.BulletHighVelocity, Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/Weapon/PurpleES").Value;
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
