using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class LimitResponseModuleBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Limit Response Module");
            //Description.SetDefault("提升45%伤害，100%暴击率，75%移动速度");
            Main.debuff[Type] = false; // 这是一个增益效果，不是Debuff
            Main.buffNoTimeDisplay[Type] = false; // 允许显示Buff的持续时间
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 0.45f; // 提升45%伤害
            player.GetCritChance(DamageClass.Generic) = 100; // 暴击率提升至100%
            player.moveSpeed += 0.75f; // 提升75%移动速度

            
        }

       
    }
}