using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class NSDD_Enemy : ModBuff
    {

        public override void Update(NPC npc, ref int buffIndex)
        {
            // 降低30%的攻击力
            npc.damage = (int)(npc.damage * 0.7); // 将攻击力降低为原来的70%

            // 降低30%的移速
            npc.velocity *= 0.7f; // 将速度降低为原来的70%
        }
    }
}
