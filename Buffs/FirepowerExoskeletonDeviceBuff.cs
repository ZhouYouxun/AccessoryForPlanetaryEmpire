using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;


namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class FirepowerExoskeletonDeviceBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {

        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) += 2.0f;
        }
    }
}


