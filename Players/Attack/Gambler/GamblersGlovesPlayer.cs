using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Buffs;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class GamblersGlovesPlayer : ModPlayer
    {
        public bool hasGamblersGloves;
        private bool nextAttackBoosted;

        public override void ResetEffects()
        {
            hasGamblersGloves = false;
        }

        // 主处理函数：统一处理免疫判定
        private void TryTriggerGloveEffect(Vector2 sourcePosition)
        {
            if (Main.rand.NextFloat() < 0.20f)
            {
                // ✅ 成功免疫，添加状态
                Player.immune = true;
                Player.immuneTime = 300;
                Player.AddBuff(ModContent.BuffType<INV>(), 300, true);

                // 🔴 红色Dust冲击波
                for (int i = 0; i < 80; i++)
                {
                    float angle = Main.rand.NextFloat(MathHelper.TwoPi);
                    float speed = Main.rand.NextFloat(4f, 8f);
                    Vector2 velocity = angle.ToRotationVector2() * speed;

                    Dust d = Dust.NewDustPerfect(Player.Center, DustID.Blood, velocity, 100, Color.Red, 1.8f);
                    d.noGravity = true;
                }
            }
            else
            {
                // ❌ 免疫失败，受到1.5倍伤害并获得5秒增益
                Player.AddBuff(ModContent.BuffType<GamblersGloves>(), 300, true);
                nextAttackBoosted = true;

                // 🔵 蓝色喷射粒子（从玩家向两侧喷出）
                for (int i = 0; i < 25; i++)
                {
                    Vector2 offset = new Vector2(Main.rand.NextFloat(-20f, 20f), Main.rand.NextFloat(-32f, 32f));
                    Vector2 pos = Player.Center + offset;
                    Vector2 velocity = new Vector2(Main.rand.NextBool() ? -1f : 1f, Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(2f, 5f);

                    Dust d = Dust.NewDustPerfect(pos, DustID.Electric, velocity, 150, Color.Cyan, 1.3f);
                    d.noGravity = true;
                }
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (hasGamblersGloves)
            {
                TryTriggerGloveEffect(npc.Center);
                if (!Player.HasBuff(ModContent.BuffType<INV>()))
                    modifiers.FinalDamage *= 1.5f;
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (hasGamblersGloves)
            {
                TryTriggerGloveEffect(proj.Center);
                if (!Player.HasBuff(ModContent.BuffType<INV>()))
                    modifiers.FinalDamage *= 1.5f;
            }
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (nextAttackBoosted)
            {
                damage *= 2f;
                nextAttackBoosted = false;

                // 💥 显示强化提示
                CombatText.NewText(Player.getRect(), Color.GreenYellow, "攻击已被增强!", dramatic: false, dot: false);
            }
        }
    }
}
