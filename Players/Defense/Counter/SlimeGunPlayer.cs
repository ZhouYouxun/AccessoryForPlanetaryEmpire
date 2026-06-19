using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class SlimeGunPlayer : ModPlayer
    {
        public bool hasSlimeGun = false;
        private int hitCount;

        public override void ResetEffects()
        {
            hasSlimeGun = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (hasSlimeGun)
            {
                hitCount++;
                if (hitCount >= 5)
                {
                    SummonSlimeShields();
                    hitCount = 0;
                }
            }
        }

        private void SummonSlimeShields()
        {
            // 从鼠标位置生成弹幕，而不是从玩家位置
            Vector2 mousePosition = Main.MouseWorld;

            for (int i = 0; i < 5; i++)
            {
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-3f, 3f));
                Projectile.NewProjectile(Player.GetSource_FromThis(), mousePosition, velocity, ModContent.ProjectileType<SlimeShieldProjectile>(), 0, 0, Player.whoAmI);
            }

            // 播放音效（去掉文本提示）
            SoundEngine.PlaySound(SoundID.Item46, Player.position);
        }
    }
}