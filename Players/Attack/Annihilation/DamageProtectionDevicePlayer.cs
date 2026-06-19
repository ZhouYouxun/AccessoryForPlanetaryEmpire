using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria;
using Terraria.ModLoader;
using System;
using AccessoryForPlanetaryEmpire.Global;
using AccessoryForPlanetaryEmpire.Buffs;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace AccessoryForPlanetaryEmpire.Players.Attack.Annihilation
{
    public class DamageProtectionPlayer : ModPlayer
    {

        // 添加一个布尔变量来跟踪是否激活了伤害保护
        public bool hasDamageProtection = false;

        public override void ResetEffects()
        {
            hasDamageProtection = false;
        }



        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            // 当玩家被NPC击中时，如果启用了伤害保护，则添加或刷新Buff
            if (hasDamageProtection)
            {
                CombatText.NewText(Player.getRect(), Color.IndianRed, "恭喜你，你被击中了（", dramatic: false, dot: false);
                Player.AddBuff(ModContent.BuffType<DamageProtectionDeviceBuff>(), 600); // 600帧等于10秒
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
            // 当玩家被弹幕击中时，如果启用了伤害保护，且为敌对弹幕，同样添加或刷新Buff
            if (hasDamageProtection && !proj.friendly)
            {
                CombatText.NewText(Player.getRect(), Color.IndianRed, "恭喜你，你被击中了（", dramatic: false, dot: false);
                Player.AddBuff(ModContent.BuffType<DamageProtectionDeviceBuff>(), 600); // 600帧等于10秒
            }
        }

    }
}


