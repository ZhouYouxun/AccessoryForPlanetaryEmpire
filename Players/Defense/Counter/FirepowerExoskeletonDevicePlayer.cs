using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Buffs;
using Terraria.Audio;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class FirepowerExoskeletonDevicePlayer : ModPlayer
    {
        public bool firepowerExoskeleton;
        private int attackSpeedBoostDuration;
        private const int attackSpeedBoostTime = 300; // 5秒（300帧）
        private const int cooldownDuration = 900; // 15秒冷却时间（900帧）
        private int cooldownTimer = 0; // 冷却计时器

        public override void ResetEffects()
        {
            firepowerExoskeleton = false;
        }

        public override void UpdateDead()
        {
            // 确保死亡时重置 Buff 和冷却
            Player.ClearBuff(ModContent.BuffType<FirepowerExoskeletonDeviceBuff>());
            cooldownTimer = 0;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (firepowerExoskeleton && cooldownTimer <= 0)
            {
                if (info.Damage >= Player.statLifeMax2 * 0.15)
                {
                    // 施加 FirepowerExoskeletonDeviceBuff，持续5秒
                    Player.AddBuff(ModContent.BuffType<FirepowerExoskeletonDeviceBuff>(), attackSpeedBoostTime);

                    // 开始冷却
                    cooldownTimer = cooldownDuration;

                    // 生成粒子特效
                    CreateExplosionEffect(Player.Center);
                }
            }
        }

        public override void PostUpdate()
        {
            if (attackSpeedBoostDuration > 0)
            {
                attackSpeedBoostDuration--;
                Player.GetDamage(DamageClass.Generic) += 2.00f;
            }

            // 更新冷却计时器
            if (cooldownTimer > 0)
            {
                cooldownTimer--;
            }
        }

        // 创建爆炸粒子特效的方法
        private void CreateExplosionEffect(Vector2 position)
        {
            float time = Main.GlobalTimeWrappedHourly;

            // 🔴 1.主爆炸火焰粒子（箭头感 + 规整上冲）
            for (int i = 0; i < 60; i++)
            {
                float angle = MathHelper.TwoPi * i / 60f;
                Vector2 direction = angle.ToRotationVector2();
                Vector2 pos = position + direction * Main.rand.NextFloat(12f, 24f);

                Dust d = Dust.NewDustPerfect(pos, Utils.SelectRandom(Main.rand, DustID.RedTorch, DustID.OrangeTorch, DustID.Blood));
                d.velocity = Vector2.UnitY * Main.rand.NextFloat(3.2f, 5.2f); // 向上冲
                d.noGravity = true;
                d.fadeIn = 1f;
                d.scale = Main.rand.NextFloat(2.2f, 3.0f);
                d.rotation = MathHelper.PiOver2; // 始终朝上（像箭头）
            }


            // 💥 2.环形扩散（能量圈感）
            for (int i = 0; i < 24; i++)
            {
                float angle = MathHelper.TwoPi * i / 24f;
                Vector2 offset = angle.ToRotationVector2() * 48f;
                Vector2 pos = position + offset;

                Dust d = Dust.NewDustPerfect(pos, DustID.Teleporter);
                d.velocity = -offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2.5f, 4f);
                d.scale = Main.rand.NextFloat(2.1f, 2.6f);
                d.noGravity = true;
            }

            // ⚡ 3.红色光点特效（调用外包）
            PLALightingBoltsSystem.SpawnRedSparkExplosion(position);

            // 🔊 4.播放强烈爆炸音效
            SoundEngine.PlaySound(SoundID.Item14 with { Volume = 1.0f, Pitch = -0.2f }, position);
        }




    }
}