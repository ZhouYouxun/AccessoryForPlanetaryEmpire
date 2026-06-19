using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class EnergizedChitinPROJ : GlobalProjectile
    {
        public static void CheckAndDestroyEnemyProjectiles(Projectile minionProj)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.hostile && proj.Hitbox.Intersects(minionProj.Hitbox))
                {
                    // 1/4 的概率清除敌方弹幕
                    if (Main.rand.NextFloat() < 0.25f)
                    {
                        proj.Kill(); // 销毁敌方射弹
                    }
                }
            }
        }

        public static void EnhanceFriendlyProjectiles(Projectile minionProj)
        {
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.friendly && !proj.minion && proj.owner == minionProj.owner && proj.Hitbox.Intersects(minionProj.Hitbox))
                {
                    // 只有战士、射手和法师的弹幕能够触发增强效果
                    if (proj.DamageType == DamageClass.Melee || proj.DamageType == DamageClass.Ranged || proj.DamageType == DamageClass.Magic)
                    {
                        // 1/2 的概率增强我方弹幕
                        if (Main.rand.NextFloat() < 0.5f)
                        {
                            proj.damage = (int)(proj.damage * 1.5f); // 增加50%的伤害
                        }
                    }
                }
            }
        }











    }
}