using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Achievements;
using AccessoryForPlanetaryEmpire.Players.Attack.ModProj;
using System.Reflection;
using AccessoryForPlanetaryEmpire.Players.Attack;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class NebulaBraceletPROJ : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        private int Cooldown = 0;


        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            if (Main.player[projectile.owner].GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                // projectile.velocity *= 1.5f;
            }
            // 如果满足 hasUniversalgravitation 和 IsEligibleProjectile 条件，则设置持续时间
            Player player = Main.LocalPlayer;
            if (player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                projectile.timeLeft = (int)(7.5f * 60f); // 设置弹幕的持续时间为 7.5 秒
            }
        }


        public override void AI(Projectile projectile)
        {
            if (Cooldown > 0)
            {
                Cooldown--;
                return;
            }

            Player player = Main.player[projectile.owner];
            if (player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                Vector2 targetPosition = Main.MouseWorld;
                float distanceToMouse = Vector2.Distance(projectile.Center, targetPosition);
                float minimumSpeed = 10f;
                float maximumSpeed = 30f;

                if (distanceToMouse > 5f * 16f)
                {
                    // 距离较远时直接飞向鼠标位置，并逐渐加速
                    float speed = MathHelper.Clamp(distanceToMouse / 16f, minimumSpeed, maximumSpeed);
                    Vector2 desiredVelocity = (targetPosition - projectile.Center).SafeNormalize(Vector2.Zero) * speed;
                    projectile.velocity = Vector2.Lerp(projectile.velocity, desiredVelocity, 0.1f); // 平滑调整速度
                }
                else
                {
                    // 接近鼠标时，进入圆周轨道，保持恒定速度20
                    float circleRadius = 5f * 16f;
                    float angularSpeed = 0.1f;
                    float angle = projectile.ai[0];

                    angle += angularSpeed;
                    if (angle > MathHelper.TwoPi)
                        angle -= MathHelper.TwoPi;

                    Vector2 offset = angle.ToRotationVector2() * circleRadius;
                    Vector2 desiredPosition = targetPosition + offset;
                    Vector2 smoothedPosition = Vector2.Lerp(projectile.Center, desiredPosition, 0.1f);
                    projectile.velocity = (smoothedPosition - projectile.Center).SafeNormalize(Vector2.Zero) * minimumSpeed;

                    projectile.ai[0] = angle; // 存储当前角度
                }
            }

            Cooldown = 10; // 每10帧调用一次
        }




        //public override void AI(Projectile projectile)
        //{
        //    if (Cooldown > 0)
        //    {
        //        Cooldown--;
        //        return;
        //    }

        //    Player player = Main.player[projectile.owner];
        //    if (player.GetModPlayer<NebulaBraceletPlayer>().hasNebulaBracelet && DamageClassUtils.IsEligibleProjectile(projectile))
        //    {
        //        // 直接将弹幕位置设置为鼠标位置
        //        projectile.Center = Main.MouseWorld;

        //        // 如果需要，可以设置固定的速度朝向鼠标，但在这里直接设置位置使得跟踪非常生硬
        //        projectile.velocity = Vector2.Zero; // 弹幕速度为0，确保它直接停留在鼠标位置
        //    }

        //    Cooldown = 10; // 每10帧调用一次
        //}




    }
}
