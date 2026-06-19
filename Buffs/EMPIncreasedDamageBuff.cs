using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.ID;
using AccessoryForPlanetaryEmpire.Players.Defense;
using AccessoryForPlanetaryEmpire.Players;

namespace AccessoryForPlanetaryEmpire.Buffs
{
    public class EMPIncreasedDamageBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Increased Damage");
            //Description.SetDefault("Your damage is increased by 3.5% for each projectile cleared.");
            Main.debuff[Type] = false; // 这个buff不是debuff
            Main.buffNoSave[Type] = true; // 这个buff不会保存

        }

        public override void Update(Player player, ref int buffIndex)
        {
            // 在这里可以添加其他buff相关的逻辑
            // 在这里应用增益效果
            EMPClearBulletPlayer modPlayer = player.GetModPlayer<EMPClearBulletPlayer>();
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

        //// 更新粒子效果的方法
        //public override void UpdateBadLifeRegen(Player player, ref int damage)
        //{
        //    foreach (Dust dust in Main.dust)
        //    {
        //        if (dust.active && dust.customData is float rotationSpeed)
        //        {
        //            dust.rotation += rotationSpeed; // 旋转粒子
        //            dust.scale *= 0.98f; // 缩小粒子
        //            dust.alpha += 2; // 渐变粒子的透明度
        //        }
        //    }
        //}
    }
}
