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
    public class WindrunnerBootsBuff : ModBuff
    {

        public override void Update(Player player, ref int buffIndex)
        {
            player.immune = true; // 设置玩家为无敌状态
            player.immuneTime = 60; // 给玩家一秒钟无敌时间
            
            player.GetDamage(DamageClass.Generic) -= 0.9f; // 几乎无法造成任何伤害
                                                           
            CreateFootParticleEffect(player); // 在玩家脚底生成粒子特效
        }

        // 创建脚底粒子特效的方法
        private void CreateFootParticleEffect(Player player)
        {
            int dustID = DustID.Cloud;
            float duration = 1.5f; // 粒子的持续时间
            float sizeMultiplier = 1.5f; // 粒子的大小倍数
            float brightnessMultiplier = 2f; // 粒子的亮度倍数
            Vector2 footPosition = player.Bottom; // 玩家脚底的位置
            Vector2[] offsets = new Vector2[]
            {
                new Vector2(-16, 4), // 左边两个位置
                new Vector2(-8, 4),
                new Vector2(0, 4),  // 中间位置
                new Vector2(8, 4),  // 右边两个位置
                new Vector2(16, 4)
            };

            // 随机生成粒子特效
            foreach (var offset in offsets)
            {
                Vector2 particlePosition = footPosition + offset;
                Dust dust = Dust.NewDustPerfect(particlePosition, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, sizeMultiplier);
                dust.noGravity = true;
                dust.fadeIn = duration;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }
        }

    }
}
