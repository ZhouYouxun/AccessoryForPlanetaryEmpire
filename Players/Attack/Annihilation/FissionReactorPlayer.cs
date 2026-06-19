using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    internal class FissionReactorPlayer : ModPlayer
    {
        public bool hasFissionReactor;

        public override void ResetEffects()
        {
            hasFissionReactor = false;
        }

        public override void UpdateLifeRegen()
        {
            if (hasFissionReactor)
            {
                // 计算损失的生命百分比
                float lifeLostPercent = 1f - (float)Player.statLife / Player.statLifeMax2;

                // 每损失10%生命，增加10%的攻击力，最大损失90%时达到90%
                float damageBonus = (int)(lifeLostPercent * 10f) * 10f;
                damageBonus = damageBonus > 90f ? 90f : damageBonus; // 限制最大加成为135%

                // 应用攻击力加成
                Player.GetDamage(DamageClass.Generic) += damageBonus / 100f; // 将百分比转换为增益
            }
        }
    }
}