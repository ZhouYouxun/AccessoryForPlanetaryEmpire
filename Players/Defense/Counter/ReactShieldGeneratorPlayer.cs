using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;
using Terraria.ID;
using Terraria.WorldBuilding;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class ReactShieldGeneratorPlayer : ModPlayer
    {
        public bool hasReactShieldGenerator = false;
        private int damageCounter = 0;
        private const int damageThreshold = 3; // 三次伤害
        private const int invincibilityDuration = 300; // 5秒钟的无敌时间（300帧）

        public override void ResetEffects()
        {
            hasReactShieldGenerator = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {

        }


        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            HandleDamage(ref modifiers);
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            HandleDamage(ref modifiers);
        }

        private void HandleDamage(ref Player.HurtModifiers modifiers)
        {
            if (hasReactShieldGenerator)
            {
                damageCounter++;
                if (damageCounter >= damageThreshold)
                {
                    Player.immune = true;
                    Player.immuneTime = invincibilityDuration;
                    //CombatText.NewText(Player.getRect(), Color.White, "无敌时间启动！", dramatic: false, dot: false);
                    Player.AddBuff(ModContent.BuffType<INV>(), 300, true);
                    damageCounter = 0; // 重置计数器

                    // 添加基于三角函数的粒子特效
                    CreateTrigEffect(Player.Center, DustID.YellowTorch, 50, 1.5f);
                }
            }


        }

        // 创建三角函数粒子特效的方法 - 及霸
        private void CreateTrigEffect(Vector2 position, int dustID, int particleCount, float duration)
        {
            for (int i = 0; i < particleCount; i++)
            {
                float progress = i / (float)particleCount;
                float angle = progress * MathHelper.TwoPi;

                // 🚩改为三角波半径：前后快速收缩感
                float radius = 60f * (0.5f + Math.Abs((progress % 0.5f) - 0.25f) * 2f);

                // 🚩位置：中心向外圆形扩散
                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 velocity = offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 3f);

                // 🚩Dust混合：黄火焰/神圣火花/圣水
                int type = Utils.SelectRandom(Main.rand, DustID.YellowTorch, DustID.GoldFlame, DustID.UnholyWater);

                Dust dust = Dust.NewDustPerfect(position + offset, type, velocity, 120, Color.Gold, Main.rand.NextFloat(2.5f, 3.5f));
                dust.noGravity = true;
                dust.fadeIn = duration;
                dust.rotation = angle;
            }
        }




    }
}