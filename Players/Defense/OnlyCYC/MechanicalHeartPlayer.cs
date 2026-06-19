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
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class MechanicalHeartPlayer : ModPlayer
    {
        public bool hasMechanicalHeart;
        public const int NormalCooldownFrames = 150 * 60; // 冷却时间为两分半
        public const int GoodWorldCooldownFrames = 30 * 60; // getGoodWorld 模式下冷却时间为30秒
        public int DeathPreventionCooldownTimer;
        private const float damageReduction = 0.8f; // 伤害减免80%
        private const int recoveryDuration = 5; // 恢复持续时间5秒
        public int CurrentCooldownFrames => Main.getGoodWorld ? GoodWorldCooldownFrames : NormalCooldownFrames;

        public override void ResetEffects()
        {
            hasMechanicalHeart = false;
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            if (hasMechanicalHeart && DeathPreventionCooldownTimer <= 0)
            {
                CombatText.NewText(Player.getRect(), Color.Red, "受到致命碰撞伤害，复活已触发！", dramatic: false, dot: false);
                Player.AddBuff(ModContent.BuffType<MechanicalHeartBuff>(), 600, true);
                Player.AddBuff(ModContent.BuffType<INV>(), 600, true);
                Player.statLife += (int)(Player.statLifeMax2 * 0.2 * recoveryDuration); // 总恢复量为总血量的100%
                DeathPreventionCooldownTimer = CurrentCooldownFrames;
                CreateHeartShapeEffect(Player.Center);
                return false; // 阻止玩家死亡
            }
            return true;
        }

        public override void UpdateDead()
        {
            hasMechanicalHeart = false; // 如果玩家死亡，重置效果
        }


        private void CreateHeartShapeEffect(Vector2 position)
        {
            int[] dustTypes = new int[] { 156, 160, 187, 226 };
            float sizeMultiplier = 5f;
            float brightnessMultiplier = 3.5f;
            float duration = 1.8f;

            // 💙 1. 多层心形：从小到大、由内向外淡出
            for (int layer = 0; layer < 3; layer++)
            {
                int particleCount = 100 + layer * 40;
                float radiusScale = sizeMultiplier * (1f + layer * 0.35f);
                Color baseColor = Color.Lerp(Color.LightBlue, Color.Cyan, layer / 3f);

                for (int i = 0; i < particleCount; i++)
                {
                    float t = MathHelper.TwoPi * i / particleCount;
                    float x = 16 * (float)Math.Pow(Math.Sin(t), 3);
                    float y = -(13 * (float)Math.Cos(t) - 5 * (float)Math.Cos(2 * t) - 2 * (float)Math.Cos(3 * t) - (float)Math.Cos(4 * t));
                    Vector2 offset = new Vector2(x, y) * radiusScale;
                    Vector2 pos = position + offset;

                    int type = Main.rand.Next(dustTypes);
                    Dust d = Dust.NewDustPerfect(pos, type, Vector2.Zero, 100, baseColor * brightnessMultiplier, 1.4f + 0.2f * layer);
                    d.noGravity = true;
                    d.fadeIn = duration + Main.rand.NextFloat(0.2f);
                    d.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                    d.customData = 0.06f + 0.02f * layer;
                    d.fadeIn = duration + Main.rand.NextFloat(0.2f); // ✔️ 渐显
                    d.scale = 1.4f + 0.2f * layer;                   // ✔️ 初始大小（间接影响消散）
                }
            }

            // 🌐 2. 外围蓝色脉冲光圈：星芒式脉冲感
            for (int i = 0; i < 40; i++)
            {
                float angle = MathHelper.TwoPi * i / 40f;
                float radius = 96f + 16f * (float)Math.Sin(i * 0.6f + Main.GlobalTimeWrappedHourly * 2f);
                Vector2 pos = position + angle.ToRotationVector2() * radius;

                int type = Main.rand.Next(dustTypes);
                Dust d = Dust.NewDustPerfect(pos, type, -angle.ToRotationVector2() * 3f, 100, Color.White * 0.6f, 1.8f);
                d.noGravity = true;
            }

            // ✨ 3. 中心跃动光尘
            for (int i = 0; i < 20; i++)
            {
                Vector2 pos = position + Main.rand.NextVector2Circular(10f, 10f);
                int type = Main.rand.Next(dustTypes);
                Dust d = Dust.NewDustPerfect(pos, type, Main.rand.NextVector2Circular(1f, 1f), 100, Color.Cyan * 0.8f, 1.2f);
                d.noGravity = true;
            }

            // 🎵 音效（科技复活感）
            SoundEngine.PlaySound(SoundID.Item92 with { Volume = 0.6f, Pitch = -0.4f }, position);
        }


        public override void PostUpdate()
        {
            if (DeathPreventionCooldownTimer > 0)
            {
                DeathPreventionCooldownTimer--;
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



    }

}
