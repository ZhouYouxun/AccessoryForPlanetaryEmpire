using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class AorticThrusterPlayer : ModPlayer
    {
        public bool hasAorticThruster;
        private bool isCooldown; // 是否在冷却中
        private int cooldownTimer; // 冷却计时器

        public override void ResetEffects()
        {
            hasAorticThruster = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            if (hasAorticThruster && Player.statLife == Player.statLifeMax2 && !isCooldown)
            {
                TriggerEffect(hurtInfo.Damage);
            }
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            if (hasAorticThruster && Player.statLife == Player.statLifeMax2 && !isCooldown)
            {
                TriggerEffect(hurtInfo.Damage);
            }
        }

        private void TriggerEffect(int damage)
        {
            // 计算红心数量
            int totalHealthToHeal = damage * 2;
            int heartCount = totalHealthToHeal / 20;

            // 随机散落红心
            for (int i = 0; i < heartCount; i++)
            {
                Vector2 spawnPosition = Player.Center + new Vector2(Main.rand.Next(-550, 550), Main.rand.Next(-100, 100));
                Item.NewItem(Player.GetSource_FromThis(), spawnPosition, ItemID.Heart);
            }

            {
                // ❤️主轴：有序螺旋扩散 + 波动速度
                for (int i = -32; i <= 32; i += 4)
                {
                    float angle = MathHelper.PiOver2 + (i / 32f) * MathHelper.Pi + Main.rand.NextFloat(-0.3f, 0.3f); // 顺时针螺旋角
                    float wave = (float)Math.Sin(i * 0.3f + Main.GlobalTimeWrappedHourly * 5f) * 1.2f;
                    Vector2 velocity = angle.ToRotationVector2() * (4.2f + wave);

                    Vector2 dustPos = Player.Bottom + new Vector2(i, -6f);
                    Dust d = Dust.NewDustPerfect(dustPos, DustID.HeartCrystal, velocity);
                    d.noGravity = true;
                    d.scale = Main.rand.NextFloat(1.9f, 2.7f);
                }

                // ❤️中心延伸出的细线（2条并排）+ 偏移随机性
                for (int side = -1; side <= 1; side += 2)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        float t = i / 12f;
                        Vector2 start = Player.Center + new Vector2(side * 2f, -6f); // 左右偏移2像素
                        Vector2 end = Player.Bottom + new Vector2(Main.rand.NextFloat(-28f, 28f), -6f); // Dust散布位置附近
                        Vector2 pos = Vector2.Lerp(start, end, t) + Main.rand.NextVector2Circular(1.5f, 1.5f); // 加抖动
                        Vector2 vel = (end - start).SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(0.1f, 0.8f);

                        Dust d = Dust.NewDustPerfect(pos, DustID.HeartCrystal, vel);
                        d.scale = Main.rand.NextFloat(1.8f, 2.2f);
                        d.noGravity = true;
                    }
                }
                // 💗粉红光点横线爆裂
                PLALightingBoltsSystem.PLA_Spawn_HeartExplosionLine(Player.Bottom);

                // 音效
                SoundEngine.PlaySound(SoundID.Item9, Player.Center);
            }


            // 开启冷却
            isCooldown = true;
            cooldownTimer = 900; // 15秒冷却（900帧）

            // 添加冷却结束时的效果
            //Main.NewText("Aortic Thruster cooling down...", Color.Red);
        }

        public override void PostUpdate()
        {
            if (isCooldown)
            {
                cooldownTimer--;

                if (cooldownTimer <= 0)
                {
                    isCooldown = false;
                    //Main.NewText("Aortic Thruster is ready!", Color.Green);

                    // 触发冷却结束时的粒子效果
                    for (int i = 0; i < 20; i++)
                    {
                        Vector2 particlePosition = Player.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(-50, 50));
                        Dust.NewDust(particlePosition, 10, 10, DustID.HealingPlus, 0f, 0f, 0, default, Main.rand.NextFloat(1f, 2f));
                    }
                }
            }
        }
    }
}
