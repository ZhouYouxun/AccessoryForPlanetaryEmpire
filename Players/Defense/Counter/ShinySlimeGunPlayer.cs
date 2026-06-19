using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using System;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class ShinySlimeGunPlayer : ModPlayer
    {
        public bool hasShinySlimeGun = false;
        private int timer;

        public override void ResetEffects()
        {
            hasShinySlimeGun = false;
        }

        public override void PostUpdate()
        {
            if (hasShinySlimeGun)
            {
                timer++;
                if (timer >= 300) // 每5秒（300帧）
                {
                    timer = 0;
                    FireSlimeShields();
                }
            }
        }

        private void FireSlimeShields()
        {
            Vector2 playerPosition = Player.Center;
            Vector2 mousePosition = Main.MouseWorld;
            Vector2 direction = (mousePosition - playerPosition).SafeNormalize(Vector2.UnitX);
            float speed = 10f; // 将速度减慢到原来的1/3

            for (int i = 0; i < 2; i++)
            {
                float randomRotation = MathHelper.ToRadians(Main.rand.NextFloat(-25f, 25f)); // 随机偏移X度
                Vector2 velocity = direction.RotatedBy(randomRotation) * speed;
                Projectile.NewProjectile(Player.GetSource_FromThis(), playerPosition, velocity, ModContent.ProjectileType<ShinySlimeShieldProjectile>(), 0, 0, Player.whoAmI);
            }
        }
    }
}