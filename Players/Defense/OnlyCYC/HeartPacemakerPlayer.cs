using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Defense.OnlyCYC
{
    public class HeartPacemakerPlayer : ModPlayer
    {
        public bool hasHeartPacemaker;

        public override void ResetEffects()
        {
            hasHeartPacemaker = false; // 每帧重置
        }

        public override void UpdateLifeRegen()
        {
            if (hasHeartPacemaker)
            {
                // 计算损失的生命百分比
                float lifeLostPercent = 1f - (float)Player.statLife / Player.statLifeMax2;
                int defenseBonus = (int)(lifeLostPercent * 10f) * 10; // 每损失10%生命，增加10点防御
                float damageReductionBonus = lifeLostPercent * 10f * 0.05f; // 每损失10%生命，增加5%的伤害减免

                // 应用防御加成
                Player.statDefense += defenseBonus;

                // 应用伤害减免加成
                Player.endurance += damageReductionBonus; // 注意：endurance 值受到 1 的限制，即最多只能减少100%的伤害

                // CombatText.NewText(Player.getRect(), Color.GreenYellow, "防御力+10，伤害减免+5%！", dramatic: false, dot: false);

            }
        }
    }

}



