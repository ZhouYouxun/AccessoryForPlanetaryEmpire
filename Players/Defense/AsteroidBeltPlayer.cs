using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.Defense
{
    public class AsteroidBeltPlayer : ModPlayer
    {
        public bool hasAsteroidBelt = false;
        public int asteroidSpawnTimer = 0; // 添加这一行


        public override void ResetEffects()
        {
            hasAsteroidBelt = false;
        }

        public override void PostUpdate()
        {
            if (!hasAsteroidBelt)
            {
                // 可以在这里添加额外的逻辑
            }
        }
    }
}
