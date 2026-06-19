using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Accessory.Attack.SolarSystem;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class MarsPlayer : ModPlayer
    {
        public bool hasMarsAccessory = false;
        public bool hasTSSAccessory = false; // 添加hasTSSAccessory
        private int fireCooldown = 180; // 180帧即3秒

        public override void ResetEffects()
        {
            hasMarsAccessory = false;
            hasTSSAccessory = false; // 重置hasTSSAccessory
        }

        public override void UpdateEquips()
        {
            if (hasMarsAccessory || hasTSSAccessory)
            {
                ManageMarsAccessory();
            }
        }

        public override void PostUpdate()
        {
            if (hasMarsAccessory || hasTSSAccessory)
            {
                ManageMarsAccessory();
            }
        }


        private void ManageMarsAccessory()
        {
            bool marsExists = false;
            Vector2 marsPosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<MarsProjectile>())
                {
                    marsExists = true;
                    marsPosition = proj.Center;
                    break;
                }
            }

            if (!marsExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                marsPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 30 * 16), (float)(Math.Sin(angle) * 30 * 16)); // 公转半径30格
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), marsPosition, Vector2.Zero, ModContent.ProjectileType<MarsProjectile>(), 0, 0, Player.whoAmI);
            }

            // 发射弹幕逻辑
            if (fireCooldown > 0)
            {
                fireCooldown--;
            }
            else
            {
                NPC target = FindClosestNPC(1000f); // 寻找最近的敌人
                if (target != null)
                {
                    Vector2 direction = (target.Center - marsPosition).SafeNormalize(Vector2.Zero);
                    float damageMultiplier = hasTSSAccessory ? 1.5f : 0.75f; // 根据hasTSSAccessory确定伤害倍率

                    // 🛡️ 获取当前武器伤害，加入保底机制
                    int baseWeaponDamage = Player.HeldItem?.damage ?? 0;
                    int finalDamage = (int)(baseWeaponDamage * damageMultiplier);

                    // ⚙️ 设置保底伤害为150
                    if (finalDamage < 150)
                    {
                        finalDamage = 150;
                    }

                    // 🔥 发射战争之火
                    int projID = Projectile.NewProjectile(
                        Player.GetSource_Accessory(Player.HeldItem),
                        marsPosition,
                        direction * 15,
                        ModContent.ProjectileType<SplittingFlamesProjectile>(),
                        finalDamage,
                        0,
                        Player.whoAmI
                    );
                    Main.projectile[projID].scale = 1.5f;
                }

                fireCooldown = hasTSSAccessory ? 120 : 180; // 根据hasTSSAccessory确定冷却时间
            }
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float closestDistance = maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy())
                {
                    float distance = Vector2.Distance(Player.Center, npc.Center);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestNPC = npc;
                    }
                }
            }

            return closestNPC;
        }
    }
}
