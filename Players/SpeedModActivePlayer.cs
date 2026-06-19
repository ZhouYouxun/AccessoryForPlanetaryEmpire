using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players
{
    public class SpeedModActivePlayer : ModPlayer
    {
        public const int CooldownFrames = 60 * 60;
        public const int ActiveFrames = 10 * 60;
        public bool speedModActiveEquipped = false;
        public int CooldownTimer;
        public int ActiveTimer;

        public override void ResetEffects()
        {
            speedModActiveEquipped = false;
        }

        public override void PostUpdate()
        {
            if (CooldownTimer > 0)
            {
                CooldownTimer--;
            }

            if (ActiveTimer > 0)
            {
                if (Player.HasBuff(ModContent.BuffType<Buffs.SpeedModActiveBuff>()))
                {
                    ActiveTimer--;
                }
                else
                {
                    ActiveTimer = 0;
                }
            }
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (speedModActiveEquipped && KeybindSystem.SMAKeybind.JustPressed) // Make sure to replace YourMod with your actual mod's namespace
            {
                if (CooldownTimer <= 0)
                {
                    Player.AddBuff(ModContent.BuffType<Buffs.SpeedModActiveBuff>(), ActiveFrames); // 10 seconds speed boost
                    ActiveTimer = ActiveFrames;
                    CooldownTimer = CooldownFrames;

                    // 创建粒子特效
                    CreateSpeedBoostEffect(Player.Center);

                    CombatText.NewText(Player.getRect(), Color.White, "动力推进装置已启用！", dramatic: false, dot: false);
                }
                else
                {
                    CombatText.NewText(Player.getRect(), Color.White, "动力推进装置尚未冷却完毕！", dramatic: false, dot: false);
                }
            }
        }

        // 创建粒子特效的方法
        private void CreateSpeedBoostEffect(Vector2 position)
        {
            int dustID = DustID.RainbowTorch;
            int particleCount = 10; // 每帧生成的粒子数
            float duration = 1f; // 粒子的持续时间
            float sizeMultiplier = 2f; // 粒子的大小倍数
            float brightnessMultiplier = 2f; // 粒子的亮度倍数

            for (int i = 0; i < particleCount; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi); // 随机生成角度
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 10f;

                for (int j = 0; j < 100; j++) // 创建100格长的直线粒子特效
                {
                    Vector2 particlePosition = position + velocity * j;
                    Dust dust = Dust.NewDustPerfect(particlePosition, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
                    dust.noGravity = true;
                    dust.fadeIn = duration;
                    dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                    dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                    dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
                }
            }
        }
    }
}
