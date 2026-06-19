using AccessoryForPlanetaryEmpire.Players;
using AccessoryForPlanetaryEmpire.Players.Defense;
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
    public class EMPUniversalBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Universal Protection");
            //Description.SetDefault("Increased defense and damage.");
            Main.debuff[Type] = false; // 这个buff不是debuff
            Main.buffNoSave[Type] = true; // 这个buff不会保存
        }

        public override void Update(Player player, ref int buffIndex)
        {
            EMPUniversalPlayer modPlayer = player.GetModPlayer<EMPUniversalPlayer>();
            player.GetDamage(DamageClass.Generic) += modPlayer.empDamageIncrease;
            // 创建无穷符号粒子特效
            CreateInfinitySymbolEffect(player);
        }

        // 创建无穷符号粒子特效的方法
        private void CreateInfinitySymbolEffect(Player player)
        {
            int dustID = DustID.UltraBrightTorch;
            float duration = 1f; // 粒子的持续时间
            float sizeMultiplier = 3f; // 粒子的大小倍数
            float brightnessMultiplier = 3f; // 粒子的亮度倍数
            float amplitude = 40f; // 振幅
            float frequency = 0.2f; // 频率
            int particleCount = 3; // 每帧生成的粒子数

            for (int i = 0; i < particleCount; i++)
            {
                // 随机生成 t 值，范围在 [0, 2 * pi)
                float t = Main.rand.NextFloat(MathHelper.TwoPi);

                // 无穷符号的参数方程
                float x = amplitude * (float)Math.Sin(t) * 2f;
                float y = amplitude * (float)Math.Sin(2 * t);

                // 创建粒子特效
                Vector2 position = player.Center + new Vector2(x, y);
                Dust dust = Dust.NewDustPerfect(position, dustID, Vector2.Zero, 100, Color.White * brightnessMultiplier, 1.5f * sizeMultiplier);
                dust.noGravity = true;
                dust.fadeIn = duration;
                dust.rotation = Main.rand.NextFloat(MathHelper.TwoPi); // 随机初始旋转
                dust.velocity = Vector2.Zero; // 初始速度为零，用来控制旋转
                dust.customData = Main.rand.NextFloat(0.05f, 0.1f); // 随机旋转速度
            }
        }




    }
}
