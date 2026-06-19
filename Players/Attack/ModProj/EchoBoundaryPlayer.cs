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
    public class EchoBoundaryPlayer : ModPlayer
    {
        public bool hasEchoBoundary = false;

        public override void ResetEffects()
        {
            hasEchoBoundary = false; // 默认状态下关闭
        }
    }
}