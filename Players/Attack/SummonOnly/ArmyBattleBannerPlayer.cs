using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameInput;
using Terraria;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Buffs;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.Attack.SummonOnly
{
    public class ArmyBattleBannerPlayer : ModPlayer
    {
        public bool hasArmyBattleBanner = false;
        public float summonDamageBonus = 0f;

        public override void ResetEffects()
        {
            hasArmyBattleBanner = false;
            summonDamageBonus = 0f;
        }

        public override void PostUpdateEquips()
        {
            if (hasArmyBattleBanner)
            {
                // 在这里不处理，移到PostUpdate中
            }
        }

        public override void PostUpdate()
        {
            if (hasArmyBattleBanner)
            {
                // 获取当前仆从栏数和哨兵栏数
                int maxSummons = Player.maxMinions;
                int maxTurrets = Player.maxTurrets;

                // 扣除一个仆从栏和一个哨兵栏
                int reducedSummons = maxSummons - 1;
                int reducedTurrets = maxTurrets - 1;

                // 强制将仆从栏位和哨兵栏位设置为1
                Player.maxMinions = 1;
                Player.maxTurrets = 1;

                // 确保不会低于0
                int extraSummonSlots = reducedSummons > 0 ? reducedSummons : 0;
                int extraTurretSlots = reducedTurrets > 0 ? reducedTurrets : 0;

                // 将每个扣除的仆从栏转化为90%的召唤伤害
                summonDamageBonus = extraSummonSlots * 0.9f + extraTurretSlots * 2;
                Player.GetDamage(DamageClass.Summon) += summonDamageBonus;
                Player.GetDamage(DamageClass.SummonMeleeSpeed) -= summonDamageBonus;
            }
        }


        //public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        //{
        //    if (hasArmyBattleBanner && item.DamageType == DamageClass.Summon)
        //    {
        //        damage += summonDamageBonus;
        //    }
        //}

    }
}
