using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace AccessoryForPlanetaryEmpire.Projectiles.Weapon
{
    public class BrassGear : ModProjectile
    {
        public override void SetStaticDefaults()
        {

        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = 6; // 穿透5次后消失
            Projectile.timeLeft = 600; // 10秒后消失
            Projectile.scale = 4.0f; // 通过调整缩放比例来变大子弹贴图
        }

        public override void AI()
        {
            // 高速自转
            // Projectile.rotation += 0.4f;

            // 逐渐减速直到停止
            if (Projectile.velocity.Length() > 1f)
            {
                Projectile.velocity *= 0.98f;
            }
            else
            {
                Projectile.velocity = Vector2.Zero;
            }

            // 生成粒子效果
            if (Projectile.timeLeft % 10 == 0)
            {
                Dust.NewDustPerfect(Projectile.Center, DustID.GoldCoin, Vector2.Zero, 0, Color.Yellow, 1.5f).noGravity = true;
            }
        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 穿透次数减少
            Projectile.penetrate--;
            if (Projectile.penetrate <= 0)
            {
                Projectile.Kill();
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // 碰撞后保持位置不变
            Projectile.velocity = Vector2.Zero;
            return false;
        }
    }
}
