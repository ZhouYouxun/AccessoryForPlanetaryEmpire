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
//using Terraria.Audio;
//using AccessoryForPlanetaryEmpire.Players;
//using Terraria.GameContent;


//namespace AccessoryForPlanetaryEmpire.Projectiles.SolarSystem
//{
//    public class NeptuneWaterProjectile : ModProjectile
//    {



//        public override void SetStaticDefaults()
//        {
//            // DisplayName.SetDefault("Neptune Water");
//        }

//        public override void SetDefaults()
//        {
//            Projectile.width = 240; // 15格 = 240像素
//            Projectile.height = 240; // 15格 = 240像素
//            Projectile.friendly = true;
//            Projectile.DamageType = DamageClass.Magic;
//            Projectile.penetrate = -1;
//            //Projectile.timeLeft = 600; // 10 seconds
//            Projectile.ignoreWater = true;
//            Projectile.tileCollide = false;
//            Projectile.extraUpdates = 1;
//            Projectile.usesLocalNPCImmunity = true;
//            Projectile.localNPCHitCooldown = 10;
//        }

//        public override void AI()
//        {
//            Player player = Main.player[Projectile.owner];

//            // 尝试跟随鼠标
//            HomeTowardsMouse();

//            // 高速旋转
//            Projectile.rotation += 0.1f;

//            // 粒子特效
//            int dustId = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
//            Main.dust[dustId].noGravity = true;
//            Main.dust[dustId].scale = 1.5f;
//            Main.dust[dustId].velocity *= 0.5f;

//            // 生成Ice粒子特效
//            GenerateIceParticles();
//        }

//        private void GenerateIceParticles()
//        {
//            // 头部位置
//            Vector2 headPosition = Projectile.Center + new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * (Projectile.width / 2);
//            // 尾部位置
//            Vector2 tailPosition = Projectile.Center - new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * (Projectile.width / 2);

//            CreateIceDust(headPosition);
//            CreateIceDust(tailPosition);
//        }

//        private void CreateIceDust(Vector2 position)
//        {
//            int dustId = Dust.NewDust(position, 0, 0, DustID.Ice);
//            Dust dust = Main.dust[dustId];
//            dust.noGravity = true;
//            dust.scale = 1.5f;
//            dust.fadeIn = 1f;
//            dust.velocity = Vector2.Zero;

//            // 让粒子向外射出并旋转
//            dust.velocity = new Vector2((float)Math.Cos(Projectile.rotation), (float)Math.Sin(Projectile.rotation)) * 2f;
//            dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
//        }

//        private void HomeTowardsMouse()
//        {
//            if (Main.myPlayer != Projectile.owner)
//                return;

//            Vector2 targetPosition = Main.MouseWorld;
//            float distance = Vector2.Distance(Projectile.Center, targetPosition);

//            // 使用缓动函数来平滑速度变化
//            float speed = 25f; // 最大速度
//            float slowingDistance = 100f; // 开始减速的距离
//            float lerpFactor = MathHelper.Clamp(distance / slowingDistance, 0f, 1f); // 缓动系数

//            Vector2 direction = (targetPosition - Projectile.Center).SafeNormalize(Vector2.Zero);
//            Projectile.velocity = Vector2.Lerp(Vector2.Zero, direction * speed, lerpFactor);

//            Projectile.netSpam = 0;
//            Projectile.netUpdate = true;
//        }

//        public override bool PreDraw(ref Color lightColor)
//        {
//            SpriteBatch spriteBatch = Main.spriteBatch;
//            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
//            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
//            Vector2 drawPosition = Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY);

//            // 绘制贴图本体
//            spriteBatch.Draw(texture, drawPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
//            return false;
//        }




//        public override void Kill(int timeLeft)
//        {
//            // 生成蓝色粒子特效
//            ShootBlueParticles();
//        }

//        private void ShootBlueParticles()
//        {
//            int particleCount = 100;
//            for (int i = 0; i < particleCount; i++)
//            {
//                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
//                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(5f, 10f);
//                Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.BlueTorch, velocity);
//                dust.noGravity = true;
//                dust.scale = 1.5f; // 调整粒子大小
//                dust.fadeIn = 1f;
//            }
//        }




//    }
//}
