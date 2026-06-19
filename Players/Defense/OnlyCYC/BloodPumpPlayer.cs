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
using Terraria.Graphics.Renderers;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class BloodPumpPlayer : ModPlayer
    {
        public const int CooldownFrames = 30 * 60;
        public const int ActiveFrames = 10 * 60;
        public int CooldownTimer;
        public int ActiveTimer;

        public bool hasBloodPump;
        public override void ResetEffects()
        {
            // 确保 hasBloodPump 每帧重置
            hasBloodPump = false;
        }

        public override void Initialize()
        {
            CooldownTimer = 0;
            ActiveTimer = 0;
        }



        public override void UpdateDead()
        {
            hasBloodPump = false;
        }

        public override void PostUpdateMiscEffects()
        {
            if (Player.whoAmI == Main.myPlayer && hasBloodPump)
            {
                if (CheckBloodPumpKeybind() && CooldownTimer <= 0)
                {
                    ActivateBloodPump();
                }
            }
        }

        private bool CheckBloodPumpKeybind()
        {
            // 这里假设已经在 KeyPlayers.cs 和 KeybindSystem.cs 中定义了相应的按键
            // 返回按键是否被按下的布尔值
            return KeybindSystem.BLKeybind.JustPressed;
        }
        private void ActivateBloodPump()
        {
            // 恢复已损失生命值的一半
            Player.statLife += (Player.statLifeMax2 - Player.statLife) / 2;
            // 添加50点生命再生和70%的伤害减免,10秒钟 (专有buff)
            Player.AddBuff(ModContent.BuffType<BloodPumpBuff>(), ActiveFrames, true);
            ActiveTimer = ActiveFrames;
            CooldownTimer = CooldownFrames;
            // Player.AddBuff(BuffID.PotionSickness, 2400, true); // 添加60秒的治疗冷却debuff
            //CombatText.NewText(Player.getRect(), Color.OrangeRed, "活血泵主动技能已开启！", dramatic: false, dot: false);

            // 创建“活血泵激活”粒子特效
            CreateEnhancedDNAXShapeEffect();
        }


        // 更新粒子效果的方法
        public override void PostUpdate()
        {
            if (CooldownTimer > 0)
            {
                CooldownTimer--;
            }

            if (ActiveTimer > 0)
            {
                if (Player.HasBuff(ModContent.BuffType<BloodPumpBuff>()))
                {
                    ActiveTimer--;
                }
                else
                {
                    ActiveTimer = 0;
                }
            }

            foreach (Dust dust in Main.dust)
            {
                if (dust.active && dust.customData is float rotationSpeed)
                {
                    dust.rotation += rotationSpeed; // 旋转粒子
                    dust.scale *= 0.98f; // 缩小粒子
                    dust.alpha += 2; // 渐变粒子的透明度
                }
            }
        }

        private void CreateEnhancedDNAXShapeEffect()
        {
            Vector2 center = Player.Center;
            float time = Main.GlobalTimeWrappedHourly;


            // 🌪 有序粒子：双螺旋基因链构成结构
            int layers = 4;
            for (int layer = 0; layer < layers; layer++)
            {
                float radius = 28f + layer * 6f;
                int points = 48;
                for (int i = 0; i < points; i++)
                {
                    float rot = MathHelper.TwoPi * i / points + time * (1.5f + 0.2f * layer);
                    Vector2 pos = center + rot.ToRotationVector2() * radius;

                    Dust d = Dust.NewDustPerfect(pos, DustID.HeartCrystal);
                    d.velocity = (pos - center).SafeNormalize(Vector2.Zero).RotatedBy(MathF.Sin(time * 5f + i)) * Main.rand.NextFloat(0.5f, 1.2f);
                    d.scale = Main.rand.NextFloat(1.8f, 2.5f);
                    d.noGravity = true;
                    d.color = (i % 2 == 0) ? new Color(255, 120, 150) : new Color(255, 80, 200);
                }
            }

            // 💥 同步外围脉冲爆炸，制造外圈电场感
            for (int i = 0; i < 60; i++)
            {
                float spin = Main.rand.NextFloat(MathHelper.TwoPi);
                float radius = Main.rand.NextFloat(48f, 72f);
                Vector2 pos = center + spin.ToRotationVector2() * radius;

                Dust d = Dust.NewDustPerfect(pos, DustID.Teleporter);
                d.velocity = -spin.ToRotationVector2() * Main.rand.NextFloat(2f, 4f);
                d.scale = Main.rand.NextFloat(1.8f, 2.5f);
                d.noGravity = true;
            }

            // 🔁 环状粒子 + 多种Dust混合
            int[] dustTypes = new int[] { 271, 164, 130 };
            int shapeLayers = 3;
            for (int layer = 0; layer < shapeLayers; layer++)
            {
                int sides = 18 + layer * 3;
                float radius = 90f + 40f * layer;
                Color baseColor = Color.Pink;

                for (int i = 0; i < sides; i++)
                {
                    float progress = i / (float)sides;
                    float angle = MathHelper.TwoPi * progress + time * (0.5f + 0.3f * layer);
                    Vector2 pos = center + angle.ToRotationVector2() * radius;

                    // 🎨 色彩律动：沿环渐变
                    float hueShift = progress * 0.4f + time * 0.2f;
                    Color finalColor = Color.Lerp(baseColor, Color.White, (float)Math.Sin(hueShift * MathHelper.TwoPi) * 0.5f + 0.5f);

                    // 🌀 Dust 粒子生成
                    int dustType = Main.rand.NextFromList(dustTypes); // 随机三种之一
                    Dust d = Dust.NewDustPerfect(pos, dustType);
                    d.velocity = -angle.ToRotationVector2() * Main.rand.NextFloat(0.8f, 1.6f);
                    d.scale = 2.2f + 0.3f * layer;
                    d.color = finalColor;
                    d.noGravity = true;
                }
            }




            PLALightingBoltsSystem.SpawnHealSparkles(center);

            SoundEngine.PlaySound(SoundID.Item29 with { Volume = 0.6f, Pitch = -0.2f }, Player.Center);
        }






    }
}
