using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;



namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class AntimatterNuclearMissilePROJ : ModProjectile
    {



        private static Texture2D whitePixel;

        public override void Load()
        {
            //whitePixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            //whitePixel.SetData(new[] { Color.White });

            // 使用内置的透明纹理代替创建自定义的 Texture2D
            // whitePixel = ModContent.Request<Texture2D>("Terraria/Images/MagicPixel").Value;
        }

        public override string Texture => "Terraria/Images/Projectile_0"; // 使用透明贴图

        private float whiteScreenIntensity = 0f;
        private const float whiteScreenDuration = 60f; // 白屏持续时间 (60帧 = 1秒)

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 100;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60; // 让导弹在1秒后爆炸

            //// 初始化白色像素纹理
            //whitePixel = new Texture2D(Main.graphics.GraphicsDevice, 1, 1);
            //whitePixel.SetData(new Color[] { Color.White });
        }

        public override void AI()
        {
            // 核导弹的简单飞行逻辑，持续向上移动
            Projectile.velocity = new Vector2(0, -5f);

            for (int i = 0; i < 5; i++) // 每帧生成5个烟雾粒子
            {
                int dustIndex = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                Main.dust[dustIndex].velocity *= 0.5f;
                Main.dust[dustIndex].noGravity = true;
            }

            if (Projectile.timeLeft == 1) // 当导弹即将消失时触发爆炸
            {
                Explode();
            }
        }

        private void Explode()
        {
            // 查找并秒杀所有BOSS
            foreach (var npc in Main.npc)
            {
                if (npc.active && npc.boss)
                {
                    npc.life = 0;
                    npc.HitEffect(); // 播放死亡效果
                    npc.checkDead();
                }
            }

            // 播放核爆炸效果和音效
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position); // 核爆音效

            // 添加屏幕震动效果
            Main.LocalPlayer.GetModPlayer<ScreenShakePlayer>().GeneralScreenShakePower = 50f;
            Main.OnPreDraw += UpdateScreenShake;

            // 添加核爆粒子效果
            //for (int i = 0; i < 100; i++)
            //{
            //    Vector2 velocity = Main.rand.NextVector2CircularEdge(10f, 10f);
            //    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Electric, velocity);
            //    dust.noGravity = true;
            //    dust.scale = 2f;
            //    dust.fadeIn = 1f;
            //}



            // 生成核爆蘑菇云粒子特效
            GenerateMushroomCloud();
        }



        private void GenerateMushroomCloud()
        {
            Vector2 explosionCenter = Projectile.Center;

            // 生成底部的蘑菇云基座
            for (int i = 0; i < 400; i++)  // 增加粒子的数量，扩大蘑菇云的规模
            {
                Vector2 basePosition = explosionCenter + Main.rand.NextVector2Circular(600f, 300f);  // 扩大基座的范围
                Dust dust = Dust.NewDustPerfect(basePosition, DustID.Smoke, Vector2.Zero);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(4.5f, 9f);  // 增大粒子的尺寸
                dust.fadeIn = 1f;
                dust.velocity = Main.rand.NextVector2Circular(10f, 5f);  // 增加向外扩散的速度
            }

            // 生成上升的蘑菇云主体
            for (int i = 0; i < 800; i++)  // 增加粒子的数量
            {
                Vector2 upwardPosition = explosionCenter + new Vector2(Main.rand.NextFloat(-300f, 300f), -Main.rand.NextFloat(0f, 600f));  // 扩大上升部分的范围
                Dust dust = Dust.NewDustPerfect(upwardPosition, DustID.CursedTorch, Vector2.Zero);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(3f, 7.5f);  // 增大粒子的尺寸
                dust.fadeIn = 1f;
                dust.velocity = Main.rand.NextVector2Circular(8f, 8f);  // 增加向外扩散的速度
            }

            // 生成扩散的蘑菇云顶
            for (int i = 0; i < 500; i++)  // 增加粒子的数量
            {
                Vector2 capPosition = explosionCenter + new Vector2(Main.rand.NextFloat(-500f, 500f), -Main.rand.NextFloat(400f, 600f));  // 扩大顶部的范围
                Dust dust = Dust.NewDustPerfect(capPosition, DustID.Smoke, Vector2.Zero);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(4.5f, 12f);  // 增大粒子的尺寸
                dust.fadeIn = 1f;
                dust.velocity = Main.rand.NextVector2Circular(12f, 12f);  // 大幅增加扩散速度
            }

            // 生成闪光效果
            for (int i = 0; i < 200; i++)  // 增加粒子的数量
            {
                Vector2 flashPosition = explosionCenter + Main.rand.NextVector2Circular(200f, 100f);  // 扩大闪光的范围
                Dust dust = Dust.NewDustPerfect(flashPosition, DustID.FireworkFountain_Yellow, Vector2.Zero);
                dust.noGravity = true;
                dust.scale = Main.rand.NextFloat(9f, 15f);  // 增大粒子的尺寸
                dust.fadeIn = 1f;
                dust.velocity = Main.rand.NextVector2Circular(15f, 7f);  // 增加向外扩散的速度
            }
        }





        private void UpdateWhiteScreen(GameTime gameTime)
        {
            if (whiteScreenIntensity > 0f)
            {
                // 减少白屏强度
                whiteScreenIntensity -= 1f / whiteScreenDuration;

                // 绘制白屏
                Main.spriteBatch.Begin();
                Main.spriteBatch.Draw(whitePixel, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White * whiteScreenIntensity);
                Main.spriteBatch.End();

                // 如果白屏效果结束，移除事件
                if (whiteScreenIntensity <= 0f)
                {
                    Main.OnPreDraw -= UpdateWhiteScreen;
                }
            }
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






    }
}