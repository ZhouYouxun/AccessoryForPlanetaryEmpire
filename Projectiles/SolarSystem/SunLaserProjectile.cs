using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using ReLogic.Content;
using Terraria.Audio;
using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.OTHER;

namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
{
    public class SunLaserProjectile : ModProjectile
    {
        private float colorChangeTimer = 0f;
        private Color currentColor;
        private float rotationSpeed = MathHelper.Pi / 12; // 每秒旋转15度
        private Vector2 targetDirection;
        private int smallRayTimer = 0; // 计时器

        public override void SetStaticDefaults()
        {
            // 设置激光的纹理
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 84; // 70% 尺寸
            Projectile.height = 84; // 70% 尺寸
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 300; // 激光持续时间，5秒
            Projectile.scale = 7f; // 激光放大倍率（70%）
            Projectile.damage = 850; // 基础伤害
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 5;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // 将激光固定在鼠标位置
            Vector2 desiredPosition = Main.MouseWorld;
            float maxDistance = 5f * 16; // 每秒移动5格
            Vector2 direction = desiredPosition - Projectile.Center;
            float distance = direction.Length();
            if (distance > maxDistance)
            {
                direction.Normalize();
                direction *= maxDistance;
            }
            Projectile.Center += direction / 60f; // 每帧移动

            // 固定垂直方向
            Projectile.rotation = MathHelper.PiOver2;

            // 改变颜色为橙色
            currentColor = Color.Orange;

            // 生成粒子特效
            for (int i = 0; i < 5; i++)
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 100, default, 3f);
                Main.dust[dustIndex].velocity *= 3f;
                Main.dust[dustIndex].noGravity = true;
            }

            // 生成岩浆特效粒子在头部、中间和尾部
            for (float i = 0; i <= 300f * 16; i += 84f * Projectile.scale)
            {
                Vector2 particlePosition = Projectile.Center + new Vector2(0, 1).SafeNormalize(Vector2.UnitY) * i;
                int dustIndex = Dust.NewDust(particlePosition, Projectile.width, Projectile.height, 35, 0f, 0f, 100, default, 8f);
                Main.dust[dustIndex].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                Main.dust[dustIndex].noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, currentColor.ToVector3() * 1.5f);

            // 播放音效
            if (Projectile.timeLeft == 300)
            {
                //SoundEngine.PlaySound(SoundID.Zombie104, player.position);
            }

            // 添加屏幕震动效果
            TriggerScreenShake(20f); // 强度为50的屏幕震动
        }

        private void TriggerScreenShake(float intensity)
        {
            Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower = intensity;
            Main.OnPreDraw += UpdateScreenShake;
        }

        private void UpdateScreenShake(GameTime gameTime)
        {
            if (Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower > 0f)
            {
                Main.screenPosition += Main.rand.NextVector2Circular(Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower, Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower);
                Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower = MathHelper.Clamp(Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower - 0.185f, 0f, 20f);
            }
            else
            {
                Main.OnPreDraw -= UpdateScreenShake;
            }
        }

        public override bool? CanDamage()
        {
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.immune[Projectile.owner] = 10; // 无敌帧
        }

        // 碰撞检测函数，确保整个激光路径都能造成伤害
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float point = 0f;
            Vector2 endPos = Projectile.Center + new Vector2(0, 1).SafeNormalize(Vector2.UnitY) * 300f * 16;
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPos, 84f * Projectile.scale, ref point);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D startTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayStart").Value;
            Texture2D middleTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayMid").Value;
            Texture2D endTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayEnd").Value;

            Vector2 start = Projectile.Center;
            Vector2 direction = new Vector2(0, 1).SafeNormalize(Vector2.UnitY);
            float laserLength = 300f * 16; // 固定长度300格

            // 绘制激光的开始部分（下方）
            Main.EntitySpriteDraw(startTexture, start - Main.screenPosition, null, currentColor, Projectile.rotation, new Vector2(startTexture.Width / 2, startTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

            // 绘制激光的中间部分（下方）
            for (float i = 0; i <= laserLength; i += middleTexture.Height * Projectile.scale)
            {
                Vector2 drawPosition = start + direction * i;
                Main.EntitySpriteDraw(middleTexture, drawPosition - Main.screenPosition, null, currentColor, Projectile.rotation, new Vector2(middleTexture.Width / 2, middleTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

                // 生成岩浆特效粒子
                if (Main.rand.Next(15) == 0) // 以1/15的概率生成粒子
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector2 particlePosition = drawPosition + new Vector2(0, j * 42f * Projectile.scale); // 缩短宽度
                        int dustIndex = Dust.NewDust(particlePosition, (int)(Projectile.width * 0.25f), (int)(Projectile.height * 0.25f), 35, 0f, 0f, 100, default, 3f);
                        Main.dust[dustIndex].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }

            // 绘制激光的结束部分（下方）
            Vector2 endPosition = start + direction * laserLength - Main.screenPosition;
            Main.EntitySpriteDraw(endTexture, endPosition, null, currentColor, Projectile.rotation, new Vector2(endTexture.Width / 2, endTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

            // 绘制激光的开始部分（上方）
            Main.EntitySpriteDraw(startTexture, start - Main.screenPosition, null, currentColor, Projectile.rotation + MathHelper.Pi, new Vector2(startTexture.Width / 2, startTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

            // 绘制激光的中间部分（上方）
            for (float i = 0; i <= laserLength; i += middleTexture.Height * Projectile.scale)
            {
                Vector2 drawPosition = start - direction * i;
                Main.EntitySpriteDraw(middleTexture, drawPosition - Main.screenPosition, null, currentColor, Projectile.rotation + MathHelper.Pi, new Vector2(middleTexture.Width / 2, middleTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

                // 生成岩浆特效粒子
                if (Main.rand.Next(15) == 0) // 以1/15的概率生成粒子
                {
                    for (int j = -1; j <= 1; j += 2)
                    {
                        Vector2 particlePosition = drawPosition + new Vector2(0, j * 42f * Projectile.scale); // 缩短宽度
                        int dustIndex = Dust.NewDust(particlePosition, (int)(Projectile.width * 0.25f), (int)(Projectile.height * 0.25f), 35, 0f, 0f, 100, default, 3f);
                        Main.dust[dustIndex].velocity = new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
                        Main.dust[dustIndex].noGravity = true;
                    }
                }
            }

            // 绘制激光的结束部分（上方）
            endPosition = start - direction * laserLength - Main.screenPosition;
            Main.EntitySpriteDraw(endTexture, endPosition, null, currentColor, Projectile.rotation + MathHelper.Pi, new Vector2(endTexture.Width / 2, endTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

            return false;
        }




    }
}
