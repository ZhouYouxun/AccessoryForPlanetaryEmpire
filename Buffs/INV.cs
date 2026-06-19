using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class INV : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.immune = true; // 设置玩家为无敌状态
            player.immuneTime = 60; // 给玩家一秒钟无敌时间
            player.immuneNoBlink = true; // 即便处于无敌状态，玩家也不会闪烁
        }
    }
}


