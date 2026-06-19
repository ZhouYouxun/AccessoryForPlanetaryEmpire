using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class BloodPumpBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; // 这个Buff不是一个减益效果
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 50; // 每秒额外增加50点生命再生
            player.endurance += 0.7f; // 增加70%的伤害减免，注意，不要超过1
        }
    }
}


