using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Gambler
{
    public class CyberChipPlayer : ModPlayer
    {
        public bool hasCyberChip;
        private float damageMultiplier = 1f;

        public override void ResetEffects()
        {
            hasCyberChip = false;
            damageMultiplier = 1f; // 重置伤害加成
        }

        public override void PostUpdate()
        {
            if (hasCyberChip)
            {
                UpdateDamageMultiplier();
            }
        }

        private void UpdateDamageMultiplier()
        {
            // 获取玩家手持物品的价值（以铜币为单位）
            Item heldItem = Player.HeldItem;
            int itemValue = heldItem.value;

            // 根据物品价值计算伤害加成倍率
            damageMultiplier = 1f + (itemValue / 10000f); // 每1金币提高1%的伤害

            // 确保伤害加成不会太高（最大提升35%）
            if (damageMultiplier > 1.35f)
            {
                damageMultiplier = 1.35f;
            }
        }


        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasCyberChip)
            {
                modifiers.SourceDamage *= damageMultiplier; // 应用伤害加成
                // CombatText.NewText(Player.getRect(), Color.GreenYellow, "伤害提升!", dramatic: false);
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (hasCyberChip && proj.DamageType == DamageClass.Melee)
            {
                modifiers.SourceDamage *= damageMultiplier; // 应用伤害加成
                // CombatText.NewText(Player.getRect(), Color.GreenYellow, "伤害提升!", dramatic: false);
            }
        }

    }
}
