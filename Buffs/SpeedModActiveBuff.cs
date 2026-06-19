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
    public class SpeedModActiveBuff : ModBuff
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Speed Boost");
            // Description.SetDefault("Movement speed increased by 200%");
            Main.debuff[Type] = false; // 这个buff不是debuff
            Main.buffNoSave[Type] = true; // 这个buff不会保存
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 增加移动速度、跳跃速度和最大下落速度
            player.moveSpeed += 2.00f;
            player.maxFallSpeed *= 2.00f;
            player.jumpSpeedBoost += 2.00f;
        }









    }
}
