using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using AccessoryForPlanetaryEmpire.Projectiles;

namespace AccessoryForPlanetaryEmpire.Players.Defense.Counter
{
    public class DarkSideOfTheMoonPlayer : ModPlayer
    {
        public bool DarkSideOfTheMoonEquipped = false;
        private int hitCount;

        public override void ResetEffects()
        {
            DarkSideOfTheMoonEquipped = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            // 检查玩家是否装备了 DarkSideOfTheMoon 并且生命值低于 50
            if (DarkSideOfTheMoonEquipped && Player.statLife < 50)
            {
                hitCount++; // 每次受伤后增加计数

                if (hitCount >= 3) // 如果受伤次数达到3次
                {
                    SummonMoonShields(); // 触发月之盾牌
                    hitCount = 0; // 重置计数
                }
            }
        }

        private void SummonMoonShields()
        {
            // 生成15个月之盾牌
            for (int i = 0; i < 15; i++)
            {
                Vector2 position = Player.Center;
                Vector2 velocity = new Vector2(Main.rand.NextFloat(-3f, 6f), Main.rand.NextFloat(-3f, 6f));
                Projectile.NewProjectile(Player.GetSource_FromThis(), position, velocity, ModContent.ProjectileType<MoonShieldProjectile>(), 0, 0, Player.whoAmI);
            }

            // 显示战斗文字和播放声音
            //CombatText.NewText(Player.getRect(), Color.White, "月之盾牌已弹出! ", true);
            SoundEngine.PlaySound(SoundID.Item46, Player.position);
        }
    }
}