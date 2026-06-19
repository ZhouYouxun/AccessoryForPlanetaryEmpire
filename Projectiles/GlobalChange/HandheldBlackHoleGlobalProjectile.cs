using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class HandheldBlackHoleGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.sentry && player.active && player.GetModPlayer<HandheldBlackHolePlayer>().hasHandheldBlackHole)
            {
                Vector2 direction = player.Center - projectile.Center;
                float distance = direction.Length();

                // 设定最小和最大速度
                float minSpeed = 5f;
                float maxSpeed = 20f;

                // 根据距离进行线性插值调整速度
                float speed = MathHelper.Lerp(maxSpeed, minSpeed, distance / 500f); // 假设500是最大距离，超过500时速度为minSpeed

                // 更新哨兵的速度
                projectile.velocity = Vector2.Normalize(direction) * speed;

                // 哨兵接近玩家时逐渐停止
                if (distance < 50f) // 假设50f是停止的临界距离
                {
                    projectile.velocity *= 0.1f; // 减慢速度
                }

                projectile.tileCollide = false; // 无视地形
            }
        }


    }
}
