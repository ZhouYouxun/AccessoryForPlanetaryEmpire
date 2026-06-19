using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class ThermalSmoke : ModProjectile
    {
        public override void SetStaticDefaults()
        {


        }

        public override void SetDefaults()
        {
            Projectile.width = 160; // 增加大小
            Projectile.height = 160; // 增加大小
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600; // 持续时间增加到10秒
            Projectile.extraUpdates = 1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.alpha = 255; // 使其不可见
        }

        public override void AI()
        {
            // 缓慢停止
            Projectile.velocity *= 0.98f; // 减慢速度，更长的滞留距离

            // 创建烟雾粒子效果
            for (int i = 0; i < 10; i++) // 增加烟雾数量
            {
                Vector2 dustPosition = Projectile.position;
                dustPosition -= Projectile.velocity * (i * 0.5f); // 增加滞留距离
                Projectile.alpha = 255;
                int dust = Dust.NewDust(dustPosition, 1, 1, DustID.Smoke, 0f, 0f, 0, default, 3f); // 增大烟雾粒子的大小
                Main.dust[dust].position = dustPosition;
                Main.dust[dust].velocity *= 0.5f; // 增加烟雾的扩散速度
                Main.dust[dust].noGravity = true;
            }
        }
    }
}
