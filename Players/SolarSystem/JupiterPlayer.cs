using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class JupiterPlayer : ModPlayer
    {
        public bool hasJupiterAccessory = false;
        public bool hasTSSAccessory = false; // 添加hasTSSAccessory

        public override void ResetEffects()
        {
            hasJupiterAccessory = false;
            hasTSSAccessory = false; // 重置hasTSSAccessory
        }

        public override void UpdateEquips()
        {
            if (hasJupiterAccessory || hasTSSAccessory)
            {
                ManageJupiterProjectile();
            }
        }

        private void ManageJupiterProjectile()
        {
            bool jupiterExists = false;
            Vector2 jupiterPosition = Vector2.Zero;
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Player.whoAmI && proj.type == ModContent.ProjectileType<JupiterProjectile>())
                {
                    jupiterExists = true;
                    jupiterPosition = proj.Center;
                    break;
                }
            }

            // 如果没有找到活跃的木星弹幕，则生成一个新的
            if (!jupiterExists)
            {
                double angle = Main.rand.NextDouble() * 2 * Math.PI;
                jupiterPosition = Player.Center + new Vector2((float)(Math.Cos(angle) * 40 * 16), (float)(Math.Sin(angle) * 20 * 16)); // 公转半径40格
                int damage = hasTSSAccessory ? 4500 : 1500; // 根据hasTSSAccessory确定伤害
                Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), jupiterPosition, Vector2.Zero, ModContent.ProjectileType<JupiterProjectile>(), damage, 0, Player.whoAmI);
            }
        }
    }
}