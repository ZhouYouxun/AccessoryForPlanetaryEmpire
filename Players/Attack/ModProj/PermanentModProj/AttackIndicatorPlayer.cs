using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj.PermanentModProj
{
    public class AttackIndicatorPlayer : ModPlayer
    {
        public bool hasDefenseIndicator = false;
        public bool hasAttackIndicator = false;

        private int additionalDamage = 3;
        private int damageReduction = 10;

        public override void ResetEffects()
        {
            // 重置初始增加的属性值
            additionalDamage = 3;
            damageReduction = 10;

            // 检查游戏进程中的事件
            bool wallOfFleshDefeated = Main.hardMode;
            bool planteraDefeated = NPC.downedPlantBoss;
            bool moonLordDefeated = NPC.downedMoonlord;

            // 根据击败的Boss调整属性值
            if (moonLordDefeated)
            {
                additionalDamage = 30;
                damageReduction = 40;
            }
            else if (planteraDefeated)
            {
                additionalDamage = 16;
                damageReduction = 25;
            }
            else if (wallOfFleshDefeated)
            {
                additionalDamage = 7;
                damageReduction = 15;
            }
        }

        public override void ModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            if (hasAttackIndicator)
            {
                // 固定提升攻击力
                damage += additionalDamage;
            }
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            if (hasDefenseIndicator)
            {
                // 固定减少所受伤害
                int reduction = CalculateDamageReduction();
                modifiers.FinalDamage.Base -= reduction; // 这里直接减少固定值的伤害
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            if (hasDefenseIndicator)
            {
                // 固定减少所受伤害
                int reduction = CalculateDamageReduction();
                modifiers.FinalDamage.Base -= reduction; // 这里直接减少固定值的伤害
            }
        }

        private int CalculateDamageReduction()
        {
            // 初始减少的伤害值
            int damageReduction = 10;

            // 检查游戏进程中的事件
            bool wallOfFleshDefeated = Main.hardMode;
            bool planteraDefeated = NPC.downedPlantBoss;
            bool moonLordDefeated = NPC.downedMoonlord;

            // 根据击败的Boss调整伤害减免值
            if (moonLordDefeated)
            {
                damageReduction = 40;
            }
            else if (planteraDefeated)
            {
                damageReduction = 25;
            }
            else if (wallOfFleshDefeated)
            {
                damageReduction = 15;
            }

            return damageReduction;
        }




    }
}


