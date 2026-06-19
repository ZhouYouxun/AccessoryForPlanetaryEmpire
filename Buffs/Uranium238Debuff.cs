using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class Uranium238Debuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Uranium 238");
            // Description.SetDefault("Movement speed reduced by 15%");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.velocity *= 0.85f; // 降低15%的移动速度
        }
    }
}
