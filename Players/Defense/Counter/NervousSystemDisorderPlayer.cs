using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using System;
using AccessoryForPlanetaryEmpire.Global;
using Terraria.GameInput;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class NervousSystemDisorderPlayer : ModPlayer
    {
        public bool hasNervousSystemDisorderDevice = false;
        private int hitCount; // 累积受击次数
        private int effectTimer; // 效果持续时间计时器
        private const int maxHits = 2; // 两次受击触发效果
        private const int effectDuration = 120; // 效果持续时间为2秒，60帧 = 1秒

        public override void ResetEffects()
        {
            hasNervousSystemDisorderDevice = false;
            // 确保效果不会延续到下一帧
            if (effectTimer > 0)
            {
                effectTimer--;
                //Player.moveSpeed += 0.40f; // 增加40%移动速度（在陆地上奔跑）
                //Player.velocity *= 1.40f; // 增加40%移动速度
            }
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (hasNervousSystemDisorderDevice)
            {
                hitCount++;

                if (hitCount >= maxHits)
                {
                    ApplyDebuffToEnemies(); // 施加削弱敌人的效果
                    CreateNervousSystemEffect(Player.Center, DustID.GemTopaz, 200, 2f); // 2秒内粒子特效
                    hitCount = 0; // 重置受击次数
                    effectTimer = effectDuration; // 设置效果持续时间为2秒
                }
            }
        }

        private void ApplyDebuffToEnemies()
        {
            // 在玩家周围100格内寻找敌对NPC并施加Buff
            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Player.Center) <= 100 * 16) // Terraria中1格=16像素
                {
                    npc.AddBuff(ModContent.BuffType<NSDD_Enemy>(), effectDuration); // 只持续2秒的Buff
                }
            }
        }

        // 创建神经系统紊乱装置粒子特效的方法
        private void CreateNervousSystemEffect(Vector2 position, int dustID, int particleCount, float duration)
        {
            float time = Main.GlobalTimeWrappedHourly;

            // 🌪 数学感Dust螺旋扩散 + 同心波动圈
            for (int i = 0; i < particleCount; i++)
            {
                float angle = MathHelper.TwoPi * i / particleCount + time * 2f;
                float radius = 48f + (float)Math.Sin(i + time * 3f) * 16f * 2f;
                Vector2 offset = angle.ToRotationVector2() * radius;

                Dust d = Dust.NewDustPerfect(position + offset, dustID, -offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 2f), 120, Color.Goldenrod, Main.rand.NextFloat(1.6f, 2.2f));
                d.noGravity = true;
                d.fadeIn = duration + Main.rand.NextFloat(0.2f);
            }

            // ✨ 光点环形散射（使用你提供的光点标准）
            for (int i = 0; i < 44; i++)
            {
                float angle = MathHelper.TwoPi * i / 24f + time * 1.5f;
                Vector2 pos = position + angle.ToRotationVector2() * 150f;
                PLALightingBoltsSystem.PLA_Spawn_PrettySparkle_YellowBurst(pos);
            }
        }




    }
}