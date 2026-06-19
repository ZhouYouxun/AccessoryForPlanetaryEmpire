using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class MedicalDronesPlayer : ModPlayer
    {
        public bool hasMedicalDrones;
        private int lifeStealCooldown = 0; // 吸血效果的冷却时间，以游戏帧数为单位


        public override void ResetEffects()
        {
            hasMedicalDrones = false;
        }
        public override void PostUpdateEquips()
        {
            if (hasMedicalDrones)
            {
                int maxMinions = 20;
                float currentMinions = Player.slotsMinions;
                if (currentMinions > maxMinions) currentMinions = maxMinions;

                // 使用Math.Floor将浮点数向下取整并转为整数
                int minionCount = (int)Math.Floor(currentMinions);

                Player.statLifeMax2 += 25 * minionCount; // 每个仆从提供25点最大生命值
                Player.lifeRegen += 5 * minionCount; // 每个仆从提供5点生命恢复
                //Player.GetDamage<SummonDamageClass>() -= 0.45f; // 降低45%召唤伤害
            }

            // 减少冷却时间
            if (lifeStealCooldown > 0)
            {
                lifeStealCooldown--;
            }
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (hasMedicalDrones && proj.DamageType == DamageClass.Summon)
            {



                if (lifeStealCooldown <= 0 && Main.rand.NextFloat() < 0.30f) // 冷却时间为0时触发吸血效果
                {
                    // 把效果转移到弹幕里面
                    // 🌿 发射医疗无人机弹幕
                    Vector2 offset = Main.rand.NextVector2Circular(40f, 40f); // 偏移位置
                    Vector2 spawnPos = proj.Center + offset;
                    Vector2 velocity = (spawnPos - Player.Center).SafeNormalize(Vector2.Zero).RotatedByRandom(0.5f) * Main.rand.NextFloat(12f, 14f);

                    float healAmount = Main.rand.NextFloat(0.05f, 0.30f) * damageDone;

                    int projIndex = Projectile.NewProjectile(
                        proj.GetSource_FromThis(),
                        spawnPos,
                        velocity,
                        ModContent.ProjectileType<MedicalDrones>(),
                        0,
                        0f,
                        Player.whoAmI,
                        healAmount // 👈 把回血量塞到 ai[0]
                    );



                    //float lifeSteal = Main.rand.NextFloat(0.05f, 0.30f) * damageDone;
                    //Player.statLife += (int)lifeSteal;
                    //Player.HealEffect((int)lifeSteal, true);

                    //GenerateHealEffect(proj);

                    lifeStealCooldown = (int)(0.25f * 60f); // 设置冷却时间为0.25秒（15帧）
                }


             



            }
        }

        private void GenerateHealEffect(Projectile proj)
        {
            Vector2 startPosition = proj.Center;
            Vector2 endPosition = Player.Center;

            int numParticles = 50;
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
}






