using AccessoryForPlanetaryEmpire.Projectiles.Accessory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;


namespace AccessoryForPlanetaryEmpire.Players.Attack
{
    public class AntimatterNuclearMissilePlayer : ModPlayer
    {
        private int bossFightTimer = 0;
        public bool hasAntimatterNuclearMissile = false; // 定义布尔值

        public override void ResetEffects()
        {
            hasAntimatterNuclearMissile = false; // 每帧重置布尔值

            // 如果没有BOSS存在，重置计时器
            if (!AnyBossAlive())
            {
                bossFightTimer = 0;
            }
        }

        public override void PostUpdate()
        {
            if (hasAntimatterNuclearMissile && AnyBossAlive()) // 只有在布尔值为 true 时才启用
            {
                bossFightTimer++;
                int requiredTime = Main.getGoodWorld ? 30000 : 60000; // 如果是 "good world" 模式，则等待时间缩减至 5 分钟 (3 万)

                if (bossFightTimer >= requiredTime)
                {
                    FireNuke();
                    bossFightTimer = 0; // 重置计时器
                }
            }
        }

        private bool AnyBossAlive()
        {
            foreach (var npc in Main.npc)
            {
                if (npc.active && npc.boss)
                {
                    return true;
                }
            }
            return false;
        }

        private void FireNuke()
        {
            // 生成核导弹
            Projectile.NewProjectile(Player.GetSource_Accessory(Player.HeldItem), Player.Center, Vector2.Zero, ModContent.ProjectileType<AntimatterNuclearMissilePROJ>(), 0, 0, Player.whoAmI);
            CombatText.NewText(Player.getRect(), Microsoft.Xna.Framework.Color.Red, "反物质核导弹已就绪，准备发射！！！", true, false);
        }
    }
}
