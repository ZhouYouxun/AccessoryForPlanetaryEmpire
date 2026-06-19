using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire; // 引用 SunLightSystem 所在的命名空间

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class ElectronicPsychedelicDrugsBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 设置为buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 提升伤害
            player.GetDamage(DamageClass.Generic) += 2.5f;
            // 当玩家有该药水效果时，开启 SunLightSystem 的效果开关
            SunLightSystem.IsPsychedelicActive = true;

            // 当药水效果结束（buff时间为0），关闭 SunLightSystem 的效果开关
            if (player.buffTime[buffIndex] == 0)
            {
                SunLightSystem.IsPsychedelicActive = false;
            }
        }

    }
}
