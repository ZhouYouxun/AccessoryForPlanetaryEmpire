using AccessoryForPlanetaryEmpire.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using AccessoryForPlanetaryEmpire.Players.Attack;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent.Achievements;
using System.Reflection;



namespace AccessoryForPlanetaryEmpire.Projectiles.GlobalChange
{
    public class NanoExcavatorProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void AI(Projectile projectile)
        {
            if (Main.player[projectile.owner].GetModPlayer<NanoExcavatorPlayer>().hasNanoExcavator && DamageClassUtils.IsEligibleProjectile(projectile))
            {
                DestroyTiles(projectile);
            }
        }

        private void DestroyTiles(Projectile projectile)
        {
            Vector2 position = projectile.position;
            int range = 3; // 破坏范围
            int minX = (int)(position.X / 16f - range);
            int maxX = (int)(position.X / 16f + range);
            int minY = (int)(position.Y / 16f - range);
            int maxY = (int)(position.Y / 16f + range);

            if (minX < 0) minX = 0;
            if (maxX > Main.maxTilesX) maxX = Main.maxTilesX;
            if (minY < 0) minY = 0;
            if (maxY > Main.maxTilesY) maxY = Main.maxTilesY;

            AchievementsHelper.CurrentlyMining = true;
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    float distanceX = Math.Abs(i - position.X / 16f);
                    float distanceY = Math.Abs(j - position.Y / 16f);
                    double distance = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
                    if (distance < range)
                    {
                        if (Main.tile[i, j] != null && Main.tile[i, j].HasTile)
                        {
                            WorldGen.KillTile(i, j, false, false, false);
                            if (!Main.tile[i, j].HasTile && Main.netMode != NetmodeID.SinglePlayer)
                            {
                                NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
                            }
                        }
                    }
                }
            }
            AchievementsHelper.CurrentlyMining = false;
        }



    }
}