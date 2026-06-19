using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj
{
    public class BulletScreenManagementTerminalPlayer : ModPlayer
    {
        public int currentEffectIndex = 0; // 当前效果的索引
        public bool hasBulletScreenManagementTerminal = false; // 是否启用了BulletScreenManagementTerminal

        public override void ResetEffects()
        {
            hasBulletScreenManagementTerminal = false; // 重置效果为未启用状态
        }
    }
}
