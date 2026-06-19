using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class DeathNoteBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 这个Buff是一个减益效果
            Main.buffNoTimeDisplay[Type] = false; // Buff的时间会被显示
            Main.buffNoSave[Type] = true; // 这个Buff不会被保存，也就是说玩家重新进入游戏时Buff会消失
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] % 300 == 0) // 每5秒执行一次
            {
                int damage = (int)(npc.life * 0.05f); // 扣除当前生命值的5%

                bool critical = false; // 这次攻击不是暴击
                int hitDirection = npc.direction; // 根据NPC面向的方向设置击退方向

                npc.SimpleStrikeNPC(damage, hitDirection, crit: critical);

                // 生成粒子效果
                if (Main.rand.NextBool(2)) // 减少粒子生成的密度
                {
                    Vector2 position = npc.Center + Main.rand.NextVector2Circular(npc.width / 2, npc.height / 2);
                    Dust dust = Dust.NewDustPerfect(position, DustID.FireworkFountain_Blue, Vector2.Zero, 0, default, 1.5f);
                    dust.noGravity = true; // 粒子不受重力影响
                }
            }
        }
    }
}
