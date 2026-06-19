using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Accessory.Defense;
using Terraria.Audio;
using Terraria.ID;


namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class GamblersTopHatPlayer : ModPlayer
    {
        public bool hasGamblersTopHat;

        public override void ResetEffects()
        {
            hasGamblersTopHat = false; // 每帧重置
        }

        public override void PostUpdate()
        {
            if (hasGamblersTopHat)
            {
                // 获取玩家手持武器的暴击率并将其转化为基础伤害提升
                int critChance = Player.HeldItem.crit;
                Player.HeldItem.damage += (int)(Player.HeldItem.damage * (critChance / 100f));
                Player.HeldItem.crit = 0; // 将暴击率归零
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasGamblersTopHat)
                TryTriggerSuperCrit(target, ref modifiers);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasGamblersTopHat)
                TryTriggerSuperCrit(target, ref modifiers);
        }


        public void TryTriggerSuperCrit(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.rand.NextFloat() < 0.005f) // 0.5%概率
            {
                modifiers.SourceDamage *= 50f;

                // 显示战斗文本
                CombatText.NewText(Player.getRect(), Color.Gold, "超级暴击!", dramatic: true);

                // 爆发特效（最大豪华）
                SpawnSuperCritVisual(target.Center);

                // 混合音效播放（3个叠加）
                SoundEngine.PlaySound(SoundID.Item68 with { Volume = 1.4f }, Player.Center);
                SoundEngine.PlaySound(SoundID.DD2_BetsyScream with { Volume = 0.8f }, Player.Center);
                SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.9f, Pitch = 0.5f }, Player.Center);
            }
        }

        private void SpawnSuperCritVisual(Vector2 center)
        {
            for (int i = 0; i < 350; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float speed = Main.rand.NextFloat(3f, 10f) * Main.rand.NextFloat(2.5f, 3.5f); // 提速 ×2.5~3.5
                Vector2 velocity = angle.ToRotationVector2() * speed;

                int type = Utils.SelectRandom(Main.rand, DustID.SilverCoin, DustID.GemDiamond, DustID.FireworkFountain_Pink, DustID.UltraBrightTorch);
                Dust d = Dust.NewDustPerfect(center, type, velocity, 150, Color.White, Main.rand.NextFloat(1.2f, 2.0f)); // 缩短生存大小
                d.noGravity = true;
                d.fadeIn = 0.3f; // 加速显现
            }

            for (int i = 0; i < 4; i++)
            {
                float scale = 0.8f + 0.3f * i;
                float speed = (0.15f + 0.05f * i) * Main.rand.NextFloat(2.5f, 3.5f); // 提速
                for (int j = 0; j < 20; j++)
                {
                    float angle = MathHelper.TwoPi * j / 30f;
                    Vector2 offset = angle.ToRotationVector2() * (20f + i * 10f);

                    Dust ring = Dust.NewDustPerfect(center + offset, DustID.GoldFlame, -offset.SafeNormalize(Vector2.Zero) * speed, 100, Color.Gold, scale * 0.85f); // 更短生命周期视觉
                    ring.noGravity = true;
                    ring.fadeIn = 0.2f;
                }
            }

            for (int i = 0; i < 50; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                float distance = Main.rand.NextFloat(60f, 150f);
                Vector2 pos = center + angle.ToRotationVector2() * distance;

                Dust spark = Dust.NewDustPerfect(pos, DustID.MartianSaucerSpark, Vector2.Zero, 200, Color.Cyan, Main.rand.NextFloat(1.2f, 1.8f)); // 稍微缩小
                spark.noGravity = true;
                spark.fadeIn = 0.1f;
            }

            // 🌪调用形式改为传 index 和总数
            for (int i = 0; i < 120; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(300f, 300f);
                PLALightingBoltsSystem.PLA_Spawn_SuperCrit(center + offset, i, 120);
            }

        }





    }
}


