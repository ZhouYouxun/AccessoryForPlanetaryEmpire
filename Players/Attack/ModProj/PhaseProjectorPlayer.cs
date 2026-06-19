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
    public class PhaseProjectorPlayer : ModPlayer
    {
        public bool hasPhaseProjector = false;

        public override void ResetEffects()
        {
            hasPhaseProjector = false;
        }
    }
}
