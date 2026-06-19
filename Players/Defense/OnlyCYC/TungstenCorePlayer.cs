using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class TungstenCorePlayer : ModPlayer
    {
        public bool hasTungstenCore; // 是否装备了碳化钨内胆
        private bool damageReductionReady = false; // 是否准备好下一次减伤
        private int cooldownTimer = 0; // 冷却计时器（可选）
        private const int CooldownMax = 0; // 暂时不用冷却，可设为60或更高
        public bool hideVisual;

        public override void ResetEffects()
        {
            hasTungstenCore = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            OnPlayerHit(hurtInfo.Damage);
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            OnPlayerHit(hurtInfo.Damage);
        }

        private void OnPlayerHit(int damage)
        {
            if (!hasTungstenCore)
                return;

            // 🧠 检查是否触发“重大伤害”
            int threshold = (int)(Player.statLifeMax2 * 0.1f);

            // 播放防御音效（仅当未隐藏特效时）
            if (!hideVisual)
            {
                SoundEngine.PlaySound(new SoundStyle("AccessoryForPlanetaryEmpire/Sounds/TungstenBlock") with { Volume = 0.4f, Pitch = 0f }, Player.Center);
            }

            if (!damageReductionReady)
            {
                // 如果伤害达到或超过最大生命的10%
                if (damage >= threshold)
                {
                    damageReductionReady = true;

                    PlayShieldImpactEffect();
                    // 💥 生成反应粒子或特效提示
                    //for (int i = 0; i < 15; i++)
                    //{
                    //    Vector2 vel = Main.rand.NextVector2CircularEdge(5f, 5f);
                    //    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.SteampunkSteam, vel.X, vel.Y, 100, default, 1.6f);
                    //    Main.dust[dust].noGravity = true;
                    //}
                }
            }
            else
            {
                // 🌑 下一次伤害减半
                int reduced = (int)(damage * 0.5f);
                int prevent = damage - reduced;

                Player.statLife += prevent; // 直接抵消掉这部分伤害
                //CombatText.NewText(Player.Hitbox, Microsoft.Xna.Framework.Color.Gray, "Damage Absorbed!", true, false);

                // 💥 可选：粒子反馈
                //for (int i = 0; i < 25; i++)
                //{
                //    Vector2 vel = Main.rand.NextVector2Circular(8f, 8f);
                //    int dust = Dust.NewDust(Player.Center, 0, 0, DustID.Smoke, vel.X, vel.Y, 150, default, 1.3f);
                //    Main.dust[dust].noGravity = true;
                //}

                // 清除状态
                damageReductionReady = false;
                cooldownTimer = CooldownMax;
            }
        }

        public override void PostUpdate()
        {
            if (cooldownTimer > 0)
                cooldownTimer--;
        }



        private void PlayShieldImpactEffect()
        {
            int dustType = 109; // 你指定的粒子

            Vector2 center = Player.Center;

            // ---------------------------
            // ① 盾牌六边形冲击波（稳定 + 有力）
            // ---------------------------
            float radius = 28f;
            for (int i = 0; i < 6; i++)
            {
                float angle = MathHelper.TwoPi / 6f * i;
                Vector2 pos = center + angle.ToRotationVector2() * radius;

                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(4f, 7f);

                Dust d = Dust.NewDustPerfect(pos, dustType, vel, 150, default, Main.rand.NextFloat(1.2f, 1.7f));
                d.noGravity = true;
            }

            // ---------------------------
            // ② 外圈金属碎屑飞散（强度来源）
            // ---------------------------
            for (int i = 0; i < 20; i++)
            {
                Vector2 vel = Main.rand.NextVector2Unit() * Main.rand.NextFloat(6f, 12f);

                Dust d = Dust.NewDustPerfect(center, dustType, vel, 120, default, Main.rand.NextFloat(1.3f, 2.0f));
                d.noGravity = true;
            }

            // ---------------------------
            // ③ 冲击火花（坚硬金属被击中的火星）
            // ---------------------------
            for (int i = 0; i < 12; i++)
            {
                float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                Vector2 pos = center + angle.ToRotationVector2() * Main.rand.NextFloat(8f, 16f);

                Vector2 vel = angle.ToRotationVector2() * Main.rand.NextFloat(3f, 6f);

                Dust d = Dust.NewDustPerfect(pos, dustType, vel, 200, default, Main.rand.NextFloat(0.9f, 1.3f));
                d.noGravity = true;
            }

            // ---------------------------
            // ④ 核心震荡（让中心也有动感）
            // ---------------------------
            for (int i = 0; i < 10; i++)
            {
                Dust d = Dust.NewDustPerfect(
                    center,
                    dustType,
                    Main.rand.NextVector2Unit() * Main.rand.NextFloat(2f, 5f),
                    150,
                    default,
                    Main.rand.NextFloat(1.4f, 2.1f)
                );
                d.noGravity = true;
            }
        }





    }
}
