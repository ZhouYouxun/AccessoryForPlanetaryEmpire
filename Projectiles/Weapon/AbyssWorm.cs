using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class AbyssWorm : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 1;
            Projectile.height = 1;
            Projectile.friendly = true;
            Projectile.penetrate = -1; // 无限穿透敌人
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 450; // 7.5秒钟
            Projectile.extraUpdates = 1;
            Projectile.light = 0.5f;
        }

        public override void AI()
        {
            // 让蠕虫进行随机运动
            Projectile.velocity += new Vector2(Main.rand.NextFloat(-0.1f, 0.1f), Main.rand.NextFloat(-0.1f, 0.1f));
            Projectile.velocity *= 0.98f; // 缓慢移动

            // 生成黑色粒子效果，形成蠕虫的身体
            for (int i = 0; i < 2; i++)
            {
                Vector2 dustPosition = Projectile.position;
                Dust dust = Dust.NewDustDirect(dustPosition, Projectile.width, Projectile.height, DustID.Shadowflame, 0f, 0f, 0, default(Color), 1f);
                dust.noGravity = true;
                dust.velocity = Projectile.velocity * 0.5f;
                dust.scale = 1.2f;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // 无限穿透敌人，直接返回false，不进行默认碰撞检测
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 对一个敌人只能造成一次伤害
            if (Projectile.localNPCImmunity[target.whoAmI] == 0)
            {
                target.immune[Projectile.owner] = 0;
            }
            else
            {
                damageDone = 0;
            }
        }
    }
    
}
