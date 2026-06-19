using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class MechanicalHeartBuff : ModBuff
    {

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 100; // 每秒额外增加50点生命再生
            player.endurance += 0.95f; // 增加95%的伤害减免，注意，不要超过1
        }
    }
}


