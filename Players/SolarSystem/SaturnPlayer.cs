using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class SaturnPlayer : ModPlayer
    {
        public bool hasSaturnAccessory = false;
        public bool hasTSSAccessory = false; // 添加hasTSSAccessory
        private int fireCooldown = 180; // 180帧即3秒

        public override void ResetEffects()
        {
            hasSaturnAccessory = false;
            hasTSSAccessory = false; // 重置hasTSSAccessory
        }

        public override void UpdateEquips()
        {
            if (hasSaturnAccessory || hasTSSAccessory)
            {
                ManageSaturnAccessory();
            }
        }
        public float GetCooldownRatio()
        {
            return 1f - fireCooldown / 180f; // 0~1之间，0代表刚发射完，1代表即将发射
        }

        private void ManageSaturnAccessory()
        {
            bool saturnExists = false;
            Vector2 saturnPosition = Vector2.Zero;
            int saturnIndex = -1;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<SaturnProjectile>())
                {
                    saturnExists = true;
                    saturnPosition = proj.Center;
                    saturnIndex = proj.whoAmI;
                    break;
                }
            }

            if (!saturnExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                saturnPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 35 * 16), (float)(Math.Sin(angle) * 35 * 16)); // 公转半径35格
                int damage = hasTSSAccessory ? 600 : 450; // 根据hasTSSAccessory确定伤害
                saturnIndex = Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), saturnPosition, Vector2.Zero, ModContent.ProjectileType<SaturnProjectile>(), damage, 10, Player.whoAmI);
            }

            if (fireCooldown > 0)
            {
                fireCooldown--;
            }
            else
            {
                Vector2[] directions = new Vector2[]
                {
                    new Vector2(1, 1), // 右下
                    new Vector2(-1, -1), // 左上
                    new Vector2(-1, 1), // 左下
                    new Vector2(1, -1) // 右上
                };

                foreach (Vector2 direction in directions)
                {
                    direction.Normalize();
                    int damage = (int)(Player.HeldItem.damage * 0.65f * (hasTSSAccessory ? 2.0f : 1.5f)); // 根据hasTSSAccessory调整伤害倍率
                    int projID = Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), saturnPosition, direction * 10, ModContent.ProjectileType<SaturnRingProjectile>(), damage, 0, Player.whoAmI, ai0: saturnIndex);
                    Main.projectile[projID].extraUpdates = 1; // 增加弹幕的更新频率
                }

                fireCooldown = 180; // 重置冷却时间
            }
        }
    }
}