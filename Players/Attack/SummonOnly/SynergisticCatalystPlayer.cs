using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class SynergisticCatalystPlayer : ModPlayer
    {
        public bool hasSynergisticCatalyst = false;
        public float damageIncrease;
        public bool eyeopen = false;

        public override void ResetEffects()
        {
            hasSynergisticCatalyst = false;
            eyeopen = false;
            damageIncrease = 0f; // 每次更新后重置增益效果
        }


        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            TriggerEffect(proj.DamageType);

            // 👇只对召唤类弹幕生效，确保是仆从造成的命中
            if (hasSynergisticCatalyst && proj.minion)
            {
                var modPlayer = Player.GetModPlayer<SynergisticCatalystPlayer>();
                if (!modPlayer.hasSynergisticCatalyst || !modPlayer.eyeopen || !proj.minion)
                    return;

                // 🌈释放五彩光点
                PLALightingBoltsSystem.SpawnRainbowBurst_Lively(proj.Center);

                // 🌟白+蓝白 Dust 大范围爆发式喷发
                float dustRadius = 20f; // 圆形半径，和光点一致
                int dustCount = 20;

                for (int i = 0; i < dustCount; i++)
                {
                    int dustType = Main.rand.NextBool() ? DustID.WhiteTorch : DustID.GemDiamond;

                    // 在圆内随机位置
                    Vector2 offset = Main.rand.NextVector2Circular(dustRadius, dustRadius);
                    Vector2 spawnPos = proj.Center + offset;

                    // 速度方向：指向外部（加强力度）
                    Vector2 velocity = offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3f, 5.5f);

                    Dust d = Dust.NewDustPerfect(spawnPos, dustType, velocity);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.2f, 1.8f);
                }

                // 🌈额外原版粒子系统（如：命中的“彩虹杖”特效）
                ParticleOrchestrator.RequestParticleSpawn(
                    clientOnly: false,
                    ParticleOrchestraType.RainbowRodHit,
                    new ParticleOrchestraSettings { PositionInWorld = proj.Center + Main.rand.NextVector2Circular(6f, 6f) },
                    proj.owner
                );

            }
        }

        private void TriggerEffect(DamageClass damageType)
        {
            if (hasSynergisticCatalyst && (damageType == DamageClass.Melee || damageType == DamageClass.Ranged || damageType == DamageClass.Magic))
            {
                if (Main.rand.NextFloat() < 0.10f)
                {
                    damageIncrease = Main.rand.NextFloat(0.05f, 0.30f);
                    Player.AddBuff(ModContent.BuffType<Buffs.SynergisticCatalystBuff>(), 600); // 10秒的Buff持续时间
                }

            }
        }








    }
}