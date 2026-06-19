using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack;
using AccessoryForPlanetaryEmpire.Players.Attack.JobSkill;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles;
using AccessoryForPlanetaryEmpire.Players.Defense;


namespace AccessoryForPlanetaryEmpire.Projectiles.Accessory
{
    public class AsteroidBeltPROJ : ModProjectile
    {
        private int destroyCount = 10; // 可以消除10次敌方弹幕

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1; // 只有一帧动画
        }

        public override void SetDefaults()
        {
            Projectile.width = 48;
            Projectile.height = 48;
            Projectile.friendly = true; // 对玩家友好
            Projectile.penetrate = -1; // 无限穿透
            Projectile.timeLeft = 36000; // To respawn and follow the mouse cursor
            Projectile.tileCollide = false; // 不与瓷砖碰撞
            Projectile.ignoreWater = true; // 不受水影响
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead || !player.GetModPlayer<AsteroidBeltPlayer>().hasAsteroidBelt)
            {
                Projectile.Kill();
                return;
            }

            double rotatingSpeed = MathHelper.TwoPi / 600; // 每10秒旋转一圈
            int radius = 200; // 公转半径
            Projectile.ai[0] += 1; // 控制旋转速度
            double angle = Projectile.ai[0] * rotatingSpeed;
            Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * radius;
            Projectile.position = player.Center + offset - new Vector2(Projectile.width / 2, Projectile.height / 2);

            // 检测碰撞并消除敌方弹幕
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile otherProj = Main.projectile[i];
                if (otherProj.active && otherProj.hostile && Projectile.Hitbox.Intersects(otherProj.Hitbox))
                {
                    otherProj.Kill();
                    destroyCount--;
                    if (destroyCount <= 0)
                    {
                        Explode();
                        break;
                    }
                }
            }
        }

        private void Explode()
        {
            // 在这里添加爆炸动画逻辑
            Projectile.Kill(); // 删除弹幕
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // 在这里添加绘制逻辑，例如绘制GIF动画
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false; // 不切割瓷砖
        }

        public override bool? CanDamage()
        {
            return false; // 不造成伤害
        }
    }
}
