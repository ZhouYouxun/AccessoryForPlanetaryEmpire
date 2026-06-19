using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;


namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory.EMPs
{
    public class BarrierProjectile : ModProjectile
    {
        public override string Texture => "AccessoryForPlanetaryEmpire/Projectiles/Accessory/INV"; // 使用透明弹幕贴图

        public override void SetDefaults()
        {
            Projectile.width = 900; // 设置光环的宽度
            Projectile.height = 900; // 设置光环的高度
            Projectile.friendly = true; // 弹幕对玩家友好
            Projectile.penetrate = -1; // 弹幕可以无限穿透
            Projectile.timeLeft = 10 * 60; // 弹幕持续时间，这里设置为10秒
            Projectile.tileCollide = false; // 弹幕不与瓷砖碰撞
            Projectile.ignoreWater = true; // 弹幕忽略水的影响
            Projectile.alpha = 100; // 初始时弹幕是半透明的
        }

        public override void AI()
        {
            // 弹幕的逻辑会每帧调用一次
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProj = Main.projectile[i];
                if (otherProj.active && otherProj.hostile && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                {
                    // 清除与屏障接触的敌方弹幕
                    otherProj.Kill();
                }
            }

            // 增加生成粒子的频率来增加粒子数量
            for (int i = 0; i < 20; i++) // 一次创建20个粒子
            {
                Vector2 position = Projectile.Center + Main.rand.NextVector2CircularEdge(650f, 650f); // 修改圆圈的半径为650
                Dust dust = Dust.NewDustPerfect(position, DustID.MagicMirror, Vector2.Zero, 150, Color.Purple, 3.5f); // 增加粒子大小
                dust.noGravity = true; // 粒子不受重力影响
                dust.fadeIn = 1f; // 让粒子逐渐消失
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = new Vector2(0, 0); // 初始速度为零
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }

            Lighting.AddLight(Projectile.Center, 0.5f, 0.1f, 0.5f); // 暗淡的光
        }

        public override bool? CanCutTiles()
        {
            return false; // 屏障不会切割瓷砖
        }

        public override bool? CanDamage()
        {
            return false; // 屏障不会造成伤害
        }
    }
}
