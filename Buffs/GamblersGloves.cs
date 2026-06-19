using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class GamblersGloves : ModBuff
    {

        public override void SetStaticDefaults()
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.5f;
            player.GetDamage(DamageClass.Generic) += 0.5f;
        }
    }
}


