using AccessoryForPlanetaryEmpire.Projectiles.SolarSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles;



namespace AccessoryForPlanetaryEmpire.Players.SolarSystem
{
    public class GravitationalFieldPlayer : ModPlayer
    {
        public bool hasGravitationalField = false;

        public override void ResetEffects()
        {
            hasGravitationalField = false;
        }

        public override void PostUpdate()
        {
            if (hasGravitationalField)
            {
                // 判断是否已经有活跃的4个"White Hole"弹幕
                bool whiteHoleExists = false;
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj.active && proj.owner == Player.whoAmI &&
                        (proj.type == ModContent.ProjectileType<WhiteHolePROJ>() ||
                         proj.type == ModContent.ProjectileType<WhiteHolePROJ2>() ||
                         proj.type == ModContent.ProjectileType<WhiteHolePROJ3>() ||
                         proj.type == ModContent.ProjectileType<WhiteHolePROJ4>()))
                    {
                        whiteHoleExists = true;
                        break;
                    }
                }

                // 如果没有找到活跃的白洞弹幕，则生成4个新的弹幕
                if (!whiteHoleExists)
                {
                    Vector2 playerCenter = Player.Center;

                    // 在玩家上下左右20格范围生成4个不同的白洞弹幕
                    Vector2[] positions = new Vector2[]
                    {
                        playerCenter + new Vector2(0, -20 * 16), // 上
                        playerCenter + new Vector2(0, 20 * 16),  // 下
                        playerCenter + new Vector2(-20 * 16, 0), // 左
                        playerCenter + new Vector2(20 * 16, 0)   // 右
                    };

                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), positions[0], Vector2.Zero, ModContent.ProjectileType<WhiteHolePROJ>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), positions[1], Vector2.Zero, ModContent.ProjectileType<WhiteHolePROJ2>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), positions[2], Vector2.Zero, ModContent.ProjectileType<WhiteHolePROJ3>(), 0, 0, Player.whoAmI);
                    Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), positions[3], Vector2.Zero, ModContent.ProjectileType<WhiteHolePROJ4>(), 0, 0, Player.whoAmI);
                }
            }
        }
    }
}