using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class PlutoPlayer : ModPlayer
    {
        public bool hasPlutoAccessory = false;

        public override void ResetEffects()
        {
            hasPlutoAccessory = false;
        }

        public override void PostUpdate()
        {
            if (hasPlutoAccessory)
            {
                ManagePlutoAccessory();
            }
        }

        private void ManagePlutoAccessory()
        {
            bool plutoExists = false;
            Vector2 plutoPosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<PlutoProjectile>())
                {
                    plutoExists = true;
                    plutoPosition = proj.Center;
                    break;
                }
            }

            if (!plutoExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                plutoPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 30 * 16), (float)(Math.Sin(angle) * 30 * 16)); // 公转半径30格
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), plutoPosition, Vector2.Zero, ModContent.ProjectileType<PlutoProjectile>(), 0, 0, Player.whoAmI);
            }
        }
    }
}
