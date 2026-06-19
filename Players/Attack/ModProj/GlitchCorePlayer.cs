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
    public class GlitchCorePlayer : ModPlayer
    {
        public bool hasGlitchCore = false;

        public override void ResetEffects()
        {
            hasGlitchCore = false;
        }
    }
}