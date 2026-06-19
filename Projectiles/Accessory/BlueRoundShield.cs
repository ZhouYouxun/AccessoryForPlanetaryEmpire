using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class BlueRoundShield : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 12; // 设置帧数为12帧
        }

        public override void SetDefaults()
        {
            Projectile.width = 50; // 根据实际情况设置宽度
            Projectile.height = 50; // 根据实际情况设置高度
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1; // 无限穿透
            Projectile.timeLeft = 2; // 持续存在
            Projectile.ignoreWater = true; // 忽略水
            Projectile.tileCollide = false; // 不与瓷砖碰撞
            Projectile.alpha = 255; // 透明度设置为完全透明（初始化时）
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // 跟随玩家
            Projectile.Center = player.Center;

            // 设置时间延长
            Projectile.timeLeft = 2;

            // 动画控制
            Projectile.frameCounter++;
            if (Projectile.frameCounter > 4) // 控制动画速度
            {
                Projectile.frame++;
                Projectile.frameCounter = 0;
            }
            if (Projectile.frame >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = 0;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            int frameWidth = texture.Width / Main.projFrames[Projectile.type]; // 每帧的宽度
            int x = frameWidth * Projectile.frame; // 计算当前帧的X坐标

            Main.spriteBatch.Draw(
                texture,
                Projectile.Center - Main.screenPosition,
                new Rectangle(x, 0, frameWidth, texture.Height),
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                new Vector2(frameWidth / 2f, texture.Height / 2f),
                Projectile.scale,
                SpriteEffects.None,
                0f
            );
            return false;
        }
    }
}
