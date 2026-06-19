using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class EarthPlayer : ModPlayer
    {
        public bool hasEarthAccessory = false;
        public bool hasTSSAccessory = false;
        private int healCooldown = 600; // 600帧即10秒
        private int healTSSCooldown = 300; // 300帧即5秒

        public override void ResetEffects()
        {
            hasEarthAccessory = false;
            hasTSSAccessory = false;
        }


        public override void PostUpdate()
        {

            if (hasEarthAccessory || hasTSSAccessory)
            {
                // 判断是否已经有一个活跃的"EarthProjectile"弹幕
                bool earthExists = false;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<EarthProjectile>())
                    {
                        earthExists = true;
                        break;
                    }
                }

                // 如果没有找到活跃的地球弹幕，则生成一个新的
                if (!earthExists)
                {
                    Vector2 position = Player.Center + new Vector2(0, -6 * 16); // 正上方-6 * 16
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), position, Vector2.Zero, ModContent.ProjectileType<EarthProjectile>(), 0, 0, Player.whoAmI);
                }
            }



            if (hasEarthAccessory)
            {
                if (healCooldown > 0)
                {
                    healCooldown--;
                }
                else
                {
                    //Player.statLife += 75; // 恢复75点血量
                    //Player.HealEffect(75, true); // 显示恢复效果
                    TriggerEarthHealEffect();

                    // 生成粒子特效
                    GenerateHealEffect();

                    healCooldown = 600; // 重置冷却时间
                }
            }

            if (hasTSSAccessory)
            {
                if (healTSSCooldown > 0)
                {
                    healTSSCooldown--;
                }
                else
                {
                    //Player.statLife += 75; // 恢复75点血量
                    //Player.HealEffect(75, true); // 显示恢复效果
                    TriggerEarthHealEffect();

                    // 生成粒子特效
                    GenerateHealEffect();

                    healTSSCooldown = 300; // 重置冷却时间
                }
            }



            //if (!earthExists)
            //{
            //    Vector2 position = Player.Center + new Vector2(0, -6 * 16); // 正上方-6 * 16
            //    int projIndex = Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), position, Vector2.Zero, ModContent.ProjectileType<EarthProjectile>(), 0, 0, Player.whoAmI);
            //    Main.projectile[projIndex].ai[0] = 0f; // 重置ai[0]，确保每次生成都从头开始
            //}
        }

        // 释放两个随机方向的治疗弹幕
        private void TriggerEarthHealEffect()
        {
            Projectile proj = FindEarthProjectile();
            if (proj != null)
            {
                for (int i = 0; i < 2; i++)
                {
                    Vector2 direction = Main.rand.NextVector2Circular(1f, 1f);
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), proj.Center, direction * 5f, ModContent.ProjectileType<EarthHealProj>(), 0, 0, Player.whoAmI);
                }
            }
        }

        private void GenerateHealEffect()
        {
            Projectile proj = FindEarthProjectile();
            if (proj != null)
            {
                Vector2 startPosition = proj.Center;
                Vector2 endPosition = Player.Center;

                int numParticles = 50; // 增加粒子数量
                for (int i = 0; i < numParticles; i++)
                {
                    float lerpValue = (float)i / (numParticles - 1);
                    Vector2 position = Vector2.Lerp(startPosition, endPosition, lerpValue);

                    // 增加随机性
                    position += new Vector2(Main.rand.Next(-10, 10), Main.rand.Next(-10, 10));

                    int dustIndex = Dust.NewDust(position, 0, 0, DustID.GreenTorch);
                    Dust dust = Main.dust[dustIndex];
                    dust.noGravity = true;
                    dust.scale = 2.0f + Main.rand.NextFloat(0.5f); // 放大粒子倍率为两倍，并引入一定的随机性
                    dust.velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f)); // 增加粒子的随机速度
                }
            }
        }

        private Projectile FindEarthProjectile()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<EarthProjectile>())
                {
                    return proj;
                }
            }
            return null;
        }
    }
}
