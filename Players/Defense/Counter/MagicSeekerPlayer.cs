using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class MagicSeekerPlayer : ModPlayer
    {
        public bool HasMagicSeeker = false;
        private int storedHealingCharges = 0; // 囤积的治疗次数
        private const int maxHealingCharges = 100; // 最大吸血次数
        private const int healingPerHit = 10; // 每次受到伤害囤积10次吸血次数
        private int healProjectileCooldown = 0; // 冷却计数器

        public override void ResetEffects()
        {
            HasMagicSeeker = false;
        }

        public override void UpdateDead()
        {
            HasMagicSeeker = false;
        }

        public override void PostUpdate()
        {
            if (healProjectileCooldown > 0)
            {
                healProjectileCooldown--; // 更新冷却
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            HandleDamage();
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            HandleDamage();
        }

        private void HandleDamage()
        {
            if (HasMagicSeeker)
            {
                // 每次受伤增加7次治疗次数，最大为100次
                if (storedHealingCharges < maxHealingCharges)
                {
                    storedHealingCharges += healingPerHit;
                    if (storedHealingCharges > maxHealingCharges)
                    {
                        storedHealingCharges = maxHealingCharges;
                    }
                }

                // 触发三角函数粒子特效
                CreateTrigEffect(Player.Center, DustID.BlueTorch, 15, 1.5f);
            }
        }

        public override void OnHitAnything(float x, float y, Entity victim)
        {
            // 检查装备和冷却
            if (HasMagicSeeker && healProjectileCooldown <= 0 && storedHealingCharges > 0 && IsEligiblePlayerClass(Player))
            {
                int weaponDamage = Player.HeldItem.damage;

                // 遍历所有玩家进行治疗
                foreach (Player p in Main.player)
                {
                    if (p.active && !p.dead)
                    {
                        float distance = Vector2.Distance(p.Center, new Vector2(x, y));
                        if (distance < 500f) // 在500像素内治疗玩家
                        {
                            int healAmount = (int)Math.Round(Main.rand.NextFloat(0.05f, 0.95f) * weaponDamage);
                            p.statLife += healAmount;
                            p.HealEffect(healAmount, broadcast: true); // 显示治疗效果
                        }
                    }
                }

                // 触发十字架粒子特效
                CreateHealingEffect(Player.Center);

                // 减少一次治疗次数
                storedHealingCharges--;
                healProjectileCooldown = 60; // 1秒冷却时间
            }
        }

        // 创建三角函数粒子特效的方法
        // 创建三角函数螺旋粒子特效 + 蓝色光点
        private void CreateTrigEffect(Vector2 position, int dustID, int particleCount, float duration)
        {
            float time = Main.GlobalTimeWrappedHourly;

            // 🌀 Dust 螺旋粒子
            for (int i = 0; i < particleCount; i++)
            {
                float angle = i * MathHelper.TwoPi / particleCount + time * 2f; // 加入时间偏移形成旋转
                float wave = (float)Math.Sin(i * 0.6f + time * 3f); // 波动影响
                float radius = 20f + wave * 8f + (time % 1f) * 25f;

                Vector2 offset = angle.ToRotationVector2() * radius;
                Vector2 pos = position + offset;
                Vector2 vel = offset.SafeNormalize(Vector2.Zero) * (1.2f + wave * 0.4f);

                int type = Main.rand.NextBool(2) ? dustID : DustID.BlueTorch;
                Dust d = Dust.NewDustPerfect(pos, type, vel * 0.5f, 100, Color.Lerp(Color.BlueViolet, Color.LightBlue, Main.rand.NextFloat()), Main.rand.NextFloat(2.6f, 3.2f));
                d.noGravity = true;
                d.fadeIn = duration;
            }

            // ✨ 光点螺旋（独立循环，增强视觉节奏）
            for (int j = 0; j < particleCount / 2; j++)
            {
                float angle = j * MathHelper.TwoPi / (particleCount / 2) + time * 1.4f;
                float radius = 30f + (float)Math.Sin(j * 1.2f + time * 4f) * 12f;
                Vector2 offset = angle.ToRotationVector2() * radius;

                Vector2 pos = position + offset;
                Vector2 vel = offset.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(1f, 2f);

                PLALightingBoltsSystem.PLA_SpawnTrigSparkle(pos, vel, Main.rand.NextFloat(0.9f, 1.2f));
            }
        }



        // 创建治疗粒子特效的方法
        private void CreateHealingEffect(Vector2 position)
        {
            int dustID = DustID.GreenTorch;
            float duration = 1.5f;
            float sizeMultiplier = 4f;
            float brightnessMultiplier = 6f;

            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue;

                // 水平线
                Vector2 horizontalOffset = new Vector2(i * 16, 0);
                Dust horizontalDust = Dust.NewDustPerfect(position + horizontalOffset, dustID, Vector2.Zero, 100, Color.Green * brightnessMultiplier, 1.5f * sizeMultiplier);
                horizontalDust.noGravity = true;
                horizontalDust.fadeIn = duration;
                horizontalDust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                horizontalDust.velocity = Vector2.Zero;
                horizontalDust.customData = Main.rand.NextFloat(0.05f, 0.1f);

                // 垂直线
                Vector2 verticalOffset = new Vector2(0, i * 16);
                Dust verticalDust = Dust.NewDustPerfect(position + verticalOffset, dustID, Vector2.Zero, 100, Color.Green * brightnessMultiplier, 1.5f * sizeMultiplier);
                verticalDust.noGravity = true;
                verticalDust.fadeIn = duration;
                verticalDust.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                verticalDust.velocity = Vector2.Zero;
                verticalDust.customData = Main.rand.NextFloat(0.05f, 0.1f);
            }
        }

        private bool IsEligiblePlayerClass(Player player)
        {
            // 默认的职业类型检测
            bool isEligible = player.HeldItem.DamageType == DamageClass.Melee ||
                              player.HeldItem.DamageType == DamageClass.Ranged ||
                              player.HeldItem.DamageType == DamageClass.Magic ||
                              player.HeldItem.DamageType == DamageClass.Throwing;

            // 检查 CalamityMod 的 RogueDamageClass
            Mod calamityMod;
            if (ModLoader.TryGetMod("CalamityMod", out calamityMod))
            {
                var rogueType = calamityMod.Code.DefinedTypes.FirstOrDefault(t => t.Name == "RogueDamageClass");

                if (rogueType != null)
                {
                    var rogueInstanceField = rogueType.GetField("Instance", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
                    var rogueInstance = rogueInstanceField?.GetValue(null) as DamageClass;
                    if (rogueInstance != null && player.HeldItem.DamageType == rogueInstance)
                    {
                        isEligible = true; // 允许 Rogue 类型职业也能触发技能
                    }
                }
            }

            return isEligible;
        }

    }
}