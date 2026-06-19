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
    public class NebulaBraceletPlayer : ModPlayer
    {
        public bool hasNebulaBracelet = false;
        public override void ResetEffects()
        {
            hasNebulaBracelet = false;
        }
    }
}
