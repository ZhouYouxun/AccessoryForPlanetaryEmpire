using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class BrassLight : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1; // 无限穿透
            Projectile.timeLeft = 50; // 飞行50格后消失
        }

        public override void AI()
        {
            // 生成黄色激光粒子特效
            Dust.NewDustPerfect(Projectile.position, DustID.GoldCoin, null, 0, Color.Yellow);

            // 使弹幕始终面朝飞行方向
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        //public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        //{
        //    // 不需要任何额外行为
        //}
    }
}
