using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    public class ControllableHaloPlayer : ModPlayer
    {
        public bool hasControllableHalo = false;

        public override void ResetEffects()
        {
            hasControllableHalo = false;
        }


        public override void PostUpdate()
        {
            if (!hasControllableHalo)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.type == ModContent.ProjectileType<ControllableHaloPROJ>() && proj.owner == Player.whoAmI)
                    {
                        //CombatText.NewText(Player.getRect(), Color.White, "已消除一个弹幕！", dramatic: false, dot: false);
                        proj.Kill(); // 消除弹幕
                    }
                }
            }
        }

    }
}
