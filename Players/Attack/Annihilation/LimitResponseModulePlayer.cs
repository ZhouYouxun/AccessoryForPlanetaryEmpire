using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class LimitResponseModulePlayer : ModPlayer
    {
        public bool hasLimitResponseModule;
        public int LimitCooldown = 0; // 冷却时间计数器
        public const int CooldownDuration = 30 * 60; // 30秒冷却，1秒60帧
        public const int BuffDuration = 15 * 60; // 15秒效果持续时间
        public int BuffTimer = 0; // 15秒计时器，用于粒子特效触发

        public override void ResetEffects()
        {
            hasLimitResponseModule = false;
        }

        public override void PreUpdate()
        {
            // 如果开关被触发
            if (hasLimitResponseModule)
            {
                // 如果冷却未结束，减少冷却计时
                if (LimitCooldown > 0)
                {
                    LimitCooldown--;
                }

                // 当血量低于10%时，检查冷却时间
                if (Player.statLife <= Player.statLifeMax2 * 0.2 && LimitCooldown == 0)
                {
                    // 赋予15秒的Buff
                    Player.AddBuff(ModContent.BuffType<LimitResponseModuleBuff>(), BuffDuration);

                    // 设置15秒计时器，用于粒子特效
                    BuffTimer = BuffDuration;

                    // 重置冷却时间为45秒
                    LimitCooldown = CooldownDuration;
                }
            }

            // 当buffTimer大于0时，触发粒子特效
            if (BuffTimer > 0)
            {
                if (Player.HasBuff(ModContent.BuffType<LimitResponseModuleBuff>()))
                {
                    BuffTimer--;
                    CreateYellowParticle(); // 调用粒子特效
                }
                else
                {
                    BuffTimer = 0;
                }
            }
        }

        private void CreateYellowParticle()
        {
            // 半径50个方块的圆形区域，方块大小为16像素，因此半径是50*16像素
            float radius = 50 * 16f;

            // 在玩家周围随机生成粒子的位置
            float angle = Main.rand.NextFloat(MathHelper.TwoPi); // 随机角度
            Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Main.rand.NextFloat(radius); // 计算生成位置的偏移量

            // 最终的生成位置
            Vector2 spawnPosition = Player.Center + offset;

            // 粒子生成的基本逻辑，直接使用你给的蓝色粒子效果
            int particleCount = 1; // 可以按需增加数量
            for (int i = 0; i < particleCount; i++)
            {
                float particleAngle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 velocity = new Vector2((float)Math.Cos(particleAngle), (float)Math.Sin(particleAngle)) * Main.rand.NextFloat(5f, 10f);
                Dust dust = Dust.NewDustPerfect(spawnPosition, DustID.GoldFlame, velocity);
                dust.noGravity = true;
                dust.scale = 5f; // 放大粒子大小
                dust.fadeIn = 1f;
            }
        }





    }
}
