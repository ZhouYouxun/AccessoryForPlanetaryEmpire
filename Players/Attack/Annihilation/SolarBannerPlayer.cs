using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Projectiles;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class SolarBannerPlayer : ModPlayer
    {
        public bool hasSolarBanner = false;
        private bool shouldIncreaseDamage = false;
        private float damageMultiplier = 1f;

        public override void ResetEffects()
        {
            hasSolarBanner = false;
            damageMultiplier = 1f;
        }


        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (hasSolarBanner)
            {
                // 排除挥砍类武器
                if (item.useStyle == ItemUseStyleID.Swing)
                {
                    return;
                }

                // 仅针对不受近战攻速影响的近战武器进行处理
                if (item.DamageType == DamageClass.MeleeNoSpeed)
                {
                    float meleeSpeed = Player.GetAttackSpeed(DamageClass.Melee); // 获取近战攻速
                    float damageMultiplier = meleeSpeed * 1.5f; // 每1%近战攻速提供1.5%伤害加成
                    damage = (int)(damage * damageMultiplier); // 显式转换为整数
                }
            }
        }


    }
}
