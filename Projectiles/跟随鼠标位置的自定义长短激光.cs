//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria;
//using Terraria.ModLoader;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Terraria.ID;
//using ReLogic.Content;

//namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
//{
//    public class SunLaserProjectile : ModProjectile
//    {
//        public override void SetStaticDefaults()
//        {
//            // 设置激光的纹理
//            Main.projFrames[Projectile.type] = 1;
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 20;
//            Projectile.height = 20;
//            Projectile.friendly = true;
//            Projectile.penetrate = -1;
//            Projectile.tileCollide = false;
//            Projectile.ignoreWater = true;
//            Projectile.timeLeft = 300; // 激光持续时间
//            Projectile.scale = 10f; // 激光放大倍率
//        }

//        public override void AI()
//        {
//            Player player = Main.player[Projectile.owner];
//            if (!player.active || player.dead)
//            {
//                Projectile.Kill();
//                return;
//            }

//            // 将激光固定在玩家头顶
//            Projectile.Center = player.Center + new Vector2(0, -50);
//            Projectile.rotation = (Main.MouseWorld - Projectile.Center).ToRotation();

//            // 生成粒子特效
//            for (int i = 0; i < 5; i++)
//            {
//                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.WhiteTorch, 0f, 0f, 100, default, 3f);
//                Main.dust[dustIndex].velocity *= 3f;
//                Main.dust[dustIndex].noGravity = true;
//            }

//            Lighting.AddLight(Projectile.Center, Color.White.ToVector3() * 1.5f);
//        }

//        public override bool? CanDamage()
//        {
//            return true;
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
//            Vector2 end = Main.MouseWorld;
//            Vector2 direction = end - start;
//            direction.Normalize();

//            // 绘制激光的开始部分
//            Main.EntitySpriteDraw(startTexture, start - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(startTexture.Width / 2, startTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

//            // 绘制激光的中间部分
//            float length = Vector2.Distance(start, end);
//            for (float i = 0; i <= length; i += middleTexture.Height * Projectile.scale)
//            {
//                Vector2 drawPosition = start + direction * i;
//                Main.EntitySpriteDraw(middleTexture, drawPosition - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(middleTexture.Width / 2, middleTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);
//            }

//            // 绘制激光的结束部分
//            Main.EntitySpriteDraw(endTexture, end - Main.screenPosition, null, lightColor, Projectile.rotation, new Vector2(endTexture.Width / 2, endTexture.Height / 2), Projectile.scale, SpriteEffects.None, 0);

//            return false;
//        }
//    }
//}
