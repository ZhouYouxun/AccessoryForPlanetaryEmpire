using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class LifeSubSpaceBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false; // 不是一个负面效果
            Main.buffNoSave[Type] = false; // 允许存档
            Main.buffNoTimeDisplay[Type] = false; // 显示时间
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 35; // 增加生命回复
            player.statDefense += 25; // 增加防御力
        }
    }
}
