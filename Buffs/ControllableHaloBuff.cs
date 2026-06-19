using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class ControllableHaloBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true; // 这个Buff是一个减益效果
            Main.buffNoTimeDisplay[Type] = false; // Buff的时间会被显示
            Main.buffNoSave[Type] = true; // 这个Buff不会被保存，也就是说玩家重新进入游戏时Buff会消失
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            if (npc.buffTime[buffIndex] % 60 == 0) // Terraria的逻辑是每帧运行60次，所以%60等于每秒执行一次
            {
                // int damage = 200; // 扣除的生命值
                int damage = (int)(npc.lifeMax * 0.005f); // 每次扣除5‰的生命值

                bool critical = false; // 这次攻击不是暴击
                int hitDirection = npc.direction; // 根据NPC面向的方向设置击退方向

                npc.SimpleStrikeNPC(damage, hitDirection, crit: critical);
                //if (Main.netMode == NetmodeID.Server) // 如果在多人游戏服务器上
                //{
                //    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, damage, 0f, hitDirection, critical ? 1 : 0);
                //}
            }

            // 生成粒子效果
            if (Main.rand.NextBool(2)) // 减少粒子生成的密度
            {
                Vector2 position = npc.Center + Main.rand.NextVector2Circular(npc.width / 2, npc.height / 2);
                Dust dust = Dust.NewDustPerfect(position, DustID.MagicMirror, Vector2.Zero, 0, default, 2.0f); // 使用MagicMirror粒子效果，调整大小
                dust.noGravity = true; // 粒子不受重力影响
                dust.velocity *= 0.75f; // 控制粒子速度
                dust.scale *= 0.75f; // 控制粒子大小
                dust.color = Color.Cyan; // 设定粒子颜色为青色
                dust.fadeIn = 1.5f; // 使粒子变得更亮
                dust.rotation = Main.rand.NextFloat(0, MathHelper.TwoPi); // 粒子旋转
                dust.velocity += new Vector2(0.1f, 0.1f); // 粒子旋转速度
                dust.alpha = 100; // 粒子透明度
            }






        }







    }
}
