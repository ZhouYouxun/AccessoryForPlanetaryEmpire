using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Buffs;
using AccessoryForPlanetaryEmpire.Players.Defense;
using AccessoryForPlanetaryEmpire.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AccessoryForPlanetaryEmpire
{
    public static class ScreenShakeManager
    {
        public static float GeneralScreenShakePower;
        private static float shakeDecay = 0.9f;

        public static void ApplyShake()
        {
            if (GeneralScreenShakePower > 0)
            {
                Vector2 shakeOffset = new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-1, 1)) * GeneralScreenShakePower;
                Main.screenPosition += shakeOffset;

                GeneralScreenShakePower *= shakeDecay;

                Main.screenPosition -= shakeOffset;
            }
        }
    }
}
