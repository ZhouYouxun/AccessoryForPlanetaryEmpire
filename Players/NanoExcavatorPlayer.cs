using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using AccessoryForPlanetaryEmpire.Accessory.Others;

namespace AccessoryForPlanetaryEmpire.Players
{
    public class NanoExcavatorPlayer : ModPlayer
    {
        public bool hasNanoExcavator;

        public override void ResetEffects()
        {
            hasNanoExcavator = false;
        }
    }
}