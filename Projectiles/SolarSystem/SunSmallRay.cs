//using System;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria.ID;
//using ReLogic.Content;
//using Terraria.Audio;

//namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
//{
//    public class SunSmallRay : ModProjectile
//    {
//        private Vector2 initialDirection;

//        public override void SetStaticDefaults()
//        {
//            // 设置激光的纹理
//            Main.projFrames[Projectile.type] = 1;
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 84; // 宽度设置为激光的宽度
//            Projectile.height = 84; // 高度设置为激光的高度
//            Projectile.friendly = true;
//            Projectile.penetrate = -1; // 无限穿透
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.timeLeft = 48; // 持续时间，0.8秒
//            Projectile.scale = 0.8f; // 放大倍率
//            Projectile.damage = 300; // 基础伤害
//        }

//        public override void AI()
//        {
//            Player player = Main.player[Projectile.owner];
//            if (!player.active || player.dead)
//            {
//                Projectile.Kill();
//                return;
//            }

//            // 固定激光位置在玩家处
//            Projectile.Center = player.Center;

//            // 在AI的第一个tick设置初始方向并固定方向
//            if (Projectile.ai[0] == 0)
//            {
//                initialDirection = Projectile.velocity;
//                Projectile.rotation = initialDirection.ToRotation(); // 设置初始旋转方向
//                Projectile.velocity = Vector2.Zero; // 将速度设为零，确保不移动
//                Projectile.ai[0] = 1;
//            }

//            // 生成黄色粒子特效
//            for (int i = 0; i < 3; i++)
//            {
//                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.YellowTorch, 0f, 0f, 100, default, 2f);
//                Main.dust[dustIndex].velocity *= 2f;
//                Main.dust[dustIndex].noGravity = true;
//            }

//            SoundEngine.PlaySound(SoundID.Item12, player.position);

//        }

//        // 碰撞检测函数，确保整个激光路径都能造成伤害
//        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
//        {
//            float point = 0f;
//            Vector2 endPos = Projectile.Center + initialDirection.SafeNormalize(Vector2.UnitY) * 200f * 16;
//            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, endPos, 84f * Projectile.scale, ref point);
//        }

//        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
//        {
//            target.immune[Projectile.owner] = 10; // 无敌帧
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            Texture2D startTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayStart").Value;
//            Texture2D middleTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayMid").Value;
//            Texture2D endTexture = ModContent.Request<Texture2D>("AccessoryForPlanetaryEmpire/Projectiles/SolarSystem/SunRayEnd").Value;

//            Vector2 start = Projectile.Center;
//            Vector2 direction = initialDirection.SafeNormalize(Vector2.UnitY);
//            float laserLength = 200f * 16; // 固定长度200格

//            // 绘制激光的开始部分
//            Main.EntitySpriteDraw(startTexture, start - Main.screenPosition, null, Color.OrangeRed, Projectile.rotation, new Vector2(startTexture.Width / 2, startTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

//            // 绘制激光的中间部分
//            for (float i = 0; i <= laserLength; i += middleTexture.Height * Projectile.scale)
//            {
//                Vector2 drawPosition = start + direction * i;
//                Main.EntitySpriteDraw(middleTexture, drawPosition - Main.screenPosition, null, Color.OrangeRed, Projectile.rotation, new Vector2(middleTexture.Width / 2, middleTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
//            }

//            // 绘制激光的结束部分
//            Vector2 endPosition = start + direction * laserLength - Main.screenPosition;
//            Main.EntitySpriteDraw(endTexture, endPosition, null, Color.OrangeRed, Projectile.rotation, new Vector2(endTexture.Width / 2, endTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

//            return false;
//        }







//    }
//}