using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using AccessoryForPlanetaryEmpire.Projectiles;

namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj
{
    public class AITrackingDevicePlayer : ModPlayer
    {
        public bool hasAITrackingDevice = false;
        public bool SuoyouZhuizong;
        public override void ResetEffects()
        {
            hasAITrackingDevice = false;
        }
    }
}
