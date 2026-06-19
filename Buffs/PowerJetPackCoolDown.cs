using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::AccessoryForPlanetaryEmpire.Players;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class PowerJetPackCoolDown : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Power JetPack CoolDown");
            // Description.SetDefault("Power JetPack is on cooldown");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 仅仅是一个显示冷却时间的Buff，无需其他功能
        }
    }
}
