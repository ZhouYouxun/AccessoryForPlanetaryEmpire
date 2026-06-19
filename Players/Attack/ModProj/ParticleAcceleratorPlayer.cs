using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Players.Attack.ModProj
{
    public class ParticleAcceleratorPlayer : ModPlayer
    {
        public bool hasParticleAccelerator = false;

        public override void ResetEffects()
        {
            hasParticleAccelerator = false; // 每帧重置为关闭，只有主文件开启时才会启用
        }
    }
}