using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Accessory.JobAcc.Summon;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players
{
    public class InfSummonPlayer : ModPlayer
    {
        public bool hasInfSummon;

        public override void ResetEffects()
        {
            hasInfSummon = false;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (hasInfSummon && item.DamageType == DamageClass.Summon)
            {
                damage *= 0; // 将召唤伤害设为0
                Player.GetDamage(DamageClass.Summon) *= 0;
            }
        }
    }
}
