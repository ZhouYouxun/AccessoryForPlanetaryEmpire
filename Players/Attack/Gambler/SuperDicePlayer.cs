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
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Accessory.Defense;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class SuperDicePlayer : ModPlayer
    {
        public bool hasSuperDice;
        public bool hideVisual;


        public override void ResetEffects()
        {
            // 确保 hasSuperDice 每帧重置
            hasSuperDice = false;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (hasSuperDice)
            {
                // 对召唤武器也能生效
                // if (item.DamageType != DamageClass.Summon)
                {
                    // 根据模式调整随机伤害浮动范围
                    float minMultiplier = Main.getGoodWorld ? 1.2f : 0.6f;
                    float maxMultiplier = Main.getGoodWorld ? 2.5f : 1.8f;

                    // 在指定范围内随机一个数
                    float randomDamageMultiplier = Main.rand.NextFloat(minMultiplier, maxMultiplier);
                    damage *= randomDamageMultiplier;
                }
            }
        }


        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            if (hasSuperDice)
            {
                // 对于召唤武器不进行修改
                if (item.DamageType != DamageClass.Summon)
                {
                    // 生成一个在当前暴击率和100%之间的随机浮点数
                    float randomCrit = Main.rand.NextFloat(crit, 100f);
                    crit = randomCrit;
                }
            }
        }


        private int superDiceTimer = 0;

        public override void PostUpdate()
        {
            if (hasSuperDice && !hideVisual && Player.active)
            {
                superDiceTimer++;

                int cycleLength = 8 * 60;     // 8 秒周期
                int shakeDuration = 3 * 60;   // 前 3 秒是摇骰子特效
                int throwMoment = 3 * 60;     // 第 3 秒末释放一次冲击波

                int timer = superDiceTimer % cycleLength;

                Vector2 center = Player.Center;

                // 🌀阶段一：摇骰子期间，释放小范围特效
                if (timer < shakeDuration)
                {
                    // 轨迹线 Dust
                    if (Main.rand.NextBool(2))
                    {
                        Vector2 offset = Main.rand.NextVector2Circular(60f, 60f);
                        Vector2 velocity = offset.SafeNormalize(Vector2.Zero) * 2f;

                        Dust trail = Dust.NewDustPerfect(center, DustID.GemDiamond, velocity, 150, Color.White, 1.4f);
                        trail.noGravity = true;
                        trail.fadeIn = 1.2f;
                    }

                    // 点阵闪烁 Dust
                    for (int i = 0; i < 2; i++)
                    {
                        Vector2 pos = center + Main.rand.NextVector2Circular(80f, 80f);
                        Color color = new Color(
                            Main.rand.Next(180, 255),
                            Main.rand.Next(180, 255),
                            Main.rand.Next(180, 255)
                        );
                        Dust flash = Dust.NewDustPerfect(pos, DustID.FireworkFountain_Blue, Vector2.Zero, 0, color, 1.1f);
                        flash.noGravity = true;
                        flash.scale = 1.2f;
                    }
                }

                // 💥阶段二：在第 3 秒末释放冲击波粒子
                else if (timer == throwMoment)
                {
                    // 释放 20 个放射性 Dust
                    for (int i = 0; i < 20; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 20f;
                        Vector2 velocity = angle.ToRotationVector2() * 5f;

                        Dust boom = Dust.NewDustPerfect(center, DustID.FireworkFountain_Blue, velocity, 150, Color.White, 1.8f);
                        boom.noGravity = true;
                        boom.scale = 1.4f;
                    }

                    // 💡可选：加点音效
                    //SoundEngine.PlaySound(SoundID.Item4 with { Volume = 0.6f }, center);
                }

                // 第 3–8 秒为冷却阶段，无需处理
            }
        }




    }
}
