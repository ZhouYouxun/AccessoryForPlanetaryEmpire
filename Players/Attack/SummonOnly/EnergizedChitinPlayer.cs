using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.GlobalChange;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class EnergizedChitinPlayer : ModPlayer
    {
        public bool hasEnergizedChitin;

        public override void ResetEffects()
        {
            hasEnergizedChitin = false;
        }

        public override void PostUpdateEquips()
        {
            if (hasEnergizedChitin)
            {
                foreach (Projectile proj in Main.projectile)
                {
                    if (proj.active && proj.owner == Player.whoAmI && proj.minion)
                    {
                        float distance = Vector2.Distance(proj.Center, Player.Center);

                        if (distance < 400f) // 小于25格的距离（1格=16像素）
                        {
                            // 检查并销毁敌方射弹
                            EnergizedChitinPROJ.CheckAndDestroyEnemyProjectiles(proj);
                            // 释放红色粒子特效
                            ReleaseParticles(proj.Center, DustID.Torch, true);  // 红色近距离效果
                        }
                        else if (distance > 880f) // 大于55格的距离
                        {
                            // 增强我方射弹
                            EnergizedChitinPROJ.EnhanceFriendlyProjectiles(proj);
                            // 释放蓝色粒子特效
                            ReleaseParticles(proj.Center, DustID.BlueTorch, false); // 蓝色远距离效果
                        }
                    }
                }
            }
        }

        private void ReleaseParticles(Vector2 center, int dustType, bool isClose)
        {
            if (isClose)
            {
                // 🔴红色：稳定护盾环
                for (int i = 0; i < 12; i++)
                {
                    float angle = Main.GameUpdateCount * 0.1f + MathHelper.TwoPi * i / 12f;
                    Vector2 offset = new Vector2(16f, 0f).RotatedBy(angle);
                    Dust dust = Dust.NewDustPerfect(center + offset, dustType);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                    dust.scale = 1.2f;
                }
            }
            else
            {
                // 🔵蓝色：旋转发散的螺旋环
                for (int i = 0; i < 14; i++)
                {
                    float spin = Main.GameUpdateCount * 0.2f;
                    float angle = spin + MathHelper.TwoPi * i / 14f;
                    float radius = 12f + i * 1.5f; // 螺旋感：越后越远
                    Vector2 offset = angle.ToRotationVector2() * radius;

                    Dust dust = Dust.NewDustPerfect(center + offset, dustType);
                    dust.velocity = offset.SafeNormalize(Vector2.Zero) * 0.5f; // 轻微外扩速度
                    dust.noGravity = true;
                    dust.scale = 1.3f;
                }
            }
        }









    }
}