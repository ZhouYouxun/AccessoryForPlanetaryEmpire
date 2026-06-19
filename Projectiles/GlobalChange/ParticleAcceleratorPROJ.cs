using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using AccessoryForPlanetaryEmpire.Players;
using System.Reflection;


namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class ParticleAcceleratorPROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private float initialSpeed;
        private float elapsedTime;
        private int initialDamage;
        private int Cooldown = 0;


        public override void AI(Projectile projectile)
        {
            if (Cooldown > 0)
            {
                Cooldown--;
                return;
            }
            Player player = Main.player[projectile.owner];
            var modPlayer = player.GetModPlayer<ParticleAcceleratorPlayer>();

            // 只有在装备Particle Accelerator且弹幕类型符合条件时才进行加速
            if (modPlayer.hasParticleAccelerator && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                if (initialSpeed == 0)
                {
                    initialSpeed = projectile.velocity.Length(); // 记录初始速度
                    initialDamage = projectile.damage; // 记录初始伤害
                    projectile.velocity *= 0.2f; // 将速度设为初始速度的20%
                    projectile.damage = (int)(initialDamage * 0.2f); // 初始伤害设为20%
                }

                elapsedTime += 1f / 60f; // 记录已经过的时间

                if (elapsedTime <= 3f)
                {
                    float multiplier = 0.2f + (elapsedTime / 3f) * 2.8f; // 逐渐从20%增加到300%
                    projectile.velocity = Vector2.Normalize(projectile.velocity) * (initialSpeed * multiplier);
                    projectile.damage = (int)(initialDamage * multiplier); // 同步调整伤害
                }
            }
            Cooldown = 10;

        }

    }
}