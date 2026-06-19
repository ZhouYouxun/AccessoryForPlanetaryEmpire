using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class TamingWhipRingPlayer : ModPlayer
    {
        public bool hasTamingWhipRing = false;

        public override void ResetEffects()
        {
            hasTamingWhipRing = false;
        }

        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 检查玩家是否装备了TamingWhipRing并且是否是召唤物弹幕
            if (hasTamingWhipRing && proj.DamageType == DamageClass.Summon)
            {
                // 50% 的概率触发效果
                if (Main.rand.NextFloat() < 0.50f)
                {
                    // 给予玩家 TamingWhipRingBuff，持续 5 秒（300 ticks）
                    Player.AddBuff(ModContent.BuffType<Buffs.TamingWhipRingBuff>(), 300);

                    // 粒子效果
                    for (int i = 0; i < 20; i++) // 生成20个粒子
                    {
                        // 计算粒子的位置和速度
                        Vector2 position = proj.Center;
                        Vector2 velocity = Vector2.Normalize(target.Center - position) * Main.rand.NextFloat(2f, 6f); // 粒子速度方向随机
                        velocity = velocity.RotatedByRandom(MathHelper.ToRadians(30)); // 添加随机旋转
                        velocity *= Main.rand.NextFloat(0.5f, 1.5f); // 随机化粒子速度

                        // 随机偏移和大小
                        float scale = Main.rand.NextFloat(0.5f, 1.5f);

                        // 创建粒子
                        Dust dust = Dust.NewDustPerfect(position, DustID.GoldCoin, velocity, 150, default, scale);
                        dust.noGravity = true; // 无重力效果
                        dust.fadeIn = 1f; // 粒子逐渐消失
                        dust.rotation = Main.rand.NextFloat(0f, MathHelper.TwoPi); // 随机旋转
                        //dust.shader = GameShaders.Armor.GetSecondaryShader(59, Player); // 使用彩纸效果的shader
                    }
                }
            }
        }








    }
}